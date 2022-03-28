<%@ Page Language="C#" AutoEventWireup="true" Inherits="test2" Codebehind="test2.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        function dtaa() {
            this.prefixText = null;
            this.con = '';
            this.custID = null;
        }
        function callService() {
            var dtaaa = new dtaa();
            dtaaa.prefixText = 'b';
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "TestHandler.ashx",
//                url: "CustomerAuto.asmx/GetCustomer",
//                url: "CustomerAuto.asmx/GetCustomerWOSerialize",
//                //data: '{"prefixText":' + JSON.stringify(request.term) + ',"con":' + JSON.stringify(document.getElementById('ctl00_ContentPlaceHolder1_hdnCon').value) + '}',
                data: JSON.stringify(dtaaa),                
                dataType: "json",
                async: true,
                success: function(data) {
                alert(data);
                    alert(data.d);
                    alert($.parseJSON(data.d));
                    alert(JSON.stringify(data.d));
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    var err = eval("(" + XMLHttpRequest.responseText + ")");
                    alert(err.Message);
                }
            });
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <input id="Button1" type="button" value="button" onclick="callService()" />
    <img src="companylogo.ashx" />
    <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Button" />
    </form>
    <table width ="250px" style="border:solid 1px #000">
        <tr>
            <th style="border:solid 1px #ccc">
                invoice</th>
            <th style="border:solid 1px #ccc">
                amount</th>
        </tr>
        <tr>
            <td style="border:solid 1px #ccc">
                123</td>
            <td style="border:solid 1px #ccc">
                100</td>
        </tr>
        <tr>
            <th>
              total</th>
            <th>
               100</th>
        </tr>
    </table>
</body>
</html>
