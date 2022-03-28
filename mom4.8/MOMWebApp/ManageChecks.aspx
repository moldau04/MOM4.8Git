<%@ Page Title="Manage Checks || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="ManageChecks" Codebehind="ManageChecks.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
   <style type="text/css">
       
        @media screen and (max-width: 2048px) {

                .rgDataDiv {
                    height: 50vh !important;
                }

                .RadGrid_Material {
                    font-size: 0.9rem !important;
                }
            }

            @media screen and (max-width: 2304px) {

                .rgDataDiv {
                    height: 52vh !important;
                }

                .RadGrid_Material {
                    font-size: 0.9rem !important;
                }
            }

            @media screen and (max-width: 1920px) {

                .rgDataDiv {
                    height: 47vh !important;
                }
            }

            @media screen and (max-width: 1706px) {

                .rgDataDiv {
                    height: 42vh !important;
                }

                .RadGrid_Material {
                    font-size: 0.9rem !important;
                }
            }

            @media screen and (max-width: 1688px) {

                .rgDataDiv {
                    height: 42vh !important;
                }

                .RadGrid_Material {
                    font-size: 0.9rem !important;
                }
            }

            @media screen and (max-width: 1366px) {

                .rgDataDiv {
                    height: 30vh !important;
                }

                .RadGrid_Material {
                    font-size: 0.9rem !important;
                }
            }
       @media only screen and (min-width: 250px) and (max-width: 700px) {
           .w-100 {
               width: 100% !important;
           }
       }
    </style>
        
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
                                            <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Manage Checks</div>
                                            <div class="buttonContainer">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkWriteCheck" runat="server" OnClick="lnkWriteCheck_Click">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkEditCheck" runat="server" OnClick="lnkEditCheck_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks menuAction">
                                                    <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                    </a>
                                                </div>
                                                 <div class="btnlinks">
                                                    <asp:HyperLink ID="lnkVoidCheck" runat="server" Style="cursor: pointer;" onclick="OpenVoidPopupEdit('void');">Void</asp:HyperLink>
                                                </div>
                                                <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                    <li>
                                                        <div class="btnlinks">
                                                            <%--<asp:HyperLink ID="lnkVoidCheck" runat="server" Style="cursor: pointer;" onclick="OpenVoidPopupEdit('void');">Void</asp:HyperLink>--%>
                                                            <asp:HiddenField ID="hdnCDID" runat="server" />
                                                            <asp:HiddenField ID="hdnCD" runat="server" />
                                                            <asp:HiddenField ID="hdnBankID1" runat="server" />
                                                            <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                                                                <Windows>
                                                                    <telerik:RadWindow ID="VoidCheckWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                                                        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                                                        runat="server" Modal="true" Width="500" Height="300">
                                                                        <ContentTemplate>
                                                                            <div class="m-t-15" >
                                                                                <div class="form-section-row" id="dvVoid" runat="server">
                                                                                    <div class="form-section2">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <asp:TextBox ID="txtVoidDate" runat="server" CssClass="datepicker_mom"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="rfvVoidDate" runat="server"
                                                                                                    ControlToValidate="txtVoidDate" Display="None" ErrorMessage="Void date is Required"
                                                                                                    ValidationGroup="SubCheck" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="vceVoidDate" runat="server" Enabled="True"
                                                                                                    PopupPosition="Right" TargetControlID="rfvVoidDate" />
                                                                                                <asp:RegularExpressionValidator ID="rfvVoidDate1" ControlToValidate="txtVoidDate"
                                                                                                    ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                                                    runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                                                                </asp:RegularExpressionValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="vceVoidDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                                                                    TargetControlID="rfvVoidDate1" />
                                                                                                <label>Please enter check Void Date. </label>

                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div style="clear: both;"></div>
                                                                                <div class="form-section-row" id="dvEditCheck" runat="server">
                                                                                    <div class="form-section2">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <asp:TextBox ID="txtCheckNo" runat="server" MaxLength="9" autocomplete="off"
                                                                                                    onkeypress="return isNumberKey(event,this)" onchange="IsExistCheckNo();"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="rfvCheckNo" runat="server"
                                                                                                    ControlToValidate="txtCheckNo" Display="None" ErrorMessage="Check number is Required"
                                                                                                    ValidationGroup="Check" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="vceCheckNo" runat="server" Enabled="True"
                                                                                                    PopupPosition="Right" TargetControlID="rfvCheckNo" />
                                                                                                <label for="txtCheckNo">Check No. </label>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div style="clear: both;"></div>

                                                                                <footer class="footer-css-top-btn">
                                                                                    <div class="btnlinks">
                                                                                        <asp:LinkButton ID="lnkSave" runat="server" OnClientClick="CloseVoidModal()" OnClick="lnkSave_Click" ValidationGroup="SubCheck" CausesValidation="true"> Save </asp:LinkButton>
                                                                                    </div>
                                                                                    <div class="btnlinks">
                                                                                        <asp:LinkButton ID="lbtnCheckSave" runat="server" OnClick="lbtnCheckSave_Click" CausesValidation="true" ValidationGroup="Check"> Save </asp:LinkButton>
                                                                                    </div>
                                                                                </footer>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </telerik:RadWindow>
                                                                    <telerik:RadWindow ID="ReprintCheckRange" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                                                    Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                                                    runat="server" Modal="true" Width="500" Height="250">
                                                                    <ContentTemplate>
                                                                        <div class="m-t-15" >
                                                                            <div class="form-section-row">
                                                                                <div class="form-section">
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label class="drpdwn-label">Bank Account </label>
                                                                                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="browser-default" ValidationGroup="Check"
                                                                                                    >
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator runat="server" ID="rfvBank" ControlToValidate="ddlBank"
                                                                                                    ErrorMessage="Please select Bank" Display="None" InitialValue="0"
                                                                                                    ValidationGroup="Check"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="vceBank" runat="server" Enabled="True" PopupPosition="Right"
                                                                                                    TargetControlID="rfvBank" />                                    </div>
                                                                                    </div>
                                                                                </div>
                                                                                <%--<div class="form-section3-blank">
                                                                                    &nbsp;
                                                                                </div>--%>
                                                                                <div class="form-section2">
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <asp:TextBox ID="txtcheckfrom" runat="server" MaxLength="19" CssClass="Contact-search"></asp:TextBox>
                                                                                            <label for="txtcheckfrom">Starting CheckNo</label>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="form-section2-blank">
                                                                                    &nbsp;
                                                                                </div>
                                                                                <div class="form-section2">
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <asp:TextBox ID="txtcheckto" runat="server" MaxLength="19" CssClass="Contact-search"></asp:TextBox>
                                                                                            <label for="txtcheckto">Ending CheckNo</label>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div style="clear: both;"></div>
                                                                            <footer class="footer-css" >
                                                                                <div class="btnlinks">
                                                                                    <asp:LinkButton ID="lnkVendSave" runat="server" OnClientClick="OpentemplateModal();return false" >Print Checks</asp:LinkButton>
                                                                                </div>
                                                                            </footer>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </telerik:RadWindow>
                                                                    <telerik:RadWindow ID="RadWindowTemplates" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                                                        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                                                        runat="server" Modal="true" Width="1100"  Title="Check Templates">
                                                                       <ContentTemplate>
                                                                            <div>
                                                                                <%--<div class='col s5' style="width: 100%; float: left;">
                                                                                    <div class='cr-title' style="    padding-top: 5px;   font-size: large;   padding-bottom: 5px;">Check Templates </div>
                                                                                </div>--%>
                                                                                <div class='col s5 select-tem-main'>
                                                                                    <div class='cr-title'>Select a check template. Please note checks will be saved after you exit this screen. </div>
                                                                                </div>
                                                                                
                                                                                <div class='col s5 cr-main'>
                                                                                    <div class='cr-box'>
                                                                                        <div class='cr-title'>AP – check top </div>
                                                                                        
                                                                                        <%--<div class='cr-img'>
                                                                                            
                                                                                            <asp:Label ID="lbltopcom" runat="server" Text="XYZ Company" style="position: absolute; padding-left: 20px; padding-top: 15px; font-weight: bolder; font-size: 12px;"></asp:Label>
                                                                                            <asp:Label ID="lbltopdd" runat="server" Text="9418 Galvin Ave, ,San Diago, Suit #100" style="position: absolute; padding-left: 20px; padding-top: 60px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lbltopemail" runat="server" Text="info@expertservicesolution.com" style="position: absolute; padding-left: 20px; padding-top: 80px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                            
                                                                                        </div>
                                                                                        <div class='cr-img'>
                                                                                            <img src='images/ReportImages/ApTopCheck.jpg' alt='' style="position: absolute;margin-top: 40px;height: 265px;width: 320px;">
                                                                                        </div>--%>
                                                                                        <div class='cr-date' >
                                                                                            <div class='cr-iocn'>
                                                                                                <asp:DropDownList ID="ddlApTopCheckForLoad" runat="server"
                                                                                                    CssClass="browser-default" OnSelectedIndexChanged="ddlApTopCheckForLoad_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                                <div class='cr-iocn'>
                                                                                                    <asp:ImageButton ID="imgPrintTemp1" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp1_Click" ToolTip="Export to PDF" />
                                                                                                    <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="25px" Width="25px"  OnClick="ImageButton7_Click" ToolTip="Edit Template" />
                                                                                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkSaveDefault_Click" ToolTip="Set as Default" />
                                                                                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" OnClick="ImageButton3_Click" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div style="clear: both;"></div>
                                                                                </div>
                                                                                  <div class='col s5 cr-main'>
                                                                                    <div class='cr-box'>
                                                                                        <div class='cr-title'>AP – check middle </div>
                                                                                        
                                                                                        <%--<div class='cr-img'>
                                                                                            
                                                                                            <asp:Label ID="lblmidcom" runat="server" Text="XYZ Company" style="position: absolute; padding-left: 20px; padding-top: 15px; font-weight: bolder; font-size: 12px;"></asp:Label>
                                                                                            <asp:Label ID="lblmidadd" runat="server" Text="9418 Galvin Ave, ,San Diago, Suit #100" style="position: absolute; padding-left: 20px; padding-top: 60px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lblmidemail" runat="server" Text="info@expertservicesolution.com" style="position: absolute; padding-left: 20px; padding-top: 80px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                        </div>
                                                                                        <div class='cr-img'>
                                                                                            <img src='images/ReportImages/MidCheck.jpg' alt='' style="position: absolute;margin-top: 40px;height: 265px;width: 320px;">
                                                                                        </div>--%>
                                                                                        <div class='cr-date' >

                                                                                            <asp:DropDownList ID="ddlApMiddleCheckForLoad" runat="server"
                                                                                                CssClass="browser-default" OnSelectedIndexChanged="ddlApMiddleCheckForLoad_SelectedIndexChanged">
                                                                                            </asp:DropDownList>
                                                                                            <div class='cr-iocn' >
                                                                                                <asp:ImageButton ID="imgPrintTemp2" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp2_Click" ToolTip="Export to PDF" />
                                                                                                <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="30px" Width="30px" OnClick="ImageButton8_Click" ToolTip="Edit Template" />
                                                                                                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkSaveApMiddleCheck_Click" ToolTip="Set as Default" />
                                                                                                <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" OnClick="ImageButton6_Click" />

                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div style="clear: both;"></div>
                                                                                </div>
                                                                                  <div class='col s5 cr-main'>
                                                                                    <div class='cr-box'>
                                                                                        <div class='cr-title'>AP – Detailed check top </div>
                                                                                        
                                                                                        <%--<div class='cr-img'>
                                                                                            
                                                                                            <asp:Label ID="lbldetailcom" runat="server" Text="XYZ Company" style="position: absolute; padding-left: 20px; padding-top: 15px; font-weight: bolder; font-size: 12px;"></asp:Label>
                                                                                            <asp:Label ID="lbldetailadd" runat="server" Text="9418 Galvin Ave, ,San Diago, Suit #100" style="position: absolute; padding-left: 20px; padding-top: 60px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lbldetailemail" runat="server" Text="info@expertservicesolution.com" style="position: absolute; padding-left: 20px; padding-top: 80px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                        </div>
                                                                                        <div class='cr-img'>
                                                                                            <img src='images/ReportImages/TopDetailCheck.jpg' alt='' style="position: absolute;margin-top: 40px;height: 265px;width: 320px;">
                                                                                        </div>--%>
                                                                                        <div class='cr-date' >
                                                                                            <asp:DropDownList ID="ddlTopChecksForLoad" runat="server"
                                                                                                CssClass="browser-default" OnSelectedIndexChanged="ddlTopChecksForLoad_SelectedIndexChanged">
                                                                                            </asp:DropDownList>

                                                                                            <div class='cr-iocn' >
                                                                                                <asp:ImageButton ID="imgPrintTemp6" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp6_Click" ToolTip="Export to PDF" />
                                                                                                <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="30px" Width="30px" OnClick="ImageButton9_Click" ToolTip="Edit Template" />
                                                                                                <asp:ImageButton ID="ImageButton13" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkTopChecks_Click" />
                                                                                                <asp:ImageButton ID="ImageButton14" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" OnClick="ImageButton14_Click" />

                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div style="clear: both;"></div>
                                                                            </div>
                                                                            <div class="btnlinks">
                                                                                <asp:LinkButton ID="btnSave2" runat="server" Visible="false" ValidationGroup="Check" CausesValidation="true" OnClick="btnSubmit_Click">
                                                                                                   Cut Check
                                                                                </asp:LinkButton>
                                                                                <asp:Label ID="txtMessage" runat="server" ForeColor="Green"></asp:Label>
                                                                            </div>
                                                                          <div id="loaders" class="loaded1" ><img src="images/ajax-loader-trans.gif" style="height: 30px;" /> </div>
                                                                        </ContentTemplate>

                                                                    </telerik:RadWindow>
                                                                    <telerik:RadWindow ID="RadWindowFirstReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true" 
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1200" Height="700" >
                <ContentTemplate>
                    <div class="rptSti">
                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner1" runat="server" OnSaveReport="StiWebDesigner1_SaveReport" Height="700" Width="100%" OnSaveReportAs="StiWebDesigner1_SaveReportAs" OnExit="StiWebDesigner1_Exit" />
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowSecondReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1200" Height="700" >
                <ContentTemplate>
                    <div class="rptSti">
                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner2" runat="server" OnSaveReport="StiWebDesigner2_SaveReport" Height="700" Width="100%" OnSaveReportAs="StiWebDesigner2_SaveReportAs" OnExit="StiWebDesigner2_Exit" />
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowThirdReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1200" Height="700" >
                <ContentTemplate>
                    <div class="rptSti">
                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner3" runat="server" OnSaveReport="StiWebDesigner3_SaveReport" Height="700" Width="100%" OnSaveReportAs="StiWebDesigner3_SaveReportAs" OnExit="StiWebDesigner3_Exit" />
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

                                                                </Windows>
                                                            </telerik:RadWindowManager>
                                                        </div>
                                                    </li>
                                                </ul>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkDeleteCheck" OnClientClick="return  confirm('Are you sure that you want to delete check ?');" runat="server" OnClick="lnkDeleteCheck_Click">Delete</asp:LinkButton>
                                                </div>

                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnExcel" runat="server" OnClick="btnExcel_Click">Export to Excel</asp:LinkButton>

                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkPrint" runat="server" OnClick="lnkPrint_Click">Print</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks" style="display:none;">
                                                    <asp:HyperLink ID="lnkEditCheckNum" runat="server" Style="cursor: pointer;" onclick="OpenVoidPopupEdit('edit');">Edit Check</asp:HyperLink>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnReprintRange" runat="server" OnClientClick="OpenReprintCheckRangeModal();return false">
                                                     Reprint Check 
                                                </asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnchecknobill" runat="server" OnClick="btnchecknobill_Click">
                                                     Quick Check
                                                </asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkProcess" ForeColor="Red" runat="server" OnClientClick="return CheckProcess();" OnClick="lnkProcess_Click" > Process</asp:LinkButton>
                                                </div>
                                                <div class="rght-content">
                                                    <div class="btnclosewrap">
                                                        <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                                    </div>
                                                </div>
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
            <ul class="tabs tab-demo-active white"  id="tabProject" runat="server" style="width: 100%; display: contents;">
                        <li class="tab col s2" id="liVendorCheckhead" runat="server">
                            <a class="white-text waves-effect waves-light active" id="liVendorCheck" href="#activeone" runat="server" ><i class="mdi-action-verified-user"></i>&nbsp;Vendor Check</a>
                        </li>
                        <li class="tab col s2" id="liPayCheckhead" runat="server">
                            <a class="white-text waves-effect waves-light" id="liPayCheck" href="#two" runat="server" ><i class="mdi-action-group-work"></i>&nbsp;Pay Check</a>
                        </li>

             </ul>

            <div id="activeone" class="col s12 tab-container-border lighten-4" style="display: block;">
                        <div class="row">
            <div class="srchpane">
                <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth ser-css2" >
                        Date
                    </div>
                    <div class="srchinputwrap">
                        <%--<label for="txtFromDate">From</label>--%>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="datepicker_mom w-100" MaxLength="28" ></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <%--<label for="txtToDate">To</label>--%>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="datepicker_mom w-100" MaxLength="28" ></asp:TextBox>
                    </div>
                    <div class="srchinputwrap tabcontainer">
                        <ul class="tabselect accrd-tabselect" id="testradiobutton">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDate" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_date('dec','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDay" runat="server">
                                    <input type="radio" id="rdDay" name="rdCal" value="rdDay" onclick="SelectDate('Day', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#lblDay', 'hdnChecksDate', 'rdCal')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeek" runat="server">
                                    <input type="radio" id="rdWeek" name="rdCal" value="rdWeek" onclick="SelectDate('Week', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblWeek', 'hdnChecksDate', 'rdCal')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonth" runat="server">
                                    <input type="radio" id="rdMonth" name="rdCal" value="rdMonth" onclick="SelectDate('Month', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblMonth', 'hdnChecksDate', 'rdCal')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarter" runat="server">
                                    <input type="radio" id="rdQuarter" name="rdCal" value="rdQuarter" onclick="SelectDate('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblQuarter', 'hdnChecksDate', 'rdCal')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYear" runat="server">
                                    <input type="radio" id="rdYear" name="rdCal" value="rdYear" onclick="SelectDate('Year', 'ctl00_ContentPlaceHolder1_txtFromDate', 'ctl00_ContentPlaceHolder1_txtToDate', '#ctl00_ContentPlaceHolder1_lblYear', 'hdnChecksDate', 'rdCal')" />
                                    Year
                                </label>
                            </li>
                            <li>
                                <asp:LinkButton ID="incDate" runat="server" OnClientClick="dec_date('inc','ctl00_ContentPlaceHolder1_txtToDate','ctl00_ContentPlaceHolder1_txtFromDate','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                            </li>
                        </ul>
                    </div>
                     <div class="srchinputwrap rdleftmgn">
                        <div class="rdpairing" style="padding-top:5px">
                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="udpRdo">
                                <ContentTemplate>
                                    <div class="rd-flt">
                                        <asp:RadioButton ID="rdocheck" CssClass="with-gap rdoJournal" runat="server" Text=" Checks"  GroupName="JE" AutoPostBack="true" OnCheckedChanged="rdocheck_CheckedChanged" />
                                    </div>
                                    <div class="rd-flt">
                                        <asp:RadioButton ID="rdoRecurring" CssClass="with-gap" runat="server" Text=" Recurring Checks" GroupName="JE" AutoPostBack="true" OnCheckedChanged="rdoRecurring_CheckedChanged" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                    <div class="col lblsz2 lblszfloat">
                        <div class="row">
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <%--<asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                                        <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                                    <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </span>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="upPannelSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="srchpaneinner">
                            <div class="srchtitle srchtitlecustomwidth ser-css2" >
                                Search
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlSearch" runat="server" CssClass="browser-default selectst selectsml" AutoPostBack="True" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default selectst" Visible="false">
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="srchcstm"></asp:TextBox>
                            </div>
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlPaytype" CssClass="browser-default selectst" runat="server" Visible="false">
                                </asp:DropDownList>
                            </div>
                            <div class="srchinputwrap btnlinksicon srchclr">
                                <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click"><i class="mdi-action-search"></i>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
                            </div>
            <div class="grid_container">
                <div class="form-section-row m-b-0" >
                    <telerik:RadAjaxManager ID="RadAjaxManager_Checks" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkDeleteCheck">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Checks" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                    
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkClear">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Checks" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Checks" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Checks" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDate" />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDate" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="rdocheck">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Checks" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkProcess" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkVoidCheck" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkPrint" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkEditCheckNum" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="btnchecknobill" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="btnReprintRange" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="rdoRecurring">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Checks" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkProcess" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                     <telerik:AjaxUpdatedControl ControlID="lnkVoidCheck" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkPrint" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lnkEditCheckNum" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="btnReprintRange" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="btnchecknobill" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkProcess">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Checks" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="lnkClearPayCheck">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayChecks" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDatePayCheck" />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDatePayCheck" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCountPayCheck" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearchPayCheck">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayChecks" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCountPayCheck" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkShowAllPayCheck">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_PayChecks" LoadingPanelID="RadAjaxLoadingPanel_Checks" />
                                    <telerik:AjaxUpdatedControl ControlID="txtFromDatePayCheck" />
                                    <telerik:AjaxUpdatedControl ControlID="txtToDatePayCheck" />
                                    <telerik:AjaxUpdatedControl ControlID="lblRecordCountPayCheck" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Checks" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div class="RadGrid RadGrid_Material">
                        <%--<telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_Checks.ClientID %>");
                                    var columns = grid.get_masterTableView().get_columns();
                                    for (var i = 0; i < columns.length; i++) {
                                        columns[i].resizeToFit(false, true);
                                    }
                                }
                                var requestInitiator = null;
                                var selectionStart = null;

                                function requestStart(sender, args) {
                                    requestInitiator = document.activeElement.id;
                                    if (document.activeElement.tagName == "INPUT") {
                                        selectionStart = document.activeElement.selectionStart;
                                    }
                                }

                                function responseEnd(sender, args) {
                                    var element = document.getElementById(requestInitiator);
                                    if (element && element.tagName == "INPUT") {
                                        element.focus();
                                        element.selectionStart = selectionStart;
                                    }
                                }
                            </script>
                        </telerik:RadCodeBlock>--%>
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Checks" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Checks">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Checks" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                AllowCustomPaging="True" OnNeedDataSource="RadGrid_Checks_NeedDataSource" 
                                OnExcelMLExportRowCreated="RadGrid_Checks_ExcelMLExportRowCreated" 
                                OnExcelMLExportStylesCreated="RadGrid_Checks_ExcelMLExportStylesCreated"
                                OnPreRender="RadGrid_Checks_PreRender" 
                                OnItemEvent="RadGrid_Checks_ItemEvent" 
                                OnItemCreated="RadGrid_Checks_ItemCreated"
                                >
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>

                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false" UseAllDataFields="true">
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridTemplateColumn UniqueName="lblIndexID" Display="false" DataField="ID" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndexID" Text='<%# Eval("ID") %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnID" Value='<%# Bind("ID") %>' runat="server" />
                                                <asp:HiddenField ID="hdnRef" Value='<%# Eval("Ref") %>' runat="server" />
                                                <asp:HiddenField ID="hdnTransID" Value='<%# Bind("TransID") %>' runat="server" />
                                                <asp:HiddenField ID="hdnBatch" Value='<%# Bind("Batch") %>' runat="server" />
                                                <asp:HiddenField ID="hdnSel" Value='<%# Eval("Sel") %>' runat="server" />
                                                <asp:HiddenField ID="hdnBankID" runat="server" Value='<%# Eval("Bank") %>' />
                                                <asp:Label ID="lblBatch" Text='<%# Bind("Batch") %>' runat="server" />
                                                <asp:Label ID="lblIndex" Text='<%# Bind("ID") %>' runat="server" />
                                                <asp:Label ID="lblSel" Text='<%# Eval("Sel") %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="fDate" DataType="System.String" UniqueName="fDate"  SortExpression="fDate" AutoPostBackOnFilter="true"
                                            CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfDate" Text='<%# Eval("fDate")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))):""%> ' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Ref" UniqueName="Ref" SortExpression="Ref" AutoPostBackOnFilter="true" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Ref #" ShowFilterIcon="false" HeaderStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lnkRef" runat="server" Text='<%# Bind("Ref") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="fDesc" HeaderText="Vendor" SortExpression="fDesc" UniqueName="fDesc"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="BankName" HeaderText="Bank" SortExpression="BankName" UniqueName="BankName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="TypeName" HeaderText="Payment Type" SortExpression="TypeName" UniqueName="TypeName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" HeaderStyle-Width="100">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="StatusName" HeaderText="Status" HeaderStyle-Width="100" SortExpression="StatusName" UniqueName="StatusName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false" >
                                        </telerik:GridBoundColumn>

                                        <telerik:GridBoundColumn DataField="Company" HeaderText="Company" HeaderStyle-Width="100" SortExpression="Company" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" UniqueName="Company"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn DataField="Amount" UniqueName="Amount"  FooterAggregateFormatString="{0:c}"  Aggregate="Sum" SortExpression="QuotedPrice" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Amount" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>' runat="server"
                                                    ForeColor='<%# Convert.ToDouble(Eval("amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' />

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <FilterMenu CssClass="RadFilterMenu_CheckList">
                                </FilterMenu>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
                            
                </div>
             <div id="two" class="col s12 tab-container-border lighten-4" style="display: block;">
                        <div class="row">
            <div class="srchpane">
                 <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth ser-css2">
                        <asp:Label ID="lblStart" runat="server" Text="Date"></asp:Label>
                    </div>
                    <div class="srchinputwrap">
                        <%--<label>From</label>--%>
                        <asp:TextBox ID="txtFromDatePayCheck" runat="server" CssClass="datepicker_mom" Width="100px"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <%--<label>To</label>--%>
                        <asp:TextBox ID="txtToDatePayCheck" runat="server" CssClass="datepicker_mom" Width="100px"></asp:TextBox>
                    </div>
                    <div class="srchinputwrap">
                        <ul class="tabselect accrd-tabselect" id="testradiobuttonPayCheck">
                            <li>
                                <asp:LinkButton AutoPostBack="False" ID="decDatePayCheck" runat="server" Text="<i class='mdi-hardware-keyboard-arrow-left'></i>" OnClientClick="dec_datepaycheck('dec','ctl00_ContentPlaceHolder1_txtToDatePayCheck','ctl00_ContentPlaceHolder1_txtFromDatePayCheck','rdCal');return false;"></asp:LinkButton>
                            </li>
                            <li>
                                <label id="lblDayPayCheck" runat="server">
                                    <input type="radio" id="rdDayPayCheck" name="rdCal" value="rdDay" onclick="SelectDatepaycheck('Day', 'ctl00_ContentPlaceHolder1_txtFromDatePayCheck', 'ctl00_ContentPlaceHolder1_txtToDatePayCheck', '#lblDayPayCheck', 'hdnChecksDate', 'rdCal')" />
                                    Day
                                </label>
                            </li>
                            <li>
                                <label id="lblWeekPayCheck" runat="server">
                                    <input type="radio" id="rdWeekPayCheck" name="rdCal" value="rdWeek" onclick="SelectDatepaycheck('Week', 'ctl00_ContentPlaceHolder1_txtFromDatePayCheck', 'ctl00_ContentPlaceHolder1_txtToDatePayCheck', '#ctl00_ContentPlaceHolder1_lblWeekPayCheck', 'hdnChecksDate', 'rdCal')" />
                                    Week
                                </label>
                            </li>
                            <li>
                                <label id="lblMonthPayCheck" runat="server">
                                    <input type="radio" id="rdMonthPayCheck" name="rdCal" value="rdMonth" onclick="SelectDatepaycheck('Month', 'ctl00_ContentPlaceHolder1_txtFromDatePayCheck', 'ctl00_ContentPlaceHolder1_txtToDatePayCheck', '#ctl00_ContentPlaceHolder1_lblMonthPayCheck', 'hdnChecksDate', 'rdCal')" />
                                    Month
                                </label>
                            </li>
                            <li>
                                <label id="lblQuarterPayCheck" runat="server">
                                    <input type="radio" id="rdQuarterPayCheck" name="rdCal" value="rdQuarter" onclick="SelectDatepaycheck('Quarter', 'ctl00_ContentPlaceHolder1_txtFromDatePayCheck', 'ctl00_ContentPlaceHolder1_txtToDatePayCheck', '#ctl00_ContentPlaceHolder1_lblQuarterPayCheck', 'hdnChecksDate', 'rdCal')" />
                                    Quarter
                                </label>
                            </li>
                            <li>
                                <label id="lblYearPayCheck" runat="server">
                                    <input type="radio" id="rdYearPayCheck" name="rdCal" value="rdYear" onclick="SelectDatepaycheck('Year', 'ctl00_ContentPlaceHolder1_txtFromDatePayCheck', 'ctl00_ContentPlaceHolder1_txtToDatePayCheck', '#ctl00_ContentPlaceHolder1_lblYearPayCheck', 'hdnChecksDate', 'rdCal')" />
                                    Year
                                </label>
                            </li>
                            <li>
                                <asp:LinkButton ID="incDatePayCheck" runat="server" OnClientClick="dec_datepaycheck('inc','ctl00_ContentPlaceHolder1_txtToDatePayCheck','ctl00_ContentPlaceHolder1_txtFromDatePayCheck','rdCal');return false" Text="<i class='mdi-hardware-keyboard-arrow-right'></i>"></asp:LinkButton>
                            </li>
                        </ul>
                    </div>
                    
                    <div class="col lblsz2 lblszfloat">
                        <div class="row">
                            <span class="tro trost">
                                <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkChk_CheckedChanged" AutoPostBack="True" Text="Incl. Closed" style="display:none;"></asp:CheckBox>
                                <%--<asp:Label ID="lblChkSelect" runat="server">Incl. Closed</asp:Label>--%>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkShowAllPayCheck" runat="server" OnClick="lnkShowAllPayCheck_Click">Show All </asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClearPayCheck" runat="server" OnClick="lnkClearPayCheck_Click">Clear</asp:LinkButton>
                            </span>
                             <span class="tro trost" style="display:none;">
                                <i class="mdi-social-notifications"></i>
                            </span>
                            <span class="tro trost">
                                <asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblRecordCountPayCheck" runat="server">0 Record(s) found.</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </span>
                        </div>
                    </div>
                </div>
               <%-- <asp:UpdatePanel ID="upPannelSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <div class="srchpaneinner">
                            <div class="srchtitle srchtitlecustomwidth ser-css2" >
                                Employee
                            </div>
                          <%--  <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlSearch" runat="server" CssClass="browser-default selectst selectsml" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>--%>

                            
                            <div class="srchinputwrap">
                                <asp:DropDownList ID="ddlEmp" runat="server" CssClass="browser-default selectst" >
                                    
                                </asp:DropDownList>
                            </div>
                            
                            <div class="srchinputwrap">
                                <div class="btnlinksicon">
                                    <asp:LinkButton ID="lnkSearchPayCheck" runat="server" OnClick="lnkSearchPayCheck_Click"><i class="mdi-action-search"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
                
            </div>
                            </div>
            <div class="grid_container">
                <div class="form-section-row m-b-0" style="margin-bottom: 0 !important;">
                    
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_PayChecks" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div class="RadGrid RadGrid_Material">
                        <%--<telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_Checks.ClientID %>");
                                    var columns = grid.get_masterTableView().get_columns();
                                    for (var i = 0; i < columns.length; i++) {
                                        columns[i].resizeToFit(false, true);
                                    }
                                }
                                var requestInitiator = null;
                                var selectionStart = null;

                                function requestStart(sender, args) {
                                    requestInitiator = document.activeElement.id;
                                    if (document.activeElement.tagName == "INPUT") {
                                        selectionStart = document.activeElement.selectionStart;
                                    }
                                }

                                function responseEnd(sender, args) {
                                    var element = document.getElementById(requestInitiator);
                                    if (element && element.tagName == "INPUT") {
                                        element.focus();
                                        element.selectionStart = selectionStart;
                                    }
                                }
                            </script>
                        </telerik:RadCodeBlock>--%>
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_PayCheck" runat="server" LoadingPanelID="RadAjaxLoadingPanel_PayChecks">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_PayChecks" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                AllowCustomPaging="True" OnNeedDataSource="RadGrid_PayChecks_NeedDataSource" OnPreRender="RadGrid_PayChecks_PreRender" OnItemEvent="RadGrid_PayChecks_ItemEvent" 
                                OnItemCreated="RadGrid_PayChecks_ItemCreated" OnExcelMLExportRowCreated="RadGrid_PayChecks_ExcelMLExportRowCreated" 
                                OnExcelMLExportStylesCreated="RadGrid_PayChecks_ExcelMLExportStylesCreated">
                               
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>

                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                    <Columns>
                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                        </telerik:GridClientSelectColumn>
                                        <telerik:GridTemplateColumn UniqueName="lblIndexIDPay" Display="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <%# Container.ItemIndex %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnIDPay" Value='<%# Eval("ID") %>' runat="server" />
                                                <asp:HiddenField ID="hdnSelPay" Value='<%# Eval("Sel") %>' runat="server" />
                                                <asp:HiddenField ID="hdnStatusPay" Value='<%# Eval("Status") %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="fdate" DataField="fdate" SortExpression="fdate" AutoPostBackOnFilter="true" DataType="System.String"
                                            CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfDatePay" Text='<%# Eval("fDate")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))):""%> ' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Ref" DataField="Ref" HeaderText="Ref" SortExpression="Ref"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                            ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="lblRefPay" runat="server" Text='<%# Bind("Ref") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn UniqueName="Name" DataField="Name" HeaderText="Payee" SortExpression="Name"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn UniqueName="TInc" DataField="TInc" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="TInc" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="Gross" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTInc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TInc", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("TInc"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="FIT" DataField="FIT" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="FICA" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="FIT" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFIT" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FIT", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("FIT"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="FICA" DataField="FICA" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="FICA" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="FICA" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFica" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FICA", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("FICA"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="MEDI" DataField="MEDI" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="FICA" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="MEDI" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMedi" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MEDI", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("MEDI"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="SIT" DataField="SIT" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="SIT" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="SIT" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSIT" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "SIT", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("SIT"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TOther" DataField="TOther" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="TOther" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="Other" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTOther" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TOther", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("TOther"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="TDed" DataField="TDed" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="TDed" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="Total" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTDed" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TDed", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("TDed"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Net" DataField="Net" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="Net" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Decimal" HeaderText="Net" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNet" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Net", "{0:c}")%>' ForeColor='<%# Convert.ToDouble(Eval("Net"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>



                                        
                                    </Columns>
                                </MasterTableView>
                                <FilterMenu CssClass="RadFilterMenu_CheckList">
                                </FilterMenu>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
                            
                </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hdnChecksSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
       

        function OpenVoidPopupEdit(txtMsg) {
            
            var grid = $find("<%=RadGrid_Checks.ClientID %>");
            var MasterTable = grid.get_masterTableView();
            var selectedRows = MasterTable.get_selectedItems();
            var ID = "";
            for (var i = 0; i < selectedRows.length; i++) {
                var row = selectedRows[i];
                ID = MasterTable.getCellByColumnUniqueName(row, "lblIndexID").innerHTML;
                ID = $(ID).html();
            }
          
            if (ID != "") {
                $.ajax({
                    type: "POST",
                    //url: "ManageChecks.aspx/VoidCheckEdit",
                    url: "AccountAutoFill.asmx/VoidCheckEdit",
                    data: '{lblIndex: "' + ID + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        
                        var obj = JSON.parse(response.d);
                        //alert($.parseJSON(response.d.ID));
                        if (txtMsg == 'delete') {
                            validateOpenVoid(txtMsg, obj[0]["Sel"] , obj[0]["Ref"] ) 
                        }
                        else if (obj[0]["Sel"]  > 0) { validateOpenVoid(txtMsg, obj[0]["Sel"] , obj[0]["Ref"] ) }
                        else {
                            
                            var wnd = $find('<%=VoidCheckWindow.ClientID %>');
                            $("#<%=hdnCDID.ClientID%>").val(obj[0]["ID"] );
                            
                            if (txtMsg == 'void') {
                                $("#<%=dvVoid.ClientID%>").show();
                                $("#<%=lnkSave.ClientID%>").show();
                                $("#<%=lbtnCheckSave.ClientID%>").hide();
                                $("#<%=dvEditCheck.ClientID%>").hide();
                                $("#<%=txtVoidDate.ClientID%>").val(obj[0]["VoidDate"] );
                                wnd.set_title("Void Check");
                            }
                            else {
                                $("#<%=dvVoid.ClientID%>").hide();
                                $("#<%=dvEditCheck.ClientID%>").show();
                                $("#<%=lnkSave.ClientID%>").hide();
                                $("#<%=lbtnCheckSave.ClientID%>").show();
                                $("#<%=hdnCD.ClientID%>").val(obj[0]["ID"] );
                                $("#<%=hdnBankID1.ClientID%>").val(obj[0]["Bank"] );
                                $("#<%=txtCheckNo.ClientID%>").val(obj[0]["Ref"] );
                                wnd.set_title("Edit Check #");
                            }
                            window.radopen(null, "VoidCheckWindow");
                            Materialize.updateTextFields();
                        }
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            }
            else {
                selectCheckWarning(txtMsg);
            }
        }
        function CheckProcess() {
            var result = false;
            $("#<%=RadGrid_Checks.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                return confirm('Are you sure you want to process this adjustment?');
            }
            else {
                alert('Please select a Recurring entry to process.')
                return false;
            }
        }
        function isNumberKey(evt, txt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function dtaa() {
            this.checkno = null;
            this.bank = null;
            this.cdId = null;
        }
        function IsExistCheckNo() {

            var valCheckNo = $("#<%=txtCheckNo.ClientID%>").val();
            var valBank = $("#<%=hdnBankID1.ClientID%>").val();
            var valCD = $("#<%=hdnCD.ClientID%>").val();
            var dtaaa = new dtaa();
            dtaaa.checkno = valCheckNo;
            dtaaa.bank = valBank;
            dtaaa.cdId = valCD;
            debugger;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "AccountAutoFill.asmx/CheckNumValidOnEdit",
                data: JSON.stringify(dtaaa),
                dataType: "json",
                async: true,
                success: function (data) {
                    debugger;
                    if (data.d == true) {
                        noty({
                            text: 'Check #' + valCheckNo + ' is already in exists in bank account. Since duplicate check numbers are not allowed, the check generation process cannot continue.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 15000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                        $("#<%=txtCheckNo.ClientID%>").val('');
                    }
                },
                failure: function (response) {
                    alert(response);
                },
                error: function (result) {
                    alert("Due to unexpected errors we were unable to load availability");
                }
            });
        }
        function validateOpenVoid(txtMsg, sel, ref) {
            var result = false;

            $("#<%=RadGrid_Checks.ClientID %>").find('tr:not(:first,:last)').each(function () {
                debugger;
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                    result = true;
                });
            });
            if (sel == "1") {
                noty({
                    text: 'This check has cleared the bank and can therefore not be ' + txtMsg + '.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            }
            else if (sel == "2") {
                noty({
                    text: 'This check is not open and can therefore not be ' + txtMsg + '.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            }
            else {
                if (result == true && txtMsg == 'edit') {
                    return true;
                }
                else if (result == true && txtMsg != 'edit') {
                      result =confirm('Are you sure that you want to ' + txtMsg + ' check # ' + ref + '?');
                    return result;
                }
                else {
                    noty({
                        text: 'Please select an check to ' + txtMsg + ' check.',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 5000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                    return false;
                }
            }
        }
        function selectCheckWarning(txtMsg) {
            noty({
                text: 'Please select an check to ' + txtMsg + ' check.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
            return false;
        }
        function closedCheckWarning() {
            noty({
                text: 'This check is not open and can therefore not be deleted.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function CssClearLabel() {
            $('#<%=lblDay.ClientID%>').removeClass("labelactive");
            $('#<%=lblWeek.ClientID%>').removeClass("labelactive");
            $('#<%=lblMonth.ClientID%>').removeClass("labelactive");
            $("#<%=lblQuarter.ClientID%>").removeClass("labelactive");
            $('#<%=lblYear.ClientID%>').removeClass("labelactive");
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('label input[type=radio]').click(function () {
                $('input[name="' + this.name + '"]').each(function () {
                    $(this.parentNode).toggleClass('labelactive', this.checked);
                });
            });
            if (typeof (Storage) !== "undefined") {

                // Retrieve
                var SesVar = '<%= Convert.ToString(Session["lblChecksActive"])%>';
                var val;
                val = localStorage.getItem("hdnChecksDate");
                if (SesVar == '2') {
                    $("#<%=lblDay.ClientID%>").addClass("");
                    $("#<%=lblWeek.ClientID%>").addClass("");
                    $("#<%=lblMonth.ClientID%>").addClass("");
                    $("#<%=lblQuarter.ClientID%>").addClass("");
                    $("#<%=lblYear.ClientID%>").addClass("");
                }
                else {
                    if (val == 'Day') {
                        $("#<%=lblDay.ClientID%>").addClass("labelactive");
                        document.getElementById("rdDay").checked = true;
                    }
                    else if (val == 'Week') {

                        $("#<%=lblWeek.ClientID%>").addClass("labelactive");
                        document.getElementById("rdWeek").checked = true;
                    }
                    else if (val == 'Month') {

                        $("#<%=lblMonth.ClientID%>").addClass("labelactive");
                        document.getElementById("rdMonth").checked = true;
                    }
                    else if (val == 'Quarter') {

                        $("#<%=lblQuarter.ClientID%>").addClass("labelactive");
                        document.getElementById("rdQuarter").checked = true;
                    }
                    else if (val == 'Year') {

                        $("#<%=lblYear.ClientID%>").addClass("labelactive");
                        document.getElementById("rdYear").checked = true;
                    }
                    else {
                        $("#<%=lblMonth.ClientID%>").addClass("labelactive");
                        document.getElementById("rdMonth").checked = true;
                    }
                }
            }
        });
    </script>
    <script type="text/javascript">
        function dec_date(select, txtDateTo, txtDateFrom, rdGroup) {
            var select = select;
            var txtDateTo = txtDateTo;
            var txtDateFrom = txtDateFrom;
            var rdGroup = rdGroup;
            var xday;
            var xWeek;
            var xMonth;
            var xYear;
            var xQuarter;
            if (select == "dec") {
                xday = -1;
                xWeek = -7;
                xMonth = -1;
                xQuarter = -3;
                xYear = -1;
            }
            if (select == "inc") {

                xday = 1;
                xWeek = 7;
                xMonth = 1;
                xQuarter = 3;
                xYear = 1;
            }
            var radio = document.getElementsByName(rdGroup); //Client ID of the RadioButtonList1 
            var selected;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) { // Checked property to check radio Button check or not
                    //alert("Radio button having value " + radio[i].value + " was checked."); // Show the checked value
                    selected = radio[i].value;

                }
                if (selected == "") {
                    selected = 'rdWeek';
                }
            }
            if (selected == 'rdDay') {

                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xday);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xday);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdWeek') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xWeek);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xWeek);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdMonth') {
                //dec the from date

                Date.isLeapYear = function (year) {
                    return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
                };

                Date.getDaysInMonth = function (year, month) {
                    return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
                };

                Date.prototype.isLeapYear = function () {
                    return Date.isLeapYear(this.getFullYear());
                };

                Date.prototype.getDaysInMonth = function () {
                    return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
                };

                Date.prototype.addMonths = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + value);
                    this.setDate(Math.min(n, this.getDaysInMonth()));
                    return this;
                };

                Date.prototype.addMonthsLast = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + 1);
                    if (this.getDaysInMonth() == 31) {

                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));

                    }

                    return this;
                };

                Date.prototype.addMonthsLastDec = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() - 1);
                    if (this.getDaysInMonth() == 31) {

                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));

                    }

                    return this;
                };
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt).toDateString();
                var newdate = new Date(date);

                newdate.addMonths(xMonth);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;


                //dec the to date 
                if (select == 'dec') {
                    var ti = document.getElementById(txtDateTo).value;
                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLastDec(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateTo).value = someFormattedDate;
                }

                else {
                    var ti = document.getElementById(txtDateTo).value;

                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLast(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateTo).value = someFormattedDate;
                }
            }


            else if (selected == 'rdQuarter') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setMonth(newdate.getMonth() + xQuarter);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                //decrease date range 
                if (select == 'dec') {
                    xQuarter = -3;

                    if (DATE.getMonth() == 11) {
                        NEWDATE.setDate(NEWDATE.getDate() - 1);
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);

                    }
                    else if (DATE.getMonth() == 5) {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                        NEWDATE.setDate(NEWDATE.getDate() + 1);

                    }
                    else {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);

                    }

                }
                else {
                    xQuarter = 3;
                    NEWDATE.setDate(NEWDATE.getDate() - 1);
                    NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                    if (NEWDATE.getMonth() == 11 || NEWDATE.getMonth() == 12 || DATE.getMonth() == 11) {
                        NEWDATE.setDate(31);
                    } else {
                        if (DATE.getMonth() == 5) { NEWDATE.setDate(NEWDATE.getDate() + 1); }
                        else { NEWDATE.setDate(NEWDATE.getDate()); }

                    }
                }
                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdYear') {

                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setFullYear(newdate.getFullYear() + xYear);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setFullYear(NEWDATE.getFullYear() + xYear);
                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }

            return false;

        }
        function SelectDate(type, txtDateFrom, txtdateTo, label, UniqueVal, rdGroup) {
            var type = type;
            var txtDateFrom = txtDateFrom;
            var txtdateTo = txtdateTo;
            var UniqueVal = UniqueVal;
            var label = label;
            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = datestring;
                document.getElementById(txtDateFrom).value = datestring;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnChecksSelectDtRange.ClientID%>').value = "Day";
            }
            if (type == 'Week') {

                Date.prototype.GetFirstDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay())));
                }

                Date.prototype.GetLastDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay() + 6)));
                }
                var today = new Date();
                var Firstdate = today.GetFirstDayOfWeek();
                var day = Firstdate.getDate();
                var month = Firstdate.getMonth() + 1;
                var year = Firstdate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var Lastdate = today.GetLastDayOfWeek();
                var day = Lastdate.getDate();
                var month = Lastdate.getMonth() + 1;
                var year = Lastdate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnChecksSelectDtRange.ClientID%>').value = "Week";
            }
            if (type == 'Month') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfMonth = new Date(y, m, 1);
                var lastDayOfMonth = new Date(y, m + 1, 0);
                var day = FirstDayOfMonth.getDate();
                var month = FirstDayOfMonth.getMonth() + 1;
                var year = FirstDayOfMonth.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDayOfMonth.getDate();
                var month = lastDayOfMonth.getMonth() + 1;
                var year = lastDayOfMonth.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnChecksSelectDtRange.ClientID%>').value = "Month";
            }
            if (type == 'Quarter') {
                var d = new Date();
                var quarter = Math.floor((d.getMonth() / 3));
                var firstDate = new Date(d.getFullYear(), quarter * 3, 1);
                var lastDate = new Date(firstDate.getFullYear(), firstDate.getMonth() + 3, 0);
                var day = firstDate.getDate();
                var month = firstDate.getMonth() + 1;
                var year = firstDate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDate.getDate();
                var month = lastDate.getMonth() + 1;
                var year = lastDate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnChecksSelectDtRange.ClientID%>').value = "Quarter";
            }
            if (type == 'Year') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfYear = new Date(y, 1, 1);
                var lastDayOfYear = new Date(y, 11, 31);
                var day = FirstDayOfYear.getDate();
                var month = FirstDayOfYear.getMonth();
                var year = FirstDayOfYear.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDayOfYear.getDate();
                var month = lastDayOfYear.getMonth() + 1;
                var year = lastDayOfYear.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnChecksSelectDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnChecksSelectDtRange.ClientID%>').value);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }

        function dec_datepaycheck(select, txtDateTo, txtDateFrom, rdGroup) {
            var select = select;
            var txtDateTo = txtDateTo;
            var txtDateFrom = txtDateFrom;
            var rdGroup = rdGroup;
            var xday;
            var xWeek;
            var xMonth;
            var xYear;
            var xQuarter;
            if (select == "dec") {
                xday = -1;
                xWeek = -7;
                xMonth = -1;
                xQuarter = -3;
                xYear = -1;
            }
            if (select == "inc") {

                xday = 1;
                xWeek = 7;
                xMonth = 1;
                xQuarter = 3;
                xYear = 1;
            }
            var radio = document.getElementsByName(rdGroup); //Client ID of the RadioButtonList1 
            var selected;
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) { // Checked property to check radio Button check or not
                    //alert("Radio button having value " + radio[i].value + " was checked."); // Show the checked value
                    selected = radio[i].value;

                }
                if (selected == "") {
                    selected = 'rdWeek';
                }
            }
            if (selected == 'rdDay') {

                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xday);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xday);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdWeek') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setDate(newdate.getDate() + xWeek);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setDate(NEWDATE.getDate() + xWeek);

                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdMonth') {
                //dec the from date

                Date.isLeapYear = function (year) {
                    return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
                };

                Date.getDaysInMonth = function (year, month) {
                    return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
                };

                Date.prototype.isLeapYear = function () {
                    return Date.isLeapYear(this.getFullYear());
                };

                Date.prototype.getDaysInMonth = function () {
                    return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
                };

                Date.prototype.addMonths = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + value);
                    this.setDate(Math.min(n, this.getDaysInMonth()));
                    return this;
                };

                Date.prototype.addMonthsLast = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() + 1);
                    if (this.getDaysInMonth() == 31) {

                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));

                    }

                    return this;
                };

                Date.prototype.addMonthsLastDec = function (value) {

                    var n = this.getDate();
                    this.setDate(1);
                    this.setMonth(this.getMonth() - 1);
                    if (this.getDaysInMonth() == 31) {

                        this.setDate(Math.max(n, this.getDaysInMonth()));
                    }
                    else {
                        this.setDate(Math.min(n, this.getDaysInMonth()));

                    }

                    return this;
                };
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt).toDateString();
                var newdate = new Date(date);

                newdate.addMonths(xMonth);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;


                //dec the to date 
                if (select == 'dec') {
                    var ti = document.getElementById(txtDateTo).value;
                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLastDec(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateTo).value = someFormattedDate;
                }

                else {
                    var ti = document.getElementById(txtDateTo).value;

                    var date = new Date(ti).toDateString();
                    var newdateti = new Date(date);
                    newdateti.addMonthsLast(xMonth);

                    var dd = newdateti.getDate();
                    var mm = newdateti.getMonth() + 1;
                    var y = newdateti.getFullYear();

                    var someFormattedDate = mm + '/' + dd + '/' + y;
                    document.getElementById(txtDateTo).value = someFormattedDate;
                }
            }


            else if (selected == 'rdQuarter') {
                //dec the from date 
                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setMonth(newdate.getMonth() + xQuarter);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                //decrease date range 
                if (select == 'dec') {
                    xQuarter = -3;

                    if (DATE.getMonth() == 11) {
                        NEWDATE.setDate(NEWDATE.getDate() - 1);
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);

                    }
                    else if (DATE.getMonth() == 5) {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                        NEWDATE.setDate(NEWDATE.getDate() + 1);

                    }
                    else {
                        NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);

                    }

                }
                else {
                    xQuarter = 3;
                    NEWDATE.setDate(NEWDATE.getDate() - 1);
                    NEWDATE.setMonth(NEWDATE.getMonth() + xQuarter);
                    if (NEWDATE.getMonth() == 11 || NEWDATE.getMonth() == 12 || DATE.getMonth() == 11) {
                        NEWDATE.setDate(31);
                    } else {
                        if (DATE.getMonth() == 5) { NEWDATE.setDate(NEWDATE.getDate() + 1); }
                        else { NEWDATE.setDate(NEWDATE.getDate()); }

                    }
                }
                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }
            else if (selected == 'rdYear') {

                var tt = document.getElementById(txtDateFrom).value;

                var date = new Date(tt);
                var newdate = new Date(date);

                newdate.setFullYear(newdate.getFullYear() + xYear);

                var dd = newdate.getDate();
                var mm = newdate.getMonth() + 1;
                var y = newdate.getFullYear();

                var someFormattedDate = mm + '/' + dd + '/' + y;
                document.getElementById(txtDateFrom).value = someFormattedDate;
                //dec the to date 

                var TT = document.getElementById(txtDateTo).value;

                var DATE = new Date(TT);
                var NEWDATE = new Date(DATE);

                NEWDATE.setFullYear(NEWDATE.getFullYear() + xYear);
                var DD = NEWDATE.getDate();
                var MM = NEWDATE.getMonth() + 1;
                var Y = NEWDATE.getFullYear();

                var someFormattedDATE = MM + '/' + DD + '/' + Y;
                document.getElementById(txtDateTo).value = someFormattedDATE;
            }

            return false;

        }
        function SelectDatepaycheck(type, txtDateFrom, txtdateTo, label, UniqueVal, rdGroup) {
            var type = type;
            var txtDateFrom = txtDateFrom;
            var txtdateTo = txtdateTo;
            var UniqueVal = UniqueVal;
            var label = label;
            if (type == 'Day') {
                var todaydate = new Date();
                var day = todaydate.getDate();
                var month = todaydate.getMonth() + 1;
                var year = todaydate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = datestring;
                document.getElementById(txtDateFrom).value = datestring;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnChecksSelectDtRange.ClientID%>').value = "Day";
            }
            if (type == 'Week') {

                Date.prototype.GetFirstDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay())));
                }

                Date.prototype.GetLastDayOfWeek = function () {
                    return (new Date(this.setDate(this.getDate() - this.getDay() + 6)));
                }
                var today = new Date();
                var Firstdate = today.GetFirstDayOfWeek();
                var day = Firstdate.getDate();
                var month = Firstdate.getMonth() + 1;
                var year = Firstdate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var Lastdate = today.GetLastDayOfWeek();
                var day = Lastdate.getDate();
                var month = Lastdate.getMonth() + 1;
                var year = Lastdate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnChecksSelectDtRange.ClientID%>').value = "Week";
            }
            if (type == 'Month') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfMonth = new Date(y, m, 1);
                var lastDayOfMonth = new Date(y, m + 1, 0);
                var day = FirstDayOfMonth.getDate();
                var month = FirstDayOfMonth.getMonth() + 1;
                var year = FirstDayOfMonth.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDayOfMonth.getDate();
                var month = lastDayOfMonth.getMonth() + 1;
                var year = lastDayOfMonth.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnChecksSelectDtRange.ClientID%>').value = "Month";
            }
            if (type == 'Quarter') {
                var d = new Date();
                var quarter = Math.floor((d.getMonth() / 3));
                var firstDate = new Date(d.getFullYear(), quarter * 3, 1);
                var lastDate = new Date(firstDate.getFullYear(), firstDate.getMonth() + 3, 0);
                var day = firstDate.getDate();
                var month = firstDate.getMonth() + 1;
                var year = firstDate.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDate.getDate();
                var month = lastDate.getMonth() + 1;
                var year = lastDate.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnChecksSelectDtRange.ClientID%>').value = "Quarter";
            }
            if (type == 'Year') {
                var date = new Date(), y = date.getFullYear(), m = date.getMonth();
                var FirstDayOfYear = new Date(y, 1, 1);
                var lastDayOfYear = new Date(y, 11, 31);
                var day = FirstDayOfYear.getDate();
                var month = FirstDayOfYear.getMonth();
                var year = FirstDayOfYear.getFullYear();
                var datestring = month + "/" + day + "/" + year;
                document.getElementById(txtDateFrom).value = datestring;
                var day = lastDayOfYear.getDate();
                var month = lastDayOfYear.getMonth() + 1;
                var year = lastDayOfYear.getFullYear();
                var dateString = month + "/" + day + "/" + year;
                document.getElementById(txtdateTo).value = dateString;
                $(label).addClass("labelactive");
                document.getElementById('<%= hdnChecksSelectDtRange.ClientID%>').value = "Year";
            }
            if (typeof (Storage) !== "undefined") {
                // Store
                localStorage.setItem(UniqueVal, document.getElementById('<%= hdnChecksSelectDtRange.ClientID%>').value);
            }
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "CssActive";
            var clickSearchButton = document.getElementById("<%= lnkSearch.ClientID %>");
            clickSearchButton.click();
            document.getElementById('<%= hdnCssActive.ClientID%>').value = "1";
        }
    </script>
    <script type="text/javascript">
         function OpenReprintCheckRangeModal() {
            <%--$('#<%=txtVendorType.ClientID%>').val("");
            $('#<%=txtremarksvendor.ClientID%>').val("");
            $('#<%=txtVendorType.ClientID%>').prop("readonly", false);--%>
            var wnd = $find('<%=ReprintCheckRange.ClientID %>');
            wnd.set_title("Reprint Check");
            wnd.Show();
        }
        function CloseReprintCheckRangeModal() {
            var wnd = $find('<%=ReprintCheckRange.ClientID %>');
            wnd.Close();
            
        }
        function OpentemplateModal() {
            debugger;
            var d1 = parseInt($('#<%=txtcheckfrom.ClientID%>').val());
            var d2 = parseInt($('#<%=txtcheckto.ClientID%>').val());

if (d2 < d1) {
    noty({
                text: 'Ending checkno is not less then starting checkno.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
    return;
}
            CloseReprintCheckRangeModal();
            var wnd = $find('<%=RadWindowTemplates.ClientID %>');
            //wnd.set_title("Re-Print Check Range");
            wnd.Show();
        }
        function ClosetemplateModal() {
            var wnd = $find('<%=RadWindowTemplates.ClientID %>');
            wnd.Close();
            //$('html, body').animate({ scrollTop: $('#vendorType').offset().top }, 'slow');
        }
        function CloseVoidModal() {
            var wnd = $find('<%=VoidCheckWindow.ClientID %>');
            wnd.Close();
        }
    </script>
</asp:Content>



