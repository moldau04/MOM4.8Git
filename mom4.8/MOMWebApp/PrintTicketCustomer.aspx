<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="PrintTicketCustomer" Codebehind="PrintTicketCustomer.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb10" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function pageLoad() {
            $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);
            $addHandler($get("A1"), 'click', hideModalPopupViaClientCust);
        }

        function hideModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }
    </script>
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
                    <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Service History</asp:Label>
                    <asp:LinkButton runat="server" ID="lnkBack"
                    OnClick="lnkBack_Click">Back</asp:LinkButton>
                <%--<asp:LinkButton ID="LinkButton1" runat="server"
                    OnClick="LinkButton1_Click">Mail Report</asp:LinkButton>--%>
                <a id="LinkButton1" runat="server" onclick="$('#setuppopup').modal('show');">Mail Report</a>
            </div>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div class="table-scrollable">
                        <rsweb10:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="550px"
                            BorderColor="Gray" BorderStyle="None" BorderWidth="1px" ShowPageNavigationControls="false" AsyncRendering="false"
                            ShowZoomControl="False">
                        </rsweb10:ReportViewer>
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
                                <%--<asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                                    CausesValidation="False" />
                                <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                                    TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                                    PopupDragHandleControlID="programmaticPopupDragHandle" BackgroundCssClass="pnlUpdateoverlay"
                                    DropShadow="True" RepositionMode="RepositionOnWindowResizeAndScroll">
                                </asp:ModalPopupExtender>--%>
                                <asp:Panel runat="server" ID="programmaticPopup">
                                    <asp:Panel runat="Server" ID="programmaticPopupDragHandle">
                                    </asp:Panel>
                                    <div>
                                        <div class="col-lg-12 col-md-12">
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
                                        </div>

                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="modal-footer custsetup-btn">
                                <asp:Button ID="hideModalPopupViaClientButton" href="#" CssClass="btn default" data-dismiss="modal" runat="server" Text="Close" CausesValidation="False" />
                                <%-- <a id="hideModalPopupViaClientButton" href="#" style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px;">Close</a>--%>
                                <%-- <asp:LinkButton runat="server" ID="LinkButton2" Text="Send" OnClick="hideModalPopupViaServerConfirm_Click"
                                    CssClass="btn blue" ValidationGroup="mail" />--%>
                                <%--<a id="hideModalPopupViaClientButton" role="button" CssClass="btn default" data-dismiss="modal" runat="server">Close</a>--%>
                                <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Send" OnClick="hideModalPopupViaServerConfirm_Click"
                                    CssClass="btn blue" ValidationGroup="mail" />
                            </div>
                            <div class="clearfix"></div>
                        </div>
                        <!-- /.modal-content -->
                    </div>
                    <!-- /.modal-dialog -->
                </div>


            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>




</asp:Content>
