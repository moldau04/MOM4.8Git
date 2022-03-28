<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="BankRecon" CodeBehind="BankRecon.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <style>
        input[readonly] {
            background-color: #fff !important;
        }

        input[readonly] {
            color: inherit !important;
        }

        input[disabled="disabled"] {
            background-color: #fff !important;
        }

        input[disabled] {
            background-color: #fff !important;
        }

        textarea.materialize-textarea {
            padding: 1.2rem 0;
        }
    </style>
    <script type="text/javascript">        
        $(document).ready(function () {
            $("#<%=txtServiceAccount.ClientID%>").prop("disabled", true);
            $("#<%=txtInterestAccount.ClientID%>").prop("disabled", true);
            $("#<%=txtServiceChargeDate.ClientID%>").prop("disabled", true);
            $("#<%=txtInterestDate.ClientID%>").prop("disabled", true);

            if (parseFloat($("#<%=txtServiceChrgAmount.ClientID%>").val()) != 0) {

                $("#<%=txtServiceChargeDate.ClientID%>").prop("disabled", false);
                $("#<%=txtServiceAccount.ClientID%>").prop("disabled", false);
            }
            else {
                $("#<%=txtServiceChargeDate.ClientID%>").prop("disabled", true);
                $("#<%=txtServiceChargeDate.ClientID%>").prop("disabled", true);
            }
            if (parseFloat($("#<%=txtInterestAmount.ClientID%>").val()) != 0) {
                $("#<%=txtInterestDate.ClientID%>").prop("disabled", false);
                $("#<%=txtInterestAccount.ClientID%>").prop("disabled", false);
            } else {
                $("#<%=txtInterestDate.ClientID%>").prop("disabled", true);
                $("#<%=txtInterestAccount.ClientID%>").prop("disabled", true);
            }
        });

        window.onload = calculateAmt;
        function calculateAmt() {
            var count = 0;
            var chkAmt = 0.00;
            $("#<%=gvCheck.ClientID%> tbody input[id*='chkSelect']:checkbox").each(function (index) {

                if ($(this).is(':checked')) {
                    var hdnAmount = $(this).closest('tr').find('td #hdnAmount');
                    var amt = parseFloat(hdnAmount.val());
                    amt = amt * -1;
                    chkAmt = chkAmt + amt;
                    count++;
                }
                else {
                    $(this).closest('tr').removeAttr("style");
                }
            });
            $("#<%=lblCheckCount.ClientID%>").text(count);
            $("#<%=lblCheckAmount.ClientID%>").text(cleanUpCurrency('$' + chkAmt.toLocaleString("en-US", { minimumFractionDigits: 2 })));
            var count = 0;
            var depAmt = 0.00;
            $("#<%=gvDeposit.ClientID%> tbody input[id*='chkSelect']:checkbox").each(function (index) {

                if ($(this).is(':checked')) {
                    //SelectedRowStyle('<%=gvDeposit.ClientID %>')
                    //var chkSelect = $(this).attr('id');

                    //var hdnAmount = document.getElementById(chkSelect.replace('chkSelect', 'hdnAmount'));
                    var hdnAmount = $(this).closest('tr').find('td #hdnAmount');
                    var amt = parseFloat(hdnAmount.val());

                    depAmt = depAmt + amt;
                    count++;
                }
                else {
                    $(this).closest('tr').removeAttr("style");
                }
            });

            $("#<%=lblDepositCount.ClientID%>").text(count);
            $("#<%=lblDepositAmount.ClientID%>").text(cleanUpCurrency('$' + depAmt.toLocaleString("en-US", { minimumFractionDigits: 2 })));
            calculateDifference()
        }

        ///////////// Validate Form ////////////////////
        function validateForm() {
            //debugger;
            var check = false;
            var msg = "";
            check = true;

            return check;
        }

        function AddReconClick(hyperlink) {
            
            if (validateForm()) {
                
                if (confirm('You are about to process bank reconcilation. Are you sure you want to proceed?')) {
                    /// Add Below code due to ES-7998 Gable Elevator || Bank Reconciliation Process Not Working
                    /// Ending balance can be $0.00 
                    if ($("#<%=txtEndingBalance.ClientID%>").val() == '$0.00') {
                        $("#<%=txtEndingBalance.ClientID%>").val('0.00');
                    }

                    return true;


                } else { return false; }
            }
            else return false;
        }

        function isDecimalKey(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;

            var number = el.value.split('.');
            if (charCode != 46 && charCode > 31 && (charCode > 57)) {
                return false;
            }

            if (number.length > 1 && charCode == 46) {
                return false;
            }
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                return false;
            }

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

        function VisibleTxtService(obj) {
            if (obj.id == "<%=txtServiceChrgAmount.ClientID%>") {
                var endBalance = parseFloat(document.getElementById(obj.id).value);

                if (endBalance > 0) {

                    $("#<%=txtServiceChargeDate.ClientID%>").prop("disabled", false);
                    $("#<%=txtServiceAccount.ClientID%>").prop("disabled", false);

                    $("#<%=txtServiceChargeDate.ClientID%>").val($("#<%=txtStatementDate.ClientID%>").val())
                }
                else {
                    $("#<%=txtServiceChargeDate.ClientID%>").prop("disabled", true);
                    $("#<%=txtServiceAccount.ClientID%>").prop("disabled", true);
                }
            }
            else {
                var endBalance = parseFloat(document.getElementById(obj.id).value);

                if (endBalance > 0) {
                    $("#<%=txtInterestDate.ClientID%>").prop("disabled", false);
                    $("#<%=txtInterestAccount.ClientID%>").prop("disabled", false);

                    $("#<%=txtInterestDate.ClientID%>").val($("#<%=txtStatementDate.ClientID%>").val())
                }
                else {
                    $("#<%=txtInterestDate.ClientID%>").prop("disabled", true);
                    $("#<%=txtInterestAccount.ClientID%>").prop("disabled", true);
                }
            }
            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toLocaleString("en-US", { minimumFractionDigits: 2 });
            }

            calculateDifference()
        }

        function calculateCheckAmount() {
            var count = 0;
            var chkAmt = 0.00;
            $("#<%=gvCheck.ClientID%> tbody input[id*='chkSelect']:checkbox").each(function (index) {

                if ($(this).is(':checked')) {
                    var hdnAmount = $(this).closest('tr').find('#hdnAmount');
                    var amt = parseFloat(hdnAmount.val());
                    if (!isNaN(amt)) {
                        amt = amt * -1;
                        chkAmt = chkAmt + amt;
                        count++;
                    }
                }
                else {
                    $(this).closest('tr').removeAttr("style");
                }
            });

            $("#<%=lblCheckCount.ClientID%>").text(count);
            $("#<%=lblCheckAmount.ClientID%>").text(cleanUpCurrency('$' + chkAmt.toLocaleString("en-US", { minimumFractionDigits: 2 })));
            calculateDifference()
        }

        function calculateDepositAmount() {

            var count = 0;
            var depAmt = 0.00;
            $("#<%=gvDeposit.ClientID%> tbody input[id*='chkSelect']:checkbox").each(function (index) {

                if ($(this).is(':checked')) {
                    var hdnAmount = $(this).closest('tr').find('#hdnAmount');
                    var amt = parseFloat(hdnAmount.val());

                    if (!isNaN(amt)) {
                        depAmt = depAmt + amt;
                        count++;
                    }
                }
                else {
                    $(this).closest('tr').removeAttr("style");
                }
            });

            $("#<%=lblDepositCount.ClientID%>").text(count);
            $("#<%=lblDepositAmount.ClientID%>").text(cleanUpCurrency('$' + depAmt.toLocaleString("en-US", { minimumFractionDigits: 2 })));

            calculateDifference()
        }

        function calculateDifference() {

            var endBal = 0;
            var sc = 0;
            var interest = 0;
            var chkAmt = 0;
            var depAmt = 0;
            var beginBal = 0;

            chkAmt = parseFloat($("#<%=lblCheckAmount.ClientID%>").text().toString().replace(/[\$\(\),]/g, ''));
            depAmt = parseFloat($("#<%=lblDepositAmount.ClientID%>").text().toString().replace(/[\$\(\),]/g, ''));

            if ($("#<%=lblBeginBalance.ClientID%>").val().toString().includes("(")) {
                beginBal = parseFloat($("#<%=lblBeginBalance.ClientID%>").val().toString().replace(/[\$\(\),]/g, ''));
                beginBal = beginBal * -1;
            } else {
                beginBal = parseFloat($("#<%=lblBeginBalance.ClientID%>").val().toString().replace(/[\$\(\),]/g, ''));
            }


            if (!isNaN(convertNumber($("#<%=txtEndingBalance.ClientID%>").val().replace("$", '')))) {
                endBal = convertNumber($("#<%=txtEndingBalance.ClientID%>").val().replace("$", ''));
            }

            if (!isNaN(parseFloat($("#<%=txtServiceChrgAmount.ClientID%>").val().toString().replace(/[\$\(\),]/g, '')))) {
                sc = parseFloat($("#<%=txtServiceChrgAmount.ClientID%>").val().toString().replace(/[\$\(\),]/g, ''));
            }
            if (!isNaN(parseFloat($("#<%=txtInterestAmount.ClientID%>").val().toString().replace(/[\$\(\),]/g, '')))) {
                interest = parseFloat($("#<%=txtInterestAmount.ClientID%>").val().toString().replace(/[\$\(\),]/g, ''));
            }

            //(Beginning Balance + Deposits + Interest Income)  – (Checks + Service charge) = Ending Balance
            var difference = parseFloat(beginBal);

            difference = (difference + depAmt).toFixed(2);
            difference = parseFloat(difference) + parseFloat(interest);
            difference = parseFloat(difference) - parseFloat(chkAmt);
            difference = parseFloat(difference) - parseFloat(sc);
            difference = parseFloat(difference) - parseFloat(endBal);

            if (difference == -0) {
                difference = difference * -1;
            }

            if (difference < 0) {
                $("#<%=lblDifference.ClientID%>").parent().css({ color: "red" });

            }
            else {
                $("#<%=lblDifference.ClientID%>").parent().css({ color: "#333" });
            }
            $("#<%=lblDifference.ClientID%>").val(cleanUpCurrency('$' + parseFloat(difference.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $("#<%=hdnDifference.ClientID%>").val(difference.toFixed(3))

            $("#<%=txtEndingBalance.ClientID%>").val(cleanUpCurrency('$' + endBal.toLocaleString("en-US", { minimumFractionDigits: 2 })))
      <%-- if (!isNaN(parseFloat($("#<%=txtEndingBalance.ClientID%>").val().toString().replace(/[\$\(\),]/g, '')))) {

                $("#<%=txtEndingBalance.ClientID%>").val("$" + parseFloat($("#<%=txtEndingBalance.ClientID%>").val().toString().replace(/[\$\(\),]/g, '')).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            }--%>
            if (typeof (Materialize) != 'undefined' && typeof (Materialize.updateTextFields) == 'function') {
                Materialize.updateTextFields();
            }
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

        function DifferenceValidation(sender, args) {
            if ($("#<%=lblDifference.ClientID %>").val() == '') {
                sender.innerHTML = "Your bank reconciliation is off. Please correct any mistakes before proceeding.";
                args.IsValid = false;
            } else if ($("#<%=lblDifference.ClientID %>").val() != '') {
                var diff = ($("#<%=lblDifference.ClientID %>").val())
                if (diff != "$0.00" && diff != "($0.00)") {
                    sender.innerHTML = "Your bank reconciliation is off by $" + diff + ". Please correct any mistakes before proceeding.";
                    args.IsValid = false;
                }
                else {
                    args.IsValid = true;
                }
            }
            else {
                args.IsValid = false;
            }
        }

        function SCDtValidation(sender, args) {
            if (document.getElementById("<%=txtServiceChrgAmount.ClientID%>").value != '') {
                var sc = parseFloat(document.getElementById("<%=txtServiceChrgAmount.ClientID%>").value)
                if (sc != 0) {
                    if (document.getElementById("<%=txtServiceChargeDate.ClientID%>").value == '') {
                        args.IsValid = false;
                    }
                    else {
                        args.IsValid = true;
                    }
                }
                else
                    args.IsValid = true;
            }
            else
                args.IsValid = true;

        }

        function InterestDtValidation(sender, args) {
            if (document.getElementById("<%=txtInterestAmount.ClientID%>").value != '') {
                var int = parseFloat(document.getElementById("<%=txtInterestAmount.ClientID%>").value)
                if (int != 0) {
                    if (document.getElementById("<%=txtInterestDate.ClientID%>").value == '') {
                        args.IsValid = false;
                    }
                    else {
                        args.IsValid = true;
                    }
                }
                else
                    args.IsValid = true;
            }
            else
                args.IsValid = true;
        }

        function SCAcctValidation(sender, args) {

            if (document.getElementById("<%=txtServiceChrgAmount.ClientID%>").value != '') {
                var sc = parseFloat(document.getElementById("<%=txtServiceChrgAmount.ClientID%>").value)
                if (sc != 0) {
                    var hdnSC = document.getElementById('<%= hdnServiceAcct.ClientID %>');
                    if (hdnSC.value == '') {
                        args.IsValid = false;
                    }
                    else if (hdnSC.value == '0') {
                        args.IsValid = false;
                    }
                }
                else
                    args.IsValid = true;
            } else
                args.IsValid = true;
        }

        function IntAcctValidation(sender, args) {

            if (document.getElementById("<%=txtInterestAmount.ClientID%>").value != '') {
                var sc = parseFloat(document.getElementById("<%=txtInterestAmount.ClientID%>").value)
                if (sc != 0) {
                    var hdnInt = document.getElementById('<%= hdnInterestAcct.ClientID %>');
                    if (hdnInt.value == '') {
                        args.IsValid = false;
                    }
                    else if (hdnInt.value == '0') {
                        args.IsValid = false;
                    }
                }
                else
                    args.IsValid = true;
            } else
                args.IsValid = true;
        }

        function displayBankRecon() {
            $("#DivBankRecon").hide();
            $("#BankReconReport").show();

              <%--document.getElementById('<%=lnkExportPdf.ClientID%>').click();--%>
              <%--$('#<%=lnkExportPdf.ClientID%>').click();--%>
              <%--$("#<%=lnkExportPdf.ClientID%>").trigger("click");--%>
            //return false;
        }

        function calculateDepChk() {
            //calculateCheckAmount();
            //calculateDepositAmount();
            calculateDifference();
        }
    </script>
    <script type="text/javascript">
        $(function () {
            ///////////// Ajax call for account auto search ////////////////////     
            addAutoComplete();
        });

        function addAutoComplete() {
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.Acct = null;
            }
            $("#<%=txtServiceAccount.ClientID%>").autocomplete({
                open: function (e, ui) {
                    /* create the scrollbar each time autocomplete menu opens/updates */
                    $(".ui-autocomplete").mCustomScrollbar({

                        setHeight: 182,
                        theme: "dark-3",
                        keyboard: {
                            enable: true,
                            scrollType: "stepless"
                        },
                        autoExpandScrollbar: true
                    });
                },
                response: function (e, ui) {
                    /* destroy the scrollbar after each search completes, before the menu is shown */
                    $(".ui-autocomplete").mCustomScrollbar("destroy");
                },
                source: function (request, response) {



                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetAccountName",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var itemList = $.parseJSON(data.d);
                            //Remove "Add New" item
                            itemList.splice(0, 1);
                            var item;
                            var ddlBank = $("#<%=ddlBank.ClientID%>").val();
                            for (var i = 0; i <= itemList.length - 1; i++) {
                                item = itemList[i];
                                if (item.BankID == ddlBank) {
                                    itemList.splice(i, 1);
                                    break;
                                }
                            }

                            response(itemList);
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load account name");
                        }
                    });
                },
                messages: {
                    noResults: '',
                    results: function () { }
                },
                select: function (event, ui) {

                    $("#<%=txtServiceAccount.ClientID%>").val(ui.item.label);
                    $("#<%=hdnServiceAcct.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtServiceAccount.ClientID%>").val(ui.item.label);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.value;
                    var result_item = item.label;
                    var result_desc = item.acct;


                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }

                    if (result_value == 0 || result_value == ' < Add New > ') {
                        return $("<li></li>")
                            .data("ui-autocomplete-item", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("ui-autocomplete-item", item)
                            .append("<a>" + result_item + ", <span>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }

                };

            $("#<%=txtInterestAccount.ClientID%>").autocomplete({

                open: function (e, ui) {
                    /* create the scrollbar each time autocomplete menu opens/updates */
                    $(".ui-autocomplete").mCustomScrollbar({

                        setHeight: 182,
                        theme: "dark-3",
                        keyboard: {
                            enable: true,
                            scrollType: "stepless"
                        },
                        autoExpandScrollbar: true
                    });
                },
                response: function (e, ui) {
                    /* destroy the scrollbar after each search completes, before the menu is shown */
                    $(".ui-autocomplete").mCustomScrollbar("destroy");
                },

                source: function (request, response) {

                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetAccountName",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var itemList = $.parseJSON(data.d);
                            //Remove "Add New" item                          
                            itemList.splice(0, 1);
                            var item;
                            var ddlBank = $("#<%=ddlBank.ClientID%>").val();
                            for (var i = 0; i <= itemList.length - 1; i++) {
                                item = itemList[i];
                                if (item.BankID == ddlBank) {
                                    itemList.splice(i, 1);
                                    break;
                                }
                            }

                            response(itemList);
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load account name");
                        }
                    });
                },

                messages: {
                    noResults: '',
                    results: function () { }
                },
                select: function (event, ui) {
                    $("#<%=txtInterestAccount.ClientID%>").val(ui.item.label);
                    $("#<%=hdnInterestAcct.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtInterestAccount.ClientID%>").val(ui.item.label);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                var ula = ul;
                var itema = item;
                var result_value = item.value;
                var result_item = item.label;
                var result_desc = item.acct;

                var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                result_item = result_item.replace(x, function (FullMatch, n) {
                    return '<span class="highlight">' + FullMatch + '</span>'
                });
                if (result_desc != null) {
                    result_desc = result_desc.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                }

                if (result_value == 0 || result_value == ' < Add New > ') {
                    return $("<li></li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);
                }
                else {
                    return $("<li></li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a>" + result_item + ", <span>" + result_desc + "</span></a>")
                        .appendTo(ul);
                }

            };
        }

        function gvCheck_RowSelected(sender, args) {
            //calculateCheckAmount();

            var obj = document.getElementById(args.get_id())
            var id = $(obj).closest('tr').find('#hdnID').val();
            var hdnAmount = parseFloat($(obj).closest('tr').find('#hdnAmount').val() * (-1));

            var hdnListCredit = $("#<%=hdnListCredit.ClientID%>").val();
            if (hdnListCredit == "") {
                hdnListCredit = hdnListCredit + id;
            } else {
                hdnListCredit = hdnListCredit + "," + id;
            }
            $("#<%=hdnListCredit.ClientID%>").val(hdnListCredit);

            var item = hdnListCredit.split(",");
            var count = 0;
            for (var i = 0; i < item.length; i++) {
                if (item[i] != "") {
                    count = count + 1
                }
            }
            $("#<%=lblCheckCount.ClientID%>").text(count);

            var chkAmt = parseFloat($("#<%=lblCheckAmount.ClientID%>").text().toString().replace(/[\$\(\),]/g, ''));
            chkAmt = parseFloat(chkAmt) + parseFloat(hdnAmount);

            $("#<%=lblCheckAmount.ClientID%>").text(cleanUpCurrency('$' + parseFloat(chkAmt.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            calculateDifference();
        }

        function gvCheck_RowDeselected(sender, args) {
            //calculateCheckAmount();

            var obj = document.getElementById(args.get_id())
            var id = $(obj).closest('tr').find('#hdnID').val();
            var hdnAmount = parseFloat($(obj).closest('tr').find('#hdnAmount').val() * (-1));

            var hdnListCredit = $("#<%=hdnListCredit.ClientID%>").val();

            hdnListCredit = hdnListCredit.replace(id, '');
            hdnListCredit = hdnListCredit.replace(",,", ',');
            if (hdnListCredit == ",") {
                hdnListCredit = "";
            }


            $("#<%=hdnListCredit.ClientID%>").val(hdnListCredit);
            var item = hdnListCredit.split(",");
            var count = 0;
            for (var i = 0; i < item.length; i++) {
                if (item[i] != "") {
                    count = count + 1
                }
            }
            $("#<%=lblCheckCount.ClientID%>").text(count);
            var chkAmt = parseFloat($("#<%=lblCheckAmount.ClientID%>").text().toString().replace(/[\$\(\),]/g, ''));
            chkAmt = parseFloat(chkAmt) - parseFloat(hdnAmount);

            $("#<%=lblCheckAmount.ClientID%>").text(cleanUpCurrency('$' + parseFloat(chkAmt.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            calculateDifference();
        }

        function gvDeposit_RowSelected(sender, args) {
            //calculateDepositAmount();


            var obj = document.getElementById(args.get_id())
            var id = $(obj).closest('tr').find('#hdnID').val();
            var hdnAmount = parseFloat($(obj).closest('tr').find('#hdnAmount').val());

            var hdnListDebit = $("#<%=hdnListDebit.ClientID%>").val();
            if (hdnListDebit == "") {
                hdnListDebit = hdnListDebit + id;
            } else {
                hdnListDebit = hdnListDebit + "," + id;
            }
            $("#<%=hdnListDebit.ClientID%>").val(hdnListDebit);
            var item = hdnListDebit.split(",");

            var count = 0;
            for (var i = 0; i < item.length; i++) {
                if (item[i] != "") {
                    count = count + 1
                }
            }

            $("#<%=lblDepositCount.ClientID%>").text(count);

            var depositAmt = parseFloat($("#<%=lblDepositAmount.ClientID%>").text().toString().replace(/[\$\(\),]/g, ''));
            depositAmt = parseFloat(depositAmt) + parseFloat(hdnAmount);

            $("#<%=lblDepositAmount.ClientID%>").text(cleanUpCurrency('$' + parseFloat(depositAmt.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            calculateDifference();
        }

        function gvDeposit_RowDeselected(sender, args) {
            //calculateDepositAmount();
            var obj = document.getElementById(args.get_id())
            var id = $(obj).closest('tr').find('#hdnID').val();
            var hdnAmount = parseFloat($(obj).closest('tr').find('#hdnAmount').val());

            var hdnListDebit = $("#<%=hdnListDebit.ClientID%>").val();

            hdnListDebit = hdnListDebit.replace(id, '');
            hdnListDebit = hdnListDebit.replace(",,", ',');
            if (hdnListDebit == ",") {
                hdnListDebit = "";
            }

            $("#<%=hdnListDebit.ClientID%>").val(hdnListDebit);
            var item = hdnListDebit.split(",");
            var count = 0;
            for (var i = 0; i < item.length; i++) {
                if (item[i] != "") {
                    count = count + 1
                }
            }
            $("#<%=lblDepositCount.ClientID%>").text(count);
            var depositAmt = parseFloat($("#<%=lblDepositAmount.ClientID%>").text().toString().replace(/[\$\(\),]/g, ''));
            depositAmt = parseFloat(depositAmt) - parseFloat(hdnAmount);
            $("#<%=lblDepositAmount.ClientID%>").text(cleanUpCurrency('$' + parseFloat(depositAmt.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            calculateDifference();
        }

        function isValidAmount() {
            debugger
            var flag = true;
            var txtServiceChrgAmount = $("#<%=txtServiceChrgAmount.ClientID %>");
            var txtInterestAmount = $("#<%=txtInterestAmount.ClientID %>");

            parseFloat(txtInterestAmount.val().toString().replace(/[\$\(\),]/g, ''));
            if (parseFloat(txtServiceChrgAmount.val().toString().replace(/[\$\(\),]/g, '')) < 0) {
                flag = false;
                noty({ text: 'Service charge amount must be a positive number. Please correct to proceed.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });

            } else {
                if (parseFloat(txtInterestAmount.val().toString().replace(/[\$\(\),]/g, '')) < 0) {
                    noty({ text: 'Interest amount must be a positive number. Please correct to proceed.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    flag = false;
                }
            }
            return flag;
        }
    </script>


    <script type="text/javascript">
        function UpdateLabel() {
            if (typeof (Materialize) != 'undefined' && typeof (Materialize.updateTextFields) == 'function') {
                Materialize.updateTextFields();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-action-account-balance"></i>&nbsp; Bank Reconciliation</div>
                                    <asp:Panel ID="pnlSave" runat="server">
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <asp:LinkButton Text="Save" ID="lnkBtnSave" runat="server" ToolTip="Save" OnClientClick="return isValidAmount();" OnClick="lnkBtnSave_Click" ValidationGroup="bankrecon"
                                                    CausesValidation="false"></asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton Text="Reconcile" ID="lnkBtnRecon" runat="server" ToolTip="Reconcile" OnClick="lnkBtnRecon_Click" ValidationGroup="bankrecon"
                                                    CausesValidation="true" Enabled="true" OnClientClick="return AddReconClick(this)"></asp:LinkButton>

                                            </div>

                                            <div class="btnlinks">
                                                <a class="dropdown-button" id="lnkReport" runat="server" data-beloworigin="true" href="#!" data-activates="dropdown1">Reports
                                                </a>
                                            </div>

                                            <ul id="dropdown1" class="dropdown-content">
                                                <li>
                                                    <asp:LinkButton Text="Bank Rec Progress Report" ID="lnkBankRecProgressReport" runat="server" ToolTip="Bank Rec Progress Report" OnClick="lnkBankRecReport_Click" ></asp:LinkButton>
                                                </li>
                                            </ul>

                                            <div class="btnlinks" id="DivButton">
                                                <asp:LinkButton Text="Reprint Cleared Item List Report" ID="lnkReprint" runat="server" ToolTip="Reprint" OnClick="lnkReprint_Click" Visible="false"></asp:LinkButton>
                                                <asp:TextBox ID="txtRecId" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <div class="btnclosewrap">
                                        <%--<a href="#"><i class="mdi-content-clear"></i></a>--%>
                                        <asp:LinkButton CssClass="mdi-content-clear" ID="lnkBtnClose" runat="server" ToolTip="Close" OnClick="lnkBtnClose_Click"
                                            TabIndex="39"></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <asp:UpdatePanel ID="udpBankName" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="editlabel">
                                                    <asp:Label ID="lblBankName" runat="server"></asp:Label>
                                                </div>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="container" id="DivBankRecon">
                <asp:HiddenField ID="hdnDifference" runat="server" />
                <asp:HiddenField ID="hdnListCredit" runat="server" />
                <asp:HiddenField ID="hdnListDebit" runat="server" />
                <div class="row">
                    <div class="srchpane">
                        <div class="form-section-row">
                            <div class="section-ttle">Bank Details</div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <label class="drpdwn-label">Bank Account</label>
                                        <asp:DropDownList ID="ddlBank" runat="server" CssClass="browser-default" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged" AutoPostBack="true" onchange="resetAccount()"></asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvBank" ControlToValidate="ddlBank"
                                            ErrorMessage="Select Bank account." Display="None" InitialValue="0"
                                            ValidationGroup="bankrecon"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceBank" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvBank" />
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hdnChartID" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="input-field col s12">
                                    <div class="row">
                                        <label for="txtStatementDate">Statement Date</label>
                                        <asp:TextBox ID="txtStatementDate" ClientIDMode="Static" runat="server"
                                            OnTextChanged="txtStatementDate_TextChanged" AutoPostBack="true" autocomplete="off" />
                                        <asp:CalendarExtender ID="txtStatementDate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtStatementDate">
                                        </asp:CalendarExtender>
                                    </div>
                                </div>
                                <div class="input-field col s12">
                                    <div class="row" id="dvCompanyPermission" runat="server">
                                        <label class="drpdwn-label">Company</label>
                                        <asp:DropDownList ID="ddlCompany" runat="server" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="true"
                                            CssClass="browser-default" TabIndex="4">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3-blank">
                                <div class="row">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s5">
                                    <div class="row">
                                        <%--  <%--<span style="float: right !important; font-size: 0.9em; color: #000 !important;">$0.00</span>--%>
                                        <asp:UpdatePanel ID="updVendorBal" runat="server">
                                            <ContentTemplate>
                                                <label for="lblBeginBalance">Begining Balance</label>

                                                <asp:TextBox ReadOnly="true" ID="lblBeginBalance" ClientIDMode="Static" runat="server" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlBank" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                                <div class="input-field col s2">
                                    <div class="row">
                                        &nbsp;
                                    </div>
                                </div>
                                <div class="input-field col s5">
                                    <div class="row">
                                        <%--<span style="float: right !important; font-size: 0.9em; color: #000 !important;">$0.00</span>--%>
                                        <asp:UpdatePanel ID="upDifference" runat="server">
                                            <ContentTemplate>
                                                <label>Difference</label>
                                                <asp:TextBox ReadOnly="true" ID="lblDifference" runat="server"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="row">
                                <div class="input-field col s5">
                                    <div class="row">
                                        <label for="txtServiceChargeDate">Service Charge Date</label>
                                        <%--<asp:TextBox runat="server" ID="txtSCDate" CssClass="datepicker_mom" Enabled="false"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtServiceChargeDate" runat="server" />
                                        <asp:CalendarExtender ID="txtServiceChargeDate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtServiceChargeDate">
                                        </asp:CalendarExtender>
                                        <asp:RegularExpressionValidator ID="revServiceDt" ControlToValidate="txtServiceChargeDate" ValidationGroup="bankrecon"
                                            ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="vcServiceDt" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="revServiceDt" />
                                        <%--<asp:RequiredFieldValidator runat="server" ID="rfvSCDt" ControlToValidate="txtServiceChargeDate"
                                            ErrorMessage="Please enter Service charge date." Display="None"
                                            ValidationGroup="bankrecon"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceSCDt" runat="server" Enabled="True" PopupPosition="BottomRight"
                                            TargetControlID="rfvSCDt" />--%>
                                        <asp:CustomValidator ID="cvSCDt" runat="server" ControlToValidate="txtServiceChargeDate"
                                            ValidateEmptyText="true" ClientValidationFunction="SCDtValidation"
                                            ValidationGroup="bankrecon" ErrorMessage="Please select Service charge date" Display="None">
                                        </asp:CustomValidator>
                                        <%--OnServerValidate="cvSCDt_ServerValidate"--%>
                                        <asp:ValidatorCalloutExtender ID="vceSCDt" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="cvSCDt" />
                                    </div>
                                </div>
                                <div class="input-field col s2">
                                    <div class="row">
                                        &nbsp;
                                    </div>
                                </div>

                                <div class="input-field col s5">
                                    <div class="row">
                                        <label for="txtServiceChrgAmount">Service Charge Amount</label>
                                        <%--<asp:TextBox runat="server" ID="txtSCAmount" Enabled="false"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtServiceChrgAmount" runat="server" onkeypress="return isDecimalKey(this,event);"
                                            onchange="VisibleTxtService(this);" />
                                        <asp:RequiredFieldValidator runat="server" ID="rfvServiceChrgAmount" ControlToValidate="txtServiceChrgAmount"
                                            ErrorMessage="Please enter Service charge amount." Display="None"
                                            ValidationGroup="bankrecon"></asp:RequiredFieldValidator>


                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ValueToCompare="0" ControlToValidate="txtServiceChrgAmount" ValidationGroup="bankrecon"
                                            ErrorMessage="Service charge amount must be a positive number. Please correct to proceed." Operator="GreaterThanEqual" Display="None" Type="Currency"></asp:CompareValidator>
                                        <asp:ValidatorCalloutExtender ID="vceServiceChrgAmount" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                            TargetControlID="rfvServiceChrgAmount" />
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                            TargetControlID="CompareValidator1" />
                                    </div>
                                </div>
                                </div>
                                <div class="row">
                                <div class="input-field col s5">
                                    <div class="row">
                                        <label for="txtInterestDate">Interest Date</label>
                                        <%--<asp:TextBox runat="server" ID="txtIDate" CssClass="datepicker_mom" Enabled="false"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtInterestDate" runat="server" />

                                        <asp:CalendarExtender ID="txtInterestDate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtInterestDate">
                                        </asp:CalendarExtender>

                                        <asp:RegularExpressionValidator ID="revInterestDt" ControlToValidate="txtInterestDate" ValidationGroup="bankrecon"
                                            ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="vceInterestDt" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="revInterestDt" />
                                        <asp:CustomValidator ID="cvInterestDt" runat="server" ValidateEmptyText="true" ControlToValidate="txtInterestDate" ClientValidationFunction="InterestDtValidation"
                                            ValidationGroup="bankrecon" ErrorMessage="Please select Interest date" Display="None"></asp:CustomValidator>
                                        <asp:ValidatorCalloutExtender ID="vceInterestDt1" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="cvInterestDt" />                                       
                                    </div>
                                </div>
                                <div class="input-field col s2">
                                    <div class="row">
                                        &nbsp;
                                    </div>
                                </div>
                                <div class="input-field col s5">
                                    <div class="row">
                                        <label for="txtInterestAmount">Interest Amount</label>
                                        <%--<asp:TextBox runat="server" ID="txtIAmount" Enabled="false"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtInterestAmount" runat="server" onkeypress="return isDecimalKey(this,event);" onchange="VisibleTxtService(this);" />
                                        <asp:RequiredFieldValidator runat="server" ID="rfvInterestAmount" ControlToValidate="txtInterestAmount"
                                            ErrorMessage="Please enter Interest amount." Display="None"
                                            ValidationGroup="bankrecon"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceInterestAmount" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                            TargetControlID="rfvInterestAmount" />
                                        <asp:CompareValidator ID="rfvInterestAmount2" runat="server" ValueToCompare="0" ControlToValidate="txtInterestAmount" Display="None" ValidationGroup="bankrecon"
                                            ErrorMessage="Interest amount must be a positive number. Please correct to proceed." Operator="GreaterThanEqual" Type="Currency"></asp:CompareValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                            TargetControlID="rfvInterestAmount2" />
                                    </div>
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
                                        <%--<asp:TextBox runat="server" ID="txtEnding" Enabled="false"></asp:TextBox>--%>
                                        <asp:UpdatePanel ID="upEndingBalance" runat="server">
                                            <ContentTemplate>
                                                <label for="txtEndingBalance">Ending Balance</label>
                                                <asp:TextBox ID="txtEndingBalance" runat="server"
                                                    onchange="calculateDifference();" autocomplete="off" ValidationGroup="bankrecon" ClientIDMode="Static" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvEndingBalance" ControlToValidate="txtEndingBalance"
                                            ErrorMessage="Please enter Ending Balance." Display="None"
                                            ValidationGroup="bankrecon"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceEndingBalance" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                            TargetControlID="rfvEndingBalance" />
                                        <asp:CustomValidator ID="cvDifference" runat="server" ValidateEmptyText="true"
                                            ClientValidationFunction="DifferenceValidation"
                                            ValidationGroup="bankrecon" ErrorMessage="Your bank reconciliation is off. Please correct any mistakes before proceeding."
                                            Display="None"></asp:CustomValidator>
                                        <asp:ValidatorCalloutExtender ID="vceDifference" runat="server" Enabled="True" PopupPosition="BottomRight"
                                            TargetControlID="cvDifference" />
                                    </div>
                                </div>

                                <div class="input-field col s12">
                                    <div class="row">
                                        <label for="txtServiceAccount">Service Charge Account</label>
                                        <%--<asp:TextBox runat="server" ID="txtCharge"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtServiceAccount" runat="server" autocomplete="off" />
                                        <asp:HiddenField ID="hdnServiceAcct" runat="server" />
                                        <asp:CustomValidator ID="cvSCAcct" ValidateEmptyText="true" runat="server" ControlToValidate="txtServiceAccount" ValidationGroup="bankrecon"
                                            ErrorMessage="Please select Service charge account" ClientValidationFunction="SCAcctValidation"
                                            Display="None"></asp:CustomValidator>
                                        <asp:ValidatorCalloutExtender ID="vceSCAcct" runat="server" Enabled="True" PopupPosition="BottomRight"
                                            TargetControlID="cvSCAcct">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                </div>

                                <div class="input-field col s12">
                                    <div class="row">
                                        <label for="txtInterest">Interest Account</label>
                                        <%--<asp:TextBox runat="server" ID="txtInterest"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtInterestAccount" runat="server" />
                                        <asp:HiddenField ID="hdnInterestAcct" runat="server" />
                                        <asp:CustomValidator ID="cvIntAcct" ValidateEmptyText="true" runat="server" ControlToValidate="txtInterestAccount" ValidationGroup="bankrecon"
                                            ErrorMessage="Please select Interest account" ClientValidationFunction="IntAcctValidation"
                                            Display="None"></asp:CustomValidator>
                                        <asp:ValidatorCalloutExtender ID="vceIntAcct" runat="server" Enabled="True" PopupPosition="BottomRight"
                                            TargetControlID="cvIntAcct">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-section-row">
                            <div class="section-ttle">Items you have marked as cleared:</div>
                            <div class="form-section3">
                                <div class="text-field col s12">
                                    <div class="row">
                                        <%--<label style="font-size: 0.9em; color: #222; font-weight: bold;">0 Checks and Payments</label>
                                        <span style="float: right !important; font-size: 0.9em; color: #000 !important;">$0.00</span>--%>

                                        <asp:Label ID="lblCheckCount" runat="server" class="lable-for"  Text="0"></asp:Label>
                                        <label class="lable-for" >Checks and Payments </label>
                                        <asp:Label class="lable-for1" ID="lblCheckAmount" runat="server"  Text="0.00"></asp:Label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-section3-blank">
                                <div class="row">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="form-section3">
                                <div class="text-field col s12">
                                    <div class="row">
                                        <%--<label style="font-size: 0.9em; color: #222; font-weight: bold;">0 Deposits and Other Credits</label>--%>
                                        <asp:Label ID="lblDepositCount" class="lable-for" runat="server"  Text="0"></asp:Label>
                                        <label class="lable-for" >Deposits and Other Credits</label>

                                        <%--<span style="float: right !important; font-size: 0.9em; color: #000 !important;">$0.00</span>--%>
                                        <asp:Label ID="lblDepositAmount" class="lable-for1" runat="server"  Text="0.00"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%--Main grid--%>
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="ddlBank">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="gvCheck" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="gvDeposit" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="hdnListCredit" />
                                    <telerik:AjaxUpdatedControl ControlID="hdnListDebit" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="gvDeposit">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="gvDeposit" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="hdnListCredit" />
                                    <telerik:AjaxUpdatedControl ControlID="hdnListDebit" />

                                </UpdatedControls>
                            </telerik:AjaxSetting>

                            <telerik:AjaxSetting AjaxControlID="gvCheck">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="gvCheck" LoadingPanelID="RadAjaxLoadingPanel1" />
                                    <telerik:AjaxUpdatedControl ControlID="hdnListCredit" />
                                    <telerik:AjaxUpdatedControl ControlID="hdnListDebit" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                            <script type="text/javascript">
                               <%-- function pageLoad() {
                                    var grid = $find("<%= gvDeposit.ClientID %>");
                                    var columns = grid.get_masterTableView().get_columns();
                                    for (var i = 0; i < columns.length; i++) {
                                        columns[i].resizeToFit(false, true);
                                    }
                                }--%>

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
                                <%--  function scrollItemToTop(itemID) {
                                    console.log($telerik.$($get(itemID)).offset().top);
                                    var grid = $find("<%= gvDeposit.ClientID %>");
                                    grid.scrollTo(0, $telerik.$($get(itemID)).offset().top);
                                }--%>
                            </script>

                        </telerik:RadCodeBlock>

                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadPersistenceManager ID="RadPersistenceManager1" runat="server">
                                <PersistenceSettings>
                                    <telerik:PersistenceSetting ControlID="gvCheck" />
                                    <telerik:PersistenceSetting ControlID="gvDeposit" />
                                </PersistenceSettings>
                            </telerik:RadPersistenceManager>
                            <div class="form-section2">
                                <div class="section-ttle">Open Checks/Credits</div>
                                <div class="grid_container">
                                    <div class="form-section-row pmd-card">
                                        <div class="RadGrid RadGrid_Material">

                                            <telerik:RadGrid RenderMode="Auto" ID="gvCheck" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" AllowMultiRowSelection="true"
                                                ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                                OnNeedDataSource="gvCheck_NeedDataSource"
                                                OnPreRender="gvCheck_PreRender"
                                                OnItemCreated="gvCheck_ItemCreated">
                                                <CommandItemStyle />
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="true"></Selecting>
                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                    <ClientEvents OnRowSelected="gvCheck_RowSelected" OnRowDeselected="gvCheck_RowDeselected" />
                                                </ClientSettings>
                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                    <Columns>
                                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Display="false">
                                                            <ItemTemplate>
                                                                <%--<asp:CheckBox ID="chkSelect" ClientIDMode="Static" runat="server" Checked='<%# Eval("Status").Equals("T")? true : false %>' />--%>
                                                                <asp:HiddenField ID="hdnStatus" ClientIDMode="Static" Value='<%# Bind("Status") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnID" ClientIDMode="Static" Value='<%# Bind("ID") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnAmount" ClientIDMode="Static" Value='<%# Bind("Amount") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnBatch" ClientIDMode="Static" Value='<%# Bind("Batch") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnTypeNum" ClientIDMode="Static" Value='<%# Bind("TypeNum") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                                        </telerik:GridClientSelectColumn>

                                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="Ref" HeaderText="Ref" SortExpression="Ref" DataType="System.String"
                                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false" HeaderStyle-Width="60">
                                                            <ItemTemplate>
                                                                <%--<asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>'></asp:Label>--%>
                                                                <asp:HyperLink ID="hlRef" runat="server" Text='<%# Bind("Ref") %>' Target="_blank"
                                                                    NavigateUrl='<%# Eval("Url").ToString()+ "&page=bankrecon" %>' ForeColor="#0066CC"></asp:HyperLink>
                                                                <asp:Label ID="lblRef" runat="server" Font-Size="Small" Text='<%# Bind("Ref")%>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                            </FooterTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="fDate" HeaderText="Date" SortExpression="fDate" DataType="System.String"
                                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false" HeaderStyle-Width="90">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfDate" runat="server" Text='<%# Bind("fDate","{0:MM/dd/yyyy}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <%--Description--%>
                                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="fDesc" HeaderText="Description" SortExpression="fDesc" DataType="System.String"
                                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false"  HeaderStyle-Width="260">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfDesc" runat="server" Text='<%# Eval("fDesc") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <%--Type--%>
                                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="TypeName" HeaderText="Type" SortExpression="Quan" DataType="System.String"
                                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false"  HeaderStyle-Width="70">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblType1" runat="server" Text='<%# Eval("TypeName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <%--Amount--%>

                                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="Amt" SortExpression="Amt" HeaderStyle-Width="80" AutoPostBackOnFilter="false" CurrentFilterFunction="EqualTo" HeaderText="Amount" ShowFilterIcon="false" FooterAggregateFormatString="{0:c}" Aggregate="Sum">
                                                            <ItemTemplate>
                                                                <%--<asp:Label ID="lblAmount" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>'></asp:Label>--%>
                                                                <asp:Label ID="lblAmount" runat="server" Font-Size="Small" Text='<%# DataBinder.Eval(Container.DataItem, "Amt", "{0:c}")%>'
                                                                    ForeColor='<%# Convert.ToDouble(Eval("Amt"))<0?System.Drawing.Color.Red: System.Drawing.Color.Black %>'></asp:Label>
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
                            </div>
                            <div class="form-section3-blank">
                                <div class="row">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="form-section2">
                                <div class="section-ttle">Open Deposits/Debits</div>
                                <div class="grid_container">
                                    <div class="form-section-row pmd-card" >
                                        <div class="RadGrid RadGrid_Material">

                                            <telerik:RadGrid RenderMode="Auto" ID="gvDeposit" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" AllowMultiRowSelection="true"
                                                ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                                OnItemCreated="gvDeposit_ItemCreated"
                                                OnPreRender="gvDeposit_PreRender"
                                                OnNeedDataSource="gvDeposit_NeedDataSource">
                                                <CommandItemStyle />
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="true"></Selecting>
                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                    <ClientEvents OnRowSelected="gvDeposit_RowSelected" OnRowDeselected="gvDeposit_RowDeselected" />
                                                </ClientSettings>

                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                    <Columns>
                                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" Display="false">
                                                            <ItemTemplate>
                                                                <%--<asp:CheckBox ID="chkSelect" ClientIDMode="Static" runat="server" Checked='<%# Eval("Status").Equals("T")? true : false %>' />--%>
                                                                <asp:HiddenField ID="hdnStatus" ClientIDMode="Static" Value='<%# Bind("Status") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnID" ClientIDMode="Static" Value='<%# Bind("ID") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnBatch" ClientIDMode="Static" Value='<%# Bind("Batch") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnTypeNum" ClientIDMode="Static" Value='<%# Bind("TypeNum") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnAmount" ClientIDMode="Static" Value='<%# Bind("Amount") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                                        </telerik:GridClientSelectColumn>
                                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="Ref" HeaderText="Ref" SortExpression="Ref" DataType="System.String"
                                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false">
                                                            <ItemTemplate>
                                                                <%--<asp:Label ID="lblId" runat="server" Text='<%# Eval("ID") %>'></asp:Label>--%>
                                                                <asp:HyperLink ID="hlRef" runat="server" Text='<%# Bind("Ref") %>' Target="_blank"
                                                                    NavigateUrl='<%# Eval("Url").ToString()+ "&page=bankrecon" %>' ForeColor="#0066CC"></asp:HyperLink>
                                                                <asp:Label ID="lblRef" runat="server" Font-Size="Small" Text='<%# Bind("Ref")%>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                            </FooterTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="fDate" HeaderText="Date" SortExpression="fDate" DataType="System.String"
                                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false"  HeaderStyle-Width="90">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfDate" runat="server" Text='<%# Bind("fDate","{0:MM/dd/yyyy}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <%--Description--%>
                                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="fDesc" HeaderText="Description" SortExpression="fDesc" DataType="System.String"
                                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false"  HeaderStyle-Width="260">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfDesc" runat="server" Text='<%# Eval("fDesc") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>


                                                        <%--Type--%>
                                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="TypeName" HeaderText="Type" SortExpression="Quan" DataType="System.String"
                                                            AutoPostBackOnFilter="false" CurrentFilterFunction="Contains"
                                                            ShowFilterIcon="false"  HeaderStyle-Width="70">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("TypeName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <%--Amount--%>

                                                        <telerik:GridTemplateColumn FilterDelay="5" DataField="Amt" SortExpression="Amt" HeaderStyle-Width="80" AutoPostBackOnFilter="false" CurrentFilterFunction="EqualTo" HeaderText="Amount" ShowFilterIcon="false" FooterAggregateFormatString="{0:c}" Aggregate="Sum">
                                                            <ItemTemplate>
                                                                <%--<asp:Label ID="lblAmount" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>'></asp:Label>--%>
                                                                <asp:Label ID="lblAmount" runat="server" Font-Size="Small" Text='<%# DataBinder.Eval(Container.DataItem, "Amt", "{0:c}")%>'
                                                                    ForeColor='<%# Convert.ToDouble(Eval("Amt"))<0?System.Drawing.Color.Red: System.Drawing.Color.Black %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>

                                                </MasterTableView>
                                                <FilterMenu CssClass="RadFilterMenu_DepositList">
                                                </FilterMenu>

                                            </telerik:RadGrid>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </telerik:RadAjaxPanel>

                        <%--End main grid--%>

                        <%--grid test--%>

                        <%--End grid test--%>
                        <div class="col-sm-12 col-md-12">
                            <div class="com-cont">
                                <div class="clearfix"></div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="BankReconReport" style="display: none;" class="container">
        <div class="com-cont">
            <rsweb:ReportViewer ID="rvBankRecon" runat="server" Width="800px" Height="1500px" BackColor="White"
                BorderColor="Gray" BorderStyle="None" BorderWidth="1px" ShowPageNavigationControls="true" PageCountMode="Actual"
                AsyncRendering="false" ShowZoomControl="False">
            </rsweb:ReportViewer>
            <div class="clearfix"></div>
        </div>
        <div class="clearfix"></div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {


            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

        });
        function resetAccount() {
            $("#<%=txtServiceAccount.ClientID%>").val('');
            $("#<%=hdnServiceAcct.ClientID%>").val('0');
            $("#<%=txtInterestAccount.ClientID%>").val('');
            $("#<%=hdnInterestAcct.ClientID%>").val('0');

        }
    </script>
</asp:Content>
