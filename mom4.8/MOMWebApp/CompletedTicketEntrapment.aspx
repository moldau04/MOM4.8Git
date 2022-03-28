<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Mom.master" CodeBehind="CompletedTicketEntrapment.aspx.cs" Inherits="MOMWebApp.CompletedTicketEntrapment" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />

    <style type="text/css">
        body:nth-of-type(1) img[src*="Blank.gif"] {
            display: none;
        }

        table input[type="text"], input[type="password"], input[type="email"], input[type="url"], input[type="time"], input[type="date"], input[type="datetime-local"], input[type="tel"], input[type="number"], input[type="search"] {
            background-color: white !important;
            margin: 0px !important;
            height: 17px !important;
        }

        td, th {
            padding: 0px 5px !important;
            display: table-cell;
            text-align: left;
            vertical-align: middle;
        }

        .materialize-textarea {
            max-height: 100px !important;
            height: 100px !important;
        }

        body {
            background-color: white !important;
        }

        .RadComboBox_Metro .rcbInner {
            padding-top: 0px !important;
        }

        .stiJsViewerPageShadow {
            height: auto !important;
        }

        .RadComboBoxDropDown.RadComboBoxDropDown_Metro {
            width: 220px !important;
        }
    </style>
    <script type="text/javascript">
        function showMailReport() {
            jQuery("#txtTo").text = "";
            jQuery("#txtCC").text = "";
            //$("#programmaticModalPopup").show();
            //$('#incomepopup').modal('show');

            var radwindow = $find('<%=mailWindow.ClientID %>');
            radwindow.show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="mainWindow">
        <div class="divbutton-container">
            <div id="divButtons">
                <div id="breadcrumbs-wrapper">
                    <header>
                        <div class="container row-color-grey">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">
                                        <div class="page-title"><i class="mdi-action-swap-vert-circle"></i>&nbsp; Completed Ticket Level
                                            <asp:Label CssClass="title_text" ID="lblLevelNames" runat="server"></asp:Label>
                                            Report</div>
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <a id="mailReport" href="javascript:void(0);" onclick="showMailReport();return false;">Email</a>
                                            </div>
                                            <ul class="nomgn hideMenu menuList">
                                                <li>
                                                    <asp:Label CssClass="title_text" ID="lblHeader" runat="server"></asp:Label></li>
                                                <li>
                                                    <asp:LinkButton CssClass="icon-closed" runat="server" CausesValidation="false" ToolTip="close"
                                                        OnClick="lnkClose_Click"></asp:LinkButton></li>
                                            </ul>
                                        </div>
                                        <div class="btnclosewrap">
                                            <asp:LinkButton runat="server" OnClick="lnkClose_Click" ID="lnkClose"><i class="mdi-content-clear"></i></asp:LinkButton>
                                        </div>
                                        <div class="rght-content">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </header>
                </div>
            </div>
        </div>
        <div class="container">
            <div class="grid_container">
                <div class="form-section-row" style="margin-bottom: 0 !important;">
                    <cc1:StiWebViewer ID="StiWebViewerCompletedTicket" Height="800px" RequestTimeout="20000" CacheMode="None" runat="server" ViewMode="Continuous" ScrollbarsMode="true"
                        OnGetReport="StiWebViewerCompletedTicket_GetReport" OnGetReportData="StiWebViewerCompletedTicket_GetReportData" />
                </div>
            </div>

        </div>
    </div>


    <telerik:RadWindowManager ID="RadWindowManagerCustomer" runat="server">
        <Windows>
            <telerik:RadWindow ID="mailWindow" Skin="Material" VisibleTitlebar="true" Title="Mail" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="600" Height="450">
                <ContentTemplate>
                    <div style="margin-top: 15px;">
                        <div class="form-section-row">
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
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
                                        <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtFrom">From</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
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
                                        <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtTo"> To</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-section-row">

                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">

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
                                        <asp:TextBox ID="txtCC" runat="server"></asp:TextBox>
                                        <asp:Label runat="server" AssociatedControlID="txtCC">CC</asp:Label>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section2-blank">
                                &nbsp;
                            </div>
                            <div class="form-section2">
                                <div class="input-field col s12">
                                    <div class="row">
                                        &nbsp;
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="form-section-row">

                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:Label runat="server" AssociatedControlID="txtBody">Mail Body</asp:Label>
                                        <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" CssClass="materialize-textarea" Text="This is report email sent from Mobile Office Manager. Please find the Completed Ticket Level Report attached."></asp:TextBox>
                                    </div>
                                </div>

                            </div>

                        </div>


                        <div style="clear: both;"></div>

                        <footer style="float: right;">
                            <div class="btnlinks">
                                <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Save" OnClick="hideModalPopupViaServerConfirm_Click"
                                    ValidationGroup="mail" />
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
</asp:Content>