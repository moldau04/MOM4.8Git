 

<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="SendAppointment" Codebehind="SendAppointment.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <link href="Design/css/pikaday.css" rel="stylesheet" />
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
          <div class="divbutton-container" style="height:40px!important;">
        <div id="divButtons1" class="">
            <div id="breadcrumbs-wrapper1">
                <header>
                    <div class="container row-color-grey" style="height:32px!important">
                        <div class="row" style="margin-bottom:0px;">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="fa fa-envelope"></i>&nbsp;Safety Test Appointment</div>
                                    <div class="buttonContainer">
                                    </div>
                                    <div class="btnlinks">
                                         <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click1"   >Send</asp:LinkButton>
                                    </div>
                                    <div class="btnlinks">                        
                                         <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click"   >Close</asp:LinkButton>
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
                 <div class="row" style="display:flex; margin-bottom: 0 !important;">
                     <div class="col-6" style="width: 50%; padding: 0px 10px;">
                         <div class="fc-label">
                             <label>From</label>
                         </div>
                         <div class="fc-input">
                             <asp:TextBox ID="txtEmailFrom" runat="server"
                                 CssClass="form-control"
                                 TabIndex="9" ToolTip="From" Placeholder="From"></asp:TextBox>
                                 </div>


                     </div>
                     <div class="col-6" style="width: 50%; padding: 0px 10px;">
                         <div class="fc-label">
                             <label>To</label>
                         </div>
                         <div class="fc-input">
                             <asp:TextBox ID="txtEmailTo" runat="server"
                                 CssClass="form-control"
                                 TabIndex="9" ToolTip="To" Placeholder="To"></asp:TextBox>

                         </div>
                     </div>
                     </div>
                <div class="row" style="display:flex;margin-bottom: 0 !important;">
                     <div class="col-6" style="width: 50%; padding: 0px 10px;">
                         <div class="fc-label">
                             <label>Start</label>
                         </div>
                         <div class="fc-input">
                             <asp:TextBox ID="txtStart" runat="server"
                                 CssClass="form-control"
                                 TabIndex="9" ToolTip="Start" Placeholder="Start"></asp:TextBox>

                         </div>
                     </div>

                     <div class="col-6" style="width: 50%; padding: 0px 10px;">
                         <div class="fc-label">
                             <label>End</label>
                         </div>
                         <div class="fc-input">
                             <asp:TextBox ID="TextEnd" runat="server"
                                 CssClass="form-control"
                                 TabIndex="9" ToolTip="End" Placeholder="End"></asp:TextBox>

                         </div>
                     </div>
                 </div>
                 <div class="row" style="display:flex;margin-bottom: 0 !important;">
                          <div class="col-6" style="width: 50%; padding: 0px 10px;">
                        <div class="fc-label">
                            <label> Location</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtLocation" runat="server"
                                CssClass="form-control"
                                TabIndex="9" ToolTip="Location" Placeholder="Location"></asp:TextBox>
                          
                        </div>
                    </div>
                  <div class="col-6" style="width: 50%; padding: 0px 10px;"> 
                        <div class="fc-label">
                            <label>Subject</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtSubject" runat="server"
                                CssClass="form-control"
                                TabIndex="9" ToolTip="Subject" Placeholder="Subject"></asp:TextBox>
                        </div>
                    </div>
                     </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>Body</label>
                        </div>
                        <div class="fc-input">
                            
                            <CKEditor:CKEditorControl ID="txtBody"    runat="server" Width="100%" Height="200" Toolbar="Full"
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


