<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" Inherits="EditMultiReceivePayment" MasterPageFile="~/Mom.master" CodeBehind="EditMultiReceivePayment.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <style>
        .srchtitle {
            margin-top: 18px;
            margin-right: 40px;
        }
        .textRightAlign{
            text-align:right;
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
    
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="overlay">
        <img src="images/wheel.GIF" alt="Be patient..." style="position: fixed; margin-top: 25%; margin-left: 50%;" />
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager_gvInvoice" runat="server">
        <AjaxSettings>     
            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoice" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                </UpdatedControls>
            </telerik:AjaxSetting> 
             <telerik:AjaxSetting AjaxControlID="txtlsInvoice">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoice" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                  
                </UpdatedControls>
            </telerik:AjaxSetting> 
            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoice" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
      
                </UpdatedControls>
            </telerik:AjaxSetting> 

          
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_gvInvoice" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-editor-attach-money"></i>&nbsp;<asp:Label ID="lblHeader" runat="server">Edit Batch Receipt</asp:Label></div>
                                    <div class="btnlinks">
                                        <asp:LinkButton ToolTip="Save" ID="btnSubmit" runat="server" OnClientClick="return validateGrid(this);" OnClick="btnSubmit_Click" ValidationGroup="Payment">Save</asp:LinkButton>
                                    </div>

                                    <div class="btnclosewrap">
                                        <asp:LinkButton ToolTip="Close" ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click">
                                                    <i class="mdi-content-clear"></i>
                                        </asp:LinkButton>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                </header>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="srchpane">
                <div class="srchpaneinner">
                    <div class="srchtitle" style="padding-left: 15px;">
                        Date
                    </div>
                    <div class="srchinputwrap" style="width: 140px">
                        <div class="row">
                            <asp:TextBox ID="txtDate" CssClass="datepicker_mom" runat="server" autocomplete="off" Style="height:1.5rem;margin:16px"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfvDate" ControlToValidate="txtDate"
                                ErrorMessage="Please enter Date." Display="None"
                                ValidationGroup="Payment">
                            </asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True" PopupPosition="Right"
                                TargetControlID="rfvDate" />
                            <asp:RegularExpressionValidator ID="revDate" ControlToValidate="txtDate" ValidationGroup="Payment"
                                ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                            </asp:RegularExpressionValidator>
                            <asp:ValidatorCalloutExtender ID="vceDate1" runat="server" Enabled="True" PopupPosition="Right"
                                TargetControlID="revDate" />

                        </div>
                    </div>
                    <div class="srchtitle" style="padding-left: 0px; margin-left: 40px;">
                        Payment Method
                    </div>
                    <div class="srchinputwrap" style="width: 150px">
                        <asp:DropDownList ID="ddlPayment" class="browser-default" runat="server" onchange="ddl_changed(this)">
                        </asp:DropDownList>
                    </div>
                    <div class="srchtitle" style="padding-left: 0px; margin-left: 40px;">
                        Bank
                    </div>
                    <div class="srchinputwrap" style="width: 150px">
                        <asp:DropDownList ID="ddlBank" runat="server" CssClass="browser-default"></asp:DropDownList>

                    </div>

                    <div class="srchinputwrap" style="width: 150px; padding-left: 0px; margin-left: 40px; margin-top: 18px">

                        <asp:CheckBox ID="chkCreateDeposit" CssClass="css-checkbox" Text="Create Deposit" runat="server" />
                    </div>


                </div>
            </div>
        </div>
        <div class="row">


            <div class="srchpane">
                <div class="srchpaneinner">
                    <div class="srchtitle" style="padding-left: 15px;">
                        Invoice
                    </div>
                    <div class="srchinputwrap" style="width: 300px">

                        <asp:AutoCompleteExtender runat="server" Enabled="true" ServicePath="CustomerAuto.asmx" ServiceMethod="ServiceGetOpenInvoice" MinimumPrefixLength="1" CompletionInterval="10"
                            EnableCaching="false" CompletionSetCount="10" TargetControlID="txtlsInvoice" ID="AutoCompleteExtender1" UseContextKey="True"
                            FirstRowSelected="false"
                            CompletionListCssClass="autocomplete_completionListElement"
                            DelimiterCharacters="," 
                            EnableViewState="true">
                        </asp:AutoCompleteExtender>

                        <asp:TextBox ID="txtlsInvoice" runat="server" AutoPostBack="true"></asp:TextBox>
                        <asp:HiddenField ID="Confirm_Value" runat="server" ClientIDMode="Static" />
                    </div>
                   
                      <div class="srchtitle" style="padding-left: 15px;">
                       Check
                    </div>
                    <div class="srchinputwrap" style="width: 140px">
                        <asp:TextBox ID="txtCheck" runat="server" AutoPostBack="true"></asp:TextBox>
                      
                    </div>

                    <div class="srchtitle" style="padding-left: 15px;">
                       
                    </div>
                    <div class="srchinputwrap" style="width: 150px; padding-left: 0px; margin-right: 40px; margin-top: 18px">
                       
                       <asp:CheckBox ID="chkSeparateInvoice" CssClass="css-checkbox" Text="Separate per invoice" runat="server" />
                    </div>

                    <div class="srchinputwrap srchclr btnlinksicon" style="margin-left: -10px; margin-top: 5px;">
                        <asp:LinkButton ID="lnkSearch" runat="server" OnClick="lnkSearch_Click" CausesValidation="false" ToolTip="Search"><i class="mdi-action-search"></i></asp:LinkButton>
                    </div>
                </div>
                <div class="grid_container">
                    <div class="form-section-row" style="margin-bottom: 0 !important;">

                        <div class="RadGrid RadGrid_Material FormGrid" style="margin-bottom: 10px!important;">
                            <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                <script type="text/javascript">
                                    function pageLoad() {
                                        var grid = $find("<%= RadGrid_gvInvoice.ClientID %>");
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
                            <telerik:RadAjaxPanel ID="RadAjaxPanel_gvInvoice" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                <div style="display: none">
                                    <asp:LinkButton ID="btnAddnewRow" runat="server" CausesValidation="False"
                                        ToolTip="Add New Row" OnClick="btnAddnewRow_Click">New</asp:LinkButton>

                                </div>
                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvInvoice" ShowFooter="True" AllowSorting="true" OnNeedDataSource="RadGrid_gvInvoice_NeedDataSource" OnPreRender="RadGrid_gvInvoice_PreRender"
                                    runat="server" Width="100%" AllowMultiRowSelection="true" CssClass="RadGrid_Material">
                                    <CommandItemStyle />
                                    <GroupingSettings CaseSensitive="false" />
                                    <ClientSettings AllowColumnsReorder="false" EnableAlternatingItems="false" ReorderColumnsOnClient="false" EnablePostBackOnRowClick="false" Selecting-AllowRowSelect="false">
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                        <ClientEvents OnKeyPress="AddNewRows" />
                                        
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="40" AllowFiltering="false" ItemStyle-Width="0.5%" FooterStyle-Width="0.5%">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="ibtnDeleteRow" OnClientClick="return confirmDelete()"
                                                        CausesValidation="false" ToolTip="Delete" runat="server" Style="color: #000; font-size: 1.5em; top: 0px;" Width="20px"
                                                        OnClick="ibtnDeleteRow_Click"><i class="mdi-navigation-cancel" style=" color: #f00;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>

                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnOrderNo" Value='<%# Eval("OrderNo") %>' runat="server"></asp:HiddenField>
                                                     <asp:HiddenField ID="hdnLine" Value='<%# Container.ItemIndex +1 %>' runat="server"></asp:HiddenField>
                                                    <asp:Label ID="lblIndex" Visible="false" runat="server" Text="<%# Container.ItemIndex +1 %>"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:LinkButton ID="lnkAddnewRow" runat="server" CausesValidation="False" Style="color: #000; font-size: 1.5em;" Width="20px"
                                                        ToolTip="Add New Row" OnClick="lnkAddnewRow_Click"><i class="mdi-content-add-circle" style="color: #2bab54;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>


                                            <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" ItemStyle-Width="30px" HeaderStyle-Width="40px" Exportable="false">
                                                <HeaderTemplate>
                                                    <input id="chkAll" type="checkbox" class="css-checkbox" />
                                                    <label for="chkAll" class="css-label">&nbsp;</label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" class="css-checkbox" Text=" " CommandName="ClearRow"  Checked='<%#Convert.ToBoolean(Eval("isChecked"))%>'
                                                  Enabled ='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"enableEdit"))==true? true:false%>'
                                                        />
                                                    <asp:HiddenField ID="hdnChk" runat="server" Value="" />
                                                    <asp:HiddenField runat="server" ID="hdnOwner" Value='<%# Eval("Owner")%>' />
                                                    <asp:HiddenField runat="server" ID="hdnAmount" Value='<%# Eval("Amount")%>' />
                                                    <asp:HiddenField runat="server" ID="hdnDueAmount" Value='<%# Eval("AmountDue")%>' />
                                                    <asp:HiddenField runat="server" ID="hdnLocID" Value='<%# Eval("LocID")%>' />
                                                    <asp:HiddenField runat="server" ID="hdnBatchReceive" Value='<%# Eval("BatchReceive")%>' />
                                                    <asp:HiddenField runat="server" ID="hdnSTax" Value='<%# Eval("STax")%>' />
                                                    <asp:HiddenField runat="server" ID="hdnTotal" Value='<%# Eval("Total")%>' />      
                                                 <%--    <asp:HiddenField runat="server" ID="hdnID" Value='<%# Eval("ID")%>' />  --%>
                                                     <asp:HiddenField runat="server" ID="hdnInvoice" Value='<%# Eval("Invoice")%>' />
                                                      <asp:HiddenField runat="server" ID="hdnReceiptID" Value='<%# Eval("ReceiptID")%>' />
                                                      <asp:HiddenField runat="server" ID="hdnDepID" Value='<%# Eval("DepID")%>' />
                                                      <asp:HiddenField runat="server" ID="hdnDepStatus" Value='<%# Eval("DepStatus")%>' />
                                                      <asp:HiddenField runat="server" ID="hdnenableEdit" Value='<%# Eval("enableEdit")%>' />
                                                     <asp:HiddenField runat="server" ID="hdnRefTranID" Value='<%# Eval("RefTranID")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                           
                                            <telerik:GridTemplateColumn DataField="OwnerName" SortExpression="loc" AutoPostBackOnFilter="true"
                                                HeaderText="Customer" ShowFilterIcon="false" HeaderStyle-Width="220px">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtOwnerName" CssClass="form-control input-sm input-small-num browser-default txtOwnerNameSearch"
                                                        Style="font-size: 0.9rem !important;" Width="220"
                                                        Text='<%# Eval("OwnerName")%>'></asp:TextBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="loc" SortExpression="loc" AutoPostBackOnFilter="true"
                                                HeaderText="Loc ID" ShowFilterIcon="false"  HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLocID" runat="server" Text='<%#Eval("LocID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="LocationName" SortExpression="loc" AutoPostBackOnFilter="true"
                                                HeaderText="Location" ShowFilterIcon="false"  HeaderStyle-Width="220px">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtLocationName" CssClass="form-control input-sm input-small-num browser-default txtLocationNameSearch"
                                                        Style="font-size: 0.9rem !important;" Width="220"
                                                        Text='<%# Eval("LocationName")%>'></asp:TextBox>

                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                             <telerik:GridTemplateColumn AllowFiltering="false"  ShowFilterIcon="false"  HeaderText="Check" SortExpression="CheckNumber"  HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txt_gCheck" CssClass="form-control input-sm input-small-num browser-default "
                                                        Style="font-size: 0.9rem !important;" Width="100"
                                                        Text='<%# Eval("CheckNumber")%>'></asp:TextBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="Invoice" SortExpression="fDate" AutoPostBackOnFilter="true"
                                                HeaderText="Invoices" ShowFilterIcon="false"  HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:AutoCompleteExtender runat="server" Enabled="true" ServicePath="CustomerAuto.asmx" ServiceMethod="ServiceGetInvoicePay" MinimumPrefixLength="1" CompletionInterval="10"
                                                        EnableCaching="false" CompletionSetCount="10" TargetControlID='txtInvoice' ID="AutoCompleteExtender" UseContextKey="True"
                                                        FirstRowSelected="false"
                                                        CompletionListCssClass="autocomplete_completionListElement"
                                                        DelimiterCharacters=",">
                                                    </asp:AutoCompleteExtender>

                                                    <asp:TextBox runat="server" ID="txtInvoice" Width="80" onkeyup="SetContextKey(this)" onblur="UpdateAmount(this)"
                                                        Text='<%# Eval("Invoice")%>'></asp:TextBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                             <telerik:GridTemplateColumn DataField="Amount" AutoPostBackOnFilter="true"   SortExpression="Amount"
                                                HeaderText="Pretax Amount" ShowFilterIcon="false" HeaderStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPretax" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>' Style="float: right;"></asp:Label>
                                                </ItemTemplate>                                              
                                            </telerik:GridTemplateColumn>
                                              <telerik:GridTemplateColumn DataField="STax" AutoPostBackOnFilter="true"  SortExpression="STax" 
                                                HeaderText="Sales Tax Amount" ShowFilterIcon="false" HeaderStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSalesTax" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "STax", "{0:c}")%>' Style="float: right;"></asp:Label>
                                                </ItemTemplate>                                              
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn DataField="Total" SortExpression="Total" AutoPostBackOnFilter="true"
                                                HeaderText="Total Amount" ShowFilterIcon="false" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Total", "{0:c}")%>' Style="float: right;"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalOrigAmount" runat="server" Style="float: right;padding-right:5px" Font-Bold="True" />
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn DataField="AmountDue" FooterStyle-HorizontalAlign="Right" SortExpression="AmountDue" AutoPostBackOnFilter="true"
                                                HeaderText="Amount Due" ShowFilterIcon="false" HeaderStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDueAmount" runat="server" Text=<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"isChecked"))==true? string.Format("{0:c}" ,Convert.ToDouble( DataBinder.Eval(Container.DataItem, "AmountDue"))-Convert.ToDouble( DataBinder.Eval(Container.DataItem, "paymentAmt"))):DataBinder.Eval(Container.DataItem, "AmountDue", "{0:c}")%> Style="float: right;"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalDueAmount" runat="server" Style="float: right;" Font-Bold="True" />
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn DataField="paymentAmt" SortExpression="paymentAmt" AutoPostBackOnFilter="true"
                                                HeaderText="Payment" ShowFilterIcon="false" UniqueName="paymentAmt"  HeaderStyle-HorizontalAlign="Right"  HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnPrevDue" Value='<%# Eval("AmountDue") %>' runat="server" />
                                                    <asp:TextBox ID="txtPAmount" runat="server" MaxLength="15" Text='<%# DataBinder.Eval(Container.DataItem, "paymentAmt", "{0:c}") %>'
                                                        EnableViewState="false"  CssClass="textRightAlign" ReadOnly='<%#Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"enableEdit"))==true? false:true%>'>
                                                    </asp:TextBox>
                                                    <asp:HiddenField ID="hdPAmount" Value='<%# DataBinder.Eval(Container.DataItem, "paymentAmt", "{0:c}") %>' runat="server" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalPayAmount" runat="server" Style="float: right; padding-right:5px" Font-Bold="True" />
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>

                                </telerik:RadGrid>
                            </telerik:RadAjaxPanel>
                        </div>
                    </div>
                    <div class="cf"></div>
                </div>
            </div>

        </div>
    </div>

    <asp:HiddenField ID="hdnCon" runat="server" />
      <asp:HiddenField runat="server" ID="hdnMultiDep" Value='0' />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
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

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

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

       function convertNumber(obj) {
            //debugger
            var expression = /[\(\)]/g;

            if (obj.match(expression))     /// check is parentheses exists (negative value)
            {
                return parseFloat(obj.replace(/[\$\(\),]/g, '')) * (-1);
            }
            else {
                return parseFloat(obj.replace(/[\$\(\),]/g, ''));
            }

        }

        function GetInvoiceTotal() {
            var total = 0.00;
            $("[id*=txtPAmount]").each(function () {
                var txtPay = $(this).attr('id')
                var expression = /[\(\)]/g
                var chk = document.getElementById(txtPay.replace('txtPAmount', 'chkSelect'));                
                if ($(chk).prop('checked') == true) {
                    if ($(this).val() != '') {
                      // debugger
                        var val = $(this).val()
                        if (val.match(expression)) {                           
                            total = total - parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                        }
                        else {                            
                            total = total + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));                           
                        }
                    }
                }
            });

            if (total == -0) {
                total = 0;
            }
            var totalval = cleanUpCurrency('$' + total.toLocaleString("en-US", { minimumFractionDigits: 2 }))
            $("[id*=lblTotalPayAmount]").text(totalval);

            var totalAmount = 0.00;
            $("[id*=hdnTotal]").each(function () {
                var expression = /[\(\)]/g
                if ($(this).val() != '') {
                    var val = $(this).val();
                     if (val.match(expression)) {                           
                            totalAmount = totalAmount - parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                        }
                        else {                            
                            totalAmount = totalAmount + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));                           
                    }

                   
                }
            });
            if (totalAmount == -0) {
                totalAmount = 0;
            }
            var totalAmountval = cleanUpCurrency('$' + totalAmount.toLocaleString("en-US", { minimumFractionDigits: 2 }))
            $("[id*=lblTotalOrigAmount]").text(totalAmountval);

            var totalDueAmount = 0.00;                                               


             $("[id*=hdnPrevDue]").each(function () {
                // debugger
                 var txtPay = $(this).attr('id')

                 var lblDueAmount = document.getElementById(txtPay.replace('hdnPrevDue', 'lblDueAmount'));
                 var expression = /[\(\)]/g
                 var chk = document.getElementById(txtPay.replace('hdnPrevDue', 'chkSelect'));

                 var val = lblDueAmount.innerHTML;
                 if (val.match(expression)) {
                     totalDueAmount = totalDueAmount - parseFloat(lblDueAmount.innerHTML.replace(/[\$\(\),]/g, ''));
                 }
                 else {
                     totalDueAmount = totalDueAmount + parseFloat(lblDueAmount.innerHTML.replace(/[\$\(\),]/g, ''));
                 }
            });
            if (totalDueAmount == -0) {
                totalDueAmount = 0;
            }

            var totalDueAmountval = cleanUpCurrency('$' + totalDueAmount.toLocaleString("en-US", { minimumFractionDigits: 2 }))
            $("[id*=lblTotalDueAmount]").text(totalDueAmountval);



        }


        function cleanUpCurrency(s) {
            var expression = '-';
            if (s.match(expression)) {
                return s.replace("$-", "\($") + ")";
            }
            else {
                return s;
            }
        }
    </script>
    <script type="text/javascript">

        function pageLoad(sender, args) {
            Materialize.updateTextFields();
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = document.getElementById('<%=hdnCon.ClientID%>').value;
            }

            $("[id*=chkSelect]").change(function () {              
                var chk = $(this).attr('id');
                var PaidCredit = document.getElementById(chk.replace('chkSelect', 'lblIsCredit'));
                if ($(PaidCredit).text() != '2') {
                    var txtPay = document.getElementById(chk.replace('chkSelect', 'txtPAmount'));
                    var lblDue = document.getElementById(chk.replace('chkSelect', 'lblDueAmount'));
                    var hdnPrevDue = document.getElementById(chk.replace('chkSelect', 'hdnPrevDue'));

                    var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
                    var prevDue = parseFloat($(hdnPrevDue).val())
                    var pay = 0;

                    var rpay = cleanUpCurrency('$' + pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    var rprevDue = cleanUpCurrency('$' + prevDue.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                    if ($(this).prop('checked') == true) {

                        $(txtPay).val(rprevDue)
                        $(lblDue).text(rpay)
                        
                    }
                    else if ($(this).prop('checked') == false) {

                        $(txtPay).val(rpay)
                        $(lblDue).text(rprevDue)
                        $(this).closest('tr').removeAttr("style");
                    }
                    GetInvoiceTotal();
                }
            });

            $("[id*=chkAll]").change(function () {
                $("#<%=RadGrid_gvInvoice.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', $(this).prop("checked"));
                $("#<%=RadGrid_gvInvoice.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                    var chk = $(this).attr('id');

                    var txtPay = document.getElementById(chk.replace('chkSelect', 'txtPAmount'));
                    var lblDue = document.getElementById(chk.replace('chkSelect', 'lblDueAmount'));
                    var hdnPrevDue = document.getElementById(chk.replace('chkSelect', 'hdnPrevDue'));

                    var prevDue = parseFloat($(hdnPrevDue).val())

                    var pay = 0;

                    var rpay = cleanUpCurrency('$' + pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    var rprevDue = cleanUpCurrency('$' + prevDue.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                    if ($(this).prop('checked') == true) {

                        $(txtPay).val(rprevDue)
                        $(lblDue).text(rpay)
                       
                    }
                    else if ($(this).prop('checked') == false) {

                        $(txtPay).val(rpay)
                        $(lblDue).text(rprevDue)
                        $(this).closest('tr').removeAttr("style");
                    }
                });

                GetInvoiceTotal();
            });

            //txtOwnerName
            $("[id*=txtOwnerName]").autocomplete({

                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetCustomerWithInactive",
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
                    var hdnOwner = document.getElementById(txtOwnerName.replace('txtOwnerName', 'hdnOwner'));
                    $(hdnOwner).val(ui.item.value);
                    $(this).val(ui.item.label);

                    var lblLocID = document.getElementById(txtOwnerName.replace('txtOwnerName', 'lblLocID'));
                    var hdnLocID = document.getElementById(txtOwnerName.replace('txtOwnerName', 'hdnLocID'));
                    var txtLocationName = document.getElementById(txtOwnerName.replace('txtOwnerName', 'txtLocationName'));

                    var lblAmount = document.getElementById(txtOwnerName.replace('txtOwnerName', 'lblAmount'));
                    var lblDueAmount = document.getElementById(txtOwnerName.replace('txtOwnerName', 'lblDueAmount'));
                    var hdnAmount = document.getElementById(txtOwnerName.replace('txtOwnerName', 'hdnAmount'));
                    var hdnPrevDue = document.getElementById(txtOwnerName.replace('txtOwnerName', 'hdnPrevDue'));
                    var txtInvoice = document.getElementById(txtOwnerName.replace('txtOwnerName', 'txtInvoice'));
                    var hdnSTax = document.getElementById(txtOwnerName.replace('txtOwnerName', 'hdnSTax'));
                    var hdnTotal = document.getElementById(txtOwnerName.replace('txtOwnerName', 'hdnTotal'));
                      var hdnRefTranID = document.getElementById(txtOwnerName.replace('txtOwnerName', 'hdnRefTranID'));

                    lblLocID.innerHTML = "";
                    hdnLocID.value = "";
                    txtLocationName.value = "";

                    lblAmount.innerHTML = "$0.00";
                    lblDueAmount.innerHTML = "$0.00"
                    hdnAmount.value = 0;
                    hdnPrevDue.value = 0;
                    hdnTotal.value = 0;
                    hdnSTax.value = 0;
                    txtInvoice.value = "";
                    hdnRefTranID.value = "";
                    GetInvoiceTotal();
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

            $.each($(".txtOwnerNameSearch"), function (index, item) {
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
                        url: "CustomerAuto.asmx/GetLocationWithInactive",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                                showNoty('Due to unexpected errors we were unable to load job code', 'warning');
                          
                        }
                    });
                },
                select: function (event, ui) {
                  // debugger
                    var txtLocationName = this.id;
                    var lblLocID = document.getElementById(txtLocationName.replace('txtLocationName', 'lblLocID'));
                    var hdnLocID = document.getElementById(txtLocationName.replace('txtLocationName', 'hdnLocID'));

                     var txtOwnerName = document.getElementById(txtLocationName.replace('txtLocationName', 'txtOwnerName'));
                    var hdnOwner = document.getElementById(txtLocationName.replace('txtLocationName', 'hdnOwner'));

                    lblLocID.innerHTML = ui.item.ID;
                    hdnLocID.value = ui.item.ID;

                      txtOwnerName.value = ui.item.CompanyName;
                    hdnOwner.value = ui.item.OwnerID;
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
            $("[id*=txtPAmount]").change(function () {             

                var amt = convertNumber($(this).val().replace("$", ''))
                amt = parseFloat(amt)

                var amtval = cleanUpCurrency('$' + amt.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                $(this).val(amtval)

                var txtPay = $(this).attr('id');
                var lblDue = document.getElementById(txtPay.replace('txtPAmount', 'lblDueAmount'));
                var hdnPrevDue = document.getElementById(txtPay.replace('txtPAmount', 'hdnPrevDue'));
                var chk = document.getElementById(txtPay.replace('txtPAmount', 'chkSelect'));
                //var pay = $(this).val().toString().replace(/[\(,]/g, '');
                //var pay = pay.toString().replace(/[\$\),]/g, '');
                pay = amt;
                if (pay == '') {
                    pay = 0;
                    $(this).val('$' + pay.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                }

                if (pay != 0) {

                    pay = parseFloat(pay);
                    var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
                    var prevDue = parseFloat($(hdnPrevDue).val())
                    var IsNeg = false;

                    if (pay < 0) {
                        IsNeg = true;
                        pay = pay * -1;
                        prevDue = prevDue * -1;
                    }


                    due = prevDue - pay;
                    if (IsNeg) {
                        pay = pay * -1;
                        prevDue = prevDue * -1;
                        due = due * -1;
                    }

                   // var rPay = cleanUpCurrency('$' + pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    var rDue = cleanUpCurrency('$' + due.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                   // $(this).val(rPay);
                    $(lblDue).text(rDue);
                    $(chk).prop('checked', true);
                   
                }
                else {
                    $(chk).prop('checked', false);
                    $(this).closest('tr').removeAttr("style");
                }
               var lblDue = document.getElementById(txtPay.replace('txtPAmount', 'lblDueAmount'));
                var txtInvoice = document.getElementById(txtPay.replace('txtPAmount', 'txtInvoice'));
                var line = document.getElementById(txtPay.replace('txtPAmount', 'hdnLine'));
                var hdnReceiptID = document.getElementById(txtPay.replace('txtPAmount', 'hdnReceiptID'));
                if (hdnReceiptID.value == "0") {
                    UpdateDueAmount(line.value,txtInvoice.value,amt,hdnPrevDue.value);
                }
                  
                GetInvoiceTotal();
            });
            Materialize.updateTextFields();
        }


        function GetTextBoxValue(sender, eventArgs) {
           
            var obj = sender._element;
            var hdnOwner = document.getElementById(obj.id.replace('ddlInvoice', 'hdnOwner'));
            var lblLocID = document.getElementById(obj.id.replace('ddlInvoice', 'lblLocID'));
            var context = eventArgs.get_context();
            context["contextHdnOwner"] = hdnOwner.value;
            context["contextHdnLocID"] = lblLocID.innerHTML;
        }

        function dtaa() {
            this.prefixText = null;
            this.con = document.getElementById('<%=hdnCon.ClientID%>').value;
        }
        function UpdateAmount(sender) {

            var txtInvoice = sender.id;
            var lblAmount = sender.id.replace('txtInvoice', 'lblAmount');
            var lblDueAmount = sender.id.replace('txtInvoice', 'lblDueAmount');
            var lblAmount = sender.id.replace('txtInvoice', 'lblAmount');
            var lblPretax = sender.id.replace('txtInvoice', 'lblPretax');
            var lblStax = sender.id.replace('txtInvoice', 'lblSalesTax');
            var hdnSTax = sender.id.replace('txtInvoice', 'hdnSTax');
            var hdnTotal = sender.id.replace('txtInvoice', 'hdnTotal');
            var hdnAmount = sender.id.replace('txtInvoice', 'hdnAmount');
            var txtPAmount = sender.id.replace('txtInvoice', 'txtPAmount');
            var hdnPrevDue = sender.id.replace('txtInvoice', 'hdnPrevDue');

            var txtOwnerName = sender.id.replace('txtInvoice', 'txtOwnerName');
            var hdnOwner = sender.id.replace('txtInvoice', 'hdnOwner');

            var txtLocationName = sender.id.replace('txtInvoice', 'txtLocationName');
            var hdnLocID = sender.id.replace('txtInvoice', 'hdnLocID');
            var lblLocID = sender.id.replace('txtInvoice', 'lblLocID');
                 var hdnLine = sender.id.replace('txtInvoice', 'hdnLine');
             var hdnInvoice = sender.id.replace('txtInvoice', 'hdnInvoice');
            var hdnReceiptID = sender.id.replace('txtInvoice', 'hdnReceiptID');
              var hdnRefTranID = sender.id.replace('txtInvoice', 'hdnRefTranID');
            var dtaaa = new dtaa();
            dtaaa.prefixText = document.getElementById(txtInvoice).value;
            if (dtaaa.prefixText.trim() != '') {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "CustomerAuto.asmx/GetAmountInvoice",
                    data: JSON.stringify(dtaaa),
                    dataType: "json",
                    async: true,
                    success: function (data) {

                        var obj = JSON.parse(data.d);
                        if (obj.length > 0) {
                            var amount = obj[0].Amount;
                            var dueAmount = obj[0].AmountDue;
                            var sTax = obj[0].STax;
                            var Total = obj[0].Total;
                            document.getElementById(txtInvoice).value = obj[0].Invoice;
                            document.getElementById(lblStax).innerHTML = '$' + sTax.toLocaleString("en-US", { minimumFractionDigits: 2 });
                            document.getElementById(lblPretax).innerHTML = '$' + amount.toLocaleString("en-US", { minimumFractionDigits: 2 });
                            document.getElementById(lblDueAmount).innerHTML = '$0.00';
                            document.getElementById(lblAmount).innerHTML = '$' + Total.toLocaleString("en-US", { minimumFractionDigits: 2 });
                           // document.getElementById(hdnPrevDue).value = dueAmount ;

                           var SumPaymentAmount= getDueAmount(document.getElementById(hdnLine).value,obj[0].Invoice,dueAmount);

                            document.getElementById(hdnPrevDue).value = (dueAmount - SumPaymentAmount);

                            document.getElementById(hdnAmount).value = amount;
                            document.getElementById(hdnSTax).value = sTax;
                            document.getElementById(hdnTotal).value = Total;
                            document.getElementById(txtPAmount).value = '$' + (dueAmount-SumPaymentAmount).toLocaleString("en-US", { minimumFractionDigits: 2 }); ;

                            document.getElementById(txtOwnerName).value = obj[0].OwnerName;
                            document.getElementById(hdnOwner).value = obj[0].Owner;

                            document.getElementById(txtLocationName).value = obj[0].LocationName;
                            document.getElementById(hdnLocID).value = obj[0].LocID;
                            document.getElementById(lblLocID).innerHTML = obj[0].LocID;
                             document.getElementById(hdnRefTranID).value = obj[0].RefTranID;

                            GetInvoiceTotal();
                        } else {
                            if ( document.getElementById(hdnInvoice).value == "" && document.getElementById(hdnReceiptID).value == "0") {
                                document.getElementById(txtInvoice).value = "";
                                document.getElementById(lblDueAmount).innerHTML = '$0.00';
                                document.getElementById(lblAmount).innerHTML = '$0.00';
                                document.getElementById(hdnPrevDue).value = 0;
                                document.getElementById(hdnAmount).value = 0;
                                document.getElementById(hdnSTax).value = 0;
                                document.getElementById(hdnTotal).value = 0;
                                document.getElementById(txtPAmount).value = 0;

                                document.getElementById(txtOwnerName).value = "";
                                document.getElementById(hdnOwner).value = 0;
                                document.getElementById(txtLocationName).value = "";
                                document.getElementById(hdnLocID).value = "";
                                document.getElementById(lblLocID).innerHTML = "";
                                document.getElementById(hdnRefTranID).value = "";
                                //noty({ text: 'Invoice invalid', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                                GetInvoiceTotal();
                            } else {
                                document.getElementById(txtInvoice).value  = document.getElementById(hdnInvoice).value;
                            }
                       
                        }
                    },
                    error: function (result) {
                          showNoty('Due to unexpected errors we were unable to load job code', 'warning');
                     
                    }
                });

            } else {
                            document.getElementById(txtInvoice).value = "";
                            GetInvoiceTotal();
                        }

        }

        function SetContextKey(sender) {
          //  debugger
            var AutoCompleteExtender = sender.id.replace('txtInvoice', 'AutoCompleteExtender');
            try {
                var hdnOwner = document.getElementById(sender.id.replace('txtInvoice', 'hdnOwner'));
                var lblLocID = document.getElementById(sender.id.replace('txtInvoice', 'lblLocID'));
                $find(AutoCompleteExtender).set_contextKey(hdnOwner.value + "$" + lblLocID.innerHTML);
            } catch (e) {
                $find(AutoCompleteExtender).set_contextKey("0$");
            }
        }

        function AddNewRows(sender, eventArgs) {
            var $focused = $(':focus');
            var flag=0
           
            if ($focused[0].id.indexOf("txtOwnerName") !== -1 || $focused[0].id.indexOf("txtLocationName") !== -1 || $focused[0].id.indexOf("txtInvoice") !== -1|| $focused[0].id.indexOf("txtPAmount") !== -1) {
                flag=1
            }

            if (eventArgs.get_keyCode() == 40) {
                if (flag == 0) {
                     document.getElementById('<%=btnAddnewRow.ClientID%>').click();
                }
               
             
            }

        }
        
        function validateGrid() {          
           
        
           // Confirm();
            var countItem = 0;
            try {
              
                 var isEmptyLocation = true;
              $("#<%=RadGrid_gvInvoice.ClientID %>").find('tr:not(:first,:last)').each(function () {
                try {
                    var $tr = $(this);
                    var temp = $tr.find('input[id*=hdnLocID]').val();                   
                    if (temp!=undefined && (temp == "0" || temp == "")) {
                          isEmptyLocation= false;
                      
                    }                   
                } catch (ex) { }           

            });  

                if (isEmptyLocation == false) {
                   showNoty('Location is empty', 'warning');
                 
                return false;
                }

                //check selected item
                $("#<%=RadGrid_gvInvoice.ClientID %>").find('tr:not(:first,:last)').each(function () {
                    var $tr = $(this);
                    $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                        countItem++;
                    });
                });
              // debugger
                if (countItem == 0) {
                    showNoty('Please select item.', 'warning');
                   
                    return false;
                } else {
                     if (ValidateLess() == true) {
                                showNoty('Due Amount should equal Payment Amount Total for multi locations.', 'warning');
                           
                            return false;
                        } else {
                            if ($("#<%=chkCreateDeposit.ClientID %>").prop('disabled') == false) {

                                if ($("#<%=chkCreateDeposit.ClientID %>").prop('checked') == true) {
                                    if ($("#<%=ddlBank.ClientID %>").val() == 0) {
                                        showNoty('Please select a bank', 'warning');

                                        return false;
                                    }
                                }
                            }
                                                                      
                        }
                   <%-- //check double invoice                    
                    if (find_duplicateInvoice() > 0) {
                          showNoty('Duplicate invoice.', 'warning');
                     
                        return false;
                    } else {
                        if (ValidateLess() == true) {
                                showNoty('Due Amount should equal Payment Amount Total for multi locations.', 'warning');
                           
                            return false;
                        } else {
                            if ($("#<%=chkCreateDeposit.ClientID %>").prop('disabled') == false) {

                                if ($("#<%=chkCreateDeposit.ClientID %>").prop('checked') == true) {
                                    if ($("#<%=ddlBank.ClientID %>").val() == 0) {
                                        showNoty('Please select a bank', 'warning');

                                        return false;
                                    }
                                }
                            }
                                                                      
                        }
                    }--%>
                }
                return true;
            } catch (ex) {
                return false;
            }

        }
        function showNoty(msg, type) {
            var notyclose_id = $("#noty_topCenter_layout_container>li:first-child>.noty_bar").attr('id');
            var noty_list_count = $("#noty_topCenter_layout_container li").size();
            if (noty_list_count > 0) {
                $.noty.close(notyclose_id);
            }
            if (noty_list_count == 0) {
                noty({ text: msg, type: type, layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: true, dismissQueue: true });
            }
        }
        function find_duplicateInvoice() {
            var arrInvoice = [];
            var strInvoice = "";
            $("[id$='RadGrid_gvInvoice']").find('tbody tr').each(function () {
                try {
                    var $tr = $(this);
                    if ($tr.find('input[id*=txtInvoice]').attr('id') != "" && typeof $tr.find('input[id*=txtInvoice]').attr('id') != 'undefined') {
                        var temp = new Array();
                        strInvoice = $tr.find('input[id*=txtInvoice]').val();
                        temp = strInvoice.split(",");
                        for (var i = 0; i < temp.length; i++) {
                            arrInvoice.push(temp[i]);
                        }
                    }
                } catch (e) {

                }
            });
            var object = {};
            var result = [];

            arrInvoice.forEach(function (item) {
                if (!object[item])
                    object[item] = 0;
                object[item] += 1;
            })

            for (var prop in object) {
                if (object[prop] >= 2) {
                    result.push(prop);
                }
            }
            return result;
        }
        function ValidateLess() {
            //debugger
            var flag = false;
             $("#<%=RadGrid_gvInvoice.ClientID %>").find('tbody tr').each(function () {
                try {
                    var $tr = $(this);
                    if ($tr.find('input[id*=hdnPrevDue]').attr('id') != "" && typeof $tr.find('input[id*=hdnPrevDue]').attr('id') != 'undefined') {
                        //var amountPay = $tr.find('input[id*=txtPAmount]').val().replace(/[\$\(\),]/g, '');
                        //var amountDue = $tr.find('input[id*=hdnPrevDue]').val().replace(/[\$\(\),]/g, '');
                        var txtPAmount = $tr.find('input[id*=txtPAmount]');
                        var hdnPrevDue = $tr.find('input[id*=hdnPrevDue]');
                        var amountPay = txtPAmount.val().replace("$", '')
                        amountPay = parseFloat(convertNumber(amountPay))
                        var amountDue = convertNumber(hdnPrevDue.val().replace("$", ''))
                        amountDue = parseFloat(amountDue)

                        if (parseFloat(amountPay) != parseFloat(amountDue)) {
                            var temp = new Array();
                            strInvoice = $tr.find('input[id*=txtInvoice]').val();

                            if ((strInvoice != "0" && strInvoice != "") && parseFloat(amountPay) < 0) {
                                flag = true;
                            } else {
                                temp = strInvoice.split(",");
                                var tempLocation = new Array();
                                strLocation = $tr.find('input[id*=txtLocationName]').val();

                                tempLocation = strLocation.split(";");
                                if (temp.length > 1) {
                                    if (tempLocation.length > 1) {
                                        flag = true;
                                    }
                                }
                            }

                        }

                    }
                } catch (e) {
                    return false;
                }
            });
            return flag;
        }
        function Confirm() {          

            if (document.getElementById('<%=chkCreateDeposit.ClientID %>').checked == true) {
                var Val;
                if (confirm("Do you want to print?") == true) {
                    Val = "Yes";
                } else {
                    Val = "No";
                }

                document.getElementById("Confirm_Value").value = Val;
            }
        }

        function confirmDelete() {
              var count = 0;
             $("#<%=RadGrid_gvInvoice.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                    var chk = $(this).attr('id');

               
                    if ($(this).prop('checked') == false) {

                        count = count + 1;
                    }
                    
            });
            if (count == 0) {
                alert("There is no unchecked item to detele.");
                return false;
            } else {
              return   confirm("Are you sure you want to delete unchecked rows?")
            }
        }
    </script>
     <script type="text/javascript">
             Sys.Application.add_init(appl_init);

             function appl_init() {

                 var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
                 pgRegMgr.add_beginRequest(BlockUI);
                 pgRegMgr.add_endRequest(UnblockUI);
             }

             function BlockUI(sender, args) {
                var control =document.getElementById('<%=txtlsInvoice.ClientID%>');
                 if (args._postBackElement.id != control.id) {
                     document.getElementById("overlay").style.display = "block";
                 }
                // document.getElementById("overlay").style.display = "block";
             }
             function UnblockUI(sender, args) {               
                document.getElementById("overlay").style.display = "none";

             }
         </script>
     <script>
        function getDueAmount(line, Ref, dueAmount) {
           
            var sumPayAmount = 0;
            $("#<%=RadGrid_gvInvoice.ClientID %>").find('tbody tr').each(function () {
                try {
                    var $tr = $(this);
                    var txtInvoice = $tr.find('input[id*=txtInvoice]');
                    var txtPAmount = $tr.find('input[id*=txtPAmount]');                  
                    var hdnLine = $tr.find('input[id*=hdnLine]');
                    if (parseFloat(hdnLine.val()) < parseFloat(line)) {
                        strInvoice = txtInvoice.val();
                        temp = strInvoice.split(",");
                        if (temp.length > 1) {
                            for (var i = 0; i < temp.length; i++) {
                                if (temp[i] == Ref) {
                                    if (txtPAmount.val() != '') {
                                        sumPayAmount = sumPayAmount + parseFloat(dueAmount);
                                    }
                                }
                            }
                        } else {
                            if (txtInvoice.val() == Ref) {
                                if (txtPAmount.val() != '') {
                                    sumPayAmount = sumPayAmount + parseFloat(txtPAmount.val().replace(/[\$\(\),]/g, ''));
                                }
                            }
                        }
                    }
                } catch (e) {

                }
            });
            return sumPayAmount;
        }

         function UpdateDueAmount(line, Ref, PayAmount, DueAmount) {
             var flag = false;
            var LastAmount =  parseFloat(DueAmount) - parseFloat(PayAmount);
            $("#<%=RadGrid_gvInvoice.ClientID %>").find('tbody tr').each(function () {
                try {
                  //  debugger
                    var $tr = $(this);
                    var txtInvoice = $tr.find('input[id*=txtInvoice]');
                    var hdnPrevDue = $tr.find('input[id*=hdnPrevDue]');      
                     var txtPAmount = $tr.find('input[id*=txtPAmount]');      
                    var hdnLine = $tr.find('input[id*=hdnLine]');
                    var chkSelect = $tr.find('input[id*=chkSelect]');
                    var lblDueAmount = $tr.find('span[id*=lblDueAmount]');
                   

                    if (parseFloat(hdnLine.val()) > parseFloat(line)) {
                        if (txtInvoice.val() == Ref) {     
                            
                            if (LastAmount < 0) {
                                hdnPrevDue.val(0);
                            } else {
                                hdnPrevDue.val(LastAmount);
                                lblDueAmount.text (cleanUpCurrency('$' + LastAmount.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                                txtPAmount.val("$0.00");
                                 LastAmount = LastAmount - parseFloat(txtPAmount.val().replace(/[\$\(\),]/g, ''));
                            }
                            chkSelect.prop("checked", false);
                            flag = true;
                        }                       
                    }
                } catch (e) {

                }
            });
            if (flag == true) {
                var str='We have detected multiple payments for this invoice '+Ref+ ' . Please review all payments.'
                 noty({ text: str, type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }
        }
    </script>
</asp:Content>
