<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Mom.master" Inherits="CollectionPopup" Codebehind="CollectionPopup.aspx.cs" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Design/css/grid.css" rel="stylesheet" />
    <style>
        #main {
            padding-left:0px !important;
        }
       .RadGrid_Bootstrap .rgEditForm{
           border:none !important;
       }
         .RadGrid_Bootstrap .rgEditForm label{
         font-weight:bolder;
       }
        textarea.materialize-textarea {
            padding: 1rem 0 0 0;
            min-height: 2rem;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">   
    <telerik:RadAjaxManager ID="RadAjaxManager_gvContacts" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkAddnew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="contactWindow" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnEdit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="contactWindow" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkContactSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvContacts" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                    <telerik:AjaxUpdatedControl ControlID="divInvoiceStatements" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkCustomerRemarks">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="CustRemarks" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkLocationRemarks">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LocRemarks" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSaveNote">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_CollectionNotes" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                    <telerik:AjaxUpdatedControl ControlID="CollectionNotes_1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
<telerik:AjaxSetting AjaxControlID="chkShowAllNote">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_CollectionNotes" LoadingPanelID="RadAjaxLoadingPanel_Customer" />
                    <telerik:AjaxUpdatedControl ControlID="CollectionNotes_1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadWindowManager ID="RadWindowManagerCustomer" runat="server">
        <Windows>
            <telerik:RadWindow ID="contactWindow" Skin="Material" VisibleTitlebar="true" Title="Add Contact" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="600" Height="400">
                <ContentTemplate>
                    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanelContact">
                        <div style="margin-top: 15px;">
                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtContcName"
                                                Display="None" ErrorMessage="Contact Name Required" SetFocusOnError="True" ValidationGroup="cont">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" PopupPosition="left" TargetControlID="RequiredFieldValidator12">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:TextBox ID="txtContcName" runat="server" CssClass="Contact-search" MaxLength="50"></asp:TextBox>
                                            <asp:Label runat="server" AssociatedControlID="txtContcName">Contact Name</asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:TextBox ID="txtTitle" runat="server" MaxLength="50"></asp:TextBox>
                                            <asp:Label runat="server" AssociatedControlID="txtTitle">Title</asp:Label>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:TextBox ID="txtContPhone" runat="server" MaxLength="22"></asp:TextBox>
                                            <asp:Label runat="server" AssociatedControlID="txtContPhone">Phone</asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:TextBox ID="txtContFax" runat="server" MaxLength="22"></asp:TextBox>
                                            <asp:Label runat="server" AssociatedControlID="txtContFax">Fax</asp:Label>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:TextBox ID="txtContCell" runat="server" CssClass="form-control" MaxLength="22"></asp:TextBox>
                                            <asp:Label runat="server" AssociatedControlID="txtContCell">Cell</asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:TextBox ID="txtContEmail" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>

                                            <asp:Label runat="server" AssociatedControlID="txtContEmail">Email</asp:Label>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtContEmail"
                                                Display="None" ErrorMessage="Invalid Email" ValidationGroup="cont" SetFocusOnError="True"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                            </asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                            </asp:ValidatorCalloutExtender>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="form-section-row">
                                <div class="form-section4">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <div class="checkrow">
                                                <asp:CheckBox ID="chkEmailTicket" runat="server" class="filled-in" />
                                                <label for="chkEmailTicket" style="top: -1px !important">Ticket</label>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="form-section4">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <div class="checkrow">
                                                <asp:CheckBox ID="chkEmailInvoice" runat="server" class="filled-in" />
                                                <label for="chkEmailInvoice" style="top: -1px !important">Invoice/Statement</label>
                                            </div>
                                        </div>
                                    </div>

                                </div>

                                <div class="form-section4">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <div class="checkrow">
                                                <asp:CheckBox ID="chkShutdownA" runat="server" class="filled-in" />
                                                <asp:Label runat="server" AssociatedControlID="chkShutdownA" Style="top: -1px !important">Shutdown Alert</asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="form-section4">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <div class="checkrow">
                                                <asp:CheckBox ID="chkTestProposals" runat="server" class="filled-in" />
                                                <label for="chkTestProposals" style="top: -1px !important">Test Proposals</label>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div style="clear: both;"></div>

                            <footer style="float: right;">
                                <div class="btnlinks">
                                    <asp:LinkButton ID="lnkContactSave" runat="server" OnClick="lnkContactSave_Click" ValidationGroup="cont">Save</asp:LinkButton>
                                </div>
                            </footer>
                        </div>
                    </telerik:RadAjaxPanel>
                </ContentTemplate>
            </telerik:RadWindow>
            <telerik:RadWindow ID="mailWindow" Skin="Material" VisibleTitlebar="true" Title="Send Mail" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="300" Height="280">
                <ContentTemplate>
                    <div style="margin-top: 15px;">
                        <div class="form-section-row">
                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <div class="input-field col s4" style="margin-top: 4px; margin-bottom: 4px;" id="div1" runat="server">
                                            <div class="checkrow">
                                                <asp:CheckBox ID="chkInvoiceMail" Checked="true" runat="server" CssClass="filled-in" />
                                                <asp:Label runat="server" Style="margin-top: -7px!important;" AssociatedControlID="chkInvoiceMail">Invoice</asp:Label>
                                            </div>
                                        </div>
                                        <div class="input-field col s4" style="margin-top: 4px; margin-bottom: 4px;" id="div2" runat="server">
                                            <div class="checkrow">
                                                <asp:CheckBox ID="chkCustomerStatementMail" runat="server" CssClass="filled-in" />
                                                <asp:Label runat="server" Style="margin-top: -7px!important;" AssociatedControlID="chkCustomerStatementMail">Customer Statements</asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-section-row" id="InvoiceTemplate">
                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <label class="drpdwn-label">Invoice Template</label>
                                        <asp:DropDownList ID="drpInvoiceTemplate" runat="server"
                                            CssClass="browser-default">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-section-row" id="CustomerStatement" style="display: none;">
                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <label class="drpdwn-label">Customer Statement Template</label>
                                        <asp:DropDownList ID="drpCustomerStatementTempate" runat="server"
                                            CssClass="browser-default">
                                            <asp:ListItem Value="1">Template-1</asp:ListItem>
                                            <asp:ListItem Value="2">Template-2</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>

                        <footer style="float: right;">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkSendMail" runat="server" OnClick="lnkSendMail_Click" CausesValidation="false">Mail</asp:LinkButton>
                            </div>
                        </footer>
                    </div>

                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Customer" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="form-section-row" style="">
        <div class="row">
            <div class="col s12 m12 l12" style="padding-right: 0px;">
                <div class="row">
                    <div class="est-tabs" style="margin-top: 20px;">
                        <div class="col s12">
                            <ul class="tabs tab-demo-active white" style="width: 100%; margin-bottom: 15px;">
                                <li class="tab col s2">
                                    <a class="white-text waves-effect waves-light active" href="#activeone">Contacts</a>
                                </li>
                                <li class="tab col s2">
                                    <a class="white-text waves-effect waves-light" href="#two">&nbsp;Customer Remarks</a>
                                </li>
                                <li class="tab col s2" runat="server" id="liLocationRemarks">
                                    <a class="white-text waves-effect waves-light" href="#three">&nbsp;Location Remarks</a>
                                </li>
                                <li class="tab col s2">
                                    <a class="white-text waves-effect waves-light" href="#four">&nbsp;Notes</a>
                                </li>
                            </ul>
                        </div>

                        <div class="col s12">
                            <div id="activeone" class="col s12 tab-container-border lighten-4" style="display: block;">
                                <div class="row">
                                    <div class="form-content-wrap" style="overflow: auto;">
                                        <div class="tabs-custom-mgn1 mn-ht">
                                            <div class="form-section-row" style="margin-bottom:5px;">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkAddnew" runat="server" CausesValidation="False" OnClientClick="return AddContactClick(this);" OnClick="lnkAddnew_Click">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ToolTip="Edit" ID="btnEdit" runat="server" OnClientClick="return EditContactClick(this);" CausesValidation="False" OnClick="btnEdit_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnDelete" ToolTip="Delete" runat="server" CausesValidation="False" OnClientClick="return DeleteContactClick(this);" OnClick="btnDelete_Click">Delete</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkSave" ToolTip="Save" runat="server" CausesValidation="False" OnClick="lnkSave_Click">Save</asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="form-section-row">
                                                <div class="input-field col s4" style="margin-top: 4px; margin-bottom: 4px;" id="divPrintInvoice" runat="server">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkPrintOnly" runat="server" CssClass="filled-in" />
                                                        <asp:Label runat="server" Style="margin-top: -7px!important; padding-left:10px;" AssociatedControlID="chkPrintOnly">Print Invoice/Statements</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s4" style="margin-top: 4px; margin-bottom: 4px;" id="divEmailInvoice" runat="server">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkEmail" runat="server" CssClass="filled-in" />
                                                        <asp:Label runat="server" Style="margin-top: -7px!important; padding-left:10px;" AssociatedControlID="chkEmail">Email Invoice/Statements</asp:Label>
                                                    </div>
                                                </div>
                                                 <div class="input-field col s4" style="margin-top: 4px; margin-bottom: 4px;" id="divCustStatement" runat="server">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkNoCustStatement" runat="server" CssClass="filled-in" />
                                                        <asp:Label runat="server" Style="margin-top: -7px!important; padding-left:10px;" AssociatedControlID="chkNoCustStatement">No Customer Statement</asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section-row" id="divInvoiceStatements" runat="server">
                                                <div class="input-field col s6">
                                                    <label for="<%#txtEmailToInv.ClientID %>">
                                                        <asp:LinkButton runat="server" OnClick="lnkInvoiceStatementsEmailTo_Click" Style="color: #105099!important;" ID="lnkInvoiceStatementsEmailTo">
                                                            Invoice/Statements Email To
                                                        </asp:LinkButton>
                                                    </label>
                                                    <%--     <asp:Label runat="server" AssociatedControlID="txtEmailToInv"></asp:Label>--%>
                                                    <asp:TextBox ID="txtEmailToInv" runat="server" AutoCompleteType="Disabled" MaxLength="500"></asp:TextBox>
                                                </div>
                                                <div class="input-field col s6">
                                                    <label for="<%#txtEmailCCInv.ClientID %>">
                                                        <asp:LinkButton runat="server" OnClick="lnkInvoiceStatementsEmailCC_Click" Style="color: #105099!important;" ID="lnkInvoiceStatementsEmailCC">
                                                              Invoice/Statements Email CC
                                                        </asp:LinkButton>
                                                    </label>
                                                    <%--  <asp:Label runat="server" AssociatedControlID="txtEmailCCInv"></asp:Label>--%>
                                                    <asp:TextBox ID="txtEmailCCInv" runat="server" AutoCompleteType="Disabled" MaxLength="500"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-section-row">
                                                <div class="grid_container">
                                                    <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                        <div class="RadGrid RadGrid_Material FormGrid">
                                                            <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                                            </telerik:RadCodeBlock>
                                                            <telerik:RadAjaxPanel ID="RadAjaxPanel_gvContacts" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Customer">
                                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvContacts" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                    ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" PagerStyle-AlwaysVisible="true" Width="100%" FilterType="CheckList" OnPreRender="RadGrid_gvContacts_PreRender"
                                                                    AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvContacts_NeedDataSource" OnItemCreated="RadGrid_gvContacts_ItemCreated">
                                                                    <CommandItemStyle />
                                                                    <GroupingSettings CaseSensitive="false" />
                                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                        <Selecting AllowRowSelect="True"></Selecting>

                                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                        <Resizing ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
                                                                    </ClientSettings>
                                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                        <Columns>
                                                                            <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:HiddenField ID="hdnSelected" runat="server" />
                                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                                                            </telerik:GridClientSelectColumn>

                                                                            <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="5" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIndex" runat="server" Text='<%# Container.ItemIndex %>'></asp:Label>
                                                                                    <asp:HiddenField ID="hdContactID" runat="server" Value='<%# Eval("contactid") %>' />
                                                                                    <asp:HiddenField ID="hdCType" runat="server" Value='<%# Eval("ctype") %>' />
                                                                                      <asp:HiddenField ID="hdEmail" runat="server" Value='<%# Eval("Email") %>' />
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn UniqueName="lblIndexID" Display="false" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <%# Container.ItemIndex %>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn DataField="ContactID" SortExpression="ContactID" AutoPostBackOnFilter="true"
                                                                                CurrentFilterFunction="Contains" HeaderText="ContactID" ShowFilterIcon="false" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("ContactID") %>'></asp:Label>
                                                                                </ItemTemplate>

                                                                            </telerik:GridTemplateColumn>


                                                                            <telerik:GridTemplateColumn DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" HeaderStyle-Width="100"
                                                                                CurrentFilterFunction="Contains" HeaderText="Name" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                                </ItemTemplate>

                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn DataField="Title" SortExpression="Title" AutoPostBackOnFilter="true" HeaderStyle-Width="100"
                                                                                CurrentFilterFunction="Contains" HeaderText="Title" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                                                                </ItemTemplate>

                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn DataField="Phone" SortExpression="Phone" AutoPostBackOnFilter="true" HeaderStyle-Width="100"
                                                                                CurrentFilterFunction="Contains" HeaderText="Phone" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPhn" runat="server"><%#Eval("Phone")%></asp:Label>
                                                                                </ItemTemplate>

                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn DataField="Fax" SortExpression="Fax" AutoPostBackOnFilter="true" HeaderStyle-Width="100"
                                                                                CurrentFilterFunction="Contains" HeaderText="Fax" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblFx" runat="server"><%#Eval("Fax")%></asp:Label>
                                                                                </ItemTemplate>

                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn DataField="Cell" SortExpression="Cell" AutoPostBackOnFilter="true" HeaderStyle-Width="100"
                                                                                CurrentFilterFunction="Contains" HeaderText="Cell" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCell" runat="server"><%#Eval("Cell")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn DataField="Email" SortExpression="Email" AutoPostBackOnFilter="true" HeaderStyle-Width="150"
                                                                                CurrentFilterFunction="Contains" HeaderText="Email" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <%-- <asp:Label ID="lblEmail" runat="server"><%#Eval("Email")%></asp:Label>--%>
                                                                                    <asp:LinkButton runat="server" OnCommand="lnkEmail_Command" CommandArgument='<%#Eval("Email")%>' CommandName="MailPopup" Style="width: 150px; word-wrap: break-word" ID="lnkEmail"><%#Eval("Email")%></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn DataField="ctype" SortExpression="ctype" AutoPostBackOnFilter="true" HeaderStyle-Width="80"
                                                                                CurrentFilterFunction="Contains" HeaderText="Contact Type" ShowFilterIcon="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblctype" runat="server"><%#Eval("ctype")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>

                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="80" HeaderText="Tickets" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" SortExpression="Tickets">
                                                                                <ItemTemplate>
                                                                                    <div style="text-align: center">
                                                                                        <asp:CheckBox ID="chkTicket" runat="server" Enabled="false" Checked='<%# ((System.Data.DataRowView)Container.DataItem).DataView.Table.Columns.Contains("EmailTicket")== true?Convert.ToBoolean((Eval("EmailTicket")==DBNull.Value ? false:Eval("EmailTicket"))):false%>' />
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Invoice/Statements" ShowFilterIcon="false" HeaderStyle-Width="80" SortExpression="Invoice/Statements">
                                                                                <ItemTemplate>
                                                                                    <div style="text-align: center">
                                                                                        <asp:CheckBox ID="chkInvoice" runat="server" Enabled="false" Checked='<%#  ((System.Data.DataRowView)Container.DataItem).DataView.Table.Columns.Contains("EmailRecInvoice")== true?Convert.ToBoolean((Eval("EmailRecInvoice")==DBNull.Value ? false:Eval("EmailRecInvoice"))):false%>' />
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Shutdown" ShowFilterIcon="false" HeaderStyle-Width="80" SortExpression="Shutdown">
                                                                                <ItemTemplate>
                                                                                    <div style="text-align: center">
                                                                                        <asp:CheckBox ID="chkShutdown" runat="server" Enabled="false" Checked='<%#   ((System.Data.DataRowView)Container.DataItem).DataView.Table.Columns.Contains("ShutdownAlert")== true?Convert.ToBoolean((Eval("ShutdownAlert")==DBNull.Value ? false:Eval("ShutdownAlert"))):false%>' />
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Tests" ShowFilterIcon="false" HeaderStyle-Width="80" SortExpression="Tests">
                                                                                <ItemTemplate>
                                                                                    <div style="text-align: center">
                                                                                        <asp:CheckBox ID="chkTests" runat="server" Enabled="false" Checked='<%#   ((System.Data.DataRowView)Container.DataItem).DataView.Table.Columns.Contains("EmailRecTestProp")== true?Convert.ToBoolean((Eval("EmailRecTestProp")==DBNull.Value ? false:Eval("EmailRecTestProp"))):false%>' />
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                        </Columns>
                                                                    </MasterTableView>

                                                                </telerik:RadGrid>
                                                            </telerik:RadAjaxPanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="two" class="col s12 tab-container-border lighten-4" style="display: none;">
                                <div class="tabs-custom-mgn1">
                                    <div class="row" runat="server" id="CustRemarks">
                                        <div class="input-field col s12">
                                            <div class="row">
                                                <asp:Label runat="server" AssociatedControlID="txtCustomerRemarks">Customer Remarks</asp:Label>
                                                <asp:TextBox ID="txtCustomerRemarks" runat="server" TextMode="MultiLine" MaxLength="8000" AutoCompleteType="Disabled" CssClass="materialize-textarea"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkCustomerRemarks" runat="server" CausesValidation="False" OnClick="lnkCustomerRemarks_Click">Save</asp:LinkButton>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div id="three" class="col s12 tab-container-border lighten-4" style="display: none;">
                                <div class="tabs-custom-mgn1">
                                    <div class="row" runat="server" id="LocRemarks">
                                        <div class="input-field col s12">
                                            <div class="row">
                                                <asp:Label runat="server" AssociatedControlID="txtLocationRemarks">Location Remarks</asp:Label>
                                                <asp:TextBox ID="txtLocationRemarks" runat="server" TextMode="MultiLine" MaxLength="8000" AutoCompleteType="Disabled" CssClass="materialize-textarea"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkLocationRemarks" runat="server" CausesValidation="False" OnClick="lnkLocationRemarks_Click">Save</asp:LinkButton>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div id="four" class="col s12 tab-container-border lighten-4" style="display: none;">
                                <div class="tabs-custom-mgn1">
                                    <div class="form-section-row" runat="server" id="CollectionNotes_1" style="margin-bottom: -10px!important;">
                                        <div class="input-field col s6">
                                            <div class="row" style="margin-bottom: 0px!important;">
                                                <asp:Label runat="server" AssociatedControlID="txtCollectionNote">Add Notes</asp:Label>
                                                <asp:TextBox ID="txtCollectionNote" runat="server" TextMode="MultiLine" MaxLength="8000" AutoCompleteType="Disabled" CssClass="materialize-textarea"></asp:TextBox>
                                               <%-- <asp:RequiredFieldValidator ID="rfvCollectionNote"
                                                    runat="server" ControlToValidate="txtCollectionNote" Display="None" ErrorMessage="Collection Note is required"
                                                    SetFocusOnError="True" ValidationGroup="note"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender
                                                    ID="vceCollectionNote" runat="server" Enabled="True"
                                                    PopupPosition="BottomLeft" TargetControlID="rfvCollectionNote" />--%>
                                            </div>

                                        </div>
                                       
                                        <div class="input-field col s6">
                                            <div class="row" style="margin-bottom: 0px!important;">
                                                <asp:Label runat="server" AssociatedControlID="txtCollectionNote">Default Notes</asp:Label>
                                                <asp:TextBox ID="txtDefaultNotes" runat="server" TextMode="MultiLine" MaxLength="8000" AutoCompleteType="Disabled" CssClass="materialize-textarea"></asp:TextBox>
                                            </div>
                                           
                                        </div>
                                    </div>
                                    <div class="form-section-row">
                                        <div class="input-field col s6">
                                             <asp:CheckBox runat="server" ID="chkShowAllNote"  AutoPostBack ="true" OnCheckedChanged="showAllNote_CheckedChanged"  CssClass="css-checkbox" Text="Show All Notes"/>
                                            </div>
                                         <div class="input-field col s6">
                                            <div class="btnlinks" style="float:right!important; ">
                                                <asp:LinkButton ID="lnkSaveNote" runat="server" OnClick="lnkSaveNote_Click" CausesValidation="true" ValidationGroup="note">Save</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-section-row">
                                        <div class="RadGrid RadGrid_Material  FormGrid">


                                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Customer">
                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_CollectionNotes" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                    PagerStyle-AlwaysVisible="true" OnNeedDataSource="RadGrid_CollectionNotes_NeedDataSource" 
                                                    OnPreRender="RadGrid_CollectionNotes_PreRender"
                                                    OnItemCreated="RadGrid_CollectionNotes_ItemCreated"                                                
                                                    OnUpdateCommand="RadGrid_CollectionNotes_UpdateCommand"
                                                    ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" 
                                                    AllowCustomPaging="True"
                                                    AllowAutomaticUpdates="True"
                                                    AllowAutomaticDeletes="true">
                                                    <CommandItemStyle />
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                        <Columns>
                                                          
                                                             <telerik:GridEditCommandColumn UniqueName="EditCommandColumn"  HeaderStyle-Width="50"></telerik:GridEditCommandColumn>
                                                            <telerik:GridBoundColumn DataField="Notes" HeaderText="Notes"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Notes" 
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                             <telerik:GridBoundColumn DataField="LocName" HeaderText="Location"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Notes"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridBoundColumn DataField="CreatedDate" HeaderText="Date/Time" HeaderStyle-Width="170"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" SortExpression="CreatedDate"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                                 <telerik:GridBoundColumn DataField="CreatedBy" HeaderText="User" HeaderStyle-Width="120"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="CreatedBy"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>
                                                       
                                                              <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="50" AutoPostBackOnFilter="false" ShowFilterIcon="false" AllowFiltering="false"> 
                                                                <ItemTemplate >
                                                                    <asp:ImageButton ID="ibDeleteNote" runat="server" CausesValidation="false" OnClick="ibDeleteNote_Click" data-id='<%# Eval("id") %>'
                                                                        ImageUrl="images/menu_delete.png" Width="13px" OnClientClick='<%# "return ConfirmDelete();" %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                        <EditFormSettings EditFormType="Template">
                                                            <FormTemplate>
                                                                <div class="form-section-row">
                                                                    <div class="input-field col s12">
                                                                        <div class="row" style="margin-bottom: 0px!important;">
                                                                            <asp:Label runat="server" AssociatedControlID="txtNotesUpdate"> Notes</asp:Label>
                                                                            <asp:TextBox ID="txtNotesUpdate" runat="server" TextMode="MultiLine" MaxLength="8000" AutoCompleteType="Disabled" CssClass="materialize-textarea" Text='<%# Eval("Notes") %>'></asp:TextBox>
                                                                            <asp:HiddenField ID="hdnNoteID" runat="server" Value='<%# Eval("ID") %>' />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="btnlinks">
                                                                    <asp:LinkButton ID="lnkUpdateNote" runat="server" CausesValidation="False" CommandName="Update">Save</asp:LinkButton>
                                                                </div>
                                                                <div class="btnlinks">
                                                                    <asp:LinkButton ID="lnkCancelNote" runat="server" CausesValidation="False" CommandName="Cancel">Cancel</asp:LinkButton>
                                                                </div>
                                                            </FormTemplate>
                                                        </EditFormSettings>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </telerik:RadAjaxPanel>
                                        </div>
                                    </div>
                                    <div class="form-section3-blank">
                                        &nbsp;
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="cf"></div>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hdnAddeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewContact" Value="Y" />     
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(function () {
            $("[id*=txtContPhone]").mask("(999) 999-9999? Ext 99999");
            $("[id*=txtContPhone]").bind('paste', function () { $(this).val(''); });
            $("[id*=txtContCell]").mask("(999) 999-9999");
            $("[id*=txtContFax]").mask("(999) 999-9999");
        });       
       
        ///-Contact permission

        function AddContactClick(hyperlink) {
            debugger;
            var IsAdd = document.getElementById('<%= hdnAddeContact.ClientID%>').value;
            if (IsAdd == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function EditContactClick(hyperlink) {
            debugger;
            var IsEdit = document.getElementById('<%= hdnEditeContact.ClientID%>').value;
            if (IsEdit == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function DeleteContactClick(hyperlink) {
            var IsDelete = document.getElementById('<%= hdnDeleteContact.ClientID%>').value;
            if (IsDelete == "Y") {
                return confirm("Are you sure you want to delete this?");
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function RefreshParentPage() {
            top.location.reload();
        }

        $(document).ready(function () {
            $('#ctl00_ContentPlaceHolder1_contactWindow_C_txtContPhone').mask("(999) 999-9999? Ext 99999");
            $('#ctl00_ContentPlaceHolder1_contactWindow_C_txtContPhone').bind('paste', function () { $(this).val(''); });
            $('#ctl00_ContentPlaceHolder1_contactWindow_C_txtContCell').mask("(999) 999-9999");
            $('#ctl00_ContentPlaceHolder1_contactWindow_C_txtContFax').mask("(999) 999-9999");
        });

        $(document).ready(function () {
            //$("#ctl00_ContentPlaceHolder1_mailWindow_C_drpEmailOption").change(function () {
            //    var value = $('select#ctl00_ContentPlaceHolder1_mailWindow_C_drpEmailOption option:selected').val();
            //    if (value == "1") {
            //        $("#InvoiceTemplate").css("display", "block");
            //        $("#CustomerStatement").css("display", "none");
            //    }
            //    else {
            //        $("#InvoiceTemplate").css("display", "none");
            //        $("#CustomerStatement").css("display", "block");
            //    }
            //});
        });

        function pageLoad(sender, args) {
            Materialize.updateTextFields();
        }
         function ConfirmDelete() {
             return confirm('Are you sure you want to delete this note ?');

        }
        function updateparent()
        {
            window.parent.document.getElementById('lnkRefressScreen').click();
        }
     
    </script>

</asp:Content>
