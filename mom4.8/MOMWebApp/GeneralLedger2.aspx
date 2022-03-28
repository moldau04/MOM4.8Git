<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/HomeMaster.master" Inherits="GeneralLedger2" Codebehind="GeneralLedger2.aspx.cs" %>

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
          body:nth-of-type(1) img[src*="Blank.gif"] {
                display: none;
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
            <%--<ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="<%=ResolveUrl("~/Home.aspx") %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
               <li>
                    <span>Financial Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Balance Sheet</span>
                </li>

            </ul>--%>
        </div>

        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">General Ledger</asp:Label></li>
                        <li>
                            <ul class="nav navbar-nav pull-right">
                                <li class="dropdown dropdown-user">
                                    <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print" data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
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

                    <div class="table-scrollable" style="border: none">
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
                                        
                                   
                                                <label for="">Search the accounts</label>
                                            <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control" Width="350">                                               
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvAccount" ControlToValidate="ddlAccount"
                                                ErrorMessage="Please select Account" Display="None" InitialValue="0"
                                                ValidationGroup="Check"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceAccount" runat="server" Enabled="True" PopupPosition="Right"
                                                TargetControlID="rfvAccount" />
                                            <%--<asp:DropDownList ID="ddlGroupBy" runat="server" CssClass="form-control" Width="150">
                                                    <asp:ListItem Value="0">Group all transaction</asp:ListItem>
                                                    <asp:ListItem Value="1">Group by date</asp:ListItem>
                                            </asp:DropDownList>--%>
                                                   
                                            <asp:LinkButton ID="lnkSearch" CssClass="btn submit" runat="server" CausesValidation="true" ToolTip="Refresh" ValidationGroup="search" 
                                                    OnClick="lnkSearch_Click"><i class="fa fa-refresh"></i></asp:LinkButton>
                                
                                        </div>  
                                        </div>  
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanelCheckBoxes" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:RadioButton ID="rdDetail" Text="Detail" runat="server" GroupName="rdSumDetail" Checked="true" OnCheckedChanged="rdSumDetail_CheckedChanged" AutoPostBack="true" style="margin-left: 39px;"/>
                                        <asp:RadioButton ID="rdSummary" Text="Summary" runat="server" GroupName="rdSumDetail" OnCheckedChanged="rdSumDetail_CheckedChanged" AutoPostBack="true" style="margin-left: 39px;"/>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="clearfix"></div>
                                <div class="col-lg-12 col-md-12">
                                    <asp:UpdatePanel ID="upGeneralLedger" runat="server" UpdateMode="Always">
                                            <ContentTemplate>         
                                                <rsweb:ReportViewer ID="rvGeneralLedger" runat="server" Width="800px" Height="1500px"
                                                    BorderColor="Gray" BorderStyle="None" BorderWidth="1px"
                                                    AsyncRendering="false" ShowZoomControl="False" OnReportRefresh="rvGeneralLedger_ReportRefresh">
                                                </rsweb:ReportViewer>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="lnkSearch" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

