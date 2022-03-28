<%@ Page Title="" Language="C#" MasterPageFile="~/homemaster.master" AutoEventWireup="true" Inherits="ACHPayment" Codebehind="ACHPayment.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .ACHtxt {
            height: 28px;
            width: 200px;
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="clearfix">
        </div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <asp:LinkButton ID="lnkBack" runat="server" CausesValidation="False"
                        OnClick="lnkBack_Click">&lt; Back to Invoice</asp:LinkButton>
                </div>
            </div>
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <asp:HiddenField ID="hdnInvoices" runat="server" />
                    <div style="padding-bottom: 10px">
                    </div>
                    <div class="clearfix"></div>
                    <div>
                        <div class="table-scrollable" style="border: none">
                            <asp:Panel runat="server" ID="pnlPay">
                                <table class="table-scrollable" style="width: 100%;">
                                    <tr>
                                        <td style="padding: 1%; width: 15%;"></td>
                                        <td style="padding: 0%; width: 30%;"></td>

                                        <td rowspan="9" style="padding-right: 90px; width: 55%;">

                                            <asp:GridView ID="GridViewACHPayment" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                PageSize="3" AllowPaging="false" AllowSorting="false" ShowFooter="true"
                                                EmptyDataText="No Records Found...">
                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select" Visible="true">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkcheck" AutoPostBack="true" OnCheckedChanged="chkcheck_CheckedChanged"
                                                                runat="server"
                                                                group="g1"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bank Routing #" SortExpression="RoutingNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblroutingno" runat="server" Text='<%# Eval("RoutingNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bank Account #" SortExpression="AccountNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAccountNo" runat="server" Text='<%# Eval("AccountNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Account Holder's Name" SortExpression="Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle CssClass="footer" />
                                                <RowStyle CssClass="evenrowcolor" />
                                                <SelectedRowStyle CssClass="selectedrowcolor" />

                                            </asp:GridView>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 1%">Invoices(s) #</td>
                                        <td style="padding: 0%">
                                            <asp:TextBox ID="txtInvoices" runat="server" CssClass="ACHtxt"></asp:TextBox></td>

                                    </tr>

                                    <tr>
                                        <td style="padding: 1%">Bank Routing #</td>
                                        <td>
                                            <asp:TextBox ID="txtRouting"
                                                autocomplete="off" runat="server" MaxLength="17" Style="float: left;" CssClass="ACHtxt"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ControlToValidate="txtRouting" ValidationGroup="Ach"
                                                Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator37_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator37">
                                            </asp:ValidatorCalloutExtender>
                                            <ul class="nav navbar-nav pull-left">
                                                <li class="dropdown dropdown-user">
                                                    <a href="#" title="Bank Routing #" data-toggle="dropdown" class="dropdown-toggle" data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important; font-size: 14px;">&nbsp;?
                                                    </a>
                                                    <ul id="PaymentMenu" class="dropdown-menu dropdown-menu-default">
                                                        <li>
                                                            <img width="350" height="150" src="images/check.png" />
                                                        </li>
                                                    </ul>
                                                </li>

                                            </ul>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 1%">Bank Account #</td>
                                        <td>
                                            <asp:TextBox ID="txtBankAc"  autocomplete="off" MaxLength="16" runat="server" CssClass="ACHtxt"></asp:TextBox>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBankAc" ValidationGroup="Ach"
                                                Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1"
                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 1%">Account Holder's Name</td>
                                        <td>
                                            <asp:TextBox ID="txtName"  autocomplete="off" MaxLength="20" runat="server" CssClass="ACHtxt"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName" ValidationGroup="Ach"
                                                Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2"
                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 1%">Amount</td>
                                        <td>
                                            <asp:TextBox ID="txtAmount" runat="server" MaxLength="10" CssClass="ACHtxt"
                                                autocomplete="off" ReadOnly="true"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender_txtAmount" runat="server"
                                                FilterType="Numbers,Custom" ValidChars="." TargetControlID="txtAmount">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtAmount" ValidationGroup="Ach"
                                                Display="None" ErrorMessage="Amount Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3"
                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
                                            </asp:ValidatorCalloutExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 1%"></td>
                                        <td style="padding: 0%"></td>

                                    </tr>
                                    <tr>
                                        <td style="padding: 1%"></td>
                                        <td style="padding: 0%">
                                            <asp:Button ID="btnPayment" ValidationGroup="Ach" CssClass="btn btn-default" runat="server" OnClientClick="return IsValid();" Text="Make Payment" OnClick="btnPayment_Click" />
                                            <asp:Button ID="btnCancel" CssClass="btn btn-danger" runat="server" CausesValidation="False"
                                                Text="Cancel" OnClick="btnCancel_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding: 1%"></td>
                                        <td style="padding: 0%"></td>

                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="table-scrollable" style="border: none">
                        <asp:Label ID="lblErr" runat="server" Style="color: Red; font-weight: bold; font-size: larger;"></asp:Label>
                        <asp:Label ID="lblMSG" runat="server" Style="font-weight: bold; font-size: 18px;" ForeColor="Red"></asp:Label>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>
    </div>
  
    <%--Show Loader--%>
    <script type="text/javascript">

        function ShowProgress() {

            setTimeout(function () {

                var loading = $(".loading");
                loading.show();
                $("#loading").show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            });
        }

        function IsValid() {

            if (Page_ClientValidate()) {
                ShowProgress();
                return true;
            }
            else { return false; }
        }
    </script>
    <style type="text/css">
        .pnlUpdateoverlay {
            background-color: #fff;
            height: 100%;
            width: 100%;
            position: fixed;
            top: 0px;
            z-index: 10010;
            filter: alpha(opacity=50);
            -moz-opacity: 0.8;
            opacity: 0.8;
        }

        .loading {
            font-family: Arial;
            font-size: 10pt;
            border: 5px solid #316b9d;
            width: 200px;
            height: 120px;
            display: none;
            position: fixed;
            background-color: White;
            z-index: 10011;
        }
    </style> 
</asp:Content>  