<%@ Page Title="" Language="C#" MasterPageFile="~/MOM.master" AutoEventWireup="true"
    Inherits="AddReceivePayment" Codebehind="AddReceivePayment.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"> 
    <style>
        .highlight {
            background-color: Yellow;
        }

        .highlighted {
            background-color: Yellow;
        }
        .RadGrid_Material th a {
            padding:0;
            
        }
        .rgHeader .collapsible-body a {
            padding: 0px!important;
            font-size: 0.8rem!important;
        }
       
        .rgHeaderWrapper .RadGrid_Material .rgHeader {
            padding: 5px 8px!important;
        }
        .RadGrid_Material .rgHeader {
             padding: 5px 4px!important;
        }
        .collapsible-body a {
            padding: 0 0px 0 2px !important;
        font-size: 0.8rem;
        }
    </style>
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    
    <script type="text/javascript">
        function showMessageAndRedirect(strMsg, type, url) {
             noty({
                 text: strMsg,
                 type: type,
                 layout: 'topCenter',
                 closeOnSelfClick: false,
                 timeout: false,
                 theme: 'noty_theme_default',
                 closable: true
             });

             setTimeout(function () {
                 window.location.replace(url);
             }, 1000); //will call the function after 1 secs.

         }
       
        function isNumberKey(evt, txt) {

            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        var isSave = false;
     
        function VisibleRowOnFocus(txt) {  //To make row's textbox visible
           
            $('#<%=RadGrid_gvInvoice.ClientID %> input:text.non-trans').each(function () {

                $(this).removeClass("non-trans");
                $(this).addClass("texttransparent");
            });
            $('#<%=RadGrid_gvInvoice.ClientID %> select.non-trans').each(function () {

                $(this).removeClass("non-trans");
                $(this).addClass("texttransparent");

            });

            var txtPAmount = document.getElementById(txt.id);
            $(txtPAmount).removeClass("texttransparent");
            $(txtPAmount).addClass("non-trans");
        }
        function isDecimalKey(el, evt) {

            //var re = /^-?\d*\.?\d{0,6}$/;
            //var text = $(evt).val();

            //var isValid = (text.match(re) !== null);
            //if (isValid)
            //    return true;
            //else
            //    return false;

            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 46)
                return false;

            return true;

        }
        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }
        function ChkCustID(sender, args) {
            var hdnCustID = document.getElementById('<%= hdnCustID.ClientID %>');
            if (hdnCustID.value == '') {
                args.IsValid = false;
            }
            else if (hdnCustID.value == '0') {
                args.IsValid = false;
            }
        }

        function ChkLocID(sender, args) {
           
            var total = 0.00;
            var expression = /[\(\)]/g                     // to identify parentheses
            $("[id*=txtPAmount]").each(function () {
                var txtPay = $(this).attr('id')
                //var expression = /^\$?\(?[\d,\.]*\)?$/;      // to identify parentheses and $

                var chk = document.getElementById(txtPay.replace('txtPAmount', 'chkSelect'));
                if ($(chk).prop('checked') == true) {
                    if ($(this).val() != '') {

                        var val = $(this).val()

                        if (val.match(expression))     /// check is parentheses exists (negative value)
                        {
                            total = total - parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                        }
                        else {
                            total = total + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                        }
                    }
                }
            });
            var vartotal = 0;
            if ($("#<%=txtAmount.ClientID %>").val().match(expression))     /// check is parentheses exists (negative value)
            {
                vartotal = parseFloat($("#<%=txtAmount.ClientID %>").val().replace(/[\$\(\),]/g, '')) * -1;
            }
            else {
                vartotal = parseFloat($("#<%=txtAmount.ClientID %>").val().replace(/[\$\(\),]/g, ''));
            }
            if (total != vartotal) {
                var hdnLocID = document.getElementById('<%= hdnLocID.ClientID %>');
                if (hdnLocID.value == '') {
                    args.IsValid = false;
                    //alert('invalid')
                }
                else if (hdnLocID.value == '0') {
                    args.IsValid = false;
                    //alert('invalid')
                }
            }

        }
    </script>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(function () {
                $('.js-amt').keypress(function (e) {
                    if (e.which != 46 && e.which != 45 && e.which != 46 &&
                        !(e.which >= 48 && e.which <= 57)) {
                        return false;
                    }
                });
                var query = "";

                $("#<%=txtCustomer.ClientID %>").keyup(function (event) {
                    var hdnCustID = document.getElementById('<%=hdnCustID.ClientID %>');
                    if (document.getElementById('<%=txtCustomer.ClientID %>').value == '') {
                        hdnCustID.value = '';
                    }
                });

                $("#<%=txtLocation.ClientID %>").keyup(function (event) {
                    var hdnLocId = document.getElementById('<%=hdnLocID.ClientID %>');
                     var hdnLocStatus = document.getElementById('<%=hdnLocStatus.ClientID %>');
                    if (document.getElementById('<%=txtLocation.ClientID %>').value == '') {
                        hdnLocId.value = '';
                         hdnLocStatus.value = '';
                    }
                });
            });

        }
        function cleanUpCurrency(s) {

            var expression = '-';

            //Check if it is in the proper format
            if (s.match(expression)) {
                //It matched - strip out - and append parentheses 
                return s.replace("$-", "\($") + ")";

            }
            else {
                return s;
            }
        }

    </script>




   
    <%--    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>--%>

    <style>
        .rptSti tr:nth-child(2n+1) {
            background: none !important;
        }
        .rm_mm td {
            background-color: none !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-editor-attach-money"></i>&nbsp;<asp:Label ID="lblHeader" runat="server">Add Receive Payment</asp:Label></div>
                                    
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                        <asp:LinkButton ToolTip="Save" ID="btnSubmit" runat="server"  OnClick="btnSubmit_Click" ValidationGroup="Payment" OnClientClick="return ValidateZero();">Save</asp:LinkButton>
                                    </div>
                                        <div class="btnlinks" >
                                            <asp:LinkButton ID="lnkPrint" runat="server" ToolTip="Print" CausesValidation="true" OnClick="lnkPrint_Click" >Report</asp:LinkButton>
                                             
                                        </div>
                                     <div class="btnlinks" >                                           
                                              <asp:LinkButton ID="lnkWriteOff" runat="server" ToolTip="Write Off" CausesValidation="true" OnClientClick="return OpenWriteOffWindow();disableAccountWriteOff();" OnClick="lnkWriteOff_Click" >Write off</asp:LinkButton>                                        
                                         
                                        </div>
                                          <div class="btnlinks" >                                           
                                              <asp:LinkButton ID="lnkTranfer" runat="server" ToolTip="Transfer Payment" CausesValidation="true" OnClientClick=" OpenTransferPaymentWindow(); return false;" >Transfer Payment</asp:LinkButton>                                        
                                         
                                        </div>
                                        <div class="btnlinks" >                                           
                                              <asp:LinkButton ID="lnkCredit" runat="server" ToolTip="Credit" OnClientClick="return OpenCreditWindow();" OnClick="lnkCredit_Click" CausesValidation="true" >Credit</asp:LinkButton>                                        
                                         
                                        </div>
                                         <div class="btnlinks" >                                           
                                              <asp:LinkButton ID="lnkUnApply" runat="server" ToolTip="Unapply" CausesValidation="true" OnClientClick=" return OpenUnapply(); " OnClick="lnkUnApply_Click" >Unapply</asp:LinkButton>                                        
                                         
                                        </div>
                                    </div>
                                       <div class="buttonContainer">
                                       
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ToolTip="Close" ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click">
                                                    <i class="mdi-content-clear"></i>
                                        </asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <div class="editlabel" id="trDeposit" runat="server" visible="false">
                                            Deposit #:&nbsp;<asp:HyperLink ID="lnkDepID" runat="server" Target="_blank"></asp:HyperLink>                                                                            

                                        </div>
                                        <div class="editlabel" style="display: none" runat="server" id="divlblRef">
                                            <asp:Label ID="lblReceiveRef" runat="server" Text="Ref #"></asp:Label>
                                            <asp:Label ID="lblReceiveID" runat="server"></asp:Label>
                                        </div>

                                    </div>
                                    <div style="clear:both;"></div>
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
                                    <li><a href="#accrdPaymentTab">Payment Info</a></li>
                                    <li><a href="#accrdOutstanding">Outstanding Payments</a></li>
                                    <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlNext" runat="server">
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False"
                                                OnClick="lnkFirst_Click">
                                                <i class="fa fa-angle-double-left"></i>
                                            </asp:LinkButton></span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False"
                                                OnClick="lnkPrevious_Click">
                                                <i class="fa fa-angle-left"></i>
                                            </asp:LinkButton></span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False"
                                                OnClick="lnkNext_Click">
                                                <i class="fa fa-angle-right"></i>
                                            </asp:LinkButton></span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False" CssClass="icon-last"
                                                OnClick="lnkLast_Click">
                                                <i class="fa fa-angle-double-right"></i>
                                            </asp:LinkButton></span>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>


        </div>
    </div>
    <div class="container accordian-wrap">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <telerik:RadAjaxManager ID="RadAjaxManager_gvInvoice" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="RadAjaxManager_gvInvoice">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoice" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
 <telerik:AjaxSetting AjaxControlID="RadGrid_Location">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Location" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="btnSelectCustomer">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoice" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Location" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="btnSelectLoc">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoice" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>  
                            <telerik:AjaxSetting AjaxControlID="btnSelectDate">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoice" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />                           
                                </UpdatedControls>
                            </telerik:AjaxSetting> 
                            <telerik:AjaxSetting AjaxControlID="ddlFilter">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoice" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="btnSubmit">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoice" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelWriteOff" />
                                    <telerik:AjaxUpdatedControl ControlID="hdnIsWriteOff"/>
                                    <telerik:AjaxUpdatedControl ControlID="hdnIsCredit"/>
                                    <telerik:AjaxUpdatedControl ControlID="txtAmount" />
                                    <telerik:AjaxUpdatedControl ControlID="hdnAmountSelected" />  
                                    <telerik:AjaxUpdatedControl ControlID="divSuccess"  />
                                     <telerik:AjaxUpdatedControl ControlID="lblCustomerBalance" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                           <telerik:AjaxSetting AjaxControlID="lnkWriteOff">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelWriteOff" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                                     <telerik:AjaxUpdatedControl ControlID="hdnIsWriteOff"  />
                                    
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkCredit">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCredit" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                                     <telerik:AjaxUpdatedControl ControlID="hdnIsCredit"  />
                                    
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSaveWriteOff">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoice" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelWriteOff"/>
                                    <telerik:AjaxUpdatedControl ControlID="divSuccess"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSaveCredit">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoice" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelCredit"/>
                                    <telerik:AjaxUpdatedControl ControlID="divSuccess"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                             <telerik:AjaxSetting AjaxControlID="lnkTranfer">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelTransfer" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                                    
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                             <telerik:AjaxSetting AjaxControlID="lnkSaveTransfer">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoice" LoadingPanelID="RadAjaxLoadingPanel_gvInvoice" />
                                    <telerik:AjaxUpdatedControl ControlID="RadAjaxTransferPanel"/>
                                     <telerik:AjaxUpdatedControl ControlID="lblCustomerBalance" />
                                    <telerik:AjaxUpdatedControl ControlID="txtAmount" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_gvInvoice" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li>
                            <div id="accrdPaymentTab" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-action-info"></i>Payment Info</div>
                            <div class="alert alert-success" runat="server" id="divSuccess">
                                <button type="button" class="close" data-dismiss="alert">×</button>
                                These month/year period is closed out. You do not have permission to add/update this record.
                            </div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Content" runat="server" EnableAJAX="true">

                                        <div class="form-content-pd">
                                            <div class="form-section-row">

                                                <div class="form-section3">
                                                     <div class="input-field col s5">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Filter option</label>
                                                            <asp:DropDownList ID="ddlFilter"   class="browser-default" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged"  >
                               
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="input-field2 col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                                <asp:TextBox ID="txtInvoice" runat="server" CssClass="searchinput_invoice clearable"
                                                                onkeypress="return isNumberKey(event,this)" placeholder="Search by Invoice#">
                                                            </asp:TextBox>
                                                            <asp:Label runat="server" ID="lbltxtInvoice" AssociatedControlID="txtInvoice">Invoice #</asp:Label>
                                                            </div>
                                                        </div>
                                                      <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Customer</label>
                                                            <asp:DropDownList ID="ddlCustomer"  class="browser-default" runat="server" onchange="ddl_changedCustomer()" >
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row" id="divCustomer" runat="server">
                                                            <asp:Button ID="btnSelectCustomer" runat="server" Text="Button" Style="display: none;" OnClick="btnSelectCustomer_Click" />
                                                            <asp:Button ID="btnSelectLoc" runat="server" Text="Button" Style="display: none;" OnClick="btnSelectLoc_Click" />
                                                            <asp:Button ID="btnSelectDate" runat="server" Text="Button" Style="display: none;" OnClick="btnSelectDate_Click" />
                                                            
                                                            <asp:HiddenField ID="hdnDateValidate" runat="server" />
                                                            <asp:HiddenField ID="hdnOwner" runat="server" />
                                                            <asp:HiddenField ID="hdnButtonClick" runat="server" />
                                                            <asp:HiddenField ID="hdnDate" runat="server" />
                                                            <input id="hdnCon" runat="server" type="hidden" />
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvCustomer" ControlToValidate="txtCustomer"
                                                                ErrorMessage="Please select Customer" Display="None" ValidationGroup="Payment">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="vceCustomer" runat="server" Enabled="True" PopupPosition="Right"
                                                                TargetControlID="rfvCustomer" />
                                                            <asp:CustomValidator ID="cvCustomer" runat="server" ControlToValidate="txtCustomer" ValidationGroup="Payment"
                                                                ErrorMessage="Please select the existing Customer" ClientValidationFunction="ChkCustID"
                                                                Display="None">
                                                            </asp:CustomValidator>
                                                            <asp:ValidatorCalloutExtender ID="vceCustomer1" runat="server" Enabled="True"
                                                                TargetControlID="cvCustomer">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:TextBox ID="txtCustomer" runat="server" class="clearable"
                                                                autocomplete="off" placeholder="Search by customer name, phone#, address etc.">
                                                            </asp:TextBox>
                                                            <asp:HiddenField ID="hdnCustID" runat="server" />
                                                            <asp:Label runat="server" ID="Label2" AssociatedControlID="txtCustomer">Customer</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:CustomValidator ID="cvLocation" runat="server" ControlToValidate="txtLocation" ValidationGroup="Payment"
                                                                ErrorMessage="Check/Payment amount and total payment mismatched. Location name is required" SetFocusOnError="true"
                                                                Display="None" ValidateEmptyText="true" OnServerValidate="cvLocation_ServerValidate">
                                                            </asp:CustomValidator>
                                                            <asp:ValidatorCalloutExtender ID="vceLocation1" runat="server" Enabled="True"
                                                                TargetControlID="cvLocation">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:TextBox ID="txtLocation" runat="server"
                                                                autocomplete="off" placeholder="Search by location name, phone#, address etc." CssClass="searchinputloc ui-autocomplete-input clearable">
                                                            </asp:TextBox>
                                                            <asp:HiddenField ID="hdnLocID" runat="server" />
                                                             <asp:HiddenField ID="hdnLocStatus" runat="server" />
                                                            <asp:Label runat="server" ID="Label1" AssociatedControlID="txtLocation">Location</asp:Label>
                                                        </div>
                                                    </div>
                                                    <%--<div class="input-field col s12">
                                                        <div class="row">
                                                            
                                                        </div>
                                                    </div>--%>
                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <div class="input-field2 col s5">
                                                        <div class="row">
                                                            
                                                            <asp:TextBox ID="txtDate" CssClass="datepicker_mom" runat="server" autocomplete="off" onchange="changePayDate();"></asp:TextBox>
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
                                                            <label for="txtDate" class="date-label">Date</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field2 col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Payment Method</label>
                                                            <asp:DropDownList ID="ddlPayment" class="browser-default" runat="server" onchange="ddl_changed(this)">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtCheck" runat="server"
                                                                autocomplete="off" Text=" ">
                                                            </asp:TextBox>
                                                            <label for="txtCheck" id="lblCheck" class="active">Check #</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field2 col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"
                                                                MaxLength="15" autocomplete="off" Style="text-align: right" >
                                                            </asp:TextBox>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvAmount" ControlToValidate="txtAmount"
                                                                ErrorMessage="Please enter Amount." Display="None"
                                                                ValidationGroup="Payment">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="vceAmount" runat="server" Enabled="True" PopupPosition="Right"
                                                                TargetControlID="rfvAmount" />
                                                            <label for="lblAmount" id="lblAmount" class="active">Check Amount</label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtMemo" runat="server" autocomplete="off"></asp:TextBox>
                                                            <asp:RequiredFieldValidator runat="server" ID="rfvMemo" ControlToValidate="txtMemo"
                                                                ErrorMessage="Please enter Memo." Display="None"
                                                                ValidationGroup="Payment">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="vceMemo" runat="server" Enabled="True" PopupPosition="TopLeft"
                                                                TargetControlID="rfvMemo" />
                                                            <label for="txtMemo">Memo</label>
                                                        </div>
                                                        <div id="dvCompanyPermission" runat="server" style="display: none">
                                                            <asp:TextBox ID="txtCompany" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                            <label for="txtCompany">Company</label>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>

                                                <div class="form-section3" >
                                                    <div class="input-field col s5">
                                                        <div class="row">
                                                            <label class="active">Customer Balance</label>
                                                            <%--<asp:Label ID="" runat="server" Style="float: right !important; font-size: 0.9em; color: #000 !important;"></asp:Label>--%>
                                                              <asp:TextBox ID="lblCustomerBalance" runat="server" CssClass="form-control"
                                                                MaxLength="15" autocomplete="off" Style="text-align: right" ></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="input-field2 col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                     <div class="input-field col s5">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtStatus" runat="server" style="margin-top:7px!important"></asp:TextBox>
                                                            <label for="txtStatus">Status</label>
                                                            </div>
                                                         </div>

                                                    <div class="input-field col s12 ">
                                                        <div class="row">
                                                            <label class="active">Deposit to:</label>
                                                             <asp:TextBox ID="lblDepositTo" runat="server" CssClass="form-control"
                                                                MaxLength="15" autocomplete="off" Style="text-align: right" ></asp:TextBox>
                                                        </div>
                                                    </div>
                                                   
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            
                                                        </div>
                                                    </div>
                                                    
                                                </div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                    </telerik:RadAjaxPanel>
                                </div>
                            </div>
                        </li>

                        <li>
                            <div id="accrdOutstanding" class="collapsible-header accrd accordian-text-custom active"><i class="mdi-content-flag"></i>Outstanding Payments</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">

                                        <div class="grid_container">
                                            <div class="form-section-row mb">
                                                
                                                <div class="RadGrid RadGrid_Material FormGrid mb-10" >
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
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvInvoice" ShowFooter="True" AllowSorting="true" OnNeedDataSource="RadGrid_gvInvoice_NeedDataSource" OnPreRender="RadGrid_gvInvoice_PreRender"
                                                            runat="server" Width="100%" AllowMultiRowSelection="true" CssClass="RadGrid_Material" OnItemCreated="RadGrid_gvInvoice_ItemCreated">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" EnableAlternatingItems="false" ReorderColumnsOnClient="true">
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                <Columns>

                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" ItemStyle-Width="40px" HeaderStyle-Width="40px" Exportable="false">
                                                                        <HeaderTemplate>
                                                                            <input id="chkAll" type="checkbox" class="css-checkbox" />
                                                                            <label for="chkAll" class="css-label">&nbsp;</label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" class="css-checkbox" Text=" " CommandName="ClearRow" Enabled='<%# Eval("IsCredit").Equals(2) ? false : true %>' />
                                                                            <asp:HiddenField ID="hdnChk" runat="server" Value="" />
                                                                             <asp:HiddenField ID="lblIsCredit" Value='<%# Eval("IsCredit") %>' runat="server" />
                                                                             <asp:HiddenField ID="hdnType" Value='<%# Eval("OpenARType") %>' runat="server" />
                                                                              <asp:HiddenField ID="hdnID" Value='<%# Bind("Ref") %>' runat="server" />
                                                                             <asp:HiddenField ID="hdnLoc" Value='<%# Eval("loc") %>' runat="server" />
                                                                              <asp:HiddenField ID="hdnRefTranID" Value='<%# Eval("RefTranID") %>' runat="server" />
                                                                            
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn AllowFiltering="false" Visible="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                          
                                                                            <asp:HiddenField ID="hdnPaymentID" Value='<%# Bind("PaymentID") %>' runat="server" />
                                                                          
                                                                            <asp:HiddenField ID="hdnCheck" Value='<%# Eval("TransID").Equals(0) ? false : true %>' runat="server" />
                                                                            <asp:Label ID="lblType" Text='<%# Eval("OpenARType") %>' runat="server"/>
                                                                            
                                                                        
                                                                            
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="loc" SortExpression="loc" AutoPostBackOnFilter="true"
                                                                        HeaderText="Location" ShowFilterIcon="false" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLoc" runat="server" Text='<%#Eval("loc")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Ref" SortExpression="Ref" AutoPostBackOnFilter="true"
                                                                        HeaderText="Ref" ShowFilterIcon="false">
                                                                        <ItemTemplate>                                                                          
                                                                            <asp:Label ID="lblRef" runat="server" Text='<%#Eval("Ref")%>' Visible="false"></asp:Label>
                                                                            <asp:HyperLink ID="hlRef" runat="server" Text='<%# Bind("Ref") %>' Target="_blank" NavigateUrl='<%# Eval("OpenARType").ToString()=="1" ? "adddeposit.aspx?id=" +Eval("ref") : (Eval("OpenARType").ToString()=="2"  ?"addreceivepayment.aspx?id=" +Eval("ref"):"addinvoice.aspx?uid=" +Eval("ref")) %>' ForeColor="#0066CC"></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="ID" SortExpression="ID" AutoPostBackOnFilter="true"
                                                                        HeaderText="ID" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server"><%#Eval("ID")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Tag" SortExpression="Tag" AutoPostBackOnFilter="true"
                                                                        HeaderText="Location Name" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdnLocStatus" Value='<%# Eval("LocStatus") %>' runat="server" />
                                                                             <asp:Label ID="lblTag" runat="server" Text='<%# Eval("Tag") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Status" SortExpression="Status" AutoPostBackOnFilter="true"
                                                                        HeaderText="Status" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server"><%#Eval("status")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="manualInv" SortExpression="manualInv" AutoPostBackOnFilter="true"
                                                                        HeaderText="Manual Invoice" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblManualInv" runat="server"><%#Eval("manualInv")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="fDate" SortExpression="fDate" AutoPostBackOnFilter="true"
                                                                        HeaderText="Invoice date" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("fDate", "{0:MM/dd/yy}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label Text="Totals" runat="server" Font-Bold="True" />
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="Amount" SortExpression="Amount" AutoPostBackOnFilter="true"
                                                                        HeaderText="Pretax Amount" ShowFilterIcon="false" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPretaxAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>' Style="float: right;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalPretaxAmount" runat="server" class="bottom-result"  />
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="STax" SortExpression="STax" AutoPostBackOnFilter="true"
                                                                        HeaderText="Sales Tax" ShowFilterIcon="false" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSalesTax" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "STax", "{0:c}")%>' Style="float: right;"><%--<%#Eval("balance")%>--%></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalSalesTax" runat="server" class="bottom-result" />
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="OrigAmount" SortExpression="OrigAmount" AutoPostBackOnFilter="true"
                                                                        HeaderText="Original Amount" ShowFilterIcon="false" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOrigAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OrigAmount", "{0:c}")%>' Style="float: right;"><%--<%#Eval("balance")%>--%></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalOrigAmount" runat="server" class="bottom-result" />
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="DueAmount" FooterStyle-HorizontalAlign="Right" SortExpression="DueAmount" AutoPostBackOnFilter="true"
                                                                        HeaderText="Amount Due" ShowFilterIcon="false" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDueAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DueAmount", "{0:c}")%>' Style="float: right;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalDueAmount" runat="server" class="bottom-result" />
                                                                        </FooterTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn DataField="PrevDueAmount" SortExpression="PrevDueAmount" AutoPostBackOnFilter="true"
                                                                        HeaderText="Payment" ShowFilterIcon="false" UniqueName="PrevDueAmount" HeaderStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField ID="hdnPrevDue" Value='<%# Eval("PrevDueAmount") %>' runat="server" />
                                                                            <asp:TextBox ID="txtPAmount" runat="server" MaxLength="15" Text='<%# DataBinder.Eval(Container.DataItem, "paymentAmt", "{0:c}") %>'
                                                                                onfocus="VisibleRowOnFocus(this);" EnableViewState="false" Enabled='<%# Eval("IsCredit").Equals(2) ? false : true %>'>
                                                                            </asp:TextBox>
                                                                            <asp:HiddenField ID="hdPAmount" Value='<%# DataBinder.Eval(Container.DataItem, "paymentAmt", "{0:c}") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalPayAmount" runat="server" class="bottom-result"/>
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
                                    <div style="clear: both;"></div>
                                </div>
                            </div>
                        </li>

                        <li id="tbLogs" runat="server" style="display: none">
                            <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Logs</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row mb" >
                                                <div class="RadGrid RadGrid_Material">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock6" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                try {
                                                                    var grid = $find("<%= RadGrid_gvLogs.ClientID %>");
                                                                    var columns = grid.get_masterTableView().get_columns();
                                                                    for (var i = 0; i < columns.length; i++) {
                                                                        columns[i].resizeToFit(false, true);
                                                                    }
                                                                } catch (e) {

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
                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_gvLogs" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvLogs" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvLogs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true" OnItemCreated="RadGrid_gvLogs_ItemCreated"
                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvLogs_NeedDataSource">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false" DataKeyNames="fUser">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbldate" runat="server" Text='<%# Eval("CreatedStamp", "{0:M/d/yyyy}")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                                        CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbltime" runat="server" Text='<%# Eval("CreatedStamp","{0: hh:mm tt}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="fUser" SortExpression="fUser" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUpdBy" runat="server" Text='<%# Eval("fUser") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="Field" SortExpression="Field" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Field" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblField" runat="server" Text='<%# Eval("field") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="OldVal" SortExpression="OldVal" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="Old Value" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOldVal" runat="server" Text='<%# Eval("OldVal") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn DataField="NewVal" SortExpression="NewVal" AutoPostBackOnFilter="true"
                                                                        CurrentFilterFunction="Contains" HeaderText="New Value" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNewVal" runat="server" Text='<%# Eval("NewVal") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </telerik:RadAjaxPanel>



                                                </div>

                                            </div>
                                        </div>

                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdTabIndex" runat="server" />
    <input id="hdnPatientId" runat="server" type="hidden" />
        
     <input id="hdnIsWriteOff" runat="server" type="hidden"  value="0"/>
    <input id="hdnIsCredit" runat="server" type="hidden"  value="0"/>
 <input id="hdnAmountSelected" runat="server" type="hidden"  value="0"/>

    <telerik:RadWindowManager ID="RadWindowManagerAddOpp" runat="server">
        <Windows>
            
            <telerik:RadWindow ID="RadWindowWriteOff" Skin="Material" VisibleTitlebar="true" Title="Write off" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="550">
                <ContentTemplate>
                    <telerik:RadAjaxPanel ID="RadAjaxPanelWriteOff" runat="server" LoadingPanelID="RadAjaxLoadingPanel_WriteOff" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                        <div class="margin-tp">

                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <label for="txtWriteOffDate">Date</label>
                                        <asp:TextBox ID="txtWriteOffDate" CssClass="datepicker_mom" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDateWriteOff" ControlToValidate="txtWriteOffDate"
                                            ErrorMessage="Please enter Date." Display="None"
                                            ValidationGroup="PaymentWriteOff">
                                        </asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvDateWriteOff" />
                                        <asp:RegularExpressionValidator ID="revWriteOff" ControlToValidate="txtWriteOffDate" ValidationGroup="PaymentWriteOff"
                                            ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="revWriteOff" />

                                    </div>
                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>

                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <label class="active" >Billing Code</label>
                                        <asp:DropDownList ID="ddlCode" runat="server" DataTextField="BillCode" DataValueField="ID" />
                                    </div>
                                </div>


                            </div>
                            <div class="form-section-row">
                                <div class="input-field col s12">
                                    <asp:TextBox ID="txtDescription" runat="server" />
                                    <label for="txtDescription">Description</label>
                                </div>
                            </div>


                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <asp:TextBox ID="txtInvoiceWriteOff" runat="server" Enabled="false" />
                                        <label for="txtInvoiceWriteOff">Invoice</label>
                                    </div>
                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <asp:TextBox ID="txtWriteOffAmount" runat="server" Enabled="false" ClientIDMode="Static" />
                                        <label for="txtWriteOffAmount">Amount</label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section-row">
                                <div class="input-field col s12">
                                    <asp:TextBox ID="txtWriteOffProject" runat="server" />
                                    <label for="txtWriteOffProject">Project</label>
                                </div>
                            </div>
                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <asp:TextBox ID="txtWriteOffCustID" runat="server" />
                                        <label for="txtWriteOffCustID">Customer ID</label>
                                    </div>
                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <asp:TextBox ID="txtWriteOffCust" runat="server" />
                                        <label for="txtWriteOffCust">Customer</label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-section-row">
                                <div class="input-field col s12">
                                    <asp:TextBox ID="txtWriteOffLoc" runat="server" />
                                    <label for="txtWriteOffLoc">Location</label>
                                </div>
                            </div>

                            <div style="clear: both;"></div>
                            <div class="top-area">
                                <div class="btnlinks">
                                    <asp:LinkButton ID="lnkSaveWriteOff" runat="server" OnClick="lnkSaveWriteOff_Click" OnClientClick="return ValidateWriteOffAccount();">Save</asp:LinkButton>
                                    <asp:HiddenField ID="hdnIsCreditWriteOff" runat="server" value="1"/>
                                        <asp:HiddenField ID="hdnTransID" runat="server" value="0"/>
                                </div>
                            </div>
                        </div>

                     </telerik:RadAjaxPanel>
                </ContentTemplate>

            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowCredit" Skin="Material" VisibleTitlebar="true" Title="Credit" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="250">
                <ContentTemplate>
                    <telerik:RadAjaxPanel ID="RadAjaxPanelCredit" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Credit" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                        <div class="margin-tp">

                            <div class="form-section-row">
                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <label for="txtCreditDate">Date</label>
                                        <asp:TextBox ID="txtCreditDate" CssClass="datepicker_mom" runat="server" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDateCredit" ControlToValidate="txtCreditDate"
                                            ErrorMessage="Please enter Date." Display="None"
                                            ValidationGroup="PaymentCredit">
                                        </asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvDateCredit" />
                                        <asp:RegularExpressionValidator ID="revCredit" ControlToValidate="txtCreditDate" ValidationGroup="PaymentCredit"
                                            ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="revCredit" />

                                    </div>
                                </div>
                                <div class="form-section2-blank">
                                    &nbsp;
                                </div>

                                <div class="form-section2">
                                    <div class="input-field col s12">
                                        <%--<label class="active" >Billing Code</label>
                                        <asp:DropDownList ID="DropDownList1" runat="server" DataTextField="BillCode" DataValueField="ID" />--%>
                                        
                                        <asp:TextBox ID="txtCreditAmount" runat="server" Enabled="false" ClientIDMode="Static" />
                                        <label for="txtCreditAmount">Amount</label>
                                        <asp:HiddenField ID="hdnInvoiceCredit" runat="server" />
                                        <asp:HiddenField ID="hdnJobIDCredit" runat="server" />
                                    </div>
                                </div>

                                <div class="form-section-row">
                                <div class="input-field col s12">
                                    <asp:TextBox ID="txtDescriptionCredit" runat="server" />
                                    <label for="txtDescriptionCredit">Description</label>
                                </div>
                            </div>


                            </div>

                            
                            
                            <div style="clear: both;"></div>
                            <div class="top-area">
                                <div class="btnlinks">
                                    <asp:LinkButton ID="lnkSaveCredit" runat="server" OnClick="lnkSaveCredit_Click" >Save</asp:LinkButton>
                                    <asp:HiddenField ID="hdnIsCreditWriteOff1" runat="server" value="1"/>
                                        <asp:HiddenField ID="hdnTransIDCredit" runat="server" value="0"/>
                                </div>
                            </div>
                        </div>

                     </telerik:RadAjaxPanel>
                </ContentTemplate>

            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowTransfer" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="600" Height="400" Title="Transfer Payment">
                <ContentTemplate>
                         <telerik:RadAjaxPanel ID="RadAjaxTransferPanel" runat="server" LoadingPanelID="RadAjaxLoadingPanel_WriteOff" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                    <div class="margin-tp">

                        <div class="form-section-row">
                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:HiddenField ID="hdnNewLocID" runat="server" />
                                    
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtAccount"
                                            Display="None" ErrorMessage="Location is required" SetFocusOnError="True"
                                            ValidationGroup="Transfer" ></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator11_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator15"  PopupPosition="BottomLeft">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtAccount" runat="server" onkeyup="EmptyValue(this);"
                                            autocomplete="off" placeholder="Search by acct# and name"></asp:TextBox>
                                        <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtAccount"
                                            EnableCaching="False" ServiceMethod="GetActiveLocation" UseContextKey="True" MinimumPrefixLength="0"
                                            CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListItemCssClass="autocomplete_listItem"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListElementID="ListDivisor"
                                            OnClientItemSelected="aceLoc_itemSelected"
                                            ID="AutoCompleteExtender" DelimiterCharacters="" CompletionInterval="250">
                                        </asp:AutoCompleteExtender>
                                        <div id="ListDivisor"></div>
                                        <label for="txtAccount">Location</label>
                                    </div>


                                </div>
                            </div>
                        </div>
                       <div class="form-section-row">
                               <div class="form-section">
                                    <div class="input-field col s12">
                                          <label for="lbAddress">Location Info</label>
                                        <asp:TextBox ID="txtNewAddress" runat="server" TextMode="multiline" Columns="50" Rows="5"  CssClass="materialize-textarea pd-negate"/>
                                      
                                    </div>
                              
                                </div>
                          </div>
                       
                        <div class="form-section-row">
                            <div class="base">
                                <div class="btnlinks">
                                     <asp:LinkButton ID="lnkSaveTransfer" runat="server" OnClientClick="return ValidateTransfer();" OnClick="lnkSaveTransfer_Click" ValidationGroup="Transfer">Transfer</asp:LinkButton>
                                </div>                                
                               
                            </div>
                        </div>
                      
                       
                       
                    </div>
                              </telerik:RadAjaxPanel>
                </ContentTemplate>
            </telerik:RadWindow>


         

        </Windows>
    </telerik:RadWindowManager>
       <telerik:RadWindow ID="LocationsWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
                runat="server" Modal="true" Width="1050" Height="635">
                <ContentTemplate>
                    <telerik:RadAjaxPanel ID="RadAjaxPanel32" runat="server">
                       <div class="margin-tp">
                    <div class="form-section-row">
                        <div class="form-section">
                            <div class="row mb">
                                <div class="grid_container" id="divMemberGrid" runat="server">
                                    <div class="RadGrid RadGrid_Material RadGrid RadGrid_Popup">

                                          <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Location" AllowFilteringByColumn="false" ShowFooter="false" PageSize="1000"
                                                    ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                                    AllowCustomPaging="false" Width="100%" 
                                                    OnNeedDataSource="RadGrid_Location_NeedDataSource">
                                                    <CommandItemStyle />
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>

                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false  ">
                                                        <Columns>
                                                            <telerik:GridClientSelectColumn UniqueName="chkLocationSelect" HeaderStyle-Width="28">
                                                            </telerik:GridClientSelectColumn>
                                                            <telerik:GridTemplateColumn HeaderStyle-Width="30px" Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdnLocSelected" runat="server" Value='<%# Bind("loc") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblloc" runat="server" Text='<%# Bind("loc") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DataField="locid" SortExpression="locid" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="Contains" HeaderText="Acct #" ShowFilterIcon="false" AllowFiltering="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("locid") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DataField="name" SortExpression="name" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="Contains" HeaderText="Location Name" ShowFilterIcon="false" AllowFiltering="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("tag") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DataField="address" SortExpression="address" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="Contains" HeaderText="Address" ShowFilterIcon="false" AllowFiltering="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFx" runat="server"><%#Eval("Address")%></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DataField="City" SortExpression="City" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="Contains" HeaderText="City" ShowFilterIcon="false" AllowFiltering="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCity" runat="server"><%#Eval("city")%></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DataField="type" SortExpression="type" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="Contains" HeaderText="Type" ShowFilterIcon="false" AllowFiltering="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblType" runat="server"><%#Eval("type")%></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DataField="status" SortExpression="status" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false" AllowFiltering="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblstatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DataField="balance" SortExpression="balance" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="EqualTo" HeaderText="Balance" FooterAggregateFormatString="{0:c}" Aggregate="Sum" AllowFiltering="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBalance" runat="server" ForeColor='<%# Convert.ToDouble(Eval("balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                        </Columns>
                                                    </MasterTableView>
                                                    <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                    </FilterMenu>
                                                </telerik:RadGrid>

                                    </div>
                                </div>
                            </div>
                             <div class="btnlinks">
                             <asp:LinkButton ID="lnkPopupOK" runat="server" OnClientClick="return ValidateLocation()" OnClick="lnkPopupOK_Click">OK</asp:LinkButton>
                        </div>
                        </div>
                    </div>
                  
                  
                </div>
            </telerik:RadAjaxPanel>
        </ContentTemplate>
    </telerik:RadWindow>

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
        function ddl_changed(ddl) {
            //debugger;
            if (ddl.value == 0) {
                document.getElementById('lblCheck').innerHTML = 'Check';
                document.getElementById('lblAmount').innerHTML = 'Check amount';
            }
            else {
                document.getElementById('lblCheck').innerHTML = 'Reference';
                document.getElementById('lblAmount').innerHTML = 'Amount received';
            }
        }

        function check(obj) { }
        function SelectedRowStyle(gridview) {
            var grid = document.getElementById(gridview);
            $('#' + gridview + ' tr').each(function () {
                var $tr = $(this);
                var chk = $tr.find('input[id*=chkSelect]');
                if (chk.prop('checked') == true) {
                    $tr.css('background-color', '#e0eefe');
                }
            })
        }

        function GetInvoiceTotal() {
            var total = 0.00;
            var totalDueAmount = 0.00;
            $("[id*=txtPAmount]").each(function () {

                var txtPay = $(this).attr('id')
                var expression = /[\(\)]/g                     // to identify parentheses
                var chk = document.getElementById(txtPay.replace('txtPAmount', 'chkSelect'));
                if ($(chk).prop('checked') == true) {
                    if ($(this).val() != '') {

                        var val = $(this).val()

                        if (val.match(expression))     /// check is parentheses exists (negative value)
                        {
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
            $("#<%=txtAmount.ClientID %>").val(totalval);
            $("[id*=lblTotalPayAmount]").text(totalval);            

            $("#<%=RadGrid_gvInvoice.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                var chk = $(this).attr('id');
                var expression = /[\(\)]/g;
                var lblDue = document.getElementById(chk.replace('chkSelect', 'lblDueAmount'));
                var val = $(lblDue).text();

                if (val.match(expression)) {
                    totalDueAmount = totalDueAmount - parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''));
                }
                else {
                    totalDueAmount = totalDueAmount + parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''));
                }
            });

            var totalBalance = cleanUpCurrency('$' + totalDueAmount.toLocaleString("en-US", { minimumFractionDigits: 2 }))
            $("[id*=lblTotalDueAmount]").text(totalBalance);
        }
        function check(val) {
            //console.log(val);
        }
        function getCurrentTotalPayment() {
             var total = 0.00;          
            $("[id*=txtPAmount]").each(function () {

                var txtPay = $(this).attr('id')
                var expression = /[\(\)]/g                     // to identify parentheses
                var chk = document.getElementById(txtPay.replace('txtPAmount', 'chkSelect'));
                if ($(chk).prop('checked') == true) {
                    if ($(this).val() != '') {

                        var val = $(this).val()

                        if (val.match(expression))     /// check is parentheses exists (negative value)
                        {
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
            $("[id*=hdnAmountSelected]").value = 0;
            $("[id*=hdnAmountSelected]").val(totalval);
        }
        function pageLoad(sender, args) {          
            loadControlAutoComplete();
            CheckInvoicesCheckBox();
            $("[id*=btnSubmit]").unbind("click");
            $("[id*=btnSubmit]").bind({
                click: function () {

                    return ValidateLess(this);
                },

            });
           
            $("[id*=chkSelect]").change(function () {
                //debugger
                var chk = $(this).attr('id');
                var PaidCredit = document.getElementById(chk.replace('chkSelect', 'lblIsCredit'));
                if ($(PaidCredit).val() != '2') {
                    var txtPay = document.getElementById(chk.replace('chkSelect', 'txtPAmount'));
                    var lblDue = document.getElementById(chk.replace('chkSelect', 'lblDueAmount'));
                    var hdnPrevDue = document.getElementById(chk.replace('chkSelect', 'hdnPrevDue'));
                    var hdPAmount = document.getElementById(chk.replace('chkSelect', 'hdPAmount'));
                    var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
                    var prevDue = parseFloat($(hdnPrevDue).val())
                    var pay = 0;

                    var rpay = cleanUpCurrency('$' + pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    var rprevDue = cleanUpCurrency('$' + prevDue.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                    if ($(this).prop('checked') == true) {
                        $(hdPAmount).val(rprevDue)
                        $(txtPay).val(rprevDue)
                        $(lblDue).text(rpay)
                        SelectedRowStyle('<%=RadGrid_gvInvoice.ClientID %>')
                    }
                    else if ($(this).prop('checked') == false) {
                        $(hdPAmount).val(rpay)
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
                    var PaidCredit = document.getElementById(chk.replace('chkSelect', 'lblIsCredit'));
                    if ($(PaidCredit).val() != '2') {
                        var txtPay = document.getElementById(chk.replace('chkSelect', 'txtPAmount'));
                        var lblDue = document.getElementById(chk.replace('chkSelect', 'lblDueAmount'));
                        var hdnPrevDue = document.getElementById(chk.replace('chkSelect', 'hdnPrevDue'));
                        var hdPAmount = document.getElementById(chk.replace('chkSelect', 'hdPAmount'));
                        var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
                        var prevDue = parseFloat($(hdnPrevDue).val())
                        var pay = 0;

                        var rpay = cleanUpCurrency('$' + pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        var rprevDue = cleanUpCurrency('$' + prevDue.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                        if ($(this).prop('checked') == true) {
                            $(hdPAmount).val(rpay)
                            $(txtPay).val(rprevDue)
                            $(lblDue).text(rpay)
                            SelectedRowStyle('<%=RadGrid_gvInvoice.ClientID %>')
                        }
                        else if ($(this).prop('checked') == false) {
                            $(hdPAmount).val(rprevDue)
                            $(txtPay).val(rpay)
                            $(lblDue).text(rprevDue)
                            $(this).closest('tr').removeAttr("style");
                        }
                    }
                });

                GetInvoiceTotal();
            });

            $("[id*=txtPAmount]").change(function () {
                //debugger;
                var txtPay = $(this).attr('id');
                var lblDue = document.getElementById(txtPay.replace('txtPAmount', 'lblDueAmount'));
                var hdnPrevDue = document.getElementById(txtPay.replace('txtPAmount', 'hdnPrevDue'));
                var hdPAmount = document.getElementById(txtPay.replace('txtPAmount', 'hdPAmount'));
                var chk = document.getElementById(txtPay.replace('txtPAmount', 'chkSelect'));
                //var pay = $(this).val().toString().replace(/[\(,]/g, '');
                //var pay = pay.toString().replace(/[\$\),]/g, '');

                var pay = $(this).val().toString();
              
                 var expression = /[\(\)]/g
                        if (pay.match(expression))     /// check is parentheses exists (negative value)
                        {
                            pay = -1 * parseFloat(pay.replace(/[\$\(\),]/g, ''));
                        }
                        else {
                            pay = parseFloat(pay.replace(/[\$\(\),]/g, ''));
                        }

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
                    /* */

                    if (pay > prevDue) {
                        alert("Payment cannot be greater than amount due");

                        return;
                    }

                    if (IsNeg) {
                        pay = pay * -1;
                        prevDue = prevDue * -1;
                        due = due * -1;
                    }

                    var rPay = cleanUpCurrency('$' + pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    var rDue = cleanUpCurrency('$' + due.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    $(this).val(rPay);
                    $(lblDue).text(rDue);
                    $(chk).prop('checked', true);
                    SelectedRowStyle('<%=RadGrid_gvInvoice.ClientID %>')
                    hdPAmount.value = rPay;
                }
                else {
                    $(chk).prop('checked', false);
                    $(this).closest('tr').removeAttr("style");
                }

                GetInvoiceTotal();
            });

            


            Materialize.updateTextFields();

            var customerText = $("#<%=txtCustomer.ClientID %>").val();
            if (customerText) {
                $("#<%=txtCustomer.ClientID %>").addClass('x');
            }

            var locationText = $("#<%=txtLocation.ClientID %>").val();
            if (locationText) {
                $("#<%=txtLocation.ClientID %>").addClass('x');
            }

            var invoiceText = $("#<%=txtInvoice.ClientID %>").val();
            if (invoiceText) {
                $("#<%=txtInvoice.ClientID %>").addClass('x');
            }

           $("#<%=txtWriteOffDate.ClientID %>").pikaday({
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(1900, 1, 1),
                maxDate: new Date(2100, 12, 31),
                yearRange: [1900, 2100]
            });
            Materialize.updateTextFields();

        }

        function CheckMultipleLocation() {
            var oldValue = "";
            var retValue = false;
            $("#<%=RadGrid_gvInvoice.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                var chk = $(this).attr('id');
                if ($(this).prop("checked") == true) {
                    var lblID = document.getElementById(chk.replace('chkSelect', 'lblID'));
                    var data = $(lblID).text();
                    if (oldValue == "") {
                        oldValue = data;
                    }
                    if (oldValue == data) {
                        oldValue = data;
                    }
                    else {
                        retValue = true;
                    }
                }
            });
            return retValue;
        }
        $(document).ready(function () {
            //debugger
            var customerText = $("#<%=txtCustomer.ClientID %>").val();
            if (customerText) {
                $("#<%=txtCustomer.ClientID %>").addClass('x');
            }

            var locationText = $("#<%=txtLocation.ClientID %>").val();
            if (locationText) {
                $("#<%=txtLocation.ClientID %>").addClass('x');
            }

            var invoiceText = $("#<%=txtInvoice.ClientID %>").val();
            if (invoiceText) {
                $("#<%=txtInvoice.ClientID %>").addClass('x');
            }

            CheckInvoicesCheckBox();
          
        });

        function tog(v) { return v ? 'addClass' : 'removeClass'; }
        $(document).on('input', '.clearable', function () {
            $(this)[tog(this.value)]('x');
        }).on('mousemove', '.x', function (e) {
            $(this)[tog(this.offsetWidth - 18 < e.clientX - this.getBoundingClientRect().left)]('onX');
        }).on('touchstart click', '.onX', function (ev) {
            var id = $(this).attr('id');
            ClearData(id);
            ev.preventDefault();
            $(this).removeClass('x onX').val('').change();
        });

        function ClearData(id) {
            if (id == "<%=txtCustomer.ClientID %>") {
                $("#<%=txtCustomer.ClientID %>").val('');
                $("#<%=txtLocation.ClientID %>").val('');
                $("#<%=hdnCustID.ClientID %>").val('');
                $("#<%=hdnLocID.ClientID %>").val('');
                  $("#<%=hdnLocStatus.ClientID %>").val('');
            }

            if (id == "<%=txtLocation.ClientID %>") {
                $("#<%=txtLocation.ClientID %>").val('');
                $("#<%=hdnLocID.ClientID %>").val('');
                  $("#<%=hdnLocStatus.ClientID %>").val('');
            }
        }

        function CheckInvoicesCheckBox() {
            //debugger;
            $("#<%=RadGrid_gvInvoice.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                var chk = $(this).attr('id');
                var txtPay = document.getElementById(chk.replace('chkSelect', 'txtPAmount'));
                var data = $(txtPay).val();
                if (data != '$0.00') {
                    $(this).prop('checked', true);
                    SelectedRowStyle('<%=RadGrid_gvInvoice.ClientID %>');
                }
            });
        }
        function countSelectedOnGrid() {
            var number = 0;
            $("#<%=RadGrid_gvInvoice.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:first:checked').each(function (index, value) {
                    number = number + 1;
                });
            });
            return number;
        }

        function ValidateLess(element) {
            debugger
            var totalPay = 0;
            var flag = 0;
            $("#<%=RadGrid_gvInvoice.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                var chk = $(this).attr('id');
                if ($(this).prop('checked') == true) {
                    var txtPay = document.getElementById(chk.replace('chkSelect', 'txtPAmount'));
                    var hdnPrevDue = document.getElementById(chk.replace('chkSelect', 'hdnPrevDue'));
                    var PaidCredit = document.getElementById(chk.replace('chkSelect', 'lblIsCredit'));
                    if ($(PaidCredit).val() != '2') {
                        var dataPay = $(txtPay).val();
                        var dataPrevDue = $(hdnPrevDue).val();
                        var expression = /[\(\)]/g
                        if (dataPay.match(expression))     /// check is parentheses exists (negative value)
                        {
                            dataPay = -1 * parseFloat($(txtPay).val().replace(/[\$\(\),]/g, ''));
                        }
                        else {
                            dataPay = parseFloat($(txtPay).val().replace(/[\$\(\),]/g, ''));
                        }
                        if (dataPrevDue.match(expression))     /// check is parentheses exists (negative value)
                        {
                            dataPrevDue = -1 * parseFloat($(hdnPrevDue).val().replace(/[\$\(\),]/g, ''));
                        }
                        else {
                            dataPrevDue = parseFloat($(hdnPrevDue).val().replace(/[\$\(\),]/g, ''));
                        }

                        if (dataPrevDue < 0) {
                            if (dataPay * (-1) > dataPrevDue * (-1)) {
                                flag = 1
                                return;
                            }
                        } else {
                            if (dataPay > dataPrevDue) {
                                flag = 1
                                return;
                            }
                        }
                    }
                }
            });
            if (flag == 1) {
                noty({
                    text: 'Payment cannot be greater than amount due',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: false,
                    theme: 'noty_theme_default',
                    closable: true
                });

                return false;
            } else {
                var totalPay = $("[id*=lblTotalPayAmount]").text().replace(/[\$\(\),]/g, '');

                var fTotalPay = parseFloat(totalPay);

                var amountPay = parseFloat($("#<%=txtAmount.ClientID %>").val().replace(/[\$\(\),]/g, ''));
                var famountPay = parseFloat(amountPay);
                if (famountPay < fTotalPay) {

                    var permission = $("#<%=lnkWriteOff.ClientID %>").is(":visible");
                    if (permission == false) {
                        noty({
                            text: 'Check Amount should not less than Payment Amount Total.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: false,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                        return false;
                    }
                    if (countSelectedOnGrid() > 0) {
                        if (confirm("Check Amount should not less than Payment Amount Total.Do you want to write off amount?")) {
                            $("#<%=hdnIsWriteOff.ClientID %>").val("1");

                            getCurrentTotalPayment();
                            var countcheck = 0;
                            var ID = "";
                            var Amount = "";
                            var today = new Date();
                            var isCredit = "";
                            var custID = "";
                            $('#<%=txtWriteOffDate.ClientID%>').val(today.toLocaleDateString("en-US"));

                            $('#<%=txtDescription.ClientID%>').val('');
                            $('#<%=txtInvoiceWriteOff.ClientID%>').val('');
                            $('#<%=txtWriteOffAmount.ClientID%>').val('');
                            $('#<%=txtWriteOffCust.ClientID%>').val('');
                            $('#<%=txtWriteOffLoc.ClientID%>').val('');

                            $('#<%=txtWriteOffCustID.ClientID%>').val("");
                            $("#<%=RadGrid_gvInvoice.ClientID %>").find('tr:not(:first,:last)').each(function () {
                                var $tr = $(this);
                                $tr.find('input[type="checkbox"]:first:checked').each(function (index, value) {
                                    if (countcheck == 0) {

                                        // debugger
                                        ID = $tr.find('input[id*=hdnID]').val();
                                        Amount = $tr.find('input[id*=hdnPrevDue]').val();

                                        custID = $tr.find('span[id*=lblID]').text();
                                        isCredit = $tr.find('input[id*=lblIsCredit]').val();
                                        countcheck = 1;
                                    }
                                });
                            });

                            if (ID != "") {
                                if (isCredit == 1) {
                                    noty({
                                        text: 'Please select a invoice',
                                        type: 'warning',
                                        layout: 'topCenter',
                                        closeOnSelfClick: false,
                                        timeout: false,
                                        theme: 'noty_theme_default',
                                        closable: true
                                    });
                                    return false;
                                } else {
                                    $('#<%=txtWriteOffAmount.ClientID%>').val(parseFloat(Amount).toLocaleString("en-US", { minimumFractionDigits: 2 }));
                                    $('#<%=txtWriteOffCustID.ClientID%>').val(custID);
                                    var wnd = $find('<%=RadWindowWriteOff.ClientID %>');
                                    wnd.Show();
                                    Materialize.updateTextFields();
                                    return true;
                                }
                            } else {
                                return false;
                            }

                        } else {
                            return false;
                        }
                    }


                }
                else {
                    if (famountPay > fTotalPay) {
                        var txtLocation = $("[id*=txtLocation]").val();
                        if (txtLocation == "") {
                            var lsLoc = [];
                            $("#<%=RadGrid_gvInvoice.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                                var chk = $(this).attr('id');
                                if ($(this).prop('checked') == true) {
                                    var grid_hdnLoc = document.getElementById(chk.replace('chkSelect', 'hdnLoc'));
                                    var locID = $(grid_hdnLoc).val()
                                    if (lsLoc.includes(locID) == false) {
                                        lsLoc.push(locID)
                                    }
                                }
                            });
                            if (lsLoc.length == 1) {
                                $("#<%=hdnLocID.ClientID %>").val(lsLoc[0]);
                            } else {
                                var wnd = $find('<%=LocationsWindow.ClientID %>');
                                  wnd.set_title("Locations")
                                wnd.Show();
                                return true;
                            }
                        } else {
                            return true;
                        }
                    } else {
                        return true;
                    }


                    //  element.onclick = function () { return false; };

                }
            }

        }

        function ddl_changedCustomer() {
         //debugger
            var ddl = $("#<%=ddlCustomer.ClientID %>");
            if ( $("#<%=ddlCustomer.ClientID %>").val() != 0) {
                var str = $("#<%=ddlCustomer.ClientID %>").find("option:selected").text();
               
                $("#<%=txtCustomer.ClientID %>").val (str);       
                var hdnCustID = document.getElementById('<%=hdnCustID.ClientID %>');
                hdnCustID.value = $("#<%=ddlCustomer.ClientID %>").val();
                 $("#<%=txtLocation.ClientID %>").val('');
                $("#<%=hdnLocID.ClientID %>").val('');
                  $("#<%=hdnLocStatus.ClientID %>").val('');
               
            }
            if ($("#<%=ddlFilter.ClientID %>").val() != 1) {
                document.getElementById('<%=btnSelectCustomer.ClientID %>').click();
            }                         
        }           
        function getUrlParameter(name) {
            name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
            var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
            var results = regex.exec(location.search);
            return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
        };
        function changePayDate() {
                       
            var txtDateVal = $("#<%=txtDate.ClientID %>").val();
            var txtDate1Val = $("#<%=hdnDateValidate.ClientID %>").val();
            if (txtDateVal != txtDate1Val) {
                document.getElementById('<%=btnSelectDate.ClientID %>').click();
            }
            
        }
    </script>
    
     <script>
           function loadControlAutoComplete() {
                function dtaa() {
                this.prefixText = null;
                this.con = document.getElementById('<%=hdnCon.ClientID %>').value;
                //this.custID = null;
            }


            $("#<%=txtCustomer.ClientID %>").autocomplete({
             
                source: function (request, response) {
                    //debugger
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetCustomerWithInactive",
                        //data: '{"prefixText":' + JSON.stringify(request.term) + ',"con":' + JSON.stringify(document.getElementById('ctl00_ContentPlaceHolder1_hdnCon').value) + '}',
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load customers");
                        }
                      
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtCustomer.ClientID %>").val(ui.item.label);
                    $("#<%=hdnCustID.ClientID %>").val(ui.item.value);
                    $("#<%=txtLocation.ClientID %>").val('');
                    $("#<%=hdnLocID.ClientID %>").val('');
                     $("#<%=hdnLocStatus.ClientID %>").val('');
                    document.getElementById('<%=btnSelectCustomer.ClientID %>').click();
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtCustomer.ClientID %>").val(ui.item.label);
                    $("#<%=hdnCustID.ClientID %>").val(ui.item.value);
                    $("#<%=txtCustomer.ClientID %>").focus();
                    return false;
                },

                minLength: 0,
                delay: 250
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                   //_renderItem = function (ul, item) {
                   var result_item = item.label;
                   var result_desc = item.desc;
                var result_CustomerStatus = item.CustomerStatus;
                var result_CustomerBalance = item.CustBalance;                
                   var x = new RegExp('\\b' + queryloc, 'ig'); // notice the escape \ here...            
                   result_item = result_item.replace(x, function (FullMatch, n) {
                       return '<span class="highlight">' + FullMatch + '</span>'
                   });
                   if (result_desc != null) {
                       result_desc = result_desc.replace(x, function (FullMatch, n) {
                           return '<span class="highlight">' + FullMatch + '</span>'
                       });
                   }
                if (result_CustomerStatus == 1) {
                       return $("<li></li>")
                           .data("item.autocomplete", item)
                           //.append("<a>" + result_item + ", <span style='color:Red;'>" + result_desc + "</span></a>")
                           .append("<span style='color:Red;'>" + result_item + " , Bal: " + result_CustomerBalance +"</span>")
                           .appendTo(ul);
                   }
                   else {
                       return $("<li></li>")
                           .data("item.autocomplete", item)
                           .append("<span style='color:Gray;'>" + result_item + " , Bal: " + result_CustomerBalance +"</span>")
                           .appendTo(ul);
                   }

               };


            $("#<%=txtAmount.ClientID %>").change(function () {
                //debugger;
                //var temp = $(this).val().replace(/[\$\(\),]/g, '')
                //var amt = parseFloat($(this).val().replace(/[\$\(\),]/g, ''))               
              var amt=  convertNumber($(this).val().replace("$",''))
                 amt = parseFloat(amt)

                var amtval = cleanUpCurrency('$' + amt.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                $(this).val(amtval)

            });

            $("#<%=txtInvoice.ClientID%>").change(function () {

                var txtInvoice = $(this);
                var strRef = $(this).val();

                var txtInvoice1 = $(txtInvoice).attr('id');

                if (strRef != '') {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        //url: "AccountAutoFill.asmx/GetInvoiceNos",
                        url: "AccountAutoFill.asmx/GetInvoiceNosChange",
                        data: '{"prefixText":' + strRef + ',"Customer":' + JSON.stringify(document.getElementById('ctl00_ContentPlaceHolder1_hdnCustID').value) + ',"Loc":' + JSON.stringify(document.getElementById('ctl00_ContentPlaceHolder1_hdnLocID').value) + '}',
                        dataType: "json",
                        async: true,
                        success: function (data) {

                            var ui = $.parseJSON(data.d);

                            if (ui.length == 0) {
                                noty({
                                    text: 'Invoice #' + strRef + ' could not be found!',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: false,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                            }
                            else {

                                if (ui[0].Status == "1") {
                                   
                                    console.log('Invoice #' + strRef + ' is already paid.!');
                                }
                                else if (ui[0].Status == "2") {
                                  
                                    console.log('Invoice #' + strRef + ' is voided.!');
                                }
                                else {
                                    if (ui[0].Loc) {
                                        $("#<%=txtCustomer.ClientID %>").val(ui[0].OwnerName);
                                        $("#<%=hdnCustID.ClientID %>").val(ui[0].Owner);
                                        $("#<%=txtLocation.ClientID %>").val(ui[0].Tag);
                                        $("#<%=hdnLocID.ClientID %>").val(ui[0].Loc);
                                         $("#<%=hdnLocStatus.ClientID %>").val(ui[0].LocStatus);
                                        document.getElementById('<%=btnSelectLoc.ClientID %>').click();
                                    }
                                    else {
                                        noty({
                                            text: 'Invoice #' + strRef + ' could not be found!',
                                            type: 'warning',
                                            layout: 'topCenter',
                                            closeOnSelfClick: false,
                                            timeout: false,
                                            theme: 'noty_theme_default',
                                            closable: true
                                        });

                                    }
                                }

                            }
                        },
                        error: function (result) {
                            //alert("Due to unexpected errors we were unable to load Invoice#");
                            console.log("Due to unexpected errors we were unable to load Invoice#");
                        }
                    });
                }

            });

            $("#<%=txtInvoice.ClientID%>").autocomplete({

                source: function (request, response) {
                    var dta = new dtaa();
                    dta.prefixText = request.term;
                    query = request.term;

                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetInvoiceNos",
                         data: '{"prefixText":' + JSON.stringify(request.term) + ',"Customer":' + JSON.stringify('') + ',"Loc":' + JSON.stringify('') + '}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            //alert("Due to unexpected errors we were unable to load invoice");
                            console.log("Due to unexpected errors we were unable to load invoice");
                        }
                    });
                },
                select: function (event, ui) {
                    var str = ui.item.Ref;
                    if (str == "No Record Found!") {
                        $(this).val("");
                    }
                    else {
                        $(this).val(ui.item.Ref);
                        $("#<%=txtCustomer.ClientID %>").val(ui.item.OwnerName);
                        $("#<%=hdnCustID.ClientID %>").val(ui.item.Owner);
                        $("#<%=txtLocation.ClientID %>").val(ui.item.Tag);
                        $("#<%=hdnLocID.ClientID %>").val(ui.item.Loc);
                        $("#<%=hdnLocStatus.ClientID %>").val(ui.item.LocStatus);
                        $("#<%=hdTabIndex.ClientID %>").val("2");
                        document.getElementById('<%=btnSelectLoc.ClientID %>').click();

                    }
                    return false;
                },
                focus: function (event, ui) {

                    $(this).val(ui.item.Ref);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".searchinput_invoice"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {

                    var result_item = item.Ref;
                    //var result_amt = item.Due;
                    //var result_inv = item.LabelRef;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            

                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + "</a>")
                        //.append("<a>" + result_inv + " " + result_item + ", <span style='color:Gray;'>" + result_amt + "</span></a>")
                        .appendTo(ul);

                };
            });


            ///////////// Ajax call for location auto search ////////////////////
            var queryloc = "";
                $("#<%=txtLocation.ClientID %>").autocomplete({

                    source: function (request, response) {
                        //  if (document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value != '') {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.custID = 0;
                        if (document.getElementById('<%=hdnCustID.ClientID %>').value != '' && document.getElementById('<%=hdnCustID.ClientID %>').value != 'undefined') {
                          
                            dtaaa.custID = document.getElementById('<%=hdnCustID.ClientID %>').value;
                        }
                        queryloc = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "CustomerAuto.asmx/GetLocationWithInactive",
                            //  data: "{'prefixText':'" + request.term + "','con':'" + document.getElementById('ctl00_ContentPlaceHolder1_hdnCon').value + "','custID':" + document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value + "}",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load customers");
                            }
                        });
                    },
                    select: function (event, ui) {
                        $("#<%=txtLocation.ClientID %>").val(ui.item.label);
                        $("#<%=hdnLocID.ClientID %>").val(ui.item.value);
                        $("#<%=hdnLocStatus.ClientID %>").val(ui.item.LocStatus);
                        $("#<%=hdTabIndex.ClientID %>").val("1");
                        document.getElementById('<%=btnSelectLoc.ClientID %>').click();

                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%=txtLocation.ClientID %>").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    //_renderItem = function (ul, item) {
                    var result_item = item.label;
                    var result_desc = item.desc;
                    var result_LocStatus = item.LocStatus;
                    var result_LocBalance = item.LocBalance;
                    var x = new RegExp('\\b' + queryloc, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    if (result_LocStatus == 1) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<span class='auto_item' style='color:Red;'>" + result_item + "</span><span class='auto_desc' style='color:Red;'>" + result_desc + "</span><span class='auto_item' style='color:Red;'> Bal:" + result_LocBalance + "</span>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<span class='auto_item'>" + result_item + "</span><span class='auto_desc'>" + result_desc + "</span><span class='auto_item'> Bal:" + result_LocBalance + "</span>")
                            .appendTo(ul);
                    }
                    
                   };


               

           }
        $(document).ready(function () {
            loadControlAutoComplete();
            var ref = getUrlParameter('uid');
            if (ref != '') {
                if ($('#<%= txtInvoice.ClientID %>').val()!= '') {
                    document.getElementById('<%=btnSelectLoc.ClientID %>').click();
                }
            }                        
            
         });
          
         function EmptyValue(txt) {
            if ($(txt).val() == '') { $('#<%= hdnPatientId.ClientID %>').val(''); }
         }
         
           
     </script>
  

    <script>
        function disableAccountWriteOff() {
            <%-- if ( $("#<%=hdnIsCreditWriteOff.ClientID %>").val() == "1") {
                     $('#<%=ddlCode.ClientID%>').attr('disabled', true);
                }--%>
        }

         function OpenWriteOffWindow() {
            var countItem = 0;
            var countcheck = 0;
            var ID = "";
            var Amount = "";
            var today = new Date();
            var isCredit = "";
            var custID = "";
             var hdnLocStatus = 0;
                var hdnTransID = 0;
            $('#<%=txtWriteOffDate.ClientID%>').val(today.toLocaleDateString("en-US"));

            $('#<%=txtDescription.ClientID%>').val('');
            $('#<%=txtInvoiceWriteOff.ClientID%>').val('');
            $('#<%=txtWriteOffAmount.ClientID%>').val('');
            $('#<%=txtWriteOffCust.ClientID%>').val('');
            $('#<%=txtWriteOffLoc.ClientID%>').val('');
             $('#<%=txtWriteOffLoc.ClientID%>').val('');
            $('#<%=hdnTransID.ClientID%>').val('');
            $("#<%=RadGrid_gvInvoice.ClientID %>").find('tr:not(:first,:last)').each(function () {
                //debugger
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:first:checked').each(function (index, value) {
                    if (countcheck == 0) {
                        // debugger
                        ID = $tr.find('input[id*=hdnID]').val();
                        Amount = $tr.find('input[id*=hdnPrevDue]').val();
                        debugger
                        custID = $tr.find('span[id*=lblID]').text();
                        isCredit = $tr.find('input[id*=hdnType]').val();
                        hdnLocStatus = $tr.find('input[id*=hdnLocStatus]').val();
                        hdnTransID= $tr.find('input[id*=hdnRefTranID]').val();
                        countcheck = 1;
                    }
                    countItem = countItem + 1;
                });

            });
            if (countItem > 1) {
                noty({
                    text: 'Please select only one invoice',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: false,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            }
            //if (hdnLocStatus != 0) {
            //    noty({
            //        text: 'This location is inactive. Please change the location name before proceeding',
            //        type: 'warning',
            //        layout: 'topCenter',
            //        closeOnSelfClick: false,
            //        timeout: false,
            //        theme: 'noty_theme_default',
            //        closable: true
            //    });
            //    return false;
            //}


            if (Amount != '') {
                if (confirm("Are you sure you want to write off this item in the amount off " + cleanUpCurrency('$' + parseFloat(Amount).toLocaleString("en-US", { minimumFractionDigits: 2 })) + " ?")) {
                    $('#<%=txtInvoiceWriteOff.ClientID%>').val(ID);
                    $('#<%=txtWriteOffAmount.ClientID%>').val(parseFloat(Amount).toLocaleString("en-US", { minimumFractionDigits: 2 }));


                    $('#<%=txtWriteOffCustID.ClientID%>').val(custID);
                    $("#<%=hdnIsWriteOff.ClientID %>").val("0");
                    $("#<%=hdnIsCreditWriteOff.ClientID %>").val(isCredit);

                      $("#<%=hdnTransID.ClientID %>").val(hdnTransID);

                    var wnd = $find('<%=RadWindowWriteOff.ClientID %>');
                    wnd.Show();
                    Materialize.updateTextFields();

                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }


        }
        function OpenCreditWindow() {
            var countItem = 0;
            var countcheck = 0;
            var ID = "";
            var Amount = "";
            var today = new Date();
            var isCredit = "";
            var custID = "";
            var hdnLocStatus = 0;
            var hdnTransID = 0;
            $('#<%=txtCreditDate.ClientID%>').val(today.toLocaleDateString("en-US"));

            $('#<%=txtDescriptionCredit.ClientID%>').val('');
            $('#<%=hdnInvoiceCredit.ClientID%>').val('');
            $('#<%=txtCreditAmount.ClientID%>').val('');
            
            $('#<%=hdnTransIDCredit.ClientID%>').val('');
            $("#<%=RadGrid_gvInvoice.ClientID %>").find('tr:not(:first,:last)').each(function () {
                //debugger
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:first:checked').each(function (index, value) {
                    if (countcheck == 0) {
                        // debugger
                        ID = $tr.find('input[id*=hdnID]').val();
                        Amount = $tr.find('input[id*=hdnPrevDue]').val();
                        debugger
                        custID = $tr.find('span[id*=lblID]').text();
                        isCredit = $tr.find('input[id*=hdnType]').val();
                        hdnLocStatus = $tr.find('input[id*=hdnLocStatus]').val();
                        hdnTransID = $tr.find('input[id*=hdnRefTranID]').val();
                        countcheck = 1;
                    }
                    countItem = countItem + 1;
                });

            });
            
            if (countItem > 1) {
                noty({
                    text: 'Please select only one invoice',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: false,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            }
            if (isCredit != 0) {
                noty({
                    text: 'Credit will be applied only on invoice.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: false,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            }
            //if (hdnLocStatus != 0) {
            //    noty({
            //        text: 'This location is inactive. Please change the location name before proceeding',
            //        type: 'warning',
            //        layout: 'topCenter',
            //        closeOnSelfClick: false,
            //        timeout: false,
            //        theme: 'noty_theme_default',
            //        closable: true
            //    });
            //    return false;
            //}


            if (Amount != '') {
                if (confirm("Are you sure you want to credit this item in the amount of " + cleanUpCurrency('$' + parseFloat(Amount).toLocaleString("en-US", { minimumFractionDigits: 2 })) + " ?")) {
                    $('#<%=hdnInvoiceCredit.ClientID%>').val(ID);
                    $('#<%=txtCreditAmount.ClientID%>').val(parseFloat(Amount).toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    
                    $("#<%=hdnIsCredit.ClientID %>").val("0");
                    $("#<%=hdnIsCreditWriteOff1.ClientID %>").val(isCredit);

                    $("#<%=hdnTransIDCredit.ClientID %>").val(hdnTransID);

                    var wnd = $find('<%=RadWindowCredit.ClientID %>');
                    wnd.Show();
                    Materialize.updateTextFields();

                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }


        }
        function ChkTotalAmount() {
            noty({
                text: 'Check amount is more then selected invoice payment, you can not proceed',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: false,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function CloseWriteOffWindow() {
            var wnd = $find('<%=RadWindowWriteOff.ClientID %>');
            wnd.Close();
        }
        function CloseCreditWindow() {
            var wnd = $find('<%=RadWindowCredit.ClientID %>');
            wnd.Close();
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
        function OpenUnapply() {
            if (confirm("Are you sure you want to unapply payment?")) {

                return true;
            } else {
                return false;
            }

        }
        function OpenTransferPaymentWindow() {
            var hasInv = 0;
            var isApplied = 0;
            var Amount = "";
              var Original = "";
            var countItem = 0;
            var isCredit = "";
            var hdnAcctID = document.getElementById('<%= hdnNewLocID.ClientID %>');
            var txtNewAddress = document.getElementById('<%= txtNewAddress.ClientID %>');
            var txtAccount = document.getElementById('<%= txtAccount.ClientID %>');
            hdnAcctID.value = "0";
            txtNewAddress.innerHTML = "";
            txtAccount.value = "";
            $("#<%=RadGrid_gvInvoice.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:first:checked').each(function (index, value) {
                    isCredit = $tr.find('input[id*=hdnType]').val();
           
                     //  debugger
                    Amount = convertNumber($tr.find('input[id*=hdnPrevDue]').val());
                    Original = convertNumber($tr.find('span[id*=lblOrigAmount]').text().replace('$',''));
                                  
                    if (isCredit == "2") {
                        countItem = countItem + 1;
                        if (Amount != Original) {
                            isApplied = 1;
                        }

                    } else {
                        hasInv = 1;
                    }

                });
            });
            if (hasInv == 1) {
                 noty({
                    text: 'Please do not select invoice to transfer',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: false,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            }
            if (countItem == 0) {
                noty({
                    text: 'Please select a credit to transfer',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: false,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return false;
            } else {
                //debugger
                if (isApplied == 1) {
                     noty({
                    text: 'Payment has already been partially applied and cannot be transfer',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: false,
                    theme: 'noty_theme_default',
                    closable: true
                    });

                } else {
                     var oWnd = $find("<%=RadWindowTransfer.ClientID%>");              
                oWnd.show();
                }
               
            }

        }
         function CloseTransferPaymentWindow() {
            var wnd = $find('<%=RadWindowTransfer.ClientID %>');
            wnd.Close();
        }
        function aceLoc_itemSelected(sender, e) {
            //debugger
            var hdnAcctID = document.getElementById('<%= hdnNewLocID.ClientID %>');
            var txtNewAddress = document.getElementById('<%= txtNewAddress.ClientID %>');
            var result=e.get_value().split("^");
            hdnAcctID.value = result[0];
            txtNewAddress.innerHTML = result[1];
            

        }
        function ValidateTransfer() {
          
            var hdnAcctID = document.getElementById('<%= hdnNewLocID.ClientID %>');
            if (hdnAcctID.value != 0) {
                return true
            } else {
                 noty({
                        text: 'Please select the location before proceeding',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: false,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                return false;
            }
            

        }
        function ValidateLocation() {
            var flag = false; 
            $("#<%=RadGrid_Location.ClientID%> input[id*='chkLocationSelectSelectCheckBox']:checkbox").each(function (index) {
                    if ($(this).prop('checked') == true) {
                        flag = true;
                    }
            });
            if (flag == false)
                noty({
                        text: 'Please select location.',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: false,
                        theme: 'noty_theme_default',
                        closable: true
                    });
            return flag;
        }

    </script>
    <script>
        function ValidateZero() {     
            debugger
            var flag = true; 
             //check Inactive Location
            var hdnLocStatus = document.getElementById('<%= hdnLocStatus.ClientID %>');
            if (hdnLocStatus.value != 0) {
                //flag = false;
                //noty({
                //        text: 'This location is inactive. Please change the location name before proceeding',
                //        type: 'warning',
                //        layout: 'topCenter',
                //        closeOnSelfClick: false,
                //        timeout: false,
                //        theme: 'noty_theme_default',
                //        closable: true
                //    });
            } else {
                var hasInactiveLoc = 0;
                $("#<%=RadGrid_gvInvoice.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                      var chk = $(this).attr('id');             
                   
                    if ($(this).prop('checked') == true) {
                         var hdnLocStatus = document.getElementById(chk.replace('chkSelect', 'hdnLocStatus'));
                      
                        if ($(hdnLocStatus).val() == 1) {
                            hasInactiveLoc = 1
                        }
                    }

                });
                if (hasInactiveLoc == 1) {
                    //flag = false;
                    //noty({
                    //    text: 'This location is inactive. Please change the location name before proceeding',
                    //    type: 'warning',
                    //    layout: 'topCenter',
                    //    closeOnSelfClick: false,
                    //    timeout: false,
                    //    theme: 'noty_theme_default',
                    //    closable: true
                    //});
                } else {
                     var countCheck = 0;
                    var amount = parseFloat($("#<%=txtAmount.ClientID %>").val().replace(/[\$\(\),]/g, ''))
                    if (amount == 0) {

                        $("#<%=RadGrid_gvInvoice.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {

                            var chk = $(this).attr('id');
                            if ($(this).prop('checked') == true) {
                                countCheck = countcheck + 1;
                            }

                        });
                        if (countCheck == 0) {
                            flag = false;
                        }
                    }
                    if (flag == false) {
                        noty({
                            text: 'Please enter the amount or select any invoice',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: false,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                    }
                }



               
            }


           
            return flag;
        }
    </script>
  
    <script>
        function CloseLocationsWindow() {
            
            var wnd = $find('<%=LocationsWindow.ClientID %>');
            wnd.Close();
        }

        function ShowTeamMemberWindow(txtTeamMember) {           

            var wnd = $find('<%=LocationsWindow.ClientID %>');
            wnd.set_title("Location " );
            wnd.Show();
        }

        function ValidateWriteOffAccount() {          
            var obj = $("#<%=ddlCode.ClientID %>").val();
            if (obj != 0) {
                return true;
            } else {
                 noty({
                            text: 'Please select billing code.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: false,
                            theme: 'noty_theme_default',
                            closable: true
                });
                return false;
            }          
            
        }
    </script>
</asp:Content>

