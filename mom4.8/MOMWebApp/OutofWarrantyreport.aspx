<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="OutofWarrantyreport" Codebehind="OutofWarrantyreport.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     
    <div class="page-content">


        <div class="page-cont-top">
            <div class="page-bar-right">
                
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Out of Warranty report</asp:Label></li>
                        <li> <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="close"
                                OnClick="lnkClose_Click"></asp:LinkButton></li>

                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                      <div class="search-customer">
                        <div class="sc-form"> 


                            <asp:Label ID="lblSdate" runat="server" Text="Passed inspection Date From"  ClientIDMode="Static" />
                            <asp:TextBox ID="txtfromDate" runat="server" CssClass="form-control input-sm input-small  " MaxLength="50"
                                Width="80px" ClientIDMode="Static"  ></asp:TextBox>
                            <asp:CalendarExtender ID="txtfromDate_CalendarExtender" Format="MM/dd/yyyy" runat="server" Enabled="True"
                                TargetControlID="txtfromDate">
                            </asp:CalendarExtender>

                            <asp:Label ID="lblEdate" runat="server" Text="Date To"  />
                            <asp:TextBox ID="txtToDate"  runat="server" CssClass="form-control input-sm input-small  " MaxLength="50"
                                Width="80px" ClientIDMode="Static"  ></asp:TextBox>
                            <asp:CalendarExtender ID="txtToDate_CalendarExtender" Format="MM/dd/yyyy" runat="server" Enabled="True"
                                TargetControlID="txtToDate">
                            </asp:CalendarExtender>
                            <asp:LinkButton ID="lnkSearch" CssClass="btn submit" runat="server" OnClick="lnkSearch_Click"  ><i class="fa fa-search"></i></asp:LinkButton>
                            
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div>
                    <asp:UpdatePanel ID="upJob" runat="server">
                        <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lnkSearch" />
                            </Triggers>
                        <ContentTemplate>
                        <div style="margin-left: 5px;margin-top:5px;">
                            <rsweb:ReportViewer ID="rvJob" runat="server" ShowPageNavigationControls="true" Width="80%"  Height="700px"
                                BorderColor="Gray" BorderStyle="None" BorderWidth="1px"
                                ShowZoomControl="False">
                            </rsweb:ReportViewer>
                        </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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

