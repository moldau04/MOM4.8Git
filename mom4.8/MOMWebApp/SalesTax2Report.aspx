<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/HomeMaster.master" Inherits="SalesTax2Report" Codebehind="SalesTax2Report.aspx.cs" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .styleDiv1 {
            float: left;
            width: 385px;
            margin-right: 15px;
        }

        .styleDiv2 {
            float: right;
            width: 100px;
        }
    </style>
    <script type="text/javascript">

        function showMailReport() {
            jQuery("#txtTo").text = "";
            jQuery("#txtCC").text = "";
            $("#programmaticModalPopup").show();
            $('#balancesheetpopup').modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="page-content">

        <div class="page-cont-top">
           
        </div>

        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Sales Tax Report</asp:Label></li>
                       <%-- <li><a id="LinkButton1" onclick="showMailReport();" class="icon-mail" title="Mail Report"></a></li>--%>
                        <li>
                            <ul class="nav navbar-nav pull-right">
                                <li class="dropdown dropdown-user">
                                    <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print dropdown-css" data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
                                    <ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
                                        <li><a href="CustomersReport.aspx?type=Customer"><span>Add New Report</span><div style="clear: both;"></div>
                                        </a></li>
                                    </ul>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ToolTip="Close" ID="lnkClose" runat="server" CausesValidation="false"
                                OnClick="lnkClose_Click"></asp:LinkButton></li>
                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">

                    <div class="table-scrollable n-bor " style="border: none">
                       <div class="col-lg-12 col-md-12">
                            <div class="search-customer">
                                
                         <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                       <div class="sc-form">
                            
                            <div>
                                <label for="">Start</label>
                               
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"
                                            MaxLength="50" Width="130px" autocomplete="off"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtStartDate">
                                        </asp:CalendarExtender>
                                         <asp:RequiredFieldValidator ID="rfvStartDt"
                                            runat="server" ControlToValidate="txtStartDate" Display="None" ErrorMessage="Start date is Required"  ValidationGroup="search" 
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                         <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="rfvStartDt" />
                                        <asp:RegularExpressionValidator ID="vceStartDt" ControlToValidate = "txtStartDate" ValidationGroup="search" 
                                            ValidationExpression = "^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="vceStartDt" />

                                <label for="" >End</label>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control"
                                            MaxLength="50" Width="130px" autocomplete="off"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtEndDate">
                                        </asp:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvEndDt"
                                            runat="server" ControlToValidate="txtEndDate" Display="None" ErrorMessage="End date is Required"  ValidationGroup="search" 
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceEndDt" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="rfvEndDt" />
                                        <asp:RegularExpressionValidator ID="rfvEndDt1" ControlToValidate = "txtEndDate" ValidationGroup="search" 
                                            ValidationExpression = "^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="vceEndDt1" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvEndDt1" />
                                                                                           
                                 <asp:LinkButton ID="lnkSearch" CssClass="btn submit" runat="server" CausesValidation="true" ToolTip="Refresh" ValidationGroup="search" 
                                            OnClick="lnkSearch_Click"><i class="fa fa-refresh"></i></asp:LinkButton>
                                
                            </div>
                                      
                        </div>
                                           </ContentTemplate>
                                </asp:UpdatePanel>

                        <div class="clearfix"></div>
                        <div class="col-lg-12 col-md-12">
                            <div></div>
                                   
                            <rsweb:ReportViewer ID="rvSalesTax" runat="server" Width="1050px" Height="1500px"
                                BorderColor="Gray" BorderStyle="None" BorderWidth="1px"
                                AsyncRendering="false" ShowZoomControl="False" OnReportRefresh="rvSalesTax_ReportRefresh">
                            </rsweb:ReportViewer>
                         
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
            </div>
        </div>
</asp:Content>

