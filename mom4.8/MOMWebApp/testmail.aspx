<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" Inherits="testmail" Codebehind="testmail.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">

        function setIframeHeight(iframe) {
            if (iframe) {
                var iframeWin = iframe.contentWindow || iframe.contentDocument.parentWindow;
                if (iframeWin.document.body) {

                    var height = iframeWin.document.documentElement.scrollHeight || iframeWin.document.body.scrollHeight;
                    var width = iframeWin.document.documentElement.scrollWidth || iframeWin.document.body.scrollWidth;
                    iframe.height = (height + 10) + "px";
                    iframe.width = (width + 10) + "px";

                }
            }
        };

        //        $(document).ready(function() {

        //            $("#<%=ifbody.ClientID%>").load(function() {
        //                setIframeHeight(this);
        //            });

        //        });

       
    </script>

    <style type="text/css">
        .style1
        {
            font-size: medium;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
    <asp:PostBackTrigger  ControlID="dlAttachments"/>
    </Triggers>
   <ContentTemplate>
            <table>
                <tr>
                    <td rowspan="5" valign="top">
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <table runat="server" id="tblhost" visible="false">
                            <tr>
                                <td>
                                    Host
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHost" runat="server" Width="250px"></asp:TextBox>
                                </td>
                                <td>
                                    Port
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPort" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Username
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUsername" runat="server" Width="250px"></asp:TextBox>
                                </td>
                                <td>
                                    Password
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPass" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                   <%-- <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" Text="send" Visible="False" />
                                    <asp:Button ID="btnEml" runat="server" OnClick="btnEml_Click" Text="loademl" Visible="False" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
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
                        &nbsp;
                        <asp:Button ID="btnDownload" runat="server" 
                            Text="Download mails from mail server to MOM db" />
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
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnRefresh" runat="server" OnClick="Button1_Click" Text="Refresh"
                            Visible="False" />
                    </td>
                    <td class="style1">
                        Mails downloaded on MOM database
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkCompose" runat="server" OnClick="lnkCompose_Click" 
                                        Visible="False">Compose</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkInbox" runat="server" onclick="lnkInbox_Click">Inbox</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkOutbox" runat="server" onclick="lnkOutbox_Click">Sent</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <asp:GridView ID="gvmail" runat="server" AutoGenerateColumns="False" CssClass="altrowstable"
                            EmptyDataText="No records to display" PageSize="20">
                            <AlternatingRowStyle CssClass="oddrowcolor" />
                            <FooterStyle CssClass="footer" />
                            <RowStyle CssClass="evenrowcolor" />
                            <SelectedRowStyle CssClass="selectedrowcolor" />
                            <Columns>
                                <asp:TemplateField HeaderText="Subject">
                                    <ItemTemplate>
                                        <asp:Label ID="lnkMsgID" Visible="false" runat="server" Text='<%# Eval("guid") %>'></asp:Label>
                                        <asp:LinkButton ID="lnkSub" runat="server" Text='<%# Eval("subject") %>' OnClick="lnkSub_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="From">
                                    <ItemTemplate>
                                        <asp:Label ID="lnkFrom" runat="server" Text='<%# Eval("from") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="To">
                                    <ItemTemplate>
                                        <asp:Label ID="lnkTo" runat="server" Text='<%# Eval("to") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date Sent">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSentdate" runat="server" Text='<%# Eval("SentDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Rec. Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecdate" runat="server" Text='<%# Eval("recDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td valign="top">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <div style="width: 110px; margin-left: 60px;">
                <asp:Panel ID="pnlBody" runat="server" Visible="False">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="register_lbl">
                                            From
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFrom" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="register_lbl">
                                            To
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTo" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="register_lbl">
                                            CC
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCC" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="register_lbl">
                                            Bcc
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBcc" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="register_lbl">
                                            Subject
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSub" runat="server" Style="font-weight: bold;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="register_lbl">
                                            Attachments
                                        </td>
                                        <td>
                                            <asp:DataList ID="dlAttachments" runat="server" CellPadding="0" CellSpacing="5" 
                                                RepeatColumns="5" RepeatDirection="Horizontal">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="imgbtnAttachment" runat="server" 
                                                        OnClick="imgbtnAttachment_Click" Text='<%# Eval("FileName") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <iframe id="ifbody" runat="server" name="ifbody" style="border: none; width: 1024px;
                                    height: 600px"></iframe>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlCompose" runat="server" Visible="false" >
                    <table>
                        <tr>
                            <td class="register_lbl">
                                &nbsp;
                                <asp:Button ID="btnSendMAil" runat="server" OnClick="btnSendMAil_Click" 
                                    Text="Send" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="register_lbl">
                                From
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmailFrom" runat="server" CssClass="register_input_bg_address"
                                    Placeholder="From" TabIndex="9" ToolTip="From"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="txtEmailFrom_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtEmailFrom">
                                </asp:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtEmailFrom"
                                    Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ValidationGroup="compose" Enabled="False"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator7_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator7">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="register_lbl">
                                To
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="register_input_bg_address" Placeholder="To"
                                    TabIndex="9" ToolTip="To"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtEmail">
                                </asp:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtEmail"
                                    Display="None" ErrorMessage="Email Required" SetFocusOnError="True" ValidationGroup="compose"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator9_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator9">
                                </asp:ValidatorCalloutExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                    Display="None" ErrorMessage="Invalid E-Mail Address" ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                    ValidationGroup="compose"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="register_lbl">
                                CC
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmailCc" runat="server" CssClass="register_input_bg_address"
                                    Placeholder="Cc" TabIndex="9" ToolTip="Cc"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="txtEmailCc_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtEmailCc">
                                </asp:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtEmailCc"
                                    Display="None" ErrorMessage="Invalid E-Mail Address" ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                    ValidationGroup="compose"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator6_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator6">
                                </asp:ValidatorCalloutExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="register_lbl">
                                Subject
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubject" runat="server" CssClass="register_input_bg_customer"
                                    Placeholder="Subject" TabIndex="9" ToolTip="Subject"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txtBody" runat="server" Columns="50" Height="357px" Rows="10" TextMode="MultiLine"
                                    Width="750px"></asp:TextBox>
                                <asp:HtmlEditorExtender ID="htmlEditorExtender1" runat="server" Enabled="True" EnableSanitization="False"
                                    TargetControlID="txtBody" DisplaySourceTab="true" OnImageUploadComplete="htmlEditorExtender1_ImageUploadComplete">
                                    <Toolbar>
                                        <asp:Bold />
                                        <asp:Italic />
                                        <asp:Underline />                                        
                                    </Toolbar>
                                </asp:HtmlEditorExtender>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

        //        Sys.Application.add_load
        //        (
        //            function() {
        //                $("#<%=ifbody.ClientID%>").load(function() {
        //                    setIframeHeight(this);
        //                });
        //            }
        //        )
        
    </script>

</asp:Content>
