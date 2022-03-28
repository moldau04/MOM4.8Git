<%@ Page Language="C#" MasterPageFile="~/mom.master" AutoEventWireup="true" Inherits="ApprovePO" Codebehind="ApprovePO.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Namespace="CustomControls" TagPrefix="cc1" Assembly="MOMWebApp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

      <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection"> 
    <link rel="stylesheet" href="js/signature/jquery.signaturepad.css" />
    <script type="text/javascript" src="js/Signature/jquery.signaturepad.js"></script>

    <style>

        .sign-title {
            background-color:darkslateblue !important;
        }

        .sigPad {
    float: left;
    margin-top: 15px;
    width: 100%;
}


    </style>
    <script type="text/javascript">

        $(document).ready(function () {

            try {
                $('.sigPad').signaturePad();
                jQuery('.sign-title-l').click(function () {
                    jQuery('#hdnDrawdata').val("");
                });
                $("#signbg").click(function () {
                    if (isCanvasSupported()) {
                        $("#sign").toggle();
                        $("#sign").focus();
                    }
                    else {
                        alert('Signature not supported on this web browser.');
                    }
                });
                $("#sign").blur(function () {
                    $("#sign").hide();
                });
                $("#convertpngbtn").click(function () {
                    $("#sign").hide();
                    toImage();
                });
                var oImg = document.getElementById("<%=imgSign.ClientID%>");
                var ImgHdn = document.getElementById("<%=hdnImg.ClientID%>");
                oImg.src = ImgHdn.value;
            } catch{ }
        });
        function isCanvasSupported() {
            var elem = document.createElement('canvas');
            return !!(elem.getContext && elem.getContext('2d'));
        }
        ///////////////////////////////    Convert signature to image      ////////////////////////////////
        function toImage() {
            var hdnDrawdata = document.getElementById("hdnDrawdata");
            var hdnImg = document.getElementById("<%=hdnImg.ClientID%>");
            var oImgElement = document.getElementById("<%=imgSign.ClientID%>");
            var canvas = document.getElementById("canvas");
            if (hdnDrawdata.value != "") {
                var img = canvas.toDataURL("image/png");
                oImgElement.src = img;
                hdnImg.value = img;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="col s12 m12 l12">
                                        <div class="row">
                                            <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Approve PO</div>
                                            <div style="font-weight:bold;font-size:19px;padding:5px;color:forestgreen;"><asp:Label CssClass="title_text" ID="Label1" runat="server"> </asp:Label>  </div>
                                            <div class="buttonContainer">
                                        <div class="btnlinks">
                                             
                                              <asp:LinkButton ID="btnApprove" runat="server"   Visible="false" OnClick="btnApprove_Click" >Approve</asp:LinkButton>
                                            
                                        </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton   ID="btnDecline" runat="server"   Visible="false" OnClick="btnApprove_Click">Decline</asp:LinkButton>
                                                    </div>
                                    </div>
                                            <div class="btnclosewrap">
                                                <a ID="lnkClose" href="Home.aspx"   ><i class="mdi-content-clear"></i></a>
                                            </div>
                                            <div class="rght-content">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>

          
        </div>
    </div>

    <div class="container accordian-wrap">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <div class="card cardradius">
                        <div class="card-content">
                            <div class="form-content-wrap form-content-wrapwd formpdtop2">
                                <div class="form-content-pd">
                                    <div class="form-section-row">
                                        <div class="col s12 m12 l12" style="padding-right: 0px;">
                                            <div class="row">
                                                <div class="form-section-row">
                                                    <div class="section-ttle">   <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Approve Purchase Order</asp:Label></div>
                                                    <div class="form-input-row">
                                                        <div class="form-section3">
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                   
                                                                    <label>PO#</label>
                                                                   <asp:TextBox ID="txtPO" runat="server" CssClass="form-control" TabIndex="2" onkeypress="return isNumberKey(event,this)" Enabled="false"
                                                MaxLength="50"></asp:TextBox>
                                                                     
                                                                </div>
                                                            </div>
                                                           
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label>Total$</label>
                                                                    <asp:TextBox ID="lblTotalAmount" runat="server" CssClass="form-control"  onkeypress="return isNumberKey(event,this)" Enabled="false" ></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label>Status</label>
                                                             <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control" TabIndex="2" Enabled="false"
                                                Text="New"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                              <div class="row" id="divStatusComments" runat="server">
                                                                       <label>Status Comments</label>
                                       
                                            <asp:TextBox ID="txtStatusComment" runat="server" Rows="3" CssClass="materialize-textarea mtarea" MaxLength="2000" TextMode="MultiLine">
                                            </asp:TextBox>
                                                                </div>
                                                                </div>

                                                        </div>
                                                        <div class="form-section3-blank">&nbsp;</div>
                                                        <div class="form-section3">
                                                             <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label>Vendor</label>
                                                               <asp:TextBox ID="txtVendor" runat="server" CssClass="form-control" TabIndex="1" MaxLength="75" Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdnVendorID" runat="server" />
                                                                </div>
                                                            </div>
                                                          
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                  <asp:HiddenField ID="hdnTotal" runat="server" /> 
                                      
                              
                                        <div class="fc-input">
                                            <label>Due Date</label>
                                            <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control" TabIndex="2" Enabled="false"
                                                onkeypress="return false;"></asp:TextBox>
                                        </div>
                                                                </div>
                                                            </div>
                                                               <div class="input-field col s12" id="Div1" runat="server">
                                                                <div class="row">
                                                                    <label>Date</label>
                                                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TabIndex="2" Enabled="false"
                                                MaxLength="15" onkeypress="return false;"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                              <div class="input-field col s12" id="Div2" runat="server">
                                                                <div class="row">
                                                                    <label>Address</label>
                                                                       <asp:TextBox ID="txtAddress" runat="server" Rows="3" CssClass="materialize-textarea mtarea" MaxLength="200"
                                                TextMode="MultiLine" Enabled="false"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                           
                                                        </div>
                                                        <div class="form-section3-blank">&nbsp;</div>
                                                        <div class="form-section3">
                                                         
                                                          
                                                            <div class="input-field col s12" id="Div3" runat="server">
                                                                <div class="row">
                                                                      <div class="signature form-col" style="margin-top: 0px !important" runat="server" id="divSignature">
                                        <div class="fc-label">
                                             <label >Signature </label>
                                        </div>
                                        <div id="signbg" class="fc-input" style="width: 268px; height: 100px;">
                                            <asp:Image ID="imgSign" runat="server" />
                                            <asp:HiddenField ID="hdnImg" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-col">
                                         
                                        <div id="sign" tabindex="0" style="display: none; width: 268px; height: 160px;" class="sign_popup sigPad fc-input">
                                            <div  >
                                                <a class="clearButton"  > <i class="mdi-action-delete"  style="float:left;font-size:25px !important; "></i> </a>

                                                <a id="convertpngbtn"   ><i class="mdi-action-add-shopping-cart"  style="float:right;font-size:25px !important; "  ></i></a>
                                            </div>
                                            <div class="sig">
                                                <div class="typed">
                                                </div>
                                                <canvas class="pad" style="border: 1px solid black; position: relative; background-color: #fff; width: 268px; height: 135px;"
                                                    id="canvas"></canvas>
                                                <input id="hdnDrawdata" tabindex="43" type="hidden" name="output" class="output" />
                                            </div>
                                        </div>
                                    </div>
                                                                </div>
                                                            </div>
                                                         
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="cf"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
        </div>
              
                     
             
           
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server"> </asp:Content>

