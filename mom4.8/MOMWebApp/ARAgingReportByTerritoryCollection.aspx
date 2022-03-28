<%@ Page Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" CodeBehind="ARAgingReportByTerritoryCollection.aspx.cs" Inherits="MOMWebApp.ARAgingReportByTerritoryCollection" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        body:nth-of-type(1) img[src*="Blank.gif"] {
            display: none;
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
                                                <asp:Label CssClass="title_text" ID="lblHeader" runat="server">AR Aging by Salesperson Report</asp:Label>
                                            </div>
                                            <div class="buttonContainer">
                                                <div class="btnlinks">
                                                    <a onclick="showMailReport();" style="cursor: pointer;" title="Mail Report">Email</a>
                                                </div>
                                            </div>
                                            <div class="btnlinks">
                                                <a class="dropdown-button" data-beloworigin="true" href="customersreport.aspx" data-activates="dropdown1">Reports
                                                </a>
                                            </div>
                                            <ul id="dropdown1" class="dropdown-content">
                                                <li>
                                                    <a href="InvoicesReport.aspx" class="-text">Invoice Summary Report</a>
                                                </li>
                                            </ul>
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
            <div class="srchpane">

                <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                    Search
                </div>
                <div class="srchinputwrap">
                    <telerik:RadComboBox RenderMode="Auto" ID="rcTerritory" CssClass="browser-default" DropDownAutoWidth="Enabled" runat="server" Filter="StartsWith" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                        EmptyMessage="-- Select --" Skin="Metro">
                    </telerik:RadComboBox>
                </div>
                <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                    As of date
                </div>
                <div class="srchinputwrap">
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="srchcstm"
                        MaxLength="50" Width="130px" autocomplete="off"></asp:TextBox>
                    <asp:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtEndDate">
                    </asp:CalendarExtender>
                    <asp:RequiredFieldValidator ID="rfvEndDt"
                        runat="server" ControlToValidate="txtEndDate" Display="None" ErrorMessage="End date is Required" ValidationGroup="search"
                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="vceEndDt" runat="server" Enabled="True"
                        PopupPosition="Right" TargetControlID="rfvEndDt" />
                    <asp:RegularExpressionValidator ID="rfvEndDt1" ControlToValidate="txtEndDate" ValidationGroup="search"
                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                    </asp:RegularExpressionValidator>
                    <asp:ValidatorCalloutExtender ID="vceEndDt1" runat="server" Enabled="True" PopupPosition="Right"
                        TargetControlID="rfvEndDt1" />
                </div>
                <div class="srchinputwrap rdleftmgn">
                    <div class="rdpairing">
                        <div class="rd-flt">
                            <asp:RadioButton ID="rdExpandAll" Text="Detail" runat="server" GroupName="rdExpColl" OnCheckedChanged="rdExpCollAll_CheckedChanged" Checked="true" AutoPostBack="true" />
                        </div>
                        <div class="rd-flt">
                            <asp:RadioButton ID="rdCollapseAll" Text="Summary" runat="server" GroupName="rdExpColl" OnCheckedChanged="rdExpCollAll_CheckedChanged" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="srchinputwrap rdleftmgn">
                    <div class="rdpairing">
                        <asp:CheckBox ID="chkIncludeNotes" Text="Include Notes" runat="server" CssClass="css-checkbox" />
                    </div>
                </div>
                <div class="srchinputwrap rdleftmgn">
                    <div class="rdpairing">
                        <asp:CheckBox ID="chkCreditFlag" Text="Credit Flag" runat="server" CssClass="css-checkbox" />
                    </div>
                </div>
                <div class="srchinputwrap srchclr btnlinksicon" style="margin-left: -15px; margin-top: -2px;">
                    <asp:LinkButton ID="lnkSearch" runat="server" CausesValidation="true" ToolTip="Refresh"
                        OnClick="lnkSearch_Click" ValidationGroup="search"><i class="mdi-action-search"></i></asp:LinkButton>
                </div>
            </div>
            <div class="grid_container">
                <cc1:StiWebViewer ID="StiWebViewerARReport" Height="800px" runat="server" ScrollbarsMode="true" RequestTimeout="900000" Zoom="100" BackgroundColor="White"
                    OnGetReport="StiWebViewerARReport_GetReport" OnGetReportData="StiWebViewerARReport_GetReportData" ViewMode="Continuous" Visible="true" />

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
                                        <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" CssClass="materialize-textarea" Text="This is report email sent from Mobile Office Manager. Please find the AR Aging by Salesperson Report attached."></asp:TextBox>
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
