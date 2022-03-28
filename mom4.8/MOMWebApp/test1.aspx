<%@ Page Language="C#" AutoEventWireup="true" Inherits="test1" Codebehind="test1.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="js/jquery-1.7.1.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="css/Notifycss/buttons.css" />

    <script type="text/javascript" src="/js/Notifyjs/jquery.noty.js"></script>

    <script type="text/javascript" src="/js/Notifyjs/layouts/bottom.js"></script>

    <script type="text/javascript" src="/js/Notifyjs/layouts/bottomCenter.js"></script>

    <script type="text/javascript" src="/js/Notifyjs/layouts/bottomLeft.js"></script>

    <script type="text/javascript" src="/js/Notifyjs/layouts/bottomRight.js"></script>

    <script type="text/javascript" src="/js/Notifyjs/layouts/center.js"></script>

    <script type="text/javascript" src="/js/Notifyjs/layouts/centerLeft.js"></script>

    <script type="text/javascript" src="/js/Notifyjs/layouts/centerRight.js"></script>

    <script type="text/javascript" src="/js/Notifyjs/layouts/inline.js"></script>

    <script type="text/javascript" src="/js/Notifyjs/layouts/top.js"></script>

    <script type="text/javascript" src="/js/Notifyjs/layouts/topCenter.js"></script>

    <script type="text/javascript" src="/js/Notifyjs/layouts/topLeft.js"></script>

    <script type="text/javascript" src="/js/Notifyjs/layouts/topRight.js"></script>

    <!-- themes -->

    <script type="text/javascript" src="/js/Notifyjs/themes/default.js"></script>

    <link href="css/smoothness/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />

    <script src="js/ui/jquery-ui-1.8.17.custom.min.js" type="text/javascript"></script>

    <script language='Javascript' type="text/javascript">
        function addFile() {
            var ni = document.getElementById("fileDiv");
            var objFileCount = document.getElementById("fileCount");
            var num = (document.getElementById("fileCount").value - 1) + 2;
            objFileCount.value = num;
            var newdiv = document.createElement("div");
            var divIdName = "file" + num + "Div";
            newdiv.setAttribute("id", divIdName);
            newdiv.innerHTML = '<input type="file" name="attachment" id="attachment"/><a href="#" onclick="javascript:removeFile(' + divIdName + ');">Remove </a>';
            ni.appendChild(newdiv);
        }


        function removeFile(divName) {
            var d = document.getElementById("fileDiv");
            d.removeChild(divName);
        }

        function Handler() {
            var objimage = document.getElementById("image");
            objimage.setAttribute('src', "imagehandler.ashx?catid=test");
        }
    </script>
    

</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager runat="server" />   
    <asp:Button ID="btnUploads" runat="server" Text="Upload" />
    <asp:AjaxFileUpload ID="ajaxUpload1" OnUploadComplete="ajaxUpload1_OnUploadComplete"
        runat="server" />
    <div>
        <input type="file" name="attachment" runat="server" id="attachment" onchange="document.getElementById('moreUploadsLink').style.display = 'block';" />
        <input type="hidden" value="0" id="fileCount" />
        <div id="fileDiv">
        </div>
        <div id="moreUploadsLink">
            <a href="javascript:addFile();">Attach another File</a>
        </div>
        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
        <br />
        <asp:Label ID="lblStatus" runat="server"></asp:Label>
        for single file upload set div id="moreUploadsLink" style="display: none"
    </div>
    <asp:AsyncFileUpload OnUploadedComplete="AsyncFileUpload1_UploadedComplete" runat="server"
        ID="AsyncFileUpload1" Width="400px" UploaderStyle="Traditional" UploadingBackColor="#CCFFFF"
        ThrobberID="myThrobber" />
        <asp:Label runat="server" ID="myThrobber" Style="display: none;"><img align="absmiddle" alt="" src="images/ajax-loader-trans.gif" /></asp:Label>
    <input id="Button1" type="button" value="Handler" onclick="Handler();" />
    <img id="image" alt="" src="" /> 
    </form>
</body>
</html>
