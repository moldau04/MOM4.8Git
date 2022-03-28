<%@ Page Language="C#" MasterPageFile="~/Mom.master"  AutoEventWireup="true" Inherits="OpportunityReport" Codebehind="OpportunityReport.aspx.cs" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <script>
        function openMailForm() {
            window.onload = window.radopen(null, "RadCreateWindow");
        }
       
        window.onload = function () {
            var reloading = sessionStorage.getItem("reloading");
            if (reloading) {
                sessionStorage.removeItem("reloading");
                openMailForm();
            }
        }
        function reloadP() {
            sessionStorage.setItem("reloading", "true");
            document.location.reload();
        }
        function cancel() {
            var window1 = $find('<%=RadCreateWindow.ClientID %>');
            window1.Close();
            return false;
        }
        function refreshPage(url) {
            window.setTimeout(function () {
                window.location.href = url;
            }, 7500);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;<asp:Label CssClass="title_text" ID="lblHeader" runat="server">Opportunities Report</asp:Label></div>

                                    <div class="btnlinks">
                                        <%--<asp:LinkButton ID="lnkEmail" ToolTip="Email" runat="server" CausesValidation="false" OnClientClick="reloadP();return false"
                                                OnClick="lnkEmail_Click">Mail Report</asp:LinkButton>--%>
                                    </div>

                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label ID="lblInv" Visible="false" runat="server"></asp:Label>
                                        </div>

                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                                OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
    <cc1:StiWebViewer ID="StiWebViewerOpportunity" Height="800px" runat="server" ScrollbarsMode="true" RequestTimeout="900000" Zoom="100" BackgroundColor="White"
        OnGetReport="StiWebViewerOpportunity_GetReport" OnGetReportData="StiWebViewerOpportunity_GetReportData" ViewMode="Continuous" />

    <div class="clearfix"></div>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadCreateWindow" runat="server" Modal="true" Width="900" Height="700">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-12 col-md-12">
                            <div class="section-ttle">
                                <span class="mail-height" >Mail Invoice
                                </span>
                                <span class="mail-sub" >
                                    <div class="btnlinks" runat="server" id="lnkCancelContact">
                                        <%--<asp:LinkButton ID="LnkSend" runat="server"
                                            OnClick="LnkSend_Click">Send</asp:LinkButton>
                                        <asp:LinkButton ID="lnkCloseMail" runat="server" OnClientClick="return cancel()" OnClick="lnkCloseMail_Click" Text="Close"></asp:LinkButton>--%>
                                    </div>
                                </span>
                            </div>
                        </div>
                        <!-- edit-tab start -->
                        <div class="col-lg-12 col-md-12">
                            <div class="com-cont">
                                <div class="col-md-8 col-lg-8">
                                    <div class="form-col">
                                        <div class="fc-label">
                                        </div>
                                        <div class="fc-input">
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            <label>From</label>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtEmailFrom" runat="server"
                                                CssClass="form-control" Text="info@mom.com"
                                                TabIndex="9" ToolTip="From" Placeholder="From"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtEmailFrom_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                TargetControlID="txtEmailFrom">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server"
                                                ControlToValidate="txtEmailFrom" Display="None"
                                                ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator7_ValidatorCalloutExtender"
                                                runat="server" Enabled="True"
                                                TargetControlID="RegularExpressionValidator7">
                                            </asp:ValidatorCalloutExtender>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            To
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtEmail" runat="server"
                                                CssClass="form-control"
                                                TabIndex="9" ToolTip="To" Placeholder="To"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                TargetControlID="txtEmail">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtEmail"
                                                Display="None" ErrorMessage="Email Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator9_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator9">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                ValidationGroup="mail"></asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                                runat="server" Enabled="True"
                                                TargetControlID="RegularExpressionValidator1">
                                            </asp:ValidatorCalloutExtender>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            CC
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtEmailCc" runat="server"
                                                CssClass="form-control"
                                                TabIndex="9" ToolTip="Cc" Placeholder="Cc"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtEmailCc_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                TargetControlID="txtEmailCc">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                                ControlToValidate="txtEmailCc" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                ValidationGroup="mail"></asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator6_ValidatorCalloutExtender"
                                                runat="server" Enabled="True"
                                                TargetControlID="RegularExpressionValidator6">
                                            </asp:ValidatorCalloutExtender>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Subject
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtSubject" runat="server"
                                                CssClass="form-control"
                                                TabIndex="9" ToolTip="Subject" Placeholder="Subject"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Body
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Columns="50"
                                                Rows="10" Height="350px" CssClass="form-control"></asp:TextBox>
                                            <asp:HtmlEditorExtender ID="htmlEditorExtender1" TargetControlID="txtBody"
                                                runat="server" EnableSanitization="False" Enabled="True">
                                            </asp:HtmlEditorExtender>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <!-- edit-tab end -->
                        <div class="clearfix"></div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <!-- END DASHBOARD STATS -->
    <div class="clearfix"></div>
</asp:Content>
