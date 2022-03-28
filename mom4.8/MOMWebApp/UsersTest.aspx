<%@ Page Title="Wages || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="UsersTest" EnableEventValidation="false" CodeBehind="UsersTest.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="grid_container">
            <div class="form-section-row" style="margin-bottom: 0 !important;">
            
                 <div class="form-section-row">
                <div class="btnlinks">
                    <asp:LinkButton ID="lnkAddWage" runat="server" CausesValidation="false" OnClick="lnkAddWage_Click"  >Add</asp:LinkButton>
                    </div>
                    <div class="btnlinks">
                    <asp:LinkButton ID="lnkEditWage" runat="server" CausesValidation="false" OnClick="lnkEditWage_Click" >Edit</asp:LinkButton> 
                    </div>
                    <div class="btnlinks">
                    <asp:LinkButton ID="lnkDeleteWage" runat="server" CausesValidation="false"  OnClick="lnkDeleteWage_Click" OnClientClick="return confirm('Do you really want to delete this Wage ?');" >Delete</asp:LinkButton>
                    </div>
                     <div class="btnlinks">
                    <asp:LinkButton ID="lnkCopyBtn" runat="server" Visible="true" CausesValidation="false"  OnClick="lnkCopyWage_Click" >Copy</asp:LinkButton>
                    </div>
                 </div>


                    <div id="pnlWageGv" class="form-section-row" runat="server">
                    <div class="grid_container">
                    <div class="form-section-row" style="margin-bottom: 0 !important;">
                    <div class="RadGrid RadGrid_Material FormGrid">
                        <telerik:RadCodeBlock ID="RadCodeBlock5" runat="server">
                          <script type="text/javascript">
                        function pageLoad() {
                            var grid = $find("<%= RadGrid_WageSchedules.ClientID %>");
                            var columns = grid.get_masterTableView().get_columns();
                            for (var i = 0; i < columns.length; i++) {
                                columns[i].resizeToFit(false, true);
                            }
                        }

                        var requestInitiator = null;
                        var selectionStart = null;

                        function requestStart(sender, args) {
                            requestInitiator = document.activeElement.id;
                            if (document.activeElement.tagName == "INPUT") {
                                selectionStart = document.activeElement.selectionStart;
                            }
                        }

                        function responseEnd(sender, args) {
                            var element = document.getElementById(requestInitiator);
                            if (element && element.tagName == "INPUT") {
                                element.focus();
                                element.selectionStart = selectionStart;
                            }
                        }
                    </script>
                </telerik:RadCodeBlock>

                <telerik:RadAjaxPanel ID="RadAjaxPanel5" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Setup" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_WageSchedules" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                        OnNeedDataSource="RadGrid_WageSchedule_NeedDataSource" PagerStyle-AlwaysVisible="true"
                        ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true"  Width="100%" AllowCustomPaging="True">
                        <CommandItemStyle />
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                            <Selecting AllowRowSelect="True"></Selecting>
                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                        </ClientSettings>
                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                            <Columns>
                                <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                </telerik:GridClientSelectColumn>
                                <telerik:GridTemplateColumn UniqueName="lblWageId" FilterDelay="5" DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false"
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWageId" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn UniqueName="lblWageFdesc" FilterDelay="5" DataField="fdesc" HeaderText="Description" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fdesc"
                                    ShowFilterIcon="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn UniqueName="lblrem" FilterDelay="5" DataField="Remarks" HeaderText="Remarks" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Remarks"
                                    ShowFilterIcon="false">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
            </div>
            </div>
            </div>
            </div>

             <div id="pnlWageAddEdit" class="form-section-row" runat="server" visible="false">
                                                                                    <div class="btncontainer">
                                                                                        <div class="btnlinks">
                                                                                            <asp:LinkButton ID="lnkWageSave" AutoPostBack="true"  runat="server" ValidationGroup="wage" OnClick="lnkWageSave_Click">Save</asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="btnlinks">
                                                                                            <asp:LinkButton ID="lnkWageClose" AutoPostBack="true" runat="server" OnClick="lnkWageClose_Click" CausesValidation="false">Close</asp:LinkButton>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="section-ttle">General</div>
                                                                                    <asp:HiddenField ID="hdnWageID" runat="server" />
                                                                                    <div class="form-section3">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtDesc">Description</label>

                                                                                                <asp:TextBox ID="txtDesc" runat="server"> </asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="rfvDesc" runat="server" ControlToValidate="txtDesc" ValidationGroup="wage"
                                                                                                    Display="None" ErrorMessage="Description Required" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceDesc" runat="server" Enabled="True"
                                                                                                    TargetControlID="rfvDesc">
                                                                                                </asp:ValidatorCalloutExtender>

                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label>Misc. Exp Account</label>
                                                                                                <asp:TextBox ID="txtGLAcct" autocomplete="off" runat="server" onkeyup="EmptyValue(this);" />
                                                                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtGLAcct"
                                                                                                    EnableCaching="False" ServiceMethod="GetAccounts" UseContextKey="True" MinimumPrefixLength="0"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                                    CompletionListElementID="lstAcct"
                                                                                                    OnClientItemSelected="aceGL_itemSelected"
                                                                                                    ID="AutoCompleteExtender2" DelimiterCharacters="" CompletionInterval="250">
                                                                                                </asp:AutoCompleteExtender>

                                                                                                <asp:CustomValidator ID="cvtxtGLAcct" runat="server" ControlToValidate="txtGLAcct" ValidationGroup="wage"
                                                                                                    ErrorMessage="Please select the existing GL account" ClientValidationFunction="ChkGLAcct"
                                                                                                    Display="None"></asp:CustomValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceGLAcct" runat="server" Enabled="True"
                                                                                                    TargetControlID="cvtxtGLAcct">
                                                                                                </asp:ValidatorCalloutExtender>

                                                                                                <asp:RequiredFieldValidator ID="rfvGLAcct" runat="server" ControlToValidate="txtGLAcct" ValidationGroup="wage"
                                                                                                    Display="None" ErrorMessage="GL Acct Required" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceGLAcct1" runat="server" Enabled="True"
                                                                                                    TargetControlID="rfvGLAcct">
                                                                                                </asp:ValidatorCalloutExtender>

                                                                                                <div id="lstAcct"></div>
                                                                                                <asp:HiddenField ID="hdnGLAcct" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtMileageAcct">Mileage Exp. Account</label>
                                                                                                <asp:TextBox ID="txtMileageAcct" runat="server" autocomplete="off" onkeyup="EmptyValue(this);" />
                                                                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtMileageAcct"
                                                                                                    EnableCaching="False" ServiceMethod="GetAccounts" UseContextKey="True" MinimumPrefixLength="0"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                                    CompletionListElementID="lstMilegAcct"
                                                                                                    OnClientItemSelected="aceMil_itemSelected"
                                                                                                    ID="AutoCompleteExtender3" DelimiterCharacters="" CompletionInterval="250">
                                                                                                </asp:AutoCompleteExtender>
                                                                                                <div id="lstMilegAcct"></div>
                                                                                                <asp:HiddenField ID="hdnMilegAcct" runat="server" />

                                                                                                <asp:CustomValidator ID="cvMilegAcct" runat="server" ControlToValidate="txtMileageAcct" ValidationGroup="wage"
                                                                                                    ErrorMessage="Please select the existing GL account" ClientValidationFunction="ChkMilAcct"
                                                                                                    Display="None"></asp:CustomValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceMilegAcct" runat="server" Enabled="True"
                                                                                                    TargetControlID="cvMilegAcct">
                                                                                                </asp:ValidatorCalloutExtender>

                                                                                                <asp:RequiredFieldValidator ID="rfvMilegAcct" runat="server" ControlToValidate="txtMileageAcct" ValidationGroup="wage"
                                                                                                    Display="None" ErrorMessage="Mileage Acct Required" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceMilegAcct1" runat="server" Enabled="True"
                                                                                                    TargetControlID="rfvMilegAcct">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtReimbAcct">Reimb. Exp Account</label>
                                                                                                <asp:TextBox ID="txtReimbAcct" runat="server" autocomplete="off" onkeyup="EmptyValue(this);" />
                                                                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtReimbAcct"
                                                                                                    EnableCaching="False" ServiceMethod="GetAccounts" UseContextKey="True" MinimumPrefixLength="0"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                                    CompletionListElementID="lstReimbAcct"
                                                                                                    OnClientItemSelected="aceReimb_itemSelected"
                                                                                                    ID="AutoCompleteExtender4" DelimiterCharacters="" CompletionInterval="250">
                                                                                                </asp:AutoCompleteExtender>
                                                                                                <div id="lstReimbAcct"></div>
                                                                                                <asp:HiddenField ID="hdnReimbAcct" runat="server" />

                                                                                                <asp:CustomValidator ID="cvReimbAcct" runat="server" ControlToValidate="txtReimbAcct" ValidationGroup="wage"
                                                                                                    ErrorMessage="Please select the existing GL account" ClientValidationFunction="ChkReimbAcct"
                                                                                                    Display="None"></asp:CustomValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceReimbAcct" runat="server" Enabled="True"
                                                                                                    TargetControlID="cvReimbAcct">
                                                                                                </asp:ValidatorCalloutExtender>

                                                                                                <asp:RequiredFieldValidator ID="rfvReimbAcct" runat="server" ControlToValidate="txtReimbAcct" ValidationGroup="wage"
                                                                                                    Display="None" ErrorMessage="Reimb Acct Required" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceReimbAcct1" runat="server" Enabled="True"
                                                                                                    TargetControlID="rfvReimbAcct">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtZoneAcct">Zone Exp. Account</label>
                                                                                                <asp:TextBox ID="txtZoneAcct" runat="server" autocomplete="off" onkeyup="EmptyValue(this);" />

                                                                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtZoneAcct"
                                                                                                    EnableCaching="False" ServiceMethod="GetAccounts" UseContextKey="True" MinimumPrefixLength="0"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                                    CompletionListElementID="lstZoneAcct"
                                                                                                    OnClientItemSelected="aceZone_itemSelected"
                                                                                                    ID="AutoCompleteExtender5" DelimiterCharacters="" CompletionInterval="250">
                                                                                                </asp:AutoCompleteExtender>
                                                                                                <div id="lstZoneAcct"></div>
                                                                                                <asp:HiddenField ID="hdnZoneAcct" runat="server" />

                                                                                                <asp:CustomValidator ID="cvZone" runat="server" ControlToValidate="txtZoneAcct" ValidationGroup="wage"
                                                                                                    ErrorMessage="Please select the existing GL account" ClientValidationFunction="ChkZoneAcct"
                                                                                                    Display="None"></asp:CustomValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceZoneAcct" runat="server" Enabled="True"
                                                                                                    TargetControlID="cvZone">
                                                                                                </asp:ValidatorCalloutExtender>

                                                                                                <asp:RequiredFieldValidator ID="rfvZoneAcct" runat="server" ControlToValidate="txtZoneAcct" ValidationGroup="wage"
                                                                                                    Display="None" ErrorMessage="Zone Acct Required" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceZoneAcct1" runat="server" Enabled="True"
                                                                                                    TargetControlID="rfvZoneAcct">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s4">
                                                                                            <div class="row">
                                                                                                <label class="drpdwn-label">Status</label>
                                                                                                <asp:DropDownList ID="ddlWageStatus" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s4">
                                                                                            <div class="row">
                                                                                                <label class="drpdwn-label">Global</label>
                                                                                                <asp:DropDownList ID="ddlGlobal" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2 mgntp10">
                                                                                            <div class="row">
                                                                                                <asp:CheckBox ID="chkField" Text="Field" CssClass="css-checkbox" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>

                                                                                    <div class="section-ttle">Rates</div>
                                                                                    <div class="form-section3">
                                                                                        <div class="section-ttle">Pay Rate</div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtRegularRate">Regular</label>
                                                                                                <asp:TextBox ID="txtRegularRate" runat="server" Style="text-align: right" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtOvertimeRate">Overtime</label>
                                                                                                <asp:TextBox ID="txtOvertimeRate" runat="server" Style="text-align: right" />
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtTime">1.7 Time</label>
                                                                                                <asp:TextBox ID="txtTime" runat="server" Style="text-align: right" />
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtDoubleTime">Double Time</label>
                                                                                                <asp:TextBox ID="txtDoubleTime" runat="server" Style="text-align: right" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtTravelTime">Travel Time</label>
                                                                                                <asp:TextBox ID="txtTravelTime" runat="server" Style="text-align: right" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-sectioncustom1">
                                                                                        <div class="form-section2">
                                                                                            <div class="section-ttle">Burden Rate</div>
                                                                                        </div>
                                                                                        <div class="form-section3-blank">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="form-section2">

                                                                                            <div class="section-ttle">GL Exp Account</div>

                                                                                        </div>
                                                                                        <div class="input-field col s6-small">
                                                                                            <div class="row">
                                                                                                <label for="txtCReg">Regular</label>
                                                                                                <asp:TextBox ID="txtCReg" runat="server" Style="text-align: right" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s-gap">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s6-small">
                                                                                            <div class="row">
                                                                                                <asp:TextBox ID="txtRegGL" runat="server" autocomplete="off" placeholder="Search by acct# and name" onkeyup="EmptyValue(this);" />
                                                                                                <asp:HiddenField ID="hdnRegGL" runat="server" />
                                                                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtRegGL"
                                                                                                    EnableCaching="False" ServiceMethod="GetAccounts" UseContextKey="True" MinimumPrefixLength="0"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                                    CompletionListElementID="lstRegGL"
                                                                                                    OnClientItemSelected="aceReg_itemSelected"
                                                                                                    ID="AutoCompleteExtender6" DelimiterCharacters="" CompletionInterval="250">
                                                                                                </asp:AutoCompleteExtender>
                                                                                                <div id="lstRegGL"></div>
                                                                                                <asp:CustomValidator ID="cvRegGL" runat="server" ControlToValidate="txtRegGL" ValidationGroup="wage"
                                                                                                    ErrorMessage="Please select the existing GL account" ClientValidationFunction="ChkRegGL"
                                                                                                    Display="None"></asp:CustomValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceRegGL" runat="server" Enabled="True"
                                                                                                    TargetControlID="cvRegGL" PopupPosition="Right">
                                                                                                </asp:ValidatorCalloutExtender>

                                                                                                <asp:RequiredFieldValidator ID="rfvRegGL" runat="server" ControlToValidate="txtRegGL" ValidationGroup="wage"
                                                                                                    Display="None" ErrorMessage="Reg GL Required" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceRegGL1" runat="server" Enabled="True"
                                                                                                    TargetControlID="rfvRegGL" PopupPosition="Right">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s6-small">
                                                                                            <div class="row">
                                                                                                <label for="txtCOT">Overtime</label>
                                                                                                <asp:TextBox ID="txtCOT" runat="server" Style="text-align: right" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s-gap">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s6-small">
                                                                                            <div class="row">
                                                                                                <asp:TextBox ID="txtOTGL" runat="server" autocomplete="off" placeholder="Search by acct# and name" onkeyup="EmptyValue(this);" />
                                                                                                <asp:HiddenField ID="hdnOTGL" runat="server" />
                                                                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtOTGL"
                                                                                                    EnableCaching="False" ServiceMethod="GetAccounts" UseContextKey="True" MinimumPrefixLength="0"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                                    CompletionListElementID="lstOTGL"
                                                                                                    OnClientItemSelected="aceOT_itemSelected"
                                                                                                    ID="AutoCompleteExtender7" DelimiterCharacters="" CompletionInterval="250">
                                                                                                </asp:AutoCompleteExtender>
                                                                                                <div id="lstOTGL"></div>
                                                                                                <asp:CustomValidator ID="cvOTGL" runat="server" ControlToValidate="txtOTGL" ValidationGroup="wage"
                                                                                                    ErrorMessage="Please select the existing GL account" ClientValidationFunction="ChkOTGL"
                                                                                                    Display="None"></asp:CustomValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vcOTGL" runat="server" Enabled="True"
                                                                                                    TargetControlID="cvOTGL" PopupPosition="BottomLeft">
                                                                                                </asp:ValidatorCalloutExtender>

                                                                                                <asp:RequiredFieldValidator ID="rfvOT" runat="server" ControlToValidate="txtOTGL" ValidationGroup="wage"
                                                                                                    Display="None" ErrorMessage="OT GL Required" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceOTGL1" runat="server" Enabled="True"
                                                                                                    TargetControlID="rfvOT" PopupPosition="BottomLeft">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s6-small">
                                                                                            <div class="row">
                                                                                                <label for="txtCNT">1.7 Time</label>
                                                                                                <asp:TextBox ID="txtCNT" runat="server" Style="text-align: right" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s-gap">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s6-small">
                                                                                            <div class="row">
                                                                                                <asp:TextBox ID="txtNTGL" runat="server" autocomplete="off" placeholder="Search by acct# and name" onkeyup="EmptyValue(this);" />
                                                                                                <asp:HiddenField ID="hdnNTGL" runat="server" />
                                                                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtNTGL"
                                                                                                    EnableCaching="False" ServiceMethod="GetAccounts" UseContextKey="True" MinimumPrefixLength="0"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                                    CompletionListElementID="lstNTGL"
                                                                                                    OnClientItemSelected="aceNT_itemSelected"
                                                                                                    ID="AutoCompleteExtender8" DelimiterCharacters="" CompletionInterval="250">
                                                                                                </asp:AutoCompleteExtender>
                                                                                                <div id="lstNTGL"></div>
                                                                                                <asp:CustomValidator ID="cvNTGL" runat="server" ControlToValidate="txtNTGL" ValidationGroup="wage"
                                                                                                    ErrorMessage="Please select the existing GL account" ClientValidationFunction="ChkRegGL"
                                                                                                    Display="None"></asp:CustomValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceNTGL" runat="server" Enabled="True"
                                                                                                    TargetControlID="cvNTGL" PopupPosition="BottomLeft">
                                                                                                </asp:ValidatorCalloutExtender>

                                                                                                <asp:RequiredFieldValidator ID="rfvNTGL" runat="server" ControlToValidate="txtNTGL" ValidationGroup="wage"
                                                                                                    Display="None" ErrorMessage="NT GL Required" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceNTGL1" runat="server" Enabled="True"
                                                                                                    TargetControlID="rfvNTGL" PopupPosition="BottomLeft">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s6-small">
                                                                                            <div class="row">
                                                                                                <label for="txtCDT">Double Time</label>
                                                                                                <asp:TextBox ID="txtCDT" runat="server" Style="text-align: right" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s-gap">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s6-small">
                                                                                            <div class="row">
                                                                                                <asp:TextBox ID="txtDTGL" runat="server" autocomplete="off" placeholder="Search by acct# and name" onkeyup="EmptyValue(this);" />
                                                                                                <asp:HiddenField ID="hdnDTGL" runat="server" />
                                                                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtDTGL"
                                                                                                    EnableCaching="False" ServiceMethod="GetAccounts" UseContextKey="True" MinimumPrefixLength="0"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                                    CompletionListElementID="lstDTGL"
                                                                                                    OnClientItemSelected="aceDT_itemSelected"
                                                                                                    ID="AutoCompleteExtender9" DelimiterCharacters="" CompletionInterval="250">
                                                                                                </asp:AutoCompleteExtender>
                                                                                                <div id="lstDTGL"></div>
                                                                                                <asp:CustomValidator ID="cvDTGL" runat="server" ControlToValidate="txtDTGL" ValidationGroup="wage"
                                                                                                    ErrorMessage="Please select the existing GL account" ClientValidationFunction="ChkDTGL"
                                                                                                    Display="None"></asp:CustomValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceDTGL" runat="server" Enabled="True"
                                                                                                    TargetControlID="cvDTGL" PopupPosition="BottomLeft">
                                                                                                </asp:ValidatorCalloutExtender>

                                                                                                <asp:RequiredFieldValidator ID="rfvDTGL" runat="server" ControlToValidate="txtDTGL" ValidationGroup="wage"
                                                                                                    Display="None" ErrorMessage="DT GL Required" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceDTGL1" runat="server" Enabled="True"
                                                                                                    TargetControlID="rfvDTGL" PopupPosition="BottomLeft">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s6-small">
                                                                                            <div class="row">
                                                                                                <label for="txtCTT">Travel Time</label>
                                                                                                <asp:TextBox ID="txtCTT" runat="server" Style="text-align: right" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s-gap">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s6-small">
                                                                                            <div class="row">
                                                                                                <asp:TextBox ID="txtTTGL" CssClass="form-control txtFont" runat="server" autocomplete="off" placeholder="Search by acct# and name" onkeyup="EmptyValue(this);" />
                                                                                                <asp:HiddenField ID="hdnTTGL" runat="server" />
                                                                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtTTGL"
                                                                                                    EnableCaching="False" ServiceMethod="GetAccounts" UseContextKey="True" MinimumPrefixLength="0"
                                                                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                                    CompletionListElementID="lstTTGL"
                                                                                                    OnClientItemSelected="aceTT_itemSelected"
                                                                                                    ID="AutoCompleteExtender10" DelimiterCharacters="" CompletionInterval="250">
                                                                                                </asp:AutoCompleteExtender>
                                                                                                <div id="lstTTGL"></div>
                                                                                                <asp:CustomValidator ID="cvTTGL" runat="server" ControlToValidate="txtTTGL" ValidationGroup="wage"
                                                                                                    ErrorMessage="Please select the existing GL account" ClientValidationFunction="ChkTTGL"
                                                                                                    Display="None"></asp:CustomValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceTTGL" runat="server" Enabled="True"
                                                                                                    TargetControlID="cvTTGL" PopupPosition="BottomLeft">
                                                                                                </asp:ValidatorCalloutExtender>

                                                                                                <asp:RequiredFieldValidator ID="rfvTTGL" runat="server" ControlToValidate="txtTTGL" ValidationGroup="wage"
                                                                                                    Display="None" ErrorMessage="TT GL Required" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                                                                <asp:ValidatorCalloutExtender ID="vceTTGL1" runat="server" Enabled="True"
                                                                                                    TargetControlID="rfvTTGL" PopupPosition="BottomLeft">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>


                                                                                    <div class="form-section3">
                                                                                        <div class="section-ttle" style="margin-bottom: 8px;">Subject To</div>

                                                                                        <div class="input-field col s5 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkFIT" CssClass="css-checkbox" Text="FIT" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkSIT" CssClass="css-checkbox" Text="SIT" runat="server" />
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s5 mgntp10" style="clear: both;">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkFICA" CssClass="css-checkbox" Text="FICA" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkVacation" CssClass="css-checkbox" Text="Vacation" runat="server" />
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s5 mgntp10" style="clear: both;">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkMEDI" CssClass="css-checkbox" Text="MEDI" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkWorkComp" CssClass="css-checkbox" Text="Work Comp" runat="server" />
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s5 mgntp10" style="clear: both;">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkFUTA" CssClass="css-checkbox" Text="FUTA" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkUnion" CssClass="css-checkbox" Text="UNION" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>

                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-sectioncustom1">
                                                                                        <div class="section-ttle">Remarks</div>
                                                                                        <asp:TextBox ID="txtRemark" runat="server" class="materialize-textarea" TextMode="MultiLine" MaxLength="8000"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
            </div>
        </div>
   
</asp:Content>
