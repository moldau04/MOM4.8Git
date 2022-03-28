<%@ Page Title="Write Checks || MOM" Language="C#" MasterPageFile="~/Mom.master" EnableEventValidation="false" ValidateRequest="false" AutoEventWireup="true" Inherits="WriteChecks" Codebehind="WriteChecks.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">


    <script type="text/javascript">
        var a = ['', 'One ', 'Two ', 'Three ', 'Four ', 'Five ', 'Six ', 'Seven ', 'Eight ', 'Nine ', 'Ten ', 'Eleven ', 'Twelve ', 'Thirteen ', 'Fourteen ', 'Fifteen ', 'Sixteen ', 'Seventeen ', 'Eighteen ', 'Nineteen '];
        var b = ['', '', 'Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];

        function inWords(num) {
            if ((num = num.toString()).length > 9) return 'overflow';
            n = ('000000000' + num).substr(-9).match(/^(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
            if (!n) return; var str = '';
            str += (n[1] != 0) ? (a[Number(n[1])] || b[n[1][0]] + ' ' + a[n[1][1]]) + 'crore ' : '';
            str += (n[2] != 0) ? (a[Number(n[2])] || b[n[2][0]] + ' ' + a[n[2][1]]) + 'lakh ' : '';
            str += (n[3] != 0) ? (a[Number(n[3])] || b[n[3][0]] + ' ' + a[n[3][1]]) + 'thousand ' : '';
            str += (n[4] != 0) ? (a[Number(n[4])] || b[n[4][0]] + ' ' + a[n[4][1]]) + 'hundred ' : '';
            str += (n[5] != 0) ? ((str != '') ? 'and ' : '') + (a[Number(n[5])] || b[n[5][0]] + ' ' + a[n[5][1]]) + '' : '';
            return str;
        }
        function KeyPressed(sender, eventArgs) {
            //debugger
            if (eventArgs.get_keyCode() == 40) {
                document.getElementById('<%=btnAddNewLines.ClientID%>').click();
                return false;
            }
        }
        $(window.document).keydown(function (event) {
            if (event.which == 117) {
                document.getElementById('<%=btnCopyPrevious.ClientID%>').click();
                return false;
            }
        })
        function resetIndexF6() {
            //debugger
            var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
            $(hdnSelectPOIndex).val(-1);
        }
        (function ($) {
            $.extend({
                toDictionary: function (query) {
                    var parms = {};
                    var items = query.split("&");
                    for (var i = 0; i < items.length; i++) {
                        var values = items[i].split("=");
                        var key1 = decodeURIComponent(values.shift().replace(/\+/g, '%20'));
                        var key = key1.split('$')[key1.split('$').length - 1];
                        var value = values.join("=")
                        parms[key] = decodeURIComponent(value.replace(/\+/g, '%20'));
                    }
                    return (parms);
                }
            })
        })(jQuery);
        (function ($) {
            $.fn.serializeFormJSON = function () {
                var o = [];
                $(this).find('tr:not(:first, :last)').each(function () {
                    var elements = $(this).find('input, textarea, select')
                    if (elements.size() > 0) {
                        var serialized = $(this).find('input, textarea, select').serialize();
                        var item = $.toDictionary(serialized);
                        o.push(item);
                    }
                });
                return o;
            };
        })(jQuery);
        function itemJSON() {

            var rawData = $('#<%=RadGrid_gvJobCostItems.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            $('#<%=hdnGLItem.ClientID%>').val(formData);
        }
        function disablebuttons() {

            itemJSON();
            var isValid = true;
            <%--if (isValid) {
                if ($('#<%=lblTotalAmount.ClientID%>').text() != '') {
                    var tAmt = parseFloat($('#<%=lblTotalAmount.ClientID%>').text().replace(/[\$\(\),]/g, ''));
                    if (parseFloat(tAmt) > 0) {
                        itemJSON();
                        return true;
                    }
                    else {
                        noty({
                            text: 'Check amount not acceptable, the check generation process cannot continue.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 15000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                        return false;
                    }
                }
                else {
                    noty({
                        text: 'Check amount not acceptable, the check generation process cannot continue.',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 15000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                    return false;
                }
            }
            else {
                return isValid;
            }--%>
            return isValid;
        }
        function ChkVendor(source, arguments) {
            var bill = GetParameterValues('bill');
            //alert(bill);
            if (bill == "c") {
                //alert(bill);
                arguments.IsValid = arguments.Value != '';

            }
            else {
                arguments.IsValid = true;
            }
        }
        function InitializeGrids(Gridview) {

            $("#" + Gridview).on('click', 'a.addButton', function () {
                var $tr = $(this).closest('table').find('tr').eq(1);
                var $clone = $tr.clone();
                $clone.find('input:text').val('');
                $clone.insertAfter($tr.closest('table').find('tr').eq($tr.closest('table').find('tr').length - 2));
            });

            var rowone = $("#" + Gridview).find('tr').eq(1);
            $("input", rowone).each(function () {
                $(this).blur();
            });
        }
         ////////// This function will be called during delete operation to call two functions ///////////
        function CallCalAmountTax() {
            CalculateTotalAmt();
            CalculateTotalUseTaxExpense();
        }
        /////////////////// To calculate Total and to make Gridview Amount Value to 2 decimal ////////////

        function CalculateTotal(obj) {
            try {
                var masterTable = $find("<%=RadGrid_gvJobCostItems.ClientID%>").get_masterTableView();
                var count = masterTable.get_dataItems().length;
                var item;
                for (var i = 0; i < count; i++) {
                    item = masterTable.get_dataItems()[i];
                    var Qty = item.findElement("txtGvQuan");
                    var Amount = item.findElement("txtGvAmount");
                    var Price = item.findElement("txtGvPrice");
   //                 var Price = item.findElement("hdnchkTaxable");

   //                 if(checkbox.checked == true){
   //     $(hdnchkTaxable).val('1');
   // }else{
   //     $(hdnchkTaxable).val('0');
   //}

                    var QtyVal = $(Qty).val();
                    var AmountVal = $(Amount).val();
                    if (QtyVal != "" && AmountVal != "") {
                        if (!isNaN(parseFloat(QtyVal)) && !isNaN(parseFloat(AmountVal)) && parseFloat(QtyVal) != 0) {
                            var QtyPrice = parseFloat(AmountVal) / parseFloat(QtyVal);
                            $(Price).val(QtyPrice.toFixed(2));
                        } else {
                            $(Price).val("");
                        }
                    }
                }
            } catch (e) {

            }
            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
                CalTotalVal(obj);
            }
            else {
                CalTotalVal(obj);
            }

            CalculateTotalAmt();
            CalculateTotalUseTaxExpense();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        function CalculateUseTaxTotal(obj) {

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(4);
            }
            CalculateTotalUseTaxExpense();
        }
        function CalculateTotalUseTaxExpense() {

            var tAmount = 0.00;
            var totalTax = 0.00;
            $("[id*=txtGvUseTax]").each(function () {

                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {

                        totalTax = totalTax + parseFloat($(this).val());
                        var totalAmount = jQuery(this).parent().parent().find('.clsAmount').val();
                        if (totalAmount != null && totalAmount != "") {
                            tAmount = tAmount + parseFloat(totalAmount) * parseFloat($(this).val()) / 100;
                        }
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });

            $('#<%=lblTotalUseTax.ClientID%>').text(tAmount.toFixed(2));
        }


        function CalculateTotalAmt() {
            debugger;
            var tAmount = 0.00;
            $("[id*=txtGvAmount]").each(function () {

                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {

                        var totalAmount = jQuery(this).parent().parent().find('.clsAmount').val();
                        if (totalAmount != null && totalAmount != "") {
                            tAmount = tAmount + parseFloat($(this).val());
                        }
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            var tAmt = 0.00;
            //if ($('#<%=lblTotalAmount.ClientID%>').text() != '')
            //{
                
            CalculatePayTotal();
            CalculatePayTotalSelected(); 
            tAmt = parseFloat($('#<%=lblTotalAmount11.ClientID%>').text().replace(/[\$\(\),]/g, ''));
            //}
            
            <%--$('#<%=lblTotalAmount.ClientID%>').text(tAmount.toFixed(2));            
            $('#<%=lblTotalAmount11.ClientID%>').text(tAmount.toFixed(2));            
            $('[id*=lblTotalAmt]').text(tAmount.toFixed(2));--%>
            //$('[id*=lblAmountPerTotalGrid]').text(tAmount.toFixed(2));
            $("#<%=lblTotalAmount11.ClientID%>").html(cleanUpCurrency("$" + parseFloat(parseFloat(tAmount)).toLocaleString("en-US", { minimumFractionDigits: 2 })));    

            $('#<%=lblTotalAmount.ClientID%>').text((parseFloat(tAmt) + parseFloat(tAmount)).toFixed(2)); 
            <%--$('#<%=lblTotalAmount11.ClientID%>').text((parseFloat(tAmt) + parseFloat(tAmount)).toFixed(2));--%>
            $("#<%=lblTotalAmount11.ClientID%>").html(cleanUpCurrency("$" + parseFloat((parseFloat(tAmt) + parseFloat(tAmount))).toLocaleString("en-US", { minimumFractionDigits: 2 })));    
            $('[id*=lblTotalAmt]').text((parseFloat(tAmt) + parseFloat(tAmount)).toFixed(2));

            var totalQty = 0.00;
            $("[id*=txtGvQuan]").each(function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totalQty = totalQty + parseFloat($(this).val());
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            $('[id*=lblTotalQty]').text(totalQty.toFixed(2));


             var tAmountstax = 0.00;
            
            $("[id*=txtGvStaxAmount]").each(function () {

                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {

                        //var totalAmount = jQuery(this).parent().parent().find('.clsAmount').val();
                        //if (totalAmount != null && totalAmount != "") {
                            tAmountstax = tAmountstax + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                        //}
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });

            
            $('[id*=lblSalesTaxTotal]').text(tAmountstax.toFixed(2));

            var tAmountgsttax = 0.00;
            $("[id*=lblGstTax]").each(function () {

                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {

                        //var totalAmount = jQuery(this).parent().parent().find('.clsAmount').val();
                        //if (totalAmount != null && totalAmount != "") {
                            tAmountgsttax = tAmountgsttax + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                        //}
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });

            
            $('[id*=lblGstTaxTotal]').text(tAmountgsttax.toFixed(2));

            var tAmountamttax = 0.00;

             //tAmountamttax = parseFloat(tAmountstax.toFixed(2)) + parseFloat(tAmountgsttax.toFixed(2));
            var totactualamt = 0.00;
            //totactualamt = parseFloat($('[id*=lblAmountPerTotalGrid]').text());  
            totactualamt = parseFloat(tAmount);  
            
            var totsaletaxamt = 0.00;
            totsaletaxamt = parseFloat($('[id*=lblSalesTaxTotal]').text());
            var totGSTtaxamt = 0.00;
            totGSTtaxamt = parseFloat($('[id*=lblGstTaxTotal]').text());

            
            if (isNaN(totsaletaxamt)) {
                
                totsaletaxamt = 0.00;
            }
            if (isNaN(totGSTtaxamt)) {
                
                totGSTtaxamt = 0.00;
            }

            tAmountamttax = totactualamt+totsaletaxamt+totGSTtaxamt;
            //$("[id*=lblAmountWithTax]").each(function () {

            //    if (!jQuery.trim($(this).text()) == '') {
            //        if (!isNaN(parseFloat($(this).text()))) {

            //            //var totalAmount = jQuery(this).parent().parent().find('.clsAmount').val();
            //            //if (totalAmount != null && totalAmount != "") {
            //                tAmountamttax = tAmountamttax + parseFloat($(this).text().replace(/[\$\(\),]/g, ''));
            //            //}
            //        } else
            //            $(this).text('');
            //    }
            //    else {
            //        $(this).text('');
            //    }
            //});
            
            
            $('[id*=lblAmountWithTaxTotal]').text(tAmountamttax.toFixed(2));
            $('#<%=lblTotalAmount11.ClientID%>').text(tAmountamttax.toFixed(2));
            CalculateTotalUseTaxExpense();





            var _currencyInWord = inWords(parseFloat(Math.trunc(parseFloat(tAmountamttax) )));
            var d = parseFloat(tAmountamttax) - Math.trunc(parseFloat(tAmountamttax));
            if (d > 0) {
                d = Math.round(d * 100);
                _currencyInWord = _currencyInWord + " And " + d + " / 100";
            }
            _currencyInWord = "*** " + _currencyInWord + "****************";
             $("#<%=lblDollar.ClientID%>").html(_currencyInWord);            
            $("#<%=hdnTPay.ClientID%>").val(parseFloat(tAmountamttax).toString());


            
            <%--var _currencyInWord = inWords(parseFloat(Math.trunc(parseFloat(tAmt) + parseFloat(tAmount))));
            var d = parseFloat(tAmt) + parseFloat(tAmount) - Math.trunc(parseFloat(tAmt) + parseFloat(tAmount));
            if (d > 0) {
                d = Math.round(d * 100);
                _currencyInWord = _currencyInWord + " And " + d + " / 100";
            }
            _currencyInWord = "*** " + _currencyInWord + "****************";
             $("#<%=lblDollar.ClientID%>").html(_currencyInWord);            
            $("#<%=hdnTPay.ClientID%>").val(parseFloat(tAmt) + parseFloat(tAmount).toString());--%>

           
           
            CalculatePayTotalSelected(); 
        }


        
        $("#<%=txtVendor.ClientID%>").keyup(function (event) {

                    var hdnVendorID = document.getElementById('<%=hdnVendorID.ClientID%>');
                    if (document.getElementById('<%=txtVendor.ClientID%>').value == '') {
                        hdnVendorID.value = '';
                    }
                });
         //////////////////////To make row's textbox visible///////////////////////////////////////////
        function VisibleRows(row, txt, gridview, event) {
            var rowst = document.getElementById(row)

            var grid = document.getElementById(gridview);
            $('#' + gridview + ' input:text.non-trans').each(function () {
                $(this).removeClass("non-trans");
                $(this).addClass("texttransparent");
            });
            $('#' + gridview + ' select.non-trans').each(function () {
                $(this).removeClass("non-trans");
                $(this).addClass("texttransparent");
            });

            var txtGvAcctNo = document.getElementById(txt);
            $(txtGvAcctNo).removeClass("texttransparent");
            $(txtGvAcctNo).addClass("non-trans");

            var txtGvDesc = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvDesc'));
            $(txtGvDesc).removeClass("texttransparent");
            $(txtGvDesc).addClass("non-trans");

            var txtGvAmount = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvAmount'));
            $(txtGvAmount).removeClass("texttransparent");
            $(txtGvAmount).addClass("non-trans");

            var txtGvUseTax = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvUseTax'));
            $(txtGvUseTax).removeClass("texttransparent");
            $(txtGvUseTax).addClass("non-trans");

            var txtGvLoc = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvLoc'));
            $(txtGvLoc).removeClass("texttransparent");
            $(txtGvLoc).addClass("non-trans");

            var txtGvJob = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvJob'));
            $(txtGvJob).removeClass("texttransparent");
            $(txtGvJob).addClass("non-trans");

            var txtGvPhase = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvPhase'));
            $(txtGvPhase).removeClass("texttransparent");
            $(txtGvPhase).addClass("non-trans");

            var txtGvItem = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvItem'));
            $(txtGvItem).removeClass("texttransparent");
            $(txtGvItem).addClass("non-trans");

        }
        /////////////////////////////////////////////////////////////////////////////
        function lblselectp() {
            
            $("#<%=lblSelectedPayment.ClientID%>").text($("#<%=lblRunBalance.ClientID%>").text());
        }
        function OnGridCommand(sender, args) {
            if (args.get_commandName() == "Page") {
            <% CheckAllCheckbox(); %>

                

            }
            else if (args.get_commandName() == "PageSize") {
            <% CheckAllCheckbox(); %>
            }
        }
        function ddlVendoronchange(value) {
            
            if (value == "-1") {
                $("#<%=lblSelectedPayment.ClientID%>").text($("#<%=lblRunBalance.ClientID%>").text());
                $("#<%=lblSelectedVendorCount.ClientID%>").text($("#<%=lblTotalVendorCount.ClientID%>").text());

                $("#<%=lblVendorBal.ClientID%>").text("$0.00");
            }
            else if (value == "0"){
                $("#<%=lblSelectedVendorCount.ClientID%>").text("0");
                $("#<%=lblVendorBal.ClientID%>").text("$0.00");
            }
            else {
                $("#<%=lblSelectedVendorCount.ClientID%>").text("1");
            }
        }
        function CalculatePayTotal() {
            var tPay = 0;
            var tDisc = 0; 
            var tBal = 0;
            $("#<%=gvBills.ClientID %>").find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);

                if ($tr.find('input[id*=txtGvPay]').attr('id') != "" && typeof $tr.find('input[id*=txtGvPay]').attr('id') != 'undefined') {
                    var payment = $tr.find('input[id*=txtGvPay]').val().replace(/[\$\(\),]/g, '');

                    if (!isNaN(parseFloat(payment))) {
                        tPay += parseFloat(payment);
                    }
                }


                //if ($tr.find('input[id*=txtGvDisc]').attr('id') != "" && typeof $tr.find('input[id*=txtGvDisc]').attr('id') != 'undefined') {
                  //  var disc = $tr.find('input[id*=txtGvDisc]').val().replace(/[\$\(\),]/g, '');

//                    if (!isNaN(parseFloat(disc))) {
  //                      tDisc += parseFloat(disc);
    //                }
      //          }

                if ($tr.find('[id*=lblBalance]').attr('id') != "" && typeof $tr.find('[id*=lblBalance]').attr('id') != 'undefined') {
                    //var bal = $tr.find('[id*=lblBalance]').text().replace(/[\$\(\),]/g, '');
                    var bal = $tr.find('[id*=lblBalance]').text().replace(/[\$\,]/g, '');
                    if (bal.includes('(')) { 
                        bal = bal.replace(/[\$\(\),]/g, '');
                        bal = -bal;
                    }
                    
                   if (!isNaN(parseFloat(bal))) {
                        tBal += parseFloat(bal);
                    }
                }
               
                

//////////////

if ($tr.find('[id*=chkSelect]').attr('id') != "" && typeof $tr.find('[id*=chkSelect]').attr('id') != 'undefined') 
{
if ($tr.find('[id*=chkSelect]').prop('checked') == true) 
{
if ($tr.find('input[id*=txtGvDisc]').attr('id') != "" && typeof $tr.find('input[id*=txtGvDisc]').attr('id') != 'undefined') 
{
                    var disc = $tr.find('input[id*=txtGvDisc]').val().replace(/[\$\(\),]/g, '');

                    if (!isNaN(parseFloat(disc))) {
                        tDisc += parseFloat(disc);
}
                    }
                }
}

                    
//////////////



            });
            var _currencyInWord = inWords(parseFloat(Math.trunc(tPay)));
            var d = tPay - Math.trunc(tPay);
            if (d > 0) {
                d = Math.round(d * 100);
                _currencyInWord = _currencyInWord + " And " + d + " / 100";
            }
            _currencyInWord = "*** " + _currencyInWord + "****************";
            $("#<%=lblSelectedPayment.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            <%--$("#<%=lblRunBalance.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));--%>
            $("#<%=lblRequirement.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $("#<%=lblTotalAmount.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $("#<%=lblDollar.ClientID%>").html(_currencyInWord);
            $("#<%=hdnTPay.ClientID%>").val(tPay.toString());
            $('.cls-payment').html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $("#<%=lblTotalDiscount.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tDisc).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('.cls-disc').html(cleanUpCurrency("$" + parseFloat(tDisc).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            
            <%--$("#<%=lblTotalAmount11.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));    --%>
            $('.cls-bal').html(cleanUpCurrency("$" + parseFloat(tBal).toLocaleString("en-US", { minimumFractionDigits: 2 })));
<% GetPaymentTotal();%>
//var vdis = $("#<%=lblTotalDiscount.ClientID%>").html().replace(/[\$\(\),]/g, '');
//$('.cls-disc').html(cleanUpCurrency("$" + parseFloat(vdis).toLocaleString("en-US", { minimumFractionDigits: 2 })));
        }

        function CalculatePayTotalSelected() {
            var tPay = 0;
            var tDisc = 0;
            var tvendors = 0;
            var titems = 0;
            var tBal = 0;
            $("#<%=gvBills.ClientID %>").find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);

                if ($tr.find('input[id*=txtGvPay]').attr('id') != "" && typeof $tr.find('input[id*=txtGvPay]').attr('id') != 'undefined') {
                    var payment = $tr.find('input[id*=txtGvPay]').val().replace(/[\$\(\),]/g, '');

                    if (!isNaN(parseFloat(payment))) {
                        tPay += parseFloat(payment);
                    }
                }
                //if ($tr.find('input[id*=txtGvDisc]').attr('id') != "" && typeof $tr.find('input[id*=txtGvDisc]').attr('id') != 'undefined') {
                  //  var disc = $tr.find('input[id*=txtGvDisc]').val().replace(/[\$\(\),]/g, '');

//                    if (!isNaN(parseFloat(disc))) {
  //                      tDisc += parseFloat(disc);
    //                }
      //          }

                if ($tr.find('[id*=lblBalance]').attr('id') != "" && typeof $tr.find('[id*=lblBalance]').attr('id') != 'undefined') {
                   // var bal = $tr.find('[id*=lblBalance]').text().replace(/[\$\(\),]/g, '');
                    var bal = $tr.find('[id*=lblBalance]').text().replace(/[\$\,]/g, '');
                    if (bal.includes('(')) { 
                        bal = bal.replace(/[\$\(\),]/g, '');
                        bal = -bal;
                    }
                    if (!isNaN(parseFloat(bal))) {
                        tBal += parseFloat(bal);
                    }
                }


//////////////

if ($tr.find('[id*=chkSelect]').attr('id') != "" && typeof $tr.find('[id*=chkSelect]').attr('id') != 'undefined') 
{
if ($tr.find('[id*=chkSelect]').prop('checked') == true) 
{
if ($tr.find('input[id*=txtGvDisc]').attr('id') != "" && typeof $tr.find('input[id*=txtGvDisc]').attr('id') != 'undefined') 
{
                    var disc = $tr.find('input[id*=txtGvDisc]').val().replace(/[\$\(\),]/g, '');

                    if (!isNaN(parseFloat(disc))) {
                        tDisc += parseFloat(disc);
}
                    }
                }
}

                    
//////////////


            });

        

            //var vitems = calculateItems();
            //$("#<%=lblOpenItems.ClientID%>").html(vitems.toString());
           <%-- var vendorCount = calculateVendor();
            
            $("#<%=lblVCountValue.ClientID%>").html(vendorCount.toString());--%>

            //$("#<%=lblAutoSelectBalance.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('.cls-payment').html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $("#<%=lblTotalDiscount.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tDisc).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('.cls-disc').html(cleanUpCurrency("$" + parseFloat(tDisc).toLocaleString("en-US", { minimumFractionDigits: 2 })));

            $('.cls-bal').html(cleanUpCurrency("$" + parseFloat(tBal).toLocaleString("en-US", { minimumFractionDigits: 2 })));
<% GetPaymentTotal();%>
//var vdis = $("#<%=lblTotalDiscount.ClientID%>").html().replace(/[\$\(\),]/g, '');
//$('.cls-disc').html(cleanUpCurrency("$" + parseFloat(vdis).toLocaleString("en-US", { minimumFractionDigits: 2 })));
        }

        function calculateItems() {
            var grid = document.getElementById("<%=gvBills.ClientID%>"); //
            var count = 0;
            for (var i = 0; i < <%=gvBills.Items.Count - 2%>; i++) {

                var CheckBox1 = $("input[id*=chkSelect]")//chkRow Id of Check box
                if (CheckBox1[i].checked) {
                    count = count + 1;
                }
            }

            return count;
        }
        function calculateVendor() {
            var grid = document.getElementById("<%=gvBills.ClientID%>"); //
            var count = 0;
            var prevValue = "";
            for (var i = 1; i < grid.rows.length - 1; i++) {
                var CheckBox1 = $("input[id*=chkSelect]")

                //if (i == 1)
                //{
                //    prevValue = grid.rows[i].cells[1].childNodes[1].innerHTML.toString();

                //}
                if (CheckBox1[i - 1].checked) {
                    if (grid.rows[i].cells[1].childNodes[1].innerHTML.toString() == prevValue) {
                        count = count;
                    }
                    else {
                        count = count + 1;
                    }
                    prevValue = grid.rows[i].cells[1].childNodes[1].innerHTML;
                }

            }

            return count;
        }

        //function pageLoad(sender, args) {
        $("#<%=txtDateBefore.ClientID %>").hide();
        $("#<%=txtdated.ClientID %>").hide();
        $(function () {
            function a_dta() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
            }
             var queryven = "";
            function dtaaven() {
                this.prefixText = null;
                this.con = null;
                this.Acct = null;
            }
            $("[id*=txtGvUseTax]").autocomplete({
                    //open: function (e, ui) {
                    //    /* create the scrollbar each time autocomplete menu opens/updates */
                    //    $(".ui-autocomplete").mCustomScrollbar({
                    //        setHeight: 182,
                    //        theme: "dark-3",
                    //        autoExpandScrollbar: true
                    //    });
                    //},
                    //response: function (e, ui) {
                    //    /* destroy the scrollbar after each search completes, before the menu is shown */
                    //    $(".ui-autocomplete").mCustomScrollbar("destroy");
                    //},

                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;

                        var str = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/getUseTaxSearch",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {

                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load account name");
                            }
                        });
                    },
                    select: function (event, ui) {

                        if (ui.item.value == 0)
                            window.location.href = "addbills.aspx";
                        else {
                            var txtGvUseTax = this.id;
                            $(this).val(ui.item.Rate);

                            var hdnUtax = document.getElementById(txtGvUseTax.replace('txtGvUseTax', 'hdnUtax'));
                            var hdnUtaxGL = document.getElementById(txtGvUseTax.replace('txtGvUseTax', 'hdnUtaxGL'));

                            $(hdnUtax).val(ui.item.Name);
                            $(hdnUtaxGL).val(ui.item.GL);
                        }

                        return false;
                    },
                    focus: function (event, ui) {

                        $(this).val(ui.item.Rate);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
                $.each($(".tsearchinput"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var ula = ul;
                        var itema = item;
                        var result_value = item.Rate;
                        var result_item = item.Name;
                        var result_desc = item.GL;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });
                        }

                        if (result_value == 0) {
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a>" + result_item + "</a>")
                                .appendTo(ul);
                        }
                        else {
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_value + "</span></a>")
                                .appendTo(ul);
                        }
                    };
            });

            

            $("[id*=txtGvLoc]").autocomplete({
                //open: function (e, ui) {
                //    /* create the scrollbar each time autocomplete menu opens/updates */
                //    $(".ui-autocomplete").mCustomScrollbar({
                //        setHeight: 182,
                //        theme: "dark-3",
                //        autoExpandScrollbar: true
                //    });
                //},
                //response: function (e, ui) {
                //    /* destroy the scrollbar after each search completes, before the menu is shown */
                //    $(".ui-autocomplete").mCustomScrollbar("destroy");
                //},

                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetJobLocations",
                        data: '{"prefixText": "' + dtaaa.prefixText + '", "IsJob": "' + false + '", "con": "' + dtaaa.con + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load phase details");
                            
                        }
                    });
                },
                select: function (event, ui) {

                    var txtGvLoc = this.id;
                    var txtGvJob = document.getElementById(txtGvLoc.replace('txtGvLoc', 'txtGvJob'));
                    var hdnJobID = document.getElementById(txtGvLoc.replace('txtGvLoc', 'hdnJobID'));

                    $(hdnJobID).val(ui.item.ID);
                    $(txtGvJob).val(ui.item.fDesc);
                    $(this).val(ui.item.Tag);

                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.fDesc);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val());
            })
            $.each($(".jsearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {

                    var ula = ul;
                    var itema = item;
                    var result_value = item.ID;
                    var result_item = item.fDesc;
                    var result_desc = item.Tag;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>';
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>';
                        });
                    }

                    if (result_value == 0) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a><b> Job: </b> " + result_item + ", <span style='color:Gray;'><b> Loc: </b>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
            });

            $("#<%=txtDiscGL.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new a_dta();
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
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load accounts");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtDiscGL.ClientID%>").val(ui.item.label);
                    $("#<%=hdnDiscGL.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtDiscGL.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                    .bind('click', function () { $(this).autocomplete("search"); })
                    .data("ui-autocomplete")._renderItem = function (ul, item) {
                        //debugger;
                        var ula = ul;
                        var itema = item;
                        var result_value = item.value;
                        var result_item = item.acct;
                        var result_desc = item.label;

                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });
                        }

                        if (result_value == 0) {
                            //return $("<li></li>")
                            //.data("item.autocomplete", item)
                            //.append("<a>" + result_item + "</a>")
                            //.appendTo(ul);
                        }
                        else {
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                                .appendTo(ul);
                        }

                }
            $("#<%=txtDiscGLs.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new a_dta();
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
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load accounts");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtDiscGLs.ClientID%>").val(ui.item.label);
                    $("#<%=hdnDiscGLs.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtDiscGLs.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                    .bind('click', function () { $(this).autocomplete("search"); })
                    .data("ui-autocomplete")._renderItem = function (ul, item) {
                        //debugger;
                        var ula = ul;
                        var itema = item;
                        var result_value = item.value;
                        var result_item = item.acct;
                        var result_desc = item.label;

                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });
                        }

                        if (result_value == 0) {
                            //return $("<li></li>")
                            //.data("item.autocomplete", item)
                            //.append("<a>" + result_item + "</a>")
                            //.appendTo(ul);
                        }
                        else {
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                                .appendTo(ul);
                        }

                }
            $("[id*=txtGvUseTax]").keypress(function(e){
                var keyCode = e.which;
                
                 return false;
    
            });
            $("#<%=txtVendor.ClientID%>").autocomplete({

                //open: function (e, ui) {
                //    /* create the scrollbar each time autocomplete menu opens/updates */
                //    $(".ui-autocomplete").mCustomScrollbar({
                //        setHeight: 182,
                //        theme: "dark-3",
                //        autoExpandScrollbar: true
                //    });
                //},
                //response: function (e, ui) {
                //    /* destroy the scrollbar after each search completes, before the menu is shown */
                //    $(".ui-autocomplete").mCustomScrollbar("destroy");
                //},
                source: function (request, response) {

                    var dtaaa = new dtaaven();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetVendorName",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load vendor name");
                        }
                    });
                },
                select: function (event, ui) {
                    var str = ui.item.Name;
                    if (str == "No Record Found!") {
                        $("#<%=txtVendor.ClientID%>").val("");
                    }
                    else {
                        $("#<%=txtVendor.ClientID%>").val(ui.item.Name);
                        $("#<%=hdnVendorID.ClientID%>").val(ui.item.ID);

                        $("#<%=ddlVendor.ClientID%>").val(ui.item.ID);

                        if (document.getElementById('txtqstgv').style.display == 'block') {

                            $("#<%=txtqst.ClientID%>").text(ui.item.STax);
                            $("#<%=hdnQST.ClientID%>").val(ui.item.STaxRate);
                            $("#<%=hdnQSTGL.ClientID%>").val(ui.item.STaxGL);
                            $("#<%=hdnSTaxType.ClientID%>").val(ui.item.STaxType);
                            $("#<%=hdnSTaxName.ClientID%>").val(ui.item.STaxName);
                        }
                        else {
                            $("#<%=txtqst.ClientID%>").text("0");
                            $("#<%=hdnQST.ClientID%>").val("0");
                            $("#<%=hdnQSTGL.ClientID%>").val("0");
                            $("#<%=hdnSTaxType.ClientID%>").val("0");
                            $("#<%=hdnSTaxName.ClientID%>").val("0");
                        }
                        
                        if (document.getElementById('txttaxcodegv').style.display == 'block') {
                            $("#<%=txtusetaxc.ClientID%>").text(ui.item.UTax);
                            $("#<%=hdnusetaxc.ClientID%>").val(ui.item.UTaxRate);
                            $("#<%=hdnusetaxcGL.ClientID%>").val(ui.item.SUaxGL);
                            $("#<%=hdnUTaxType.ClientID%>").val(ui.item.UTaxType);
                            $("#<%=hdnUTaxName.ClientID%>").val(ui.item.UtaxName);
                        }
                        else {
                            $("#<%=txtusetaxc.ClientID%>").text("0");
                            $("#<%=hdnusetaxc.ClientID%>").val("0");
                            $("#<%=hdnusetaxcGL.ClientID%>").val("0");
                            $("#<%=hdnUTaxType.ClientID%>").val("0");
                            $("#<%=hdnUTaxName.ClientID%>").val("0");
                        }


                        CallSelectedIndexChanged();
                        
                        <%--$("#<%=txtPaid.ClientID%>").val(ui.item.Days);
                        $("#<%=txtDueIn.ClientID%>").val(ui.item.Term);
                        

                        if ($('#<%= txtDueIn.ClientID %>').val() != '') {
                            var newduedt = new Date();
                            newduedt.setDate(newduedt.getDate() + parseInt($('#<%= txtDueIn.ClientID %>').val()));
                            var dd = newduedt.getDate();
                            var mm = newduedt.getMonth() + 1;
                            var y = newduedt.getFullYear();
                            if (parseInt(dd) < 10) { dd = "0" + dd; }
                            if (parseInt(mm) < 10) { mm = "0" + mm;}
                            var someFormattedDate = mm + '/'+ dd + '/'+ y;
                            $("#<%=txtDueDate.ClientID%>").val(someFormattedDate);
                        }--%>

                    }

                    return false;
                },
                focus: function (event, ui) {


                    $("#<%=txtVendor.ClientID%>").val(ui.item.Name);

                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ID;
                    var result_item = item.Name;
                    var result_Company = item.Company;
                    var result_desc = item.desc;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span>' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span>' + FullMatch + '</span>'
                        });
                    }
                    if (result_value == 0) {

                        return $("<li></li>")
                            .data("ui-autocomplete-item", item)
                            .append("<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>")
                            .appendTo(ul);
                    }
                    else {
                        var append_data = "";
                        //Premission Check.....
                        var chk = '<%=Convert.ToString(Session["COPer"]) %>';
                        if (chk == "1") {
                            append_data = "<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>" + "<span class='con_hide' style='color:Gray;'> ," + result_Company + "</span></a>";
                        }
                        else {
                            append_data = "<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>";
                        }

                        return $("<li ></li>")
                            .data("ui-autocomplete-item", item)
                            .append(append_data)
                            .appendTo(ul);
                    }
                }
        



            $("[id*=chkSelect]").change(function () {
                try {
                    var chk = $(this).attr('id');
                    // debugger;
                    /////////////////////
                    //var txtPay = $(this).parent().find('input[id$="txtGvPay"]').attr('id');
                    //var txtDisc = $(this).parent().next().next().next().next().find('input[id$="txtGvDisc"]').attr('id');
                    //var lblDue = $(this).parent().next().next().next().find('span[id$="lblBalance"]').attr('id');
                    //var hdnPrevDue = $(this).parent().next().next().next().find('input:hidden[id$="hdnPrevDue"]').attr('id');
                    var hdnSelected = document.getElementById(chk.replace('chkSelect', 'hdnSelected'));
                    var hdnOriginal = document.getElementById(chk.replace('chkSelect', 'hdnOriginal'));
                    var hdnPrevDue = document.getElementById(chk.replace('chkSelect', 'hdnPrevDue'));
                    var lblDue = document.getElementById(chk.replace('chkSelect', 'lblBalance'))
                    var txtDisc = document.getElementById(chk.replace('chkSelect', 'txtGvDisc'));
                    var txtPay = document.getElementById(chk.replace('chkSelect', 'txtGvPay'));
                    var pay = $(txtPay).val().toString().replace(/[\$\(\),]/g, '');
                    var disc = $(txtDisc).val().toString().replace(/[\$\(\),]/g, '');
                    //////////////////////
                    var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
                    var prevDue = parseFloat($(hdnOriginal).val()-$(hdnSelected).val())
                    var pay = 0;

                    var rpay = pay.toLocaleString("en-US", { minimumFractionDigits: 2 });
                    var rprevDue = prevDue.toLocaleString("en-US", { minimumFractionDigits: 2 });
                    if ($(this).prop('checked') == true) {

                        $(txtPay).val(rprevDue)
                        $(txtDisc).val('0.00')
                        $(lblDue).text(cleanUpCurrency('$' + rpay))
                        SelectedRowStyle('<%=gvBills.ClientID %>')
                    }
                    else if ($(this).prop('checked') == false) {
                        $(txtPay).val(rpay)
                        $(txtDisc).val(rpay)
                        $(lblDue).text(cleanUpCurrency('$' + rprevDue))
                      //  $(lblDue).text(cleanUpCurrency('$' + pay))
                        $(this).closest('tr').removeAttr("style");
                    }
                    CalculatePayTotal();
                    CalculatePayTotalSelected();

                } catch (e) {

                }

            });




            $("[id*=txtGvDisc]").change(function () {
                //debugger;
                var txtDisc = $(this).attr('id');
                var chk = $(this).parent().prevAll().find('input:checkbox[id$="chkSelect"]');
                var lblDue = $(this).parent().prev().find('span[id$="lblBalance"]');
                var hdnPrevDue = $(this).parent().prev().find('input:hidden[id$="hdnPrevDue"]');
                var disc = $(this).val().toString().replace(/[\$\(\),]/g, '');
                var pay = $(this).parent().next().find('input[id$="txtGvPay"]').val().replace(/[\$\(\),]/g, '');
                var hdnDisc = $(this).parent().prev().find('input:hidden[id$="hdnDisc"]');
                var hdnSelected = $(this).parent().prev().find('input:hidden[id$="hdnSelected"]');
                var hdnOriginal = $(this).parent().prev().find('input:hidden[id$="hdnOriginal"]');
                
                if (pay == '') {
                    pay = 0;
                    $(this).parent().next().find('input[id$="txtGvPay"]').val('$0.00')
                }
                if (disc == '') {
                    disc = 0;
                    $(this).val('$0.00')
                }

                var Selected = parseFloat($(hdnSelected).val());
                var Original = parseFloat($(hdnOriginal).val());

                var Duepayment = parseFloat(Original) - parseFloat(Selected) - parseFloat(pay) - parseFloat(disc);
                
                if (parseFloat(Duepayment) >= 0) {
                    //$(this).parent().prev().find('span[id$="lblBalance"]').val(parseFloat(Duepayment).toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    $(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                }
                else {
                    disc = 0;
                    var Duepayment = parseFloat(Original) - parseFloat(Selected) - parseFloat(pay) - parseFloat(disc);
                    
                    $(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                }
                total = parseFloat(pay) + parseFloat(disc);
                if (total != 0) {
                    
                    //debugger;
                    //disc = parseFloat(disc);
                    //var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''));
                    //var prevDue = parseFloat($(hdnPrevDue).val())
                    //alert(due);
                    //alert(prevDue);
                    //var IsNeg = false;
                    //if (disc < 0) {
                    //    IsNeg = true;
                    //    pay = pay * -1;
                    //    disc = disc * -1;
                    //    prevDue = prevDue * -1;
                    //    total = total * -1;
                    //}
                    //// debugger;
                    //if (prevDue < total) {
                    //    if (prevDue < disc) {
                    //        pay = 0;
                    //        disc = prevDue;
                    //    }
                    //    else {
                    //        pay = prevDue - disc;
                    //    }
                    //    total = parseFloat(pay) + parseFloat(disc);
                    //}

                    //due = prevDue - total;
                    //if (IsNeg) {
                    //    pay = pay * -1;
                    //    prevDue = prevDue * -1;
                    //    disc = disc * -1;
                    //    due = due * -1;
                    //}


                    //var payy = parseFloat(pay) + parseFloat(due);
                    $(this).val(disc.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    //$(lblDue).text(cleanUpCurrency('$' + due.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    //$(this).parent().next().find('input[id$="txtGvPay"]').val(payy.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                    $(chk).prop('checked', true);
                    SelectedRowStyle('<%=gvBills.ClientID %>')
                }
                else {
                    $(chk).prop('checked', false);
                    $(this).closest('tr').removeAttr("style");
                }
                CalculatePayTotal();
                CalculatePayTotalSelected();
                 document.getElementById('<%=btnSelectChkBox.ClientID%>').click();
            });



            <%--$("[id*=txtGvDisc]").change(function () {
                var txtDisc = $(this).attr('id');
                var chk = $(this).parent().prevAll().find('input:checkbox[id$="chkSelect"]');
                var lblDue = $(this).parent().prev().find('span[id$="lblBalance"]');
                var hdnPrevDue = $(this).parent().prev().find('input:hidden[id$="hdnPrevDue"]');
                var disc = $(this).val().toString().replace(/[\$\(\),]/g, '');
                var pay = $(this).parent().next().find('input[id$="txtGvPay"]').val().replace(/[\$\(\),]/g, '');

                if (pay == '') {
                    pay = 0;
                    $(this).parent().next().find('input[id$="txtGvPay"]').val('$0.00')
                }
                if (disc == '') {
                    disc = 0;
                    $(this).val('$0.00')
                }
                
                total = parseFloat(pay) + parseFloat(disc);
                if (total != 0) {
                    disc = parseFloat(disc);
                    var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''));
                    var prevDue = parseFloat($(hdnPrevDue).val())
                    var IsNeg = false;
                    if (disc < 0) {
                        IsNeg = true;
                        pay = pay * -1;
                        disc = disc * -1;
                        prevDue = prevDue * -1;
                        total = total * -1;
                    }
                    // debugger;
                    if (prevDue < total) {
                        if (prevDue < disc) {
                            pay = 0;
                            disc = prevDue;
                        }
                        else {
                            pay = prevDue - disc;
                        }
                        total = parseFloat(pay) + parseFloat(disc);
                    }

                    due = prevDue - total;
                    if (IsNeg) {
                        pay = pay * -1;
                        prevDue = prevDue * -1;
                        disc = disc * -1;
                        due = due * -1;
                    }

                    $(this).val(disc.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    $(lblDue).text(cleanUpCurrency('$' + due.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    $(this).parent().next().find('input[id$="txtGvPay"]').val(pay.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                    $(chk).prop('checked', true);
                    SelectedRowStyle('<%=gvBills.ClientID %>')
                }
                else {
                    $(chk).prop('checked', false);
                    $(this).closest('tr').removeAttr("style");
                }
                CalculatePayTotal();
                CalculatePayTotalSelected();
            });
          $("[id*=txtGvPay]").change(function () {
                //     debugger;
                var txtPay = $(this).attr('id');
                var chk = $(this).parent().prevAll().find('input:checkbox[id$="chkSelect"]');
                var lblDue = $(this).parent().prev().prev().find('span[id$="lblBalance"]');
                var hdnPrevDue = $(this).parent().prev().prev().find('input:hidden[id$="hdnPrevDue"]');
                var pay = $(this).val().toString().replace(/[\$\(\),]/g, '');
                var disc = $(this).parent().prev().find('input[id$="txtGvDisc"]').val().toString().replace(/[\$\(\),]/g, '');
                var total = 0;
                if (pay == '') {
                    pay = 0;
                    $(this).val('$0.00')
                }
                if (disc == '') {
                    disc = 0;
                    $(this).parent().prev().find('input[id$="txtGvDisc"]').val('$0.00')
              }
              alert(disc);
                total = parseFloat(pay) + parseFloat(disc);
                if (total != 0) {
                    pay = parseFloat(pay);
                    var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''));
                    var prevDue = parseFloat($(hdnPrevDue).val())
                    var IsNeg = false;
                    if (pay < 0) {
                        IsNeg = true;
                        pay = pay * -1;
                        disc = disc * -1;
                        prevDue = prevDue * -1;
                        total = total * -1;
                    }
                    //debugger;
                    if (prevDue < total) {
                        pay = prevDue - disc;
                        total = parseFloat(pay) + parseFloat(disc);
                    }

                    due = prevDue - total;
                    if (IsNeg) {
                        pay = pay * -1;
                        prevDue = prevDue * -1;
                        disc = disc * -1;
                        due = due * -1;
                    }

                    $(this).val(pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    $(lblDue).text(cleanUpCurrency('$' + due.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    $(chk).prop('checked', true);
                    SelectedRowStyle('<%=gvBills.ClientID %>')
                }
                else {
                    $(chk).prop('checked', false);
                    $(this).closest('tr').removeAttr("style");
                }
                //GetInvoiceTotal();
                CalculatePayTotal();
                CalculatePayTotalSelected(); 
                document.getElementById('<%=btnSelectChkBox.ClientID%>').click();
              
            });
        });--%>







        $("[id*=txtGvPay]").change(function () {
                   // debugger;
                    var txtPay = $(this).attr('id');
                
                var chk = $(this).parent().prevAll().find('input:checkbox[id$="chkSelect"]');
                var lblDue = $(this).parent().prev().prev().find('span[id$="lblBalance"]');
                var hdnPrevDue = $(this).parent().prev().prev().find('input:hidden[id$="hdnPrevDue"]');
                var pay = $(this).val().toString().replace(/[\$\(\),]/g, '');
                var disc = $(this).parent().prev().find('input[id$="txtGvDisc"]').val().toString().replace(/[\$\(\),]/g, '');

                var hdnDisc = $(this).parent().prev().prev().find('input:hidden[id$="hdnDisc"]');
                var hdnSelected = $(this).parent().prev().prev().find('input:hidden[id$="hdnSelected"]');
                var hdnOriginal = $(this).parent().prev().prev().find('input:hidden[id$="hdnOriginal"]');

                //-----------------Start:Commented By Juily - 19-12-2019 ---------------//
                //var total = 0;
                //-----------------End: By Juily - 19-12-2019 -------------------------//
                if (pay == '') {
                    pay = 0;
                    $(this).val('$0.00')
                }
                if (disc == '') {
                    disc = 0;
                    $(this).parent().prev().find('input[id$="txtGvDisc"]').val('$0.00')
                }

                var Selected = parseFloat($(hdnSelected).val());
                var Original = parseFloat($(hdnOriginal).val());
                

            var Duepayment = parseFloat(Original) - parseFloat(Selected) - parseFloat(pay) - parseFloat(disc);
            //------------------Start:Code By Juily - 19-12-2019 --------------------//
            if ((parseFloat(Duepayment) < 0 && parseFloat(Original) > 0) || (parseFloat(Duepayment) > 0 && parseFloat(Original) < 0))
                {
                        noty({
                            text: 'OverPayment is not allowed.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: true,
                            timeout: false,
                            theme: 'noty_theme_default',
                            closable: false
                    });

                    pay =0;
                    var Duepayment = parseFloat(Original) - parseFloat(Selected) - parseFloat(pay) - parseFloat(disc);
                    $(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    //$(lblDue).text('$0.00');
                    total = parseFloat(pay) + parseFloat(disc);
                    if (total == 0)
                    {
                        $(this).val(pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        $(chk).prop('checked', true);
                        SelectedRowStyle('<%=gvBills.ClientID %>')
                    }
                    else
                    {
                        $(chk).prop('checked', false);
                        $(this).closest('tr').removeAttr("style");
                    }
                }
                else
                {
                    //$(lblDue).text('$0.00');
                    $(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    total = parseFloat(pay) + parseFloat(disc);
                    if (total != 0)
                    {
                        $(this).val(pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        $(chk).prop('checked', true);
                        SelectedRowStyle('<%=gvBills.ClientID %>')
                    }
                    else
                    {
                        $(chk).prop('checked', false);
                        $(this).closest('tr').removeAttr("style");
                    }
                }

            //-------------------End: By Juily - 19-12-2019-------------------------//

            //------------------Start:Code By Juily - 19-12-2019 --------------------//
                //$(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
            //-------------------End: By Juily - 19-12-2019-------------------------//

                //if (parseFloat(Duepayment) >= 0) {
                //    //$(this).parent().prev().find('span[id$="lblBalance"]').val(parseFloat(Duepayment).toLocaleString("en-US", { minimumFractionDigits: 2 }));
                //    $(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                //}
                //else {
                //    pay = 0;
                //    var Duepayment = parseFloat(Original) - parseFloat(Selected) - parseFloat(pay) - parseFloat(disc);
                    
                //    $(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                //}
            //-----------------Start:Commented By Juily - 19-12-2019 ---------------//
                <%-- %>total = parseFloat(pay) + parseFloat(disc);
                if (total != 0) {
                    //pay = parseFloat(pay);
                    //var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''));
                    //var prevDue = parseFloat($(hdnPrevDue).val())
                    //var IsNeg = false;
                    //if (pay < 0) {
                    //    IsNeg = true;
                    //    pay = pay * -1;
                    //    disc = disc * -1;
                    //    prevDue = prevDue * -1;
                    //    total = total * -1;
                    //}
                    ////debugger;
                    //if (prevDue < total) {
                    //    pay = prevDue - disc;
                    //    total = parseFloat(pay) + parseFloat(disc);
                    //}

                    //due = prevDue - total;
                    //if (IsNeg) {
                    //    pay = pay * -1;
                    //    prevDue = prevDue * -1;
                    //    disc = disc * -1;
                    //    due = due * -1;
                    //}

                    $(this).val(pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    //$(lblDue).text(cleanUpCurrency('$' + due.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    $(chk).prop('checked', true);
                    SelectedRowStyle('<%=gvBills.ClientID %>')
                }
                else {
                    $(chk).prop('checked', false);
                    $(this).closest('tr').removeAttr("style");
                }--%>
            //-------------------End: By Juily - 19-12-2019-------------------------//
                //GetInvoiceTotal();
                CalculatePayTotal();
                CalculatePayTotalSelected();
                //PageMethods.GetPaymentRetainCheckbox();
                 document.getElementById('<%=btnSelectChkBox.ClientID%>').click();
              }) 
            });


        $('input[name="radio-group"]').change(function () {

            if (document.getElementById('rdDateBefore').checked) {
                $("#<%=txtDateBefore.ClientID %>").show();
                $("#<%=txtdated.ClientID %>").hide();
            }
            if (document.getElementById('rdDated').checked) {
                $("#<%=txtdated.ClientID %>").show();
                $("#<%=txtDateBefore.ClientID %>").hide();

            }
            if (document.getElementById('rdDue').checked) {
                $("#<%=txtdated.ClientID %>").hide();
                $("#<%=txtDateBefore.ClientID %>").hide();

            }
            if (document.getElementById('rdClear').checked) {
                $("#<%=txtdated.ClientID %>").hide();
                $("#<%=txtDateBefore.ClientID %>").hide();

            }
            if (document.getElementById('rdRegard').checked) {
                $("#<%=txtdated.ClientID %>").hide();
                $("#<%=txtDateBefore.ClientID %>").hide();

            }

            //var val = document.getElementById('rdDue').checked;
            //alert(val);
        });



        //}
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
        function cancel() {

          <%--  window.parent.document.getElementById('<%=mpeTemplate.ClientID%>').click();--%>
            window.parent.document.getElementById('<%=btnSave.ClientID%>').click();
            return true;
        }

        function cancel() {
            //debugger;
            window.parent.document.getElementById('btnCancel').click();

            window.parent.document.getElementById('<%=btnSave.ClientID%>').click();
            return true;
        }


        function cancelbatch() {
            //debugger;
            window.parent.document.getElementById('bbtnClose').click();

            window.parent.document.getElementById('btnCncl').click();
            return true;
        }



        function ReloadPage() {
            return true;
        }
        function isNumberKeyCheck(evt, txt) {
            var valpaymethod = $("#<%=ddlPayment.ClientID%>").val();
            if (valpaymethod == "0") {
                var charCode = (evt.which) ? evt.which : event.keyCode

                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
            else {
                return true;
            }
        }

        function isNumberKey(evt, txt) {

            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        $(document).ready(function () {
            $("#btnPrint").click(function () {
                $("#<%=pnlCheck.ClientID%>").print();
                return (false);
            });
            InitializeGrids('<%=RadGrid_gvJobCostItems.ClientID%>');


            $("[id*=txtGvUseTax]").keypress(function (e) {
                var keyCode = e.which;
               
                return false;
               
            });  





        });




         
        function isDecimalKey(el, evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode;

            var number = el.value.split('.');
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
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
        function dtaa() {
            //this.prefixText = null;
            //this.con = null;
            this.checkno = null;
            this.bank = null;

        }
        function IsExistCheckNo() {

            var valCheckNo = $("#<%=txtNextCheck.ClientID%>").val();
            var valBank = $("#<%=ddlBank.ClientID%>").val();
            var valPaymenttype = $("#<%=ddlPayment.ClientID%>").val();
            var dtaaa = new dtaa();
            dtaaa.checkno = valCheckNo;
            dtaaa.bank = valBank;
            dtaaa.paytype = valPaymenttype;
            
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "AccountAutoFill.asmx/CheckNumValid",
                data: JSON.stringify(dtaaa),
                dataType: "json",
                async: true,
                success: function (data) {

                    if (data.d == true) {
                        noty({
                            text: 'Check #' + valCheckNo + ' already exists. Since duplicate check numbers are not supported, the check generation process cannot continue.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 15000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                        $("#<%=txtNextCheck.ClientID%>").val('');
                    }
                    Materialize.updateTextFields();
                    return true;
                },
                failure: function (response) {
                    //alert(response);
                    return false;
                },
                error: function (result) {
                    alert("Due to unexpected errors we were unable to load availability");
                    return false;
                }
            });
        }

        function OnChange(txt) {
            $("#mesg")[0].innerHTML = "";
        }
        function displayWriteCheck() {
            $("#dvWriteCheck").show();
            $("#btnPrint").show();
            $("#<%=btnSubmit.ClientID %>").show();
              <%-- $("#<%=btnPrintCheck.ClientID%>").show();--%>
            $("#<%=btnCutCheck.ClientID%>").hide();
        }
        function displayapplycredit(showhide) {
            if (showhide == "1") {
                $("#<%=btnApplyCredit.ClientID %>").show();
            }
            else {
                $("#<%=btnApplyCredit.ClientID %>").hide();
            }
            
        }
        function VisibleRow(row, txt, gridview, event) {  //To make row's textbox visible

            var rowst = document.getElementById(row);
            var grid = document.getElementById(gridview);
            $('#<%=gvBills.ClientID %> input:text.non-trans').each(function () {

                $(this).removeClass("non-trans");
                $(this).addClass("texttransparent");
            });
            $('#<%=gvBills.ClientID %> select.non-trans').each(function () {

                $(this).removeClass("non-trans");
                $(this).addClass("texttransparent");

            });

            var txtGvDisc = document.getElementById(txt);
            $(txtGvDisc).removeClass("texttransparent");
            $(txtGvDisc).addClass("non-trans");

            var txtGvPay = document.getElementById(txt.replace('txtGvDisc', 'txtGvPay'));
            $(txtGvPay).removeClass("texttransparent");
            $(txtGvPay).addClass("non-trans");

        }
        
        function DiscEmptyValidate(sender, args) {

            var tDisc = 0;
            <%--$("#<%=gvBills.ClientID %>").find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);
                if ($tr.find('input[id*=txtGvDisc]').attr('id') != "" && typeof $tr.find('input[id*=txtGvDisc]').attr('id') != 'undefined') {
                    var disc = $tr.find('input[id*=txtGvDisc]').val().replace(/[\$\(\),]/g, '');

                    if (!isNaN(parseFloat(disc))) {
                        tDisc += parseFloat(disc);
                    }
                }
            });--%>
            var vdis = $("#<%=lblTotalDiscount.ClientID%>").html().replace(/[\$\(\),]/g, '');
            if (!isNaN(parseFloat(vdis))) {
                        tDisc += parseFloat(vdis);
                    }
            if (tDisc != 0) {
                var hdnDiscGL = document.getElementById('<%=hdnDiscGL.ClientID%>');
                if (hdnDiscGL.value == '') {
                    args.IsValid = false;
                }
            }
        }
        function DiscEmptyValidates(sender, args) {

            var tDisc = 0;
            <%--$("#<%=gvBills.ClientID %>").find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);
                if ($tr.find('input[id*=txtGvDisc]').attr('id') != "" && typeof $tr.find('input[id*=txtGvDisc]').attr('id') != 'undefined') {
                    var disc = $tr.find('input[id*=txtGvDisc]').val().replace(/[\$\(\),]/g, '');

                    if (!isNaN(parseFloat(disc))) {
                        tDisc += parseFloat(disc);
                    }
                }
            });--%>
            var vdis = $("#<%=lblTotalDiscount.ClientID%>").html().replace(/[\$\(\),]/g, '');
            if (!isNaN(parseFloat(vdis))) {
                        tDisc += parseFloat(vdis);
                    }
            if (tDisc != 0) {
                var hdnDiscGLs = document.getElementById('<%=hdnDiscGLs.ClientID%>');
                if (hdnDiscGLs.value == '') {
                    args.IsValid = false;
                }
            }
        }
        function Alert() {
            alert('alert')
        }


        //function isDecimalKey(el, evt) {

        //    var charCode = (evt.which) ? evt.which : event.keyCode;

        //    var number = el.value.split('.');
        //    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        //        return false;
        //    }

        //    if (number.length > 1 && charCode == 46) {
        //        return false;
        //    }
        //    var caratPos = getSelectionStart(el);
        //    var dotPos = el.value.indexOf(".");
        //    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        //        return false;
        //    }
        //    return true;
        //}
        //function getSelectionStart(o) {
        //    if (o.createTextRange) {
        //        var r = document.selection.createRange().duplicate()
        //        r.moveEnd('character', o.value.length)
        //        if (r.text == '') return o.value.length
        //        return o.value.lastIndexOf(r.text)
        //    } else return o.selectionStart
        //}
        function opencCeateForm() {
            //alert("Hello");
            //modal.hide();
            window.radopen(null, "RadCreateWindow");
        }



        function SelectedRowStyle(gridview) {
            var grid = document.getElementById(gridview);
            $('#' + gridview + ' tr').each(function () {
                var $tr = $(this);
                var chk = $tr.find('input[id*=chkSelect]');
                if (chk.prop('checked') == true) {
                    $tr.css('background-color', '#c3dcf8');
                    $tr.css('font-weight', 'bold');
                }
            })
        }
    </script>


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
     
    <%--<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>--%>
    <telerik:RadAjaxManager ID="RadAjaxManager_WC" runat="server" OnAjaxRequest="RadAjaxManager_WC_AjaxRequest1">
        <AjaxSettings>

            <%--<telerik:AjaxSetting AjaxControlID="ddlPayment" EventName="SelectedIndexChanged"> 
             <UpdatedControls> 
                 <telerik:AjaxUpdatedControl ControlID="txtNextCheck"  LoadingPanelID="RadAjaxLoadingPanel_WC"/> 
             </UpdatedControls> 
         </telerik:AjaxSetting>--%>
            <%--<telerik:AjaxSetting AjaxControlID="RadAjaxManager_WC"> 
             <UpdatedControls> 
                 <telerik:AjaxUpdatedControl ControlID="btnSubmit" /> 
             </UpdatedControls> 
         </telerik:AjaxSetting>--%>

             <telerik:AjaxSetting AjaxControlID="btnAddNewLines">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvnobills" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnCopyPrevious">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvnobills" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowTemplates" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowTemplates" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="RadAjaxManager_WC">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnCutCheck" />
                     <telerik:AjaxUpdatedControl ControlID="btnSelectChkBox" />
                    
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="chkBatch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowAutomaticSelectionForPayment" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowTemplates" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="ddlVendor">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvBills" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkSaveAutopayment">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvBills" LoadingPanelID="RadAjaxLoadingPanel_WC" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%--<telerik:AjaxSetting AjaxControlID="gvBills">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvBills" LoadingPanelID="RadAjaxLoadingPanel_WC" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
            <telerik:AjaxSetting AjaxControlID="chkSelect">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblTotalAmount" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="chkSelect">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblRequirement" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="chkSelect">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblVendorBal" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="chkSelect">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblTotalOrig" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_WC"  runat="server">
    </telerik:RadAjaxLoadingPanel>


    <div runat="server" id="divSuccess">
        <button type="button" class="close" data-dismiss="alert">×</button>
        These month/year period is closed out. You do not have permission to add/update this record.
    </div>

    <div style="height: 65px !important;">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-communication-contacts"></i>&nbsp;
                                         <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Write Checks</asp:Label>
                                        <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <div class="btnlinks">
                                                <%--<asp:LinkButton ID="btnSubmit" runat="server" CausesValidation="true" TabIndex="38" Visible="false" OnClientClick="return disablebuttons();"  OnClick="btnSubmit_Click1">--%>
                                                <asp:LinkButton ID="btnSubmit" runat="server" CausesValidation="true" TabIndex="38" Visible="false" OnClientClick="disableButton(this,''); javascript:return disablebuttons();"  OnClick="btnSubmit_Click1">
                                            Save & Print Check
                                                </asp:LinkButton>
                                            </div>
                                            
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSelectChkBox" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="btnSave" runat="server" ValidationGroup="Check" CausesValidation="true" OnClick="btnSubmit_Click" Style="display: none;">
                                           Submit
                                        </asp:LinkButton>
                                    </div>

                                    <div class="btnlinks">
                                        <asp:LinkButton runat="server" ID="btnCutCheck" OnClick="btnCutCheck_Click" CausesValidation="true" AutoPostBack="true"  OnClientClick="checkvalidation()">Process Payment</asp:LinkButton>
                                    </div>
                                    <%--<asp:UpdatePanel ID="UpdatePanelapplycredit" runat="server">
                                        <ContentTemplate>--%>
                                    <div class="btnlinks">
                                        <%--<asp:LinkButton runat="server" ID="btnApplyCredit" OnClick="btnApplyCredit_Click" AutoPostBack="true" Style="display: none;" OnClientClick="OpenApplyCreditModal();return false">Apply Credit</asp:LinkButton>--%>
                                        <asp:LinkButton runat="server" ID="btnApplyCredit"  Style="display: none;" OnClientClick="OpenApplyCreditModal();return false">Apply Credit</asp:LinkButton>
                                    </div>
                                           <%-- </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlVendor" />                                            
                                        </Triggers>
                                    </asp:UpdatePanel>--%>
                                     <asp:Button ID="btnSelectChkBox" runat="server" CausesValidation="False" OnClick="btnSelectChkBox_Click" 
                                            Style="display: none;" Text="Button" />
                                    <div class="rght-content">
                                        <%--<div class="editlabel">
                                            <span id="chkNumber">Check Number: 1224</span>
                                        </div>--%>
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click" ToolTip="Close"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                    <div class="col s12 m12 l12" runat="server" id="dvbilldetailtop">
                        <div class="row">
                            <div class="tblnks">
                                <ul class="anchor-links" id="ulAccrd">
                                    <li>
                                        <div style="float: left;">
                                            <asp:LinkButton ID="chkBatch" runat="server" OnClick="chkBatch_CheckedChanged" CausesValidation="false">Select for Check Run</asp:LinkButton>
                                        </div>


                                        <div style="display: flex; flex-wrap: nowrap;">
                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" >
                                                <ContentTemplate>
                                                    
                                                     <div style="float: left; margin-left: 5px;">
                                                        <asp:Label ID="lblVendorCount" runat="server" Text="Vendors:" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblVCountValue" runat="server" Visible="false"></asp:Label>
                                                    </div>
                                                    <div style="float: left; margin-left: 5px;">
                                                        <asp:Label ID="lblOI" runat="server" Text="Selected Items:" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOpenItems" runat="server" Visible="false"></asp:Label>
                                                    </div>

                                                    <div style="float: left; margin-left: 5px;">
                                                        <asp:Label ID="lblBal" runat="server" Text="Payment:" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblAutoSelectBalance" runat="server" Visible="false"></asp:Label>
                                                    </div>
                                                        
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                                     <asp:AsyncPostBackTrigger ControlID="btnSelectChkBox" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            </div>









                                       
                                </ul>
                            </div>

                            <div class="tblnksright">
                                <%--&nbsp;--%>
                                 
                                    
                                        <%-- <div style="float: right;">
                                    <asp:Label ID="Label2" runat="server" Text="Running Total Selected Bill Count "></asp:Label>
                                </div>
                                <div style="float: right;">
                                    <asp:Label ID="lblruntotbillcount" runat="server" Text="1111"></asp:Label>
                                </div>
                                         <div style="float: right;">
                                    <asp:Label ID="Label3" runat="server" Text="Selected Vendor Bill Count "></asp:Label>
                                </div>
                                <div style="float: right;">
                                    <asp:Label ID="Label1" runat="server" Text="2222"></asp:Label>
                                </div>--%>
                                      <asp:UpdatePanel ID="updtrunbillcount" runat="server">
                                            <ContentTemplate>
                                     <div style=" display: flex; flex-wrap: nowrap; float:right; padding-right:30px;">
  <div style="margin: 5px;">Running Total Selected Bill Count : </div>
  <div style="margin: 5px; padding-right:5px;"><asp:Label ID="lblruntotbillcount" runat="server" Text="0"></asp:Label></div>
  <%--<div style="margin: 5px;">Selected Vendor Bill Count : </div>  
  <div style="margin: 5px; padding-right:5px;"><asp:Label ID="lblruntotbillcountvendor" runat="server" Text="0"></asp:Label></div>--%>
 
</div>
                                                </ContentTemplate>
                                          <Triggers>
                                              <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                                     <asp:AsyncPostBackTrigger ControlID="btnSelectChkBox" />
                                          </Triggers>
                                          </asp:UpdatePanel>   

                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="srchpane-advanced" id="srchpaneupr" runat="server" >
                <div class="form-content-wrap">

                    <div class="form-content-pd">

                        <div class="form-section-row" id="dpane" runat="server">
                            <div class="form-section3" style="height:150px !important">
                            <%--<div class="form-section3-div4">--%>
                                <div class="input-field col s12">
                                    <div class="row">
                                        

                                        <asp:RequiredFieldValidator runat="server" ID="rfvtxtvendor" ErrorMessage="Please select Vendor"
                                                                        Display="None" ControlToValidate="txtVendor" ></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender111" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="rfvtxtvendor" />
                                                                   <%-- <asp:CustomValidator ID="cvVendor" runat="server" ClientValidationFunction="ChkVendor"
                                                                        ControlToValidate="txtVendor" Display="None" ErrorMessage="Please select the vendor"
                                                                        SetFocusOnError="True" Enabled="False" ValidationGroup="QuickCheck"></asp:CustomValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceVendor1" runat="server" Enabled="True"
                                                                        PopupPosition="BottomLeft" TargetControlID="cvVendor">
                                                                    </asp:ValidatorCalloutExtender>--%>

                                                                    <asp:TextBox ID="txtVendor" runat="server" MaxLength="75"
                                                                        placeholder="Search by vendor"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnVendorID" runat="server" />
                                                                    <asp:HiddenField ID="hdEditCase" Value="false" runat="server" />
                                                                    <label for="txtVendor">Vendor</label>

                                     </div>
                                    </div>
                                <div class="input-field col s12">
                                    <div class="row">
                                         
                                        <asp:TextBox ID="txtbillref" runat="server"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvbillref" ErrorMessage="Please enter Ref" ControlToValidate="txtbillref"
                                                                        Display="None" ></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender222" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                        TargetControlID="rfvbillref" />

                                        <%--<asp:CustomValidator ID="cvbillref" runat="server" ClientValidationFunction="Chkbillref"
                                                                        ControlToValidate="txtbillref" Display="None" ErrorMessage="Please enter Ref"
                                                                        SetFocusOnError="True" Enabled="False" ValidationGroup="QuickCheck"></asp:CustomValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                        PopupPosition="BottomLeft" TargetControlID="cvbillref">
                                                                    </asp:ValidatorCalloutExtender>--%>

                                                                    <label for="txtbillref">Ref No</label>
                                    </div>
                                    </div>

                                <%--<div class="input-field col s12 pdrt lblfield" style="display:none;" id="txttaxcodegv">
                                                                <div class="row">
                                                                    <span class="ttlab">Use Tax</span>
                                                                    <span class="ttlval"><asp:Label ID="txtusetaxc" runat="server" Text="0.00"></asp:Label></span>
                                                                    <asp:HiddenField ID="hdnusetaxcGL" runat="server" />
                                                                    <asp:HiddenField ID="hdnusetaxc" runat="server" />
                                                                    <asp:HiddenField ID="hdnUTaxType" runat="server" />
                                                                    <asp:HiddenField ID="hdnUTaxName" runat="server" />
                                                                </div>
                                                            </div>--%>
                                <div class="input-field col s12" id="txtdiscgv">
                                    <div class="row">
                                        <span id="txttaxcodegv"></span>
                                        <span class="ttlval" style="display:none;"><asp:Label ID="txtusetaxc" runat="server" Text="0.00"></asp:Label></span>
                                                                    <asp:HiddenField ID="hdnusetaxcGL" runat="server" />
                                                                    <asp:HiddenField ID="hdnusetaxc" runat="server" />
                                                                    <asp:HiddenField ID="hdnUTaxType" runat="server" />
                                                                    <asp:HiddenField ID="hdnUTaxName" runat="server" />

                                        <asp:HiddenField ID="hdnDiscGLs" runat="server" />
                                        <asp:CustomValidator ID="cvDiscGLs" runat="server" ValidateEmptyText="true"
                                            ClientValidationFunction="DiscEmptyValidates"
                                            ControlToValidate="txtDiscGLs"
                                             ErrorMessage="Please select Discount GL account"
                                            Display="None"></asp:CustomValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="cvDiscGLs" />

                                       

                                        <asp:TextBox runat="server" ID="txtDiscGLs" Placeholder="Search by acct# and name"></asp:TextBox>
                                        <asp:Label runat="server" ID="lbltxtDiscGLs" AssociatedControlID="txtDiscGLs">Discount GL</asp:Label>
                                    </div>
                                </div>
                                </div>
                            <%--</div>--%>
                            <div class="form-section3-blank">
                                
                                &nbsp;
                            </div>
                        
                            <div class="form-section3">
                            
                                <div class="input-field col s12" >
                                    <div class="row">
                                     
                                         <asp:TextBox ID="txtMemo1" runat="server"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvMemo1" ErrorMessage="Please enter Memo" ControlToValidate="txtMemo1"
                                                                        Display="None" ></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceMemo" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                        TargetControlID="rfvMemo1" />
                                        <%--<asp:CustomValidator ID="cvbillmemo" runat="server" ClientValidationFunction="Chkbillmemo"
                                                                        ControlToValidate="txtMemo1" Display="None" ErrorMessage="Please enter bill memo"
                                                                        SetFocusOnError="True" Enabled="False" ValidationGroup="QuickCheck"></asp:CustomValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                                                        PopupPosition="BottomLeft" TargetControlID="cvbillmemo">
                                                                    </asp:ValidatorCalloutExtender>--%>
                                                                    <label for="txtMemo1">Bill Memo</label>
                                         </div>
                                         </div>
                                    <div class="text-field col s12"> 
                                        <div class="row">
                                        <label style="font-size: 0.9em;">Bill Total</label>
                                            <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                                <asp:Label ID="lblTotalAmount11" runat="server" ></asp:Label>
                                            </span>
                                    


                                     </div>
                                             </div>

                                 <div class="input-field col s12 pdrt lblfield" id="txtgstgv" runat="server">
                                                                <div class="row">
                                                                    <span class="ttlab">GST</span>
                                                                    <span class="ttlval"><asp:Label ID="txtgst" runat="server" Text="0.00"></asp:Label></span>
                                                                    <asp:HiddenField ID="hdnGSTGL" runat="server" />
                                                                    <asp:HiddenField ID="hdnGST" runat="server" />
                                                                </div>
                                                            </div>

                                </div>
                            <%--</div>--%>
                            <div class="form-section3-blank">
                            
                                &nbsp;
                            </div>
                             <div class="form-section3">
                            
                                <div class="input-field col s12" >
                                   
                                    <div class="row">
                                        <div class="input-field col s6" style="padding-top:27px;" >
                                         <%--<asp:Panel ID="pnlRecurring" runat="server" style="display: flex;" >--%>
                                        <%--<span class="angleicons" >--%>
                                            <asp:CheckBox ID="chkIsRecurr" runat="server" Text="Is Recurring" CssClass="css-checkbox" onclick="showfreq();"  />
                                            </div>
                                        <%--</span>
                                         <span class="angleicons" style="width: 150px; display:none" id="dvfreq">--%>
                                        <div class="input-field col s6" style="display:none;" id="dvfreq">
                                        <asp:DropDownList ID="ddlFrequency" runat="server" CssClass="browser-default"></asp:DropDownList>
                                       </div>
                                            <%--</span>--%>
                                             <%--</asp:Panel>--%>
                                        
                                    </div>
                                    </div>
                                    <div class="text-field col s12">
                                    <div class="row">
                                    <label style="font-size: 0.9em;">Use Tax</label>
                                        <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                            
                                                    <asp:Label ID="lblTotalUseTax" runat="server" ></asp:Label>
                                                
                                        </span>
                                      
                                        </div>
                                        </div>

                                  <div class="input-field col s12 pdrt lblfield" style="display:none;" id="txtqstgv">
                                                                <div class="row">
                                                                    <span class="ttlab" id="spansalestax" runat="server">Sales Tax</span>
                                                                    <span class="ttlval"><asp:Label ID="txtqst" runat="server" Text="0.00"></asp:Label></span>
                                                                    <asp:HiddenField ID="hdnQSTGL" runat="server" />
                                                                    <asp:HiddenField ID="hdnQST" runat="server" />
                                                                    <asp:HiddenField ID="hdnSTaxType" runat="server" />
                                                                    <asp:HiddenField ID="hdnSTaxName" runat="server" />
                                                                </div>
                                                            </div>

                                </div>
                            </div>
                            <%--<div class="form-section3-blank">
                            
                                &nbsp;
                            </div>
                             <div class="form-section3">
                            
                                <div class="input-field col s12" >
                                    <div class="row">
                                       
                                     </div>
                                </div>
                            </div>--%>
                           
                        </div>

                        <div class="form-section-row" id="srchpane" runat="server">
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <label class="drpdwn-label">Search for Bills</label>
                                        <asp:DropDownList ID="ddlInvoice" runat="server" CssClass="browser-default" OnSelectedIndexChanged="ddlInvoice_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0">All</asp:ListItem>
                                            <asp:ListItem Value="1">Outstanding</asp:ListItem>
                                            <asp:ListItem Value="2">Due</asp:ListItem>
                                            <asp:ListItem Value="3">Credit Bills</asp:ListItem>
                                        </asp:DropDownList>

                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtSearchDate" runat="server" CssClass="datepicker_mom" MaxLength="10"
                                                    autocomplete="off"></asp:TextBox>
                                                <%--<asp:CalendarExtender ID="txtSearchDate_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtSearchDate">
                                                </asp:CalendarExtender>--%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                                <div class="input-field col s12">
                                    <div class="row">
                                        <label class="drpdwn-label">Vendor</label>
                                        <asp:DropDownList ID="ddlVendor" runat="server" CssClass="browser-default" onchange="ddlVendoronchange(this.value);" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged"
                                            AutoPostBack="true" TabIndex="1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvVendor" ControlToValidate="ddlVendor"
                                            ErrorMessage="Please select Vendor" Display="None" InitialValue="0"
                                            ValidationGroup="Checks"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceVendor" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvVendor" />
                                    </div>
                                </div>
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:HiddenField ID="hdnDiscGL" runat="server" />
                                        <asp:CustomValidator ID="cvDiscGL" runat="server" ValidateEmptyText="true"
                                            ClientValidationFunction="DiscEmptyValidate"
                                            ControlToValidate="txtDiscGL"
                                            ErrorMessage="Please select Discount GL account"
                                            Display="None"></asp:CustomValidator>
                                        <asp:ValidatorCalloutExtender ID="vceDiscGL" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="cvDiscGL" />
                                        <asp:TextBox runat="server" ID="txtDiscGL" Placeholder="Search by acct# and name"></asp:TextBox>
                                        <asp:Label runat="server" ID="lbltxtDiscGL" AssociatedControlID="txtDiscGL">Discount GL</asp:Label>
                                    </div>
                                </div>
                                <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                    <div class="row">
                                        <label class="drpdwn-label">Company</label>
                                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="browser-default" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="text-field col s12" runat="server" id="vendorBalance">
                                    <div class="row">
                                        <label style="font-size: 0.9em;">Vendor Balance</label>
                                        <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                            <asp:UpdatePanel ID="updVendorBal" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblVendorBal" runat="server"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlVendor" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </span>
                                    </div>
                                </div>                              
                                  <div class="text-field col s12" runat="server" id="DiscTaken">
                                    <div class="row">
                                        <label style="font-size: 0.9em;">Discount Taken</label>
                                        <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                            <asp:UpdatePanel ID="updTotalDisc" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTotalDiscount" runat="server"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                                     <asp:AsyncPostBackTrigger ControlID="btnSelectChkBox" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </span>
                                    </div>
                                </div>
                                <div class="text-field col s12" runat="server" id="Div1">
                                    <div class="row">
                                        <label style="font-size: 0.9em;">Selected Vendor Count</label>
                                        <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                            <asp:UpdatePanel ID="UpdateSelectedVendorCount" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblSelectedVendorCount" runat="server"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                                     <asp:AsyncPostBackTrigger ControlID="btnSelectChkBox" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="text-field col s12" runat="server" id="SelectPay">
                                    <div class="row">
                                        <label style="font-size: 0.9em;">Selected Vendor Payment</label>
                                        <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                            <asp:UpdatePanel ID="updSelectedPay" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblSelectedPayment" runat="server"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                                     
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </span>
                                    </div>
                                    <asp:HiddenField runat="server" ID="hdnTPay" />
                                </div>
                                 <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                <ContentTemplate>
                               <div class="text-field col s12" runat="server" id="runningBalance">
                                    <div class="row">
                                        <label style="font-size: 0.9em;">Running Total Balance Selected</label>                                        
                                        <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                            <asp:Label ID="lblRunBalance" runat="server"></asp:Label>
                                        </span>
                                    </div>
                                </div>
                                 <div class="text-field col s12" runat="server" id="vendorCount">
                                    <div class="row">
                                        <label style="font-size: 0.9em;">Running Vendor Count Selected</label>
                                        <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                            <asp:Label ID="lblTotalVendorCount" runat="server"></asp:Label>
                                        </span>
                                    </div>
                                </div>
                                                     </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                                     <asp:AsyncPostBackTrigger ControlID="btnSelectChkBox" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                            </div>
                        </div>
                        <div id="dvWriteCheck" style="display: none;">
                            <%-- <asp:UpdatePanel runat="server">
                                <ContentTemplate>--%>
                            <div class="form-section-row" id="pnlWriteCheck">
                                <div class="section-ttle">
                                    Generate Check 
                                </div>
                                <div class="form-section3">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <label class="drpdwn-label">Bank Account </label>
                                            <asp:DropDownList ID="ddlBank" runat="server" CssClass="browser-default" ValidationGroup="Check"
                                                OnSelectedIndexChanged="ddlBank_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvBank" ControlToValidate="ddlBank"
                                                ErrorMessage="Please select Bank" Display="None" InitialValue="0"
                                                ValidationGroup="Check"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceBank" runat="server" Enabled="True" PopupPosition="Right"
                                                TargetControlID="rfvBank" />
                                        </div>
                                    </div>
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <label class="drpdwn-label">Form of Payment</label>

                                            <asp:DropDownList ID="ddlPayment" runat="server" CssClass="browser-default"
                                                OnSelectedIndexChanged="ddlPayment_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvPayment" ControlToValidate="ddlPayment"
                                                ErrorMessage="Please select Payment" Display="None" InitialValue="-1"
                                                ValidationGroup="Check"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vcePayment" runat="server" Enabled="True" PopupPosition="Right"
                                                TargetControlID="rfvPayment" />

                                        </div>
                                    </div>
                                </div>
                                <div class="form-section3-blank">
                                    &nbsp;
                                </div>
                                <div class="form-section3">
                                    <div class="text-field col s12" id="divCurrentBalance" runat="server">
                                        <div class="row">
                                            <label style="font-size: 0.9em;">Current Balance </label>
                                            <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblCurrentBal" runat="server"></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlBank" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                                         <asp:AsyncPostBackTrigger ControlID="btnSelectChkBox" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </span>
                                        </div>
                                    </div>

                                    <div class="text-field col s12">
                                        <div class="row">
                                            <label style="font-size: 0.9em;">Requirement</label>
                                            <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <div>
                                                            <asp:Label ID="lblRequirement" runat="server"></asp:Label>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                                         <asp:AsyncPostBackTrigger ControlID="btnSelectChkBox" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-section3-blank">
                                    &nbsp;
                                </div>
                                <div class="form-section3">
                                    <div class="text-field col s12" id="divEndingBalance" runat="server">
                                        <div class="row" >
                                            <label style="font-size: 0.9em;">Ending Balance</label>
                                            <span style="float: right !important; font-size: 0.9em; color: #000 !important;">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblCheckEndingBalance" runat="server"></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlBank" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                                         <asp:AsyncPostBackTrigger ControlID="btnSelectChkBox" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </span>
                                        </div>
                                    </div>

                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvNextCheck" ControlToValidate="txtNextCheck"
                                                        ErrorMessage="Please enter check number" Display="None"
                                                        ValidationGroup="Check"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceNextCheck" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                        TargetControlID="rfvNextCheck" />

                                                    <asp:TextBox ID="txtNextCheck" runat="server" MaxLength="19"
                                                        onkeypress="return isNumberKey(event,this)" autocomplete="off" onchange="return IsExistCheckNo();"></asp:TextBox>
                                                    <asp:Label runat="server" ID="lblCheck" AssociatedControlID="txtNextCheck">Next Check#</asp:Label>

                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlBank" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlPayment" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="pnlCheck" runat="server" class="form-section-row checkdiv">
                                <div class="section-ttle checktitle">
                                    Check 
                                </div>
                                <div class="checkcontent">

                                    <div class="input-field col s11">
                                        <div class="row">
                                            &nbsp;    
                                        </div>
                                    </div>
                                    <div class="input-field col s1">
                                        <asp:Label ID="lblDate" runat="server" AssociatedControlID="txtDate">Date</asp:Label>
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="datepicker_mom" autocomplete="off" Width="80" ValidationGroup="Check"></asp:TextBox>
                                        <%--   <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtDate">
                                                        </asp:CalendarExtender>--%>
                                        <asp:RequiredFieldValidator ID="rfvDate"
                                            runat="server" ControlToValidate="txtDate" Display="None" ErrorMessage="Date is Required" ValidationGroup="Check"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                            TargetControlID="rfvDate" />
                                        <asp:RegularExpressionValidator ID="revDate" ControlToValidate="txtDate" ValidationGroup="Check"
                                            ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="vceDate1" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                            TargetControlID="revDate" />
                                    </div>


                                    <div class="text-field col s10">
                                        <div class="row">

                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <label style="float:left;">Pay to the order of</label>
                                                    <asp:Label ID="lblVendor" runat="server" Text="" CssClass="checklbl"></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>
                                    </div>

                                    <div class="text-field col s2">
                                        <div class="row" style="float:right;">
                                            <asp:UpdatePanel ID="updTotalAmount" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                                     <asp:AsyncPostBackTrigger ControlID="btnSelectChkBox" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                    <div class="text-field col s11">
                                        <div class="row">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblDollar" CssClass="checklbl" runat="server"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnCutCheck" />
                                                     <asp:AsyncPostBackTrigger ControlID="btnSelectChkBox" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                    <div class="text-field col s1">
                                        <div class="row" style="float: right;">
                                            Dollars
                                        </div>
                                    </div>


                                    <div class="input-field col s12">
                                        <div class="row">
                                            <asp:Label ID="lblMemo" runat="server" AssociatedControlID="txtMemo" CssClass="checklbl">Memo</asp:Label>
                                            <asp:TextBox ID="txtMemo" runat="server" autocomplete="off">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>


                            </div>
                            <%-- </ContentTemplate>
                            </asp:UpdatePanel> --%>
                        </div>
                    </div>

                </div>
            </div>






            <div class="grid_container">
                <div class="form-section-row" style="margin-bottom: 0 !important;">
                    <div class="RadGrid RadGrid_Material FormGrid">
                       <%-- <telerik:RadCodeBlock ID="RadCodeBlock_Bills" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= gvBills.ClientID %>");
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
                                    try {
                                        var element = document.getElementById(requestInitiator);
                                        if (element && element.tagName == "INPUT") {
                                            element.focus();
                                            element.selectionStart = selectionStart;
                                        }
                                    } catch (e) {

                                    }
                                    Materialize.updateTextFields();
                                }
                            </script>
                        </telerik:RadCodeBlock>--%>

                        <%--<telerik:RadAjaxPanel ID="RadAjaxPanel_Bills" runat="server" LoadingPanelID="RadAjaxLoadingPanel_WC" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">--%>
                            <%--<telerik:RadGrid RenderMode="Auto" ID="gvBills" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList" ViewStateMode="Enabled"
                                AllowCustomPaging="True" OnNeedDataSource="gvBills_NeedDataSource" OnItemCreated="gvBills_ItemCreated" OnPageIndexChanged="gvBills_PageIndexChanged" >
                                <CommandItemStyle />                                
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="false" EnableAlternatingItems="false" ReorderColumnsOnClient="false">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    <ClientEvents OnCommand="OnGridCommand"  />
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50" ShowFilterIcon="false" UniqueName="ClientSelectColumn">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server"/>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true"/>
                                                <asp:HiddenField ID="hdnPJID" Value='<%# Bind("PJID") %>' runat="server" />
                                                <asp:HiddenField ID="hdnTRID" Value='<%# Bind("TRID") %>' runat="server" />
                                                <asp:HiddenField ID="hdnRef" Value='<%# Bind("Ref") %>' runat="server" />
                                                <asp:HiddenField ID="hdnfDesc" Value='<%# Bind("fDesc") %>' runat="server" />
                                                <asp:HiddenField ID="hdnIsSelected" Value='<%# Bind("IsSelected") %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn Visible="false" HeaderText="Spec" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSpec" runat="server" Text='<%# Bind("Spec")%>'></asp:Label>
                                                <asp:Label ID="lblBillfdesc" runat="server" Text='<%# Bind("billDesc")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Vendor" DataField="Name" SortExpression="Name" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" Text='<%# Bind("Name")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="90" DataField="fDate" SortExpression="fDate" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfDate" runat="server" Text='<%# String.Format("{0:MM/dd/yy}", Eval("fDate"))%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Ref" HeaderStyle-Width="170" FooterStyle-Width="170" DataField="Ref" SortExpression="Ref" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlInvoice" runat="server" Text='<%# Bind("Ref") %>' Target="_blank" NavigateUrl='<%# "addbills.aspx?id=" +Eval("PJID")  %>' ForeColor="#0066CC"></asp:HyperLink>
                                                <asp:Label ID="lblRef" runat="server" Text='<%# Bind("ref") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTo" Style="margin-left: 15px;" runat="server"> Total:-</asp:Label>
                                            </FooterTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn FilterDelay="5" DataField="Due" HeaderText="Due" SortExpression="Due" DataFormatString="{0:MM/dd/yy}" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="90"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>


                                        <telerik:GridTemplateColumn HeaderText="Original" DataField="Original" HeaderStyle-Width="100" FooterStyle-Width="100" SortExpression="Original" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrig" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Original", "{0:c}") %>' DataFormatString="{0:n}"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalOrig" Style="margin-left: 15px;" runat="server" class="cls-payment1"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Balance" DataField="Balance" FooterStyle-Width="100" HeaderStyle-Width="100" SortExpression="Balance" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>'></asp:Label>
                                                
                                                <asp:HiddenField ID="hdnSelected" Value='<%# Bind("Selected") %>' runat="server" />
                                                <asp:HiddenField ID="hdnPrevDue" Value='<%# Bind("Balance") %>' runat="server" />
                                                <asp:HiddenField ID="hdnOriginal" Value='<%# Bind("Original") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalBalance" Style="margin-left: 15px;" class="cls-bal" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Disc" SortExpression="Discount" HeaderStyle-Width="80" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvDisc" runat="server" CssClass="texttransparent" Text='<%# DataBinder.Eval(Container.DataItem, "Discount", "{0:n}") %>' onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                <asp:HiddenField ID="hdnDisc" Value='<%# Bind("Discount") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalDisc" Style="margin-left: 15px;" runat="server" class="cls-disc"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Payment" SortExpression="Payment" HeaderStyle-Width="100" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvPay" runat="server" CssClass="texttransparent" Text='<%# DataBinder.Eval(Container.DataItem, "payment", "{0:n}") %>' onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                               
                                                 </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalPay" Style="margin-left: 15px;" runat="server" class="cls-payment"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn FilterDelay="5" DataField="StatusName" HeaderText="Status" SortExpression="StatusName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

                                    </Columns>
                                </MasterTableView>
                                 <FilterMenu CssClass="RadFilterMenu_CheckList">
                                </FilterMenu>
                            </telerik:RadGrid>--%>
                        <%--</telerik:RadAjaxPanel>

                        <telerik:RadAjaxPanel ID="RadAjaxPanel_gvnobills" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvnobills" ClientEvents-OnRequestStart="requestStartxy" ClientEvents-OnResponseEnd="responseEndxy">--%>
                            <%--<telerik:RadGrid RenderMode="Auto" ID="gvnobills" ShowFooter="True"
                                ShowStatusBar="true" runat="server" AllowSorting="true" Width="100%" OnPreRender="gvnobills_PreRender" OnItemCommand="gvnobills_ItemCommand" onblur="resetIndexF6()">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    <ClientEvents OnKeyPress="KeyPressed" />
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowSorting="false" ShowFooter="True" DataKeyNames="AcctID">
                                    <Columns>
                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Container.DataSetIndex+1 %>'></asp:Label>                                                
                                                
                                                <asp:HiddenField ID="hdnId" Value='<%# Container.DataSetIndex+1 %>' runat="server" />
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                         <telerik:GridTemplateColumn DataField="AcctNo" SortExpression="AcctNo" AutoPostBackOnFilter="true"
                                            HeaderText="Account" ShowFilterIcon="false" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                            <ItemTemplate>

                                                <asp:TextBox ID="txtGvAcctNo" runat="server" CssClass="form-control texttransparent searchinput"
                                                    Width="100%" Height="26px" Text='<%# Bind("AcctNo") %>' Style="font-size: 12px;"></asp:TextBox>
                                                <asp:HiddenField ID="hdnAcctID" Value='<%# Bind("AcctID") %>' runat="server" />
                                                <asp:HiddenField ID="hdnIndex" Value='<%#Container.ItemIndex%>' runat="server" />
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:Label ID="lblTotal" Text="Total" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn DataField="Price" AutoPostBackOnFilter="true" AllowFiltering="false"
                                            CurrentFilterFunction="Contains" HeaderText="Amount" ShowFilterIcon="false" HeaderStyle-Width="80" ItemStyle-Width="80">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvPrice"
                                                    runat="server"
                                                    CssClass="form-control  texttransparent"
                                                    autocomplete="off"
                                                    MaxLength="15"
                                                    onchange="CalTotalVal(this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblAmountPerTotalGrid" runat="server" Style="text-align: left;"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn DataField="fDesc" SortExpression="fDesc" AutoPostBackOnFilter="true"
                                            HeaderText="Memo" ShowFilterIcon="false" HeaderStyle-Width="260" ItemStyle-Width="260">
                                            <ItemTemplate>
                                                
                                                <asp:TextBox ID="txtGvDesc" runat="server" CssClass="materialize-textarea" TextMode="MultiLine"
                                                                                                MaxLength="8000" Style="padding: 0px!important; min-height: 0rem;"
                                                     Text='<%# Bind("fDesc") %>' ></asp:TextBox>
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn SortExpression="delete" AutoPostBackOnFilter="true"
                                            HeaderText="Action" ShowFilterIcon="false" HeaderStyle-Width="70">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibDelete" runat="server" CommandName="DeleteTransaction"
                                                    CommandArgument='<%# Container.DataSetIndex %>'
                                                    ImageUrl="~/images/glyphicons-17-bin.png" Width="13px" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>--%>


                          



                            
                                                                        





                        <%--</telerik:RadAjaxPanel>--%>
                    </div>
                </div>
            </div>













         <div>
                <div class="container accordian-wrap">
                    <div class="row">
                        <div class="col s12 m12 l12">
                            <div class="row">
                                <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">




                                    <li id="liaccrdGlAccount" runat="server">
                                        <div id="accrdGlAccount" runat="server" class="collapsible-header accrd accordian-text-custom "><i class="mdi-action-info"></i>GL Account Info</div>
                                        <div class="collapsible-body">
                                            <div class="form-content-wrap">
                                                <div class="form-content-pd">
                                                     <telerik:RadAjaxPanel ID="RadAjaxPanel_gvDepositGL" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
                                                                        ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <div class="form-section-row">
                                                        <div class="grid_container" style="overflow-y:scroll;">
                                                            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
                                                            </telerik:RadAjaxLoadingPanel>
                                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
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
                                                                   
                                                                        

                                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvJobCostItems" ShowFooter="True"
                                ShowStatusBar="true" runat="server" AllowSorting="true" Width="120%" OnPreRender="RadGrid_gvJobCostItems_PreRender" OnItemCommand="RadGrid_gvJobCostItems_ItemCommand" onblur="resetIndexF6()">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    <ClientEvents OnKeyPress="KeyPressed" />
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowSorting="false" ShowFooter="True" DataKeyNames="JobID">
                                    <Columns>
                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Container.DataSetIndex+1 %>'></asp:Label>
                                                <asp:Label ID="lblTID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                <%--  <asp:HiddenField ID="hdnTID" Value='<%# Bind("ID") %>' runat="server" />--%>
                                                <asp:HiddenField ID="hdnId" Value='<%# Container.DataSetIndex+1 %>' runat="server" />
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="JobID" SortExpression="JobID" AutoPostBackOnFilter="true"
                                            HeaderText="Project" ShowFilterIcon="false" >
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvJob" runat="server" CssClass="form-control texttransparent psearchinput"
                                                    Width="100%" Height="26px" Text='<%# Bind("JobName") %>' Style="font-size: 12px;"></asp:TextBox>
                                                <asp:HiddenField ID="hdnJobID" Value='<%# Bind("JobID") %>' runat="server" />
                                                <asp:HiddenField ID="hdnTID" Value='<%# Bind("ID") %>' runat="server" />
                                                <asp:HiddenField ID="hdnIndex" Value='<%#Container.ItemIndex%>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" Text="Total" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false" SortExpression="Ticket" 
                                            CurrentFilterFunction="Contains" HeaderText="Ticket" ShowFilterIcon="false" HeaderStyle-Width="70" ItemStyle-Width="70">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvTicket" runat="server" CssClass="texttransparent" Text='<%# Bind("Ticket") %>' autocomplete="off" Style="height: 26px; width: 100%; font-size: 12px;"></asp:TextBox>

                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="PhaseID" SortExpression="PhaseID" AutoPostBackOnFilter="true"
                                            HeaderText="Code" ShowFilterIcon="false" HeaderStyle-Width="70" ItemStyle-Width="70">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvPhase" runat="server" CssClass="form-control texttransparent phsearchinput"
                                                    Text='<%# Bind("Phase") %>' Width="100%" Height="26px" Style="font-size: 12px;"></asp:TextBox>
                                                <asp:HiddenField ID="hdnPID" Value='<%# Bind("PhaseID") %>' runat="server" />
                                                <asp:HiddenField ID="hdnTypeId" Value='<%# Bind("TypeID") %>' runat="server" />
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>
                                        <%--<telerik:GridTemplateColumn DataField="ItemID" SortExpression="ItemID" AutoPostBackOnFilter="true" 
                                            HeaderText="Item" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvItem" runat="server" CssClass="form-control texttransparent pisearchinput"
                                                    Width="100%" Height="26px" Text='<%# Bind("ItemDesc") %>' Style="font-size: 12px;"></asp:TextBox>
                                                <asp:HiddenField ID="hdnItemID" Value='<%# Eval("ItemID") == DBNull.Value ? "" : Eval("ItemID")  %>' runat="server" />
                                                <asp:HiddenField ID="hdOpSq" Value='<%# Eval("OpSq")%>' runat="server" />
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>--%>
                                        <telerik:GridTemplateColumn DataField="ItemID" SortExpression="ItemID" AutoPostBackOnFilter="true"
                                            HeaderText="Item" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvItem" runat="server" CssClass="form-control texttransparent pisearchinput"
                                                    Width="100%" Height="26px" Text='<%# Bind("ItemDesc") %>' Style="font-size: 12px;"></asp:TextBox>
                                                <asp:HiddenField ID="hdnItemID" Value='<%# Eval("ItemID") == DBNull.Value ? "" : Eval("ItemID")  %>' runat="server" />
                                                <asp:HiddenField ID="hdOpSq" Value='<%# Eval("OpSq")%>' runat="server" />
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="fDesc" SortExpression="fDesc" AutoPostBackOnFilter="true"
                                            HeaderText="Item Description" ShowFilterIcon="false" HeaderStyle-Width="200" ItemStyle-Width="200">
                                            <ItemTemplate>
                                                <%--<asp:TextBox ID="txtGvDesc" runat="server" CssClass="form-control texttransparent"
                                                    Width="100%" Height="26px" Text='<%# Bind("fDesc") %>' Style="font-size: 12px;"></asp:TextBox>--%>
                                                <asp:TextBox ID="txtGvDesc" runat="server" CssClass="materialize-textarea" TextMode="MultiLine"
                                                                                                MaxLength="8000" Style="padding: 0px!important; min-height: 26px; font-size:12px;"
                                                     Text='<%# Bind("fDesc") %>' ></asp:TextBox>
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="AcctNo" SortExpression="AcctNo" AutoPostBackOnFilter="true"
                                            HeaderText="Acct No." ShowFilterIcon="false" HeaderStyle-Width="150" ItemStyle-Width="150">
                                            <ItemTemplate>

                                                <asp:TextBox ID="txtGvAcctNo" runat="server" CssClass="form-control texttransparent searchinput"
                                                    Width="100%" Height="26px" Text='<%# Bind("AcctNo") %>' Style="font-size: 12px;"></asp:TextBox>
                                                <asp:HiddenField ID="hdnAcctID" Value='<%# Bind("AcctID") %>' runat="server" />

                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Quan" SortExpression="Quan" AutoPostBackOnFilter="true" 
                                            HeaderText="Quan" ShowFilterIcon="false" HeaderStyle-Width="70" ItemStyle-Width="70">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvQuan" runat="server"
                                                    Width="100%" Height="26px"
                                                    Text='<%# Bind("Quan") %>'
                                                    onchange="CalTotalVal(this);"
                                                    Style="font-size: 12px;"></asp:TextBox>
                                                <asp:HiddenField ID="hdnQuantity" Value='<%# Bind("Quan") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalQty" runat="server" Style="text-align: left;"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>


                                        <telerik:GridTemplateColumn DataField="Price" AutoPostBackOnFilter="true" AllowFiltering="false" 
                                            CurrentFilterFunction="Contains" HeaderText="Price" ShowFilterIcon="false" HeaderStyle-Width="100" ItemStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvPrice"
                                                    runat="server" Width="100%" Height="26px"
                                                    CssClass="form-control  texttransparent"
                                                    autocomplete="off"
                                                    MaxLength="15"
                                                    onchange="CalTotalVal(this);"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <%--  Text='<%# Bind("Price") %>'--%>
                                        <telerik:GridTemplateColumn DataField="Amount" SortExpression="Amount" AutoPostBackOnFilter="true"  HeaderStyle-Width="110" ItemStyle-Width="110"
                                            HeaderText="$ Amount" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvAmount"
                                                    runat="server"
                                                    CssClass="form-control texttransparent clsAmount" autocomplete="off"
                                                    MaxLength="15"
                                                    Width="100%"
                                                    DataFormatString="{0:c}"
                                                    onkeypress="return isDecimalKey(this,event);"
                                                    Height="26px" Text='<%# Bind("Amount") %>'
                                                    Style="text-align: right; font-size: 12px;"
                                                    onchange="CalculateTotal(this);"></asp:TextBox>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                                <asp:Label ID="lblAmountPerTotalGrid" runat="server" Style="text-align: left;"></asp:Label>
                                            </FooterTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Amount" SortExpression="Amount" AutoPostBackOnFilter="true"
                                            HeaderText="Use Tax" ShowFilterIcon="false" HeaderStyle-Width="110" ItemStyle-Width="110">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvUseTax" runat="server" CssClass="form-control texttransparent clsUseTax tsearchinput"
                                                    autocomplete="off" Width="100%" Height="26px"
                                                    Text='<%# Eval("UseTax") == DBNull.Value ? "" : Eval("UseTax") %>'
                                                    Style="text-align: right; font-size: 12px;" onblur="CalculateUseTaxTotal(this);" onkeypress="return false;"></asp:TextBox>
                                                <asp:HiddenField ID="hdnUtax" Value='<%# Bind("UName") %>' runat="server" />
                                                <asp:HiddenField ID="hdnUtaxGL" Value='<%# Bind("UtaxGL") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGvUseTaxGrid" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <%-- Sales Tax Columns --%>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="Taxable" UniqueName="Gtax" ItemStyle-CssClass="cent" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100" HeaderStyle-Width="100" FooterStyle-Width="100">
                                                                                        <HeaderTemplate>
                                                                                            <asp:CheckBox ID="chkSelectAllGtax" runat="server"/><span style="padding-left: 10px;">GST</span>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkGTaxable" runat="server" Checked='<%#Convert.ToBoolean(Eval("GTax"))%>'></asp:CheckBox>
                                                                                            
                                                                                            <asp:HiddenField ID="hdnchkGTaxable" Value='<%# Bind("GTax") %>' runat="server" />
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>






                                       

                                                                                    

                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="GST Tax" UniqueName="GTaxAmt" HeaderStyle-Width="100" FooterStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblGstTax" runat="server" Enabled="false"
                                                                                                Style="color: #2392D7; text-align: center;" Height="26px" Text='<%# Eval("GTaxAmt") != DBNull.Value ? Convert.ToDouble(Eval("GTaxAmt")).ToString("N", System.Globalization.CultureInfo.InvariantCulture) : "" %>'
                                                                                                MaxLength="15"></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnGSTTaxAm" Value='<%# Bind("GTaxAmt") %>' runat="server" />
                                                                                            <asp:HiddenField ID="hdnGSTTaxGL" Value='<%# Bind("GSTTaxGL") %>' runat="server" />
                                                                                            <asp:FilteredTextBoxExtender ID="lblGstTax_FilteredTextBoxExtender" runat="server"
                                                                                                Enabled="True" TargetControlID="lblGstTax" ValidChars="1234567890.-">
                                                                                            </asp:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblGstTaxTotal" runat="server" Style="text-align: center;"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                         <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="Taxable" UniqueName="stax" ItemStyle-CssClass="cent" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100" HeaderStyle-Width="100" FooterStyle-Width="100">
                                                                                        <HeaderTemplate>
                                                                                            <asp:CheckBox ID="chkSelectAllStax" runat="server"/><span style="padding-left: 10px;">PST</span>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkTaxable" runat="server" Checked='<%#Convert.ToBoolean(Eval("stax"))%>'></asp:CheckBox>
                                                                                            <asp:HiddenField ID="hdnchkTaxable" Value='<%# Bind("stax") %>' runat="server" />
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                     <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-CssClass="stax-css" UniqueName="STaxAmt" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="100" FooterStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <%--<asp:TextBox ID="lblSalesTax" runat="server" Enabled="false"
                                                                                                Style="color: #2392D7; text-align: center;" Height="26px" Text='<%# Eval("STaxAmt") != DBNull.Value ? Convert.ToDouble(Eval("STaxAmt")).ToString("N", System.Globalization.CultureInfo.InvariantCulture) : "" %>'
                                                                                                MaxLength="15"></asp:TextBox> --%>
                                                                                            <asp:TextBox ID="lblSalesTax" runat="server" 
                                                    autocomplete="off" Width="100%" Height="26px"
                                                    Text='<%# Eval("STaxRate") == DBNull.Value ? "" : Eval("STaxRate") %>'
                                                    Style="display:none;"  onkeypress="return false;" ></asp:TextBox>
                                                    <asp:TextBox ID="txtGvStaxAmount" runat="server" 
                                                    autocomplete="off" Width="100%" Height="26px"
                                                    Text='<%# Eval("STaxAmt") != DBNull.Value ? Convert.ToDouble(Eval("STaxAmt")).ToString("N", System.Globalization.CultureInfo.InvariantCulture) : "" %>'
                                                    onblur="TotalwithTax(this);" onkeypress="return isDecimalKey(this,event);" 
                                                    CssClass="form-control texttransparent clsAmount" 
                                                    MaxLength="15" ></asp:TextBox>
                                                                                            <asp:HiddenField ID="hdnSTaxAm" Value='<%# Bind("STaxAmt") %>' runat="server" />
                                                                                            <asp:HiddenField ID="hdnSTaxGL" Value='<%# Bind("STaxGL") %>' runat="server" />
                                                                                            <asp:FilteredTextBoxExtender ID="lblSalesTax_FilteredTextBoxExtender" runat="server"
                                                                                                Enabled="True" TargetControlID="lblSalesTax" ValidChars="1234567890.-">
                                                                                            </asp:FilteredTextBoxExtender>
                                                                                            <asp:RequiredFieldValidator ID="rfvSalesTax" runat="server" ControlToValidate="lblSalesTax"
                                                                                                Display="None" Enabled="False" ErrorMessage="Sales Tax Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender ID="rfvSalesTax_ValidatorCalloutExtender" runat="server"
                                                                                                Enabled="True" PopupPosition="TopLeft" TargetControlID="rfvSalesTax">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblSalesTaxTotal" runat="server" Style="text-align: center;"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="Amount" UniqueName="Amount" HeaderStyle-CssClass="cent" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" HeaderStyle-Width="100" FooterStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label Style="font-weight: bold; color: #2392D7; text-align: center;" ID="lblAmountWithTax" runat="server"
                                                                                                Text='<%# Eval("AmountTot") != DBNull.Value ? Convert.ToDouble(Eval("AmountTot")).ToString("N", System.Globalization.CultureInfo.InvariantCulture) : "" %>'></asp:Label>
                                                                                            <asp:HiddenField ID="hdnAmountWithTax" Value='<%# Bind("AmountTot") %>' runat="server" />
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblAmountWithTaxTotal" runat="server"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    
                                        <%-- Sales Tax Columns --%>

                                        <telerik:GridTemplateColumn DataField="Amount" SortExpression="Amount" AutoPostBackOnFilter="true"
                                            HeaderText="Location Name" ShowFilterIcon="false" >
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvLoc" runat="server" CssClass="form-control texttransparent jsearchinput"
                                                    Width="100%" Height="26px" Text='<%# Bind("Loc") %>' Style="font-size: 12px;"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="20" ShowFilterIcon="false" Display="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnLine" Value='<%# Bind("Line") %>' runat="server" />
                                                <asp:HiddenField ID="hdnReceive" Value='<%# Bind("Amount") %>' runat="server" />
                                                <asp:HiddenField ID="hdnPrvInQuan" Value='<%# Bind("PrvInQuan") %>' runat="server" />
                                                <asp:HiddenField ID="hdnPrvIn" Value='<%# Bind("PrvIn") %>' runat="server" />
                                                <asp:HiddenField ID="hdnOutstandQuan" Value='<%# Bind("OutstandQuan") %>' runat="server" />
                                                <asp:HiddenField ID="hdnOutstandBalance" Value='<%# Bind("OutstandBalance") %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn SortExpression="delete" AutoPostBackOnFilter="true"
                                            HeaderText="Action" ShowFilterIcon="false" HeaderStyle-Width="70">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibDelete" runat="server" CommandName="DeleteTransaction"
                                                    CommandArgument='<%# Container.DataSetIndex %>'
                                                    ImageUrl="~/images/glyphicons-17-bin.png" Width="13px" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>


                                                                    
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    
                                            
                                            <div class="btnlinks" >
                <%--<asp:LinkButton ID="btnAddNewLines" runat="server" CausesValidation="false" 
                     Text="Add New Lines" OnClientClick="itemJSON();"
                    OnClick="lbtnAddNewLines_Click"></asp:LinkButton>--%>
                <asp:LinkButton ID="btnAddNewLines" runat="server" CausesValidation="False" Style="color: #000; font-size: 1.5em; display:contents; padding: 0" Width="20px"
                ToolTip="Add New Row" OnClientClick="itemJSON();" 
                    OnClick="lbtnAddNewLines_Click" ><i class="mdi-content-add-circle" style="color: #2bab54;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>
                                                
                <asp:LinkButton ID="btnCopyPrevious" runat="server" CausesValidation="false" OnClientClick="itemJSON();" OnClick="btnCopyPrevious_Click"
                     Text="Copy Previous" Style="display: none;"></asp:LinkButton>
            </div>
                                                </telerik:RadAjaxPanel>
                                                    <div class="cf"></div>
                                                </div>
                                                </div>
                                        </div>
                                    </li>





                                    <li id="adEditPayment" runat="server">
                                        <div id="accrdPayment" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-action-info"></i>Bills Info</div>
                                        <div class="collapsible-body">
                                            <div class="form-content-wrap">
                                                <div class="form-content-pd">
                                                    <div class="form-section-row">
                                                        <div class="grid_container">
                                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                               
                                                                <div class="RadGrid RadGrid_Material FormGrid">
                                                                     <telerik:RadCodeBlock ID="RadCodeBlock_Bills" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= gvBills.ClientID %>");
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
                                    try {
                                        var element = document.getElementById(requestInitiator);
                                        if (element && element.tagName == "INPUT") {
                                            element.focus();
                                            element.selectionStart = selectionStart;
                                        }
                                    } catch (e) {

                                    }
                                   // Materialize.updateTextFields();
                                }
                            </script>
                        </telerik:RadCodeBlock>

                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Bills" runat="server" LoadingPanelID="RadAjaxLoadingPanel_WC" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                        <%--<telerik:RadGrid RenderMode="Auto" ID="gvBills" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" FilterType="CheckList" ViewStateMode="Enabled"
                                AllowCustomPaging="True" OnNeedDataSource="gvBills_NeedDataSource" OnItemCreated="gvBills_ItemCreated" OnPageIndexChanged="gvBills_PageIndexChanged" >--%>
                                                                        <telerik:RadGrid RenderMode="Auto" ID="gvBills" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" FilterType="CheckList" ViewStateMode="Enabled"
                                OnNeedDataSource="gvBills_NeedDataSource" OnItemCreated="gvBills_ItemCreated" OnPageIndexChanged="gvBills_PageIndexChanged" >

                                <%--                                             <telerik:RadGrid RenderMode="Auto" ID="gvBills" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true"
                                FilterType="CheckList" AllowCustomPaging="false" OnNeedDataSource="gvBills_NeedDataSource"
                                OnPageIndexChanged="gvBills_PageIndexChanged" OnPageSizeChanged="gvBills_PageSizeChanged" EnableLinqExpressions="false" ShowGroupPanel="false" >--%>

                                <CommandItemStyle />                                
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="false" EnableAlternatingItems="false" ReorderColumnsOnClient="false">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    <ClientEvents OnCommand="OnGridCommand"  />
                                </ClientSettings>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="50" ShowFilterIcon="false" UniqueName="ClientSelectColumn">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" runat="server"/>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true"/>
                                                <asp:HiddenField ID="hdnPJID" Value='<%# Bind("PJID") %>' runat="server" />
                                                <asp:HiddenField ID="hdnTRID" Value='<%# Bind("TRID") %>' runat="server" />
                                                <asp:HiddenField ID="hdnRef" Value='<%# Bind("Ref") %>' runat="server" />
                                                <asp:HiddenField ID="hdnfDesc" Value='<%# Bind("fDesc") %>' runat="server" />
                                                <asp:HiddenField ID="hdnIsSelected" Value='<%# Bind("IsSelected") %>' runat="server" />
                                                <asp:HiddenField ID="hdnbillspec" Value='<%# Bind("Spec") %>' runat="server" />
                                                <asp:HiddenField ID="hdnbillspecstatus" Value='<%# Bind("StatusName") %>' runat="server" />                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn Visible="false" HeaderText="Spec" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSpec" runat="server" Text='<%# Bind("Spec")%>'></asp:Label>
                                                <asp:Label ID="lblBillfdesc" runat="server" Text='<%# Bind("billDesc")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Vendor" UniqueName="Name" HeaderStyle-Width="170" DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendor" runat="server" Text='<%# Bind("Name")%>'></asp:Label>
                                                <asp:HiddenField ID="hdnbillvendorid" Value='<%# Bind("Vendor") %>' runat="server" />                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="VendorType" UniqueName="Type" HeaderStyle-Width="170" DataField="Type" SortExpression="Type" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorType" runat="server" Text='<%# Bind("VendorType")%>'></asp:Label>                                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Date" UniqueName="fDate" HeaderStyle-Width="90" DataField="fDate" SortExpression="fDate" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfDate" runat="server" Text='<%# String.Format("{0:MM/dd/yy}", Eval("fDate"))%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Ref" UniqueName="Ref" HeaderStyle-Width="150" FooterStyle-Width="170" DataField="Ref" SortExpression="Ref" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlInvoice" runat="server" Text='<%# Bind("Ref") %>' Target="_blank" NavigateUrl='<%# "addbills.aspx?id=" +Eval("PJID")  %>' ForeColor="#0066CC"></asp:HyperLink>
                                                <asp:Label ID="lblRef" runat="server" Text='<%# Bind("ref") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTo" Style="margin-left: 15px;" runat="server"> Total:-</asp:Label>
                                            </FooterTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="Due" UniqueName="Due" HeaderText="Due" SortExpression="Due" DataFormatString="{0:MM/dd/yy}" DataType="System.String"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="90"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>


                                        <telerik:GridTemplateColumn HeaderText="Original" DataField="Original" UniqueName="Original" HeaderStyle-Width="100" FooterStyle-Width="100" SortExpression="Original"  AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrig" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Original", "{0:c}") %>' DataFormatString="{0:n}"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalOrig" Style="margin-left: 15px;" runat="server" class="cls-payment1"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Balance" DataField="Balance" UniqueName="Balance" FooterStyle-Width="100" HeaderStyle-Width="100" SortExpression="Balance"  AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>'></asp:Label>
                                                
                                                <asp:HiddenField ID="hdnSelected" Value='<%# Bind("Selected") %>' runat="server" />
                                                <asp:HiddenField ID="hdnPrevDue" Value='<%# Bind("Balance") %>' runat="server" />
                                                <asp:HiddenField ID="hdnOriginal" Value='<%# Bind("Original") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalBalance" Style="margin-left: 15px;" class="cls-bal" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Disc" DataField="Discount" UniqueName="Discount" SortExpression="Discount" HeaderStyle-Width="80" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvDisc" runat="server" CssClass="texttransparent" Text='<%# DataBinder.Eval(Container.DataItem, "Discount", "{0:n}") %>' onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                <asp:HiddenField ID="hdnDisc" Value='<%# Bind("Discount") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalDisc" Style="margin-left: 15px;" runat="server" class="cls-disc"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Payment" SortExpression="Payment" DataField="Payment" UniqueName="Payment" HeaderStyle-Width="100" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvPay" runat="server" CssClass="texttransparent" Text='<%# DataBinder.Eval(Container.DataItem, "payment", "{0:n}") %>' onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                               
                                                 </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalPay" Style="margin-left: 15px;" runat="server" class="cls-payment"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Description" UniqueName="fDesc" HeaderStyle-Width="230" DataField="fDesc" SortExpression="fDesc" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataType="System.String">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvDesc" runat="server" Text='<%# Bind("billDesc")%>' ></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="StatusName" UniqueName="StatusName" HeaderText="Status" SortExpression="StatusName"
                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="100"
                                            ShowFilterIcon="false">
                                        </telerik:GridBoundColumn>

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
    <%--</div>--%>

    <telerik:RadWindowManager ID="RadWindowManagerWC" runat="server">
        <Windows>

            <telerik:RadWindow ID="RadWindowAutomaticSelectionForPayment" Skin="Material" VisibleTitlebar="true" Title="Automatic Selection For Payment" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="500" Height="400">
                <ContentTemplate>
                    <div>
                        <div>
                            <div class="col-lg-12 col-md-12" style="padding-left: 0px; padding-right: 0px;">
                                <div class="section-ttle">
                                    Select For Payment Options
                                </div>
                                <div class="com-cont">

                                    <div style="clear: both;"></div>


                                    <div class="rd-flt">
                                        <input id="rdDue" class="with-gap" name="radio-group" type="radio" checked value="rdDue">
                                        <label for="rdDue" class="radio-gap-label">All due items, based on due date</label>
                                    </div>
                                    <div style="clear: both;"></div>
                                    <div class="rd-flt">
                                        <input id="rdDated" class="with-gap" name="radio-group" type="radio" value="rdDated">
                                        <label for="rdDated" class="radio-gap-label">All items dated on or before</label>
                                        <div class="fc-input" style="padding-left: 33px">
                                            <asp:TextBox ID="txtdated" runat="server" CssClass="datepicker_wc" MaxLength="10"
                                                autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                            <%--   <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                TargetControlID="txtdated">
                                            </asp:CalendarExtender>--%>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                    <div class="rd-flt">
                                        <input id="rdDateBefore" class="with-gap" name="radio-group" type="radio" value="rdDateBefore">
                                        <label for="rdDateBefore" class="radio-gap-label">All items due on or before</label>
                                        <div class="fc-input" style="padding-left: 33px">
                                            <asp:TextBox ID="txtDateBefore" runat="server" CssClass="datepicker_wc" MaxLength="10"
                                                autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                            <%-- <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                TargetControlID="txtDateBefore">
                                            </asp:CalendarExtender>--%>
                                        </div>
                                    </div>
                                    <div style="clear: both;"></div>
                                    <div class="rd-flt">
                                        <input id="rdRegard" class="with-gap" name="radio-group" type="radio" value="rdRegard">
                                        <label for="rdRegard" class="radio-gap-label">All items, regardless of due date</label>
                                    </div>
                                    <div style="clear: both;"></div>
                                    <div class="rd-flt">
                                        <input id="rdClear" class="with-gap" name="radio-group" type="radio" value="rdClear">
                                        <label for="rdClear" class="radio-gap-label">Clear all items</label>
                                    </div>
                                    <div class="clearfix"></div>

                                    <div class="rd-flt">
                                        <div style="margin-top: 10px; margin-bottom: 10px; margin-left: 7px;">
                                            <input id="chkVH" class="css-checkbox" name="checkbox-group" type="checkbox" value="chkVH">
                                            <label for="chkVH" class="css-label">Verified or Selected</label>
                                        </div>
                                        <div style="clear: both;"></div>
                                        <div style="margin-bottom: 10px; margin-left: 7px;">
                                            <input id="chkDisc" class="css-checkbox" name="checkbox-group" type="checkbox" value="chkDisc">
                                            <label for="chkDisc" class="css-label">Take Discounts where Available</label>
                                        </div>

                                    </div>

                                </div>
                            </div>

                        </div>
                        <div style="clear: both;"></div>
                        <div class="btnlinks">
                            <asp:LinkButton ID="lnkSaveAutopayment" runat="server" OnClick="lnkSaveAutopayment_Click" CausesValidation="false">Save</asp:LinkButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowTemplates" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1100" OnClientClose="OnClientCloseHandler" Title="Check Templates">
                <ContentTemplate>
                    <div>
                        <div class='col s5' style="width: 100%; float: left;">
                            <div class='cr-title' style="    padding-top: 5px;   font-size: initial;   padding-bottom: 5px;">Select a check template. Please note checks will be saved after you exit this screen. </div>
                        </div>
                        <div class='col s5' style="width: 30%; float: left; padding-top: 5px;  margin-bottom: 15px;margin-right: 30px;">
                            <div class='cr-box'>
                                <div class='cr-title'>AP – check top </div>
                                <%--<div class='cr-img'>
                                    <asp:Label ID="lbltopcom" runat="server" Text="XYZ Company" style="position: absolute; padding-left: 20px; padding-top: 15px; font-weight: bolder; font-size: 12px;"></asp:Label>
                                    <asp:Label ID="lbltopdd" runat="server" Text="9418 Galvin Ave, ,San Diago, Suit #100" style="position: absolute; padding-left: 20px; padding-top: 60px; font-size: 10px;" Visible="false"></asp:Label>
                                    <asp:Label ID="lbltopemail" runat="server" Text="info@expertservicesolution.com" style="position: absolute; padding-left: 20px; padding-top: 80px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                            
                                </div>
                                <div class='cr-img'>
                                    <img src='images/ReportImages/ApTopCheck.jpg' alt='' style="position: absolute;margin-top: 40px;height: 265px;width: 320px;">
                                </div>--%>
                                 <div class='cr-date' >
                                    <div class='cr-iocn'>
                                        <asp:DropDownList ID="ddlApTopCheckForLoad" runat="server"
                                            CssClass="browser-default" OnSelectedIndexChanged="ddlApTopCheckForLoad_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <div style="margin-left: 30%;">
                                            <asp:ImageButton ID="imgPrintTemp1" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp1_Click" ToolTip="Export to PDF" />
                                            <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="25px" Width="25px" OnClick="ImageButton7_Click" ToolTip="Edit Template" />
                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkSaveDefault_Click" ToolTip="Set as Default" />
                                            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" OnClick="ImageButton3_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div style="clear: both;"></div>
                        </div>
                          <div class='col s5' style="width: 30%; float: left; padding-top: 5px;  margin-bottom: 15px;margin-right: 30px;">
                            <div class='cr-box'>
                                <div class='cr-title'>AP – check middle </div>
                                <%--<div class='cr-img'>
                                    <asp:Label ID="lblmidcom" runat="server" Text="XYZ Company" style="position: absolute; padding-left: 20px; padding-top: 15px; font-weight: bolder; font-size: 12px;"></asp:Label>
                                    <asp:Label ID="lblmidadd" runat="server" Text="9418 Galvin Ave, ,San Diago, Suit #100" style="position: absolute; padding-left: 20px; padding-top: 60px; font-size: 10px;" Visible="false"></asp:Label>
                                    <asp:Label ID="lblmidemail" runat="server" Text="info@expertservicesolution.com" style="position: absolute; padding-left: 20px; padding-top: 80px; font-size: 10px;" Visible="false"></asp:Label>
                                </div>
                                <div class='cr-img'>
                                    <img src='images/ReportImages/MidCheck.jpg' alt='' style="position: absolute;margin-top: 40px;height: 265px;width: 320px;">
                                </div>--%>
                                <div class='cr-date' >

                                    <asp:DropDownList ID="ddlApMiddleCheckForLoad" runat="server"
                                        CssClass="browser-default" OnSelectedIndexChanged="ddlApMiddleCheckForLoad_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <div class='cr-iocn' style="margin-left: 30%;">
                                        <asp:ImageButton ID="imgPrintTemp2" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp2_Click" ToolTip="Export to PDF" />
                                        <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="30px" Width="30px" OnClick="ImageButton8_Click" ToolTip="Edit Template" />
                                        <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkSaveApMiddleCheck_Click" ToolTip="Set as Default" />
                                        <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" OnClick="ImageButton6_Click" />

                                    </div>
                                </div>
                            </div>
                            <div style="clear: both;"></div>
                        </div>
                          <div class='col s5' style="width: 30%; float: left; padding-top: 5px; margin-bottom: 15px;margin-right: 30px;">
                            <div class='cr-box'>
                                <div class='cr-title'>AP – Detailed check top </div>
                                <%--<div class='cr-img'>
                                    <asp:Label ID="lbldetailcom" runat="server" Text="XYZ Company" style="position: absolute; padding-left: 20px; padding-top: 15px; font-weight: bolder; font-size: 12px;"></asp:Label>
                                    <asp:Label ID="lbldetailadd" runat="server" Text="9418 Galvin Ave, ,San Diago, Suit #100" style="position: absolute; padding-left: 20px; padding-top: 60px; font-size: 10px;" Visible="false"></asp:Label>
                                    <asp:Label ID="lbldetailemail" runat="server" Text="info@expertservicesolution.com" style="position: absolute; padding-left: 20px; padding-top: 80px; font-size: 10px;" Visible="false"></asp:Label>
                               </div>
                               <div class='cr-img'>
                                   <img src='images/ReportImages/TopDetailCheck.jpg' alt='' style="position: absolute;margin-top: 40px;height: 265px;width: 320px;">
                               </div>--%>
                               <div class='cr-date' >
                                    <asp:DropDownList ID="ddlTopChecksForLoad" runat="server"
                                        CssClass="browser-default" OnSelectedIndexChanged="ddlTopChecksForLoad_SelectedIndexChanged">
                                    </asp:DropDownList>

                                    <div class='cr-iocn' style="margin-left: 30%;">
                                        <asp:ImageButton ID="imgPrintTemp6" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp6_Click" ToolTip="Export to PDF" />
                                        <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="30px" Width="30px" OnClick="ImageButton9_Click" ToolTip="Edit Template" />
                                        <asp:ImageButton ID="ImageButton13" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkTopChecks_Click" />
                                        <asp:ImageButton ID="ImageButton14" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" OnClick="ImageButton14_Click" />

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>
                    </div>
                    <div class="btnlinks">
                        <asp:LinkButton ID="btnSave2" runat="server" Visible="false" ValidationGroup="Check" CausesValidation="true" OnClick="btnSubmit_Click">
                                           Cut Check
                        </asp:LinkButton>
                        <asp:Label ID="txtMessage" runat="server" ForeColor="Green"></asp:Label>
                    </div>
                </ContentTemplate>

            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowFirstReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1200" Height="700" OnClientClose="OnClientCloseHandler">
                <ContentTemplate>
                    <div class="rptSti">
                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner1" runat="server" OnSaveReport="StiWebDesigner1_SaveReport" Height="700" Width="100%" OnSaveReportAs="StiWebDesigner1_SaveReportAs" OnExit="StiWebDesigner1_Exit" />
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowSecondReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1200" Height="700" OnClientClose="OnClientCloseHandler">
                <ContentTemplate>
                    <div class="rptSti">
                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner2" runat="server" OnSaveReport="StiWebDesigner2_SaveReport" Height="700" Width="100%" OnSaveReportAs="StiWebDesigner2_SaveReportAs" OnExit="StiWebDesigner2_Exit" />
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowThirdReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1200" Height="700" OnClientClose="OnClientCloseHandler">
                <ContentTemplate>
                    <div class="rptSti">
                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner3" runat="server" OnSaveReport="StiWebDesigner3_SaveReport" Height="700" Width="100%" OnSaveReportAs="StiWebDesigner3_SaveReportAs" OnExit="StiWebDesigner3_Exit" />
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowWarehouse" Skin="Material" VisibleTitlebar="true" Title="Select Op Squence" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="300" Height="200" OnClientClose="OnClientCloseHandler1">
                <ContentTemplate>
                    <div id="opDiv">
                    </div>
                    <div style="clear: both;"></div>

                    <div class="btnlinks">
                        <a href="javascript:void(0);" onclick="SetOpToHiddenField();">Save Changes</a>
                        &nbsp;&nbsp;
                         <a href="javascript:void(0);" onclick="SetOpToHiddenPop();">Cancel</a>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="ReprintCheckRange" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                                                    Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                                                    runat="server" Modal="true" Width="200" Height="180">
                                                                    <ContentTemplate>
                                                                        <div style="margin-top: 15px;">
                                                                            <div class="form-section-row">
                                                                                <div class="form-section">
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label class="drpdwn-label">Apply Date </label>
                                                                                                 <%--<label for="rdDated" class="radio-gap-label">Apply Date</label>--%>
                                        <div class="fc-input" >
                                            <asp:TextBox ID="txtaplyDate" runat="server" CssClass="datepicker_wc" MaxLength="10"
                                                autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                            <%--   <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                TargetControlID="txtdated">
                                            </asp:CalendarExtender>--%>
                                        </div>
                                                                                                <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlBank"
                                                                                                    ErrorMessage="Please select Bank" Display="None" InitialValue="0"
                                                                                                    ValidationGroup="Check"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True" PopupPosition="Right"
                                                                                                    TargetControlID="rfvBank" />--%>                                    </div>
                                                                                    </div>
                                                                                </div>
                                                                                <%--<div class="form-section3-blank">
                                                                                    &nbsp;
                                                                                </div>--%>
                                                                                
                                                                            </div>
                                                                            <div style="clear: both;"></div>
                                                                            <footer style="float: left; padding-left: 0 !important; margin-top: -30px;">
                                                                                <div class="btnlinks">
                                                                                    <asp:LinkButton ID="lnkApplyCredit" runat="server" OnClientClick="CloseApplyCreditModal();" OnClick="btnApplyCredit_Click" CausesValidation="false" AutoPostBack="true">Apply Credit</asp:LinkButton>
                                                                                </div>
                                                                            </footer>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </telerik:RadWindow>

        </Windows>
    </telerik:RadWindowManager>
    <asp:HiddenField runat="server" ID="hdnSelectPOIndex" />
    <asp:HiddenField ID="hdnRowField" runat="server" />
    <asp:HiddenField ID="hdnBatch" runat="server" />
    <asp:HiddenField ID="hdnTransID" runat="server" />
    <asp:HiddenField ID="hdnStatus" runat="server" />
    <asp:HiddenField ID="hdnGLItem" runat="server" />
    <asp:HiddenField runat="server" ID="hdnInvDefaultAcctID" Value="" />
    <asp:HiddenField runat="server" ID="hdnInvDefaultAcctName" Value="" />
    <asp:HiddenField runat="server" ID="hdOpSeqID" />
    <asp:HiddenField runat="server" ID="hdLineNo" />
    <asp:HiddenField ID="hdnIsAutoCompleteSelected"  ClientIDMode="Static" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script defer src="https://use.fontawesome.com/releases/v5.0.10/js/all.js"></script>
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

            $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetIsSalesTaxAPBill",
                        //data: '{"prefixText": "' + dtaaa.prefixText + '", "IsJob": "' + false + '", "con": "' + dtaaa.con + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            //response($.parseJSON(data.d));
                            var ui = JSON.parse(data.d);

                                if (ui.length > 0) {
                                        var IsSalesTaxAPBill = ui[0].IsSalesTaxAPBill ;
                                        var IsUseTaxAPBill = ui[0].IsUseTaxAPBill;
                                        if (IsSalesTaxAPBill == "1") 
                                        {
                                            
                                            //document.getElementById('txtgstgv').style.display = 'block';
                                            document.getElementById('txtqstgv').style.display = 'block';
                                        } else {
                                        
                                            
                                            //document.getElementById('txtgstgv').style.display = 'none';
                                            document.getElementById('txtqstgv').style.display = 'none';
                                    }

                                    if (IsUseTaxAPBill == "1") 
                                        {
                                            document.getElementById('txttaxcodegv').style.display = 'block';
                                            
                                        } else {
                                        
                                            document.getElementById('txttaxcodegv').style.display = 'none';
                                            
                                        }
                                    //$(txtGvAcctName).val(ui[0].DefaultAcct);
                                }
                            

                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load phase details");
                        }
                    });
            
            var bqs = GetParameterValues('bill');
            var vqs = GetParameterValues('vid');
            var rqs = GetParameterValues('ref');
            if (bqs == 'c') {
                
                ValidatorEnable($('#<%=rfvtxtvendor.ClientID %>')[0], true);
                
                var tAmount = 0.00;
                if ($find("<%=RadGrid_gvJobCostItems.ClientID%>") != null) {
                    var masterTable = $find("<%=RadGrid_gvJobCostItems.ClientID%>").get_masterTableView();
                    var count = masterTable.get_dataItems().length;
                    var item;
                    for (var i = 0; i < count; i++) {
                        item = masterTable.get_dataItems()[i];
                        var Qty = item.findElement("txtGvQuan");
                        var Amount = item.findElement("txtGvAmount");
                        var Price = item.findElement("txtGvPrice");
                        var QtyVal = $(Qty).val();
                        var AmountVal = $(Amount).val();
                        if (QtyVal != "" && AmountVal != "") {
                            if (!isNaN(parseFloat(QtyVal)) && !isNaN(parseFloat(AmountVal)) && parseFloat(QtyVal) != 0) {
                                var QtyPrice = parseFloat(AmountVal) / parseFloat(QtyVal);
                                tAmount = tAmount + parseFloat(AmountVal);
                            }
                        }
                    }
                }
                if (parseFloat(tAmount) > 0) {
                    ValidatorEnable($('#<%=rfvbillref.ClientID %>')[0], true);
                ValidatorEnable($('#<%=rfvMemo1.ClientID %>')[0], true);
                }
                else {
                    ValidatorEnable($('#<%=rfvbillref.ClientID %>')[0], false);
                ValidatorEnable($('#<%=rfvMemo1.ClientID %>')[0], false);
                }


            }
            else {
                
                ValidatorEnable($('#<%=rfvtxtvendor.ClientID %>')[0], false);
                ValidatorEnable($('#<%=rfvbillref.ClientID %>')[0], false);
                ValidatorEnable($('#<%=rfvMemo1.ClientID %>')[0], false);
                
                

            }
            if (bqs != null && vqs != null && rqs != null) {

                 
                
            
                
                var totalpay = 0;
                var tPay = 0;
                var tDisc = 0;
                var tBal = 0;
                var tPay1 = 0;
                var tDisc1 = 0;
                var tBal1 = 0;
                var tDisc = 0;
                var tvendors = 0;
                var titems = 0;
                $("#<%=gvBills.ClientID %>").find('tr:not(:first, :last)').each(function () {
                    var $tr = $(this);
                    
                    var chk = $tr.find('input[id*=chkSelect]');
                    var hlInvoice = $tr.find('[id*=hlInvoice]').text();
                    if (rqs == hlInvoice) {
                        
                        //if (ret == true) {
                        //    $tr.css('background-color', '#c3dcf8');
                        //    chk.prop('checked', true);
                        //}
                        //else {
                        //    chk.prop('checked', false);
                        //    $tr.removeAttr("style");
                        //}
                        chk.prop('checked', true);
                        $tr.css('background-color', '#c3dcf8');
                        var hdnSelected = $tr.find('[id*=hdnSelected]');
                        var hdnOriginal = $tr.find('[id*=hdnOriginal]');
                        var hdnPrevDue = $tr.find('[id*=hdnPrevDue]');
                        var lblDue = $tr.find('[id*=lblBalance]');
                        var txtDisc = $tr.find('input[id*=txtGvDisc]');
                        var hdnDisc = $tr.find('[id*=hdnDisc]');
                        var txtPay = $tr.find('input[id*=txtGvPay]');

                        var pay = $(txtPay).val().toString().replace(/[\$\(\),]/g, '');
                        var disc = $(txtDisc).val().toString().replace(/[\$\(\),]/g, '');

                        var OrgDisc = parseFloat($(hdnDisc).val());
                        var rOrgDisc = OrgDisc.toLocaleString("en-US", { minimumFractionDigits: 2 });
                        //////////////////////
                        var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''));
                        //var prevDue = parseFloat($(hdnOriginal).val() - $(hdnSelected).val());
                        var prevDue = parseFloat($(hdnOriginal).val() - $(hdnSelected).val() - $(hdnDisc).val());
                        var pay = 0;

                        var rpay = pay.toLocaleString("en-US", { minimumFractionDigits: 2 });
                        var rprevDue = prevDue.toLocaleString("en-US", { minimumFractionDigits: 2 });


                        //if ($(this).prop('checked') == true) {

                            $(txtPay).val(rprevDue)
                            $(txtDisc).val(rOrgDisc)
                            //$(txtDisc).val('0.00')
                            $(lblDue).text(cleanUpCurrency('$' + rpay))
                            SelectedRowStyle('<%=gvBills.ClientID %>')
                        //}
                        

                        CalculatePayTotal();
                        CalculatePayTotalSelected();
                        document.getElementById('<%=btnSelectChkBox.ClientID%>').click();
                    }

                    // if ($tr.find('input[id*=txtGvPay]').attr('id') != "" && typeof $tr.find('input[id*=txtGvPay]').attr('id') != 'undefined') {
                    //var payments = $tr.find('input[id*=txtGvPay]').val().replace(/[\$\(\),]/g, '');

                    //if (!isNaN(parseFloat(payments))) {
                    //    totalpay += parseFloat(payments);
                    //}
                //}


                });
                //$('#<%=lblTotalAmount11.ClientID%>').text(totalpay.toFixed(2));






                

            }

        });

        function SetOpToHiddenPop() {
            var selectedLineNo = $("#<%=hdLineNo.ClientID%>").val();
            var lineItem = $("#lineItem999").val();
            $("#" + selectedLineNo).val(lineItem);

            var radwindow = $find('<%=RadWindowWarehouse.ClientID %>');
            radwindow.close();
        }

        function OnClientCloseHandler1(sender, args) {
            var selectedLineNo = $("#<%=hdLineNo.ClientID%>").val();
            var lineItem = $("#lineItem999").val();
            $("#" + selectedLineNo).val(lineItem);
        }

        function SetOpToHiddenField() {
            var selectedRow = $("#<%=hdOpSeqID.ClientID%>").val();
            var selectedLineNo = $("#<%=hdLineNo.ClientID%>").val();
            var selectedCode = $('input[name=opSquence]:checked').val();
            $("#" + selectedRow).val(selectedCode);
            var lineItem = $("#lineItem" + selectedCode).val();
            //alert(lineItem);
            $("#" + selectedLineNo).val(lineItem);

            var radwindow = $find('<%=RadWindowWarehouse.ClientID %>');
            radwindow.close();
            $("#" + selectedLineNo).val(lineItem);
        }


        function pageLoad(sender, args) {
            $("#<%=txtdated.ClientID %>").hide();
            $("#<%=txtDateBefore.ClientID %>").hide();

            $('input[name="radio-group"]').change(function () {           
                if (document.getElementById('rdDateBefore').checked) {
                    $("#<%=txtDateBefore.ClientID %>").show();
                    $("#<%=txtdated.ClientID %>").hide();
                }
                if (document.getElementById('rdDated').checked) {
                    $("#<%=txtdated.ClientID %>").show();
                    $("#<%=txtDateBefore.ClientID %>").hide();

                }
                if (document.getElementById('rdDue').checked) {
                    $("#<%=txtdated.ClientID %>").hide();
                    $("#<%=txtDateBefore.ClientID %>").hide();

                }
                if (document.getElementById('rdClear').checked) {
                    $("#<%=txtdated.ClientID %>").hide();
                    $("#<%=txtDateBefore.ClientID %>").hide();
                }
                if (document.getElementById('rdRegard').checked) {
                    $("#<%=txtdated.ClientID %>").hide();
                    $("#<%=txtDateBefore.ClientID %>").hide();

                }

            });

            $('.datepicker_wc').pikaday({
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(2000, 1, 1),
                maxDate: new Date(2100, 12, 31),
                yearRange: [2000, 2100]
            });


            $("[id*=chkSelect]").change(function () {
                //debugger;
                try {
                    var chk = $(this).attr('id');
                    // debugger;
                    /////////////////////
                    //var txtPay = $(this).parent().find('input[id$="txtGvPay"]').attr('id');
                    //var txtDisc = $(this).parent().next().next().next().next().find('input[id$="txtGvDisc"]').attr('id');
                    //var lblDue = $(this).parent().next().next().next().find('span[id$="lblBalance"]').attr('id');
                    //var hdnPrevDue = $(this).parent().next().next().next().find('input:hidden[id$="hdnPrevDue"]').attr('id');


                    var hdnSelected = document.getElementById(chk.replace('chkSelect', 'hdnSelected'));
                    var hdnOriginal = document.getElementById(chk.replace('chkSelect', 'hdnOriginal'));
                    var hdnPrevDue = document.getElementById(chk.replace('chkSelect', 'hdnPrevDue'));
                    var lblDue = document.getElementById(chk.replace('chkSelect', 'lblBalance'))
                    var txtDisc = document.getElementById(chk.replace('chkSelect', 'txtGvDisc'));
                    var hdnDisc = document.getElementById(chk.replace('chkSelect', 'hdnDisc'));
                    var txtPay = document.getElementById(chk.replace('chkSelect', 'txtGvPay'));

                    var pay = $(txtPay).val().toString().replace(/[\$\(\),]/g, '');
                    var disc = $(txtDisc).val().toString().replace(/[\$\(\),]/g, '');

                    var OrgDisc = parseFloat($(hdnDisc).val());
                    var rOrgDisc = OrgDisc.toLocaleString("en-US", { minimumFractionDigits: 2 });
                    //////////////////////
                    var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''));
                    //var prevDue = parseFloat($(hdnOriginal).val() - $(hdnSelected).val());
                    var prevDue = parseFloat($(hdnOriginal).val() - $(hdnSelected).val()-$(hdnDisc).val());
                    var pay = 0;

                    var rpay = pay.toLocaleString("en-US", { minimumFractionDigits: 2 });
                    var rprevDue = prevDue.toLocaleString("en-US", { minimumFractionDigits: 2 });

                    
                    if ($(this).prop('checked') == true) {

                        $(txtPay).val(rprevDue)
                        $(txtDisc).val(rOrgDisc)
                        //$(txtDisc).val('0.00')
                        $(lblDue).text(cleanUpCurrency('$' + rpay))
                        SelectedRowStyle('<%=gvBills.ClientID %>')
                    }
                    else if ($(this).prop('checked') == false) {
                        $(txtPay).val(rpay)
                        //$(txtDisc).val(rpay)
                        $(txtDisc).val(rOrgDisc)
                        $(lblDue).text(cleanUpCurrency('$' + rprevDue))
                        $(this).closest('tr').removeAttr("style");
                    }
                    
                    CalculatePayTotal();
                    CalculatePayTotalSelected();
                    document.getElementById('<%=btnSelectChkBox.ClientID%>').click();
                } catch (e) {

                }
            });

            $("[id*=chkSelectAll]").change(function () {
                //debugger;
                var ret = $(this).prop('checked');
                var tPay = 0;
                var tDisc = 0;
                var tBal = 0;
                var tPay1 = 0;
                var tDisc1 = 0;
                var tBal1 = 0;
                var tDisc = 0;
                var tvendors = 0;
                var titems = 0;
                  $("#<%=gvBills.ClientID %>").find('tr:not(:first, :last)').each(function () {
                    var $tr = $(this);
                    var chk = $tr.find('input[id*=chkSelect]');
                    if (ret == true) {
                        $tr.css('background-color', '#c3dcf8');
                        chk.prop('checked', true);
                    }
                    else {
                        chk.prop('checked', false);
                        $tr.removeAttr("style");
                    }

                    var ch_id = chk.attr('id');
                    if (ch_id != undefined) {
                        if (ch_id != "ctl00_ContentPlaceHolder1_gvBills_ctl00_ctl02_ctl00_chkSelectAll") {
                            CalGrid(ch_id);
                        }
                    }
                    if ($tr.find('input[id*=txtGvPay]').attr('id') != "" && typeof $tr.find('input[id*=txtGvPay]').attr('id') != 'undefined') {
                        var payment = $tr.find('input[id*=txtGvPay]').val().replace(/[\$\(\),]/g, '');

                        if (!isNaN(parseFloat(payment))) {
                            tPay += parseFloat(payment);
                            
                        }
                    }


                    if ($tr.find('input[id*=txtGvDisc]').attr('id') != "" && typeof $tr.find('input[id*=txtGvDisc]').attr('id') != 'undefined') {
                        var disc = $tr.find('input[id*=txtGvDisc]').val().replace(/[\$\(\),]/g, '');

                        if (!isNaN(parseFloat(disc))) {
                            tDisc += parseFloat(disc);
                        }
                    }

                    if ($tr.find('[id*=lblBalance]').attr('id') != "" && typeof $tr.find('[id*=lblBalance]').attr('id') != 'undefined') {
                        //var bal = $tr.find('[id*=lblBalance]').text().replace(/[\$\(\),]/g, '');
                        var bal = $tr.find('[id*=lblBalance]').text().replace(/[\$\,]/g, '');
                    if (bal.includes('(')) { 
                        bal = bal.replace(/[\$\(\),]/g, '');
                        bal = -bal;
                    }

                        if (!isNaN(parseFloat(bal))) {
                            tBal += parseFloat(bal);
                        }
                    }
                    if ($tr.find('input[id*=txtGvPay]').attr('id') != "" && typeof $tr.find('input[id*=txtGvPay]').attr('id') != 'undefined') {
                        var payment = $tr.find('input[id*=txtGvPay]').val().replace(/[\$\(\),]/g, '');

                        if (!isNaN(parseFloat(payment))) {
                            tPay1 += parseFloat(payment);
                        }
                    }
                    if ($tr.find('input[id*=txtGvDisc]').attr('id') != "" && typeof $tr.find('input[id*=txtGvDisc]').attr('id') != 'undefined') {
                        var disc = $tr.find('input[id*=txtGvDisc]').val().replace(/[\$\(\),]/g, '');

                        if (!isNaN(parseFloat(disc))) {
                            tDisc1 += parseFloat(disc);
                        }
                    }

                    if ($tr.find('[id*=lblBalance]').attr('id') != "" && typeof $tr.find('[id*=lblBalance]').attr('id') != 'undefined') {
                        //var bal = $tr.find('[id*=lblBalance]').text().replace(/[\$\(\),]/g, '');
                         var bal = $tr.find('[id*=lblBalance]').text().replace(/[\$\,]/g, '');
                    if (bal.includes('(')) { 
                        bal = bal.replace(/[\$\(\),]/g, '');
                        bal = -bal;
                    }
                        if (!isNaN(parseFloat(bal))) {
                            tBal1 += parseFloat(bal);
                        }
                    }
                }) 
                var _currencyInWord = inWords(parseFloat(Math.trunc(tPay)));
                var d = tPay - Math.trunc(tPay);
                if (d > 0) {
                    d = Math.round(d * 100);
                    _currencyInWord = _currencyInWord + " And " + d + " / 100";
                }
                _currencyInWord = "*** " + _currencyInWord + "****************";
                $("#<%=lblSelectedPayment.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                <%--$("#<%=lblRunBalance.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));--%>
                $("#<%=lblRequirement.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $("#<%=lblTotalAmount.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $("#<%=lblTotalAmount11.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $("#<%=lblDollar.ClientID%>").html(_currencyInWord);
                $("#<%=hdnTPay.ClientID%>").val(tPay.toString());
                $('.cls-payment').html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $("#<%=lblTotalDiscount.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tDisc).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $('.cls-disc').html(cleanUpCurrency("$" + parseFloat(tDisc).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                <% GetPaymentTotal();%>

                $('.cls-bal').html(cleanUpCurrency("$" + parseFloat(tBal).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                //var vitems = calculateItems();
                //$("#<%=lblOpenItems.ClientID%>").html(vitems.toString());

                //$("#<%=lblAutoSelectBalance.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay1).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $('.cls-payment').html(cleanUpCurrency("$" + parseFloat(tPay1).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $("#<%=lblTotalDiscount.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tDisc1).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $('.cls-disc').html(cleanUpCurrency("$" + parseFloat(tDisc1).toLocaleString("en-US", { minimumFractionDigits: 2 })));

                $('.cls-bal').html(cleanUpCurrency("$" + parseFloat(tBal1).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                document.getElementById('<%=btnSelectChkBox.ClientID%>').click();
            });

            var firstReport = '<%=Session["wc_first"] %>';
            if (firstReport == "true") {
                showFirstWindow();
                <%Session["wc_first"] = null; %>
            }

            var secondReport = '<%=Session["wc_second"] %>';
            if (secondReport == "true") {
                showSecondWindow();
                <%Session["wc_second"] = null; %>
            }

            var thirdReport = '<%=Session["wc_third"] %>';
            if (thirdReport == "true") {
                showThirdWindow();
                <%Session["wc_third"] = null; %>
            }

            <%--$("[id*=txtGvDisc]").change(function () {
                //debugger;
                var txtDisc = $(this).attr('id');
                var chk = $(this).parent().prevAll().find('input:checkbox[id$="chkSelect"]');
                var lblDue = $(this).parent().prev().find('span[id$="lblBalance"]');
                var hdnPrevDue = $(this).parent().prev().find('input:hidden[id$="hdnPrevDue"]');
                var disc = $(this).val().toString().replace(/[\$\(\),]/g, '');
                var pay = $(this).parent().next().find('input[id$="txtGvPay"]').val().replace(/[\$\(\),]/g, '');
                
                
                if (pay == '') {
                    pay = 0;
                    $(this).parent().next().find('input[id$="txtGvPay"]').val('$0.00')
                }
                if (disc == '') {
                    disc = 0;
                    $(this).val('$0.00')
                }

                
                total = parseFloat(pay) + parseFloat(disc);
                if (total != 0) {
                    debugger;
                    disc = parseFloat(disc);
                    var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''));
                    var prevDue = parseFloat($(hdnPrevDue).val())
                    
                    var IsNeg = false;
                    if (disc < 0) {
                        IsNeg = true;
                        pay = pay * -1;
                        disc = disc * -1;
                        prevDue = prevDue * -1;
                        total = total * -1;
                    }
                    // debugger;
                    if (prevDue < total) {
                        if (prevDue < disc) {
                            pay = 0;
                            disc = prevDue;
                        }
                        else {
                            pay = prevDue - disc;
                        }
                        total = parseFloat(pay) + parseFloat(disc);
                    }

                    due = prevDue - total;
                    if (IsNeg) {
                        pay = pay * -1;
                        prevDue = prevDue * -1;
                        disc = disc * -1;
                        due = due * -1;
                    }


                    var payy = parseFloat(pay) + parseFloat(due);
                    $(this).val(disc.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    //$(lblDue).text(cleanUpCurrency('$' + due.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    $(this).parent().next().find('input[id$="txtGvPay"]').val(payy.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                    $(chk).prop('checked', true);
                    SelectedRowStyle('<%=gvBills.ClientID %>')
                }
                else {
                    $(chk).prop('checked', false);
                    $(this).closest('tr').removeAttr("style");
                }
                CalculatePayTotal();
                CalculatePayTotalSelected();
                // document.getElementById('<%=btnSelectChkBox.ClientID%>').click();
            });--%>
            $("[id*=txtGvDisc]").change(function () {
                //debugger;
                var txtDisc = $(this).attr('id');
                var chk = $(this).parent().prevAll().find('input:checkbox[id$="chkSelect"]');
                var lblDue = $(this).parent().prev().find('span[id$="lblBalance"]');
                var hdnPrevDue = $(this).parent().prev().find('input:hidden[id$="hdnPrevDue"]');
                var disc = $(this).val().toString().replace(/[\$\(\),]/g, '');
                var pay = $(this).parent().next().find('input[id$="txtGvPay"]').val().replace(/[\$\(\),]/g, '');
                var hdnDisc = $(this).parent().prev().find('input:hidden[id$="hdnDisc"]');
                var hdnSelected = $(this).parent().prev().find('input:hidden[id$="hdnSelected"]');
                var hdnOriginal = $(this).parent().prev().find('input:hidden[id$="hdnOriginal"]');
                
                if (pay == '') {
                    pay = 0;
                    $(this).parent().next().find('input[id$="txtGvPay"]').val('$0.00')
                }
                if (disc == '') {
                    disc = 0;
                    $(this).val('$0.00')
                }

                var Selected = parseFloat($(hdnSelected).val());
                var Original = parseFloat($(hdnOriginal).val());

                var Duepayment = parseFloat(Original) - parseFloat(Selected) - parseFloat(pay) - parseFloat(disc);
                
                if (parseFloat(Duepayment) >= 0) {
                    //$(this).parent().prev().find('span[id$="lblBalance"]').val(parseFloat(Duepayment).toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    $(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                }
                else {
                    disc = 0;
                    var Duepayment = parseFloat(Original) - parseFloat(Selected) - parseFloat(pay) - parseFloat(disc);
                    
                    $(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                }
                total = parseFloat(pay) + parseFloat(disc);
                if (total != 0) {
                    
                    //debugger;
                    //disc = parseFloat(disc);
                    //var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''));
                    //var prevDue = parseFloat($(hdnPrevDue).val())
                    //alert(due);
                    //alert(prevDue);
                    //var IsNeg = false;
                    //if (disc < 0) {
                    //    IsNeg = true;
                    //    pay = pay * -1;
                    //    disc = disc * -1;
                    //    prevDue = prevDue * -1;
                    //    total = total * -1;
                    //}
                    //// debugger;
                    //if (prevDue < total) {
                    //    if (prevDue < disc) {
                    //        pay = 0;
                    //        disc = prevDue;
                    //    }
                    //    else {
                    //        pay = prevDue - disc;
                    //    }
                    //    total = parseFloat(pay) + parseFloat(disc);
                    //}

                    //due = prevDue - total;
                    //if (IsNeg) {
                    //    pay = pay * -1;
                    //    prevDue = prevDue * -1;
                    //    disc = disc * -1;
                    //    due = due * -1;
                    //}


                    //var payy = parseFloat(pay) + parseFloat(due);
                    $(this).val(disc.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    //$(lblDue).text(cleanUpCurrency('$' + due.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    //$(this).parent().next().find('input[id$="txtGvPay"]').val(payy.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                    $(chk).prop('checked', true);
                    SelectedRowStyle('<%=gvBills.ClientID %>')
                }
                else {
                    $(chk).prop('checked', false);
                    $(this).closest('tr').removeAttr("style");
                }
                CalculatePayTotal();
                CalculatePayTotalSelected();
                 document.getElementById('<%=btnSelectChkBox.ClientID%>').click();
            });


          <%--$("[id*=txtGvPay]").change(function () {
                   // debugger;
                    var txtPay = $(this).attr('id');
                
                var chk = $(this).parent().prevAll().find('input:checkbox[id$="chkSelect"]');
                var lblDue = $(this).parent().prev().prev().find('span[id$="lblBalance"]');
                var hdnPrevDue = $(this).parent().prev().prev().find('input:hidden[id$="hdnPrevDue"]');
                var pay = $(this).val().toString().replace(/[\$\(\),]/g, '');
                var disc = $(this).parent().prev().find('input[id$="txtGvDisc"]').val().toString().replace(/[\$\(\),]/g, '');
                var total = 0;
                if (pay == '') {
                    pay = 0;
                    $(this).val('$0.00')
                }
                if (disc == '') {
                    disc = 0;
                    $(this).parent().prev().find('input[id$="txtGvDisc"]').val('$0.00')
                }
                total = parseFloat(pay) + parseFloat(disc);
                if (total != 0) {
                    pay = parseFloat(pay);
                    var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''));
                    var prevDue = parseFloat($(hdnPrevDue).val())
                    var IsNeg = false;
                    if (pay < 0) {
                        IsNeg = true;
                        pay = pay * -1;
                        disc = disc * -1;
                        prevDue = prevDue * -1;
                        total = total * -1;
                    }
                    //debugger;
                    if (prevDue < total) {
                        pay = prevDue - disc;
                        total = parseFloat(pay) + parseFloat(disc);
                    }

                    due = prevDue - total;
                    if (IsNeg) {
                        pay = pay * -1;
                        prevDue = prevDue * -1;
                        disc = disc * -1;
                        due = due * -1;
                    }

                    $(this).val(pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    $(lblDue).text(cleanUpCurrency('$' + due.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    $(chk).prop('checked', true);
                    SelectedRowStyle('<%=gvBills.ClientID %>')
                }
                else {
                    $(chk).prop('checked', false);
                    $(this).closest('tr').removeAttr("style");
                }
                //GetInvoiceTotal();
                CalculatePayTotal();
                CalculatePayTotalSelected();
                //PageMethods.GetPaymentRetainCheckbox();
                 document.getElementById('<%=btnSelectChkBox.ClientID%>').click();
               
            });--%>
            $("[id*=txtGvPay]").change(function () {
                   // debugger;

                    var txtPay = $(this).attr('id');
                
                var chk = $(this).parent().prevAll().find('input:checkbox[id$="chkSelect"]');
                var lblDue = $(this).parent().prev().prev().find('span[id$="lblBalance"]');
                var hdnPrevDue = $(this).parent().prev().prev().find('input:hidden[id$="hdnPrevDue"]');
                var pay = $(this).val().toString().replace(/[\$\(\),]/g, '');
                var disc = $(this).parent().prev().find('input[id$="txtGvDisc"]').val().toString().replace(/[\$\(\),]/g, '');

                var hdnDisc = $(this).parent().prev().prev().find('input:hidden[id$="hdnDisc"]');
                var hdnSelected = $(this).parent().prev().prev().find('input:hidden[id$="hdnSelected"]');
                var hdnOriginal = $(this).parent().prev().prev().find('input:hidden[id$="hdnOriginal"]');

                //--------------Start:Comment By Juily - 19-12-2019----------------------//
                //var total = 0;
                //--------------------End: By Juily - 19-12-2019-----------------//
                if (pay == '') {
                    pay = 0;
                    $(this).val('$0.00')
                }
                if (disc == '') {
                    disc = 0;
                    $(this).parent().prev().find('input[id$="txtGvDisc"]').val('$0.00')
                }

                var Selected = parseFloat($(hdnSelected).val());
                var Original = parseFloat($(hdnOriginal).val());
                
                var Duepayment = parseFloat(Original) - parseFloat(Selected) - parseFloat(pay) - parseFloat(disc);
                //--------------Start:Code By Juily - 19-12-2019----------------------//
                
                if ((parseFloat(Duepayment) < 0 && parseFloat(Original) > 0) || (parseFloat(Duepayment) > 0 && parseFloat(Original) < 0))
                    {
                            noty({
                                text: 'OverPayment is not allowed.',
                                type: 'warning',
                                layout: 'topCenter',
                                closeOnSelfClick: true,
                                timeout: false,
                                theme: 'noty_theme_default',
                                closable: false
                        });

                        pay =0;
                        var Duepayment = parseFloat(Original) - parseFloat(Selected) - parseFloat(pay) - parseFloat(disc);
                        
                        $(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                        //$(lblDue).text('$0.00');
                        total = parseFloat(pay) + parseFloat(disc);
                        if (total == 0)
                        {
                            $(this).val(pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                            $(chk).prop('checked', true);
                            SelectedRowStyle('<%=gvBills.ClientID %>')
                        }
                        else
                        {
                            $(chk).prop('checked', false);
                            $(this).closest('tr').removeAttr("style");
                        }
                    }
                    else
                    {
                        //$(lblDue).text('$0.00');
                        $(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                        total = parseFloat(pay) + parseFloat(disc);
                        if (total != 0)
                        {
                            $(this).val(pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                            $(chk).prop('checked', true);
                            SelectedRowStyle('<%=gvBills.ClientID %>')
                        }
                        else
                        {
                            $(chk).prop('checked', false);
                            $(this).closest('tr').removeAttr("style");
                        }
                    }

                //--------------------End: By Juily - 19-12-2019-----------------//

                //--------------Start: Commented By Juily - 19-12-2019----------------------//
                //$(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                //--------------------End: By Juily - 19-12-2019-----------------//

                //if (parseFloat(Duepayment) >= 0) {
                //    //$(this).parent().prev().find('span[id$="lblBalance"]').val(parseFloat(Duepayment).toLocaleString("en-US", { minimumFractionDigits: 2 }));
                //    $(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                //}
                //else {
                //    pay = 0;
                //    var Duepayment = parseFloat(Original) - parseFloat(Selected) - parseFloat(pay) - parseFloat(disc);
                    
                //    $(lblDue).text(cleanUpCurrency('$' + Duepayment.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                //}
        //--------------Start: Commented By Juily - 19-12-2019----------------------//
               <%-- total = parseFloat(pay) + parseFloat(disc);
                if (total != 0) {
                    //pay = parseFloat(pay);
                    //var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''));
                    //var prevDue = parseFloat($(hdnPrevDue).val())
                    //var IsNeg = false;
                    //if (pay < 0) {
                    //    IsNeg = true;
                    //    pay = pay * -1;
                    //    disc = disc * -1;
                    //    prevDue = prevDue * -1;
                    //    total = total * -1;
                    //}
                    ////debugger;
                    //if (prevDue < total) {
                    //    pay = prevDue - disc;
                    //    total = parseFloat(pay) + parseFloat(disc);
                    //}

                    //due = prevDue - total;
                    //if (IsNeg) {
                    //    pay = pay * -1;
                    //    prevDue = prevDue * -1;
                    //    disc = disc * -1;
                    //    due = due * -1;
                    //}

                    $(this).val(pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    //$(lblDue).text(cleanUpCurrency('$' + due.toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    $(chk).prop('checked', true);
                    SelectedRowStyle('<%=gvBills.ClientID %>')
                }
                else {
                    $(chk).prop('checked', false);
                    $(this).closest('tr').removeAttr("style");
                }--%>
        //--------------------End: By Juily - 19-12-2019-----------------//
                //GetInvoiceTotal();
                CalculatePayTotal();
                CalculatePayTotalSelected();
                //PageMethods.GetPaymentRetainCheckbox();
                 document.getElementById('<%=btnSelectChkBox.ClientID%>').click();
               
            });
            $("#<%=txtVendor.ClientID%>").keyup(function (event) {

                var hdnVendorID = document.getElementById('<%=hdnVendorID.ClientID%>');
                if (document.getElementById('<%=txtVendor.ClientID%>').value == '') {
                    hdnVendorID.value = '';
                }
            });
           // Materialize.updateTextFields();
             function dtaas() {
                this.prefixText = null;
                this.vendor = null;
                this.con = null;
            }
             $("#<%=RadGrid_gvJobCostItems.ClientID%> tbody tr input:text, #<%=RadGrid_gvJobCostItems.ClientID%> tbody tr input:checkbox, #<%=RadGrid_gvJobCostItems.ClientID%> tbody tr select").on("focus", function (e) {
                // For F6
                var ctr = $(e)[0].target;
                var currRow = $(ctr).closest('tbody>tr');
                var hdnIndexVal = $(currRow).find("[id*=hdnIndex]").val();
                $('#<%=hdnSelectPOIndex.ClientID%>').val(hdnIndexVal);
                $(ctr).select();
                // Work around Chrome's little problem
                //$(ctr).onmouseup = function() {
                //    // Prevent further mouseup intervention
                //    $(ctr).onmouseup = null;
                //    return false;
                //};
            });
            $("[id*=txtGvJob]").focusout(function () {
                
                var txtGvJob = $(this).attr('id');
                var txtGvAcctNo = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvAcctNo'));
                var strAcctNo = $(txtGvAcctNo).val();

                var txtGvAcctNo1 = $(txtGvAcctNo).attr('id');
                var hdnAcctID = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'hdnAcctID'));
                var txtGvAcctName = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'txtGvAcctName'));

                if (strAcctNo == '') {
                    
                    var vendorId = $('#<%=hdnVendorID.ClientID%>').val();
                    if (vendorId != '') {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetGLbyVendor",
                            data: '{"vendor": "' + vendorId + '"}',
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                var ui = $.parseJSON(data.d);

                                if (ui.length > 0) {
                                    var strAcct = ui[0].Acct + ' - ' + ui[0].DefaultAcct;
                                    var GvPhase = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'txtGvPhase')).value;
                                    //-----If Inventory code select then we set default inventory Acct
                                    var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                                    var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                                    if (GvPhase == 'Inventory') {
                                        $(txtGvAcctNo).val(InvDefaultAcctName);
                                        $(hdnAcctID).val(InvDefaultAcctID);
                                    }
                                    else {
                                        $(txtGvAcctNo).val(strAcct);
                                        $(hdnAcctID).val(ui[0].DA);
                                    }
                                    //$(txtGvAcctName).val(ui[0].DefaultAcct);
                                }
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load default acct#");
                            }
                        });
                    }
                }
            });

            $("[id*=chkSelectAllGtax]").change(function () {
                //debugger;
                var ret = $(this).prop('checked');

                $("#<%=RadGrid_gvJobCostItems.ClientID %>").find('tr:not(:first, :last)').each(function () {
                    var $tr = $(this);
                    var chk = $tr.find('input[id*=chkGTaxable]');
                    if (ret == true) {
                        chk.prop('checked', true);
                    }
                    else {
                        chk.prop('checked', false);
                    }

                    var ch_id = chk.attr('id');
                    if (ch_id != undefined) {
                        if (ch_id != "ctl00_ContentPlaceHolder1_gvBills_ctl00_ctl02_ctl00_chkSelectAllGtax") {
                            CalTotalValGtax1(ch_id);
                        }
                    }

                })

            });
            $("[id*=chkSelectAllStax]").change(function () {
                //debugger;
                var ret = $(this).prop('checked');

                $("#<%=RadGrid_gvJobCostItems.ClientID %>").find('tr:not(:first, :last)').each(function () {
                    var $tr = $(this);
                    var chk = $tr.find('input[id*=chkTaxable]');
                    if (ret == true) {
                        chk.prop('checked', true);
                    }
                    else {
                        chk.prop('checked', false);
                    }

                    var ch_id = chk.attr('id');
                    if (ch_id != undefined) {
                        if (ch_id != "ctl00_ContentPlaceHolder1_gvBills_ctl00_ctl02_ctl00_chkSelectAllStax") {
                            CalTotalValStax1(ch_id);
                        }
                    }

                })

            });
            //---$$$$$ Start Items Autocomplete $$$$$$$--

            $("[id*=txtGvItem]").change(function () {
                //alert();
                var txtGvItem = $(this);
                var strItem = $(this).val();

                var txtGvItem1 = $(txtGvItem).attr('id');
                var hdnTypeId = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnTypeId'));
                var hdnPID = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnPID'));
                var txtGvItem = document.getElementById(txtGvItem1.replace('txtGvItem', 'txtGvItem'));
                var hdnItemID = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnItemID'));
                var txtGvDesc = document.getElementById(txtGvItem1.replace('txtGvItem', 'txtGvDesc'));
                var job = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnJobID')).value;
                var typeId = $(hdnTypeId).val();

                if (strItem != "") {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        // url: "AccountAutoFill.asmx/GetAutoFillItem",
                        url: "AccountAutoFill.asmx/GetPhaseExpByJobTypePO",
                        data: '{"prefixText": "' + strItem + '", "typeId": "' + typeId + '", "job": "' + job + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var ui = $.parseJSON(data.d);
                            if (ui.length == 0) {
                                //$(txtGvItem).val('');
                                $(hdnItemID).val('');
                                $(hdnPID).val('');
                            }
                            else {
                                $(txtGvItem).val(ui[0].ItemDesc1);
                                $(hdnItemID).val(ui[0].ItemID);
                                $(hdnPID).val(ui[0].Line);
                                $(txtGvDesc).val(ui[0].fDesc);
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Item");
                        }
                    });
                }
                else {
                    $(hdnPID).val('');
                    $(hdnItemID).val('');
                }
            });

            $("[id*=txtGvItem]").autocomplete({
                //open: function (e, ui) {
                //    /* create the scrollbar each time autocomplete menu opens/updates */
                //    $(".ui-autocomplete").mCustomScrollbar({
                //        setHeight: 182,
                //        theme: "dark-3",
                //        autoExpandScrollbar: true
                //    });
                //},
                //response: function (e, ui) {
                //    /* destroy the scrollbar after each search completes, before the menu is shown */
                //    $(".ui-autocomplete").mCustomScrollbar("destroy");
                //},
                source: function (request, response) {

                    var curr_control = this.element.attr('id');
                    var job = document.getElementById(curr_control.replace('txtGvItem', 'hdnJobID')).value;

                    var typeId = document.getElementById(curr_control.replace('txtGvItem', 'hdnTypeId')).value;
                    var prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetPhaseByItem",
                        data: '{"typeId": "' + typeId + '", "jobId": "' + job + '", "prefixText": "' + prefixText + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load item.");
                        },
                        complete: function () {
                            $(this).data('requestRunning', false);
                        }
                    });

                    return false;
                },
                deferRequestBy: 200,
                select: function (event, ui) {

                    var curr_control = this.id;
                    var hdnItemID = document.getElementById(curr_control.replace('txtGvItem', 'hdnItemID'));
                    var txtGvDesc = document.getElementById(curr_control.replace('txtGvItem', 'txtGvDesc'));
                    var hdnPID = document.getElementById(curr_control.replace('txtGvItem', 'hdnPID'));
                    var job = document.getElementById(curr_control.replace('txtGvItem', 'hdnJobID')).value;

                    var str = ui.item.ItemDesc;
                    var strId = ui.item.ItemID;

                    var GvPhase = document.getElementById(curr_control.replace('txtGvItem', 'txtGvPhase')).value;
                    //-----If Inventory code select then we set default inventory Acct
                    var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                    var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                    if (GvPhase == 'Inventory') {

                        var txtGvPrice = document.getElementById(curr_control.replace('txtGvItem', 'txtGvPrice'));
                        $(txtGvPrice).val(ui.item.Price);
                        CalTotalVal(txtGvPrice);
                        var txtGvAcctNo = document.getElementById(curr_control.replace('txtGvItem', 'txtGvAcctNo'));
                        var hdnAcctID = document.getElementById(curr_control.replace('txtGvItem', 'hdnAcctID'));
                        $(txtGvAcctNo).val(InvDefaultAcctName);
                        $(hdnAcctID).val(InvDefaultAcctID);
                    }


                    var CountOpsq = ui.item.CountData;
                    if (CountOpsq > 1) {

                        var hdOpSq = document.getElementById(curr_control.replace('txtGvItem', 'hdOpSq'));
                        var hdOpSq_ID = $(hdOpSq).attr('id');
                        var hdnPID_ID = $(hdnPID).attr('id');
                        $("#<%=hdOpSeqID.ClientID%>").val(hdOpSq_ID);
                        $("#<%=hdLineNo.ClientID%>").val(hdnPID_ID);

                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetOpsqList",
                            data: '{"jobId": "' + job + '", "ItemID": "' + strId + '"}',
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                var dd = $.parseJSON(data.d);
                                $('#opDiv').empty();
                                $.each(dd, function (k, v) {
                                    $('#opDiv').append('<div><input type="radio" id="' + v["Code"] + '" name="opSquence" value="' + v["Code"] + '" /><label for="' + v["Code"] + '">' + v["Code"] + ":" + v["fDesc"] + '</label><input type="hidden" id="lineItem' + v["Code"] + '" value="' + v["Line"] + '"></div>');
                                });
                                var radwindow = $find('<%=RadWindowWarehouse.ClientID %>');
                                radwindow.show();
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load item.");
                            }
                        });
                    }

                    if (strId == "0") {
                        $(this).val("");
                        $(hdnItemID).val("");
                        $(hdnPID).val("");

                    }
                    else {
                        if (ui.item.ItemID) {
                            $(txtGvDesc).val(ui.item.fDesc);
                            $(hdnItemID).val(ui.item.ItemID);
                            $(hdnPID).val(ui.item.Line);
                            $(this).val(ui.item.ItemDesc1);
                        }
                        else {
                            $(this).val("");
                            $(hdnPID).val(ui.item.Line);
                            $(txtGvDesc).val(ui.item.ItemDesc1);
                        }
                    }
                    return false;
                },
                focus: function (event, ui) {
                    if (ui.item) {
                        $(this).val(ui.item.ItemDesc1);
                    }
                    return false;
                },

                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val());
            })
            $.each($(".pisearchinput"), function (index, item) {

                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ItemID;
                    var result_item = item.ItemDesc1;
                    var result_line = item.Line;
                    var result_itemfdesc = item.fDesc;
                    var x = new RegExp('\\b' + query, 'ig');

                    try {
                        if (result_item != null) {
                            result_item = result_item.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>';
                            });
                        }

                        if (result_itemfdesc != null) {
                            result_itemfdesc = result_itemfdesc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>';
                            });
                        }
                    } catch{ }

                    if (result_line == "0") {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>  " + result_item + ", <span style='color:Gray;'><b>  </b>" + result_itemfdesc + "</span></a>")
                            .appendTo(ul);
                    }
                    else {
                        if (result_item == undefined) { result_item = 'No Record Found!'; }
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<span class='auto_item'><i style='display:inline-block; margin-right:8px;width:auto;color:#1565C0 !important;' class='fas fa-check-square' title=''></i>" + result_item + "</span>")
                            .appendTo(ul);
                    }
                };
            });
            //---$$$$$ END Items  autocomplete $$$$$$$--





            $("[id*=txtGvJob]").autocomplete({
                //open: function (e, ui) {
                //    /* create the scrollbar each time autocomplete menu opens/updates */
                //    $(".ui-autocomplete").mCustomScrollbar({
                //        setHeight: 182,
                //        theme: "dark-3",
                //        autoExpandScrollbar: true
                //    });
                //},
                //response: function (e, ui) {
                //    /* destroy the scrollbar after each search completes, before the menu is shown */
                //    $(".ui-autocomplete").mCustomScrollbar("destroy");
                //},

                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetJobLocations",
                        data: '{"prefixText": "' + dtaaa.prefixText + '", "IsJob": "' + true + '", "con": "' + dtaaa.con + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load project details");
                        }
                    });
                },
                select: function (event, ui) {
                    var txtGvJob = this.id;
                    var txtGvLoc = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvLoc'));
                    var hdnJobID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnJobID'));
                    var txtGvAcctNo = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvAcctNo'));
                    var hdnAcctID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnAcctID'));

                    $(hdnJobID).val(ui.item.ID);
                    var jobStr = ui.item.ID + ", " + ui.item.fDesc;
                    $(this).val(jobStr);
                    $(txtGvLoc).val(ui.item.Tag);
                    var GvPhase = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvPhase')).value;
                    //-----If Inventory code select then we set default inventory Acct
                    var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                        var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                    if (GvPhase == 'Inventory') {
                        $(txtGvAcctNo).val(InvDefaultAcctName);
                        $(hdnAcctID).val(InvDefaultAcctID);
                    }
                    else {
                        $(hdnAcctID).val(ui.item.GLExp);
                        var strAcct = ui.item.Acct + ' - ' + ui.item.DefaultAcct;
                        $(txtGvAcctNo).val(strAcct);
                    }
                    $('#hdnIsAutoCompleteSelected').val('1');
                    return false;
                },
                focus: function (event, ui) {
                    try {
                        $(this).val(ui.item.fDesc);
                    } catch{ }

                    return false;
                },
                change: function (event, ui) {
                    var txtGvJob = this.id;
                    var hdnJobID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnJobID'));
                    var strJob = document.getElementById(txtGvJob).value;

                    if (strJob == '') {
                        $(hdnJobID).val('')
                    }
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".psearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ID;
                    var result_item = item.fDesc;
                    var result_desc = item.Tag;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });

                    if (result_value != null) {
                        result_value = result_value.toString().replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    if (result_value == 0) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a><b> Project: </b> " + result_value + ", " + result_item + ", <span style='color:Gray;'><b> Loc: </b>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
            });

            //$("[id*=txtGvJob]").change(function () {
            //    var isAutoCompleteSelected = $('#hdnIsAutoCompleteSelected').val();
            //    $('#hdnIsAutoCompleteSelected').val('0');
            //    if (isAutoCompleteSelected != '1') {
            //        //debugger
            //        //var txtGvJob = ;
            //        var strItem = $(this).val();
            //        var txtGvJobId = $(this).attr('id');
            //        var txtGvLoc = document.getElementById(txtGvJobId.replace('txtGvJob', 'txtGvLoc'));
            //        var hdnJobID = document.getElementById(txtGvJobId.replace('txtGvJob', 'hdnJobID'));
            //        var txtGvAcctNo = document.getElementById(txtGvJobId.replace('txtGvJob', 'txtGvAcctNo'));
            //        var hdnAcctID = document.getElementById(txtGvJobId.replace('txtGvJob', 'hdnAcctID'));
            //        var txtGvJob = document.getElementById(txtGvJobId);
            //        strItem = $(hdnJobID).val();
            //        if (strItem != "") {
            //            $.ajax({
            //                type: "POST",
            //                contentType: "application/json; charset=utf-8",
            //                url: "AccountAutoFill.asmx/GetJobLocations",
            //                data: '{"prefixText": "' + strItem + '", "IsJob": "' + true + '", "con": ""}',
            //                dataType: "json",
            //                async: true,
            //                success: function (data) {
            //                    var ui = $.parseJSON(data.d);
            //                    if (ui.length == 0) {
            //                        $(txtGvJob).val('');
            //                        $(hdnJobID).val('');
            //                    }
            //                    else {
            //                        //debugger
            //                        $(hdnJobID).val(ui[0].ID);
            //                        var jobStr = ui[0].ID + ", " + ui[0].fDesc;
            //                        $(txtGvJob).val(jobStr);
            //                        $(txtGvLoc).val(ui[0].Tag);
            //                        $(hdnAcctID).val(ui[0].GLExp);
            //                        var strAcct = ui[0].Acct + ' - ' + ui[0].DefaultAcct;
            //                        $(txtGvAcctNo).val(strAcct);
            //                    }
            //                },
            //                error: function (result) {
            //                    alert("Due to unexpected errors we were unable to load project details");
            //                }
            //            });
            //        }
            //        else {
            //            $(txtGvJob).val('');
            //            $(hdnJobID).val('');
            //        }
            //    }
            //});
            $("[id*=txtGvAcctNo]").focusout(function () {
                var txtGvAcctNo = $(this);
                var strAcctNo = $(this).val();

                var txtGvAcctNo1 = $(txtGvAcctNo).attr('id');
                var hdnAcctID = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'hdnAcctID'));
                var hdnJobID = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'hdnJobID'));

                if (strAcctNo == '') {
                    var job = $(hdnJobID).val();
                    if (job != '' && job != '0') {

                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetGLExpByProject",
                            data: '{"Job": "' + job + '"}',
                            dataType: "json",
                            async: true,
                            success: function (data) {

                                var ui = $.parseJSON(data.d);

                                if (ui.length > 0) {
                                    var strAcct = ui[0].Acct + ' - ' + ui[0].DefaultAcct
                                    var GvPhase = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'txtGvPhase')).value;
                                    //-----If Inventory code select then we set default inventory Acct
                                    var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                                    var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                                    if (GvPhase == 'Inventory') {
                                        $(txtGvAcctNo).val(InvDefaultAcctName);
                                        $(hdnAcctID).val(InvDefaultAcctID);
                                        
                                    }
                                    else {
                                        $(txtGvAcctNo).val(strAcct);
                                        $(hdnAcctID).val(ui[0].GLExp);
                                    }
                                }
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load default expense acct#");
                            }
                        });
                    }
                    else {
                        
                        var vendorId = $('#<%=hdnVendorID.ClientID%>').val();
                        if (vendorId != '') {
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "AccountAutoFill.asmx/GetGLbyVendor",
                                data: '{"vendor": "' + vendorId + '"}',
                                dataType: "json",
                                async: true,
                                success: function (data) {
                                    var ui = $.parseJSON(data.d);

                                    if (ui.length > 0) {
                                        var strAcct = ui[0].Acct + ' - ' + ui[0].DefaultAcct
                                        var GvPhase = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'txtGvPhase')).value;
                                        //-----If Inventory code select then we set default inventory Acct
                                        var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                                        var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                                        if (GvPhase == 'Inventory') {
                                            $(txtGvAcctNo).val(InvDefaultAcctName);
                                            $(hdnAcctID).val(InvDefaultAcctID);
                                            
                                        }
                                        else {
                                            $(txtGvAcctNo).val(strAcct);
                                            $(hdnAcctID).val(ui[0].DA);
                                        }
                                    }
                                },
                                error: function (result) {
                                    alert("Due to unexpected errors we were unable to load default acct#");
                                }
                            });
                        }
                    }
                }
            });

            $("[id*=txtGvAcctNo]").change(function () {
                var txtGvAcctNo = $(this);
                var strAcctNo = $(this).val();
                strAcctNo = strAcctNo.split(" -")[0];
                var txtGvAcctNo1 = $(txtGvAcctNo).attr('id');
                var hdnAcctID = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'hdnAcctID'));
                var hdnJobID = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'hdnJobID'));

                if (strAcctNo != '') {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetChartByAcct",
                        data: '{"prefixText": "' + strAcctNo + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var ui = $.parseJSON(data.d);

                            if (ui.length == 0) {
                                var strAcct = $(txtGvAcctNo).val();
                                $(txtGvAcctNo).val('');
                                noty({
                                    text: 'Acct #' + strAcct + ' doesn\'t exist!',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: false,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                            }
                            else {
                                var strAcct = ui[0].Acct + ' - ' + ui[0].fDesc;
                                var GvPhase = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'txtGvPhase')).value;
                                //-----If Inventory code select then we set default inventory Acct
                                var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                                var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                                if (GvPhase == 'Inventory') {
                                    $(txtGvAcctNo).val(InvDefaultAcctName);
                                    $(hdnAcctID).val(InvDefaultAcctID);
                                    
                                }
                                else {
                                    $(txtGvAcctNo).val(strAcct);
                                    $(hdnAcctID).val(ui[0].ID);
                                }
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Acct#");
                        }
                    });
                }

            });

            $("[id*=txtGvAcctNo]").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaas();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    //debugger;
                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/spGetAccountSearchAP",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load account name");
                        }
                    });
                },
                select: function (event, ui) {

                    if (ui.item.value == 0)
                        window.location.href = "addcoa.aspx";
                    else {
                        var txtGvAcctName = this.id;
                        var hdnAcctID = document.getElementById(txtGvAcctName.replace('txtGvAcctNo', 'hdnAcctID'));
                        var strAcct = ui.item.acct + " - " + ui.item.label;
                        var GvPhase = document.getElementById(txtGvAcctName.replace('txtGvAcctNo', 'txtGvPhase')).value;
                        //-----If Inventory code select then we set default inventory Acct
                        var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                        var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                        if (GvPhase == 'Inventory') {
                            $(this).val(InvDefaultAcctName);
                            $(hdnAcctID).val(InvDefaultAcctID);
                            
                        }
                        else {
                            $(hdnAcctID).val(ui.item.value);
                            $(this).val(strAcct);
                        }
                    }

                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.acct);
                    return false;
                },
                change: function (event, ui) {

                    var txtGvAcctNo = this.id;
                    var hdnAcctID = document.getElementById(txtGvAcctNo.replace('txtGvAcctNo', 'hdnAcctID'));
                    var strAcct = document.getElementById(txtGvAcctNo).value;

                    if (strAcct == '') {
                        $(hdnAcctID).val('')
                    }
                },
                minLength: 0,
                delay: 250
            })
            $.each($(".searchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {

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

                    if (result_value == 0) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
            });
            $("[id*=txtGvPhase]").autocomplete({
                //open: function (e, ui) {
                //    /* create the scrollbar each time autocomplete menu opens/updates */
                //    $(".ui-autocomplete").mCustomScrollbar({
                //        setHeight: 182,
                //        theme: "dark-3",
                //        autoExpandScrollbar: true
                //    });
                //},
                //response: function (e, ui) {
                //    /* destroy the scrollbar after each search completes, before the menu is shown */
                //    $(".ui-autocomplete").mCustomScrollbar("destroy");
                //},
                source: function (request, response) {

                    var curr_control = this.element.attr('id');
                    var job = document.getElementById(curr_control.replace('txtGvPhase', 'hdnJobID'));
                    var prefixText = request.term;
                    var job = document.getElementById(job.id).value;
                    if (job == "0") { job = ""; }
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetPhase",
                        data: '{"jobID": "' + job + '", "prefixText": "' + prefixText + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load type.");
                        },
                        complete: function () {
                            $(this).data('requestRunning', false);
                        }
                    });
                    return false;
                },
                deferRequestBy: 200,
                select: function (event, ui) {
                    
                    var txtGvPhase = this.id;
                    var hdnTypeId = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnTypeId'));
                    var hdOpSq = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdOpSq'));
                    var str = ui.item.TypeName;
                    if (str == "No Record Found!") {
                        $(this).val("");
                    }
                    else {
                        try {
                            $(hdnTypeId).val(ui.item.Type);
                            $(this).val(ui.item.TypeName);
                            $(hdOpSq).val(ui.item.Code);
                        } catch{ }
                    }

                    var GvPhase = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvPhase')).value;
                    //-----If Inventory code select then we set default inventory Acct
                    var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                    var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                    if (GvPhase == 'Inventory') {
                        var txtGvAcctNo = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvAcctNo'));
                        var hdnAcctID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnAcctID'));
                        $(txtGvAcctNo).val(InvDefaultAcctName);
                        $(hdnAcctID).val(InvDefaultAcctID);
                    }


                    return false;
                },
                focus: function (event, ui) {
                    //debugger
                    if (ui.item != null) {
                        $(this).val(ui.item.TypeName);
                    }
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .click(function () {
                    $(this).autocomplete('search', $(this).val())
                })
            $.each($(".phsearchinput"), function (index, item) {

                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.Type;
                    var result_item = item.TypeName;
                    var result_GroupName = item.GroupName;
                    var result_Code = item.Code;
                    var result_CodeDesc = item.CodeDesc;
                    if (result_Code != null && result_Code != "")
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<span class='auto_item'><i style='display:inline-block; margin-right:8px;width:auto;color:#1565C0 !important;' class='fa fa-check-square' title=''></i>" + result_GroupName + ", " + result_Code + ", " + result_CodeDesc + ", <span style='color:Gray;'><b>  </b>" + result_item + "</span></span>")
                            .appendTo(ul);
                    else
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<span class='auto_item'><i style='display:inline-block; margin-right:8px;width:auto;color:#1565C0 !important;' title=''></i>" + result_item + "</span>")
                            .appendTo(ul);
                };
            });

            //$("[id*=txtGvPhase]").change(function () {
            //    var txtGvPhase = $(this);
            //    var strPhase = $(this).val();

            //    var txtGvPhase1 = $(txtGvPhase).attr('id');
            //    var hdnTypeId = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnTypeId'));
            //    var hdnPID = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnPID'));
            //    var txtGvItem = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'txtGvItem'));
            //    var hdnItemID = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnItemID'));
            //    var txtGvDesc = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'txtGvDesc'));

            //    if (strPhase != "") {
            //        $.ajax({
            //            type: "POST",
            //            contentType: "application/json; charset=utf-8",
            //            url: "AccountAutoFill.asmx/GetAutoFillPhase",
            //            data: '{"prefixText": "' + strPhase + '"}',
            //            dataType: "json",
            //            async: true,
            //            success: function (data) {

            //                var ui = $.parseJSON(data.d);

            //                if (ui.length == 0) {
            //                    $(txtGvPhase).val('');
            //                    $(hdnTypeId).val('');
            //                    $(hdnPID).val('');
            //                    $(txtGvItem).val('');
            //                    $(hdnItemID).val('');
            //                    noty({
            //                        text: 'Type \'' + strPhase + '\' doesn\'t exist!',
            //                        type: 'warning',
            //                        layout: 'topCenter',
            //                        closeOnSelfClick: false,
            //                        timeout: 5000,
            //                        theme: 'noty_theme_default',
            //                        closable: true
            //                    });
            //                }
            //                else {
            //                    var lbl = ui[0].Label;
            //                    var val = ui[0].Value;
            //                    $(txtGvPhase).val(lbl);
            //                    $(hdnTypeId).val(val);
            //                }
            //            },
            //            error: function (result) {
            //                alert("Due to unexpected errors we were unable to load Type");
            //            }
            //        });
            //    }
            //    else {
            //        $(hdnPID).val('');
            //        $(hdnTypeId).val('');
            //        $(txtGvItem).val('');
            //        $(hdnItemID).val('');
            //        $(txtGvDesc).val('');
            //    }
            //});
            $("[id*=txtGvPhase]").change(function () {
                //debugger
                //var txtGvPhase = $(this);
                var strPhase = $(this).val();
                var txtGvPhaseId = $(this).attr('id');
                var hdnTypeId = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'hdnTypeId'));
                var hdntxtGvPhase = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'hdntxtGvPhase'));
                var hdOpSq = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'hdOpSq'));
                //var txtGvPhase1 = $(txtGvPhase).attr('id');
                //var hdnTypeId = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnTypeId'));
                var hdnPID = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'hdnPID'));
                var txtGvItem = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'txtGvItem'));
                var hdnItemID = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'hdnItemID'));
                var txtGvDesc = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'txtGvDesc'));
                var txtGvAcctNo = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'txtGvAcctNo'));
                var hdnAcctID = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'hdnAcctID'));
                //var txtGvWarehouse = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'txtGvWarehouse'));
                //var txtGvWarehouseLocation = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'txtGvWarehouseLocation'));
                var txtGvPhase = document.getElementById(txtGvPhaseId);
                var hdnJobContr = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'hdnJobID'));
                var job = document.getElementById(hdnJobContr.id).value;
                if (job == "0") { job = ""; }
                if (strPhase != "") {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        //url: "AccountAutoFill.asmx/GetAutoFillPhase",
                        //data: '{"prefixText": "' + strPhase + '"}',
                        url: "AccountAutoFill.asmx/GetPhase",
                        data: '{"jobID": "' + job + '", "prefixText": "' + strPhase + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            //debugger
                            var ui = $.parseJSON(data.d);
                            if (ui.length == 0) {
                                $(txtGvPhase).val('');
                                $(hdnTypeId).val('');
                                $(hdnPID).val('');
                                $(txtGvItem).val('');
                                $(hdnItemID).val('');
                                noty({
                                    text: 'Type \'' + strPhase + '\' doesn\'t exist!',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: 5000,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                            }
                            else
                            {
                                $(hdnTypeId).val(ui[0].Type);
                                console.log(hdnTypeId.value);
                                $(hdOpSq).val(ui[0].Code);
                                $(txtGvPhase).val(ui[0].TypeName);
                                $(hdntxtGvPhase).val(ui[0].TypeName);
                                //console.log(hdntxtGvPhase.value);
                                //var txtGvAcctNo;
                                //var hdnAcctID;
                                //var txtGvWarehouse;
                                //var txtGvWarehouseLocation;

                                if (ui[0].TypeName == "Inventory") {
                                    try {
                                        //HideGridColums("true");
                                        //do inventory default account
                                        
                                        $(txtGvAcctNo).val(document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value);
                                        $(hdnAcctID).val(document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value);
                                        
                                        //$(txtGvWarehouse).attr('readOnly', false);
                                        //$(txtGvWarehouseLocation).attr('readOnly', false);
                                    } catch (e){ }
                                }
                                else {
                                    // HideGridColums("false");
                                    try {
                                        //txtGvWarehouse = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouse'));
                                        //txtGvWarehouseLocation = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouseLocation'));
                                        //txtGvWarehouse.readOnly = true;
                                        //txtGvWarehouseLocation.readOnly = true;
                                        //$(txtGvWarehouse).attr('readOnly', true);
                                        //$(txtGvWarehouseLocation).attr('readOnly', true);
                                        //$(txtGvWarehouse).val('');
                                        //$(txtGvWarehouseLocation).val('');
                                        //txtGvAcctNo = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvAcctNo'));
                                        //hdnAcctID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnAcctID'));
                                        if (ui[0].AcctName != '' && ui[0].AcctID != '' && ui[0].AcctName != undefined && ui[0].AcctID != undefined) {
                                            $(txtGvAcctNo).val(ui.item.AcctName);
                                            $(hdnAcctID).val(ui.item.AcctID);
                                        }
                                    } catch (e){ }
                                }
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Type");
                        }
                    });
                }
                else {
                    $(hdnPID).val('');
                    $(hdnTypeId).val('');
                    $(txtGvItem).val('');
                    $(hdnItemID).val('');
                    $(txtGvDesc).val('');
                }
            });
            $("[id*=txtGvPhase]").focusout(function () {
                $(this).change();
            });



            $("[id*=txtGvUseTax]").autocomplete({
                //open: function (e, ui) {
                //    /* create the scrollbar each time autocomplete menu opens/updates */
                //    $(".ui-autocomplete").mCustomScrollbar({
                //        setHeight: 182,
                //        theme: "dark-3",
                //        autoExpandScrollbar: true
                //    });
                //},
                //response: function (e, ui) {
                //    /* destroy the scrollbar after each search completes, before the menu is shown */
                //    $(".ui-autocomplete").mCustomScrollbar("destroy");
                //},
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/getUseTaxSearch",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                          <%-- var returnedData = $.grep($.parseJSON(data.d), function (element, index) {
                                
                                return element.Name == $("#<%=hdnUTaxName.ClientID%>").val();
                            });
                            response(returnedData);--%>

                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load account name");
                        }
                    });
                },
                deferRequestBy: 200,
                select: function (event, ui) {

                    if (ui.item.value == 0)
                        window.location.href = "addbills.aspx";
                    else {
                        var txtGvUseTax = this.id;
                        $(this).val(ui.item.Rate);

                        var hdnUtax = document.getElementById(txtGvUseTax.replace('txtGvUseTax', 'hdnUtax'));
                        var hdnUtaxGL = document.getElementById(txtGvUseTax.replace('txtGvUseTax', 'hdnUtaxGL'));

                        $(hdnUtax).val(ui.item.Name);
                        $(hdnUtaxGL).val(ui.item.GL);
                    }

                    return false;
                },
                focus: function (event, ui) {

                    $(this).val(ui.item.Rate);
                    return false;
                },
                minLength: 0,
                delay: 250

            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".tsearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.Rate;
                    var result_item = item.Name;
                    var result_desc = item.GL;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });

                    if (result_value != null) {
                        result_value = result_value.toString().replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }

                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }

                    if (result_value == 0) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_value + "</span></a>")
                            .appendTo(ul);
                    }
                };
            });


            $("[id*=txtGvUseTax]").change(function () {
                
                //var txtGvPhase = $(this);
                var strPhase = $(this).val();
                var txtGvUseTax = this.id;
                var hdnUtax = document.getElementById(txtGvUseTax.replace('txtGvUseTax', 'hdnUtax'));
                var hdnUtaxGL = document.getElementById(txtGvUseTax.replace('txtGvUseTax', 'hdnUtaxGL'));
               var dtaaa = new dtaa();
                    dtaaa.prefixText = strPhase;
                if (strPhase != "") {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        //url: "AccountAutoFill.asmx/GetAutoFillPhase",
                        //data: '{"prefixText": "' + strPhase + '"}',
                        url: "AccountAutoFill.asmx/getUseTaxSearch",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            // debugger
                            
                            var ui = $.parseJSON(data.d);
                            
                            if (ui.length == 0) {
                                
                                $(hdnUtax).val('');
                                $(hdnUtaxGL).val('');
                                $(this).val("");
                                
                                noty({
                                    text: 'Tax \'' + strPhase + '\' doesn\'t exist!',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: 5000,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                            }
                            else
                            {
                                $(this).val(ui[0].Rate);
                                $(hdnUtax).val(ui[0].Name);
                                
                                $(hdnUtaxGL).val(ui[0].GL);

                                
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Type");
                        }
                    });
                }
                else {
                    $(hdnUtax).val('');
                    $(hdnUtaxGL).val('');
                    $(this).val('');
                }
            });


        }
         function OpenApplyCreditModal() {
            <%--$('#<%=txtVendorType.ClientID%>').val("");
            $('#<%=txtremarksvendor.ClientID%>').val("");
            $('#<%=txtVendorType.ClientID%>').prop("readonly", false);--%>

             debugger;
             var tDisc = 0;
            
             var vdis = $("#<%=lblTotalDiscount.ClientID%>").html().replace(/[\$\(\),]/g, '');
             if (!isNaN(parseFloat(vdis))) {
                 tDisc += parseFloat(vdis);
             }
             if (tDisc != 0) {
                 var bqs = GetParameterValues('bill');
                 var vqs = GetParameterValues('vid');
                 var rqs = GetParameterValues('ref');
                 if (bqs == 'c') {
                     if (vqs == null) {
                        <%-- //ValidatorEnable($('#<%=rfvtxtvendor.ClientID %>')[0], true);
                         ValidatorEnable($('#<%=cvDiscGLs.ClientID %>')[0], true);

                         var rtvalue = false;
                         var v = document.getElementById("<%=cvDiscGLs.ClientID%>");
                         ValidatorValidate(v);
                         if (v.isvalid)
                             rtvalue = true;
                         else
                             rtvalue = false;--%>
                         var hdnDiscGLs = document.getElementById('<%=hdnDiscGLs.ClientID%>');
                         if (hdnDiscGLs.value == '') {
                             ValidatorEnable($('#<%=cvDiscGLs.ClientID %>')[0], true);
                     }
                     else {
                         ValidatorEnable($('#<%=cvDiscGLs.ClientID %>')[0], false);
                         var wnd = $find('<%=ReprintCheckRange.ClientID %>');
                         wnd.set_title("Apply Credit");
                         wnd.Show();
                     }


                     }
                 }
                 else {

                     var hdnDiscGL = document.getElementById('<%=hdnDiscGL.ClientID%>');
                     if (hdnDiscGL.value == '') {
                         ValidatorEnable($('#<%=cvDiscGL.ClientID %>')[0], true);
                     }
                     else {
                         ValidatorEnable($('#<%=cvDiscGL.ClientID %>')[0], false);
                         var wnd = $find('<%=ReprintCheckRange.ClientID %>');
                         wnd.set_title("Apply Credit");
                         wnd.Show();
                     }
                 }
             }
             else {
                 
                 ValidatorEnable($('#<%=cvDiscGLs.ClientID %>')[0], false);
                 ValidatorEnable($('#<%=cvDiscGL.ClientID %>')[0], false);
                 var wnd = $find('<%=ReprintCheckRange.ClientID %>');
                 wnd.set_title("Apply Credit");
                 wnd.Show();
             }




            
        }
        function CloseApplyCreditModal() {
            var wnd = $find('<%=ReprintCheckRange.ClientID %>');
            wnd.Close();
            
        }

        function showFirstWindow() {
            Sys.Application.remove_load(showFirstWindow);
            var oWindowCust = $find('<%= RadWindowFirstReport.ClientID %>');
            oWindowCust.show();
        }

        function showSecondWindow() {
            Sys.Application.remove_load(showSecondWindow);
            var oWindowCust = $find('<%= RadWindowSecondReport.ClientID %>');
            oWindowCust.show();
        }

        function showThirdWindow() {
            Sys.Application.remove_load(showThirdWindow);
            var oWindowCust = $find('<%= RadWindowThirdReport.ClientID %>');
            oWindowCust.show();
        }

        function OnClientCloseHandler(sender, args) {
            //debugger
            <%Session["wc_first"] = null; %>
            <%Session["wc_second"] = null; %>
            <%Session["wc_third"] = null; %>
            var bill = GetParameterValues('bill');
            var vid = GetParameterValues('vid');
            if (bill == null) {
                $find("<%=RadAjaxManager_WC.ClientID%>").ajaxRequest();
                
                //window.setTimeout(function () {

                //    window.location.href = "WriteChecks.aspx";

                //}, 500);
            }
            else {
                if (vid == null) {
                    noty({ text: 'Checks Saved Successfully! </br> <b>', dismissQueue: true, type: 'success', layout: 'topCenter', closeOnSelfClick: true, timeout: false, theme: 'noty_theme_default', closable: false });

                    window.setTimeout(function () {

                        window.location.href = "managechecks.aspx?f=c";

                    }, 1000);
                }
                else {
                    $find("<%=RadAjaxManager_WC.ClientID%>").ajaxRequest();
                }
            }
            
			
        }
         function GetParameterValues(param) {  
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');  
            for (var i = 0; i < url.length; i++) {  
                var urlparam = url[i].split('=');  
                if (urlparam[0] == param) {  
                    return urlparam[1];  
                }  
            }  
        }  
        function CalGrid(chk) {
            var hdnSelected = document.getElementById(chk.replace('chkSelect', 'hdnSelected'));
            var hdnOriginal = document.getElementById(chk.replace('chkSelect', 'hdnOriginal'));
            var hdnPrevDue = document.getElementById(chk.replace('chkSelect', 'hdnPrevDue'));
            var lblDue = document.getElementById(chk.replace('chkSelect', 'lblBalance'))
            var txtDisc = document.getElementById(chk.replace('chkSelect', 'txtGvDisc'));
            var txtPay = document.getElementById(chk.replace('chkSelect', 'txtGvPay'));

            var pay = $(txtPay).val().toString().replace(/[\$\(\),]/g, '');
            var disc = $(txtDisc).val().toString().replace(/[\$\(\),]/g, '');
            //////////////////////
            var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
            var prevDue = parseFloat($(hdnOriginal).val()-$(hdnSelected).val())
            var pay = 0;

            var rpay = pay.toLocaleString("en-US", { minimumFractionDigits: 2 });
            var rprevDue = prevDue.toLocaleString("en-US", { minimumFractionDigits: 2 });
            if ($("#" + chk).prop('checked') == true) {

                $(txtPay).val(rprevDue)
                $(txtDisc).val('0.00')
                $(lblDue).text(cleanUpCurrency('$' + rpay))
                SelectedRowStyle('<%=gvBills.ClientID %>')
            }
            else if ($("#" + chk).prop('checked') == false) {

                $(txtPay).val(rpay)
                $(txtDisc).val(rpay)
                $(lblDue).text(cleanUpCurrency('$' + rprevDue))
                $(this).closest('tr').removeAttr("style");
            }
            //CalculatePayTotal();
            //CalculatePayTotalSelected();
        }  

         /////////////////// To calculate Total and to make Gridview Amount Value to 2 decimal ////////////NK
        function CalTotalValStax(checkbox) {

            var cb = checkbox.id;
            var stax = document.getElementById('<%=hdnQST.ClientID%>');
            var gtax = document.getElementById('<%=hdnGST.ClientID%>');
            if (parseFloat(stax.value.toString().replace(/[\$\(\),]/g, '')) <= 0) {

                noty({
                    text: 'Please Set the Provincial Tax at vendor level',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 1500,
                    theme: 'noty_theme_default',
                    closable: true
                });
                $(hdnchkTaxable).val('0');
                checkbox.checked = false;

            }
            else {


                var staxGL = document.getElementById('<%=hdnQSTGL.ClientID%>');
                var gtaxGL = document.getElementById('<%=hdnGSTGL.ClientID%>');

                var staxType = document.getElementById('<%=hdnSTaxType.ClientID%>');

                var txtGvQuan;
                var txtGvPrice;
                var txtGvAmount;
                var lblSalesTax;
                var lblGstTax;
                var hdnGSTTaxAm;
                var hdnSTaxAm;
                var txtGvStaxAmount;
                var lblAmountWithTax;
                var valAmount;
                var hdnchkTaxable;
                var hdnSTaxGL;
                var hdnGSTTaxGL;
                var isGst = 0;
                var totamt = 0;
                var staxAmt = 0;
                var gtaxAmt = 0;
                var staxAmtGL = 0;
                var gtaxAmtGL = 0;

                txtGvPrice = document.getElementById(cb.replace('chkTaxable', 'txtGvPrice'));
                txtGvQuan = document.getElementById(cb.replace('chkTaxable', 'txtGvQuan'));
                txtGvAmount = document.getElementById(cb.replace('chkTaxable', 'txtGvAmount'));
                //lblSalesTax = document.getElementById(cb.replace('chkTaxable', 'lblSalesTax'));
                lblGstTax = document.getElementById(cb.replace('chkTaxable', 'lblGstTax'));
                lblAmountWithTax = document.getElementById(cb.replace('chkTaxable', 'lblAmountWithTax'));
                hdnAmountWithTax = document.getElementById(cb.replace('chkTaxable', 'hdnAmountWithTax'));
                hdnchkTaxable = document.getElementById(cb.replace('chkTaxable', 'hdnchkTaxable'));
                hdnSTaxGL = document.getElementById(cb.replace('chkTaxable', 'hdnSTaxGL'));
                hdnGSTTaxGL = document.getElementById(cb.replace('chkTaxable', 'hdnGSTTaxGL'));
                hdnSTaxAm = document.getElementById(cb.replace('chkTaxable', 'hdnSTaxAm'));
                txtGvStaxAmount = document.getElementById(cb.replace('chkTaxable', 'txtGvStaxAmount'));
                hdnGSTTaxAm = document.getElementById(cb.replace('chkTaxable', 'hdnGSTTaxAm'));

                gtaxAmt = parseFloat($(hdnGSTTaxAm).val());
                gtaxAmt = parseFloat(gtaxAmt) || 0;
                if (checkbox.checked == true) {
                    $(hdnchkTaxable).val('1');
                } else {
                    $(hdnchkTaxable).val('0');
                }

                isGst = 1;
                //if (document.getElementById('txtgstgv').style.display == 'block')
                //{
                //    isGst = 1;
                //}
                //else
                //{
                //    isGst = 0;                     
                //}


                if (!jQuery.trim($(txtGvQuan).val()) == '') {
                    if (isNaN(parseFloat($(txtGvQuan).val()))) {
                        $(txtGvQuan).val('0.00');
                    }
                }

                if (!jQuery.trim($(txtGvPrice).val()) == '') {
                    if (isNaN(parseFloat($(txtGvPrice).val()))) {
                        $(txtGvPrice).val('');
                    }
                }

                if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
                    if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
                        valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
                        $(txtGvAmount).val(valAmount.toFixed(2));
                        //$(txtGvAmount).val(cleanUpCurrency(parseFloat(valAmount).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    }
                } else if (!jQuery.trim($(txtGvQuan).val()) == '' && $(txtGvAmount).val() != '' && jQuery.trim($(txtGvPrice).val()) == '') {
                    if (!isNaN(parseFloat($(txtGvQuan).val())) && parseFloat($(txtGvQuan).val()) != 0 && !isNaN(parseFloat($(txtGvAmount).val()))) {
                        var valPrice = parseFloat($(txtGvAmount).val()) / parseFloat($(txtGvQuan).val());
                        $(txtGvPrice).val(valPrice.toFixed(2));
                        //$(txtGvPrice).val(cleanUpCurrency(parseFloat(valPrice).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    }
                }





                //if (isGst == 1) {
                //    if (gtax == null) {
                //        gtaxAmt = 0.00;
                //        gtaxAmtGL = 0;
                //        $(lblGstTax).val(gtaxAmt.toFixed(2));

                //        $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                //        $(hdnGSTTaxGL).val(gtaxAmtGL);
                //    }
                //    else if (gtax.value != '') {
                //        if (checkbox.checked == true) {
                //            gtaxAmt = Math.round(((parseFloat(valAmount) * parseFloat(gtax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                //            $(lblGstTax).val(gtaxAmt.toFixed(2));
                //            $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                //            gtaxAmtGL = parseInt(gtaxGL.value);
                //            $(hdnGSTTaxGL).val(gtaxAmtGL.value);
                //        }
                //        else {
                //            gtaxAmt = 0.00;
                //            gtaxAmtGL = 0;
                //        }
                //        $(lblGstTax).val(gtaxAmt.toFixed(2));

                //        $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                //        $(hdnGSTTaxGL).val(gtaxAmtGL);

                //    }

                //}

                if (checkbox.checked == true) {
                    if (parseInt(staxType.value) == 0 || parseInt(staxType.value) == 2) {
                        if (parseFloat(valAmount) < 0) {

                            staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmt = staxAmt * (-1);
                            staxAmtGL = parseInt(staxGL.value);

                        } else {
                            staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmtGL = parseInt(staxGL.value);
                        }
                    }
                    else if (parseInt(staxType.value) == 1) {
                        var oldvalAmount = valAmount;
                        //if (isGst == 1) {
                        valAmount = parseFloat(valAmount) + gtaxAmt;

                        //}


                        // if (parseFloat(gtaxAmt) > 0) {

                        if (parseFloat(valAmount) < 0) {

                            staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmt = staxAmt * (-1);
                            staxAmtGL = parseInt(staxGL.value);
                            valAmount = oldvalAmount;

                        } else {
                            staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmtGL = parseInt(staxGL.value);
                            valAmount = oldvalAmount;
                        }
                        // }
                        //else {
                        //noty({
                        //    text: 'Please This is a compound tax. Please select GST tax.',
                        //    type: 'warning',
                        //    layout: 'topCenter',
                        //    closeOnSelfClick: false,
                        //    timeout: 1500,
                        //    theme: 'noty_theme_default',
                        //    closable: true
                        //});
                        //$(hdnchkTaxable).val('0');
                        //checkbox.checked = false;
                        //staxAmt = 0.00;
                        //staxAmtGL = 0;
                        //}
                    }




                } else {
                    staxAmt = 0.00;
                    staxAmtGL = 0;
                }
                if (isNaN(staxAmt)) {

                    staxAmt = 0.00;
                }
                if (isNaN(gtaxAmt)) {

                    gtaxAmt = 0.00;
                }
                if (isNaN(valAmount)) {

                    valAmount = 0.00;
                }


                //$(lblSalesTax).val(staxAmt.toFixed(2));
                $(hdnSTaxAm).val(staxAmt.toFixed(2));
                $(txtGvStaxAmount).val(staxAmt.toFixed(2));

                $(hdnSTaxGL).val(staxAmtGL);



                totamt = valAmount + staxAmt;
                if (isGst == 1) {
                    totamt = totamt + gtaxAmt;
                }
                $(lblAmountWithTax).text(totamt.toFixed(2));
                //$(hdnAmountWithTax).val(staxAmt.toFixed(2));
                $(hdnAmountWithTax).val(totamt.toFixed(2));
            }

            //CalculateTotalAmtSST();
            CalculateTotalAmt();
        }

        function CalTotalValGtax(checkbox) {

            var cb = checkbox.id;
            var stax = document.getElementById('<%=hdnQST.ClientID%>');
            var gtax = document.getElementById('<%=hdnGST.ClientID%>');
            //if (parseFloat(stax.value.toString().replace(/[\$\(\),]/g, '')) <= 0) {

            //    noty({
            //        text: 'Please Set the Provincial Tax at vendor level',
            //        type: 'warning',
            //        layout: 'topCenter',
            //        closeOnSelfClick: false,
            //        timeout: 1500,
            //        theme: 'noty_theme_default',
            //        closable: true
            //    });
            //    $(hdnchkTaxable).val('0');
            //    checkbox.checked = false;

            //}
            //else {


            var staxGL = document.getElementById('<%=hdnQSTGL.ClientID%>');
            var gtaxGL = document.getElementById('<%=hdnGSTGL.ClientID%>');

            var staxType = document.getElementById('<%=hdnSTaxType.ClientID%>');

            var txtGvQuan;
            var txtGvPrice;
            var txtGvAmount;
            var lblSalesTax;
            var lblGstTax;
            var hdnGSTTaxAm;
            var hdnSTaxAm;
            var txtGvStaxAmount;
            var lblAmountWithTax;
            var valAmount;
            var hdnchkTaxable;
            var hdnSTaxGL;
            var hdnGSTTaxGL;
            var isGst = 0;
            var totamt = 0;
            var staxAmt = 0;
            var gtaxAmt = 0;
            var staxAmtGL = 0;
            var gtaxAmtGL = 0;

            txtGvPrice = document.getElementById(cb.replace('chkGTaxable', 'txtGvPrice'));
            txtGvQuan = document.getElementById(cb.replace('chkGTaxable', 'txtGvQuan'));
            txtGvAmount = document.getElementById(cb.replace('chkGTaxable', 'txtGvAmount'));
            //lblSalesTax = document.getElementById(cb.replace('chkTaxable', 'lblSalesTax'));
            lblGstTax = document.getElementById(cb.replace('chkGTaxable', 'lblGstTax'));
            lblAmountWithTax = document.getElementById(cb.replace('chkGTaxable', 'lblAmountWithTax'));
            hdnAmountWithTax = document.getElementById(cb.replace('chkGTaxable', 'hdnAmountWithTax'));
            hdnchkGTaxable = document.getElementById(cb.replace('chkGTaxable', 'hdnchkGTaxable'));
            hdnSTaxGL = document.getElementById(cb.replace('chkGTaxable', 'hdnSTaxGL'));
            hdnGSTTaxGL = document.getElementById(cb.replace('chkGTaxable', 'hdnGSTTaxGL'));
            hdnSTaxAm = document.getElementById(cb.replace('chkGTaxable', 'hdnSTaxAm'));
            txtGvStaxAmount = document.getElementById(cb.replace('chkGTaxable', 'txtGvStaxAmount'));
            hdnGSTTaxAm = document.getElementById(cb.replace('chkGTaxable', 'hdnGSTTaxAm'));

            hdnchkTaxable = document.getElementById(cb.replace('chkGTaxable', 'hdnchkTaxable'));


            staxAmt = parseFloat($(hdnSTaxAm).val());

            if (checkbox.checked == true) {
                $(hdnchkGTaxable).val('1');
            } else {
                $(hdnchkGTaxable).val('0');
            }


             <%--$('#<%=RadGrid_gvJobCostItems.ClientID%>').find('.stax-css').each(function () {
                        if ($(this).html() == "Sales Tax Amount") {
                            isGst = 0;
                        }
                        else {
                            isGst = 1;
                        }
            });--%>

            isGst = 1;
            //if (document.getElementById('txtgstgv').style.display == 'block')
            //{
            //    isGst = 1;
            //}
            //else
            //{
            //    isGst = 0;                     
            //}


            if (!jQuery.trim($(txtGvQuan).val()) == '') {
                if (isNaN(parseFloat($(txtGvQuan).val()))) {
                    $(txtGvQuan).val('0.00');
                }
            }

            if (!jQuery.trim($(txtGvPrice).val()) == '') {
                if (isNaN(parseFloat($(txtGvPrice).val()))) {
                    $(txtGvPrice).val('');
                }
            }

            if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
                if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
                    valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
                    $(txtGvAmount).val(valAmount.toFixed(2));
                    //$(txtGvAmount).val(cleanUpCurrency(parseFloat(valAmount).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                }
            } else if (!jQuery.trim($(txtGvQuan).val()) == '' && $(txtGvAmount).val() != '' && jQuery.trim($(txtGvPrice).val()) == '') {
                if (!isNaN(parseFloat($(txtGvQuan).val())) && parseFloat($(txtGvQuan).val()) != 0 && !isNaN(parseFloat($(txtGvAmount).val()))) {
                    var valPrice = parseFloat($(txtGvAmount).val()) / parseFloat($(txtGvQuan).val());
                    $(txtGvPrice).val(valPrice.toFixed(2));
                    //$(txtGvPrice).val(cleanUpCurrency(parseFloat(valPrice).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                }
            }




            if (checkbox.checked == true) {
                //if (isGst == 1) {
                if (gtax == null) {
                    gtaxAmt = 0.00;
                    gtaxAmtGL = 0;
                    $(lblGstTax).val(gtaxAmt.toFixed(2));

                    $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                    $(hdnGSTTaxGL).val(gtaxAmtGL);
                }
                else if (gtax.value != '') {
                    if (checkbox.checked == true) {
                        gtaxAmt = Math.round(((parseFloat(valAmount) * parseFloat(gtax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                        $(lblGstTax).val(gtaxAmt.toFixed(2));
                        $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                        gtaxAmtGL = parseInt(gtaxGL.value);
                        $(hdnGSTTaxGL).val(gtaxAmtGL.value);
                    }
                    else {
                        gtaxAmt = 0.00;
                        gtaxAmtGL = 0;
                    }
                    $(lblGstTax).val(gtaxAmt.toFixed(2));

                    $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                    $(hdnGSTTaxGL).val(gtaxAmtGL);


                    if ($(hdnchkTaxable).val() == '1') {
                        if (parseInt(staxType.value) == 1) {
                            var oldvalAmount = valAmount;
                            if (checkbox.checked == true) {
                                valAmount = parseFloat(valAmount) + gtaxAmt;
                            }
                            if (parseFloat(valAmount) < 0) {

                                staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                                staxAmt = staxAmt * (-1);
                                //staxAmtGL = parseInt(staxGL.value);
                                valAmount = oldvalAmount;

                            } else {
                                staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                                //staxAmtGL = parseInt(staxGL.value);
                                valAmount = oldvalAmount;
                            }
                        }
                    }
                    else {
                        staxAmt = 0;
                    }
                    $(hdnSTaxAm).val(parseFloat(staxAmt).toFixed(2));
                    $(txtGvStaxAmount).val(parseFloat(staxAmt).toFixed(2));








                }

            }
            else {
                gtaxAmt = 0.00;
                gtaxAmtGL = 0;
                $(lblGstTax).val(gtaxAmt.toFixed(2));

                $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                $(hdnGSTTaxGL).val(gtaxAmtGL);

                if ($(hdnchkTaxable).val() == '1') {
                    if (parseInt(staxType.value) == 1) {
                        var oldvalAmount = valAmount;
                        if (checkbox.checked == true) {
                            valAmount = parseFloat(valAmount) + gtaxAmt;
                        }
                        if (parseFloat(valAmount) < 0) {

                            staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmt = staxAmt * (-1);
                            //staxAmtGL = parseInt(staxGL.value);
                            valAmount = oldvalAmount;

                        } else {
                            staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            //staxAmtGL = parseInt(staxGL.value);
                            valAmount = oldvalAmount;
                        }
                    }
                }
                else {
                    staxAmt = 0;
                }

                $(hdnSTaxAm).val(parseFloat(staxAmt).toFixed(2));
                $(txtGvStaxAmount).val(parseFloat(staxAmt).toFixed(2));
            }

            //if (checkbox.checked == true) {
            //    if (parseInt(staxType.value) == 0 || parseInt(staxType.value) == 2) {
            //        if (parseFloat(valAmount) < 0) {

            //            staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
            //            staxAmt = staxAmt * (-1);
            //            staxAmtGL = parseInt(staxGL.value);

            //        } else {
            //            staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
            //            staxAmtGL = parseInt(staxGL.value);
            //        }
            //    }
            //    else if (parseInt(staxType.value) == 1) {
            //        var oldvalAmount = valAmount;
            //        if (isGst == 1) {
            //            valAmount = parseFloat(valAmount) + gtaxAmt;
            //        }
            //        if (parseFloat(valAmount) < 0) {

            //            staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
            //            staxAmt = staxAmt * (-1);
            //            staxAmtGL = parseInt(staxGL.value);
            //            valAmount = oldvalAmount;

            //        } else {
            //            staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
            //            staxAmtGL = parseInt(staxGL.value);
            //            valAmount = oldvalAmount;
            //        }
            //    }

            debugger;


            //} else {
            //    staxAmt = 0.00;
            //    staxAmtGL = 0;
            //}
            if (isNaN(staxAmt)) {

                staxAmt = 0.00;
            }
            if (isNaN(gtaxAmt)) {

                gtaxAmt = 0.00;
            }
            if (isNaN(valAmount)) {

                valAmount = 0.00;
            }


            //$(lblSalesTax).val(staxAmt.toFixed(2));
            //$(hdnSTaxAm).val(staxAmt.toFixed(2));
            //$(txtGvStaxAmount).val(staxAmt.toFixed(2));

            //$(hdnSTaxGL).val(staxAmtGL);



            totamt = valAmount + staxAmt;
            if (isGst == 1) {
                totamt = totamt + gtaxAmt;
            }
            $(lblAmountWithTax).text(totamt.toFixed(2));
            //$(hdnAmountWithTax).val(staxAmt.toFixed(2));
            $(hdnAmountWithTax).val(totamt.toFixed(2));
            //}

            //CalculateTotalAmtSST();
            CalculateTotalAmt();
        }
        function CalTotalValGtax1(checkbox) {
            //var cb = checkbox.id;
            var stax = document.getElementById('<%=hdnQST.ClientID%>');
            var gtax = document.getElementById('<%=hdnGST.ClientID%>');

            var staxGL = document.getElementById('<%=hdnQSTGL.ClientID%>');
            var gtaxGL = document.getElementById('<%=hdnGSTGL.ClientID%>');
            var staxType = document.getElementById('<%=hdnSTaxType.ClientID%>');
            var txtGvQuan;
            var txtGvPrice;
            var txtGvAmount;
            var lblSalesTax;
            var lblGstTax;
            var hdnGSTTaxAm;
            var hdnSTaxAm;
            var txtGvStaxAmount;
            var lblAmountWithTax;
            var valAmount;
            var hdnchkTaxable;
            var hdnSTaxGL;
            var hdnGSTTaxGL;
            var isGst = 0;
            var totamt = 0;
            var staxAmt = 0;
            var gtaxAmt = 0;
            var staxAmtGL = 0;
            var gtaxAmtGL = 0;

            txtGvPrice = document.getElementById(checkbox.replace('chkGTaxable', 'txtGvPrice'));
            txtGvQuan = document.getElementById(checkbox.replace('chkGTaxable', 'txtGvQuan'));
            txtGvAmount = document.getElementById(checkbox.replace('chkGTaxable', 'txtGvAmount'));
            //lblSalesTax = document.getElementById(cb.replace('chkTaxable', 'lblSalesTax'));
            lblGstTax = document.getElementById(checkbox.replace('chkGTaxable', 'lblGstTax'));
            lblAmountWithTax = document.getElementById(checkbox.replace('chkGTaxable', 'lblAmountWithTax'));
            hdnAmountWithTax = document.getElementById(checkbox.replace('chkGTaxable', 'hdnAmountWithTax'));
            hdnchkTaxable = document.getElementById(checkbox.replace('chkGTaxable', 'hdnchkGTaxable'));
            hdnSTaxGL = document.getElementById(checkbox.replace('chkGTaxable', 'hdnSTaxGL'));
            hdnGSTTaxGL = document.getElementById(checkbox.replace('chkGTaxable', 'hdnGSTTaxGL'));
            hdnSTaxAm = document.getElementById(checkbox.replace('chkGTaxable', 'hdnSTaxAm'));
            txtGvStaxAmount = document.getElementById(checkbox.replace('chkGTaxable', 'txtGvStaxAmount'));
            hdnGSTTaxAm = document.getElementById(checkbox.replace('chkGTaxable', 'hdnGSTTaxAm'));

            var cb = document.getElementById(checkbox);

            staxAmt = parseFloat($(hdnSTaxAm).val());

            if (cb.checked == true) {
                $(hdnchkTaxable).val('1');
            } else {
                $(hdnchkTaxable).val('0');
            }

            isGst = 1;


            if (!jQuery.trim($(txtGvQuan).val()) == '') {
                if (isNaN(parseFloat($(txtGvQuan).val()))) {
                    $(txtGvQuan).val('0.00');
                }
            }

            if (!jQuery.trim($(txtGvPrice).val()) == '') {
                if (isNaN(parseFloat($(txtGvPrice).val()))) {
                    $(txtGvPrice).val('');
                }
            }

            if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
                if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
                    valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
                    $(txtGvAmount).val(valAmount.toFixed(2));
                    //$(txtGvAmount).val(cleanUpCurrency(parseFloat(valAmount).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                }
            } else if (!jQuery.trim($(txtGvQuan).val()) == '' && $(txtGvAmount).val() != '' && jQuery.trim($(txtGvPrice).val()) == '') {
                if (!isNaN(parseFloat($(txtGvQuan).val())) && parseFloat($(txtGvQuan).val()) != 0 && !isNaN(parseFloat($(txtGvAmount).val()))) {
                    var valPrice = parseFloat($(txtGvAmount).val()) / parseFloat($(txtGvQuan).val());
                    $(txtGvPrice).val(valPrice.toFixed(2));
                    //$(txtGvPrice).val(cleanUpCurrency(parseFloat(valPrice).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                }
            }


            if (cb.checked == true) {
                //if (isGst == 1) {
                if (gtax == null) {
                    gtaxAmt = 0.00;
                    gtaxAmtGL = 0;
                    $(lblGstTax).val(gtaxAmt.toFixed(2));

                    $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                    $(hdnGSTTaxGL).val(gtaxAmtGL);
                }
                else if (gtax.value != '') {
                    if (cb.checked == true) {
                        gtaxAmt = Math.round(((parseFloat(valAmount) * parseFloat(gtax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                        $(lblGstTax).val(gtaxAmt.toFixed(2));
                        $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                        gtaxAmtGL = parseInt(gtaxGL.value);
                        $(hdnGSTTaxGL).val(gtaxAmtGL.value);
                    }
                    else {
                        gtaxAmt = 0.00;
                        gtaxAmtGL = 0;
                    }
                    $(lblGstTax).val(gtaxAmt.toFixed(2));

                    $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                    $(hdnGSTTaxGL).val(gtaxAmtGL);



                    if (parseInt(staxType.value) == 1) {
                        var oldvalAmount = valAmount;
                        if (isGst == 1) {
                            valAmount = parseFloat(valAmount) + gtaxAmt;
                        }
                        if (parseFloat(valAmount) < 0) {

                            staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmt = staxAmt * (-1);
                            //staxAmtGL = parseInt(staxGL.value);
                            valAmount = oldvalAmount;

                        } else {
                            staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            //staxAmtGL = parseInt(staxGL.value);
                            valAmount = oldvalAmount;
                        }
                    }
                    $(hdnSTaxAm).val(parseFloat(staxAmt).toFixed(2));
                    $(txtGvStaxAmount).val(parseFloat(staxAmt).toFixed(2));

                }

            }
            else {
                gtaxAmt = 0.00;
                gtaxAmtGL = 0;
                $(lblGstTax).val(gtaxAmt.toFixed(2));

                $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                $(hdnGSTTaxGL).val(gtaxAmtGL);

                if (parseInt(staxType.value) == 1) {
                    var oldvalAmount = valAmount;
                    if (isGst == 1) {
                        valAmount = parseFloat(valAmount) + gtaxAmt;
                    }
                    if (parseFloat(valAmount) < 0) {

                        staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                        staxAmt = staxAmt * (-1);
                        //staxAmtGL = parseInt(staxGL.value);
                        valAmount = oldvalAmount;

                    } else {
                        staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                        //staxAmtGL = parseInt(staxGL.value);
                        valAmount = oldvalAmount;
                    }
                }


                $(hdnSTaxAm).val(parseFloat(staxAmt).toFixed(2));
                $(txtGvStaxAmount).val(parseFloat(staxAmt).toFixed(2));
            }



            if (isNaN(staxAmt)) {

                staxAmt = 0.00;
            }
            if (isNaN(gtaxAmt)) {

                gtaxAmt = 0.00;
            }
            if (isNaN(valAmount)) {

                valAmount = 0.00;
            }

            totamt = valAmount + staxAmt;
            if (isGst == 1) {
                totamt = totamt + gtaxAmt;
            }
            $(lblAmountWithTax).text(totamt.toFixed(2));
            $(hdnAmountWithTax).val(totamt.toFixed(2));

            CalculateTotalAmt();
        }
        function CalTotalValStax1(checkbox) {

            //var cb = checkbox.id;
            var cb = document.getElementById(checkbox);
            var stax = document.getElementById('<%=hdnQST.ClientID%>');
            var gtax = document.getElementById('<%=hdnGST.ClientID%>');
            if (parseFloat(stax.value.toString().replace(/[\$\(\),]/g, '')) <= 0) {

                noty({
                    text: 'Please Set the Provincial Tax at vendor level',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 1500,
                    theme: 'noty_theme_default',
                    closable: true
                });
                $(hdnchkTaxable).val('0');
                cb.checked = false;

            }
            else {


                var staxGL = document.getElementById('<%=hdnQSTGL.ClientID%>');
                var gtaxGL = document.getElementById('<%=hdnGSTGL.ClientID%>');

                var staxType = document.getElementById('<%=hdnSTaxType.ClientID%>');

                var txtGvQuan;
                var txtGvPrice;
                var txtGvAmount;
                var lblSalesTax;
                var lblGstTax;
                var hdnGSTTaxAm;
                var hdnSTaxAm;
                var txtGvStaxAmount;
                var lblAmountWithTax;
                var valAmount;
                var hdnchkTaxable;
                var hdnSTaxGL;
                var hdnGSTTaxGL;
                var isGst = 0;
                var totamt = 0;
                var staxAmt = 0;
                var gtaxAmt = 0;
                var staxAmtGL = 0;
                var gtaxAmtGL = 0;

                txtGvPrice = document.getElementById(checkbox.replace('chkTaxable', 'txtGvPrice'));
                txtGvQuan = document.getElementById(checkbox.replace('chkTaxable', 'txtGvQuan'));
                txtGvAmount = document.getElementById(checkbox.replace('chkTaxable', 'txtGvAmount'));
                //lblSalesTax = document.getElementById(cb.replace('chkTaxable', 'lblSalesTax'));
                lblGstTax = document.getElementById(checkbox.replace('chkTaxable', 'lblGstTax'));
                lblAmountWithTax = document.getElementById(checkbox.replace('chkTaxable', 'lblAmountWithTax'));
                hdnAmountWithTax = document.getElementById(checkbox.replace('chkTaxable', 'hdnAmountWithTax'));
                hdnchkTaxable = document.getElementById(checkbox.replace('chkTaxable', 'hdnchkTaxable'));
                hdnSTaxGL = document.getElementById(checkbox.replace('chkTaxable', 'hdnSTaxGL'));
                hdnGSTTaxGL = document.getElementById(checkbox.replace('chkTaxable', 'hdnGSTTaxGL'));
                hdnSTaxAm = document.getElementById(checkbox.replace('chkTaxable', 'hdnSTaxAm'));
                txtGvStaxAmount = document.getElementById(checkbox.replace('chkTaxable', 'txtGvStaxAmount'));
                hdnGSTTaxAm = document.getElementById(checkbox.replace('chkTaxable', 'hdnGSTTaxAm'));

                gtaxAmt = parseFloat($(hdnGSTTaxAm).val());
                gtaxAmt = parseFloat(gtaxAmt) || 0;
                if (cb.checked == true) {
                    $(hdnchkTaxable).val('1');
                } else {
                    $(hdnchkTaxable).val('0');
                }

                isGst = 1;

                if (!jQuery.trim($(txtGvQuan).val()) == '') {
                    if (isNaN(parseFloat($(txtGvQuan).val()))) {
                        $(txtGvQuan).val('0.00');
                    }
                }

                if (!jQuery.trim($(txtGvPrice).val()) == '') {
                    if (isNaN(parseFloat($(txtGvPrice).val()))) {
                        $(txtGvPrice).val('');
                    }
                }

                if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
                    if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
                        valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
                        $(txtGvAmount).val(valAmount.toFixed(2));
                        //$(txtGvAmount).val(cleanUpCurrency(parseFloat(valAmount).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    }
                } else if (!jQuery.trim($(txtGvQuan).val()) == '' && $(txtGvAmount).val() != '' && jQuery.trim($(txtGvPrice).val()) == '') {
                    if (!isNaN(parseFloat($(txtGvQuan).val())) && parseFloat($(txtGvQuan).val()) != 0 && !isNaN(parseFloat($(txtGvAmount).val()))) {
                        var valPrice = parseFloat($(txtGvAmount).val()) / parseFloat($(txtGvQuan).val());
                        $(txtGvPrice).val(valPrice.toFixed(2));
                        //$(txtGvPrice).val(cleanUpCurrency(parseFloat(valPrice).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    }
                }

                if (cb.checked == true) {
                    if (parseInt(staxType.value) == 0 || parseInt(staxType.value) == 2) {
                        if (parseFloat(valAmount) < 0) {

                            staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmt = staxAmt * (-1);
                            staxAmtGL = parseInt(staxGL.value);

                        } else {
                            staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmtGL = parseInt(staxGL.value);
                        }
                    }
                    else if (parseInt(staxType.value) == 1) {
                        var oldvalAmount = valAmount;
                        //if (isGst == 1) {
                        valAmount = parseFloat(valAmount) + gtaxAmt;

                        if (parseFloat(valAmount) < 0) {

                            staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmt = staxAmt * (-1);
                            staxAmtGL = parseInt(staxGL.value);
                            valAmount = oldvalAmount;

                        } else {
                            staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmtGL = parseInt(staxGL.value);
                            valAmount = oldvalAmount;
                        }

                    }


                } else {
                    staxAmt = 0.00;
                    staxAmtGL = 0;
                }
                if (isNaN(staxAmt)) {

                    staxAmt = 0.00;
                }
                if (isNaN(gtaxAmt)) {

                    gtaxAmt = 0.00;
                }
                if (isNaN(valAmount)) {

                    valAmount = 0.00;
                }

                $(hdnSTaxAm).val(staxAmt.toFixed(2));
                $(txtGvStaxAmount).val(staxAmt.toFixed(2));

                $(hdnSTaxGL).val(staxAmtGL);

                totamt = valAmount + staxAmt;
                if (isGst == 1) {
                    totamt = totamt + gtaxAmt;
                }
                $(lblAmountWithTax).text(totamt.toFixed(2));
                $(hdnAmountWithTax).val(totamt.toFixed(2));
            }

            CalculateTotalAmt();
        }


        function TotalwithTax(txtGvStaxAmount) {
            debugger;
            var cb = txtGvStaxAmount.id;
            var stax = document.getElementById('<%=hdnQST.ClientID%>');
            var gtax = document.getElementById('<%=hdnGST.ClientID%>');

            var cbs = document.getElementById(cb.replace('txtGvStaxAmount', 'chkTaxable'));
            //var ch_id = chk.attr('id');
            //var cbs = document.getElementById(ch_id);

            var cbg = document.getElementById(cb.replace('txtGvStaxAmount', 'chkGTaxable'));
            //var ch_idg = chkg.attr('id');
            //var cbg = document.getElementById(ch_idg);

            if (parseFloat(stax.value.toString().replace(/[\$\(\),]/g, '')) <= 0) {

                noty({
                    text: 'Please Set the Provincial Tax at vendor level',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 1500,
                    theme: 'noty_theme_default',
                    closable: true
                });
                $(hdnchkTaxable).val('0');
                cbs.checked = false;

            }
            else {
                var staxGL = document.getElementById('<%=hdnQSTGL.ClientID%>');
                var gtaxGL = document.getElementById('<%=hdnGSTGL.ClientID%>');

                var staxType = document.getElementById('<%=hdnSTaxType.ClientID%>');

                var txtGvQuan;
                var txtGvPrice;
                var txtGvAmount;
                var lblSalesTax;
                var lblGstTax;
                var hdnGSTTaxAm;
                var hdnSTaxAm;
                var txtGvStaxAmount;
                var lblAmountWithTax;
                var valAmount;
                var hdnchkTaxable;
                var hdnSTaxGL;
                var hdnGSTTaxGL;
                var isGst = 0;
                var totamt = 0;
                var staxAmt = 0;
                var gtaxAmt = 0;
                var staxAmtGL = 0;
                var gtaxAmtGL = 0;

                txtGvPrice = document.getElementById(cb.replace('txtGvStaxAmount', 'txtGvPrice'));
                txtGvQuan = document.getElementById(cb.replace('txtGvStaxAmount', 'txtGvQuan'));
                txtGvAmount = document.getElementById(cb.replace('txtGvStaxAmount', 'txtGvAmount'));
                //lblSalesTax = document.getElementById(cb.replace('chkTaxable', 'lblSalesTax'));
                lblGstTax = document.getElementById(cb.replace('txtGvStaxAmount', 'lblGstTax'));
                lblAmountWithTax = document.getElementById(cb.replace('txtGvStaxAmount', 'lblAmountWithTax'));
                hdnAmountWithTax = document.getElementById(cb.replace('txtGvStaxAmount', 'hdnAmountWithTax'));
                hdnchkTaxable = document.getElementById(cb.replace('txtGvStaxAmount', 'hdnchkTaxable'));
                hdnSTaxGL = document.getElementById(cb.replace('txtGvStaxAmount', 'hdnSTaxGL'));
                hdnGSTTaxGL = document.getElementById(cb.replace('txtGvStaxAmount', 'hdnGSTTaxGL'));
                hdnSTaxAm = document.getElementById(cb.replace('txtGvStaxAmount', 'hdnSTaxAm'));
                txtGvStaxAmount = document.getElementById(cb.replace('txtGvStaxAmount', 'txtGvStaxAmount'));
                hdnGSTTaxAm = document.getElementById(cb.replace('txtGvStaxAmount', 'hdnGSTTaxAm'));

                isGst = 1;
                if (!jQuery.trim($(txtGvQuan).val()) == '') {
                    if (isNaN(parseFloat($(txtGvQuan).val()))) {
                        $(txtGvQuan).val('0.00');
                    }
                }

                if (!jQuery.trim($(txtGvPrice).val()) == '') {
                    if (isNaN(parseFloat($(txtGvPrice).val()))) {
                        $(txtGvPrice).val('');
                    }
                }

                if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
                    if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
                        valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
                        $(txtGvAmount).val(valAmount.toFixed(2));
                        //$(txtGvAmount).val(cleanUpCurrency(parseFloat(valAmount).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    }
                } else if (!jQuery.trim($(txtGvQuan).val()) == '' && $(txtGvAmount).val() != '' && jQuery.trim($(txtGvPrice).val()) == '') {
                    if (!isNaN(parseFloat($(txtGvQuan).val())) && parseFloat($(txtGvQuan).val()) != 0 && !isNaN(parseFloat($(txtGvAmount).val()))) {
                        var valPrice = parseFloat($(txtGvAmount).val()) / parseFloat($(txtGvQuan).val());
                        $(txtGvPrice).val(valPrice.toFixed(2));
                        //$(txtGvPrice).val(cleanUpCurrency(parseFloat(valPrice).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    }
                }

                gtaxAmt = parseFloat($(hdnGSTTaxAm).val());
                //gtaxAmtGL = parseInt(gtaxGL.value);
                gtaxAmt = parseFloat(gtaxAmt) || 0;
                if (gtax == null) {
                    gtaxAmt = 0.00;
                    gtaxAmtGL = 0;
                    $(lblGstTax).val(gtaxAmt.toFixed(2));

                    $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                    $(hdnGSTTaxGL).val(gtaxAmtGL);
                }
                else if (gtax.value != '') {
                    if (cbg.checked == true) {

                        $(lblGstTax).val(gtaxAmt.toFixed(2));
                        $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                        $(hdnGSTTaxGL).val(gtaxAmtGL.value);
                    }
                    else {
                        gtaxAmt = 0.00;
                        gtaxAmtGL = 0;
                    }
                    $(lblGstTax).val(gtaxAmt.toFixed(2));

                    $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                    $(hdnGSTTaxGL).val(gtaxAmtGL);

                }






                if (cbs.checked == true) {
                    staxAmt = parseFloat($(txtGvStaxAmount).val());
                    staxAmtGL = parseInt(staxGL.value);


                } else {
                    staxAmt = 0.00;
                    staxAmtGL = 0;
                }
                if (isNaN(staxAmt)) {

                    staxAmt = 0.00;
                }
                if (isNaN(gtaxAmt)) {

                    gtaxAmt = 0.00;
                }
                if (isNaN(valAmount)) {

                    valAmount = 0.00;
                }


                //$(lblSalesTax).val(staxAmt.toFixed(2));
                $(hdnSTaxAm).val(staxAmt.toFixed(2));
                $(txtGvStaxAmount).val(staxAmt.toFixed(2));

                $(hdnSTaxGL).val(staxAmtGL);



                totamt = valAmount + staxAmt;
                if (isGst == 1) {
                    totamt = totamt + gtaxAmt;
                }
                $(lblAmountWithTax).text(totamt.toFixed(2));
                //$(hdnAmountWithTax).val(staxAmt.toFixed(2));
                $(hdnAmountWithTax).val(totamt.toFixed(2));


                CalculateTotalAmt();




            }
        }
        function CalTotalVal(obj) {
            
            var txt = obj.id;

            var txtGvQuan;
            var txtGvPrice;
            var txtGvAmount;
            var lblAmountWithTax;
            var hdnAmountWithTax;
            var hdnchkTaxable;
            var chkTaxable;

            var lblSalesTax;
            var hdnSTaxAm;
            var lblGstTax;
            var hdnGSTTaxAm;

            var stax = document.getElementById('<%=hdnQST.ClientID%>');
            var gtax = document.getElementById('<%=hdnGST.ClientID%>');

            if (txt.indexOf("Quan") >= 0) {
                txtGvQuan = document.getElementById(txt);
                txtGvPrice = document.getElementById(txt.replace('txtGvQuan', 'txtGvPrice'));
                txtGvAmount = document.getElementById(txt.replace('txtGvQuan', 'txtGvAmount'));
                lblAmountWithTax = document.getElementById(txt.replace('txtGvQuan', 'lblAmountWithTax'));
                hdnAmountWithTax = document.getElementById(txt.replace('txtGvQuan', 'hdnAmountWithTax'));
                hdnchkTaxable = document.getElementById(txt.replace('txtGvQuan', 'hdnchkTaxable'));
                chkTaxable = document.getElementById(txt.replace('txtGvQuan', 'chkTaxable'));

                lblSalesTax = document.getElementById(txt.replace('txtGvQuan', 'lblSalesTax'));
                hdnSTaxAm = document.getElementById(txt.replace('txtGvQuan', 'hdnSTaxAm'));
                lblGstTax = document.getElementById(txt.replace('txtGvQuan', 'lblGstTax'));
                hdnGSTTaxAm = document.getElementById(txt.replace('txtGvQuan', 'hdnGSTTaxAm'));
            }
            else if (txt.indexOf("Price") >= 0) {
                txtGvPrice = document.getElementById(txt);
                txtGvQuan = document.getElementById(txt.replace('txtGvPrice', 'txtGvQuan'));
                txtGvAmount = document.getElementById(txt.replace('txtGvPrice', 'txtGvAmount'));
                lblAmountWithTax = document.getElementById(txt.replace('txtGvPrice', 'lblAmountWithTax'));
                hdnAmountWithTax = document.getElementById(txt.replace('txtGvPrice', 'hdnAmountWithTax'));
                hdnchkTaxable = document.getElementById(txt.replace('txtGvPrice', 'hdnchkTaxable'));
                chkTaxable = document.getElementById(txt.replace('txtGvPrice', 'chkTaxable'));

                lblSalesTax = document.getElementById(txt.replace('txtGvPrice', 'lblSalesTax'));
                hdnSTaxAm = document.getElementById(txt.replace('txtGvPrice', 'hdnSTaxAm'));
                lblGstTax = document.getElementById(txt.replace('txtGvPrice', 'lblGstTax'));
                hdnGSTTaxAm = document.getElementById(txt.replace('txtGvPrice', 'hdnGSTTaxAm'));
            }
            else if (txt.indexOf("Amount") >= 0) {
                txtGvPrice = document.getElementById(txt.replace('txtGvAmount', 'txtGvPrice'));
                txtGvQuan = document.getElementById(txt.replace('txtGvAmount', 'txtGvQuan'));
                lblAmountWithTax = document.getElementById(txt.replace('txtGvAmount', 'lblAmountWithTax'));
                hdnAmountWithTax = document.getElementById(txt.replace('txtGvAmount', 'hdnAmountWithTax'));
                txtGvAmount = document.getElementById(txt);
                hdnchkTaxable = document.getElementById(txt.replace('txtGvAmount', 'hdnchkTaxable'));
                chkTaxable = document.getElementById(txt.replace('txtGvAmount', 'chkTaxable'));

                lblSalesTax = document.getElementById(txt.replace('txtGvAmount', 'lblSalesTax'));
                hdnSTaxAm = document.getElementById(txt.replace('txtGvAmount', 'hdnSTaxAm'));
                lblGstTax = document.getElementById(txt.replace('txtGvAmount', 'lblGstTax'));
                hdnGSTTaxAm = document.getElementById(txt.replace('txtGvAmount', 'hdnGSTTaxAm'));
            }
            //else if (txt.indexOf("AmountTot") >= 0) {
            //    txtGvPrice = document.getElementById(txt.replace('lblAmountWithTax', 'txtGvPrice'));
            //    txtGvQuan = document.getElementById(txt.replace('lblAmountWithTax', 'txtGvQuan'));
            //    txtGvAmount = document.getElementById(txt.replace('lblAmountWithTax', 'txtGvAmount'));
            //    lblAmountWithTax = document.getElementById(txt);
            //}
             

            if (!jQuery.trim($(txtGvQuan).val()) == '') {
                if (isNaN(parseFloat($(txtGvQuan).val()))) {
                    $(txtGvQuan).val('0.00');
                }
            }

            if (!jQuery.trim($(txtGvPrice).val()) == '') {
                if (isNaN(parseFloat($(txtGvPrice).val()))) {
                    $(txtGvPrice).val('');
                }
            }

            if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
                if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
                    var valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
                    $(txtGvAmount).val(valAmount.toFixed(2));
                    $(lblAmountWithTax).text(valAmount.toFixed(2));
                    $(hdnAmountWithTax).val(valAmount.toFixed(2));
                    
                    //$(hdnchkTaxable).val("0");
                    //chkTaxable.checked = false;

                    //$(lblSalesTax).val("0.00");
                    //$(lblGstTax).val("0.00");
                    //$(hdnSTaxAm).val("0.00");
                    //$(hdnGSTTaxAm).val("0.00");

                    if ($(hdnchkTaxable).val() == "0") {
                        $(hdnchkTaxable).val("0");
                        if (chkTaxable != null)
{
                        chkTaxable.checked = false;
}

                        $(lblSalesTax).val("0.00");
                        $(lblGstTax).val("0.00");
                        $(hdnSTaxAm).val("0.00");
                        $(hdnGSTTaxAm).val("0.00");
                        //CalTotalValStax(chkTaxable);
                    }
                    else {
                        $(hdnchkTaxable).val("1");
                        if (chkTaxable != null) {
                            chkTaxable.checked = true;
                            CalTotalValStax(chkTaxable);
                        }
                        
                    }

                    //$(txtGvAmount).val(cleanUpCurrency(parseFloat(valAmount).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                }
            } else if (!jQuery.trim($(txtGvQuan).val()) == '' && $(txtGvAmount).val() != '' && jQuery.trim($(txtGvPrice).val()) == '') {
                if (!isNaN(parseFloat($(txtGvQuan).val())) && parseFloat($(txtGvQuan).val()) != 0 && !isNaN(parseFloat($(txtGvAmount).val()))) {
                    var valPrice = parseFloat($(txtGvAmount).val()) / parseFloat($(txtGvQuan).val());
                    $(txtGvPrice).val(valPrice.toFixed(2));
                    $(lblAmountWithTax).text($(txtGvAmount).val());
                    $(hdnAmountWithTax).val($(txtGvAmount).val());
                    
                    //$(hdnchkTaxable).val("0");
                    //chkTaxable.checked = false;

                    //$(lblSalesTax).val("0.00");
                    //$(lblGstTax).val("0.00");
                    //$(hdnSTaxAm).val("0.00");
                    //$(hdnGSTTaxAm).val("0.00");

                    if ($(hdnchkTaxable).val() == "0") {
                        $(hdnchkTaxable).val("0");
                        if (chkTaxable != null)
{
                        chkTaxable.checked = false;
}

                        $(lblSalesTax).val("0.00");
                        $(lblGstTax).val("0.00");
                        $(hdnSTaxAm).val("0.00");
                        $(hdnGSTTaxAm).val("0.00");
                        //CalTotalValStax(chkTaxable);
                    }
                    else {
                        $(hdnchkTaxable).val("1");
                        if (chkTaxable != null) {
                            chkTaxable.checked = true;
                            CalTotalValStax(chkTaxable);
                        }
                        
                    }

                    //$(txtGvPrice).val(cleanUpCurrency(parseFloat(valPrice).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                }
            }
            //CalTotalValStax(chkTaxable);
            CalculateTotalAmt();

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }
        function showfreq() {
            if (document.getElementById('<%=chkIsRecurr.ClientID%>').checked) {
                displayapplycredit("0");
                document.getElementById('dvfreq').style.display = 'block';
                var bqs = GetParameterValues('bill');
            var vqs = GetParameterValues('vid');
            var rqs = GetParameterValues('ref');
                if (bqs == 'c' && vqs == null && rqs == null) {
                    document.getElementById('<%=adEditPayment.ClientID%>').style.display = 'none';

                    <%--$('#<%=liaccrdGlAccount.ClientID %>').addClass('active');
                    $('#<%=accrdGlAccount.ClientID %>').addClass('active');--%>

                    var tAmount = 0.00;
                if ($find("<%=RadGrid_gvJobCostItems.ClientID%>") != null) {
                    var masterTable = $find("<%=RadGrid_gvJobCostItems.ClientID%>").get_masterTableView();
                    var count = masterTable.get_dataItems().length;
                    var item;
                    for (var i = 0; i < count; i++) {
                        item = masterTable.get_dataItems()[i];
                        var Qty = item.findElement("txtGvQuan");
                        var Amount = item.findElement("txtGvAmount");
                        var Price = item.findElement("txtGvPrice");
                        var QtyVal = $(Qty).val();
                        var AmountVal = $(Amount).val();
                        if (QtyVal != "" && AmountVal != "") {
                            if (!isNaN(parseFloat(QtyVal)) && !isNaN(parseFloat(AmountVal)) && parseFloat(QtyVal) != 0) {
                                var QtyPrice = parseFloat(AmountVal) / parseFloat(QtyVal);
                                tAmount = tAmount + parseFloat(AmountVal);
                            }
                        }
                    }
                }
                $('#<%=lblTotalAmount.ClientID%>').text(parseFloat(tAmount).toFixed(2)); 
                    $('#<%=lblTotalAmount11.ClientID%>').text(parseFloat(tAmount).toFixed(2));
                    $("#<%=lblTotalAmount11.ClientID%>").html(cleanUpCurrency("$" + parseFloat(parseFloat(tAmount)).toLocaleString("en-US", { minimumFractionDigits: 2 })));    
                    $('[id*=lblTotalAmt]').text(parseFloat(tAmount).toFixed(2));

                    var _currencyInWord = inWords(parseFloat(Math.trunc(parseFloat(tAmount))));
            var d = parseFloat(tAmount) - Math.trunc(parseFloat(tAmount));
            if (d > 0) {
                d = Math.round(d * 100);
                _currencyInWord = _currencyInWord + " And " + d + " / 100";
            }
            _currencyInWord = "*** " + _currencyInWord + "****************";
             $("#<%=lblDollar.ClientID%>").html(_currencyInWord);
            <%--$("#<%=hdnTPay.ClientID%>").val(tAmount.toString());--%>
                    $("#<%=hdnTPay.ClientID%>").val(parseFloat(tAmount).toString());
                                        
                }
            } else {
                document.getElementById('dvfreq').style.display = 'none';
                document.getElementById('<%=adEditPayment.ClientID%>').style.display = 'block';
                CalculateTotalAmt();
                
                
            }
        }
        function CallSelectedIndexChanged() {
        __doPostBack("<%=ddlVendor.UniqueID %>", "");
        }
        function quickcheckvalid() {
            var rtvalue = false;
            var v = document.getElementById("<%=rfvbillref.ClientID%>");
            var v1 = document.getElementById("<%=rfvMemo1.ClientID%>");
            ValidatorValidate(v);
            ValidatorValidate(v1);
            if (v.isvalid && v1.isvalid)
                rtvalue = true;
            else
                rtvalue = false;
            return rtvalue;

        }
        function showbillgl() {
            $("#<%=liaccrdGlAccount.ClientID %>").show();
        }
        function  checkvalidation() {
            var bqs = GetParameterValues('bill');
            var vqs = GetParameterValues('vid');
            var rqs = GetParameterValues('ref');
            if (bqs == 'c') {
                if (vqs == null) {
                    ValidatorEnable($('#<%=rfvtxtvendor.ClientID %>')[0], true);
                    ValidatorEnable($('#<%=cvDiscGLs.ClientID %>')[0], true);
                    ValidatorEnable($('#<%=cvDiscGL.ClientID %>')[0], false);
                    var tAmount = 0.00;


                    if ($find("<%=RadGrid_gvJobCostItems.ClientID%>") != null) {
                        var masterTable = $find("<%=RadGrid_gvJobCostItems.ClientID%>").get_masterTableView();
                        var count = masterTable.get_dataItems().length;
                        var item;
                        for (var i = 0; i < count; i++) {
                            item = masterTable.get_dataItems()[i];
                            var Qty = item.findElement("txtGvQuan");
                            var Amount = item.findElement("txtGvAmount");
                            var Price = item.findElement("txtGvPrice");
                            var QtyVal = $(Qty).val();
                            var AmountVal = $(Amount).val();
                            if (QtyVal != "" && AmountVal != "") {
                                if (!isNaN(parseFloat(QtyVal)) && !isNaN(parseFloat(AmountVal)) && parseFloat(QtyVal) != 0) {
                                    var QtyPrice = parseFloat(AmountVal) / parseFloat(QtyVal);
                                    tAmount = tAmount + parseFloat(AmountVal);
                                }
                            }
                        }
                    }
                    
                    var tAmt = parseFloat($('#<%=lblTotalAmount11.ClientID%>').text().replace(/[\$\(\),]/g, ''));
                    

                    if (parseFloat(tAmt) > 0) {
                        ValidatorEnable($('#<%=rfvbillref.ClientID %>')[0], true);
                        ValidatorEnable($('#<%=rfvMemo1.ClientID %>')[0], true);
                        
                        
                    }
                    else {
                        ValidatorEnable($('#<%=rfvbillref.ClientID %>')[0], false);
                        ValidatorEnable($('#<%=rfvMemo1.ClientID %>')[0], false);
                        
                        
                    }
                    if (quickcheckvalid() == true && parseFloat(tAmt) > 0) {
                        $("#<%=liaccrdGlAccount.ClientID %>").hide();
                    }
                    else {
                        return;
                    }

                    <%--if ($find("<%=gvBills.ClientID%>") != null) {
                        var masterTables = $find("<%=gvBills.ClientID%>").get_masterTableView();
                        var counts = masterTables.get_dataItems().length;
                        var items;
                        for (var i = 0; i < counts; i++) {
                            items = masterTables.get_dataItems()[i];
                            
                            var chkSelect = items.findElement("chkSelect");
                            var txtGvDisc = items.findElement("txtGvDisc");
                            var txtGvPay = items.findElement("txtGvPay");
                            chkSelect.disabled = true;
                            txtGvDisc.disabled = true;
                            txtGvPay.disabled = true;
                            
                        }
                    }

                    

                    if ($find("<%=RadGrid_gvJobCostItems.ClientID%>") != null) {
                        var masterTables = $find("<%=RadGrid_gvJobCostItems.ClientID%>").get_masterTableView();
                        var counts = masterTables.get_dataItems().length;
                        var items;
                        for (var i = 0; i < counts; i++) {
                            items = masterTables.get_dataItems()[i];
                            
                            var txtGvJob = items.findElement("txtGvJob");
                            var txtGvTicket = items.findElement("txtGvTicket");
                            var txtGvPhase = items.findElement("txtGvPhase");
                            var txtGvItem = items.findElement("txtGvItem");
                            var txtGvDesc = items.findElement("txtGvDesc");
                            var txtGvAcctNo = items.findElement("txtGvAcctNo");
                            var txtGvQuan = items.findElement("txtGvQuan");
                            var txtGvPrice = items.findElement("txtGvPrice");
                            var txtGvAmount = items.findElement("txtGvAmount");
                            var txtGvUseTax = items.findElement("txtGvUseTax");
                            var chkTaxable = items.findElement("chkTaxable");
                            var lblSalesTax = items.findElement("lblSalesTax");
                            var lblGstTax = items.findElement("lblGstTax");
                            var txtGvLoc = items.findElement("txtGvLoc");
                            var ibDelete = items.findElement("ibDelete");
                            
                            txtGvJob.disabled = true;
                            txtGvTicket.disabled = true;
                            txtGvPhase.disabled = true;
                            txtGvItem.disabled = true;
                            txtGvDesc.disabled = true;
                            txtGvAcctNo.disabled = true;
                            txtGvQuan.disabled = true;
                            txtGvPrice.disabled = true;
                            txtGvAmount.disabled = true;
                            if (txtGvUseTax != null) {
                                txtGvUseTax.disabled = true;
                            }
                            if (chkTaxable != null) {
                                chkTaxable.disabled = true;
                            }
                            if (lblSalesTax != null) {
                                lblSalesTax.disabled = true;
                            }
                            if (lblGstTax != null) {
                                lblGstTax.disabled = true;
                            }
                            txtGvLoc.disabled = true;
                            ibDelete.disabled = true;
                        }
                        
                        
                    }
                    document.getElementById('<%=btnAddNewLines.ClientID%>').style.display  = 'none'; --%>

                }
                else {
                    ValidatorEnable($('#<%=rfvtxtvendor.ClientID %>')[0], false);
                    ValidatorEnable($('#<%=cvDiscGLs.ClientID %>')[0], false);
                    ValidatorEnable($('#<%=cvDiscGL.ClientID %>')[0], true);
                }

            }
            else {
                
                ValidatorEnable($('#<%=rfvtxtvendor.ClientID %>')[0], false);
                ValidatorEnable($('#<%=rfvbillref.ClientID %>')[0], false);
                ValidatorEnable($('#<%=rfvMemo1.ClientID %>')[0], false);
                ValidatorEnable($('#<%=cvDiscGLs.ClientID %>')[0], false);
                ValidatorEnable($('#<%=cvDiscGL.ClientID %>')[0], true);
                
                $("#<%=liaccrdGlAccount.ClientID %>").hide();

                    <%--if ($find("<%=gvBills.ClientID%>") != null) {
                        var masterTables = $find("<%=gvBills.ClientID%>").get_masterTableView();
                        var counts = masterTables.get_dataItems().length;
                        var items;
                        for (var i = 0; i < counts; i++) {
                            items = masterTables.get_dataItems()[i];
                            
                            var chkSelect = items.findElement("chkSelect");
                            var txtGvDisc = items.findElement("txtGvDisc");
                            var txtGvPay = items.findElement("txtGvPay");
                            chkSelect.disabled = true;
                            txtGvDisc.disabled = true;
                            txtGvPay.disabled = true;
                            
                        }
                    }
                if ($find("<%=RadGrid_gvJobCostItems.ClientID%>") != null) {
                        var masterTables = $find("<%=RadGrid_gvJobCostItems.ClientID%>").get_masterTableView();
                        var counts = masterTables.get_dataItems().length;
                        var items;
                        for (var i = 0; i < counts; i++) {
                            items = masterTables.get_dataItems()[i];
                            
                            var txtGvJob = items.findElement("txtGvJob");
                            var txtGvTicket = items.findElement("txtGvTicket");
                            var txtGvPhase = items.findElement("txtGvPhase");
                            var txtGvItem = items.findElement("txtGvItem");
                            var txtGvDesc = items.findElement("txtGvDesc");
                            var txtGvAcctNo = items.findElement("txtGvAcctNo");
                            var txtGvQuan = items.findElement("txtGvQuan");
                            var txtGvPrice = items.findElement("txtGvPrice");
                            var txtGvAmount = items.findElement("txtGvAmount");
                            var txtGvUseTax = items.findElement("txtGvUseTax");
                            var chkTaxable = items.findElement("chkTaxable");
                            var lblSalesTax = items.findElement("lblSalesTax");
                            var lblGstTax = items.findElement("lblGstTax");
                            var txtGvLoc = items.findElement("txtGvLoc");
                            var ibDelete = items.findElement("ibDelete");
                            txtGvJob.disabled = true;
                            txtGvTicket.disabled = true;
                            txtGvPhase.disabled = true;
                            txtGvItem.disabled = true;
txtGvDesc.disabled = true;
txtGvAcctNo.disabled = true;
txtGvQuan.disabled = true;
txtGvPrice.disabled = true;
txtGvAmount.disabled = true;
if (txtGvUseTax != null) {
                                txtGvUseTax.disabled = true;
                            }
                            if (chkTaxable != null) {
                                chkTaxable.disabled = true;
                            }
                            if (lblSalesTax != null) {
                                lblSalesTax.disabled = true;
                            }
                            if (lblGstTax != null) {
                                lblGstTax.disabled = true;
                            }
txtGvLoc.disabled = true;
ibDelete.disabled = true;                            
                    }
                    
                    }
                document.getElementById('<%=btnAddNewLines.ClientID%>').style.display = 'none'; --%>
            }
        }

        function  checkvalidationsave() {
            
                
            ValidatorEnable($('#<%=cvDiscGL.ClientID %>')[0], true);
            ValidatorEnable($('#<%=rfvBank.ClientID %>')[0], true);
            ValidatorEnable($('#<%=rfvPayment.ClientID %>')[0], true);
            ValidatorEnable($('#<%=rfvNextCheck.ClientID %>')[0], true);
            ValidatorEnable($('#<%=rfvDate.ClientID %>')[0], true);
            ValidatorEnable($('#<%=revDate.ClientID %>')[0], true);
            
                
               
        }

        
            function Chkbillmemo(s, args) {
                var bill = GetParameterValues('bill');
                if (bill == "c") {
                    var tAmount = 0.00;
                    var masterTable = $find("<%=RadGrid_gvJobCostItems.ClientID%>").get_masterTableView();
                    var count = masterTable.get_dataItems().length;
                    var item;
                    for (var i = 0; i < count; i++) {
                        item = masterTable.get_dataItems()[i];
                        var Qty = item.findElement("txtGvQuan");
                        var Amount = item.findElement("txtGvAmount");
                        var Price = item.findElement("txtGvPrice");
                        var QtyVal = $(Qty).val();
                        var AmountVal = $(Amount).val();
                        if (QtyVal != "" && AmountVal != "") {
                            if (!isNaN(parseFloat(QtyVal)) && !isNaN(parseFloat(AmountVal)) && parseFloat(QtyVal) != 0) {
                                var QtyPrice = parseFloat(AmountVal) / parseFloat(QtyVal);
                                tAmount = tAmount + parseFloat(AmountVal);
                            }
                        }
                    }
                    if (parseFloat(tAmount) > 0) {
                        args.IsValid = args.Value != '';
                    }
                    else {
                        args.IsValid = true;
                    }


                }
                else {
                    args.IsValid = true;
                }
            }
        function Chkbillref(s, args) {
            var bill = GetParameterValues('bill');
        if(bill =="c"){
            var tAmount = 0.00;
                var masterTable = $find("<%=RadGrid_gvJobCostItems.ClientID%>").get_masterTableView();
                var count = masterTable.get_dataItems().length;
                var item;
                for (var i = 0; i < count; i++) {
                    item = masterTable.get_dataItems()[i];
                    var Qty = item.findElement("txtGvQuan");
                    var Amount = item.findElement("txtGvAmount");
                    var Price = item.findElement("txtGvPrice");
                    var QtyVal = $(Qty).val();
                    var AmountVal = $(Amount).val();
                    if (QtyVal != "" && AmountVal != "") {
                        if (!isNaN(parseFloat(QtyVal)) && !isNaN(parseFloat(AmountVal)) && parseFloat(QtyVal) != 0) {
                            var QtyPrice = parseFloat(AmountVal) / parseFloat(QtyVal);
                             tAmount = tAmount + parseFloat(AmountVal);
                        } 
                    }
                }
                if (parseFloat(tAmount) > 0) {
                    args.IsValid = args.Value != '';
                }
                else {
                    args.IsValid = true;
                }

        }
        else{
            args.IsValid = true;
        }
    }
    </script> 
</asp:Content>


