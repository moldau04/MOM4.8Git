<%@ Page Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" CodeBehind="GPSsetting.aspx.cs" Inherits="MOMWebApp.GPSsetting" %>

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
          <div style="margin-top: 15px; font-size: 1.1em !important;">
                        <table>
                            <tr>
                                <td>GPS ping interval
                                </td>
                                <td>
                                    <div class="srchinputwrap">
                                    <asp:DropDownList ID="ddlGPSPing" runat="server" onchange="AlertPing(this)" class="browser-default selectst selectsml"
                                        ToolTip="This feature is for GPS tracking app. The interval selected will be the interval at which the GPS tracking app pings the GPS satellites to get the location. Maximum the time is minimum will be the phone battery consumption. Setting the interval to zero will cause huge amout of phone battery use. Recommended interval is 1 minute.">
                                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="30 Second" Value="30000"></asp:ListItem>
                                        <asp:ListItem Text="1 Minute" Value="60000"></asp:ListItem>
                                        <asp:ListItem Text="2 Minutes" Value="120000"></asp:ListItem>
                                        <asp:ListItem Text="3 Minutes" Value="180000"></asp:ListItem>
                                        <asp:ListItem Text="4 Minutes" Value="240000"></asp:ListItem>
                                        <asp:ListItem Text="5 Minutes" Value="300000"></asp:ListItem>
                                    </asp:DropDownList>
                                        </div>
                                </td>
                            </tr>
                            <tr style="background:#fff !important;">
                                <td></td>
                                <td>
                                    <asp:Label ID="lblMSgGPS" CssClass="lblMsg" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div style="clear: both;"></div>

                        <footer style="float: left; padding-left: 0 !important;">
                            <div class="btnlinks">
                                 <asp:LinkButton ID="lnkGPS" runat="server" OnClick="lnkGPS_Click">Save</asp:LinkButton>
                            </div>
                        </footer>
                    </div>
    </div> 
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">

    <script type="text/javascript">


          function opencGPSForm() {
            window.radopen(null, "RadGPSWindow");
        }


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

        <script type="text/javascript">
        function AlertPing(control) {
            var ddlGPSPing = document.getElementById(control.id);

            if (ddlGPSPing.value == 0) {
                alert('Setting the ping interval to zero will cause significant rise in the phone battery consumption. Recommended ping interval is 1 minute.');
            }
        }    

        function DeleteDatabase() {

        }
    </script>

</asp:Content>
