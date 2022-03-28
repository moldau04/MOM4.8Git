<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="PrintWO" Codebehind="PrintWO.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        body:nth-of-type(1) img[src*="Blank.gif"]{display:none;}
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
                    <ul>
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Print WO</asp:Label>
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
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>


