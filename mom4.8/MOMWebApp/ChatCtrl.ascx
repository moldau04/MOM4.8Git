<%@ Control Language="C#" AutoEventWireup="true" Inherits="ChatCtrl" Codebehind="ChatCtrl.ascx.cs" %>

<div id="chat_widnow">
    <div id="chat_title_bar"> <span class="col-sm-9 text-primary"><strong>Online Users</strong></span>
        <div id="chat_min_button"><i class="fa fa-plus-square"></i></div>
    </div>
    <div id="chat_box" style="display: none;overflow-y:auto;">
    </div>
</div>
<div id="chat_div"></div>
<input id="hdId" type="hidden" />
<input id="hdUserName" type="hidden" />
<asp:HiddenField ID="hdnCurrentUserName" runat="server" />
<asp:HiddenField ID="hdnCurrentUserID" runat="server" />

<script src="<%=Page.ResolveUrl("~") %>Scripts/jquery.signalR-2.2.0.min.js"></script>
<!--Reference the autogenerated SignalR hub script. -->
<script src="<%=Page.ResolveUrl("~") %>signalr/hubs"></script>
<link href="<%=Page.ResolveUrl("~") %>Styles/styleChat.css" rel="stylesheet" />        
<link href="<%=Page.ResolveUrl("~") %>styles/jquery.ui.chatbox.css" rel="stylesheet" />
<script src="<%=Page.ResolveUrl("~") %>scripts/jquery.ui.chatbox.js"></script>
<script src="<%=Page.ResolveUrl("~") %>scripts/chatboxManager.min.js"></script>
