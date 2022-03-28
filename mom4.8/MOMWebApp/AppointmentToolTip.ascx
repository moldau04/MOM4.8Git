<%@ Control Language="C#" AutoEventWireup="true" Inherits="AppointmentToolTip" Codebehind="AppointmentToolTip.ascx.cs" %>
<div style="margin: 1px 1px 1px 1px; font-size: 11px; padding-bottom: 10px;">
    <div style="border-bottom: solid 1px #ccc; margin-bottom: 1px; font-size: 11px;">
        <asp:Label Font-Bold="true" runat="server" ID="lblSubject"></asp:Label></div>
    <div style="display:none;"> <b>Time range:</b>
        <asp:Label runat="server" ID="Timerange"></asp:Label></div>
    <div> <b>Date:</b>
        <asp:Label runat="server" ID="Date"></asp:Label></div>
    <div> <b>Status:</b> <asp:Label runat="server" ID="Status"></asp:Label>  <b>Category:</b>
        <asp:Label runat="server" ID="Category"></asp:Label>
       </div>
     <div> <b>Address:</b>
        <asp:Label runat="server" ID="Address"></asp:Label></div>
    <div> <b>Phone:</b>
        <asp:Label runat="server" ID="Phone"></asp:Label></div>
    
    <div> <b> Reason:</b>
        <asp:Label runat="server" ID="Reason"></asp:Label></div>
        <div> <b> Resolution:</b>
        <asp:Label runat="server" ID="Resolution"></asp:Label></div>
    <div>
    <asp:Image ID="imgreview" Visible="false" Width="15px" Height="15px" runat="server" ImageUrl="#" />

                                                 <asp:Image ID="imgConfirmed" Visible="false" Width="15px" Height="15px" runat="server" ImageUrl="#" />

                                                <asp:Image ID="imgMS" Visible="false" Width="15px" Height="15px" runat="server" ImageUrl="#" />

                                                <asp:Image ID="ImgDocument" Visible="false" Width="15px" Height="15px" runat="server" ImageUrl="#" />

                                                <asp:Image ID="imgalert" Visible="false" Width="15px" Height="15px" runat="server" ImageUrl="#" />

                                                <asp:Image ID="imgcredithold" Visible="false" Width="15px" Height="15px" runat="server" ImageUrl="#" />

                                                <asp:Image ID="ImgSignature" Visible="false" Width="15px" Height="15px" runat="server" ImageUrl="#" />

                                                <asp:Image ID="ImgChargeable" Visible="false" Width="15px" Height="15px" runat="server" ImageUrl="#" />
    </div>
</div>
