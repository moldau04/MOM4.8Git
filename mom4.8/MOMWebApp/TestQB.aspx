<%@ Page Language="C#" AutoEventWireup="true" Inherits="TestQB" Codebehind="TestQB.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
        <br />
        <br />
    <asp:GridView ID="dgrd_invoice" runat="server"  AutoGenerateColumns="False"
                                Width="100%" >
                                <Columns>
                                   
                                    <asp:TemplateField HeaderText="Facility Name" SortExpression="CustomerName">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_customername" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date of Invoice" SortExpression="InvoiceDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_invoicedate" runat="server" Text='<%# Bind("InvoiceDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" SortExpression="SubTotal">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text="$"></asp:Label>&nbsp;<asp:Label ID="lbl_amount"
                                                runat="server" Text='<%# Bind("SubTotal") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Paid Status" SortExpression="PaidStatus">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_status" runat="server" Text='<%# Bind("PaidStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle BackColor="WhiteSmoke" />
                                <EmptyDataTemplate>
                                    <table border="0" width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="No Invoice Found"></asp:Label></td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                                <HeaderStyle BackColor="LightGray" />
                                <AlternatingRowStyle BackColor="Transparent" />
                              
                            </asp:GridView>
    </div>
    </form>
</body>
</html>
