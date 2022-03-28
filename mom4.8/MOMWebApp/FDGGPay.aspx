<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" Inherits="FDGGPay" Codebehind="FDGGPay.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
   
    <table>
        <tr>
            <td width="100">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td width="100">
                <asp:LinkButton ID="lnkBack" runat="server" CausesValidation="False" 
                    onclick="lnkBack_Click">&lt; Back to Invoice</asp:LinkButton>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td width="100">
                &nbsp;</td>
            <td>
                <asp:Label ID="lblErr" runat="server" Style="color: Red; font-weight: bold; font-size: larger;"></asp:Label>
    <asp:Label ID="lblMSG" runat="server" Style="font-weight: bold; font-size: 18px;" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        </table>
    <asp:Panel runat="server" ID="pnlPay">
        <table style="width: 350px;">
            <tr>
                <td class="register_lbl">
                    Customer Name</td>
                <td>
                    <asp:TextBox ID="txtNameCustomer" runat="server" autocomplete="off" 
                        CssClass="register_input_bg" Width="250px"></asp:TextBox>
                </td>
            </tr>
           
            <tr>
                <td class="register_lbl">
                    Invoice #
                </td>
                <td>
                    <asp:Label ID="lblInvoiceName" runat="server" 
                        Style="font-weight: bold; font-size: 15px;"></asp:Label>
                </td>
            </tr>
           
            <tr>
                <td class="register_lbl">
                    Card Number<asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server"
                        ControlToValidate="txtCardNumber" Display="None" ErrorMessage="Card Number Required"
                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator36_ValidatorCalloutExtender"
                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator36">
                    </asp:ValidatorCalloutExtender>
                   
                </td>
                <td>
                    <asp:TextBox ID="txtCardNumber" runat="server" CssClass="register_input_bg"
                        autocomplete="off" MaxLength="19"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtCardNumber" runat="server"
                        FilterType="Numbers" TargetControlID="txtCardNumber">
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td class="register_lbl">
                    Expiry<asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ControlToValidate="ddlMonth"
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
                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="register_input_bg_ddl" Width="80px">
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
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="register_input_bg_ddl" Width="80px">
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
                <td class="register_lbl">
                    Name on Card
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ControlToValidate="txtNameOnCard"
                        Display="None" ErrorMessage="Name on Card Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator37_ValidatorCalloutExtender"
                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator37">
                    </asp:ValidatorCalloutExtender>
                </td>
                <td>
                    <asp:TextBox ID="txtNameOnCard" runat="server" CssClass="register_input_bg" 
                        MaxLength="26" autocomplete="off" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="register_lbl">
                    CVC<asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="txtCVC"
                        Display="None" ErrorMessage="CVC Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator41_ValidatorCalloutExtender"
                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator41">
                    </asp:ValidatorCalloutExtender>
                </td>
                <td>
                    <asp:TextBox ID="txtCVC" CssClass="register_input_bg" runat="server" MaxLength="4"
                        Width="50px" autocomplete="off"></asp:TextBox>
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
                <td class="register_lbl">
                    Amount
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ControlToValidate="txtAmount"
                        Display="None" ErrorMessage="Amount Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator38_ValidatorCalloutExtender"
                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator38">
                    </asp:ValidatorCalloutExtender>
                </td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="register_input_bg" 
                        autocomplete="off" ReadOnly="true"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtAmount" runat="server"
                        FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtAmount">
                    </asp:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td class="register_lbl">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnPayment" runat="server" Text="Make Payment" OnClick="btnPayment_Click" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click"
                        Text="Cancel" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    
     </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
