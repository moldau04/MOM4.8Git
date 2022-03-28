<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="PrintList" Codebehind="PrintList.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        //function pageLoad() {
        //    $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);
        //}

        //function hideModalPopupViaClient(ev) {
        //    ev.preventDefault();
        //    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        //    modalPopupBehavior.hide();
        //}

        function showMailReport() {
           // jQuery("#txtTo").text = "";
            jQuery("#txtCC").text = "";
            $("#programmaticModalPopup").show();
            $('#setuppopup').modal('show');
        }
 function windowclose() {
window.close();
}
    </script>

  <%--  <script type="text/javascript">
        $(document).ready(function () {

            var selectedValue = $(<%= ddlReport.ClientID  %>).val();
            if (selectedValue == "9") { $('#ForchkGroupByWorker').show(); }
            else { $('#ForchkGroupByWorker').hide(); }

            $(<%= ddlReport.ClientID  %>).change(function () {
                var selectedValue = $(this).val();
                if (selectedValue == "9") {  $('#ForchkGroupByWorker').show(); }
                else { $('#ForchkGroupByWorker').hide(); }
            });
        });
</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">

        <div class="page-cont-top">
            <%-- <ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="#">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="#">Customer Manager</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="#">Locations</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Edit Locations</span>
                </li>
            </ul>--%>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Reports</asp:Label></li>
                        <li><a id="LinkButton1" onclick="showMailReport();" title="Mail Report" class="icon-mail"></a></li>
                        <li><a id="A1" href="#" onclick="windowclose();" title="Back" class="icon-closed"></a></li>
                    </ul>
                    <%--<asp:LinkButton ID="LinkButton1" runat="server"
                    OnClientClick="showMailReport();">Mail Report</asp:LinkButton>--%>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <%--<div class="col-lg-4 col-md-4">
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label1">
                                    From
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtfromDate" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtfromDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtfromDate">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label1">
                                    To
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtToDate">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                        </div>
                    </div>--%>

                    <div class="search-customer">
                        <div class="sc-form">
                            Select Report
                                    <asp:DropDownList Width="250px" ID="ddlReport" runat="server" CssClass="form-control input-sm ">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                        <asp:ListItem Value="0">Ticket Report</asp:ListItem>
                                        <asp:ListItem Value="6">Ticket Report by WO</asp:ListItem>
                                        <asp:ListItem Value="5">Ticket Report (Signature)</asp:ListItem> 
                                        <asp:ListItem Value="1">Expense Report</asp:ListItem>
                                        <asp:ListItem Value="2">Time Sheet Report</asp:ListItem>
                                        <asp:ListItem Value="13">Time Sheet Report by Department</asp:ListItem>
                                         <asp:ListItem Value="14">Time Sheet Report ( No TT )</asp:ListItem>
                                        <asp:ListItem Value="3">Callback Report</asp:ListItem>
                                        <asp:ListItem Value="4">Details Report</asp:ListItem>
                                        <asp:ListItem Value="7">Worker Report</asp:ListItem>
                                        <asp:ListItem Value="8">TicketList Report</asp:ListItem> 
                                        <asp:ListItem Value="9">Installation Schedule</asp:ListItem>
                                        <asp:ListItem Value="10">Monthly Maintenance</asp:ListItem>
                                        <asp:ListItem Value="11">Schedule</asp:ListItem>
                                        <asp:ListItem Value="12"> Labor by Department </asp:ListItem> 

                                        
                                    </asp:DropDownList>
                            <asp:CheckBox ID="chkGrp" runat="server" Text="By Category" AutoPostBack="true" OnCheckedChanged="chkGrp_CheckedChanged" Visible="false" />
                          

                         <%-- <span id="ForchkGroupByWorker" > <asp:CheckBox ID="chkGroupByWorker" runat="server"  Text="Group By Worker" AutoPostBack="false"  /> </span>--%>
                           
                              <asp:LinkButton ID="btnSearch" CssClass="btn submit"
                                runat="server" CausesValidation="false" OnClick="btnSearch_Click">
                                        <i class="fa fa-search"></i>
                            </asp:LinkButton>
                             
                        </div>
                    </div>




                    <div class="col-lg-12 col-md-12">
                        <div class="row">
                            <div class="table-scrollable">
                                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="550px"
                                    BorderColor="Gray" BorderStyle="None" BorderWidth="1px" ShowPageNavigationControls="true" PageCountMode="Actual" AsyncRendering="false"
                                    ShowZoomControl="False" OnReportRefresh="ReportViewer1_ReportRefresh">
                                </rsweb:ReportViewer>
                            </div>
                        </div>
                    </div>

                    <div class="modal fade" id="setuppopup" tabindex="-1" role="basic" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header" style="padding: 0px">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                    <div style="background: #316b9d; padding: 10px 15px; font-size: 15px; color: #dadedf; line-height: 20px !important;">
                                        <h4 class="modal-title">
                                            <asp:Label CssClass="title_text" ID="Label15" runat="server">Setup </asp:Label>
                                        </h4>
                                    </div>
                                </div>
                                <div class="modal-body">
                                    <asp:Panel runat="server" ID="programmaticPopup">
                                        <%--  <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                                        CausesValidation="False" />
                                    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                                        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                                        PopupDragHandleControlID="programmaticPopupDragHandle" BackgroundCssClass="custsetup-popup"
                                        DropShadow="True" RepositionMode="RepositionOnWindowResizeAndScroll">
                                    </asp:ModalPopupExtender>--%>
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
                                                        Rows="5" CssClass="form-control" Text="This is report email sent from Mobile Office Manager. Please find the Ticket Report attached."></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="modal-footer custsetup-btn">
                                    <asp:Button CssClass="btn default" data-dismiss="modal" runat="server" ID="LinkButton4" Text="Close" CausesValidation="False" />
                                    <%-- <a id="hideModalPopupViaClientButton" href="#" style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px;">Close</a>--%>
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
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>
