<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/HomeMaster.master" Inherits="TicketByType" Codebehind="TicketByType.aspx.cs" %>

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
        .padding{
                padding:0px;
                }
        .dailog-css{
                background: #316b9d;
                padding: 10px 15px;
                font-size: 15px;
                color: #dadedf;
                line-height: 20px !important;
                }
        .droplist{
            padding: 2px 2px 1px 2px !important;
        }
        .radio-css{
            margin-left: 39px;
        }
       /*celender*/
       .ajax__calendar .ajax__calendar_container{
           border: 1px solid #fff!important;
            background-color: #fdfdfd!important;
            color: #000000!important;
            box-shadow:rgba(100, 100, 111, 0.2) 0px 7px 29px 0px!important;
            border-radius: 8px!important;
            padding: 0px!important;
       }
    .ajax__calendar_container TABLE {
        height:100%!important;
        width:100%!important;
    }
    .ajax__calendar_header{
        height: 25px!important;
        width: 100%!important;
        padding: 5px 2px!important;
         background-color: #316b9d!important;
         color: #fff!important;
         border-radius: 5px!important;
             margin-bottom: 5px!important;
    }
    .ajax__calendar .ajax__calendar_dayname {
    font-weight:bold!important;
    }
    .ajax__calendar_next {
        background-color: #fff;
        border-radius: 10px!important;
        margin-right: 5px;
    }
    .ajax__calendar_prev {
        background-color: #fff;
        border-radius: 10px!important;
        margin-left: 5px;
    }
    .ajax__calendar .ajax__calendar_hover{
        color:#fff!important;
    }
    .ajax__calendar .ajax__calendar_hover .ajax__calendar_day {
            background-color: #edf9ff;
            border-color: #daf2fc;
            color: #0066cc;
            border-radius: 50%!important;
            }
    .ajax__calendar_day {
        text-align:center!important;
    }
    .ajax__calendar .ajax__calendar_day {
        border-radius: 50%!important;
        border-color: none;
    }
    .ajax__calendar .ajax__calendar_active .ajax__calendar_day {
    background-color: #316b9d!important;
    border-color: #ffffff!important;
    color: #ffffff!important;
    }
    .ajax__calendar_today {
    cursor: pointer;
    padding-top: 2px;
    margin-top: 5px!important;
    margin-bottom: 5px;
    }
    .ajax__calendar .ajax__calendar_hover .ajax__calendar_title {
    color: #000!important;
}
    .ajax__calendar .ajax__calendar_active .ajax__calendar_month  {
    background-color: #316b9d!important;
    border-color: #ffffff!important;
    color: #ffffff!important;
    border-radius: 50%!important;
    }
    .ajax__calendar .ajax__calendar_hover .ajax__calendar_year {
          background-color: #316b9d!important;
    border-color: #ffffff!important;
    color: #ffffff!important;
    border-radius: 50%!important;
    }
    .ajax__calendar_month {
        height: 40px!important;
        width: 40px!important;
    }
    .ajax__calendar .ajax__calendar_month {
    border: 1px solid #ffffff;
    border-radius: 50%!important;
    margin-top: 5px;
    }
    .ajax__calendar .ajax__calendar_year {
    border: 1px solid #ffffff00;
    border-radius: 50%!important;
    }
    .ajax__calendar_year {
        height: 40px!important;
        width: 40px!important;
    }
    .ajax__calendar .ajax__calendar_active .ajax__calendar_year {
    background-color: #316b9d!important;
    border-color: #0066cc00!important;
    color: #ffffff!important;
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
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Completed Ticket List</asp:Label></li>
                        <li><a id="LinkButton1" onclick="showMailReport();" class="icon-mail" title="Mail Report"></a></li>
                        <li>
                            <ul class="nav navbar-nav pull-right">
                                <li class="dropdown dropdown-user">
                                    <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print droplist" data-hover="dropdown" data-close-others="true" ></a>
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
                                            MaxLength="50" Width="130px" onkeypress="return false;" autocomplete="off"></asp:TextBox>
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
                                <asp:UpdatePanel ID="UpdatePanelCheckBoxes" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:RadioButton ID="rdExpandAll" class="radio-css" Text="Detail" runat="server" GroupName="rdExpColl" Checked="true" OnCheckedChanged="rdExpCollAll_CheckedChanged" AutoPostBack="true" />
                                        <asp:RadioButton ID="rdCollapseAll" class="radio-css" Text="Summary" runat="server" GroupName="rdExpColl" OnCheckedChanged="rdExpCollAll_CheckedChanged" AutoPostBack="true"/>
                                    </ContentTemplate>
                                 </asp:UpdatePanel>
                                </div>
                        </div>
                               <%-- <div class="sc-form">

                                    <div>
                                          <div class="search-customer">
                                        <div class="sc-form" style="position: relative;">
                                            <label for="">Search the accounts</label>
                                            <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control input-sm input-small" AutoPostBack="true">                                               
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvAccount" ControlToValidate="ddlAccount"
                                    ErrorMessage="Please select Account" Display="None" InitialValue="0"
                                    ValidationGroup="Check"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vceAccount" runat="server" Enabled="True" PopupPosition="Right"
                                    TargetControlID="rfvAccount" />
                                            </div>
                                              </div>
                                    </div>
                                   
                                    <div>
                                        <label for="">Start</label>
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"
                                            MaxLength="50" Width="130px" onkeypress="return false;" autocomplete="off"></asp:TextBox>
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
                                        <label for="" style="margin-left: 39px; margin-bottom: 19px;">End</label>
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
                                  <asp:UpdatePanel ID="UpdatePanelCheckBoxes" runat="server" UpdateMode="Always">
                                 </asp:UpdatePanel>
                            </div>
                        </div>--%>

                         <asp:UpdatePanel ID="updRvBill" runat="server">
                          <ContentTemplate>
                        <div class="clearfix"></div>
                        <div class="col-lg-12 col-md-12">
                            <div></div>
                                   
                            <rsweb:ReportViewer ID="rvTicketList" runat="server" Width="990px" Height="1500px"
                                BorderColor="Gray" BorderStyle="None" BorderWidth="1px"
                                AsyncRendering="false" ShowZoomControl="False" OnReportRefresh="rvTicketList_ReportRefresh">
                            </rsweb:ReportViewer>
                             </div>
                         </ContentTemplate>
                         <Triggers>
                             <asp:AsyncPostBackTrigger ControlID="rdExpandAll" />
                            <asp:AsyncPostBackTrigger ControlID="rdCollapseAll" />
                             <asp:AsyncPostBackTrigger ControlID="lnkSearch" />
                             <%--<asp:AsyncPostBackTrigger ControlID="ddlInvoice" />--%>
                        </Triggers>
                    </asp:UpdatePanel>
                    </div>
                    <div class="modal fade" id="incomepopup" tabindex="-1" role="basic" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header " >
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                    <div class="dailog-css" >
                                        <h4 class="modal-title">
                                            <asp:Label CssClass="title_text" ID="Label15" runat="server">Mail </asp:Label>
                                        </h4>
                                    </div>
                                </div>
                                <div class="modal-body">
                                    <asp:Panel runat="server" ID="programmaticPopup">
                                        <asp:Panel runat="Server" ID="programmaticPopupDragHandle">
                                        </asp:Panel>
                                        <div class="col-lg-12">
                                            <div class="form-col">
                                                <div>
                                                    From
                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtFrom_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                        TargetControlID="txtFrom">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                                        ControlToValidate="txtFrom" Display="None"
                                                        ErrorMessage="Invalid E-Mail Address"
                                                        ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                        ValidationGroup="mail"></asp:RegularExpressionValidator>
                                                    <asp:ValidatorCalloutExtender ID="RegularExpressionValidator3_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator3">
                                                    </asp:ValidatorCalloutExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                        ControlToValidate="txtFrom" Display="None"
                                                        ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                                        ValidationGroup="mail"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div>
                                                    To
                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtTo" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtTo_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                        TargetControlID="txtTo">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                        ControlToValidate="txtTo" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                        ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                        ValidationGroup="mail"></asp:RegularExpressionValidator>
                                                    <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                                    </asp:ValidatorCalloutExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                        ControlToValidate="txtTo" Display="None"
                                                        ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                                        ValidationGroup="mail"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div>
                                                    CC
                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtCC" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtCC_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                        TargetControlID="txtCC">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                        ControlToValidate="txtCC" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                        ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                        ValidationGroup="mail"></asp:RegularExpressionValidator>
                                                    <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div>
                                                    <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Columns="50"
                                                        Rows="5" CssClass="form-control" Text="This is report email sent from Mobile Office Manager. Please find the AP Aging Report By Due Date attached."></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="modal-footer custsetup-btn">
                                    <asp:Button CssClass="btn default" data-dismiss="modal" runat="server" ID="LinkButton4" Text="Close" CausesValidation="False" />

                                    <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Send" OnClick="hideModalPopupViaServerConfirm_Click"
                                        CssClass="btn blue" ValidationGroup="mail" />
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <!-- /.modal-content -->
                        </div>
                        <!-- /.modal-dialog -->
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
