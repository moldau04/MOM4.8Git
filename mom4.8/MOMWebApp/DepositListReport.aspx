<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="DepositListReport" Codebehind="DepositListReport.aspx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
        .RadComboBox_Metro .rcbInner
        {
            padding-top:0px!important;
        }
        .stiJsViewerPageShadow {
            height: auto!important;
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
                                            <div class="page-title">
                                                <i class="mdi-editor-insert-drive-file"></i>&nbsp;
                                                <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Deposit List Report</asp:Label>
                                            </div>
                                            <div class="buttonContainer">
                                                <div class="btnlinks">
                                                    <a onclick="showMailReport();" style="cursor: pointer;" title="Mail Report">Email</a>
                                                </div>
                                            </div>                                        
                                            
                                            <div class="btnclosewrap">
                                                <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
     
    <div class="container">
        <div class="row">           
         
                <div class="grid_container">
                    <cc1:StiWebViewer ID="StiWebViewerARReport" Height="800px" runat="server" ScrollbarsMode="true" RequestTimeout="900000" Zoom="100" CacheMode="None" BackgroundColor="White"
                        OnGetReport="StiWebViewerARReport_GetReport" OnGetReportData="StiWebViewerARReport_GetReportData"   Visible="true" />
                   
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
                                        <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" CssClass="materialize-textarea" Text="This report is generated from Mobile Office Manager. Please find attached the Deposit List Report"></asp:TextBox>
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
</asp:Content>