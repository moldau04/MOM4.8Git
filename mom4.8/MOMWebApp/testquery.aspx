<%@ Page Language="C#" AutoEventWireup="true" Inherits="testquery" Codebehind="testquery.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <table style="width:100%;">
        <tr>
            <td>
                <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
                <asp:TextBox ID="txtPass" runat="server" TextMode="Password"></asp:TextBox>
                <asp:Button ID="btnExecute" runat="server" onclick="btnExecute_Click" 
                    Text="Execute" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtQuery" runat="server" Height="111px" TextMode="MultiLine" 
                    Width="901px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvResults" runat="server">
                </asp:GridView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
