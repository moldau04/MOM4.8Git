<%@ Page Title="Recurring Invoices || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="RecurringInvoicesNew" Codebehind="RecurringInvoicesNew.aspx.cs" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <link href="Design/css/pikaday.css" rel="stylesheet" />


    <script type="text/javascript">


        function CreditHoldnotyConfirm() {
            noty({
                dismissQueue: true,
                layout: 'topCenter',
                theme: 'noty_theme_default',
                animateOpen: { height: 'toggle' },
                animateClose: { height: 'toggle' },
                easing: 'swing',
                text: 'Some location(s) are on credit hold. Would you like to exclude contracts for locations on credit hold?',
                type: 'alert',
                speed: 500,
                timeout: false,
                closeButton: false,
                closeOnSelfClick: true,
                closeOnSelfOver: false,
                force: false,
                onShow: false,
                onShown: false,
                onClose: false,
                onClosed: false,
                buttons: [
                    {
                        type: 'btn-primary', text: 'Yes', click: function ($noty) {
                            $noty.close();
                            
                            document.getElementById("<%=lnkExclude.ClientID%>").click();
                          
                        }
                    },
                    {
                        type: 'btn-warring', text: 'No', click: function ($noty) {
                            $noty.close();
                            
                            document.getElementById("<%=lnkInclude.ClientID%>").click();
                             
                        }
                    }
                    ,
                    {
                        type: 'btn-danger', text: 'Cancel', click: function ($noty) {
                            $noty.close();

                        }
                    }
                ],
                modal: true,
                template: '<div class="noty_message"><span class="noty_text"></span><div class="noty_close"></div></div>',
                cssPrefix: 'noty_',
                custom:
                {
                    container: null
                }
            });
        }




        function AddInvoiceClick(hyperlink) {

            if (validateForm()) {

                if (confirm('You are about to process invoices for selected period. This process will generate invoices for eligible accounts. Are you sure you want to proceed?')) { return true; } else { return false; }
            }
            else return false;
        }

        // For Avoid Resubmit From
        function AvoidResubmit() {
            document.getElementById('<%=AvoidResubmit.ClientID%>').click();
        }


        $(document).ready(function () {

<%--            $("#<%= txtStartDt.ClientID %>").keypress(function (event) {
                if (event.keyCode == 13) {
                    document.getElementById('<%=lnkSearch.ClientID%>').click();
                }
            });

            $("#<%= txtEndDate.ClientID %>").keypress(function (event) {
                if (event.keyCode == 13) {
                    document.getElementById('<%=lnkSearch.ClientID%>').click();
                }
            });--%>

            $('.ddlTerms').hover(
                function () { $(this).removeClass('focus') }
            )
            ///////////// Ajax call for customer auto search ////////////////////                
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
            }
            $("#ctl00_ContentPlaceHolder1_txtCustomer").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetCustomer",
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
                        //                        error: function(XMLHttpRequest, textStatus, errorThrown) {
                        //                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                        //                            alert(err.Message);
                        //                        }
                    });
                },
                select: function (event, ui) {
                    $("#ctl00_ContentPlaceHolder1_txtCustomer").val(ui.item.label);
                    $("#ctl00_ContentPlaceHolder1_hdnPatientId").val(ui.item.value);
                    $("#ctl00_ContentPlaceHolder1_txtLocation").focus();
                    $("#ctl00_ContentPlaceHolder1_txtLocation").val('');
                    $("#ctl00_ContentPlaceHolder1_hdnLocId").val('');
                    //                 document.getElementById('ctl00_ContentPlaceHolder1_btnSelectCustomer').click();
                    return false;
                },
                focus: function (event, ui) {
                    $("#ctl00_ContentPlaceHolder1_txtCustomer").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.label;
                    var result_desc = item.desc;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    return $("<li></li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                        .appendTo(ul);
                };


            ///////////// Ajax call for location auto search ////////////////////
            var queryloc = "";
            $("#ctl00_ContentPlaceHolder1_txtLocation").autocomplete(
                {
                    source: function (request, response) {
                        //                        if (document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value != '') {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.custID = 0;
                        if (document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value != '') {
                            dtaaa.custID = document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value;
                        }
                        queryloc = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "CustomerAuto.asmx/GetLocation",
                            //                              data: "{'prefixText':'" + request.term + "','con':'" + document.getElementById('ctl00_ContentPlaceHolder1_hdnCon').value + "','custID':" + document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value + "}",
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
                        $("#ctl00_ContentPlaceHolder1_txtLocation").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_hdnLocId").val(ui.item.value);
                        //                        document.getElementById('ctl00_ContentPlaceHolder1_btnSelectLoc').click();
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#ctl00_ContentPlaceHolder1_txtLocation").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.label;
                    var result_desc = item.desc;
                    var x = new RegExp('\\b' + queryloc, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    return $("<li></li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                        .appendTo(ul);
                };


            //            $("#<%--<%=pnlOptionsHeader.ClientID%>--%>").click(function() {
            //                $("#<%--<%=pnlOptions.ClientID%>--%>").slideToggle();
            //                return false;
            //            });


            ///////////// Validations for auto search ////////////////////
            $("#ctl00_ContentPlaceHolder1_txtCustomer").keyup(function (event) {
                var hdnPatientId = document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId');
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtCustomer').value == '') {
                    hdnPatientId.value = '';
                }
            });

            $("#ctl00_ContentPlaceHolder1_txtLocation").keyup(function (event) {
                var hdnLocId = document.getElementById('ctl00_ContentPlaceHolder1_hdnLocId');
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtLocation').value == '') {
                    hdnLocId.value = '';
                }
            });
 
            $('#<%=ddlSpecialNotes.ClientID%>').change(function () {
                $("#ctl00_ContentPlaceHolder1_gvOpenCalls").css("display", "none");
            })
            //TextboxState();

            $('#<%=ddlMonths.ClientID%>').change(function () {
                // debugger;
                $("#ctl00_ContentPlaceHolder1_gvOpenCalls").css("display", "none");
                var selectedVal = $('#<%=ddlMonths.ClientID%> option:selected').attr('value');

                var invoiceDt = $('#<%=txtInvoiceDt.ClientID%>').val();
                var dt = new Date(invoiceDt);
                var mm;
                var yy;
                var dd;
                if (invoiceDt != '') {
                    dd = '1';
                    yy = dt.getFullYear();
                }

                var newInvoiceDt = (parseInt(selectedVal) + 1).toString() + '/' + dd + '/' + yy;
                $('#<%=txtInvoiceDt.ClientID%>').val(newInvoiceDt);
            });

            $('#<%=ddlYears.ClientID%>').change(function () {
                // debugger;
                $("#ctl00_ContentPlaceHolder1_gvOpenCalls").css("display", "none");
                var selectedVal = $('#<%=ddlYears.ClientID%> option:selected').attr('value');

                var invoiceDt = $('#<%=txtInvoiceDt.ClientID%>').val();
                var dt = new Date(invoiceDt);
                var mm;
                var yy;
                var dd;
                if (invoiceDt != '') {
                    dd = '1';
                    mm = dt.getMonth();
                    yy = dt.getFullYear();
                }

                var newInvoiceDt = (parseInt(mm) + 1) + '/' + dd + '/' + selectedVal;
                $('#<%=txtInvoiceDt.ClientID%>').val(newInvoiceDt);
            });
        });

        function cancel() {
            window.parent.document.getElementById('ctl00_ContentPlaceHolder1_hideModalPopupViaServer').click();
        }
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
             <%--   $("#<%= txtSubAcct.ClientID %>").val('');--%>
        }
 

        ///////////// Custom validator function for customer auto search  ////////////////////
        function ChkCustomer(sender, args) {
            var hdnPatientId = document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId');
            if (hdnPatientId.value == '') {
                args.IsValid = false;
            }
        }

        ///////////// Custom validator function for location auto search  ////////////////////
        function ChkLocation(sender, args) {
            var hdnLocId = document.getElementById('ctl00_ContentPlaceHolder1_hdnLocId');
            if (hdnLocId.value == '') {
                args.IsValid = false;
            }
        }

        ///////////// Check Date valid ////////////////////
        function isDate(txtDate) {
            var currVal = txtDate;
            if (currVal == '')
                return false;
            //Declare Regex 
            var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
            var dtArray = currVal.match(rxDatePattern); // is format OK?
            if (dtArray == null)
                return false;
            //Checks for mm/dd/yyyy format.
            dtMonth = dtArray[1];
            dtDay = dtArray[3];
            dtYear = dtArray[5];

            if (dtMonth < 1 || dtMonth > 12)
                return false;
            else if (dtDay < 1 || dtDay > 31)
                return false;
            else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
                return false;
            else if (dtMonth == 2) {
                var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
                if (dtDay > 29 || (dtDay == 29 && !isleap))
                    return false;
            }
            return true;
        }
        ///////////// Validate Form ////////////////////
        function validateForm() {
            //debugger;
            var check = false;
            var msg = "";
            var invoiceDate = $(".txtInvoiceDt").val();
            var postingDate = $(".txtPostDt").val();
            var paymentTearm = $(".ddlTerms").val();
            var remark = $(".txtRemark").val();
            var checkRequireInvoiceDt = $("[id$='_vceInvoiceDt_popupTable']").is(":visible");
            var checkRequirePostingDt = $("[id$='_vcePostDt_popupTable']").is(":visible");
            var checkRequireRemark = $("[id$='_vceNotes_popupTable']").is(":visible");


            if (checkRequireInvoiceDt || checkRequirePostingDt || checkRequireRemark) {
                check = false;
                return check;
            }

            if (invoiceDate.length === 0) {
                msg = "Please set a date range."
                alert(msg);
                check = false;
                return check;
            }
            else if (invoiceDate.length > 0 && !isDate(invoiceDate)) {
                msg = "Invoice Date is invalid. Please set a date range again."
                alert(msg);
                check = false;
                return check;
            }
            else {
                check = true;
            }

            if (postingDate.length === 0) {
                msg = "Please set a date range."
                alert(msg);
                check = false;
                return check;
            }
            else if (postingDate.length > 0 && !isDate(postingDate)) {
                msg = "Posting Date is invalid. Please set a date range again."
                alert(msg);
                check = false;
                return check;
            }
            else {
                check = true;
            }

            if (paymentTearm == "") {
                msg = "Please select Payment Terms."
                $(".ddlTerms").addClass("focus");
                alert(msg);
                check = false;
                return check;
            }
            else {
                check = true;
            }

            if (remark.length == 0) {
                msg = "Please input Remark."
                alert(msg);
                check = false;
                return check;
            }
            else {
                check = true;
            }


            <%--  var hdnCreditHold = $("#<%=hdnCreditHold.ClientID%>").val();

            var ConfirmMessage3 = "Some location(s) are on credit hold. Would you like to exclude contracts for locations on credit hold?";

               if (hdnCreditHold == "1") { 
                  if (!confirm(ConfirmMessage3)) $("#<%=hdnCreditHold.ClientID%>").val('0'); 
                }--%>

            return check;
        }

        ///////////// Validate Form ////////////////////
        function setValuePostingDate() {
            var invoiceDate = $(".txtInvoiceDt").val();

            $(".txtPostDt").val(invoiceDate);
            $("[id$='_popupTable'] .ajax__validatorcallout_innerdiv").click();
            $(".txtPostDt").focus();
            $(".txtInvoiceDt").focus();


        }

        ///////////// Check Delete ////////////////////
        function CheckDelete() {
            var checkNumberRow = 0;
            var valueRowChecked = "";
            valueRowChecked = $(".rgRecurringInvoice input[type='checkbox']:checked").closest("tr").find("a").eq(0).text();
            var checkCheckboxChecked = $(".rgRecurringInvoice input[type='checkbox']:checked").length;

            if (checkCheckboxChecked === 1) {
                checkNumberRow = 1;
            }
            else if (checkCheckboxChecked > 1) {
                checkNumberRow = 2;
            }


            if (checkNumberRow === 1) {
                return confirm('Are you sure you want to remove contract [' + valueRowChecked + '] from this period?');
            }
            else if (checkNumberRow === 2) {
                return confirm('Are you sure you want to remove selected contracts from this period?');
            }
            else {
                alert('Please select Invoice.')
                return false;
            }
        }

        ///////////// Select all checkbox ////////////////////
        function checkAllChecBox() {
            var checked = $(".chkSelectAll input").is(":checked");
            if (checked) {
                $(".chkSelect input").prop("checked", true)
            }
            else {
                $(".chkSelect input").prop("checked", false)
            }
        }

        ///////////// Unselect all checkbox ////////////////////
        function unCheckSelectAll() {
            var checked = $(".chkSelect input").is(":checked");
            var checkedAll = $(".chkSelectAll input").is(":checked");
            var checkCountCheckbox = $(".chkSelect input:checked").length;
            var checkCountCheckboxSelected = $(".chkSelect input").length
            if (checked && checkedAll) {
                $(".chkSelectAll input").prop("checked", false);
            }

            if (checkCountCheckbox === checkCountCheckboxSelected) {
                $(".chkSelectAll input").prop("checked", true);
            }

        }

        ///////////// Hide select all checkbox ////////////////////
        function hideSelectAllChkb() {
            $(".chkSelectAll").hide();
        }
        ///////////// Show select all checkbox ////////////////////
        function showSelectAllChkb() {
            $(".chkSelectAll").show();
        }


    </script>
    <style type="text/css">
        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

        [id$='vceNotes_popupTable'] {
            width: 200px;
            top: -61px !important;
        }

        [id$='_RadAjaxPanel_RecurringInvoice'] .raDiv {
            background-position: top !important;
        }

        .ddlAddNewReport {
            display: none;
        }

        .textarea-border {
            border: 1px solid #aaa !important;
            border-radius: 5px !important;
            padding-left: 10px !important;
        }

        .ddlYears {
            margin-left: -32px;
            width: 125px !important;
        }

        .ddlMonths {
            width: 125px !important;
        }

        .validationtooltip {
            top: -61px;
        }

        .model-popup-body-print-invoce {
            background-color: #316b9d;
            height: 34px;
            padding: 5px 5px 0 15px;
            font-weight: bold;
            color: white;
        }

        .close_button_Form {
            cursor: pointer;
        }

        .focus {
            border-bottom: 2px solid #4598DA !important;
            color: #4598DA;
        }
          .TaxType{
            margin-top:25px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="divbutton-container">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-content-reply-all"></i>&nbsp;Recurring Invoices</div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="true" ToolTip="Process" ValidationGroup="Invoice"
                                                OnClick="lnkProcess_Click" Enabled="true" OnClientClick="return AddInvoiceClick(this)">Process</asp:LinkButton>
                                        </div>

                                        <div class="btnlinks">
                                            <a class="dropdown-button" data-beloworigin="true" href="CustomersReport.aspx?type=Customer" data-activates="dropdown1">Reports
                                            </a>
                                        </div>
                                        <ul id="dropdown1" class="dropdown-content ddlAddNewReport">
                                            <li>
                                                <a href="CustomersReport.aspx?type=Customer" class="-text">Add New Report</a>
                                            </li>
                                            <li>
                                            
                                                <a id="lnk_InvoiceMaint" href="CustomersReport.aspx?type=Customer" runat="server" class="-text" title="Process" style="float: left" onserverclick="lnk_InvoiceMaint_Click" enabled="true" onclick="javascript:return confirm('You are about to process invoices for selected period. This process will generate invoices for eligible accounts. Are you sure you want to proceed?');">PESMCT MAINT</a>
                                            <li>
                                            
                                                <a id="lnk_InvoiceException" href="#" runat="server" class="-text" title="Process" style="float: left" onserverclick="lnk_InvoiceException_Click" enabled="true" onclick="javascript:return confirm('You are about to process invoices for selected period. This process will generate invoices for eligible accounts. Are you sure you want to proceed?');">PESMCTPESMCT EXCEPTION</a>
                                            <li>
                                  
                                                <a id="lnk_InvoiceLNY" href="#" runat="server" class="-text" title="Process" style="float: left" onserverclick="lnk_InvoiceLNY_Click" enabled="true" onclick="javascript:return confirm('You are about to process invoices for selected period. This process will generate invoices for eligible accounts. Are you sure you want to proceed?');">Invoice-LNY</a>
                                        </ul>

                                        <div class="btnlinks menuAction">
                                            <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                            </a>
                                        </div>
                                        <ul id="drpMenu" class="nomgn hideMenu menuList">
                                            <li>
                                                <div class="btnlinks btnDeleteContainer" style="margin-bottom: 4px;">
                                                    <a id="lnk_InvoiceDelete" runat="server" onserverclick="lnkDelete_Click" onclick="return CheckDelete()">Delete</a>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                </div>
                                            </li>

                                        </ul> 
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                            OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </header>
            </div>
            

            <div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12 right-details">
                        <div class="row">
                             <div class="tblnks">
                                <ul class="anchor-links">                                   
                                    <li id="li1" runat="server" ><a href="#accrdlogs">Logs</a></li>
                                </ul>
                            </div>

                            <ul class="anchor-links center-on-small-only right-align ">
                                <li>
                                    <asp:Label runat="server" ID="lblLastProcessDate" CssClass="title_text_Name_1"></asp:Label>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="lblProcessPeriod" CssClass="title_text_Name_1 "></asp:Label>
                                </li>
                                
                            </ul>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="alert alert-warning divAlert" runat="server" id="divSuccess" style="color: red">
                <button type="button" class="close" onclick="$('.divAlert').hide()">×</button>
                These month/year period is closed out. You do not have permission to add/update this record.
            </div>
            <div class="srchpane-advanced">
                <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px; min-width: 75px;">
                        Month
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlMonths" runat="server" Width="125px" CssClass="browser-default selectst selectsml ddlMonths" OnSelectedIndexChanged="ddlMonths_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                        Year
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlYears" runat="server" Width="125px " CssClass="browser-default selectst selectsml ddlYears" OnSelectedIndexChanged="ddlYears_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 20px;">
                        Special Notes
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlSpecialNotes" Width="100px" runat="server" CssClass="browser-default selectst selectsml"
                            TabIndex="7" OnSelectedIndexChanged="ddlSpecialNotes_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="-1">All</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap srchclr btnlinksicon" style="margin-left: 13px;">

                     

                        <asp:LinkButton ID="LinkButton1" CssClass="submit" runat="server" CausesValidation="false"
                            OnClick="lnkSearch_Click"><i class="mdi-action-search" style="margin-left: 0 !important;"></i></asp:LinkButton>
                    </div>

                </div>
                <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                        Customer
                    </div>

                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtCustomer" runat="server" autocomplete="off" CssClass="validate srchcstm" Width="315px"></asp:TextBox>
                        <asp:HiddenField ID="hdnPatientId" runat="server" />
                        <asp:FilteredTextBoxExtender ID="txtCustomer_FilteredTextBoxExtender" runat="server"
                            Enabled="False" FilterMode="InvalidChars" InvalidChars="'\"
                            TargetControlID="txtCustomer">
                        </asp:FilteredTextBoxExtender>
                    </div>
                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 44px;">
                        Location
                    </div>

                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtLocation" runat="server" autocomplete="off" CssClass="validate srchcstm" Width="315px"></asp:TextBox>
                        <asp:HiddenField ID="hdnLocId" runat="server" />
                        <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                            Enabled="false" FilterMode="InvalidChars" InvalidChars="'\"
                            TargetControlID="txtLocation">
                        </asp:FilteredTextBoxExtender>
                    </div>

                    <div class="srchinputwrap col lblsz2 lblszfloat">
                        <div class="row">
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClear" OnClick="lnkClear_Click" runat="server">Clear</asp:LinkButton>
                            </span>
                            <span class="tro trost">
                                <asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </span>
                        </div>
                    </div>

                    <asp:LinkButton ID="AvoidResubmit" CausesValidation="false" OnClick="AvoidResubmit_Click" runat="server"></asp:LinkButton>
                </div>

                <div class="srchpaneinner">
                    <div class="form-section-row">
                        <div class="section-ttle">Set Options</div>
                        <div class="tools">
                            <a href="javascript:;" class="collapse" data-original-title="" title=""></a>
                        </div>
                        <div class="form-section-row">
                            <div class="form-section3">
                                <div class="input-field col s5">
                                    <div class="row">
                                        <label for="datepicker">Invoice Date</label>
                                        <asp:TextBox ID="txtInvoiceDt" runat="server" CssClass="datepicker_mom txtInvoiceDt" MaxLength="28"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvInvoiceDt"
                                            runat="server" ControlToValidate="txtInvoiceDt" Display="None" ErrorMessage="Please set a date range." ValidationGroup="Invoice"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceInvoiceDt" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="rfvInvoiceDt" />
                                    </div>
                                </div>
                                <div class="input-field col s2">
                                    <div class="row">
                                        &nbsp;
                                    </div>
                                </div>

                                <div class="input-field col s5">
                                    <div class="row">
                                        <label for="txtPostDt">Posting Date</label>
                                        <asp:TextBox ID="txtPostDt" runat="server" CssClass="datepicker_mom txtPostDt" MaxLength="28"></asp:TextBox>

                                        <asp:RequiredFieldValidator ID="rfvPostDt"
                                            runat="server" ControlToValidate="txtPostDt" Display="None" ErrorMessage="Please set a date range." ValidationGroup="Invoice"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vcePostDt" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="rfvPostDt" />
                                    </div>
                                </div>


                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">

                                <div class="input-field col s12">
                                    <div class="row">
                                        <label class="drpdwn-label">Payment Terms</label>
                                        <asp:DropDownList ID="ddlTerms" TabIndex="6" runat="server" CssClass="browser-default ddlTerms">
                                            <asp:ListItem Value="">Choose your option</asp:ListItem>
                                            <asp:ListItem Value="0">Upon Receipt</asp:ListItem>
                                            <asp:ListItem Value="1">Net 10 Days</asp:ListItem>
                                            <asp:ListItem Value="2">Net 15 Days</asp:ListItem>
                                            <asp:ListItem Value="3">Net 30 Days</asp:ListItem>
                                            <asp:ListItem Value="4">Net 45 Days</asp:ListItem>
                                            <asp:ListItem Value="5">Net 60 Days</asp:ListItem>
                                            <asp:ListItem Value="6">2%-10/Net 30 Days</asp:ListItem>
                                            <asp:ListItem Value="7">Net 90 Days</asp:ListItem>
                                            <asp:ListItem Value="8">Net 180 Days</asp:ListItem>
                                            <asp:ListItem Value="9">COD</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>


                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                            </div>
                        </div>
                           <div class="form-section-row" id="divDropDownlist" runat="server">
                            <div class="form-section3">
                               <div class="input-field col s5">
                                    <div class="row">
                                        <label for="drpdwn-label" >Taxable</label>
                                        <asp:DropDownList runat="server" ID="ddlTaxType" AutoPostBack="true" OnSelectedIndexChanged="ddlTaxType_SelectedIndexChanged" CssClass="browser-default TaxType">
                                            <asp:ListItem Value="All">All</asp:ListItem>
                                            <asp:ListItem Value="PST">HST/PST Only</asp:ListItem>
                                            <asp:ListItem Value="GST">GST Only</asp:ListItem>
                                            <asp:ListItem Value="None">None</asp:ListItem>
                                        </asp:DropDownList>                                         
                                    </div>                                   
                                </div>   
                            </div>
                        </div>
                        <div class="input-field col s5" id="divCheckBox" runat="server">
                            <div class="checkrow">
                                <asp:CheckBox ID="chkTaxable" CssClass="filled-in" OnCheckedChanged="chkTaxable_CheckedChanged" AutoPostBack="true" TabIndex="5" runat="server" />

                                <label for="drpdwn-label" runat="server">Taxable</label>
                            </div>
                            <div class="input-field col s5 form-section-row" style="padding-left: 0;">
                            </div>
                        </div>
                        <div class="input-field col s12">
                            <div class="row">
                                <asp:TextBox ID="txtRemark" CssClass="textarea-border materialize-textarea txtRemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNotes" runat="server" ControlToValidate="txtRemark" Display="None" ErrorMessage="Remark is required"
                                    ValidationGroup="Invoice" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vceNotes" runat="server" Enabled="True" PopupPosition="TopLeft" Width="300" TargetControlID="rfvNotes" />
                                <label for="txtRemark" class="txtbrdlbl" id="txtbrdlbl" enableviewstate="true" runat="server">Invoice Remarks</label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="srchpaneinner">
                </div>
            </div>

            <div class="grid_container">
                <div class="form-section-row" style="margin-bottom: 0 !important;">
                    <telerik:RadAjaxManager ID="RadAjaxManager_RecurringTickets" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="lnkProcess">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="rgRecurringInvoice" LoadingPanelID="RadAjaxLoadingPanel_RecurringInvoices" />
                                     <telerik:AjaxUpdatedControl ControlID="gvLogs"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="rgRecurringInvoice" LoadingPanelID="RadAjaxLoadingPanel_RecurringInvoices" />
                                    <telerik:AjaxUpdatedControl ControlID="gvLogs"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="ddlTaxType">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="rgRecurringInvoice" LoadingPanelID="RadAjaxLoadingPanel_RecurringInvoices" />
                                    <telerik:AjaxUpdatedControl ControlID="gvLogs"  />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_RecurringInvoices" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= rgRecurringInvoice.ClientID %>");
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
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_RecurringInvoice" runat="server" CssClass="rgRecurringInvoice" LoadingPanelID="RadAjaxLoadingPanel_RecurringInvoices" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadPersistenceManager ID="RadPersistenceManager1" runat="server">
                                <PersistenceSettings>
                                    <telerik:PersistenceSetting ControlID="rgRecurringInvoice" />
                                </PersistenceSettings>
                            </telerik:RadPersistenceManager>


                                 <div class="RadGrid RadGrid_Material">

                            <telerik:RadGrid ID="rgRecurringInvoice" runat="server" AutoGenerateColumns="False" Width="100%"
                                PageSize="50"  ShowFooter="True" OnRowCommand="gvOpenCalls_RowCommand"
                                
                                AllowSorting="True" OnSorting="gvOpenCalls_Sorting" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" AllowPaging="false"
                                PagerStyle-AlwaysVisible="true"
                                ShowStatusBar="true"
                                OnNeedDataSource="rgRecurringInvoice_NeedDataSource"
                                OnItemDataBound="rgRecurringInvoice_ItemDataBound"
                                OnItemEvent="rgRecurringInvoice_ItemEvent"
                                OnPreRender="rgRecurringInvoice_PreRender"
                                OnItemCreated="rgRecurringInvoice_ItemCreated">
                                <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                <SelectedItemStyle CssClass="selectedrowcolor" />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>

                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                </ClientSettings>

                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" CanRetrieveAllData="false">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="30" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" CssClass="chkSelect" runat="server" onchange="unCheckSelectAll();" />
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" CssClass="chkSelectAll" onchange="checkAllChecBox();" runat="server" />
                                            </HeaderTemplate>
                                        </telerik:GridTemplateColumn> 
                                          <telerik:GridTemplateColumn HeaderText="Invoice#" HeaderStyle-Width="100" UniqueName="InvoiceID" SortExpression="InvoiceID" Visible="true" ShowFilterIcon="false">
                                            <ItemTemplate>
                                            <asp:Label ID="lblinvId" runat="server" Visible="false" Text='<%# Bind("InvoiceID") %>'  ></asp:Label>  
                                           
                                                     <asp:HyperLink ID="lnkinv" runat="server" NavigateUrl='<%# "addInvoice.aspx?uid=" + Eval("InvoiceID") %>'
                                                    Target="_blank" Text='<%# Bind("InvoiceID") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Contract#" SortExpression="id" Visible="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container, "ItemIndex") %>'></asp:Label>
                                            <asp:Label ID="RecurringlblLocID" runat="server" Text='<%# Bind("loc") %>'></asp:Label> 
                                            <asp:Label ID="RecurringlblJobID" runat="server" Text='<%# Bind("Job") %>'></asp:Label>
                                            <asp:Label ID="Recurringlblcredithold" runat="server" Text='<%# Bind("credit") %>'></asp:Label>              
                                               
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        
                                      

                                        <telerik:GridTemplateColumn HeaderText="Contract#" HeaderStyle-Width="100" UniqueName="job" DataField="job" SortExpression="job"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobID" Style="display: none" runat="server" Text='<%# Bind("Job") %>'></asp:Label>
                                                <asp:HyperLink ID="lnkJob" runat="server" NavigateUrl='<%# "addreccontract.aspx?rt=1&uid=" + Eval("job") %>'
                                                    Target="_blank" Text='<%# Bind("job") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Customer" HeaderStyle-Width="200" UniqueName="customername" DataField="customername" SortExpression="customername"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustName" runat="server" Text='<%# Bind("customername") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Location" HeaderStyle-Width="50" Visible="false" UniqueName="locid" DataField="locid" ShowFilterIcon="false" SortExpression="locid" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocid" runat="server" Text='<%# Bind("locid") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Acct #" HeaderStyle-Width="150" SortExpression="ItemDesc" UniqueName="ItemDesc" ShowFilterIcon="false" DataField="ItemDesc" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocid1" runat="server" Text='<%# Bind("ItemDesc") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>


                                        <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="150" SortExpression="Company" DataField="Company" ShowFilterIcon="false" UniqueName="Company" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompany" runat="server" Text='<%# Bind("Company") %>'>></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>


                                          <telerik:GridTemplateColumn HeaderStyle-Width="100"     HeaderText="Credit Hold" AllowFiltering="false" SortExpression="credit" Visible="true" ShowFilterIcon="false">
                                             
                                            <ItemTemplate> 
                                          <img id="imgCreditH" runat="server" visible='<%# (Eval("credit").ToString() == "1")?true:false %>' title="Credit Hold" src="images/MSCreditHold.png" style="float: left; width: 16px; background-color: rgba(255, 0, 0, 0.34)">
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                          
                                        <telerik:GridTemplateColumn HeaderText="LocationName" HeaderStyle-Width="200" UniqueName="locname" DataField="locname" ShowFilterIcon="false" SortExpression="locname" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                 
                                                <asp:Label ID="lblLoc" runat="server" Text='<%# Bind("locname") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>                                      

                                        <telerik:GridTemplateColumn HeaderText="Frequency" HeaderStyle-Width="100" SortExpression="frequency" DataField="frequency" ShowFilterIcon="false" UniqueName="frequency" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFreq" runat="server" Text='<%# Bind("frequency") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="ServiceType" HeaderStyle-Width="100" SortExpression="servicetype" DataField="servicetype" ShowFilterIcon="false" UniqueName="servicetype" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSvcType" runat="server" Text='<%# Bind("servicetype") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Posting date" Visible="false" SortExpression="fdate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvDate" runat="server" Text='<%# Bind("fdate", "{0:MM/dd/yy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                          <telerik:GridTemplateColumn HeaderText="Pretax Amount" HeaderStyle-Width="100" SortExpression="amount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" DataField="amount" ShowFilterIcon="false" UniqueName="amount" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPretaxAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        
                                           <telerik:GridTemplateColumn HeaderText="GST Tax" HeaderStyle-Width="70" SortExpression="GSTTax" FooterAggregateFormatString="{0:c}"  DataField="GST" UniqueName="GSTTax" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                            <ItemTemplate>
                                         
                                                 <asp:Label ID="lblGSTTax" runat="server" Text='<%# ((System.Data.DataRowView)Container.DataItem).DataView.Table.Columns.Contains("GST")== true?DataBinder.Eval(Container.DataItem, "GST", "{0:c}"):"0"  %>'></asp:Label>
                                               
                                            </ItemTemplate>
                                           
                                        </telerik:GridTemplateColumn>


                                        <telerik:GridTemplateColumn HeaderText="HST/PST Tax" HeaderStyle-Width="70" SortExpression="stax" FooterAggregateFormatString="{0:c}" Aggregate="Sum" DataField="stax" UniqueName="stax" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesTax" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "stax", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>
                                           
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Invoice Total" HeaderStyle-Width="70" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="total" DataField="total" UniqueName="InvoiceTotal" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                            <ItemTemplate>
                                                <asp:Label ID="InvInvoiceTotal" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "total", "{0:c}") %>'></asp:Label>
                                            </ItemTemplate>                                          
                                        </telerik:GridTemplateColumn>

                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>

                                     </div>
                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
            <div>
                <a href="#" runat="server" value="Add New" id="btnAddNew" onclick="Javascript:storeAcctNum();"></a>

                <asp:ModalPopupExtender ID="mpeRecurInv" BackgroundCssClass="ModalPopupBG"
                    runat="server" CancelControlID="btnCancel" OkControlID="btnOkay"
                    TargetControlID="btnAddNew" PopupControlID="pnlRecurringInv"
                    Drag="true" PopupDragHandleControlID="PopupHeader" OnOkScript="ReloadPage();">
                </asp:ModalPopupExtender>

                <div class="popup_Buttons" style="display: none">
                    <input id="btnOkay" value="Done" type="button" />
                    <input id="btnCancel" value="Cancel" type="button" />
                </div>

           
 


                 <div id="pnlRecurringInv" class="table-subcategory" style="display: none; width: 560px; height: 175px; background: #fff;">
                    <div class="popup_Container">
                        <div class="popup_Body">
                            <div class="model-popup-body model-popup-body-print-invoce" style="padding-bottom: 24px;">
                                <asp:Label CssClass="title_text" Style="float: left" ID="lblVoidCheck" runat="server">Print Invoice</asp:Label>

                                <div style="float: right;">
                                    <asp:LinkButton CssClass="save_button" ID="lnkSave" Style="color: white; margin-right: 8px;" runat="server" OnClick="lnkSave_Click"
                                        TabIndex="38" CausesValidation="true" ValidationGroup="SubCheck"> Next </asp:LinkButton>

                                    <%--   <a class="close_button_Form" id="lbtnClose" style="color: white;" onclick="cancel();">Close </a>--%>

                                    <asp:LinkButton CssClass="save_button" ID="lbtnClose" OnClientClick="cancel();" Style="color: white; margin-right: 8px;" runat="server" OnClick="lbtnClose_Click"
                                        CausesValidation="false"> Close </asp:LinkButton>

                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="col-lg-12 col-md-12" style="padding: 7px 10px 0 10px;">
                                <div class="com-cont">
                                    <%--<asp:LinkButton ID="lnkPrint" CssClass="icon-print" runat="server" ToolTip="Print" CausesValidation="true" OnClick="lnkPrint_Click"> </asp:LinkButton>--%>
                                    <div class="col-md-6 col-lg-6">
                                        <div class="form-col">
                                            <%-- by rifaqat<div class="fc-label" style="width:200px;margin-right: 15px;">
                                                    <label> Please enter check Void Date. </label>
                                                </div>--%>
                                            <div class="fc-input" style="padding-left: 20px; width: 110px;">
                                                <asp:HiddenField ID="hdnCDID" runat="server" />
                                                <%--   <asp:HiddenField ID="hdnTRID" runat="server" />
                                                    <asp:HiddenField ID="hdnBatchID" runat="server" />--%>
                                                <div class="form-group">
                                                    <div style="float: left" class="checkrow">
                                                        <label>Starting Invoice # </label>
                                                    </div>
                                                    <div style="margin-left: 100px">
                                                        <asp:TextBox ID="txtStartInv" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvStartInv" runat="server"
                                                            ControlToValidate="txtStartInv" Display="None" ErrorMessage="Start Invoice is Required"
                                                            ValidationGroup="SubCheck" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceStartInv" runat="server" Enabled="True"
                                                            PopupPosition="Right" TargetControlID="rfvStartInv" />
                                                    </div>
                                                    <br />
                                                    <div style="float: left" class="checkrow">
                                                        <label>Ending Invoice # </label>
                                                    </div>
                                                    <div style="margin-left: 100px">
                                                        <asp:TextBox ID="txtEndInv" runat="server" CssClass="form-control" Width="150px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvEndInv" runat="server"
                                                            ControlToValidate="txtEndInv" Display="None" ErrorMessage="End Invoice is Required"
                                                            ValidationGroup="SubCheck" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceEndInv" runat="server" Enabled="True"
                                                            PopupPosition="Right" TargetControlID="rfvEndInv" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-col">
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>


               

                                     <asp:Button CssClass="btn-box" ID="lnkExclude"  Style="display:none"   runat="server" OnClick="lnkExclude_Click"
                                          CausesValidation="false" Text="Yes"  >  </asp:Button>

                                            
                                    <asp:Button CssClass="btn-box" ID="lnkInclude" Style="display:none"   runat="server" OnClick="lnkInclude_Click"
                                        CausesValidation="false" Text="No">   </asp:Button>

                                      
                        

            </div>
        </div>
        <div class="accordian-wrap">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li id="tbLogs" runat="server" >
                            <div id="accrdlogs" class="collapsible-header accrd  accordian-text-custom"><i class="mdi-content-content-paste"></i>Logs</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
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
                                                        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid_gvLogs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true" OnItemCreated="RadGrid_gvLogs_ItemCreated"
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
    <asp:HiddenField ID="hdnCreditHold" runat="server" Value="0" />
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
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

            $('#addinfo').hide();
            $('.add-btn-click').click(function () {

                $('#addinfo').slideToggle('2000', "swing", function () {
                    // Animation complete.

                });

                if ($('.divbutton-container').height() != 65)
                    $('.divbutton-container').animate({ height: 65 }, 500);
                else
                    $('.divbutton-container').animate({ height: 350 }, 500);


            });

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

        });
    </script>
   
</asp:Content>





