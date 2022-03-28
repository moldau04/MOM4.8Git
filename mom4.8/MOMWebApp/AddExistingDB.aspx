<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddExistingDB" Codebehind="~/AddExistingDB.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="height: 65px !important;">
        <div class="divbutton-container">
            <div id="divButtons">
                <div id="breadcrumbs-wrapper">

                    <header>
                        <div class="container row-color-grey">
                            <div class="row">
                                <div class="col s12 m12 l12">
                                    <div class="row">

                                        <div class="page-title"><i class="mdi-action-dns"></i>&nbsp;<asp:Label runat="server" ID="lblPageTitle" Text="Add Existing Database"></asp:Label></div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click">Save</asp:LinkButton>
                                        </div>

                                        <div class="btnclosewrap">
                                            <asp:LinkButton id="lnkClose" runat="server" tooltip="Close" causesvalidation="false" onclick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </header>
                </div>
            </div>
        </div>
    </div>
    <div class="container card cardnegate rounded" style="min-height:155px;">
                        <div class="row">
                            <div class="form-content-wrap">
                                <div class="form-content-pd">
                                    <div class="form-section-row">
                                        <div class="section-ttle">Existing Database Info</div>
                                        <div class="form-section3">

                                            <div class="input-field col s12">
                                                <div class="row">
                                                    <%--<input id="fName" type="text" class="validate">--%>                                                    
                                                    <asp:TextBox ID="txtCompany" runat="server" CssClass="validate" MaxLength="75" TabIndex="1"></asp:TextBox>
                                                    <label for="fName">Company Name</label>
                                                    <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server"
                                                        ControlToValidate="txtCompany" Display="None" ErrorMessage="Company Name is Required"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceCompanyName"
                                                        runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="rfvCompanyName">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                            </div>



                                        </div>
                                        <div class="form-section3-blank">
                                            &nbsp;
                                        </div>
                                        <div class="form-section3">

                                            <div class="input-field col s12">
                                                <div class="row">
                                                    <label class="drpdwn-label">Database Type</label>
                                                    <%--<select class="browser-default">
                                                        <option>Select</option>
                                                        <option>MSM</option>
                                                    </select>--%>
                                                    <asp:DropDownList ID="ddlDBType" runat="server" CssClass="browser-default" Width="100%"
                                                        TabIndex="2">
                                                        <asp:ListItem Value="MSM">MSM</asp:ListItem>
                                                        <asp:ListItem Value="TS">TS</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-section3-blank">
                                            &nbsp;
                                        </div>
                                        <div class="form-section3">
                                            <div class="input-field col s12">
                                                <div class="row">
                                                    <%--<input id="db" type="text" class="validate">--%>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDB"
                                                        Display="None" ErrorMessage="Database is required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceDatabaseName"
                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2" PopupPosition="BottomLeft">
                                                    </asp:ValidatorCalloutExtender>
                                                    <asp:TextBox ID="txtDB" runat="server" CssClass="validate" TabIndex="3"></asp:TextBox>
                                                    <label for="db">Database Name</label>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
    <asp:HiddenField runat="server" ID="hdnRcvPymtSelectDtRange" Value="" />
    <asp:HiddenField runat="server" ID="hdnCssActive" Value="1" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });
        });
    </script>
</asp:Content>
