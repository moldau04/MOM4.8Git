 <%@ Page Title="Scheduler || MOM" Language="C#" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/Mom.master" Inherits="WebFormScheduler" CodeBehind="~/WebFormScheduler.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"> 

    <script>
        function resizeIframe(obj) {
            obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
        }  
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server"> 
   <%-- <div class="page-title"><i class="mdi-calender"></i>&nbsp;Scheduler</div>
        <div class="btnclosewrap"><a  href="Home.aspx" ><i class="mdi-content-clear"></i></a></div>--%>
     <iframe id="scheduleiframe" src="Scheduler.aspx" width="100%" frameborder="0" scrolling="no" onload="resizeIframe(this)" ></iframe> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server"></asp:Content>