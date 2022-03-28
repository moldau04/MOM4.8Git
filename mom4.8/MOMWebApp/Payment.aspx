<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" Inherits="Payment" Codebehind="Payment.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
    
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">

        <div class="page-cont-top">
            <ul class="page-breadcrumb">
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
            </ul>
            <div class="page-bar-right">
                <asp:LinkButton ID="lnkBack" runat="server" CausesValidation="False"
                                    OnClick="lnkBack_Click">&lt; Back to Invoice</asp:LinkButton>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <asp:Label ID="lblErr" runat="server" Style="color: Red; font-weight: bold; font-size: larger;"></asp:Label>
                                <asp:Label ID="lblMSG" runat="server" Style="font-weight: bold; font-size: 18px;" ForeColor="Red"></asp:Label>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <asp:Panel runat="server" ID="pnlPay">
                        <table style="width: 600px;">
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="register_lbl">Invoice #
                                </td>
                                <td>
                                    <asp:Label ID="lblInvoiceName" runat="server" Style="font-weight: bold; font-size: 15px;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="register_lbl">Card Number<asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server"
                                    ControlToValidate="txtCardNumber" Display="None" ErrorMessage="Card Number Required"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator36_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator36">
                                    </asp:ValidatorCalloutExtender>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control"
                                        autocomplete="off" MaxLength="50"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtCardNumber" runat="server"
                                        FilterType="Numbers" TargetControlID="txtCardNumber">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="register_lbl">Expiry<asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ControlToValidate="ddlMonth"
                                    Display="None" ErrorMessage="Expiry Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator39_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator39">
                                    </asp:ValidatorCalloutExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="ddlYear"
                                        Display="None" ErrorMessage="Expiry Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator40_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator40">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control"
                                        Width="80px">
                                        <asp:ListItem Value="">Month</asp:ListItem>
                                        <asp:ListItem>01</asp:ListItem>
                                        <asp:ListItem>02</asp:ListItem>
                                        <asp:ListItem>03</asp:ListItem>
                                        <asp:ListItem>04</asp:ListItem>
                                        <asp:ListItem>05</asp:ListItem>
                                        <asp:ListItem>06</asp:ListItem>
                                        <asp:ListItem>07</asp:ListItem>
                                        <asp:ListItem>08</asp:ListItem>
                                        <asp:ListItem>09</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                    </asp:DropDownList>
                                    -
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"
                    Width="80px">
                    <asp:ListItem Value="">Year</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="register_lbl">Name on Card
                <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ControlToValidate="txtNameOnCard"
                    Display="None" ErrorMessage="Name on Card Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator37_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator37">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNameOnCard" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtNameOnCard" runat="server"
                                        FilterType="UppercaseLetters,Lowercaseletters,custom" ValidChars=" " TargetControlID="txtNameOnCard">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="register_lbl">CVC<asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="txtCVC"
                                    Display="None" ErrorMessage="CVC Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator41_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator41">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCVC" CssClass="form-control" runat="server" MaxLength="4" Width="50px" autocomplete="off"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtCVC" runat="server" FilterType="Numbers"
                                        TargetControlID="txtCVC">
                                    </asp:FilteredTextBoxExtender>
                                    <a runat="server" visible="false" id="lnkcvc">What is this?</a>
                                    <asp:HoverMenuExtender ID="hmeCVC" runat="server" OffsetY="-150" OffsetX="100" PopupControlID="dialog"
                                        TargetControlID="lnkcvc">
                                    </asp:HoverMenuExtender>
                                    <div runat="server" id="dialog" style="display: none; position: absolute;" class="shadow">
                                        <img id="cvc" src="images/security-code.jpg" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="register_lbl">Amount
                <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ControlToValidate="txtAmount"
                    Display="None" ErrorMessage="Amount Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator38_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator38">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtAmount" runat="server"
                                        FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtAmount">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="register_lbl">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnPayment" runat="server" Text="Make Payment" OnClick="btnPayment_Click" />
                                    &nbsp;
                <asp:Button ID="btnCancel" runat="server" CausesValidation="False"
                    OnClick="btnCancel_Click" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>
