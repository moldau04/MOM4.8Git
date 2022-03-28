<%@ Page Title="" Language="C#" MasterPageFile="~/MOM.master" AutoEventWireup="true" Inherits="AddDeposit" CodeBehind="AddDeposit.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <%--MAKE DEPOSIT--%>
    <script type="text/javascript" src="js/jquery.formatCurrency.js"></script>
    <script type="text/javascript">
        function makeReadonly(txt) {
            $("#" + txt.id).prop('readonly', true);
        }


    </script>

    <style>
        .rptSti tr:nth-child(2n+1) {
            background: none !important;
        }

        .rm_mm td {
            background-color: none !important;
        }
         #overlay {
            position: fixed; /* Sit on top of the page content */
            display: none; /* Hidden by default */
            width: 100%; /* Full width (cover the whole page) */
            height: 100%; /* Full height (cover the whole page) */
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0,0,0,0.5); /* Black background with opacity */
            z-index: 1000000; /* Specify a stack order in case you're using a different order for other elements */
            cursor: pointer; /* Add a pointer on hover */
        }
         .alert
         {
             padding-bottom:14px;
         }
         .close{
             width:28px;
         }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        Sys.Application.add_init(appl_init);

        function appl_init() {
            var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
            pgRegMgr.add_beginRequest(BlockUI);
            pgRegMgr.add_endRequest(UnblockUI);
        }

        function BlockUI(sender, args) {
            document.getElementById("overlay").style.display = "block";
        }
        function UnblockUI(sender, args) {
            document.getElementById("overlay").style.display = "none";
        }
            function convertNumber(obj) {
            var expression = /[\(\)]/g;

            if (obj.match(expression))     /// check is parentheses exists (negative value)
            {
                return parseFloat(obj.replace(/[\$\(\),]/g, '')) * (-1);
            }
            else {
                return parseFloat(obj.replace(/[\$\(\),]/g, ''));
            }

        }
    </script>
    <div id="overlay">
        <img src="images/wheel.GIF" alt="Be patient..." class="lodder" />
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager_Deposit" runat="server">
        <AjaxSettings>

            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvDepositGL"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="btnSubmit"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvReceivePayment"></telerik:AjaxUpdatedControl>

                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-action-account-balance"></i>&nbsp;
                                        <asp:Label ID="lblHeader" runat="server">Make Deposit</asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ToolTip="Save" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ValidationGroup="Deposit" OnClientClick="return Confirm();">Save</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkPrint" runat="server" ToolTip="Print" ValidationGroup="Deposit" OnClientClick="return DisableButton()" CausesValidation="true" OnClick="lnkPrint_Click">Print </asp:LinkButton>

                                        </div>                                     
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                            OnClick="lnkClose_Click">   <i class="mdi-content-clear"></i></asp:LinkButton>

                                    </div>
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label ID="lblRef" runat="server" Text="Ref #" Visible="False"></asp:Label>
                                            <asp:Label ID="lblRefId" runat="server" Visible="False"></asp:Label>
                                        </div>

                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </header>
            </div>

            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <div class="tblnks">
                                <ul class="anchor-links">
                                    <li runat="server" id="liGLAccount"><a href="#accrdGlAccount">GL Account</a></li>
                                    <li runat="server" id="liReceiptPay"><a href="#accrdPayment">Payment Info</a></li>
                                    <li runat="server" id="liInvoice"><a href="#accrdInvoice">Invoice Info</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlNext" runat="server" Visible="false">
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False"
                                                OnClick="lnkFirst_Click">
                                          <i class="fa fa-angle-double-left"></i> </asp:LinkButton></span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False"
                                                OnClick="lnkPrevious_Click">
                                            <i class="fa fa-angle-left"></i></asp:LinkButton></span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False"
                                                OnClick="lnkNext_Click">
                                           <i class="fa fa-angle-right"></i> </asp:LinkButton></span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False"
                                                OnClick="lnkLast_Click">
                                           <i class="fa fa-angle-double-right"></i> </asp:LinkButton></span>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="srchpane-advanced" >
                <div class="card-content">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <div style="display: block;">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <asp:HiddenField ID="hdnTransId" runat="server" />
                                             <asp:HiddenField ID="hdnIsRecon" runat="server" />
                                            <div class="alert alert-success" runat="server" id="divSuccess">
                                                <button type="button" class="close" data-dismiss="alert">×</button>
                                                These month/year period is closed out. You do not have permission to add/update this record.
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Bank</label>
                                                        <asp:DropDownList ID="ddlBank" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvBank" ControlToValidate="ddlBank"
                                                            ErrorMessage="Please select Bank" Display="None" InitialValue="0"
                                                            ValidationGroup="Deposit"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceBank" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                            TargetControlID="rfvBank" />


                                                    </div>


                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field2 col s5">
                                                    <div class="row">
                                                        <label for="txtDate">Date</label>
                                                        <asp:TextBox ID="txtDateDeposite" runat="server" autocomplete="off"></asp:TextBox>

                                                        <asp:RequiredFieldValidator runat="server" ID="rfvDate" ControlToValidate="txtDateDeposite"
                                                            ErrorMessage="Please enter Date." Display="None"
                                                            ValidationGroup="Deposit"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True" PopupPosition="Right"
                                                            TargetControlID="rfvDate" />
                                                        <asp:Button runat="server" ID="btnDateDeposite" Style="display: none;" CausesValidation="false" OnClick="btnDateDeposite_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtMemo" runat="server"
                                                            autocomplete="off"></asp:TextBox>
                                                        <asp:RequiredFieldValidator runat="server" ID="rfvMemo" ControlToValidate="txtMemo"
                                                            ErrorMessage="Please enter Memo." Display="None"
                                                            ValidationGroup="Deposit"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceMemo" runat="server" Enabled="True" PopupPosition="Right"
                                                            TargetControlID="rfvMemo" />
                                                        <label for="txtMemo">Memo </label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:UpdatePanel runat="server" ID="UpdDeposit" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <label class="active">Deposit Total</label>
                                                                <asp:Label ID="lblDepositTotal" runat="server" class="main-css" ></asp:Label>
                                                                <asp:HiddenField runat="server" Value="0.00" ID="hdDepositTotal" />

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s5 new-css">
                                                    <asp:Image ID="imgCleared" runat="server" ImageUrl="~/images/icons/Cleared.png" />
                                                </div>
                                                <div class="input-field col s1">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s6 pt-12" >
                                                    <asp:Label ID="lblRecordCount" Text="" runat="server"></asp:Label>

                                                </div>
                                            </div>

                                        </div>
                                        <div class="cf"></div>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>

            </div>
            <%--Add Area--%>
            <div>
                <div class="container accordian-wrap">
                    <div class="row">
                        <div class="col s12 m12 l12">
                            <div class="row">
                                <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">

                                    <li>
                                        <div id="accrdGlAccount" class="collapsible-header accrd accordian-text-custom "><i class="mdi-action-info"></i>GL Account Info</div>
                                        <div class="collapsible-body">
                                            <div class="form-content-wrap">
                                                <div class="form-content-pd">
                                                    <div class="form-section-row">
                                                        <div class="grid_container">
                                                            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
                                                            </telerik:RadAjaxLoadingPanel>
                                                            <div class="form-section-row mb" >
                                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                                                        <script type="text/javascript">
                                                                            function pageLoad() {
                                                                                var grid = $find("<%= RadAjaxPanel_gvDepositGL.ClientID %>");
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
                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvDepositGL" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" 
                                                                      >
                                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvDepositGL" Visible="true"
                                                                            OnNeedDataSource="RadGrid_gvDepositGL_NeedDataSource"
                                                                              OnPreRender="RadGrid_gvDepositGL_PreRender"
                                                                            runat="server" AllowPaging="false" AllowSorting="true" Width="100%" AllowFilteringByColumn="true" ShowStatusBar="true" FilterType="CheckList" AllowMultiRowSelection="true">
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ClientSettings AllowColumnsReorder="true" EnableAlternatingItems="false" ReorderColumnsOnClient="true">
                                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>

                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" ShowFooter="True" AllowFilteringByColumn="true">
                                                                                <Columns>
                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="40" AllowFiltering="false" ItemStyle-Width="25px" FooterStyle-Width="25px">
                                                                                        <HeaderTemplate>
                                                                                            <asp:LinkButton ID="ibtnDeleteGLRow" OnClientClick="return confirm('Are you sure you want to delete selected rows?')"
                                                                                                CausesValidation="false" ToolTip="Delete" runat="server" Style="color: #000; font-size: 1.5em; top: 0px; padding: 0!important;" Width="20px"
                                                                                                OnClick="ibtnDeleteGLRow_Click" mouseup="CalCountAmount();"><i class="mdi-navigation-cancel button-cancel" style="padding" ></i></asp:LinkButton>

                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnOrderNo" Value='1' runat="server"></asp:HiddenField>
                                                                                            <asp:Label ID="lblIndex" Visible="false" runat="server" Text="<%# Container.ItemIndex +1 %>"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkGLAddnewRow" runat="server" CausesValidation="False" Style="color: #000; font-size: 1.5em; padding: 0!important" Width="20px"
                                                                                                ToolTip="Add New Row" mouseup="CalCountAmount();" OnClick="lnkGLAddnewRow_Click"><i class="mdi-content-add-circle add-button" ></i></asp:LinkButton>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" ItemStyle-Width="40px" HeaderStyle-Width="40px" Exportable="false">

                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkSelect" runat="server" class="css-checkbox" Text=" " CommandName="ClearRow" Checked='false' />
                                                                                            <asp:HiddenField ID="hdnChk" runat="server" Value="" />
                                                                                            <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ID")%>' />
                                                                                            <asp:HiddenField runat="server" ID="hdnTransID" Value='<%# Eval("TransID")%>' />
                                                                                            <asp:HiddenField runat="server" ID="hdnTitle" Value='<%# Eval("fTitle")%>' />
                                                                                           
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <%-- <telerik:GridTemplateColumn AllowFiltering="true" ShowFilterIcon="false" HeaderText="Tag" SortExpression="Tag" DataField="Tag" AutoPostBackOnFilter="true" DataType="System.String" AndCurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTag" runat="server" Text='<%# Eval("Tag") ==string.Empty ? "" : Eval("Tag") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>--%>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="150" DataField="Acct" SortExpression="Acct" AllowFiltering="true" 
                                                                                        HeaderText="Acct" ShowFilterIcon="false" AutoPostBackOnFilter="true" DataType="System.String" AndCurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>

                                                                                            <asp:TextBox ID="txtAcct" runat="server" CssClass="searchinput_glAccount "
                                                                                                onkeypress="return isNumberKey(event,this)" placeholder="Search by Acct#" Text='<%# Eval("Acct")%>' Visible='<%# Eval("TransID").ToString() =="0" ? true : false %>'>
                                                                                            </asp:TextBox>

                                                                                            <asp:Label ID="lblAcct" runat="server" CssClass="searchinput_glAccount "
                                                                                                Text='<%# Eval("Acct")%>' Visible='<%# Eval("TransID").ToString() =="0" ? false : true %>'>
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="200" AllowFiltering="true" ShowFilterIcon="false" HeaderText="Account Description" SortExpression="Title" DataField="fTitle" AutoPostBackOnFilter="true" DataType="System.String" AndCurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("fTitle") ==string.Empty ? "" : Eval("fTitle") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn AllowFiltering="true" ShowFilterIcon="false" HeaderText="Description" SortExpression="Desc" DataField="fDesc" AutoPostBackOnFilter="true" DataType="System.String" AndCurrentFilterFunction="Contains" HeaderStyle-Width="500">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtDesc" runat="server" Text='<%# Eval("fDesc") ==string.Empty ? "" : Eval("fDesc")  + (((System.Data.DataRowView)Container.DataItem).DataView.Table.Columns.Contains("tag")== true?(Convert.ToString(DataBinder.Eval(Container.DataItem, "tag"))==""?"":"."+Convert.ToString(DataBinder.Eval(Container.DataItem, "tag"))):"")%>'></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblTotal" runat="server" Text='Total:'></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="200" AllowFiltering="true" ShowFilterIcon="false" HeaderText="Ref #'s" SortExpression="Tag" AutoPostBackOnFilter="true" DataType="System.String" AndCurrentFilterFunction="Contains" Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtRef" runat="server" Text='<%# Eval("Ref") ==string.Empty ? "" : Eval("Ref") %>'>
                                                                                            </asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="200" AllowFiltering="true" ShowFilterIcon="false" DataField="amount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderText="Amount" SortExpression="amount" AndCurrentFilterFunction="EqualTo" AutoPostBackOnFilter="true"  DataType="System.Double">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtGLAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>'  ReadOnly ='<%# Eval("TransID").ToString() =="0" ? false : true %>'></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblTotalGLAmount" runat="server" Text=''></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                            </FilterMenu>
                                                                        </telerik:RadGrid>
                                                                    </telerik:RadAjaxPanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="adEditPayment" runat="server">
                                        <div id="accrdPayment" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-action-info"></i>Payment Info</div>
                                        <div class="collapsible-body">
                                            <div class="form-content-wrap">
                                                <div class="form-content-pd">
                                                    <div class="form-section-row">
                                                        <div class="grid_container">
                                                            <div class="form-section-row mb" >
                                                               <%-- <telerik:RadAjaxManager ID="RadAjaxManager_Deposit" runat="server">
                                                                    <AjaxSettings>
                                                                        <telerik:AjaxSetting AjaxControlID="RadGrid_gvReceivePayment">
                                                                            <UpdatedControls>
                                                                                <telerik:AjaxUpdatedControl ControlID="RadGrid_gvDeposit"></telerik:AjaxUpdatedControl>
                                                                            </UpdatedControls>
                                                                        </telerik:AjaxSetting>
                                                                    </AjaxSettings>
                                                                </telerik:RadAjaxManager>--%>
                                                                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Deposit" runat="server">
                                                                </telerik:RadAjaxLoadingPanel>
                                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                                    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                                                        <script type="text/javascript"></script>
                                                                    </telerik:RadCodeBlock>
                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvReceivePayment" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Deposit">
                                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvReceivePayment" OnNeedDataSource="RadGrid_gvReceivePayment_NeedDataSource"
                                                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%">
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ClientSettings AllowColumnsReorder="true" EnableAlternatingItems="false" ReorderColumnsOnClient="true">
                                                                                <Selecting AllowRowSelect="True"></Selecting>

                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>

                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" ShowFooter="True">
                                                                                <Columns>


                                                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" HeaderStyle-Width="50"
                                                                                        ShowFilterIcon="false">
                                                                                        <HeaderTemplate>
                                                                                            <%--<asp:CheckBox ID="ChkAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true" />--%>
                                                                                            <asp:CheckBox ID="ChkAll" runat="server" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnID" Value='<%# Bind("ID") %>' runat="server" />
                                                                                            <asp:HiddenField ID="hdnSelected" runat="server" />
                                                                                            <%--<asp:CheckBox ID="chkSelect" runat="server" OnCheckedChanged="chkSelect_CheckedChanged" AutoPostBack="true" />--%>
                                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblSelectedItems" runat="server" Text="Selected Item(s) 0"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridBoundColumn DataField="PaymentReceivedDate" AutoPostBackOnFilter="true"
                                                                                        HeaderText="Date" SortExpression="Date"
                                                                                        UniqueName="Date" DataFormatString="{0:MM/dd/yy}" HeaderStyle-Width="150">
                                                                                    </telerik:GridBoundColumn>

                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="Customer Name" SortExpression="customerName" UniqueName="customerName" HeaderStyle-Width="200">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("customerName") ==string.Empty ? "" : Eval("customerName") %>'></asp:Label>
                                                                                        </ItemTemplate>

                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="Location" SortExpression="Tag" UniqueName="Location" HeaderStyle-Width="150">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTag" runat="server" Text='<%# Eval("Tag") ==string.Empty ? "" : Eval("Tag") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" Display="false" ShowFilterIcon="false" HeaderText="Description" UniqueName="Description" HeaderStyle-Width="250">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="250"
                                                                                                autocomplete="off"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="Company" SortExpression="Company" UniqueName="Company" HeaderStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblCompany" runat="server" Text='<%# Bind("Company") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridBoundColumn DataField="CheckNumber" AutoPostBackOnFilter="true"
                                                                                        HeaderText="Check No." SortExpression="CheckNumber"
                                                                                        UniqueName="CheckNumber" HeaderStyle-Width="100">
                                                                                    </telerik:GridBoundColumn>

                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" DataField="PaymentMethod" HeaderText="Payment Method" SortExpression="PaymentMethod" HeaderStyle-Width="150">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPaymentMethod" runat="server" Text='<%# Bind("PaymentMethod") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblDate" runat="server" Text='Total:-'></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn AllowFiltering="true" ShowFilterIcon="false" HeaderStyle-Width="150" DataField="amount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderText="Amount" SortExpression="amount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAmount" runat="server"
                                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                            </FilterMenu>
                                                                        </telerik:RadGrid>

                                                                    </telerik:RadAjaxPanel>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                        </div>

                                    </li>
                                    <li id="adEditInvoice" runat="server">

                                        <div id="accrdInvoice" class="collapsible-header accrd  accordian-text-custom "><i class="mdi-action-info"></i>Invoices Info</div>
                                        <div class="collapsible-body">
                                            <div class="form-content-wrap">
                                                <div class="form-content-pd">
                                                    <div class="form-section-row">
                                                        <div class="grid_container">
                                                            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server">
                                                            </telerik:RadAjaxLoadingPanel>
                                                            <div class="form-section-row mb">
                                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                                    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                                                                        <script type="text/javascript">
                                                                            function pageLoad() {
                                                                                var grid = $find("<%= RadAjaxPanel_gvInvoiceDesposit.ClientID %>");
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
                                                                                    if (selectionStart != null) {
                                                                                         element.selectionStart = selectionStart;
                                                                                    }
                                                                                   
                                                                                }
                                                                            }
                                                                        </script>
                                                                    </telerik:RadCodeBlock>
                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvInvoiceDesposit" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
                                                                        ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvInvoiceDeposit" Visible="false"
                                                                            OnNeedDataSource="RadGrid_gvInvoiceDeposit_NeedDataSource"
                                                                            OnItemDataBound="RadGrid_gvInvoiceDeposit_ItemDataBound"
                                                                            OnPreRender="RadGrid_gvInvoiceDeposit_PreRender"
                                                                            runat="server" AllowPaging="false" AllowSorting="true" Width="100%" AllowFilteringByColumn="true" ShowStatusBar="true" FilterType="CheckList">
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ClientSettings AllowColumnsReorder="true" EnableAlternatingItems="false" ReorderColumnsOnClient="true">
                                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>

                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" ShowFooter="True" AllowFilteringByColumn="true">
                                                                                <Columns>
                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="50" AllowFiltering="false" ItemStyle-Width="0.5%" FooterStyle-Width="0.5%">
                                                                                        <HeaderTemplate>
                                                                                            <asp:LinkButton ID="ibtnDeleteInvoiceRow" OnClientClick="return confirm('Are you sure you want to delete selected rows?')"
                                                                                                CausesValidation="false" ToolTip="Delete" runat="server" Style="color: #000; font-size: 1.5em; top: 0px; padding: 0;" Width="20px"
                                                                                                OnClick="ibtnDeleteInvoiceRow_Click" mouseup="CalCountAmount();"><i class="mdi-navigation-cancel" style=" color: #f00;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>

                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnOrderNo" Value='1' runat="server"></asp:HiddenField>
                                                                                            <asp:Label ID="lblIndex" Visible="false" runat="server" Text="<%# Container.ItemIndex +1 %>"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:LinkButton ID="lnkAddnewRow" runat="server" CausesValidation="False" Style="color: #000; font-size: 1.5em; padding: 0" Width="20px"
                                                                                                ToolTip="Add New Row" OnClick="lnkAddnewRow_Click"><i class="mdi-content-add-circle" style="color: #2bab54;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" ItemStyle-Width="40px" HeaderStyle-Width="40px" Exportable="false">

                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkInvSelect" runat="server" class="css-checkbox" Text=" " CommandName="ClearRow" Checked='false' />
                                                                                            <asp:HiddenField ID="hdnChk" runat="server" Value="" />
                                                                                            <asp:HiddenField runat="server" ID="hdnOwner" Value='<%# Eval("Owner")%>' />
                                                                                            <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ID")%>' />
                                                                                            <asp:HiddenField runat="server" ID="hdnRol" Value='<%# Eval("Rol")%>' />
                                                                                            <asp:HiddenField runat="server" ID="hdnEn" Value='<%# Eval("En")%>' />
                                                                                            <asp:HiddenField runat="server" ID="hdnCompany" Value='<%# Eval("Company")%>' />
                                                                                            <asp:HiddenField runat="server" ID="hdnLoc" Value='<%# Eval("Loc")%>' />
                                                                                            <asp:HiddenField runat="server" ID="hdnAmountDue" Value='<%# Eval("AmountDue")%>' />
                                                                                              <asp:HiddenField runat="server" ID="hdnRefTranID" Value='<%# Eval("RefTranID")%>' />
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn DataField="InvoiceID" SortExpression="InvoiceID" AllowFiltering="true"
                                                                                        HeaderText="Invoices" ShowFilterIcon="false" AutoPostBackOnFilter="true" DataType="System.String" AndCurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>

                                                                                            <asp:TextBox ID="txtInvoiceID" runat="server" CssClass="searchinput_invoice " Visible='<%# Eval("ID").ToString() =="0" ? true : false %>'
                                                                                                onkeypress="return isNumberKey(event,this)" placeholder="Search by Invoice#" Text='<%# Eval("InvoiceID")%>'>
                                                                                            </asp:TextBox>
                                                                                            <asp:Label ID="lblInvoiceID" runat="server" CssClass="searchinput_glAccount "
                                                                                                Text='<%# Eval("InvoiceID")%>' Visible='<%# Eval("ID").ToString() =="0" ? false : true %>'>
                                                                                            </asp:Label>

                                                                                             
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn ShowFilterIcon="false" HeaderText="Payment Received Date" SortExpression="customerName" DataField="PaymentReceivedDate" AutoPostBackOnFilter="true" DataType="System.String" AndCurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtPaymentReceivedDate" runat="server" Text='<%# Eval("PaymentReceivedDate")!=DBNull.Value? (!(Eval("PaymentReceivedDate").Equals(DateTime.MinValue)) ? (String.Format("{0:MM/dd/yyyy}", Eval("PaymentReceivedDate"))) : "" ) : "" %>'
                                                                                                CssClass="input-sm input-small-num datepicker_mom" Width="80"
                                                                                                Style="min-width: 100%!important;"></asp:TextBox>

                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="true" ShowFilterIcon="false" HeaderText="Customer Name" SortExpression="customerName" DataField="customerName" AutoPostBackOnFilter="true" DataType="System.String" AndCurrentFilterFunction="Contains">
                                                                                        <%--<ItemTemplate>
                                                                                            <asp:TextBox ID="txtCustomerName" runat="server" Text='<%# Eval("customerName") ==string.Empty ? "" : Eval("customerName") %>'></asp:TextBox>

                                                      

                                                                                        </ItemTemplate>--%>
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox runat="server" ID="txtCustomerName" CssClass="form-control input-sm input-small-num browser-default txtCustomerNameSearch"
                                                                                                Style="font-size: 0.9rem !important;" Width="150"
                                                                                                Text='<%# Eval("customerName")%>'></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="true" ShowFilterIcon="false" HeaderText="Location" SortExpression="Tag" DataField="Tag" AutoPostBackOnFilter="true" DataType="System.String" AndCurrentFilterFunction="Contains">
                                                                                        <%--<ItemTemplate>
                                                                                            <asp:TextBox ID="txtTag" runat="server" Text='<%# Eval("Tag").ToString() ==string.Empty ? "" : Eval("Tag") %>'>
                                                                                            </asp:TextBox>
                                                                                        </ItemTemplate>--%>
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox runat="server" ID="txtLocationName" CssClass="form-control input-sm input-small-num browser-default txtLocationNameSearch"
                                                                                                Style="font-size: 0.9rem !important;" Width="150"
                                                                                                Text='<%# Eval("Tag")%>'></asp:TextBox>

                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="true" ShowFilterIcon="false" HeaderText="From Account" SortExpression="Description" AutoPostBackOnFilter="true" DataType="System.String" AndCurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblUndepositedFund" runat="server">Undeposited Funds</asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="true" ShowFilterIcon="false" HeaderText="Description" SortExpression="fDesc" AutoPostBackOnFilter="true" DataField="fDesc" DataType="System.String" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtDesc" runat="server" Text='<%# Eval("fDesc").ToString() %>'> </asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="true" ShowFilterIcon="false" HeaderText="Check Number" SortExpression="CheckNumber" AutoPostBackOnFilter="true" DataType="System.String" AndCurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtCheckNumber" runat="server" Text='<%# Eval("CheckNumber").ToString() %>'> </asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="true" ShowFilterIcon="false" HeaderText="Payment Method" SortExpression="PaymentMethod" AutoPostBackOnFilter="true" DataType="System.String" AndCurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="browser-default" />
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblTotal" runat="server" Text='Total:-'></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="true" ShowFilterIcon="false" DataField="AmountDue" FooterAggregateFormatString="{0:c}" Aggregate="Sum" HeaderText="Amount" SortExpression="amount"  CurrentFilterFunction="EqualTo" DataType="System.Double" AutoPostBackOnFilter="true"> 
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AmountDue", "{0:c}")%>' Visible='<%# Eval("ID").ToString() =="0" ? true : false %>'></asp:TextBox>
                                                                                            <asp:Label ID="lblAmount" runat="server" CssClass="searchinput_glAccount "
                                                                                                Text='<%# DataBinder.Eval(Container.DataItem, "AmountDue", "{0:c}")%>' Visible='<%# Eval("ID").ToString() =="0" ? false : true %>'>
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblTotalInvPayAmount" runat="server" Text=''></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                            </FilterMenu>
                                                                        </telerik:RadGrid>
                                                                    </telerik:RadAjaxPanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <input id="hdnCon" runat="server" type="hidden" />
      <input id="hdnAreaActive" runat="server" type="hidden" value="1" />
    <asp:HiddenField ID="Confirm_Value" runat="server" ClientIDMode="Static" />
      
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">

        var picker = new Pikaday(
            {
                field: document.getElementById('ctl00_ContentPlaceHolder1_txtDateDeposite'),
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(2000, 1, 1),
                maxDate: new Date(2100, 12, 31),
                yearRange: [2000, 2100],

                onSelect: function () {
                    CallMe();
                }
            });

        function CallMe() {
            document.getElementById('<%=btnDateDeposite.ClientID%>').click();
        }

        $(document).ready(function () {
            CalCountAmount();
            Materialize.updateTextFields();
            $(".dropdown-content.select-dropdown li").on("click", function () {
                var that = this;
                setTimeout(function () {
                    if ($(that).parent().hasClass('active')) {
                        $(that).parent().removeClass('active');
                        $(that).parent().hide();
                    }
                }, 100);
            });


        });


        $("[id*=ChkAll]").change(function () {
            $("#<%=RadGrid_gvReceivePayment.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', $(this).prop("checked"));

            $("#<%=RadGrid_gvReceivePayment.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                var chk = $(this).attr('id');
                if ($(this).prop('checked') == true) {
                    SelectedRowStyle('<%=RadGrid_gvReceivePayment.ClientID %>')
                }
                else if ($(this).prop('checked') == false) {
                    $(this).closest('tr').removeAttr("style");
                }
            });

            CalCountAmount();
        });
        $("[id*=lnkGLAddnewRow]").mouseup(function () {
            CalCountAmount();
        });
        $("[id*=txtGLAmount]").blur(function () {
            //var tmp = $(this).val().replace(/[\$\(\),]/g, '');      

            //tmp = cleanUpCurrency("$" + parseFloat(tmp).toLocaleString("en-US", { minimumFractionDigits: 2 }))
            //if (tmp != "$NaN") {
            //    $(this).val(tmp);
            //}        
            //CalCountAmount();

            var amt=  convertNumber($(this).val().replace("$",''))
                 amt = parseFloat(amt)

                var amtval = cleanUpCurrency('$' + amt.toLocaleString("en-US", { minimumFractionDigits: 2 }))
            $(this).val(amtval)
            CalCountAmount();
        });
        $("[id*=ibtnDeleteGLRow]").mouseup(function () {
            CalCountAmount();
        });
        $("[id*=txtAmount]").blur(function () {
            var hdnAmountDue = this.id.replace('txtAmount', 'hdnAmountDue');
            var currentAmount = parseFloat(document.getElementById(hdnAmountDue).value.toString().replace(/[\$\(\),]/g, ''));
            var tmp = $(this).val().replace(/[\$\(\),]/g, '');         

            tmp = cleanUpCurrency("$" + parseFloat(tmp).toLocaleString("en-US", { minimumFractionDigits: 2 }))
            if (tmp != "$NaN") {
                $(this).val(tmp);
            }
            CalCountAmount();
        });
        $("[id*=chkSelect]").change(function () {

            var chk = $(this).attr('id');
            if ($(this).prop('checked') == true) {
                SelectedRowStyle('<%=RadGrid_gvReceivePayment.ClientID %>')
            }
            else if ($(this).prop('checked') == false) {
                $(this).closest('tr').removeAttr("style");
            }

            CalCountAmount();
        });

        function CalCountAmount() {
            var _count = 0;
            var _amountTotal = 0;
            var expression = /[\(\)]/g
         
            $("#<%=RadGrid_gvReceivePayment.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                try {
                     var chk = $(this).attr('id');
                if ($(this).prop('checked') == true) {
                    var _amount = 0;
                    var lblAmount = document.getElementById(chk.replace('chkSelect', 'lblAmount'));
                    if ($(lblAmount).text().match(expression)) {
                        _amount = parseFloat($(lblAmount).text().toString().replace(/[\$\(\),]/g, ''));
                        _amount = _amount * -1
                    }
                    else {
                        _amount = parseFloat($(lblAmount).text().toString().replace(/[\$\(\),]/g, ''));
                    }

                    _amountTotal = parseFloat(_amountTotal) + parseFloat(_amount);
                    _count = parseInt(_count) + 1;
                }
                } catch (ex) {

                }

               
            });

            //GL Account
            var _countGL = 0;
            var _amountGLTotal = 0;
            //debugger
            $("#<%=RadGrid_gvDepositGL.ClientID %>").find('tr:not(:first,:last)').each(function () {
                try {
                   // debugger
                    var $tr = $(this);
                    var _amountGL = 0;
                      var acct =  $tr.find('input[id*=hdnID]').val();
                    var temp = $tr.find('input[id*=txtGLAmount]').val();
                    if (acct && acct != '') {
                        if (temp.match(expression)) {
                        _amountGL = parseFloat(temp.toString().replace(/[\$\(\),]/g, ''));
                        _amountGL = _amountGL * -1
                    }
                    else {
                        _amountGL = parseFloat(temp.toString().replace(/[\$\(\),]/g, ''));
                    }

                    _amountGLTotal = parseFloat(_amountGLTotal) + parseFloat(_amountGL);
                    _countGL = parseInt(_countGL) + 1;
                    }
                    
                } catch (ex) { }

            });
            //Invoice
            var _countInvoice = 0;
            var _amountInvoiceTotal = 0.00;
            $("#<%=RadGrid_gvInvoiceDeposit.ClientID %>").find('tr:not(:first,:last)').each(function () {
                try {
                    //debugger
                    var $tr = $(this);
                    var _amountInvoice = 0;
                    var Amount;
                    if ($tr.find('input[id*=hdnID]').val() == "0") {
                        Amount = $tr.find('input[id*=txtAmount]').val();
                    } else {
                        Amount = $tr.find('input[id*=hdnAmountDue]').val();
                    }

                    if (Amount.match(expression)) {
                        _amountInvoice = parseFloat(Amount.toString().replace(/[\$\(\),]/g, ''));
                        _amountInvoice = _amountInvoice * -1
                    }
                    else {
                        _amountInvoice = parseFloat(Amount.toString().replace(/[\$\(\),]/g, ''));
                    }
                    
                    _amountInvoiceTotal = _amountInvoiceTotal + _amountInvoice;
                    _countInvoice = parseInt(_countInvoice) + 1;

                } catch (ex) { }

            });

            debugger
            $("[id*=lblSelectedItems]").html("Selected Item(s) " + _count);
            
            //Javascript issue: 86.96 -0.01=86.94999999999999
            var displayDepAmount = parseFloat(_amountTotal + _amountGLTotal + _amountInvoiceTotal).toLocaleString("en-US", { minimumFractionDigits: 2 });
            var displayInvoiceTotal = parseFloat(_amountInvoiceTotal).toLocaleString("en-US", { minimumFractionDigits: 2 });
            var displayGLAmount = parseFloat(_amountGLTotal).toLocaleString("en-US", { minimumFractionDigits: 2 });
            if (displayDepAmount == 0) displayDepAmount = parseFloat(0.00).toLocaleString("en-US", { minimumFractionDigits: 2 });
            if (displayInvoiceTotal == 0) displayInvoiceTotal = parseFloat(0.00).toLocaleString("en-US", { minimumFractionDigits: 2 });
            if (displayGLAmount == 0) displayGLAmount = parseFloat(0.00).toLocaleString("en-US", { minimumFractionDigits: 2 });

            $("#<%=lblDepositTotal.ClientID%>").html(cleanUpCurrency("$" +displayDepAmount) );
            $("#<%=hdDepositTotal.ClientID%>").val(cleanUpCurrency("$" +displayDepAmount) );

            $("[id*=lblTotalInvPayAmount]").html(cleanUpCurrency("$" +displayInvoiceTotal));
            $("[id*=lblTotalGLAmount]").html(cleanUpCurrency("$" +displayGLAmount ));
            $("[id*=lblRecordCount]").html(_count + _countGL + _countInvoice + " Record(s) found");


        }


        function SelectedRowStyle(gridview) {
            try {
var grid = document.getElementById(gridview);
            $('#' + gridview + ' tr').each(function () {
                var $tr = $(this);
                var chk = $tr.find('input[id*=chkSelect]');
                if (chk.prop('checked') == true) {
                    $tr.css('background-color', '#e0eefe');
                }
            })
            } catch (ex) {

            }
            
        }

        var pending = false;
        function DisableButton() {
            if (pending) {
                return false;
            }
            else {
                if (Confirm() == true) {
                     if (Page_ClientValidate(""))
                    pending = true;
                }               
               return pending;
            }
        }


        function isNumberKey(evt, txt) {
            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }        

    </script>
    <script>


        function pageLoad(sender, args) {
            function dtaa() {
                this.prefixText = null;
                this.con = document.getElementById('<%=hdnCon.ClientID%>').value;
            }
             //txtOwnerName
            $("[id*=txtCustomerName]").autocomplete({

                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetCustomer",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {

                            noty({ text: 'Due to unexpected errors we were unable to load job code', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        }
                    });
                },
                select: function (event, ui) {
                   // debugger
                    var txtOwnerName = this.id;
                    var hdnOwner = document.getElementById(txtOwnerName.replace('txtCustomerName', 'hdnOwner'));
                    $(hdnOwner).val(ui.item.value);
                    $(this).val(ui.item.label);

                    var txtInvoice = document.getElementById(txtOwnerName.replace('txtCustomerName', 'txtInvoiceID'));

                    var hdnLoc = document.getElementById(txtOwnerName.replace('txtCustomerName', 'hdnLoc'));
                    var txtLocationName = document.getElementById(txtOwnerName.replace('txtCustomerName', 'txtLocationName'));

                    var txtAmount = document.getElementById(txtOwnerName.replace('txtCustomerName', 'txtAmount'));
                    var hdnAmountDue = document.getElementById(txtOwnerName.replace('txtCustomerName', 'hdnAmountDue'));
                      
                 

                    hdnLoc.value = "0";
                    txtLocationName.value = "";      
                    if (txtAmount != null) {
                         txtAmount.value = 0;
                    }
                   
                    hdnAmountDue.value = 0;       
                    if (txtInvoice!=null) {
                         txtInvoice.value = "0";
                    }
                   
                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            });

            $.each($(".txtCustomerNameSearch"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.label;

                    var result_value = item.value;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.toString().replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });

                    if (result_value == 0) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                };
            });

            //txtLocationName
            $("[id*=txtLocationName]").autocomplete({
                source: function (request, response) {
                    var hdnOwner
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    dtaaa.custID = 0;
                    var txtLocationName = this.element[0].id;
                    if (txtLocationName) {
                        hdnOwner = document.getElementById(txtLocationName.replace('txtLocationName', 'hdnOwner'));
                    }
                    if ($(hdnOwner).val() != '') {
                        dtaaa.custID = $(hdnOwner).val();
                    }
                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetLocation",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            noty({ text: 'Due to unexpected errors we were unable to load job code', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        }
                    });
                },
                select: function (event, ui) {
                  // debugger
                    var txtLocationName = this.id;
                   
                    var hdnLoc = document.getElementById(txtLocationName.replace('txtLocationName', 'hdnLoc'));

                    var txtCustomerName = document.getElementById(txtLocationName.replace('txtLocationName', 'txtCustomerName'));
                    var hdnOwner = document.getElementById(txtLocationName.replace('txtLocationName', 'hdnOwner'));

                    var txtInvoice = document.getElementById(txtLocationName.replace('txtLocationName', 'txtInvoiceID'));
                   
                    hdnLoc.value = ui.item.value;

                    txtCustomerName.value = ui.item.CompanyName;
                    hdnOwner.value = ui.item.OwnerID;
                    if (txtInvoice != null) {
                         txtInvoice.value = "0";
                    }                   
                    $(this).val(ui.item.label);
                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            });

            $.each($(".txtLocationNameSearch"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.label;
                    var result_value = item.desc;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.toString().replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });

                     return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_value + "</span>")
                        .appendTo(ul);

                
                };
            });

            $("[id*=txtInvoiceID]").autocomplete({

                source: function (request, response) {

                    var dataValue = "{ prefixText: '" + request.term + "', count: 0, contextKey: '' }";
                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/ServiceGetListOpenInvoice",
                        data: dataValue,
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            console.log("Due to unexpected errors we were unable to load invoice");
                        }
                    });
                },
                select: function (event, ui) {

                    var str = ui.item.Invoice;
                    if (str == "No Record Found!") {
                        $(this).val("");
                    }
                    else {
                        $(this).val(ui.item.Invoice);
                        debugger

                        var txtAmount = this.id.replace('txtInvoiceID', 'txtAmount');
                        var hdnAmountDue = this.id.replace('txtInvoiceID', 'hdnAmountDue');
                        var txtCustomerName = this.id.replace('txtInvoiceID', 'txtCustomerName');
                        var txtLocationName = this.id.replace('txtInvoiceID', 'txtLocationName');
                        var hdnOwner = this.id.replace('txtInvoiceID', 'hdnOwner');
                        var hdnLoc = this.id.replace('txtInvoiceID', 'hdnLoc');
                         var hdnRefTranID = this.id.replace('txtInvoiceID', 'hdnRefTranID');
                        document.getElementById(hdnAmountDue).value = cleanUpCurrency('$' + ui.item.AmountDue.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        document.getElementById(txtAmount).value = cleanUpCurrency('$' + ui.item.AmountDue.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        document.getElementById(txtCustomerName).value = ui.item.OwnerName;
                        document.getElementById(txtLocationName).value = ui.item.Tag;
                        document.getElementById(hdnOwner).value = ui.item.Owner;
                        document.getElementById(hdnLoc).value = ui.item.Loc;
                          document.getElementById(hdnRefTranID).value = ui.item.RefTranID;
                        CalCountAmount();
                        Materialize.updateTextFields();
                    }
                    return false;
                },
                focus: function (event, ui) {

                    $(this).val(ui.item.Invoice);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".searchinput_invoice"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.Invoice;

                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);

                };
            });



            //txtAcct
            $("[id*=txtAcct]").autocomplete({

                source: function (request, response) {

                    var dataValue = "{ prefixText: '" + request.term + "', count: 0, contextKey: '' }";
                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/ServiceGLAccount",
                        data: dataValue,
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            console.log("Due to unexpected errors we were unable to load invoice");
                        }
                    });
                },
                select: function (event, ui) {

                    var str = ui.item.Acct;
                    if (str == "No Record Found!") {
                        $(this).val("");
                    }
                    else {
                        $(this).val(ui.item.Acct);
                        var hdnID = this.id.replace('txtAcct', 'hdnID');
                        var hdnTitle = this.id.replace('txtAcct', 'hdnTitle');
                        var lblTitle = this.id.replace('txtAcct', 'lblTitle');
                        var txtDesc = this.id.replace('txtAcct', 'txtDesc');                       
                        var txtGLAmount = this.id.replace('txtAcct', 'txtGLAmount');

                        document.getElementById(txtGLAmount).value = 0;
                        document.getElementById(txtDesc).value = '';                       
                        document.getElementById(hdnTitle).value = ui.item.fDesc;
                        document.getElementById(lblTitle).innerHTML = ui.item.fDesc;
                        document.getElementById(hdnID).value = ui.item.ID;

                    }
                    return false;
                },
                focus: function (event, ui) {

                    $(this).val(ui.item.Acct);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".searchinput_glAccount"), function (index, item) {
                try {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var result_item = item.Acct;

                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "  : " + item.fDesc + "</a>")
                            .appendTo(ul);

                    };
                } catch (ex) { }
            });
            $("[id*=ibtnDeleteGLRow]").mouseup(function () {               
                CalCountAmount();
            });

            $("[id*=lnkGLAddnewRow]").mouseup(function () {
                CalCountAmount();
            });
            $("[id*=txtGLAmount]").blur(function () {
                //var tmp = $(this).val().replace(/[\$\(\),]/g, '');   
                //tmp = cleanUpCurrency("$" + parseFloat(tmp).toLocaleString("en-US", { minimumFractionDigits: 2 }))
                //if (tmp != "$NaN") {
                //    $(this).val(tmp);
                //}              
                //CalCountAmount();
                //Materialize.updateTextFields();
                   var amt=  convertNumber($(this).val().replace("$",''))
                 amt = parseFloat(amt)

                var amtval = cleanUpCurrency('$' + amt.toLocaleString("en-US", { minimumFractionDigits: 2 }))
            $(this).val(amtval)
                CalCountAmount();
                Materialize.updateTextFields();
            });
            $("[id*=chkSelect]").change(function () {
                var chk = $(this).attr('id');
                if ($(this).prop('checked') == true) {
                    SelectedRowStyle('<%=RadGrid_gvReceivePayment.ClientID %>')
                }
                else if ($(this).prop('checked') == false) {
                    $(this).closest('tr').removeAttr("style");
                }

                CalCountAmount();
            });
            $("[id*=txtAmount]").blur(function () {                                     
                var tmp = $(this).val().replace(/[\$\(\),]/g, '');            
                tmp = cleanUpCurrency("$" + parseFloat(tmp).toLocaleString("en-US", { minimumFractionDigits: 2 }))
                if (tmp != "$NaN") {
                    $(this).val(tmp);
                }
                CalCountAmount();
                Materialize.updateTextFields();
            });
         
            function CalCountAmount() {
                var _count = 0;
                var _amountTotal = 0;
                var expression = /[\(\)]/g
                $("#<%=RadGrid_gvReceivePayment.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                    try {
                         var chk = $(this).attr('id');
                    if ($(this).prop('checked') == true) {
                        var _amount = 0;
                        var lblAmount = document.getElementById(chk.replace('chkSelect', 'lblAmount'));
                        if ($(lblAmount).text().match(expression)) {
                            _amount = parseFloat($(lblAmount).text().toString().replace(/[\$\(\),]/g, ''));
                            _amount = _amount * -1
                        }
                        else {
                            _amount = parseFloat($(lblAmount).text().toString().replace(/[\$\(\),]/g, ''));
                        }

                        _amountTotal = parseFloat(_amountTotal) + parseFloat(_amount);
                        _count = parseInt(_count) + 1;
                    }
                    } catch (ex) { }
                
                });

                //GL Account
                var _countGL = 0;
                var _amountGLTotal = 0;
                debugger
                $("#<%=RadGrid_gvDepositGL.ClientID %>").find('tr:not(:first,:last)').each(function () {
                    try {
                        var $tr = $(this);
                        var acct =  $tr.find('input[id*=hdnID]').val();
                        
                        var _amountGL = 0;
                        var temp = $tr.find('input[id*=txtGLAmount]').val();
                        if (acct && acct != '') {
                            if (temp.match(expression)) {
                            _amountGL = parseFloat(temp.toString().replace(/[\$\(\),]/g, ''));
                            _amountGL = _amountGL * -1
                        }
                        else {
                            _amountGL = parseFloat(temp.toString().replace(/[\$\(\),]/g, ''));
                        }

                        _amountGLTotal = parseFloat(_amountGLTotal) + parseFloat(_amountGL);
                        _countGL = parseInt(_countGL) + 1;
                        }
                        
                    } catch (ex) { }

                });
                //Invoice
                var _countInvoice = 0;
                var _amountInvoiceTotal = 0.00;
                $("#<%=RadGrid_gvInvoiceDeposit.ClientID %>").find('tr:not(:first,:last)').each(function () {
                    try {
                        var $tr = $(this);
                        var _amountInvoice = 0;
                        if ($tr.find('input[id*=hdnID]').val() == "0") {
                            Amount = $tr.find('input[id*=txtAmount]').val();
                        } else {
                            Amount = $tr.find('input[id*=hdnAmountDue]').val();
                        }
                        
                        if (Amount.match(expression)) {
                            _amountInvoice = parseFloat(Amount.toString().replace(/[\$\(\),]/g, ''));
                            _amountInvoice = _amountInvoice * -1
                        }
                        else {
                            _amountInvoice = parseFloat(Amount.toString().replace(/[\$\(\),]/g, ''));
                        }

                        _amountInvoiceTotal = _amountInvoiceTotal + _amountInvoice;
                        _countInvoice = parseInt(_countInvoice) + 1;

                    } catch (ex) { }

                });
                
                  var displayDepAmount = parseFloat(_amountTotal + _amountGLTotal + _amountInvoiceTotal).toLocaleString("en-US", { minimumFractionDigits: 2 });
            var displayInvoiceTotal = parseFloat(_amountInvoiceTotal).toLocaleString("en-US", { minimumFractionDigits: 2 });
            var displayGLAmount =parseFloat(_amountGLTotal).toLocaleString("en-US", { minimumFractionDigits: 2 });
                if (displayDepAmount == 0) displayDepAmount = parseFloat(0.00).toLocaleString("en-US", { minimumFractionDigits: 2 });
                if (displayInvoiceTotal == 0) displayInvoiceTotal = parseFloat(0.00).toLocaleString("en-US", { minimumFractionDigits: 2 });
                        if (displayGLAmount == 0) displayGLAmount =parseFloat(0.00).toLocaleString("en-US", { minimumFractionDigits: 2 });

                $("#<%=lblDepositTotal.ClientID%>").html(cleanUpCurrency("$" +displayDepAmount));
                $("#<%=hdDepositTotal.ClientID%>").val(cleanUpCurrency("$" + displayDepAmount));

                $("[id*=lblSelectedItems]").html("Selected Item(s) " + _count);
                $("[id*=lblTotalInvPayAmount]").html(cleanUpCurrency("$" + displayInvoiceTotal));
                $("[id*=lblTotalGLAmount]").html(cleanUpCurrency("$" +displayGLAmount));
                $("[id*=lblRecordCount]").html(_count + _countGL + _countInvoice + " Record(s) found");

            }

        }
    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            $('a[href^="#accrd"]').on('click', function (e) {
                e.preventDefault();

                var target = this.hash;
                var $target = $(target);
                if ($(target).hasClass('active') || target == "") {

                }
                else {
                    $(target).click();
                }

                $('html, body').stop().animate({
                    'scrollTop': $target.offset().top - 125
                }, 900, 'swing');
            });
           
        });
        $(document).ready(function () {
             if ($("#<%=hdnAreaActive.ClientID %>").val() == "0") {              
                 $('#accrdGlAccount').click();
            } else {
                 $('#accrdInvoice').click()               
            }
            
        });
        function cleanUpCurrency(s) {
            if (s.indexOf('-') == 0) {
                //It matched - strip out - and append parentheses 
                return s.replace("-", "\(") + ")";
            } else if (s.indexOf('$-') == 0) {
                return s.replace("$-", "\($") + ")";
            } else {
                return s;
            }
        }
        function Confirm() {            
              Page_BlockSubmit = false;
            var isEmptyLocation = true;
              $("#<%=RadGrid_gvInvoiceDeposit.ClientID %>").find('tr:not(:first,:last)').each(function () {
                try {
                    var $tr = $(this);
                    var temp = $tr.find('input[id*=hdnLoc]').val();                   
                    if (temp!=undefined && temp == "0") {
                          isEmptyLocation= false;
                      
                    }                   
                } catch (ex) { }           

            });  

            if (isEmptyLocation == false) {
                 noty({ text: 'Location is empty', dismissQueue: true, type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: false, theme: 'noty_theme_default', closable: false });
                return false;
            }
            var _count = 0;
            $("#<%=RadGrid_gvReceivePayment.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                var chk = $(this).attr('id');
                if ($(this).prop('checked') == true) {
                    _count = parseInt(_count) + 1;
                }
            });
            debugger
            //GL Account
            var _countGL = 0;
            debugger
            $("#<%=RadGrid_gvDepositGL.ClientID %>").find('tr:not(:first,:last)').each(function () {
                try {  
                     var $tr = $(this);                       
                    var temp = $tr.find('input[id*=hdnID]').val();
                     var templblAcct = $tr.find('span[id*=lblAcct]').text();
                    if (temp && temp!= "") {
                         _countGL = parseInt(_countGL) + 1;
                    }   
                    if (templblAcct && templblAcct!= "") {
                         _countGL = parseInt(_countGL) + 1;
                    }   
                } catch (ex) { }

            });
            //Invoice
            var _countInvoice = 0;
          
            $("#<%=RadGrid_gvInvoiceDeposit.ClientID %>").find('tr:not(:first,:last)').each(function () {
                try {
                    var $tr = $(this);
                    var temp = $tr.find('input[id*=hdnAmountDue]').val();                   
                    if (temp && temp != "0") {
                        _countInvoice = parseInt(_countInvoice) + 1;
                    }                   
                } catch (ex) { }           

            });          
           
            var depVal = _count + _countGL+ _countInvoice;
            if (depVal != 0) {
                var Val;
                if (confirm("Do you want to print?") == true) {
                    Val = "Yes";
                } else {
                    Val = "No";
                }

                document.getElementById("Confirm_Value").value = Val;
               
                return true;
            } else {
                noty({ text: 'Please select a item ', dismissQueue: true, type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: false, theme: 'noty_theme_default', closable: false });
                return false;
            }
            
        }


    </script>
</asp:Content>


