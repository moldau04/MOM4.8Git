<%@ Page Title="" Language="C#" MasterPageFile="~/MOM.master" AutoEventWireup="true" Inherits="AddBills" CodeBehind="AddBills.aspx.cs" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <style>
        .rgRow > td {
            font-size: 0.9em !important;
        }

        .rgAltRow > td {
            font-size: 0.9em !important;
        }

        .hidden {
            visibility: hidden;
        }

        .form-section3-div4 {
            width: 23%;
            float: left;
        }

        .form-section3-div4blank {
            float: left;
            width: 2.5%;
        }

        .colbl-customval {
            text-align: right;
        }

        .highlight {
            background-color: Yellow;
        }

        .highlighted {
            background-color: Yellow;
        }

        ul.anchor-links li a {
            border-bottom: 1px groove !important;
        }
         .card {
            overflow: hidden;
            min-height: 183px !important;
            border-radius: 6px;
        }
    </style>
    <style type="text/css">
        @media screen and (max-width: 2048px) {

            #ctl00_ContentPlaceHolder1_RadGrid_gvPayment_GridData {
                height: 30vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }

        @media screen and (max-width: 2304px) {

            #ctl00_ContentPlaceHolder1_RadGrid_gvPayment_GridData {
                height: 32vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }

        @media screen and (max-width: 1920px) {

            #ctl00_ContentPlaceHolder1_RadGrid_gvPayment_GridData {
                height: 27vh !important;
            }
        }

        @media screen and (max-width: 1706px) {

            #ctl00_ContentPlaceHolder1_RadGrid_gvPayment_GridData {
                height: 22vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }

        @media screen and (max-width: 1688px) {

            #ctl00_ContentPlaceHolder1_RadGrid_gvPayment_GridData {
                height: 22vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }

        @media screen and (max-width: 1366px) {

            #ctl00_ContentPlaceHolder1_RadGrid_gvPayment_GridData {
                height: 10vh !important;
            }

            .RadGrid_Material {
                font-size: 0.9rem !important;
            }
        }
    </style>
    

    <%--     <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <%--    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>--%>

    <script type="text/javascript">
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
        function testconfirmSubmit() {

            itemJSON();
            var stax = document.getElementById('<%=hdnQST.ClientID%>');
            var gtax = document.getElementById('<%=hdnGST.ClientID%>');
            var staxType = document.getElementById('<%=hdnSTaxType.ClientID%>');

            var masterTable = $find("<%=RadGrid_gvJobCostItems.ClientID%>").get_masterTableView();
            var count = masterTable.get_dataItems().length;
            var item;
            for (var i = 0; i < count; i++) {
                item = masterTable.get_dataItems()[i];

                var valAmount;

                var isGst = 0;
                var totamt = 0;
                var staxAmt = 0;
                var gtaxAmt = 0;
                var staxAmtCurr = 0;
                var gtaxAmtCurr = 0;

                var gtaxAmtMax = 0;
                var gtaxAmtMin = 0;
                var staxAmtMax = 0;
                var staxAmtMin = 0;

                var txtGvQuan = item.findElement("txtGvQuan");
                var txtGvAmount = item.findElement("txtGvAmount");
                var txtGvPrice = item.findElement("txtGvPrice");

                var lblAmountWithTax = item.findElement("lblAmountWithTax");
                var hdnAmountWithTax = item.findElement("hdnAmountWithTax");

                //////////// PO QTY /AMOUNT CALCULATION //////////////////
                var hdnOutstandQuan = item.findElement("hdnOutstandQuan");
                var hdnOutstandBalance = item.findElement("hdnOutstandBalance");
                var hdnIsPO = item.findElement("hdnIsPO");

                var chkTaxable = item.findElement("chkTaxable");
                var hdnchkTaxable = item.findElement("hdnchkTaxable");
                var txtGvStaxAmount = item.findElement("txtGvStaxAmount");
                var hdnSTaxAm = item.findElement("hdnSTaxAm");

                var chkGTaxable = item.findElement("chkGTaxable");
                var hdnchkGTaxable = item.findElement("hdnchkGTaxable");
                var lblGstTax = item.findElement("lblGstTax");
                var hdnGSTTaxAm = item.findElement("hdnGSTTaxAm");

                staxAmtCurr = parseFloat($(txtGvStaxAmount).val());
                gtaxAmtCurr = parseFloat($(lblGstTax).val());


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


                if ($(hdnchkGTaxable).val() == '1') {
                    gtaxAmt = Math.round(((parseFloat(valAmount) * parseFloat(gtax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                }
                if ($(hdnchkTaxable).val() == '1') {
                    if (parseInt(staxType.value) == 0 || parseInt(staxType.value) == 2) {
                        if (parseFloat(valAmount) < 0) {
                            staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            //staxAmt = parseFloat(txtGvStaxAmount.val());
                            staxAmt = staxAmt * (-1);

                        } else {
                            staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            //staxAmt = parseFloat(txtGvStaxAmount.val());
                            //staxAmtGL = parseInt(staxGL.value);
                        }
                    }
                    else if (parseInt(staxType.value) == 1) {
                        var oldvalAmount = valAmount;
                        if (isGst == 1) {
                            valAmount = parseFloat(valAmount) + gtaxAmt;
                        }
                        if (parseFloat(valAmount) < 0) {
                            staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            //staxAmt = parseFloat(txtGvStaxAmount.val());
                            staxAmt = staxAmt * (-1);
                            //staxAmtGL = parseInt(staxGL.value);
                            valAmount = oldvalAmount;

                        } else {
                            staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            //staxAmt = parseFloat(txtGvStaxAmount.val());
                            //staxAmtGL = parseInt(staxGL.value);
                            valAmount = oldvalAmount;
                        }
                    }
                } else {
                    staxAmt = 0.00;
                    //staxAmtGL = 0;

                }

                gtaxAmtMax = parseFloat(gtaxAmt) + parseFloat(0.10);
                gtaxAmtMin = parseFloat(gtaxAmt) - parseFloat(0.10);
                staxAmtMax = parseFloat(staxAmt) + parseFloat(0.10);
                staxAmtMin = parseFloat(staxAmt) - parseFloat(0.10);

                if ((gtaxAmtCurr > gtaxAmtMax || gtaxAmtCurr < gtaxAmtMin) || (staxAmtCurr > staxAmtMax || staxAmtCurr < staxAmtMin)) {
                    var r = confirm("Please note there is a discrepancy in the calculated Tax [GST/HST/PST]. Are you sure you want to proceed?");
                    if (r == true) {
                        return;
                    } else {
                        return false;
                    }
                }

            }

        }
        function OpenErrorModal() {
            window.radopen(null, "errorWindow");
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.radWindow;
            else if (window.frameElement && window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow;
            return oWindow;
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


        function ConfirmRef(control) {
            debugger;
            disableControl(control);
            var retValEdit = false;
            var editCase = $('#<%=hdEditCase.ClientID%>').val();
            if (editCase === "true") {
                var qrStr = window.location.search;
                //qrStr = qrStr.split("?")[1].split("=")[1];
                qrStr = GetParameterValues('id');
                itemJSON();
                var urlsedit;
                if (document.getElementById('<%=chkIsRecurr.ClientID%>').checked) {
                    urlsedit = "CustomerAuto.asmx/GetBillRecurrRefExistEditAPBILL";
                }
                else {
                    urlsedit = "CustomerAuto.asmx/GetBillRefExistEditAPBILL";
                }
                $.ajax({
                    type: "POST",
                    // url: "AddBills.aspx/GetBillRefExistEdit",
                    url: urlsedit,
                    async: false,
                    data: '{Ref: "' + $("#<%=txtRef.ClientID%>")[0].value + '",VendorID: "' + $("#<%=hdnVendorID.ClientID%>")[0].value + '",PJID: "' + qrStr + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var chk = response.d;
                        if (chk === "True") {
                            var r = confirm("Ref number with this vendor already exists!");
                            <%--$("#<%=txtRef.ClientID%>")[0].value('');--%>
                            
                            if (r === true) {
                                itemJSON();
                                enableControl(control);
                                retValEdit = true;
                              <%--  $("#<%=btnSubmit.ClientID%>").click();--%>

                            } else {
                                enableControl(control);
                                $('#MOMloading').hide();
                                retValEdit = false;
                            }

                        }
                        else {
                            itemJSON();
                            enableControl(control);
                            retValEdit = true;
                        }
                    },
                    failure: function (response) {
                        enableControl(control);
                        $('#MOMloading').hide();
                        retValEdit = false;

                    }

                });
               return retValEdit;

            }
            else {
                var retVal = false;
                var vendorId = $('#<%=hdnVendorID.ClientID%>').val();
                var urlsinsert;
                if (document.getElementById('<%=chkIsRecurr.ClientID%>').checked) {
                    urlsinsert = "CustomerAuto.asmx/GetBillRecurrRefExistAPBILL";
                }
                else {
                    urlsinsert = "CustomerAuto.asmx/GetBillRefExistAPBILL";
                }
                if (vendorId !== '') {

                    $.ajax({
                        type: "POST",
                        //url: "AddBills.aspx/GetBillRefExist",
                        url: urlsinsert,
                        async: false,
                        data: '{Ref: "' + $("#<%=txtRef.ClientID%>")[0].value + '",VendorID: "' + $("#<%=hdnVendorID.ClientID%>")[0].value + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            //$(control).css("pointer-events", "all");
                            var chk = response.d;
                            if (chk === "True") {
                                var r = confirm("Ref number with this vendor already exists!");
                                if (r === true) {
                                    itemJSON();
                                    enableControl(control);
                                    retVal = true;
                              <%--  $("#<%=btnSubmit.ClientID%>").click();--%>
                                } else {
                                    enableControl(control);
                                    $('#MOMloading').hide();
                                    retVal = false;
                                }
                            }
                            else {
                                itemJSON();
                                enableControl(control);
                                retVal = true;

                            }
                        },
                        failure: function (response) {
                            enableControl(control);
                            $('#MOMloading').hide();
                            retVal = false;
                        }

                    });
                    return retVal;
                }
                else {
                    enableControl(control);
                }
            }

        }
        function chkPORPO() {
            //alert();
        }
        function disableControl(control) {
            $(control).css("pointer-events", "none");
        }

        function enableControl(control) {
            setTimeout(function () { $(control).css("pointer-events", "all"); }, 1000);

        }
        function CloseModal() {
            setTimeout(function () {
                GetRadWindow().close();
            }, 0);
        }


        $(document).ready(function () {
            function dtaa() {
                this.prefixText = null;
                this.vendor = null;
                this.con = null;
            }
            function makeReadonly(txt) {
                $("#" + txt.id).prop('readonly', true);
            }
            if ("<%=Request.QueryString["id"] %>" != null && "<%=Request.QueryString["id"] %>" != "" && "<%=Request.QueryString["id"] %>" != undefined) {
                $('#aImport').hide();
            }

            $(function () {
                $("#<%=txtQty.ClientID%>").change(function () {
                    var budgetunit = $("#<%=txtBudgetUnit.ClientID%>").val();
                    var qty = $(this).val();
                    if (budgetunit != "" && qty != "") {
                        var budgetext = parseFloat(qty) * parseFloat(budgetunit);
                        $("#<%=lblBudgetExt.ClientID%>").text(budgetext.toFixed(2));
                    }
                    if (budgetunit != "") {
                        $("#<%=txtBudgetUnit.ClientID%>").val(parseFloat(budgetunit).toFixed(2));
                    }
                    if (qty != "") {
                        $("#<%=txtQty.ClientID%>").val(parseFloat(qty).toFixed(2));
                    }
                });
                $("#<%=txtBudgetUnit.ClientID%>").change(function () {
                    var budgetunit = $(this).val();
                    var qty = $("#<%=txtQty.ClientID%>").val();
                    if (budgetunit != "" && qty != "") {
                        var budgetext = parseFloat(qty) * parseFloat(budgetunit);
                        $("#<%=lblBudgetExt.ClientID%>").text(budgetext.toFixed(2));
                    }
                    if (budgetunit != "") {
                        $("#<%=txtBudgetUnit.ClientID%>").val(parseFloat(budgetunit).toFixed(2));
                    }
                    if (qty != "") {
                        $("#<%=txtQty.ClientID%>").val(parseFloat(qty).toFixed(2));
                    }
                });
                $("#<%=txtVendor.ClientID%>").keyup(function (event) {

                    var hdnVendorID = document.getElementById('<%=hdnVendorID.ClientID%>');
                    if (document.getElementById('<%=txtVendor.ClientID%>').value == '') {
                        hdnVendorID.value = '';
                    }
                });

                <%--$("#<%=txtPO.ClientID%>").autocomplete({

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
                        var dta = new dtaa();
                        dta.prefixText = request.term;
                        if ($("#<%=hdnVendorID.ClientID%>").val() != '') {
                            dta.vendor = $("#<%=hdnVendorID.ClientID%>").val();
                        }
                        query = request.term;

                        var str = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetPO",
                            data: JSON.stringify(dta),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                                
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load po");
                            }
                        });
                    },
                    select: function (event, ui) {
                        var str = ui.item.VendorName;
                        if (str == "No Record Found!") {
                            $(this).val("");
                        }
                        else {
                            $(this).val(ui.item.PO);
                            $("#<%= txtVendor.ClientID %>").val(ui.item.VendorName);
                            $("#<%= hdnVendorID.ClientID %>").val(ui.item.Vendor);
                            $("#<%= hdnTotalAmount.ClientID %>").val(ui.item.Amount);
                            $("#<%= hdnReceivedAmount.ClientID %>").val(ui.item.ReceivedAmount);
                            document.getElementById('<%=btnSelectPo.ClientID%>').click();
                        }
                        return false;
                    },
                    focus: function (event, ui) {

                        $(this).val(ui.item.PO);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).click(function () {
                    $(this).autocomplete('search', $(this).val())
                })
                $.each($(".posearchinput"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {

                        var ula = ul;
                        var itema = item;
                        var result_value = item.PO;
                        var result_item = item.PO;

                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            

                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);

                    };
                });--%>

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
                                    $(txtGvAcctNo).val(strAcct);
                                    $(hdnAcctID).val(ui[0].ID);
                                }
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load Acct#");
                            }
                        });
                    }

                });

                $("[id*=txtGvAcctNo]").autocomplete({

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
                                $(txtGvAcctNo).val(InvDefaultAcctName);
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

                ////   $$$$ Start JOB Autocomplete $$$$$$$

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
                        var GvPhase = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvPhase'));
                        var hdnTypeId = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnTypeId'));
                        //-----If Inventory code select then we set default inventory Acct
                        //----- Commented below code on 15-Feb-2022 as per Anita mam told if user first select code Inventory and after select Project so removed code and populate Acct# from Project exp GL.
                        <%--var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                        var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                        if (GvPhase == 'Inventory') {
                            $(txtGvAcctNo).val(InvDefaultAcctName);
                            $(hdnAcctID).val(InvDefaultAcctID);
                        }
                        else {
                            $(hdnAcctID).val(ui.item.GLExp);
                            var strAcct = ui.item.Acct + ' - ' + ui.item.DefaultAcct
                            $(txtGvAcctNo).val(strAcct);
                        }--%>
                        $(hdnAcctID).val(ui.item.GLExp);
                        var strAcct = ui.item.Acct + ' - ' + ui.item.DefaultAcct
                        $(txtGvAcctNo).val(strAcct);
                        $(GvPhase).val('');
                        $(hdnTypeId).val('0');
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
                        var txtGvLoc = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvLoc'));
                        var strJob = document.getElementById(txtGvJob).value;
                        
                        if (strJob == '') {
                            $(hdnJobID).val('');
                            $(txtGvLoc).val('');
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

                ////   $$$$ End JOB Autocomplete $$$$$$$ $ 
                //// ----- $$$ Start  txtGvPhase  autocomplete  $$$ 

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
                        var txtGvDesc = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvDesc'));
                        var str = ui.item.TypeName;
                        if (str == "No Record Found!") {
                            $(this).val("");
                        }
                        else {
                            try {
                                $(hdnTypeId).val(ui.item.Type);
                                $(this).val(ui.item.TypeName);
                                $(hdOpSq).val(ui.item.Code);
                                $(txtGvDesc).val(ui.item.Desc);
                            } catch{ }
                        }

                        var GvPhase = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvPhase')).value;
                        //-----If Inventory code select then we set default inventory Acct
                        var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                        var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                        if (GvPhase == 'Inventory') {
                            try {
                                var txtGvAcctNo = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvAcctNo'));
                                var hdnAcctID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnAcctID'));

                                txtGvWarehouse = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouse'));
                                txtGvWarehouseLocation = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouseLocation'));
                                txtGvWarehouse.readOnly = false;
                                txtGvWarehouseLocation.readOnly = false;

                                $(txtGvAcctNo).val(InvDefaultAcctName);
                                $(hdnAcctID).val(InvDefaultAcctID);
                            }
                            catch (e) { }
                        }
                        else {
                            try {
                                txtGvWarehouse = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouse'));
                                txtGvWarehouseLocation = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouseLocation'));
                                txtGvWarehouse.readOnly = true;
                                txtGvWarehouseLocation.readOnly = true;
                                $(txtGvWarehouse).val('');
                                $(txtGvWarehouseLocation).val('');
                                txtGvAcctNo = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvAcctNo'));
                                hdnAcctID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnAcctID'));
                                if (ui.item.AcctName != '' && ui.item.AcctID != '' && ui.item.AcctName != undefined && ui.item.AcctID != undefined) {
                                    $(txtGvAcctNo).val(ui.item.AcctName);
                                    $(hdnAcctID).val(ui.item.AcctID);
                                }

                            }
                            catch (e) { }
                        }


                        return false;
                    },
                    focus: function (event, ui) {

                        $(this).val(ui.item.TypeName);
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
                        var result_Desc = item.Desc;
                        if (result_Code != null && result_Code != "")
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<span class='auto_item'><i style='display:inline-block; margin-right:8px;width:auto;color:#1565C0 !important;' class='fa fa-check-square' title=''></i><span style='color:Gray;'>" + result_GroupName + ",&nbsp; &nbsp;" + result_Code + ", &nbsp; &nbsp;" + result_CodeDesc + ",&nbsp; &nbsp;" + result_item + ", </span>&nbsp; &nbsp;<span style='color:Black;'><b>  " + result_Desc + " </b></span></span>")
                                .appendTo(ul);                                 
                        else
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<span class='auto_item'><i style='display:inline-block; margin-right:8px;width:auto;color:#1565C0 !important;' title=''></i>" + result_item + "</span>")
                                .appendTo(ul);
                    };
                });

                $("[id*=txtGvPhase]").change(function () {
                    var txtGvPhase = $(this);
                    var strPhase = $(this).val();

                    var txtGvPhase1 = $(txtGvPhase).attr('id');
                    var hdnTypeId = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnTypeId'));
                    var hdnPID = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnPID'));
                    var txtGvItem = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'txtGvItem'));
                    var hdnItemID = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnItemID'));
                    var txtGvDesc = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'txtGvDesc'));

                    if (strPhase != "") {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetAutoFillPhase",
                            data: '{"prefixText": "' + strPhase + '"}',
                            dataType: "json",
                            async: true,
                            success: function (data) {

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
                                else {
                                    var lbl = ui[0].Label;
                                    var val = ui[0].Value;
                                    $(txtGvPhase).val(lbl);
                                    $(hdnTypeId).val(val);
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

                //// ----- $$$ END  txtGvPhase  autocomplete  $$$ 

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
                        var id = $(this.element).prop("id");
                        //var id2 = this.element[0].id;
                        //var id3 = $(this.element.get(0)).attr('id');
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetJobLocations",
                            data: '{"prefixText": "' + dtaaa.prefixText + '", "IsJob": "' + false + '", "con": "' + dtaaa.con + '"}',
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                                var ui = $.parseJSON(data.d);

                                if (ui.length == 0) {
                                    document.getElementById(id).value = '';
                                }
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
                        var jobStr = ui.item.ID + ", " + ui.item.fDesc;
                        $(txtGvJob).val(jobStr);
                        //$(txtGvJob).val(ui.item.fDesc);
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
                    $(this).autocomplete('search', $(this).val())
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
                                .append("<a><b> Job: </b> " + result_item + ", <span style='color:Gray;'><b> Loc: </b>" + result_desc + "</span></a>")
                                .appendTo(ul);
                        }
                    };
                });

                //$("[id*=txtGvItem]").change(function () {
                //    debugger;
                //    var rowItem = this.id;
                //    var rowItemId = document.getElementById(rowItem.replace('txtGvItem', 'hdnItemID'));
                //    var rowDesc = document.getElementById(rowItem.replace('txtGvItem', 'txtGvDesc'));
                //    var rowPhase = document.getElementById(rowItem.replace('txtGvItem', 'txtGvPhase'));
                //    var rowPid = document.getElementById(rowItem.replace('txtGvItem', 'hdnPID'));
                //    var rowtid = document.getElementById(rowItem.replace('txtGvItem', 'hdnTypeId'));

                //    if ($(rowPid).val() == "0") {
                //        document.getElementById(rowItem).value = '';
                //        $(rowItemId).val('');
                //        $(rowDesc).val('');
                //        $(rowPhase).val('');
                //        $(rowPid).val('');
                //        $(rowtid).val('');
                //    }
                //});
            });

            Materialize.updateTextFields();
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ui-active-menuitem').click(function () {

                $('.ui-menu-item').removeClass('ui-autocomplete-loading');
            })
            InitializeGrids('<%=RadGrid_gvJobCostItems.ClientID%>');

            CallCalAmountTax();
        });

        ////////// This function will be called during delete operation to call two functions ///////////
        function CallCalAmountTax() {
            CalculateTotalAmt();
            CalculateTotalUseTaxExpense();
        }
        /////////////////// To calculate Total and to make Gridview Amount Value to 2 decimal ////////////

        function CalculateTotal(obj) {
            //try {
            //debugger
            var masterTable = $find("<%=RadGrid_gvJobCostItems.ClientID%>").get_masterTableView();
            var count = masterTable.get_dataItems().length;
            var item;
            for (var i = 0; i < count; i++) {
                item = masterTable.get_dataItems()[i];
                var Qty = item.findElement("txtGvQuan");
                var Amount = item.findElement("txtGvAmount");
                var Prices = item.findElement("txtGvPrice");

                var lblAmountWithTax = item.findElement("lblAmountWithTax");
                var hdnAmountWithTax = item.findElement("hdnAmountWithTax");




                //////////// PO QTY /AMOUNT CALCULATION //////////////////
                var hdnOutstandQuan = item.findElement("hdnOutstandQuan");
                var hdnOutstandBalance = item.findElement("hdnOutstandBalance");
                var hdnIsPO = item.findElement("hdnIsPO");

                var chk = item.findElement("chkTaxable");
                var chkval = item.findElement("hdnchkTaxable");
                var txtGvStaxAmount = item.findElement("txtGvStaxAmount");
                var hdnSTaxAm = item.findElement("hdnSTaxAm");

                var chkGTaxable = item.findElement("chkGTaxable");
                var hdnchkGTaxable = item.findElement("hdnchkGTaxable");
                var lblGstTax = item.findElement("lblGstTax");
                var hdnGSTTaxAm = item.findElement("hdnGSTTaxAm");

                
                if (jQuery.trim($(hdnIsPO).val()) == '2') {

                    var pobalqty = parseFloat($(hdnOutstandQuan).val());
                    var pobalamt = parseFloat($(hdnOutstandBalance).val());
                    var temTotal = 0;
                    var receiveQuan = parseFloat($(Qty).val());
                    var receiveAmnt = parseFloat($(Amount).val());
                    var Price = 0;//parseFloat($(Prices).val());
                    if (!isNaN(parseFloat($(Prices).val()))) {
                        Price = parseFloat($(Prices).val());
                    }

                    if (parseFloat(pobalamt) == 0) { pobalamt = receiveAmnt; }

                    if ($(Amount).val() == '') {
                        $(Amount).val('0.00');
                        $(hdnAmountWithTax).val('0.00');
                        $(lblAmountWithTax).text('0.00');
                        if (chk != null) {
                            document.getElementById(chk.id).checked = false;
                            $(chkval).val('0');
                            $(txtGvStaxAmount).val('0');
                            $(hdnSTaxAm).val('0');
                        }
                        if (chkGTaxable != null) {
                            document.getElementById(chkGTaxable.id).checked = false;
                            $(hdnchkGTaxable).val('0');
                            $(lblGstTax).val('0');
                            $(hdnGSTTaxAm).val('0');
                        }
                    }
                    else {
                        if (receiveAmnt > pobalamt) {
                            $(Amount).val(pobalamt);
                            $(hdnAmountWithTax).val(pobalamt);
                            $(lblAmountWithTax).text(pobalamt.toFixed(2));
                            if (chk != null) {
                                document.getElementById(chk.id).checked = false;
                                $(chkval).val('0');
                                $(txtGvStaxAmount).val('0.00');
                                $(hdnSTaxAm).val('0.00');
                            }
                            if (chkGTaxable != null) {
                                document.getElementById(chkGTaxable.id).checked = false;
                                $(hdnchkGTaxable).val('0');
                                $(lblGstTax).val('0.00');
                                $(hdnGSTTaxAm).val('0.00');
                            }
                        }
                        else {
                            $(Amount).val(receiveAmnt);
                            $(hdnAmountWithTax).val(receiveAmnt);
                            $(lblAmountWithTax).text(receiveAmnt.toFixed(2));
                            if (chk != null) {
                                document.getElementById(chk.id).checked = false;
                                $(chkval).val('0');
                                $(txtGvStaxAmount).val('0.00');
                                $(hdnSTaxAm).val('0.00');
                            }
                            if (chkGTaxable != null) {
                                document.getElementById(chkGTaxable.id).checked = false;
                                $(hdnchkGTaxable).val('0');
                                $(lblGstTax).val('0.00');
                                $(hdnGSTTaxAm).val('0.00');
                            }

                        }
                    }
                    //var receivef = parseFloat($(Amount).val());
                    //var qtyf = 0;
                    //if (Price != 0) {
                    //    qtyf = receivef / Price;

                    //}

                    //$(Qty).val(parseFloat(qtyf));

                }
                //////////// PO QTY /AMOUNT CALCULATION //////////////////
                //if (jQuery.trim($(hdnIsPO).val()) == '2' || jQuery.trim($(hdnIsPO).val()) == '0') {

                //ES-8084 Urgent!! Albany - PO Issue - create Bill directly without RPO and cut a check for 10% of the PO amount the system it is closing the PO, however, it should be under Partial-Quantity status. ------ So in this case Qty will be changes according to amount and PO status will be set  --- So remove HdnIsPO.val()==2 condtion
                // And ES-7111 AP-Disable quantity update in case bill is created refering to RPO and user changes amount CS#13836 ----- In this case if creating bill from RPO and user chnage Amount then Price will be changed 
                if (jQuery.trim($(hdnIsPO).val()) == '0') {
                    var receivef = parseFloat($(Amount).val());
                    var qtyf = 0;
                    if (Price != 0) {
                        //var Pricess = parseFloat($(Prices).val());
                        //qtyf = receivef / Pricess;
                        //$(Qty).val(parseFloat(qtyf));                    
                        var QtyKhan = parseFloat($(Qty).val());
                        var PriceKhan = parseFloat($(Prices).val());
                        PriceKhan = receivef / QtyKhan;
                        $(Prices).val(parseFloat(PriceKhan));
                    }
                }
                else {
                    var receivef = parseFloat($(Amount).val());
                    var qtyf = 0;
                    if (Price != 0) {
                        var Pricess = parseFloat($(Prices).val());
                        qtyf = receivef / Pricess;
                        $(Qty).val(parseFloat(qtyf));                                            
                    }
                }
                //$(Qty).val(parseFloat(qtyf));
                //debugger
                var QtyVal = $(Qty).val();
                var AmountVal = $(Amount).val();
                if (QtyVal != "" && AmountVal != "") {
                    if (!isNaN(parseFloat(QtyVal)) && !isNaN(parseFloat(AmountVal)) && parseFloat(QtyVal) != 0 && Price != 0) {
                        var QtyPrice = parseFloat(AmountVal) / parseFloat(QtyVal);
                        $(Prices).val(QtyPrice.toFixed(2));
                    } else {
                        $(Prices).val("");
                    }

                }

            }
            //} catch (e) {
            //    alert();
            //    alert(e);
            //}
            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
                var hdnIsPO;
                var pobalamt;
                var receiveAmnt;
                var hdnOutstandBalance;
                var Amount;
                var txt = obj.id;

                var chk;
                var chkval;
                var txtGvStaxAmount;
                var hdnSTaxAm;
                var chkGTaxable;
                var hdnchkGTaxable;
                var lblGstTax;
                var hdnGSTTaxAm;

                if (txt.indexOf("Quan") >= 0) {
                    hdnIsPO = document.getElementById(txt.replace('txtGvQuan', 'hdnIsPO'));
                    hdnOutstandBalance = document.getElementById(txt.replace('txtGvQuan', 'hdnOutstandBalance'));
                    Amount = document.getElementById(txt.replace('txtGvQuan', 'txtGvAmount'));

                    chk = document.getElementById(txt.replace('txtGvQuan', 'chkTaxable'));
                    chkval = document.getElementById(txt.replace('txtGvQuan', 'hdnchkTaxable'));
                    txtGvStaxAmount = document.getElementById(txt.replace('txtGvQuan', 'txtGvStaxAmount'));
                    hdnSTaxAm = document.getElementById(txt.replace('txtGvQuan', 'hdnSTaxAm'));
                    chkGTaxable = document.getElementById(txt.replace('txtGvQuan', 'chkGTaxable'));
                    hdnchkGTaxable = document.getElementById(txt.replace('txtGvQuan', 'hdnchkGTaxable'));
                    lblGstTax = document.getElementById(txt.replace('txtGvQuan', 'lblGstTax'));
                    hdnGSTTaxAm = document.getElementById(txt.replace('txtGvQuan', 'hdnGSTTaxAm'));

                }
                else if (txt.indexOf("Price") >= 0) {
                    hdnIsPO = document.getElementById(txt.replace('txtGvPrice', 'hdnIsPO'));
                    hdnOutstandBalance = document.getElementById(txt.replace('txtGvPrice', 'hdnOutstandBalance'));
                    Amount = document.getElementById(txt.replace('txtGvPrice', 'txtGvAmount'));

                    chk = document.getElementById(txt.replace('txtGvPrice', 'chkTaxable'));
                    chkval = document.getElementById(txt.replace('txtGvPrice', 'hdnchkTaxable'));
                    txtGvStaxAmount = document.getElementById(txt.replace('txtGvPrice', 'txtGvStaxAmount'));
                    hdnSTaxAm = document.getElementById(txt.replace('txtGvPrice', 'hdnSTaxAm'));
                    chkGTaxable = document.getElementById(txt.replace('txtGvPrice', 'chkGTaxable'));
                    hdnchkGTaxable = document.getElementById(txt.replace('txtGvPrice', 'hdnchkGTaxable'));
                    lblGstTax = document.getElementById(txt.replace('txtGvPrice', 'lblGstTax'));
                    hdnGSTTaxAm = document.getElementById(txt.replace('txtGvPrice', 'hdnGSTTaxAm'));

                }
                else if (txt.indexOf("Amount") >= 0) {
                    hdnIsPO = document.getElementById(txt.replace('txtGvAmount', 'hdnIsPO'));
                    hdnOutstandBalance = document.getElementById(txt.replace('txtGvAmount', 'hdnOutstandBalance'));
                    Amount = document.getElementById(txt.replace('txtGvAmount', 'txtGvAmount'));

                    chk = document.getElementById(txt.replace('txtGvAmount', 'chkTaxable'));
                    chkval = document.getElementById(txt.replace('txtGvAmount', 'hdnchkTaxable'));
                    txtGvStaxAmount = document.getElementById(txt.replace('txtGvAmount', 'txtGvStaxAmount'));
                    hdnSTaxAm = document.getElementById(txt.replace('txtGvAmount', 'hdnSTaxAm'));
                    chkGTaxable = document.getElementById(txt.replace('txtGvAmount', 'chkGTaxable'));
                    hdnchkGTaxable = document.getElementById(txt.replace('txtGvAmount', 'hdnchkGTaxable'));
                    lblGstTax = document.getElementById(txt.replace('txtGvAmount', 'lblGstTax'));
                    hdnGSTTaxAm = document.getElementById(txt.replace('txtGvAmount', 'hdnGSTTaxAm'));

                }

                if (jQuery.trim($(hdnIsPO).val()) != '2') {
                    CalTotalVal(obj);
                }

                if (jQuery.trim($(hdnIsPO).val()) == '2') {
                    var pobalamt = parseFloat($(hdnOutstandBalance).val());
                    var receiveAmnt = parseFloat($(Amount).val());


                    if (parseFloat(pobalamt) == 0) { pobalamt = receiveAmnt; }

                    if ($(Amount).val() == '') {
                        $(Amount).val('0.00');

                        //$(hdnAmountWithTax).val('0.00');
                        //$(lblAmountWithTax).text('0.00');


                    }
                    else {
                        if (receiveAmnt > pobalamt) {
                            $(Amount).val(pobalamt);
                            //$(hdnAmountWithTax).val(pobalamt);
                            //$(lblAmountWithTax).text(pobalamt.toFixed(2));

                        }
                        else {
                            $(Amount).val(receiveAmnt);


                        }
                    }


                }
            }
            else {
                var hdnIsPO;
                var pobalamt;
                var receiveAmnt;
                var hdnOutstandBalance;
                var Amount;
                var txt = obj.id;

                if (txt.indexOf("Quan") >= 0) {
                    hdnIsPO = document.getElementById(txt.replace('txtGvQuan', 'hdnIsPO'));
                    hdnOutstandBalance = document.getElementById(txt.replace('txtGvQuan', 'hdnOutstandBalance'));
                    Amount = document.getElementById(txt.replace('txtGvQuan', 'txtGvAmount'));
                }
                else if (txt.indexOf("Price") >= 0) {
                    hdnIsPO = document.getElementById(txt.replace('txtGvPrice', 'hdnIsPO'));
                    hdnOutstandBalance = document.getElementById(txt.replace('txtGvPrice', 'hdnOutstandBalance'));
                    Amount = document.getElementById(txt.replace('txtGvPrice', 'txtGvAmount'));
                }
                else if (txt.indexOf("Amount") >= 0) {
                    hdnIsPO = document.getElementById(txt.replace('txtGvAmount', 'hdnIsPO'));
                    hdnOutstandBalance = document.getElementById(txt.replace('txtGvAmount', 'hdnOutstandBalance'));
                    Amount = document.getElementById(txt.replace('txtGvAmount', 'txtGvAmount'));
                }

                if (jQuery.trim($(hdnIsPO).val()) != '2') {
                    CalTotalVal(obj);
                }

                if (jQuery.trim($(hdnIsPO).val()) == '2') {
                    var pobalamt = parseFloat($(hdnOutstandBalance).val());
                    var receiveAmnt = parseFloat($(Amount).val());


                    if (parseFloat(pobalamt) == 0) { pobalamt = receiveAmnt; }

                    if ($(Amount).val() == '') {
                        $(Amount).val('0.00');

                        // $(hdnAmountWithTax).val('0.00');
                        // $(lblAmountWithTax).text('0.00');
                    }
                    else {
                        if (receiveAmnt > pobalamt) {
                            $(Amount).val(pobalamt);
                            //  $(hdnAmountWithTax).val(pobalamt);
                            //  $(lblAmountWithTax).text(pobalamt.toFixed(2));
                        }
                        else {
                            $(Amount).val(receiveAmnt);
                            //  $(hdnAmountWithTax).val(receiveAmnt);
                            //  $(lblAmountWithTax).text(receiveAmnt.toFixed(2));

                        }
                    }


                }
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
        function CalculateSalesTaxTotal(obj) {

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(4);
            }
            CalculateTotalUseTaxExpense();
        }

        //////////////// Confirm Mail Send to worker ///////////////////
        function notyConfirm(ticid) {

            noty({
                dismissQueue: true,
                layout: 'topCenter',
                theme: 'noty_theme_default',
                animateOpen: { height: 'toggle' },
                animateClose: { height: 'toggle' },
                easing: 'swing',
                text: 'Do you want to update tax at vendor level?',
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

                            //window.open("mailticket.aspx?id=" + ticid + "&c=0", "_blank"); 
                            var hdnVendorID = document.getElementById('<%=hdnVendorID.ClientID%>');
                            $noty.close();
                            if (hdnVendorID.value != '') {

                                $("#<%=hdnUpdateStax.ClientID%>").val('1');
                                document.getElementById('<%=btnUpdtStax.ClientID%>').click();
                            }
                            else {
                                noty({
                                    text: 'Please select vendor.',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: 1500,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                                $("#<%=ddlSTax.ClientID%>").val('');

                            }
                        }
                    },
                    {
                        type: 'btn-danger', text: 'No', click: function ($noty) {

                            var hdnVendorID = document.getElementById('<%=hdnVendorID.ClientID%>');
                            $noty.close();
                            if (hdnVendorID.value != '') {

                                $("#<%=hdnUpdateStax.ClientID%>").val('0');
                                document.getElementById('<%=btnUpdtStax.ClientID%>').click();
                            }
                            else {
                                noty({
                                    text: 'Please select vendor.',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: 1500,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                                $("#<%=ddlSTax.ClientID%>").val('');
                            }
                            //window.open("addticket.aspx?id=" + ticid + "&comp=0&pop=1&fr=tlv", "_self");
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


        function GetSelectedSTax(ddlSTax) {

            var selectedText = ddlSTax.options[ddlSTax.selectedIndex].innerHTML;
            var selectedValue = ddlSTax.value;
            var lgl = "";
            var lrate = "";
            var ltype = "";
            var lname = "";
            //var lgl = "";
            function dtaa() {
                this.prefixText = null;
                this.vendor = null;
                this.con = null;
            }

            var dtaaa = new dtaa();

            var tytty = selectedValue;
            dtaaa.prefixText = tytty;
            query = tytty;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "AccountAutoFill.asmx/getSaleTaxSearchByName",
                data: JSON.stringify(dtaaa),
                dataType: "json",
                async: true,
                success: function (data) {

                    var maindata = JSON.parse(data.d);

                    //$("#<%=txtqst.ClientID%>").text(ui.item.STax);
                    $("#<%=hdnQST.ClientID%>").val(maindata[0].Rate);
                    $("#<%=hdnQSTGL.ClientID%>").val(maindata[0].GL);
                    $("#<%=hdnSTaxType.ClientID%>").val(maindata[0].Type);
                    $("#<%=hdnSTaxName.ClientID%>").val(maindata[0].Name);
                            <%--$("#<%=hdnSTaxState.ClientID%>").val(maindata[0].State);--%>




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





                    $("#<%=RadGrid_gvJobCostItems.ClientID %>").find('tr:not(:first, :last)').each(function () {
                        var $tr = $(this);
                        if ($tr.find('input[id*=lblSalesTax]').attr('id') != "" && typeof $tr.find('input[id*=lblSalesTax]').attr('id') != 'undefined') {
                            $tr.find('input[id*=lblSalesTax]').val(maindata[0].Rate);
                            $tr.find('input[id*=hdnSTaxAm]').val(parseFloat(0).toFixed(4));
                            $tr.find('input[id*=txtGvStaxAmount]').val(parseFloat(0).toFixed(4));
                            $tr.find('input[id*=hdnSTaxGL]').val(maindata[0].GL);
                            $tr.find('input[id*=hdnSTaName]').val(maindata[0].Name);

                            if ($tr.find('input[id*=hdnchkTaxable]') != null) {
                                var cb = $tr.find('input[id*=hdnchkTaxable]');
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
                                    //checkbox.checked = false;

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

                                    txtGvPrice = $tr.find('input[id*=txtGvPrice]');
                                    txtGvQuan = $tr.find('input[id*=txtGvQuan]');
                                    txtGvAmount = $tr.find('input[id*=txtGvAmount]');
                                    lblGstTax = $tr.find('input[id*=lblGstTax]');

                                    lblAmountWithTax = $tr.find('span[id$="lblAmountWithTax"]');
                                    hdnAmountWithTax = $tr.find('input[id*=hdnAmountWithTax]');
                                    hdnchkTaxable = $tr.find('input[id*=hdnchkTaxable]');
                                    hdnSTaxGL = $tr.find('input[id*=hdnSTaxGL]');
                                    hdnGSTTaxGL = $tr.find('input[id*=hdnGSTTaxGL]');
                                    hdnSTaxAm = $tr.find('input[id*=hdnSTaxAm]');
                                    txtGvStaxAmount = $tr.find('input[id*=txtGvStaxAmount]');
                                    hdnGSTTaxAm = $tr.find('input[id*=hdnGSTTaxAm]');

                                    gtaxAmt = parseFloat($(hdnGSTTaxAm).val());
                                    gtaxAmt = parseFloat(gtaxAmt) || 0;

                                    if (cb.val() == "1") {
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

                                    //if (isGst == 1) {
                                    //    if (gtax == null) {
                                    //        gtaxAmt = 0.00;
                                    //        gtaxAmtGL = 0;
                                    //        $(lblGstTax).val(gtaxAmt.toFixed(2));

                                    //        $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                                    //        $(hdnGSTTaxGL).val(gtaxAmtGL);
                                    //    }
                                    //    else if (gtax.value != '') {
                                    //        if (cb.val() == "1") {
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

                                    if (cb.val() == "1") {
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
                                            if (isGst == 1) {
                                                valAmount = parseFloat(valAmount) + gtaxAmt;
                                            }
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

                        }
                    });

                },
                error: function (result) {
                    alert("Due to unexpected errors we were unable to load account name");
                }
            });

            notyConfirm(selectedValue);

        }
        function disablekeys() {
            return false;
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
        function CalculateTotalAmtSST() {
            var tAmountstax = 0.00;

            //$("[id*=hdnSTaxAm]").each(function () {

            //    if (!jQuery.trim($(this).val()) == '') {
            //        if (!isNaN(parseFloat($(this).val()))) {

            //            //var totalAmount = jQuery(this).parent().parent().find('.clsAmount').val();
            //            //if (totalAmount != null && totalAmount != "") {
            //                tAmountstax = tAmountstax + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
            //            //}
            //        } else
            //            $(this).val('');
            //    }
            //    else {
            //        $(this).val('');
            //    }
            //});
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

            if (isNaN(tAmountstax)) {

                tAmountstax = 0.00;
            }
            if (isNaN(tAmountgsttax)) {

                tAmountgsttax = 0.00;
            }

            tAmountamttax = parseFloat(tAmountstax.toFixed(2)) + parseFloat(tAmountgsttax.toFixed(2));
            var totactualamt = 0.00;
            totactualamt = parseFloat($('[id*=lblAmountPerTotalGrid]').text());
            tAmountamttax = tAmountamttax + totactualamt;
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
        }
        function CalculateTotalAmt() {
            var tAmount = 0.00;
            $("[id*=txtGvAmount]").each(function () {

                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {

                        var totalAmount = jQuery(this).parent().parent().find('.clsAmount').val();
                        if (totalAmount != null && totalAmount != "") {
                            tAmount = tAmount + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                        }
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });

            $('#<%=lblTotalAmount.ClientID%>').text(tAmount.toFixed(2));
            $('[id*=lblTotalAmt]').text(tAmount.toFixed(2));
            $('[id*=lblAmountPerTotalGrid]').text(tAmount.toFixed(2));

            <%--$('#<%=lblTotalAmount.ClientID%>').text(cleanUpCurrency(parseFloat(tAmount).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('[id*=lblTotalAmt]').text(cleanUpCurrency( parseFloat(tAmount).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('[id*=lblAmountPerTotalGrid]').text(cleanUpCurrency(parseFloat(tAmount).toLocaleString("en-US", { minimumFractionDigits: 2 })));--%>




            var totalQty = 0.00;
            $("[id*=txtGvQuan]").each(function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totalQty = totalQty + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            $('[id*=lblTotalQty]').text(totalQty.toFixed(2));
            //$('[id*=lblTotalQty]').text(cleanUpCurrency(parseFloat(totalQty).toLocaleString("en-US", { minimumFractionDigits: 0 })));


            //lblSalesTax = document.getElementById(cb.replace('chkTaxable', 'lblSalesTax'));
            //lblGstTax = document.getElementById(cb.replace('chkTaxable', 'lblGstTax'));
            //lblAmount = document.getElementById(cb.replace('chkTaxable', 'lblAmount'));

            var tAmountstax = 0.00;

            //$("[id*=hdnSTaxAm]").each(function () {

            //    if (!jQuery.trim($(this).val()) == '') {
            //        if (!isNaN(parseFloat($(this).val()))) {

            //            //var totalAmount = jQuery(this).parent().parent().find('.clsAmount').val();
            //            //if (totalAmount != null && totalAmount != "") {
            //                tAmountstax = tAmountstax + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
            //            //}
            //        } else
            //            $(this).val('');
            //    }
            //    else {
            //        $(this).val('');
            //    }
            //});
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
            totactualamt = parseFloat($('[id*=lblAmountPerTotalGrid]').text());
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

            tAmountamttax = totactualamt + totsaletaxamt + totGSTtaxamt;
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
            $('#<%=lblTotalAmount.ClientID%>').text(tAmountamttax.toFixed(2));
            CalculateTotalUseTaxExpense();
        }
        //////////////////////To make row's textbox visible///////////////////////////////////////////
        function VisibleRow(row, txt, gridview, event) {
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

            var lblSalesTax = document.getElementById(txt.replace('txtGvAcctNo', 'lblSalesTax'));
            $(lblSalesTax).removeClass("texttransparent");
            $(lblSalesTax).addClass("non-trans");

        }
        /////////////////////////////////////////////////////////////////////////////
        ////////////// To check is entered charcter is number or not//////////////
        function isNum(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        //////////////// To make textbox value decimal ///////////////////////////
        function isDecimalKey(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;

            if (charCode == 45) {
                return true;
            }

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
        function enableamount(txt) {

            txt.removeAttribute('readonly');

        }
        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }

        //////////////////Calculate Due date based on Due In field///////////////////
        function changeDueDate1() {
            if (document.getElementById('<%= txtDueIn.ClientID %>').value == '') {
                document.getElementById('<%= txtDueIn.ClientID %>').value = "0"
            }

            var dueIn = parseInt(document.getElementById('<%= txtDueIn.ClientID %>').value);
            //var today = new Date();

            var today = new Date();
            if ($('#<%= txtDate.ClientID %>').val() != '') {
                today = new Date($('#<%= txtDate.ClientID %>').val());
            }
            var dueDate = new Date(today);
            dueDate.setDate(today.getDate() + dueIn);
            var d = dueDate.getDate();
            if (d < 10) d = '0' + d;
            var m = dueDate.getMonth() + 1;
            if (m < 10) m = '0' + m;
            var y = dueDate.getFullYear();

            document.getElementById('<%= txtDueDate.ClientID %>').value = m + '/' + d + '/' + y;
        }

        function changeDueDate() {
            var PostDate = new Date;
            var DueDate = new Date;
            if ($('#<%= txtDate.ClientID %>').val() != '') {
                PostDate = new Date($('#<%= txtDate.ClientID %>').val());
            }
            if ($('#<%= txtDueDate.ClientID %>').val() != '') {
                DueDate = new Date($('#<%= txtDueDate.ClientID %>').val());
            }
            var diffDays = parseInt((DueDate - PostDate) / (1000 * 60 * 60 * 24));


            document.getElementById('<%= txtDueIn.ClientID %>').value = diffDays;




        }
        function ChkVendor(sender, args) {
            var hdnVendorID = document.getElementById('<%=hdnVendorID.ClientID%>');
            if (hdnVendorID.value == '') {
                args.IsValid = false;
            }
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
        $("#<%=txtVendor.ClientID%>").keyup(function (event) {
            var hdnVendorID = document.getElementById('<%=hdnVendorID.ClientID%>');
            if (document.getElementById('<%=txtVendor.ClientID%>').value == '') {
                hdnVendorID.value = '';
            }
        });
        function addedItem(item, itemId, phaseId, typeId, type, fdesc) {
            noty({
                text: 'BOM Item added successfully!',
                type: 'success',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });

            var rowItem = $("#<%=hdnRowField.ClientID%>").val();
            var rowItemId = document.getElementById(rowItem.replace('txtGvItem', 'hdnItemID'));
            var rowDesc = document.getElementById(rowItem.replace('txtGvItem', 'txtGvDesc'));
            var rowPhase = document.getElementById(rowItem.replace('txtGvItem', 'txtGvPhase'));
            var rowPid = document.getElementById(rowItem.replace('txtGvItem', 'hdnPID'));
            var rowtid = document.getElementById(rowItem.replace('txtGvItem', 'hdnTypeId'));

            document.getElementById(rowItem).value = item;
            $(rowItemId).val(itemId);
            $(rowDesc).val(fdesc);
            $(rowPhase).val(type);
            $(rowPid).val(phaseId);
            $(rowtid).val(typeId);

            //window.parent.document.getElementById('btnCancel').click();
        }

        function aceItem_itemSelected(sender, e) {
            var hdnItemID = document.getElementById('<%= hdnItemID.ClientID %>');
            if (e.get_value() != "0") {
                hdnItemID.value = e.get_value();
                document.getElementById('<%= txtItem.ClientID %>').value = "";
            }
        }
        function SetContextKey() {
            var value = $get("<%=ddlBomType.ClientID %>").value;
            $find('<%=AutoCompleteExtender3.ClientID%>').set_contextKey($get("<%=ddlBomType.ClientID %>").value);
        }
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
            var rowItem = $("#<%=hdnRowField.ClientID%>").val();
            var rowItemId = document.getElementById(rowItem.replace('txtGvItem', 'hdnItemID'));
            var rowPhase = document.getElementById(rowItem.replace('txtGvItem', 'txtGvPhase'));
            var rowPid = document.getElementById(rowItem.replace('txtGvItem', 'hdnPID'));
            var rowtid = document.getElementById(rowItem.replace('txtGvItem', 'hdnTypeId'));

            document.getElementById(rowItem).value = '';
            $(rowItemId).val('');
            $(rowPhase).val('');
            $(rowPid).val('');
            $(rowtid).val('');

            ResetBom();
        }
        function ResetBom() {
            $("#<%=txtBudgetUnit.ClientID%>").val('0.00');
            $("#<%=lblBudgetExt.ClientID%>").val('0.00');
            $("#<%=txtQty.ClientID%>").val('0.00');
            $("#<%=txtOpSeq.ClientID%>").val('');
            $("#<%=txtItem.ClientID%>").val('');
            $("#<%=hdnItemID.ClientID%>").val('');
            $("#<%=txtJobDesc.ClientID%>").val('');
            $("#<%=txtUM.ClientID%>").val('');
            $("#<%=ddlBomType.ClientID%>").val('0');
        }
    </script>
    <script type="text/javascript">

        $(document).ready(function () {

            ///////////// Ajax call for vendor auto search ////////////////////                
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.Acct = null;
            }
            $("[id*=txtGvUseTax]").keypress(function (e) {
                var keyCode = e.which;
                /*
                  8 - (backspace)
                  32 - (space)
                  48-57 - (0-9)Numbers
                */

                //if ( (keyCode != 8 || keyCode ==32 ) && (keyCode < 48 || keyCode > 57)) { 
                return false;
                //}
            });
            $("[id*=txtTotalUseTax]").keypress(function (e) {
                var keyCode = e.which;
                /*
                  8 - (backspace)
                  32 - (space)
                  48-57 - (0-9)Numbers
                */

                //if ( (keyCode != 8 || keyCode ==32 ) && (keyCode < 48 || keyCode > 57)) { 
                return false;
                //}
            });
            $("[id*=lblSalesTax]").keypress(function (e) {
                var keyCode = e.which;
                /*
                  8 - (backspace)
                  32 - (space)
                  48-57 - (0-9)Numbers
                */

                //if ( (keyCode != 8 || keyCode ==32 ) && (keyCode < 48 || keyCode > 57)) { 
                return false;
                //}
            });
            $("#<%=txtVendor.ClientID%>").autocomplete({

                open: function (e, ui) {
                    /* create the scrollbar each time autocomplete menu opens/updates */
                    $(".ui-autocomplete").mCustomScrollbar({
                        setHeight: 182,
                        theme: "dark-3",
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

                        $("#<%=txtPaid.ClientID%>").val(ui.item.Days);
                        $("#<%=txtDueIn.ClientID%>").val(ui.item.Term);
                        $("#<%=txtVendorType.ClientID%>").val(ui.item.VendorType);

                        if (document.getElementById('txtqstgv').style.display == 'block') {

                            $("#<%=txtqst.ClientID%>").text(ui.item.STax);
                            $("#<%=hdnQST.ClientID%>").val(ui.item.STaxRate);
                            $("#<%=hdnQSTGL.ClientID%>").val(ui.item.STaxGL);
                            $("#<%=hdnSTaxType.ClientID%>").val(ui.item.STaxType);
                            $("#<%=hdnSTaxName.ClientID%>").val(ui.item.STaxName);
                            $("#<%=hdnSTaxState.ClientID%>").val(ui.item.VState);


                            var dtaaa = new dtaa();
                            dtaaa.prefixText = $("#<%=hdnSTaxState.ClientID%>").val();
                            //query = request.term;
                            $("[id*=lblSalesTax]").autocomplete(dtaaa);

                            document.getElementById('<%=btnStaxType.ClientID%>').click();


                            <%--$("#<%=RadGrid_gvJobCostItems.ClientID %>").find('tr:not(:first, :last)').each(function () {
                                var $tr = $(this);
                                if ($tr.find('input[id*=lblSalesTax]').attr('id') != "" && typeof $tr.find('input[id*=lblSalesTax]').attr('id') != 'undefined') {
                                    $tr.find('input[id*=lblSalesTax]').val($("#<%=hdnQST.ClientID%>").val());
                                    $tr.find('input[id*=hdnSTaxAm]').val(parseFloat(0).toFixed(4));
                                    $tr.find('input[id*=hdnSTaxGL]').val($("#<%=hdnQSTGL.ClientID%>").val());
                                    $tr.find('input[id*=hdnSTaName]').val($("#<%=hdnSTaxName.ClientID%>").val());
                                }
                            });--%>


                        }
                        else {
                            $("#<%=txtqst.ClientID%>").text("0");
                            $("#<%=hdnQST.ClientID%>").val("0");
                            $("#<%=hdnQSTGL.ClientID%>").val("0");
                            $("#<%=hdnSTaxType.ClientID%>").val("0");
                            $("#<%=hdnSTaxName.ClientID%>").val("0");
                            $("#<%=hdnSTaxState.ClientID%>").val("");
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




                        if ($('#<%= txtDueIn.ClientID %>').val() != '') {
                            var newduedt = new Date();
                            newduedt.setDate(newduedt.getDate() + parseInt($('#<%= txtDueIn.ClientID %>').val()));
                            var dd = newduedt.getDate();
                            var mm = newduedt.getMonth() + 1;
                            var y = newduedt.getFullYear();
                            if (parseInt(dd) < 10) { dd = "0" + dd; }
                            if (parseInt(mm) < 10) { mm = "0" + mm; }
                            var someFormattedDate = mm + '/' + dd + '/' + y;
                            $("#<%=txtDueDate.ClientID%>").val(someFormattedDate);
                        }

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
                };
        });
        function openfileDialog() {
            $("#<%=FileUploadControl.ClientID%>").click();
        }
        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=lnkFileUploaded.ClientID %>").click();
            }
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


        
            var gridCtrl;
  function GridCreated(sender, args) {
                gridCtrl = sender;
          }
  //function KeyPressed(key) {
  //  if (gridCtrl.get_element().disabled) {
  //    return false;
  //        }
  //      }
  function DisableGrid() {
                gridCtrl.get_element().disabled = "disabled";
            gridCtrl.ClientSettings.Selecting.AllowRowSelect = false;
            gridCtrl.ClientSettings.Resizing.AllowColumnResize = false;
            gridCtrl.ClientSettings.Resizing.AllowRowResize = false;
            gridCtrl.ClientSettings.AllowColumnsReorder = false;
            gridCtrl.ClientSettings.AllowDragToGroup = false;
            gridCtrl.ClientSettings.EnablePostBackOnRowClick = false;
            var links = gridCtrl.get_element().getElementsByTagName("a");
            var images = gridCtrl.get_element().getElementsByTagName("img");
            var inputs = gridCtrl.get_element().getElementsByTagName("input");
            var textareas = gridCtrl.get_element().getElementsByTagName("textarea");
            var sortButtons = gridCtrl.get_element().getElementsByTagName("span");
    for (var i = 0; i < links.length; i++) {
                links[i].href = "";
      links[i].onclick = function () {
        return false;
          }
        }
    for (var i = 0; i < images.length; i++) {
                images[i].onclick = function () {
                    return false;
                }
            }
            for (var i = 0; i < sortButtons.length; i++) {
                sortButtons[i].onclick = function () {
                    return false;
                }
            }
            for (var i = 0; i < inputs.length; i++) {
      switch (inputs[i].type) {
        case "button":
          inputs[i].onclick = function () {
            return false;
          }
          break;
        case "checkbox":
          inputs[i].disabled = "disabled";
          break;
        case "radio":
          inputs[i].disabled = "disabled";
          break;
        case "text":
          inputs[i].disabled = "disabled";
          break;
        //case "textarea":
        //  textareas[i].disabled = "disabled";
        //  break;
        case "password":
          inputs[i].disabled = "disabled";
          break;
        case "image":
          inputs[i].onclick = function () {
            return false;
          }
          break;
        case "file":
          inputs[i].disabled = "disabled";
          break;
        default:
          break;
      }
      }
      for (var i = 0; i < textareas.length; i++) {
          textareas[i].disabled = "disabled";
      }


    var scrollArea = $find("<%= RadGrid_gvJobCostItems.ClientID %>").GridDataDiv;
    if (scrollArea) {
                scrollArea.disabled = "disabled";
          }
        }
  function EnableGrid() {
                $find("<%=RadAjaxManager_gvJobCostItems.ClientID %>").ajaxRequest("");
          }
  






        function checkPORPO() {
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
                        var IsSalesTaxAPBill = ui[0].IsSalesTaxAPBill;
                        var IsUseTaxAPBill = ui[0].IsUseTaxAPBill;
                        if (IsSalesTaxAPBill == "1") {


                            document.getElementById('txtqstgv').style.display = 'block';
                            //document.getElementById('txtgstgv').style.display = 'block';
                        } else {



                            document.getElementById('txtqstgv').style.display = 'none';
                            //document.getElementById('txtgstgv').style.display = 'none';
                        }

                        if (IsUseTaxAPBill == "1") {
                            document.getElementById('txttaxcodegv').style.display = 'none';

                        } else {

                            document.getElementById('txttaxcodegv').style.display = 'none';

                        }
                        if (IsUseTaxAPBill == "1") {
                            document.getElementById('txtusetaxcodegv').style.display = 'block';

                        } else {

                            document.getElementById('txtusetaxcodegv').style.display = 'none';

                        }

                        //$(txtGvAcctName).val(ui[0].DefaultAcct);
                    }


                },
                error: function (result) {
                    alert("Due to unexpected errors we were unable to load phase details");
                }
            });
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_gvJobCostItems" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnAddNewLines">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvJobCostItems" LoadingPanelID="RadAjaxLoadingPanel_gvJobCostItems" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnCopyPrevious">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvJobCostItems" LoadingPanelID="RadAjaxLoadingPanel_gvJobCostItems" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnSelectPo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtVendor" />
                    <telerik:AjaxUpdatedControl ControlID="txtPO" />
                    <telerik:AjaxUpdatedControl ControlID="txtReceptionId" />
                    <telerik:AjaxUpdatedControl ControlID="hdnVendorID" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSTax" />
                    <telerik:AjaxUpdatedControl ControlID="hdnSTaxState" />

                    <telerik:AjaxUpdatedControl ControlID="txtgstgv" />


                    <telerik:AjaxUpdatedControl ControlID="txtqst" />
                    <telerik:AjaxUpdatedControl ControlID="hdnQST" />
                    <telerik:AjaxUpdatedControl ControlID="hdnQSTGL" />
                    <telerik:AjaxUpdatedControl ControlID="hdnSTaxType" />
                    <telerik:AjaxUpdatedControl ControlID="hdnSTaxName" />
                    <telerik:AjaxUpdatedControl ControlID="txtusetaxc" />
                    <telerik:AjaxUpdatedControl ControlID="hdnusetaxc" />
                    <telerik:AjaxUpdatedControl ControlID="hdnusetaxcGL" />
                    <telerik:AjaxUpdatedControl ControlID="hdnUTaxType" />
                    <telerik:AjaxUpdatedControl ControlID="hdnUTaxName" />
                    <%--<telerik:AjaxUpdatedControl ControlID="hdnGST"/>
                    <telerik:AjaxUpdatedControl ControlID="hdnGSTGL"/>--%>

                    <telerik:AjaxUpdatedControl ControlID="txtMemo" />
                    <telerik:AjaxUpdatedControl ControlID="hdnTotalAmount" />
                    
                    <telerik:AjaxUpdatedControl ControlID="txtPaid" />
                    <telerik:AjaxUpdatedControl ControlID="chkPOClose" />


                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvJobCostItems" LoadingPanelID="RadAjaxLoadingPanel_gvJobCostItems" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSelectRPo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtVendor" />
                    <telerik:AjaxUpdatedControl ControlID="txtPO" />
                    <telerik:AjaxUpdatedControl ControlID="txtReceptionId" />
                    <telerik:AjaxUpdatedControl ControlID="hdnVendorID" />
                    <telerik:AjaxUpdatedControl ControlID="ddlSTax" />
                    <telerik:AjaxUpdatedControl ControlID="hdnSTaxState" />

                    <telerik:AjaxUpdatedControl ControlID="txtqst" />
                    <telerik:AjaxUpdatedControl ControlID="hdnQST" />
                    <telerik:AjaxUpdatedControl ControlID="hdnQSTGL" />
                    <telerik:AjaxUpdatedControl ControlID="hdnSTaxType" />
                    <telerik:AjaxUpdatedControl ControlID="hdnSTaxName" />
                    <telerik:AjaxUpdatedControl ControlID="txtusetaxc" />
                    <telerik:AjaxUpdatedControl ControlID="hdnusetaxc" />
                    <telerik:AjaxUpdatedControl ControlID="hdnusetaxcGL" />
                    <telerik:AjaxUpdatedControl ControlID="hdnUTaxType" />
                    <telerik:AjaxUpdatedControl ControlID="hdnUTaxName" />
                    <%--<telerik:AjaxUpdatedControl ControlID="hdnGST"/>
                    <telerik:AjaxUpdatedControl ControlID="hdnGSTGL"/>--%>
                    <telerik:AjaxUpdatedControl ControlID="txtMemo" />
                    <telerik:AjaxUpdatedControl ControlID="hdnTotalAmount" />
                    
                    <telerik:AjaxUpdatedControl ControlID="txtPaid" />
                    <telerik:AjaxUpdatedControl ControlID="txtgstgv" />
                    <telerik:AjaxUpdatedControl ControlID="chkPOClose" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvJobCostItems" LoadingPanelID="RadAjaxLoadingPanel_gvJobCostItems" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_gvJobCostItems" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="addbil45">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-editor-attach-money"></i>&nbsp;
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Enter Bills</asp:Label>
                                        <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label>
                                    </div>
                                    <div class="btnlinks">
                                        <asp:LinkButton Text="Save" ID="btnSubmit" runat="server" ToolTip="Save" OnClick="btnSubmit_Click" CausesValidation="true"
                                            ValidationGroup="bills" OnClientClick="javascript:return ConfirmRef(this); disableButton(this,'bills'); javascript:return testconfirmSubmit();  itemJSON();"></asp:LinkButton>
                                    </div>
                                    <div class="btnlinks">
                                        <asp:LinkButton Text="Update Project" ID="btnSubmitJob" runat="server" ToolTip="Save Project Specific Details" OnClick="btnSubmitJob_Click" CausesValidation="true"
                                            ValidationGroup="bills" OnClientClick="disableButton(this,'bills'); itemJSON();" Visible="false"></asp:LinkButton>
                                    </div>
                                    <div class="btnlinks">
                                        <a id="aImport" runat="server" class="dropdown-button" data-beloworigin="true" href="javascript:void(0)" data-activates="dynamicUI">Import</a>
                                        <asp:LinkButton Text="Quick Check" ID="lnkQuickCheck" runat="server" ToolTip="Quick Check" OnClick="btnQuickcheck_Click" CausesValidation="true"
                                            ValidationGroup="bills" OnClientClick="javascript:return ConfirmRef(this); javascript:return testconfirmSubmit(); disableButton(this,'bills'); itemJSON();"></asp:LinkButton>
                                        <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="hidden" onchange="UploadFile(this);" />
                                        <asp:LinkButton ID="lnkFileUploaded" runat="server" Text="Import Items" CssClass="hidden" OnClick="lnkFileUploaded_Click" />
                                    </div>

                                    <ul id="dynamicUI" class="dropdown-content">
                                        <li><a id="btnImportItems" title="Import Items" href="javascript:void(0)" onclick="openfileDialog();">Upload File</a></li>
                                        <li>
                                            <asp:LinkButton Text="CSV Template" runat="server" ID="btnDownloadCSV" OnClick="btnDownloadCSV_Click" /></li>
                                        <li>
                                            <asp:LinkButton Text="Excel Template" runat="server" ID="btnDownloadExcel" OnClick="btnDownloadExcel_Click" /></li>
                                    </ul>

                                    <div class="rght-content">
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                                OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                        </div>
                                        <div class="editlabel">
                                            <asp:Label ID="lblVendorName" runat="server" Visible="false"></asp:Label>
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
                                <asp:Panel ID="pnlSave" runat="server">
                                    <ul class="anchor-links">

                                        <li><a href="#accrdBills">Bill Details</a></li>
                                        <li id="liDocuments"><a href="#accrdDocuments">Documents</a></li>
                                        <li id="liHistoryPayment" runat="server" style="display: none"><a href="#accrdPayment">Payment History</a></li>
                                        <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                    </ul>
                                </asp:Panel>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlNext" runat="server" Visible="False">
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
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False"
                                                OnClick="lnkLast_Click">
                                                <i class="fa fa-angle-double-right"></i>
                                            </asp:LinkButton></span>


                                    </asp:Panel>


                                </div>



                            </div>
                        </div>
                        <div>

                            <div class="tblnksright">
                                <asp:Panel ID="pnlRecurring" runat="server" class="pnlR-css" >
                                    <span class="angleicons p-t-15">
                                        <asp:CheckBox ID="chkPOClose" runat="server" Text="Close PO" CssClass="css-checkbox" Style="padding-right: 10px;" Visible="false" /></span>
                                    <span class="angleicons p-t-15">
                                        <asp:CheckBox ID="chkIsRecurr" runat="server" Text="Is Recurring" CssClass="css-checkbox" onclick="showfreq();" /></span>

                                    <span class="angleicons angle-css"  id="dvfreq">

                                        <asp:DropDownList ID="ddlFrequency" runat="server" CssClass="browser-default"></asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvFrequency" ErrorMessage="Please select Frequency"
                                            Display="None" ControlToValidate="ddlFrequency" ValidationGroup="Journal"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceFrequency" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvFrequency" />
                                    </span>
                                </asp:Panel>
                            </div>


                        </div>
                    </div>

                </div>
            </div>


        </div>
    </div>
    <div class="container">
        <div class="alert alert-success" runat="server" id="divSuccess">
            <button type="button" class="close" data-dismiss="alert">×</button>
            These month/year period is closed out. You do not have permission to add/update this record.
        </div>
        <div class="row">
            <div class="col s12 m12 l12 active" id="accrdBills">
                <div class="row">
                    <div class="card cardradius">
                        <div class="card-content p-t-10">
                            <div class="form-content-wrap">
                                <div class="form-content-pd">
                                    <div class="form-section-row">
                                        <asp:HiddenField ID="hdnRowField" runat="server" />
                                        <asp:HiddenField ID="hdnBatch" runat="server" />
                                        <asp:HiddenField ID="hdnTransID" runat="server" />
                                        <asp:HiddenField ID="hdnStatus" runat="server" />
                                        <asp:HiddenField ID="hdnGLItem" runat="server" />
                                        <asp:HiddenField runat="server" ID="hdnInvDefaultAcctID" Value="" />
                                        <asp:HiddenField runat="server" ID="hdnInvDefaultAcctName" Value="" />
                                        <asp:Button ID="btnSelectPo" runat="server" CausesValidation="False" OnClick="btnSelectPo_Click"
                                            Style="display: none;" Text="Button" />
                                        <asp:Button ID="btnSelectRPo" runat="server" CausesValidation="False" OnClick="btnSelectRPo_Click"
                                            Style="display: none;" Text="Button" />
                                        <asp:Button ID="btnStaxType" runat="server" CausesValidation="False" OnClientClick="itemJSON();" OnClick="btnStaxType_Click"
                                            Style="display: none;" Text="Button" />
                                        <asp:Button ID="btnUpdtStax" runat="server" CausesValidation="False" OnClick="btnUpdtStax_Click" OnClientClick="itemJSON();"
                                            Style="display: none;" Text="Button" />

                                        <div class="col s12 m12 l12">
                                            <div class="row">

                                                <div class="form-section-row">
                                                    <div class="form-input-row ml8" id="divBillInfo" runat="server">
                                                        <div class="form-section3-div4">
                                                            <div class="input-field col s12">
                                                                <div class="row">

                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvVendor" ErrorMessage="Please select Vendor"
                                                                        Display="None" ControlToValidate="txtVendor" ValidationGroup="bills"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceVendor" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="rfvVendor" />
                                                                    <asp:CustomValidator ID="cvVendor" runat="server" ClientValidationFunction="ChkVendor"
                                                                        ControlToValidate="txtVendor" Display="None" ErrorMessage="Please select the vendor"
                                                                        SetFocusOnError="True" Enabled="False" ValidationGroup="bills"></asp:CustomValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceVendor1" runat="server" Enabled="True"
                                                                        PopupPosition="BottomLeft" TargetControlID="cvVendor">
                                                                    </asp:ValidatorCalloutExtender>

                                                                    <asp:TextBox ID="txtVendor" runat="server" MaxLength="75"
                                                                        placeholder="Search by vendor"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnVendorID" runat="server" />
                                                                    <asp:HiddenField ID="hdEditCase" Value="false" runat="server" />
                                                                    <label for="txtVendor" class="active">Vendor</label>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">

                                                                    <asp:TextBox ID="txtPO" runat="server" CssClass="posearchinput"
                                                                        MaxLength="15" placeholder="PO#"></asp:TextBox>
                                                                    <asp:RangeValidator ID="rfvPO" ControlToValidate="txtPO" MinimumValue="0" MaximumValue="2147483647"
                                                                        Type="Integer" ErrorMessage="PO# must be a number" runat="server" Display="None" ValidationGroup="bills" />
                                                                    <asp:ValidatorCalloutExtender ID="vcePO" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                        TargetControlID="rfvPO" />
                                                                    <asp:HiddenField ID="hdnPO" runat="server" />
                                                                    <asp:HiddenField ID="hdnReceivedAmount" runat="server" />
                                                                    <asp:HiddenField ID="hdnTotalAmount" runat="server" />

                                                                    <label for="txtPO" class="active">PO#</label>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">

                                                                    <asp:TextBox ID="txtReceptionId" runat="server" CssClass="form-control rposearchinput" TabIndex="2" onchange="chkPORPO()"
                                                                        MaxLength="15" placeholder="RPO#"></asp:TextBox>
                                                                    <asp:RangeValidator ID="rvReceptionId" ControlToValidate="txtReceptionId" MinimumValue="0" MaximumValue="2147483647"
                                                                        Type="Integer" ErrorMessage="Reception ID must be a number" runat="server" Display="None" ValidationGroup="bills" />
                                                                    <asp:ValidatorCalloutExtender ID="vceReceptionId" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                        TargetControlID="rvReceptionId" />

                                                                    <label for="txtReceptionId" class="active">Reception No#</label>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">

                                                                    <%--<asp:TextBox ID="txtRef" runat="server" CssClass="form-control" TabIndex="2" MaxLength="50" onfocusout="ConfirmRef(this)"></asp:TextBox>--%>
<asp:TextBox ID="txtRef" runat="server" CssClass="form-control" TabIndex="2" MaxLength="50" ></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvRef" ErrorMessage="Please select Ref"
                                                                        Display="None" ControlToValidate="txtRef" ValidationGroup="bills"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceRef" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="rfvRef" />

                                                                    <label for="txtRef" class="active">Ref No.</label>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12 pdrt lblfield r-dis" id="txtgstgv" runat="server">
                                                                <div class="row">
                                                                    <span class="ttlab">GST %</span>
                                                                    <span class="ttlval">
                                                                        <asp:Label ID="txtgst" runat="server" Text="0.00"></asp:Label></span>
                                                                    <asp:HiddenField ID="hdnGSTGL" runat="server" />
                                                                    <asp:HiddenField ID="hdnGST" runat="server" />
                                                                </div>
                                                            </div>



                                                            <div class="input-field col s12 pdrt lblfield" style="display: none;" id="txttaxcodegv">
                                                                <div class="row">
                                                                    <span class="ttlab">Use Tax</span>
                                                                    <span class="ttlval">
                                                                        <asp:Label ID="txtusetaxc" runat="server" Text="0.00"></asp:Label></span>
                                                                    <asp:HiddenField ID="hdnusetaxcGL" runat="server" />
                                                                    <asp:HiddenField ID="hdnusetaxc" runat="server" />
                                                                    <asp:HiddenField ID="hdnUTaxType" runat="server" />
                                                                    <asp:HiddenField ID="hdnUTaxName" runat="server" />
                                                                </div>
                                                            </div>




                                                        </div>
                                                        <div class="form-section3-div4blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3-div4">

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtVendorType" runat="server" placeholder="  " disabled="disabled" ReadOnly="true"></asp:TextBox>
                                                                    <label for="txtVendorType" class="active">Vendor Type</label>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label>Date</label>

                                                                    <asp:TextBox ID="txtDate" runat="server" class="datepicker_mom" onchange="changeDueDate();"></asp:TextBox>
                                                                    <%-- <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                                                        TargetControlID="txtDate">
                                                                    </asp:CalendarExtender>--%>
                                                                    <asp:RequiredFieldValidator ID="rfvDate" ValidationGroup="bills"
                                                                        runat="server" ControlToValidate="txtDate" Display="None" ErrorMessage="Date is Required"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvDate" />
                                                                    <asp:RegularExpressionValidator ID="revDate" ControlToValidate="txtDate" ValidationGroup="bills"
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="revDate" />

                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">

                                                                    <asp:TextBox ID="txtPostingDate" runat="server" class="datepicker_mom"
                                                                        MaxLength="15"></asp:TextBox>

                                                                    <asp:RequiredFieldValidator ID="rfvPostDate" ValidationGroup="bills"
                                                                        runat="server" ControlToValidate="txtPostingDate" Display="None" ErrorMessage="Posting date is Required"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vcePostDate" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvPostDate" />
                                                                    <asp:RegularExpressionValidator ID="revPostDate" ControlToValidate="txtPostingDate" ValidationGroup="bills"
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vcePostDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="revPostDate" />
                                                                    <asp:Label runat="server" ID="lblPostingDate" AssociatedControlID="txtPostingDate">Posting Date</asp:Label>

                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label>Due</label>


                                                                    <asp:TextBox ID="txtDueDate" runat="server" class="datepicker_mom" onchange="changeDueDate()"
                                                                        MaxLength="15"> </asp:TextBox>
                                                                    <asp:CalendarExtender ID="txtDueDate_CalendarExtender" runat="server" Enabled="True"
                                                                        TargetControlID="txtDueDate">
                                                                    </asp:CalendarExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvDueDate"
                                                                        runat="server" ControlToValidate="txtDueDate" Display="None" ErrorMessage="Due date is Required" ValidationGroup="bills"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceDueDate" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvDueDate" />
                                                                    <asp:RegularExpressionValidator ID="revDueDate" ControlToValidate="txtDueDate" ValidationGroup="bills"
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceDueDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="revDueDate" />
                                                                </div>
                                                            </div>








                                                        </div>
                                                        <div class="form-section3-div4blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3-div4">

                                                            <div class="input-field col s12">
                                                                <div class="row">

                                                                    <div class="input-field col s5">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtDueIn" runat="server"
                                                                                MaxLength="3" onkeypress="return isNum(event)" onchange="changeDueDate1();"></asp:TextBox>
                                                                            <label for="txtDueIn">Due In</label>
                                                                        </div>
                                                                    </div>

                                                                    <div class="input-field col s2">
                                                                        <div class="row">
                                                                            &nbsp;
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s5">
                                                                        <div class="row">
                                                                            <asp:TextBox ID="txtPaid" runat="server"
                                                                                MaxLength="2" onkeypress="return isNum(event)"></asp:TextBox>
                                                                            <label for="txtPaid" class="active">If Paid</label>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtDisc" runat="server"
                                                                        MaxLength="3"></asp:TextBox>
                                                                    <label for="txtDisc">% Disc</label>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12 pdrt lblfield r-dis">
                                                                <div class="row">
                                                                    <span class="ttlab">Total</span>
                                                                    <span class="ttlval">$<asp:Label ID="lblTotalAmount" runat="server"></asp:Label></span>

                                                                </div>
                                                            </div>


                                                            <div class="input-field col s12" style="display: none;" id="txtusetaxcodegv">
                                                                <div class="row">
                                                                    <label for="drpdwn-label">Select Use Tax</label>
                                                                    <%--<span class="ttlab">Use Tax</span>--%>

                                                                    <asp:TextBox ID="txtTotalUseTax" runat="server" CssClass="form-control  clsUseTax tsearchTotalUseTax"
                                                                        MaxLength="15" onblur="CalculateUseTaxTotal(this);"></asp:TextBox>
                                                                    <asp:HiddenField ID="husetaxGL" runat="server" />
                                                                    <asp:HiddenField ID="husetaxName" runat="server" />
                                                                    <asp:HiddenField ID="husetaxRate" runat="server" />
                                                                </div>
                                                                <div class="srchclr btnlinksicon rowbtn">$<asp:Label ID="lblTotalUseTax" runat="server"></asp:Label></div>
                                                            </div>

                                                            <%--<div class="input-field col s12" style="display:none;" id="txtqstgv">
                                                                <div class="row">

                                                                    <asp:TextBox ID="txtqst" runat="server" CssClass="form-control" MaxLength="20" ></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvqst" ErrorMessage="Please enter QST"
                                                                        Display="None" ControlToValidate="txtqst" ValidationGroup="bills"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="rfvqst" />

                                                                    <label for="txtqst">Sales Tax Name with Rate</label>
                                                                    <asp:HiddenField ID="hdnQSTGL" runat="server" />
                                                                    <asp:HiddenField ID="hdnQST" runat="server" />
                                                                    <asp:HiddenField ID="hdnSTaxType" runat="server" />
                                                                    <asp:HiddenField ID="hdnSTaxName" runat="server" />
                                                                    
                                                                    
                                                                </div>
                                                            </div>--%>

                                                            <%--<div class="input-field col s12 pdrt lblfield" style="display:none;" id="txtqstgv">--%>
                                                            <div class="input-field col s12" style="display: none;" id="txtqstgv">
                                                                <div class="row">



                                                                    <%--<label for="drpdwn-label" id="spansalestax" runat="server">Select Tax</label>
                                                                    <asp:TextBox ID="txtqst" runat="server" CssClass="form-control  clsUseTax tsearchTotalUseTax"
                                                                        MaxLength="15" onblur="CalculateUseTaxTotal(this);"></asp:TextBox>--%>
                                                                    <%--<span class="ttlab" id="spansalestax" runat="server">Sales Tax</span>--%>
                                                                    <span class="ttlval" style="display: none;">
                                                                        <asp:Label ID="txtqst" runat="server" Text="0.00"></asp:Label></span>
                                                                    <asp:HiddenField ID="hdnQSTGL" runat="server" />
                                                                    <asp:HiddenField ID="hdnQST" runat="server" />
                                                                    <asp:HiddenField ID="hdnSTaxType" runat="server" />
                                                                    <asp:HiddenField ID="hdnSTaxName" runat="server" />
                                                                    <asp:HiddenField ID="hdnSTaxState" runat="server" />
                                                                    <asp:HiddenField ID="hdnUpdateStax" runat="server" />
                                                                    <label class="drpdwn-label" id="spansalestax" runat="server">Sales Tax</label>
                                                                    <asp:DropDownList ID="ddlSTax" CssClass="browser-default" runat="server" onchange="GetSelectedSTax(this)">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="form-section3-div4blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3-div4">
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default">
                                                                    </asp:DropDownList>

                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtMemo" runat="server"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvMemo" ErrorMessage="Please enter Memo" ControlToValidate="txtMemo"
                                                                        Display="None" ValidationGroup="bills"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceMemo" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                        TargetControlID="rfvMemo" />

                                                                    <label for="txtMemo" class="active">Memo</label>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtCustom1" runat="server" MaxLength="50" >
                                                                    </asp:TextBox>
                                                                    <label id="lb_txtCustom1" runat="server" for="textarea1">Custom1</label>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtCustom2" runat="server" MaxLength="50" >
                                                                    </asp:TextBox>
                                                                    <label id="lb_txtCustom2" runat="server" for="txtCustom2">Custom2</label>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">

                                                                    <asp:ImageButton ID="imgPaid" ImageUrl="~/images/icons/paid.png" runat="server" Height="40px" OnClientClick="scrollToAnchor();return false;" />
                                                                    <asp:Image ID="imgVoid" runat="server" ImageUrl="~/images/icons/void.png" Height="40px" />
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
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="grid_container scroller" style="overflow-y: scroll;">
                <div class="form-section-row m-b-0">
                    <div class="RadGrid RadGrid_Material FormGrid">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_gvJobCostItems.ClientID %>");
                                    var columns = grid.get_masterTableView().get_columns();
                                    for (var i = 0; i < columns.length; i++) {
                                        columns[i].resizeToFit(false, true);
                                    }
                                }

                                var requestInitiator = null;
                                var selectionStart = null;

                                function requestStart1(sender, args) {
                                    requestInitiator = document.activeElement.id;
                                    if (document.activeElement.tagName == "INPUT") {
                                        selectionStart = document.activeElement.selectionStart;
                                    }


                                }

                                function responseEnd1(sender, args) {
                                    try {
                                        var element = document.getElementById(requestInitiator);
                                        if (element && element.tagName == "INPUT") {
                                            element.focus();
                                            element.selectionStart = selectionStart;
                                        }

                                    } catch (e) {

                                    }
                                }
                            </script>
                        </telerik:RadCodeBlock>

                        <telerik:RadAjaxPanel ID="RadAjaxPanel_gvJobCostItems" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvJobCostItems" ClientEvents-OnRequestStart="requestStart1" ClientEvents-OnResponseEnd="responseEnd1">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvJobCostItems" ShowFooter="True"
                                ShowStatusBar="true" runat="server" AllowSorting="true" Width="120%" OnPreRender="RadGrid_gvJobCostItems_PreRender" OnItemCommand="RadGrid_gvJobCostItems_ItemCommand" onblur="resetIndexF6()">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    <ClientEvents OnKeyPress="KeyPressed" OnGridCreated="GridCreated" />
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
                                            HeaderText="Project" ShowFilterIcon="false">
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
                                                    MaxLength="8000"
                                                    Style="min-height: 32px; font-size: 12px; padding: 0px !important; height: 17px;"
                                                    Text='<%# Bind("fDesc") %>'></asp:TextBox>
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>


                                        <telerik:GridTemplateColumn DataField="WarehouseID" SortExpression="WarehouseID" AutoPostBackOnFilter="true"
                                            HeaderText="Warehouse" UniqueName="Warehouse" ShowFilterIcon="false" HeaderStyle-Width="120" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvWarehouse" ReadOnly='<%# Eval("Phase").ToString() != "Inventory" ? true : false %>'
                                                    Text='<%# Bind("Warehousefdesc") %>' Height="26px"
                                                    runat="server" CssClass="texttransparent Warehousesearchinput"></asp:TextBox>
                                                <asp:HiddenField ID="hdnWarehousefdesc" runat="server" Value='<%# Bind("Warehousefdesc") %>'></asp:HiddenField>
                                                <asp:HiddenField ID="hdnWarehouse" runat="server" Value='<%# Bind("Warehouse") %>'></asp:HiddenField>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>


                                        <telerik:GridTemplateColumn DataField="LocationID" UniqueName="WHLocID" SortExpression="LocationID" AutoPostBackOnFilter="true"
                                            HeaderText="Warehouse Location" ShowFilterIcon="false" HeaderStyle-Width="120" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvWarehouseLocation" ReadOnly='<%# Eval("Phase").ToString() != "Inventory" ? true : false %>'
                                                    Text='<%# Bind("Locationfdesc") %>' Height="26px"
                                                    runat="server" CssClass="texttransparent WarehouseLocationsearchinput "></asp:TextBox>
                                                <asp:HiddenField ID="hdnLocationfdesc" runat="server" Value='<%# Bind("Locationfdesc") %>'></asp:HiddenField>
                                                <asp:HiddenField ID="hdnWarehouseLocationID" runat="server" Value='<%# Bind("WHLocID") %>'></asp:HiddenField>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>






                                        <telerik:GridTemplateColumn DataField="AcctNo" SortExpression="AcctNo" AutoPostBackOnFilter="true"
                                            HeaderText="Acct No." ShowFilterIcon="false">
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
                                            CurrentFilterFunction="Contains" HeaderText="Price Per" ShowFilterIcon="false" HeaderStyle-Width="100" ItemStyle-Width="100">
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
                                        <telerik:GridTemplateColumn DataField="Amount" SortExpression="Amount" AutoPostBackOnFilter="true"
                                            HeaderText="Pretax Amount" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvAmount"
                                                    runat="server"
                                                    CssClass="form-control texttransparent clsAmount" autocomplete="off"
                                                    MaxLength="15"
                                                    Width="100%"
                                                    DataFormatString="{0:c}"
                                                    onkeypress="return isDecimalKey(this,event);"
                                                    ondblclick="enableamount(this)"
                                                    Height="26px" Text='<%# Bind("Amount") %>'
                                                    Style="text-align: right; font-size: 12px;"
                                                    onchange="CalculateTotal(this);"></asp:TextBox>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                                <asp:Label ID="lblAmountPerTotalGrid" runat="server" Style="text-align: left;"></asp:Label>
                                            </FooterTemplate>

                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn DataField="Amount" SortExpression="Amount" AutoPostBackOnFilter="true"
                                            HeaderText="Use Tax" ShowFilterIcon="false" HeaderStyle-Width="90" ItemStyle-Width="90">
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
                                                <asp:CheckBox ID="chkSelectAllGtax" runat="server" /><span class="pl-10" >GST</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkGTaxable" runat="server" Checked='<%#Convert.ToBoolean(Eval("GTax"))%>'></asp:CheckBox>

                                                <asp:HiddenField ID="hdnchkGTaxable" Value='<%# Bind("GTax") %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>



                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="GST Tax" UniqueName="GTaxAmt" HeaderStyle-Width="100" FooterStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:TextBox ID="lblGstTax" runat="server"
                                                    Style="color: #2392D7; text-align: center;" Height="26px" Text='<%# Eval("GTaxAmt") != DBNull.Value ? Convert.ToDouble(Eval("GTaxAmt")).ToString("N", System.Globalization.CultureInfo.InvariantCulture) : "" %>'
                                                    MaxLength="15" onblur="TotalwithGTax(this);" onkeypress="return isDecimalKey(this,event);"></asp:TextBox>
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
                                                <asp:CheckBox ID="chkSelectAllStax" runat="server" /><span class="pl-10" style="padding-left: 10px;">PST</span>
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
                                                                                                MaxLength="15"></asp:TextBox>--%>

                                                <%--                                        <asp:TextBox ID="lblSalesTax" runat="server" CssClass="form-control texttransparent clsUseTax ttsearchinput"
                                                    autocomplete="off" Width="100%" Height="26px"
                                                    Text='<%# Eval("STaxRate") == DBNull.Value ? "" : Eval("STaxRate") %>'
                                                    Style="text-align: right; font-size: 12px;" onblur="CalculateSalesTaxTotal(this);" onkeypress="return false;" ></asp:TextBox>--%>
                                                <asp:TextBox ID="lblSalesTax" runat="server"
                                                    autocomplete="off" Width="100%" Height="26px"
                                                    Text='<%# Eval("STaxRate") == DBNull.Value ? "" : Eval("STaxRate") %>'
                                                    Style="display: none;" onkeypress="return false;"></asp:TextBox>
                                                <asp:TextBox ID="txtGvStaxAmount" runat="server"
                                                    autocomplete="off" Width="100%" Height="26px"
                                                    Text='<%# Eval("STaxAmt") != DBNull.Value ? Convert.ToDouble(Eval("STaxAmt")).ToString("N", System.Globalization.CultureInfo.InvariantCulture) : "" %>'
                                                    onblur="TotalwithTax(this);" onkeypress="return isDecimalKey(this,event);"
                                                    CssClass="form-control texttransparent clsAmount"
                                                    MaxLength="15"></asp:TextBox>
                                                <asp:HiddenField ID="hdnSTaxAm" Value='<%# Bind("STaxAmt") %>' runat="server" />
                                                <asp:HiddenField ID="hdnSTaxGL" Value='<%# Bind("STaxGL") %>' runat="server" />
                                                <asp:HiddenField ID="hdnSTaName" Value='<%# Bind("STaxName") %>' runat="server" />
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
                                            HeaderText="Location Name" ShowFilterIcon="false">
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
                                                <asp:HiddenField ID="hdnIsPO" Value='<%# Bind("IsPO") %>' runat="server" />

                                                <asp:HiddenField ID="hdnOrderedQuan" Value='<%# Bind("OrderedQuan") %>' runat="server" />
                                                <asp:HiddenField ID="hdnOrdered" Value='<%# Bind("Ordered") %>' runat="server" />
                                                <asp:HiddenField ID="hdnRPOItemId" runat="server" Value='<%# Bind("RPOItemId") %>'></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnPOItemId" runat="server" Value='<%# Bind("POItemId") %>'></asp:HiddenField>
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

                            <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none" CausesValidation="False" />
                            <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="errorWindow" Skin="Material" VisibleTitlebar="true" Title="Error List" Behaviors="Default" CenterIfModal="true"
                                        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                        runat="server" Modal="true" Width="1000" Height="600">
                                        <ContentTemplate>
                                            <div class="m-t-15">
                                                <div class="col-lg-12 col-md-12 form-section-row">
                                                    <div style="float: right;">
                                                        <span>Total Rows :
                                                            <asp:Label ID="lblTotalRows" runat="server" />
                                                            |</span>
                                                        <span class="valid-green">Valid Rows :
                                                            <asp:Label ID="lblValidRows" runat="server" />
                                                            |</span>
                                                        <span class="invalid-red" >Invalid Rows :
                                                            <asp:Label ID="lblInvalidRows" runat="server" /></span>
                                                    </div>
                                                </div>
                                                <div style="clear: both;"></div>
                                                <div class="RadGrid red-grid" >
                                                    <telerik:RadGrid RenderMode="Auto" ID="gv_Errorrows" ShowFooter="false"
                                                        ShowStatusBar="false" runat="server" AllowSorting="false" Width="100%">
                                                        <CommandItemStyle />
                                                        <GroupingSettings CaseSensitive="false" />
                                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                            <Selecting AllowRowSelect="false"></Selecting>
                                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            <Resizing ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
                                                        </ClientSettings>
                                                        <MasterTableView AutoGenerateColumns="false" ShowFooter="True" DataKeyNames="AccNo">
                                                            <Columns>
                                                                <telerik:GridTemplateColumn DataField="ProjNo" SortExpression="ProjNo" AutoPostBackOnFilter="true"
                                                                    HeaderText="Project" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblProjeNo" Text='<%# Bind("ProjNo")%>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Code" SortExpression="Code" AutoPostBackOnFilter="true"
                                                                    HeaderText="Code" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCode" Text='<%# Bind("Code") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="ItemDis" SortExpression="ItemDis" AutoPostBackOnFilter="true"
                                                                    HeaderText="Item Discription" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="ItemDis" Text='<%# Bind("ItemDis") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="AccNo" SortExpression="AccNo" AutoPostBackOnFilter="true"
                                                                    HeaderText="Acc No." ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAccNo" Text='<%# Bind("AccNo") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Amount" SortExpression="Amount" AutoPostBackOnFilter="true"
                                                                    HeaderText="$ Amount" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAmount" Text='<%# Bind("Amount") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="ErrorField" SortExpression="ErrorField" AutoPostBackOnFilter="true"
                                                                    HeaderText="Error Details" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblErrorField" Text='<%# Bind("ErrorField") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </div>
                                                <div class="col-lg-12 col-md-12 form-section-row">
                                                    <h6>Here are the list of invalid rows please check Error Details column for more information. Click cancel and correct the rows and import again or click continue to exclude those rows in import.</h6>
                                                </div>
                                                <div style="clear: both;"></div>
                                                <footer>
                                                    <div class="btnlinks pnlR-css">
                                                        <asp:LinkButton Text="Continue" runat="server" ID="btnContinue" OnClick="btnContinue_Click" />
                                                        <asp:LinkButton Text="Cancel" runat="server" ID="btnCancel" OnClick="btnCancel_Click" />
                                                    </div>
                                                </footer>
                                            </div>
                                        </ContentTemplate>
                                    </telerik:RadWindow>
                                    <telerik:RadWindow ID="ReprintCheckRange" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                        runat="server" Modal="true" Width="200" Height="180">
                                        <ContentTemplate>
                                            <div class="m-t-15">
                                                <div class="form-section-row">
                                                    <div class="form-section">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Apply Date </label>
                                                                <%--<label for="rdDated" class="radio-gap-label">Apply Date</label>--%>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtaplyDate" runat="server" CssClass="datepicker_mom" MaxLength="10"
                                                                        autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                                                    <%--   <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                TargetControlID="txtdated">
                                            </asp:CalendarExtender>--%>
                                                                    <asp:HiddenField ID="hdnolddate" runat="server" />
                                                                </div>
                                                                <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlBank"
                                                                                                    ErrorMessage="Please select Bank" Display="None" InitialValue="0"
                                                                                                    ValidationGroup="Check"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True" PopupPosition="Right"
                                                                                                    TargetControlID="rfvBank" />--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%--<div class="form-section3-blank">
                                                                                    &nbsp;
                                                                                </div>--%>
                                                </div>
                                                <div style="clear: both;"></div>
                                                <footer class="footer-css-top-btn1">
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkApplyCredit" runat="server" OnClientClick="CloseApplyCreditModal();" OnClick="btnApplyCredit_Click" AutoPostBack="true">Apply Credit</asp:LinkButton>
                                                    </div>
                                                </footer>
                                            </div>
                                        </ContentTemplate>
                                    </telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
            <div class="btnlinks">
                <asp:LinkButton ID="btnAddNewLines" runat="server" CausesValidation="false" OnClientClick="itemJSON();"
                    OnClick="lbtnAddNewLines_Click" Text="Add New Lines"></asp:LinkButton>
                <asp:LinkButton ID="btnCopyPrevious" runat="server" CausesValidation="false" OnClientClick="itemJSON();"
                    OnClick="btnCopyPrevious_Click" Text="Copy Previous" Style="display: none;"></asp:LinkButton>
            </div>
        </div>
    </div>

    <div class="container accordian-wrap">
        <div class="col s12 m12 l12">
            <div class="row">
                <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                    <li id="tbDocuments" runat="server">
                        <div id="accrdDocuments" class="collapsible-header accrd accordian-text-custom"><i class="mdi-file-attachment"></i>Documents</div>
                        <div class="collapsible-body">
                            <asp:Panel ID="pnlDocPermission" runat="server">
                                <div class="form-section-row">
                                    <div class="col s12 m12 l12">
                                        <div class="row">
                                            <asp:FileUpload ID="FileUpload1" runat="server" class="dropify" AllowMultiple="true" onchange="AddDocumentClick(this);" />
                                        </div>
                                    </div>
                                </div>
                                <div class="btncontainer">
                                    <asp:Panel ID="pnlDocumentButtons" runat="server">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                            <asp:LinkButton ID="lnkUploadDoc" runat="server" CausesValidation="False" OnClick="lnkUploadDoc_Click" Style="display: none">Upload</asp:LinkButton>
                                            <asp:LinkButton ID="lnkPostback" runat="server" CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                        </div>
                                    </asp:Panel>
                                    <div style="clear: both;"></div>
                                </div>
                                <div class="form-section-row">
                                    <div class="grid_container">
                                        <div class="RadGrid RadGrid_Material FormGrid">
                                            <telerik:RadCodeBlock ID="RadCodeBlock_Documents" runat="server">
                                                <script type="text/javascript">
                                                    function pageLoad() {
                                                        var grid = $find("<%= RadGrid_Documents.ClientID %>");
                                                        var columns = grid.get_masterTableView().get_columns();
                                                        for (var i = 0; i < columns.length; i++) {
                                                            columns[i].resizeToFit(false, true);
                                                        }
                                                    }

                                                    var requestInitiator = null;
                                                    var selectionStart = null;

                                                    function requestStart2(sender, args) {
                                                        requestInitiator = document.activeElement.id;
                                                        if (document.activeElement.tagName == "INPUT") {
                                                            selectionStart = document.activeElement.selectionStart;
                                                        }


                                                    }

                                                    function responseEnd2(sender, args) {
                                                        try {
                                                            var element = document.getElementById(requestInitiator);
                                                            if (element && element.tagName == "INPUT") {
                                                                element.focus();
                                                                element.selectionStart = selectionStart;
                                                            }

                                                        } catch (e) {

                                                        }
                                                    }
                                                </script>

                                            </telerik:RadCodeBlock>

                                            <telerik:RadAjaxPanel ID="RadAjaxPanel_Documents" PostBackControls="lblName" runat="server" ClientEvents-OnRequestStart="requestStart2" ClientEvents-OnResponseEnd="responseEnd2">
                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Documents" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                    PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_Documents_PreRender" OnNeedDataSource="RadGrid_Documents_NeedDataSource"
                                                    ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                    <CommandItemStyle />
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                        <Selecting AllowRowSelect="True"></Selecting>
                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                    </ClientSettings>
                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                        <Columns>
                                                            <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                                            </telerik:GridClientSelectColumn>

                                                            <telerik:GridTemplateColumn AllowFiltering="false" Visible="false" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdnTempId" Value='<%# Eval("id").ToString() == "0"? Eval("TempId"): string.Empty %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DataField="filename" SortExpression="filename" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                HeaderText="File Name" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                                        CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                        OnClientClick="return ViewDocumentClick(this);" OnClick="lblName_Click" Text='<%# Eval("filename") %>'>
                                                                    </asp:LinkButton>

                                                                </ItemTemplate>

                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridBoundColumn FilterDelay="5" DataField="doctype" HeaderText="File Type" HeaderStyle-Width="140"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="doctype"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>

                                                            <%--<telerik:GridTemplateColumn SortExpression="portal" HeaderText="Portal" DataField="portal" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# (Eval("portal")!=DBNull.Value) ? Convert.ToBoolean(Eval("portal")): false %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>--%>

                                                            <telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                                                HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="100"
                                                                DataType="System.Int16" UniqueName='MSVisible' >
                                                                <FilterTemplate>
                                                                    <telerik:RadComboBox RenderMode="Auto" ID="ImportedFilter" runat="server" OnClientSelectedIndexChanged="ImportedFilterSelectedIndexChanged"
                                                                        SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("MSVisible").CurrentFilterValue %>'
                                                                        Width="100px" >
                                                                        <Items>
                                                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                                                            <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                                                            <telerik:RadComboBoxItem Text="No" Value="0" />
                                                                        </Items>
                                                                    </telerik:RadComboBox>
                                                                    <telerik:RadScriptBlock ID="RadScriptBlock12" runat="server">
                                                                        <script type="text/javascript">
                                                                            function ImportedFilterSelectedIndexChanged(sender, args) {
                                                                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                                                var filterVal = args.get_item().get_value();
                                                                                if (filterVal == "") {
                                                                                    tableView.filter("MSVisible", filterVal, "NoFilter");
                                                                                }
                                                                                else if (filterVal == "1") {
                                                                                    tableView.filter("MSVisible", "1", "EqualTo");
                                                                                }
                                                                                else if (filterVal == "0") {
                                                                                    tableView.filter("MSVisible", "0", "EqualTo");
                                                                                }
                                                                            }
                                                                        </script>
                                                                    </telerik:RadScriptBlock>
                                                                </FilterTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                </ItemTemplate>

                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn SortExpression="remarks" HeaderText="Remarks" DataField="remarks" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtremarks" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </telerik:RadAjaxPanel>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div style="clear: both;"></div>
                        </div>
                    </li>

                    <li id="tblPayment" runat="server" style="display: none">
                        <div id="accrdPayment" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Payment History</div>
                        <div class="collapsible-body">
                            <div class="form-content-wrap">
                                <div class="form-content-pd">
                                    <div class="grid_container">
                                        <div class="form-section-row pmd-card">
                                            <div class="RadGrid RadGrid_Material">

                                                <telerik:RadCodeBlock ID="RadCodeBlock11" runat="server">
                                                    <script type="text/javascript">
                                                        function pageLoad() {
                                                            var grid = $find("<%= RadGrid_gvPayment.ClientID %>");
                                                            var columns = grid.get_masterTableView().get_columns();
                                                            for (var i = 0; i < columns.length; i++) {
                                                                columns[i].resizeToFit(false, true);
                                                            }
                                                        }

                                                        var requestInitiator = null;
                                                        var selectionStart = null;

                                                        function requestStart3(sender, args) {
                                                            requestInitiator = document.activeElement.id;
                                                            if (document.activeElement.tagName == "INPUT") {
                                                                selectionStart = document.activeElement.selectionStart;
                                                            }


                                                        }

                                                        function responseEnd3(sender, args) {
                                                            try {
                                                                var element = document.getElementById(requestInitiator);
                                                                if (element && element.tagName == "INPUT") {
                                                                    element.focus();
                                                                    element.selectionStart = selectionStart;
                                                                }

                                                            } catch (e) {

                                                            }
                                                        }
                                                    </script>

                                                </telerik:RadCodeBlock>

                                                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvPayment" ClientEvents-OnRequestStart="requestStart3" ClientEvents-OnResponseEnd="responseEnd3">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvPayment" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                                        ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvPayment_NeedDataSource" OnItemDataBound="RadGrid_gvPayment_ItemDataBound">
                                                        <CommandItemStyle />
                                                        <GroupingSettings CaseSensitive="false" />
                                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                            <Selecting AllowRowSelect="True"></Selecting>
                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                        </ClientSettings>
                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false">
                                                            <Columns>
                                                                <telerik:GridTemplateColumn DataField="ReceivedPaymentID" AutoPostBackOnFilter="true" DataType="System.String"
                                                                    CurrentFilterFunction="Contains" HeaderText="Ref" ShowFilterIcon="false" HeaderStyle-Width="200">
                                                                    <ItemTemplate>

                                                                        <asp:HyperLink ID="lnkRef" runat="server" NavigateUrl='<%# Eval("link")%>'><%# Eval("ReceivedPaymentID") %> </asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="PaymentDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                    CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false" HeaderStyle-Width="200">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPaymentdate" runat="server" onclick='<%# String.Format("javascript:return OpenApplyCreditModal(\"{0}\")", Eval("PaymentDate","{0:M/d/yyyy}").ToString()) %>' Text='<%# Eval("PaymentDate", "{0:M/d/yyyy}")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Type" AutoPostBackOnFilter="true" DataType="System.String"
                                                                    CurrentFilterFunction="Contains" HeaderText="Type" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn DataField="fDesc" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="Description" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblfDesc" runat="server" Text='<%# Eval("fDesc") %>'></asp:Label>
                                                                    </ItemTemplate>

                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn DataField="Amount" UniqueName="Amount" FooterAggregateFormatString="{0:c}" Aggregate="Sum" SortExpression="QuotedPrice" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" HeaderText="Amount" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAmount" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>' runat="server"
                                                                            ForeColor='<%# Convert.ToDouble(Eval("Amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' />

                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblTo" Style="margin-left: 15px;" runat="server"> Total:-</asp:Label>
                                                                    </FooterTemplate>
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

                    <li id="tbLogs" runat="server" style="display: none">
                        <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Logs</div>
                        <div class="collapsible-body">
                            <div class="form-content-wrap">
                                <div class="form-content-pd">
                                    <div class="grid_container">
                                        <div class="form-section-row pmd-card">
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

                                                        function requestStart5(sender, args) {
                                                            requestInitiator = document.activeElement.id;
                                                            if (document.activeElement.tagName == "INPUT") {
                                                                selectionStart = document.activeElement.selectionStart;
                                                            }
                                                        }

                                                        function responseEnd5(sender, args) {
                                                            var element = document.getElementById(requestInitiator);
                                                            if (element && element.tagName == "INPUT") {
                                                                element.focus();
                                                                element.selectionStart = selectionStart;
                                                            }
                                                        }
                                                    </script>
                                                </telerik:RadCodeBlock>
                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_gvLogs" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvLogs" ClientEvents-OnRequestStart="requestStart5" ClientEvents-OnResponseEnd="responseEnd5">
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
                <%--</div>
            </div>--%>
            </div>
        </div>
    </div>

    <telerik:RadWindowManager ID="RadWindowManagerInv" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowWarehouse" Skin="Material" VisibleTitlebar="true" Title="Select Op Squence" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="300" Height="200" OnClientClose="OnClientCloseHandler">
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

        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadWindow ID="RadWindowPO" Skin="Material" VisibleTitlebar="true" Title="Select Bill with PO option" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450" Behaviors="Maximize,Minimize"
        runat="server" Modal="true" Width="250" Height="200" OnClientClose="OnClientClosePOHandler">
        <ContentTemplate>
            <div class="msg-cont-addbil">
                <%--<span>Would you like to create the bill by ? </span>--%>
                <span>Would you like to create the bill by quantity or amount? </span>
            </div>
            <br />
            <div class="row rdbycss" >
                <div class="rd-flt pd-25">
                    <input id="rdbyQty" runat="server" class="radio-custom" name="radio-group-multiwage" type="radio" value="1">
                    <asp:Label runat="server" ID="lblrdbyQty" CssClass="radio-custom-label" AssociatedControlID="rdbyQty">Quantity</asp:Label>
                </div>
                <div class="rd-flt pd-25">
                    <input id="rdbyAmt" runat="server" class="radio-custom" name="radio-group-multiwage" type="radio" value="0" checked>
                    <asp:Label runat="server" ID="lblrdbyAmt" CssClass="radio-custom-label" AssociatedControlID="rdbyAmt">$ Amount</asp:Label>
                </div>
            </div>
            <div style="clear: both;"></div>

            <div class="btnlinks pl-10" style="padding-left: 10px;">
                &nbsp;&nbsp; &nbsp;&nbsp;
                         <a href="javascript:void(0);" class="ClosePOW" onclick="ClosePOWindow();" >Ok</a>
                <%--<asp:Button ID="btnSelectPo1" runat="server" CausesValidation="False" OnClientClick="ClosePOWindow();" 
                                             Text="Ok" style="width: 80px; float: left; height: 30px;"/>--%>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
    <asp:HiddenField runat="server" ID="hdOpSeqID" />
    <asp:HiddenField runat="server" ID="hdLineNo" />
    <asp:HiddenField runat="server" ID="hdnSelectPOIndex" />
    <asp:HiddenField ID="hdnIsAutoCompleteSelected" ClientIDMode="Static" runat="server" />

    <div class="col-lg-12 col-md-12" style="display: none">
        <div class="com-cont">
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <asp:HiddenField ID="hdnJobId" runat="server" />
                    <div class="form-col">
                        <div class="fc-label">
                            <label>Op Sequence</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtOpSeq" runat="server" CssClass="form-control" TabIndex="2" placeholder="Select Op Sequence"
                                MaxLength="50" onkeyup="EmptyValue(this);"></asp:TextBox>
                            <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtOpSeq"
                                EnableCaching="False" ServiceMethod="GetCodeAPBILL" UseContextKey="True" MinimumPrefixLength="0"
                                CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListItemCssClass="autocomplete_listItem"
                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListElementID="lstAcct"
                                ID="AutoCompleteExtender2" DelimiterCharacters="" CompletionInterval="250">
                            </asp:AutoCompleteExtender>
                            <div id="lstAcct"></div>
                            <asp:RequiredFieldValidator ID="rfvOpSeq" ValidationGroup="item"
                                runat="server" ControlToValidate="txtOpSeq" Display="None" ErrorMessage="Please enter Op Sequence"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vceOpSeq" runat="server" Enabled="True"
                                PopupPosition="Right" TargetControlID="rfvOpSeq" />

                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>Type</label>
                        </div>
                        <div class="fc-input">
                            <asp:DropDownList ID="ddlBomType" runat="server" DataTextField="Type" CssClass="form-control" onchange="SetContextKey()">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvType" ValidationGroup="item"
                                runat="server" ControlToValidate="ddlBomType" Display="None" ErrorMessage="Please select type"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vceType" runat="server" Enabled="True"
                                PopupPosition="Right" TargetControlID="rfvType" />
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>Item</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtItem" runat="server" CssClass="form-control" TabIndex="2" placeholder="Select Item" onkeyup="SetContextKey()"
                                MaxLength="50"></asp:TextBox>
                            <asp:HiddenField ID="hdnItemID" runat="server" />
                            <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtItem"
                                EnableCaching="False" ServiceMethod="GetItemsAPBILL" UseContextKey="True" MinimumPrefixLength="0"
                                CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListItemCssClass="autocomplete_listItem"
                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListElementID="lstItem"
                                OnClientItemSelected="aceItem_itemSelected"
                                ID="AutoCompleteExtender3" DelimiterCharacters="" CompletionInterval="250">
                            </asp:AutoCompleteExtender>
                            <div id="lstItem"></div>
                            <asp:RequiredFieldValidator ID="rfvItem" ValidationGroup="item"
                                runat="server" ControlToValidate="txtItem" Display="None" ErrorMessage="Please select item"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vceItem" runat="server" Enabled="True"
                                PopupPosition="Right" TargetControlID="rfvItem" />
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>Description</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtJobDesc" runat="server" CssClass="form-control" TabIndex="2"
                                MaxLength="255"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvJobDesc" ValidationGroup="item"
                                runat="server" ControlToValidate="txtJobDesc" Display="None" ErrorMessage="Please enter description"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vceJobDesc" runat="server" Enabled="True"
                                PopupPosition="Right" TargetControlID="rfvJobDesc" />
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>Qty Required</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" TabIndex="2"
                                MaxLength="50" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvQty" ValidationGroup="item"
                                runat="server" ControlToValidate="txtQty" Display="None" ErrorMessage="Please enter quantity required"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vceQty" runat="server" Enabled="True"
                                PopupPosition="Right" TargetControlID="rfvQty" />
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>U/M</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtUM" runat="server" CssClass="form-control" TabIndex="2"
                                MaxLength="150" placeholder="Select U/M"></asp:TextBox>
                            <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="CustomerAuto.asmx" TargetControlID="txtUM"
                                EnableCaching="False" ServiceMethod="GetUMAPBILL" UseContextKey="False" MinimumPrefixLength="0"
                                CompletionListCssClass="autocomplete_completionListElement"
                                CompletionListItemCssClass="autocomplete_listItem"
                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListElementID="lstUM"
                                ID="AutoCompleteExtender1" DelimiterCharacters="" CompletionInterval="250">
                            </asp:AutoCompleteExtender>
                            <div id="lstUM"></div>
                            <asp:RequiredFieldValidator ID="rfvUM" ValidationGroup="item"
                                runat="server" ControlToValidate="txtUM" Display="None" ErrorMessage="Please select U/M"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vceUM" runat="server" Enabled="True"
                                PopupPosition="Right" TargetControlID="rfvUM" />
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>Budget Unit</label>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtBudgetUnit" runat="server" CssClass="form-control" TabIndex="2"
                                MaxLength="50" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvBudgetUnit" ValidationGroup="item"
                                runat="server" ControlToValidate="txtBudgetUnit" Display="None" ErrorMessage="Please enter Budget Unit"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vceBudgetUnit" runat="server" Enabled="True"
                                PopupPosition="Right" TargetControlID="rfvBudgetUnit" />
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            <label>Budget Ext</label>
                        </div>
                        <div class="fc-input pt-5" style="padding-top: 5px;">
                            <asp:Label ID="lblBudgetExt" runat="server" Text="0.00"></asp:Label>
                        </div>
                    </div>
                </div>


            </div>
        </div>
    </div>

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script defer src="https://use.fontawesome.com/releases/v5.0.10/js/all.js"></script>
    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
    <script type="text/javascript">
        ///-Document permission

        function AddDocumentClick(hyperlink) {

            var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
            if (IsAdd == "Y") {
                ConfirmUpload(ctl00_ContentPlaceHolder1_FileUpload1.value);
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }
        }

        function DeleteDocumentClick(hyperlink) {
            var IsDelete = document.getElementById('<%= hdnDeleteDocument.ClientID%>').value;
            if (IsDelete == "Y") {
                return checkdelete();
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }


        function ViewDocumentClick(hyperlink) {
            var IsView = document.getElementById('<%= hdnViewDocument.ClientID%>').value;
            if (IsView == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }


        function checkdelete() {
            return SelectedRowDelete('<%= RadGrid_Documents.ClientID %>', 'file');
        }

        function SelectedRowDelete(gridview, message) {
            var grid = $find(gridview);
            var MasterTable = grid.get_masterTableView();
            var Rows = null;
            if (MasterTable != null) {
                Rows = MasterTable.get_dataItems();
            }
            if (Rows != null && Rows.length > 0) {
                for (i = 0; i < Rows.length; i++) {
                    if (Rows[i].get_selected()) {
                        //return confirm('Are you sure you want to delete ' + message + ' "' + Rows[i].get_columns(1) + '" ?');
                        return true;
                    }
                }
            }
            alert('Please select ' + message + '.');
            return false;
        }

        ////////////////// Confirm Document Upload ////////////////////
<%--        function ConfirmUpload(value) {
            //debugger
            var filename;
            var fullPath = value;
            if (fullPath) {
                var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
                filename = fullPath.substring(startIndex);
                if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
                    filename = filename.substring(1);
                }
            }

            if (confirm('Upload ' + filename + '?')) { document.getElementById('<%= lnkUploadDoc.ClientID %>').click(); }
            else { document.getElementById('<%= lnkPostback.ClientID %>').click(); }
        }--%>
        function ConfirmUpload(value) {
            if (confirm('Are you sure you want to upload?')) { document.getElementById('<%= lnkUploadDoc.ClientID %>').click(); }
             else { document.getElementById('<%= lnkPostback.ClientID %>').click(); }
         }

        $(document).ready(function () {
            //Dropify Basic
            $('.dropify').dropify();
            // Used events
            var drEvent = $('.dropify-event').dropify();

            drEvent.on('dropify.beforeClear', function (event, element) {
                return confirm("Do you really want to delete \"" + element.filename + "\" ?");
            });

            drEvent.on('dropify.afterClear', function (event, element) {
                alert('File deleted');
            });

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
                    'scrollTop': $target.offset().top - 160
                }, 900, 'swing');
            });

            Materialize.updateTextFields();

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
                        var IsSalesTaxAPBill = ui[0].IsSalesTaxAPBill;
                        var IsUseTaxAPBill = ui[0].IsUseTaxAPBill;
                        if (IsSalesTaxAPBill == "1") {


                            document.getElementById('txtqstgv').style.display = 'block';
                            //document.getElementById('txtgstgv').style.display = 'block';
                        } else {



                            document.getElementById('txtqstgv').style.display = 'none';
                            //document.getElementById('txtgstgv').style.display = 'none';
                        }

                        if (IsUseTaxAPBill == "1") {
                            document.getElementById('txttaxcodegv').style.display = 'none';

                        } else {

                            document.getElementById('txttaxcodegv').style.display = 'none';

                        }
                        if (IsUseTaxAPBill == "1") {
                            document.getElementById('txtusetaxcodegv').style.display = 'block';

                        } else {

                            document.getElementById('txtusetaxcodegv').style.display = 'none';

                        }

                        //$(txtGvAcctName).val(ui[0].DefaultAcct);
                    }


                },
                error: function (result) {
                    alert("Due to unexpected errors we were unable to load phase details");
                }
            });


        });


        function SetOpToHiddenPop() {
            var selectedLineNo = $("#<%=hdLineNo.ClientID%>").val();
            var lineItem = $("#lineItem999").val();
            $("#" + selectedLineNo).val(lineItem);

            var radwindow = $find('<%=RadWindowWarehouse.ClientID %>');
            radwindow.close();
        }

        function OnClientCloseHandler(sender, args) {
            var selectedLineNo = $("#<%=hdLineNo.ClientID%>").val();
            var lineItem = $("#lineItem999").val();
            $("#" + selectedLineNo).val(lineItem);
        }
        function OnClientClosePOHandler(sender, args) {
            <%--var radwindow = $find('<%=RadWindowPO.ClientID %>');
            radwindow.close();--%>
        }
        function ClosePOWindow() {
            document.getElementById('<%=btnSelectPo.ClientID%>').click();
            var radwindow = $find('<%=RadWindowPO.ClientID %>');
            radwindow.close();

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
    </script>
    <script>
        function pageLoad(sender, args) {
            function dtaa() {
                this.prefixText = null;
                this.vendor = null;
                this.con = null;
            }
            function makeReadonly(txt) {
                $("#" + txt.id).prop('readonly', true);
            }

            // Select row index for F6 function
            <%--$("[id*=txtGvJob]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });

            $("[id*=txtGvTicket]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvTicket', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });

            $("[id*=txtGvPhase]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvPhase', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });

            $("[id*=txtGvItem]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvItem', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });

            $("[id*=txtGvDesc]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvDesc', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });

            $("[id*=txtGvAcctNo]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvAcctNo', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });

            $("[id*=txtGvQuan]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvQuan', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });

            $("[id*=txtGvPrice]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvPrice', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });

            $("[id*=txtGvAmount]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvAmount', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });

            $("[id*=txtGvUseTax]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvUseTax', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });

            $("[id*=txtGvLoc]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvLoc', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });--%>

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

            // Focus out and autocomplete
            $("#<%=txtQty.ClientID%>").change(function () {
                var budgetunit = $("#<%=txtBudgetUnit.ClientID%>").val();
                var qty = $(this).val();
                if (budgetunit != "" && qty != "") {
                    var budgetext = parseFloat(qty) * parseFloat(budgetunit);
                    $("#<%=lblBudgetExt.ClientID%>").text(budgetext.toFixed(2));
                }
                if (budgetunit != "") {
                    $("#<%=txtBudgetUnit.ClientID%>").val(parseFloat(budgetunit).toFixed(2));
                }
                if (qty != "") {
                    $("#<%=txtQty.ClientID%>").val(parseFloat(qty).toFixed(2));
                }
            });
            $("#<%=txtBudgetUnit.ClientID%>").change(function () {
                var budgetunit = $(this).val();
                var qty = $("#<%=txtQty.ClientID%>").val();
                if (budgetunit != "" && qty != "") {
                    var budgetext = parseFloat(qty) * parseFloat(budgetunit);
                    $("#<%=lblBudgetExt.ClientID%>").text(budgetext.toFixed(2));
                }
                if (budgetunit != "") {
                    $("#<%=txtBudgetUnit.ClientID%>").val(parseFloat(budgetunit).toFixed(2));
                }
                if (qty != "") {
                    $("#<%=txtQty.ClientID%>").val(parseFloat(qty).toFixed(2));
                }
            });
            $("#<%=txtVendor.ClientID%>").keyup(function (event) {

                var hdnVendorID = document.getElementById('<%=hdnVendorID.ClientID%>');
                if (document.getElementById('<%=txtVendor.ClientID%>').value == '') {
                    hdnVendorID.value = '';
                }
            });

            /////////////// $$$ GET PO $$$ /////////////////

            $("#<%=txtPO.ClientID%>").autocomplete({

                source: function (request, response) {
                    var dta = new dtaa();
                    dta.prefixText = request.term;
                    if ($("#<%=hdnVendorID.ClientID%>").val() != '') {

                        dta.vendor = $("#<%=hdnVendorID.ClientID%>").val();
                    }
                    query = request.term;

                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetPO",
                        data: JSON.stringify(dta),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                            var counting = JSON.parse(data.d).length;
                            if (counting == 1) {
                                x = $.parseJSON(data.d)[0].VendorName;
                                if (x == "No Record Found!") {
                                    //$(this).close();

                                }
                            }

                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load po");
                        }
                    });
                },
                select: function (event, ui) {
                    var str = ui.item.VendorName;
                    if (str == "No Record Found!") {
                        $(this).val("");

                    }
                    else {
                        $(this).val(ui.item.PO);
                        $("#<%= txtVendor.ClientID %>").val(ui.item.VendorName);
                        $("#<%= txtVendorType.ClientID %>").val(ui.item.VendorType);
                        $("#<%= hdnVendorID.ClientID %>").val(ui.item.Vendor);
                        $("#<%= hdnTotalAmount.ClientID %>").val(ui.item.Amount);
                        $("#<%= hdnReceivedAmount.ClientID %>").val(ui.item.ReceivedAmount);
                        $("#<%= hdnPO.ClientID %>").val(ui.item.PO);

                        Materialize.updateTextFields();
                        if (ui.item.Status != "5") {
                            if (ui.item.POReceiveBy == "9") {
                                var radwindow = $find('<%=RadWindowPO.ClientID %>');
                                radwindow.show();
                            }
                            else if (ui.item.POReceiveBy == "1") {
                                //$('input[name=rdbyQty]').attr("disabled", true);
                                $('input[name=rdbyQty]').prop("checked", true);
                                $('input[name=rdbyAmt]').prop("checked", false);
                                document.getElementById('<%=rdbyQty.ClientID%>').checked = true;
                                document.getElementById('<%=rdbyAmt.ClientID%>').checked = false;
                                $("#<%= rdbyAmt.ClientID %>").attr("disabled", true);
                                var radwindow = $find('<%=RadWindowPO.ClientID %>');
                                radwindow.show();
                            }
                            else if (ui.item.POReceiveBy == "0") {

                                //$('input[name=rdbyAmt]').attr("disabled", true);
                                $('input[name=rdbyQty]').prop("checked", false);
                                $('input[name=rdbyAmt]').prop("checked", true);
                                $("#<%= rdbyQty.ClientID %>").attr("disabled", true);
                                document.getElementById('<%=rdbyQty.ClientID%>').checked = false;
                                document.getElementById('<%=rdbyAmt.ClientID%>').checked = true;
                                var radwindow = $find('<%=RadWindowPO.ClientID %>');
                                radwindow.show();
                            }
                        }

                        <%--document.getElementById('<%=btnSelectPo.ClientID%>').click();--%>
                        $("#<%=txtReceptionId.ClientID%>").focus();
                        ////START/////ES-7054 AP Bill with PO and RPO - validation needed to force user to select an RPO/////
                        if (ui.item.Status == "5") {                                                        
                            DisableGrid();  
                            disableControl(<%=btnAddNewLines.ClientID%>);                           
                                                      
                        }
                        ////END/////ES-7054 AP Bill with PO and RPO - validation needed to force user to select an RPO/////
                        checkPORPO();



                    }

                    Materialize.updateTextFields();
                    //alert("sdsdsd");
                    return false;
                },
                focus: function (event, ui) {

                    $(this).val(ui.item.PO);

                    return false;
                },
                close: function () {
                    if ($("#<%= hdnVendorID.ClientID %>").val() == "") {
                        noty({
                            text: 'PO doesn\'t exist!',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: false,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                    }
                    //$(this).val("");

                    this.blur();

                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".posearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {

                    var ula = ul;
                    var itema = item;
                    var result_value = item.PO;
                    var result_item = item.PO;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            

                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);

                };
            });


            $("[id*=txtReceptionId]").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    dtaaa.PO = $("#<%=txtPO.ClientID%>").val();
                    if ($("#<%=hdnVendorID.ClientID%>").val() != '') {
                        dtaaa.vendor = $("#<%=hdnVendorID.ClientID%>").val();
                    }
                    query = request.term;

                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetReceivePOListSearch",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                            var counting = JSON.parse(data.d).length;
                            if (counting == 1) {
                                x = $.parseJSON(data.d)[0].VendorName;
                                if (x == "No Record Found!") {
                                    $(this).close();
                                }
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load receive po");
                        }
                    });
                },
                select: function (event, ui) {
                    var str = ui.item.Value;
                    if (str == "No Record Found!") {
                        $(this).val("");
                    }
                    else {
                        $(this).val(ui.item.Value);
                        $("#<%= txtVendorType.ClientID %>").val(ui.item.VendorType);
                        document.getElementById('<%=btnSelectRPo.ClientID%>').click();
                        enableControl(<%=btnAddNewLines.ClientID%>);    
                        // Thomas: moved this function into btnSelectRPo_Click
                        //checkPORPO();
                    }
                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.Value);
                    return false;
                },
                close: function () {
                    //debugger
                    //if ($(this).val() == "") {
                    //    noty({
                    //        text: 'PO Reception doesn\'t exist!',
                    //        type: 'warning',
                    //        layout: 'topCenter',
                    //        closeOnSelfClick: false,
                    //        timeout: false,
                    //        theme: 'noty_theme_default',
                    //        closable: true
                    //    });
                    //}
                    //$(this).val("");
                    //this.blur();
                },
                minLength: 0,
                delay: 250
            }).bind('focus', function () { $(this).autocomplete("search"); });
                //.click(function () {
                //$(this).autocomplete('search', $(this).val())
            //})
                
            $.each($(".rposearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.Value;
                    var result_ReceiveDate = item.ReceiveDate;
                    var result_RefNo = item.Ref;
                    var result_desc = "";
                    if (item.Ref != null) {
                        result_desc = "<a>" + result_item + ", <span style='color:Gray;'>" + result_RefNo + ", " + result_ReceiveDate + "</span></a>";
                    } else {
                        result_desc = "<a>" + result_item + "</a>";
                    }
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        // .append("<a>" + result_item + "</a>")
                        .append(result_desc)
                        .appendTo(ul);
                };
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



            $("[id*=txtTotalUseTax]").autocomplete({
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

                    if (ui.item.value == 0) {
                        $(this).val("");
                    }
                    else {
                        var txtGvUseTax = this.id;
                        $(this).val(ui.item.Rate);


                        $("#<%=husetaxGL.ClientID%>").val(ui.item.GL);
                        $("#<%=husetaxRate.ClientID%>").val(ui.item.Rate);
                        $("#<%=husetaxName.ClientID%>").val(ui.item.Name);

                        $("#<%=RadGrid_gvJobCostItems.ClientID %>").find('tr:not(:first, :last)').each(function () {
                            var $tr = $(this);
                            if ($tr.find('input[id*=txtGvUseTax]').attr('id') != "" && typeof $tr.find('input[id*=txtGvUseTax]').attr('id') != 'undefined') {
                                $tr.find('input[id*=txtGvUseTax]').val(parseFloat(ui.item.Rate).toFixed(4));
                                $tr.find('input[id*=hdnUtax]').val(ui.item.Name);
                                $tr.find('input[id*=hdnUtaxGL]').val(ui.item.GL);
                            }
                        });
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
            $.each($(".tsearchTotalUseTax"), function (index, item) {
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

            $("[id*=txtGvJob]").focusout(function () {

                
                var strJobsVal = $(this).val();
                //alert(strJobsVal);
                var txtGvJob = $(this).attr('id');
                var txtGvAcctNo = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvAcctNo'));
                var strAcctNo = $(txtGvAcctNo).val();

                var txtGvAcctNo1 = $(txtGvAcctNo).attr('id');
                var hdnAcctID = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'hdnAcctID'));
                var txtGvAcctName = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'txtGvAcctName'));

                var GvPhasenew = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'txtGvPhase')).value;

                var txtGvLoc = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvLoc'));
                var hdnJobID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnJobID'));
                var txtGvPhase = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvPhase'));
                var hdnTypeId = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnTypeId'));
                var hdnPID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnPID'));
                var txtGvItem = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvItem'));
                var hdnItemID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnItemID'));
                var txtGvDesc = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvDesc'));
                if (strJobsVal == "" && GvPhasenew != 'Inventory') {

                    if ($(hdnItemID).val() != "") {
                        $(txtGvDesc).val('');
                    }
                    $(txtGvLoc).val('');
                    $(hdnJobID).val('');
                    $(txtGvPhase).val('');
                    $(hdnTypeId).val('');
                    $(hdnPID).val('');
                    $(txtGvItem).val('');
                    $(hdnItemID).val('');
                   
                }



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
                    var GvPhase = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvPhase'));
                    var hdnTypeId = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnTypeId'));
                    //-----If Inventory code select then we set default inventory Acct
                    <%--var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                    var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                    if (GvPhase == 'Inventory') {
                        $(txtGvAcctNo).val(InvDefaultAcctName);
                        $(hdnAcctID).val(InvDefaultAcctID);
                    }
                    else {
                        $(hdnAcctID).val(ui.item.GLExp);
                        var strAcct = ui.item.Acct + ' - ' + ui.item.DefaultAcct;
                        $(txtGvAcctNo).val(strAcct);
                    }--%>
                    $(hdnAcctID).val(ui.item.GLExp);
                    var strAcct = ui.item.Acct + ' - ' + ui.item.DefaultAcct;
                    $(txtGvAcctNo).val(strAcct);
                    $(GvPhase).val('');
                    $(hdnTypeId).val('0');
                    $('#hdnIsAutoCompleteSelected').val('1');
                    return false;
                },
                focus: function (event, ui) {
                    try {
                        $(this).val(ui.item.fDesc);
                    } catch{ }

                    return false;
                },
                //change: function (event, ui) {
                //    var txtGvJob = this.id;
                //    var hdnJobID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnJobID'));
                //    var strJob = document.getElementById(txtGvJob).value;

                //    if (strJob == '') {
                //        $(hdnJobID).val('')
                //    }
                //},
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

            $("[id*=txtGvJob]").change(function () {
                //debugger
                var isAutoCompleteSelected = $('#hdnIsAutoCompleteSelected').val();
                $('#hdnIsAutoCompleteSelected').val('0');
                if (isAutoCompleteSelected != '1') {
                    //debugger
                    //var txtGvJob = ;
                    var strItem = $(this).val();
                    var txtGvJobId = $(this).attr('id');
                    var txtGvLoc = document.getElementById(txtGvJobId.replace('txtGvJob', 'txtGvLoc'));
                    var hdnJobID = document.getElementById(txtGvJobId.replace('txtGvJob', 'hdnJobID'));
                    var txtGvAcctNo = document.getElementById(txtGvJobId.replace('txtGvJob', 'txtGvAcctNo'));
                    var hdnAcctID = document.getElementById(txtGvJobId.replace('txtGvJob', 'hdnAcctID'));
                    var txtGvJob = document.getElementById(txtGvJobId);

                    if (strItem != "") {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetJobLocations",
                            data: '{"prefixText": "' + strItem + '", "IsJob": "' + true + '", "con": ""}',
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                var ui = $.parseJSON(data.d);
                                if (ui.length == 0) {
                                    $(txtGvJob).val('');
                                    $(hdnJobID).val('');
                                }
                                else {
                                    //debugger
                                    $(hdnJobID).val(ui[0].ID);
                                    var jobStr = ui[0].ID + ", " + ui[0].fDesc;
                                    $(txtGvJob).val(jobStr);
                                    $(txtGvLoc).val(ui[0].Tag);
                                    $(hdnAcctID).val(ui[0].GLExp);
                                    var strAcct = ui[0].Acct + ' - ' + ui[0].DefaultAcct;
                                    $(txtGvAcctNo).val(strAcct);
                                }
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load project details");
                            }
                        });
                    }
                    else {
                        $(txtGvJob).val('');
                        $(hdnJobID).val('');
                    }
                }
            });

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
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

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


            $("[id*=lblSalesTax]").autocomplete({
                open: function (e, ui) {
                    /* create the scrollbar each time autocomplete menu opens/updates */
                    $(".ui-autocomplete").mCustomScrollbar({
                        setHeight: 182,
                        theme: "dark-3",
                        autoExpandScrollbar: true
                    });
                },
                response: function (e, ui) {
                    /* destroy the scrollbar after each search completes, before the menu is shown */
                    $(".ui-autocomplete").mCustomScrollbar("destroy");
                },
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    debugger;
                    //dtaaa.prefixText = request.term;
                    //alert($("#<%=hdnSTaxState.ClientID%>").val());
                    var tytty = $("#<%=hdnSTaxState.ClientID%>").val();
                    dtaaa.prefixText = tytty;
                    query = tytty;

                    //var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/getSaleTaxSearch",
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
                        var lblSalesTax = this.id;
                        $(this).val(ui.item.Rate);



                        var hdnSTaName = document.getElementById(lblSalesTax.replace('lblSalesTax', 'hdnSTaName'));
                        var hdnSTaxGL = document.getElementById(lblSalesTax.replace('lblSalesTax', 'hdnSTaxGL'));

                        var hdnSTaxAm = document.getElementById(lblSalesTax.replace('lblSalesTax', 'hdnSTaxAm'));
                        var txtGvStaxAmount = document.getElementById(lblSalesTax.replace('lblSalesTax', 'txtGvStaxAmount'));
                        var chkTaxable = document.getElementById(lblSalesTax.replace('lblSalesTax', 'chkTaxable'));
                        var hdnchkTaxable = document.getElementById(lblSalesTax.replace('lblSalesTax', 'hdnchkTaxable'));
                        var hdnGSTTaxGL = document.getElementById(lblSalesTax.replace('lblSalesTax', 'hdnGSTTaxGL'));
                        var hdnGSTTaxAm = document.getElementById(lblSalesTax.replace('lblSalesTax', 'hdnGSTTaxAm'));
                        var lblAmountWithTax = document.getElementById(lblSalesTax.replace('lblSalesTax', 'lblAmountWithTax'));
                        var hdnAmountWithTax = document.getElementById(lblSalesTax.replace('lblSalesTax', 'hdnAmountWithTax'));
                        var lblGstTax = document.getElementById(lblSalesTax.replace('lblSalesTax', 'lblGstTax'));
                        var txtGvAmount = document.getElementById(lblSalesTax.replace('lblSalesTax', 'txtGvAmount'));
                        var txtGvPrice = document.getElementById(lblSalesTax.replace('lblSalesTax', 'txtGvPrice'));
                        var txtGvQuan = document.getElementById(lblSalesTax.replace('lblSalesTax', 'txtGvQuan'));


                        $(hdnSTaName).val(ui.item.Name);
                        $(hdnSTaxGL).val(ui.item.GL);

                        //var staxGL = document.getElementById('<%=hdnQSTGL.ClientID%>');
                //var gtaxGL = document.getElementById('<%=hdnGSTGL.ClientID%>');
                        var staxGL = ui.item.GL;
                        var gtaxGL = document.getElementById('<%=hdnGSTGL.ClientID%>');

                        var staxType = document.getElementById('<%=hdnSTaxType.ClientID%>');

                        var stax = ui.item.Rate;
                        var gtax = document.getElementById('<%=hdnGST.ClientID%>');





                        var valAmount;



                        var isGst = 0;
                        var totamt = 0;
                        var staxAmt = 0;
                        var gtaxAmt = 0;
                        var staxAmtGL = 0;
                        var gtaxAmtGL = 0;




                        //if (chkTaxable.checked == true) {
                        //    $(hdnchkTaxable).val('1');
                        //} else {
                        //    $(hdnchkTaxable).val('0');
                        //        }



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





                        if (isGst == 1) {
                            if (gtax == null) {
                                gtaxAmt = 0.00;
                                gtaxAmtGL = 0;
                                $(lblGstTax).val(gtaxAmt.toFixed(2));

                                $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                                $(hdnGSTTaxGL).val(gtaxAmtGL);
                            }
                            else if (gtax.value != '') {
                                //if (checkbox.checked == true) {
                                if ($(hdnchkTaxable).val() == "1") {
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

                            }

                        }

                        //if (chkTaxable.checked == true) {
                        if ($(hdnchkTaxable).val() == "1") {

                            if (parseInt(staxType.value) == 0 || parseInt(staxType.value) == 2) {
                                if (parseFloat(valAmount) < 0) {

                                    staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                                    staxAmt = staxAmt * (-1);
                                    staxAmtGL = parseInt(staxGL.value);

                                } else {
                                    staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                                    staxAmtGL = parseInt(staxGL.value);
                                }
                            }
                            else if (parseInt(staxType.value) == 1) {
                                var oldvalAmount = valAmount;
                                if (isGst == 1) {
                                    valAmount = parseFloat(valAmount) + gtaxAmt;
                                }
                                if (parseFloat(valAmount) < 0) {

                                    staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                                    staxAmt = staxAmt * (-1);
                                    staxAmtGL = parseInt(staxGL.value);
                                    valAmount = oldvalAmount;

                                } else {
                                    staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
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


                        //$(lblSalesTax).val(staxAmt.toFixed(2));
                        $(hdnSTaxAm).val(staxAmt.toFixed(2));
                        $(txtGvStaxAmount).val(staxAmt.toFixed(2));

                        //$(hdnSTaxGL).val(staxAmtGL);



                        totamt = valAmount + staxAmt;
                        if (isGst == 1) {
                            totamt = totamt + gtaxAmt;
                        }
                        $(lblAmountWithTax).text(totamt.toFixed(2));
                        //$(hdnAmountWithTax).val(staxAmt.toFixed(2));
                        $(hdnAmountWithTax).val(totamt.toFixed(2));


                        //CalculateTotalAmtSST();
                        CalculateTotalAmt();


                        <%--<asp:CheckBox ID="chkTaxable" runat="server" Checked='<%#Convert.ToBoolean(Eval("stax"))%>'></asp:CheckBox>
                        <asp:HiddenField ID="hdnchkTaxable" Value='<%# Bind("stax") %>' runat="server" />
                        <asp:HiddenField ID="hdnSTaxAm" Value='<%# Bind("STaxAmt") %>' runat="server" />
                        <asp:HiddenField ID="hdnSTaxGL" Value='<%# Bind("STaxGL") %>' runat="server" />
                        <asp:HiddenField ID="hdnSTaName" Value='<%# Bind("STaxName") %>' runat="server" />--%>


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
            $.each($(".ttsearchinput"), function (index, item) {
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



            //---$$$$$ Start Items Autocomplete $$$$$$$--

            $("[id*=txtGvItem]").change(function () {

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
                                //if (GvPhase == 'Materials' && job == '') {
                                //    $(hdnPID).val(1);
                                //}
                            }
                            else {
                                $(txtGvItem).val(ui[0].ItemDesc1);
                                $(hdnItemID).val(ui[0].ItemID);
                                $(hdnPID).val(ui[0].Line);
                                $(txtGvDesc).val(ui[0].fDesc);
                                //if (GvPhase == 'Materials' && job == '') {
                                //    $(hdnPID).val(1);
                                //}
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
                            //if (GvPhase == 'Materials' && job == '') {
                            //    $(hdnPID).val(1);                            
                            //}
                        }
                        else {
                            $(this).val("");
                            $(hdnPID).val(ui.item.Line);
                            $(txtGvDesc).val(ui.item.ItemDesc1);
                            //if (GvPhase == 'Materials' && job == '') {
                            //    $(hdnPID).val(1);
                            //}
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
                    var txtGvDesc = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvDesc'));
                    //var hdntxtGvPhase = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdntxtGvPhase'));
                    var str = ui.item.TypeName;
                    if (str == "No Record Found!") {
                        $(this).val("");
                    }
                    else {
                        try {
                            $(hdnTypeId).val(ui.item.Type);
                            $(this).val(ui.item.TypeName);
                            $(hdOpSq).val(ui.item.Code);
                            $(txtGvDesc).val(ui.item.Desc);
                        } catch{ }
                    }

                    var GvPhase = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvPhase')).value;
                    //-----If Inventory code select then we set default inventory Acct
                    var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                    var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;

                    if (GvPhase == 'Inventory') {
                        try {
                            var txtGvAcctNo = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvAcctNo'));
                            var hdnAcctID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnAcctID'));

                            txtGvWarehouse = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouse'));
                            txtGvWarehouseLocation = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouseLocation'));
                            txtGvWarehouse.readOnly = false;
                            txtGvWarehouseLocation.readOnly = false;

                            $(txtGvAcctNo).val(InvDefaultAcctName);
                            $(hdnAcctID).val(InvDefaultAcctID);
                        }
                        catch (e) { }
                    }
                    else {
                        try {
                            txtGvWarehouse = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouse'));
                            txtGvWarehouseLocation = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouseLocation'));
                            txtGvWarehouse.readOnly = true;
                            txtGvWarehouseLocation.readOnly = true;
                            $(txtGvWarehouse).val('');
                            $(txtGvWarehouseLocation).val('');
                            txtGvAcctNo = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvAcctNo'));
                            hdnAcctID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnAcctID'));
                            if (ui.item.AcctName != '' && ui.item.AcctID != '' && ui.item.AcctName != undefined && ui.item.AcctID != undefined) {
                                $(txtGvAcctNo).val(ui.item.AcctName);
                                $(hdnAcctID).val(ui.item.AcctID);
                            }

                        }
                        catch (e) { }
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
                    var result_Desc = item.Desc;
                    if (result_Code != null && result_Code != "")
                        return $("<li></li>")
                            .data("item.autocomplete", item)                            
                            .append("<span class='auto_item'><i style='display:inline-block; margin-right:8px;width:auto;color:#1565C0 !important;' class='fa fa-check-square' title=''></i><span style='color:Gray;'>" + result_GroupName + ",&nbsp; &nbsp;" + result_Code + ", &nbsp; &nbsp;" + result_CodeDesc + ",&nbsp; &nbsp;" + result_item + ", </span>&nbsp; &nbsp;<span style='color:Black;'><b>  " + result_Desc + " </b></span></span>")
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
                            else {
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
                                    } catch (e) { }
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
                                    } catch (e) { }
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
                                $(this).val('');

                                noty({
                                    text: 'Tax \'' + strPhase + '\' doesn\'t exist!',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: 5000,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });

                                return;
                            }
                            else {
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

            $("[id*=lblSalesTax]").change(function () {

                //var txtGvPhase = $(this);
                var strPhase = $(this).val();
                var lblSalesTax = this.id;

                var txtGvAmount = document.getElementById(lblSalesTax.replace('lblSalesTax', 'txtGvAmount'));
                var hdnSTaxAm = document.getElementById(lblSalesTax.replace('lblSalesTax', 'hdnSTaxAm'));
                var txtGvStaxAmount = document.getElementById(lblSalesTax.replace('lblSalesTax', 'txtGvStaxAmount'));

                var hdnSTaName = document.getElementById(lblSalesTax.replace('lblSalesTax', 'hdnSTaName'));
                var hdnSTaxGL = document.getElementById(lblSalesTax.replace('lblSalesTax', 'hdnSTaxGL'));
                var dtaaa = new dtaa();
                dtaaa.prefixText = strPhase;
                if (strPhase != "") {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        //url: "AccountAutoFill.asmx/GetAutoFillPhase",
                        //data: '{"prefixText": "' + strPhase + '"}',
                        url: "AccountAutoFill.asmx/getSaleTaxSearch",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            // debugger

                            var ui = $.parseJSON(data.d);

                            if (ui.length == 0) {

                                $(hdnSTaName).val('');
                                $(hdnSTaxGL).val('');
                                $(this).val('');

                                noty({
                                    text: 'Tax \'' + strPhase + '\' doesn\'t exist!',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: 5000,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });

                                return;
                            }
                            else {
                                $(this).val(ui[0].Rate);
                                $(hdnSTaName).val(ui[0].Name);
                                $(hdnSTaxGL).val(ui[0].GL);

                                //var rrGvAmount = parseFloat($(txtGvAmount).val());
                                //var rrate = parseFloat(ui[0].Rate);
                                //var rstaxamt = rrGvAmount * rrate / 100;
                                //$(hdnSTaxAm).val(rstaxamt);

                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Type");
                        }
                    });
                }
                else {
                    $(hdnSTaName).val('');
                    $(hdnSTaxGL).val('');
                    $(this).val('');
                }
            });

            //$("[id*=txtGvUseTax]").focusout(function () {
            //    $(this).change();
            //});


            //---$$$$$ END Items  autocomplete $$$$$$$--
            function dtInv() {
                this.prefixText = null;
                this.con = null;
                this.InvID = null;
            }
            $("[id*=txtGvWarehouse]").autocomplete({

                open: function (e, ui) {
                    /* create the scrollbar each time autocomplete menu opens/updates */
                    $(".ui-autocomplete").mCustomScrollbar({
                        setHeight: 182,
                        theme: "dark-3",
                        autoExpandScrollbar: true
                    });
                },
                response: function (e, ui) {
                    /* destroy the scrollbar after each search completes, before the menu is shown */
                    try {
                        $(".ui-autocomplete").mCustomScrollbar("destroy");
                    }
                    catch (e) { }
                },
                source: function (request, response) {
                    debugger
                    var dtaaa = new dtInv();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    var str = request.term;

                    var txtGvWarehouse_GetID = $(this.element).attr("id");
                    var hdnInvID = document.getElementById(txtGvWarehouse_GetID.replace('txtGvWarehouse', 'hdnItemID'));

                    var hdntxtGvPhase = document.getElementById(txtGvWarehouse_GetID.replace('txtGvWarehouse', 'txtGvPhase'));
                    if (hdntxtGvPhase.value != "Inventory") { return; }

                    console.log(hdntxtGvPhase.value);

                    var ID = $(hdnInvID).val();

                    dtaaa.InvID = ID;
                    dtaaa.isShowAll = "yes";
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetWarehouseName",
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
                    try {
                        var txtGvWarehouse = this.id;
                        var hdnWarehouse = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnWarehouse'));
                        var hdnWarehousefdesc = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnWarehousefdesc'));
                        var hdnInvID = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnItemID'));


                        var Str = ui.item.WarehouseID + ", " + ui.item.WarehouseName;

                        $(this).val(Str);

                        $(hdnWarehousefdesc).val(Str);
                        $(txtGvWarehouse).val(Str);
                        $(hdnWarehouse).val(ui.item.WarehouseID);

                        var locationID = 0;
                        var warehouseID = $(hdnWarehouse).val();
                        var invID = $(hdnInvID).val();

                    } catch (e) { }
                    return false;
                },
                focus: function (event, ui) {
                    try {
                        $(this).val(ui.item.WarehouseID);
                    } catch (e) { }
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            });

            $.each($(".Warehousesearchinput"), function (index, item) {
                if (item && typeof item == "object")
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var ula = ul;
                        var itema = item;
                        var result_value = item.ID;
                        var result_item = item.WarehouseName;
                        var result_desc = item.WarehouseID;
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
                                .append("<a style='color:blue;'>" + result_item + "</a>")
                                .appendTo(ul);
                        }
                    };
            });

            //txtGvWarehouseLocation
            $("[id*=txtGvWarehouseLocation]").autocomplete({


                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    var str = request.term;

                    var txtGvWarehouseLocation_GetID = $(this.element).attr("id");
                    var hdnWarehouse = document.getElementById(txtGvWarehouseLocation_GetID.replace('txtGvWarehouseLocation', 'hdnWarehouse'));
                    var ID = $(hdnWarehouse).val();

                    var hdntxtGvPhase = document.getElementById(txtGvWarehouseLocation_GetID.replace('txtGvWarehouseLocation', 'txtGvPhase'));
                    if (hdntxtGvPhase.value != "Inventory") { return; }

                    dtaaa.WarehouseID = ID;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetWarehouseLocation",
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
                    try {

                        var txtGvWarehouseLocation = this.id;
                        var hdnWarehouseLocationID = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnWarehouseLocationID'));
                        var hdnInvID = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnItemID'));
                        var hdnWarehouse = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnWarehouse'));
                        var hdnLocationfdesc = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnLocationfdesc'));
                        //var Str = ui.item.ID + ", " + ui.item.Name;
                        var Str = ui.item.Name;
                        $(this).val(Str);

                        $(hdnLocationfdesc).val(Str);
                        $(txtGvWarehouseLocation).val(Str);
                        $(hdnWarehouseLocationID).val(ui.item.ID);

                        var locationID = $(hdnWarehouseLocationID).val();
                        var warehouseID = $(hdnWarehouse).val();
                        var invID = $(hdnInvID).val();






                    } catch (e) { alert('error'); }



                    return false;
                },
                focus: function (event, ui) {
                    try {
                        $(this).val(ui.item.ID);
                    } catch (e) { }
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })


            $.each($(".WarehouseLocationsearchinput"), function (index, item) {
                if (item && typeof item == "object")
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var ula = ul;
                        var itema = item;
                        var result_value = item.ID;
                        var result_item = item.Name;

                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>';
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
                                .append("<a style='color:blue;'>" + result_item + "</a>")
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

                    var id = $(this.element).prop("id");
                    //var id2 = this.element[0].id;
                    //var id3 = $(this.element.get(0)).attr('id');

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetJobLocations",
                        data: '{"prefixText": "' + dtaaa.prefixText + '", "IsJob": "' + false + '", "con": "' + dtaaa.con + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                            var ui = $.parseJSON(data.d);
                            if (ui.length == 0) {
                                document.getElementById(id).value = '';
                            }
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
                    //$(txtGvJob).val(ui.item.fDesc);
                    var jobStr = ui.item.ID + ", " + ui.item.fDesc;
                    $(txtGvJob).val(jobStr);

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

            CalculateTotalAmt();
        }

        $(document).ready(function () {

            CalculateTotalAmt();
            CalculateTotalUseTaxExpense();

        });

        /////////////////// To calculate Total and to make Gridview Amount Value to 2 decimal ////////////NK
        function CalTotalValStax(checkbox) {
            debugger;
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

                valAmount = parseFloat($(txtGvAmount).val()) || 0;

                if (checkbox.checked == true) {
                    $(hdnchkTaxable).val('1');
                } else {
                    $(hdnchkTaxable).val('0');
                }

                isGst = 1;



                //if (!jQuery.trim($(txtGvQuan).val()) == '') {
                //    if (isNaN(parseFloat($(txtGvQuan).val()))) {
                //        $(txtGvQuan).val('0.00');
                //    }
                //}

                //if (!jQuery.trim($(txtGvPrice).val()) == '') {
                //    if (isNaN(parseFloat($(txtGvPrice).val()))) {
                //        $(txtGvPrice).val('');
                //    }
                //}

                //if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
                //    if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
                //        valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
                //        $(txtGvAmount).val(valAmount.toFixed(2));
                //    }
                //} else if (!jQuery.trim($(txtGvQuan).val()) == '' && $(txtGvAmount).val() != '' && jQuery.trim($(txtGvPrice).val()) == '') {
                //    if (!isNaN(parseFloat($(txtGvQuan).val())) && parseFloat($(txtGvQuan).val()) != 0 && !isNaN(parseFloat($(txtGvAmount).val()))) {
                //        var valPrice = parseFloat($(txtGvAmount).val()) / parseFloat($(txtGvQuan).val());
                //        $(txtGvPrice).val(valPrice.toFixed(2));
                //    }
                //}





                ////if (isGst == 1) {
                ////    if (gtax == null) {
                ////        gtaxAmt = 0.00;
                ////        gtaxAmtGL = 0;
                ////        $(lblGstTax).val(gtaxAmt.toFixed(2));

                ////        $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                ////        $(hdnGSTTaxGL).val(gtaxAmtGL);
                ////    }
                ////    else if (gtax.value != '') {
                ////        if (checkbox.checked == true) {
                ////            gtaxAmt = Math.round(((parseFloat(valAmount) * parseFloat(gtax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                ////            $(lblGstTax).val(gtaxAmt.toFixed(2));
                ////            $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                ////            gtaxAmtGL = parseInt(gtaxGL.value);
                ////            $(hdnGSTTaxGL).val(gtaxAmtGL.value);
                ////        }
                ////        else {
                ////            gtaxAmt = 0.00;
                ////            gtaxAmtGL = 0;
                ////        }
                ////        $(lblGstTax).val(gtaxAmt.toFixed(2));

                ////        $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                ////        $(hdnGSTTaxGL).val(gtaxAmtGL);

                ////    }

                ////}

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

            valAmount = parseFloat($(txtGvAmount).val()) || 0;

            if (checkbox.checked == true) {
                $(hdnchkGTaxable).val('1');
            } else {
                $(hdnchkGTaxable).val('0');
            }



            isGst = 1;



            //if (!jQuery.trim($(txtGvQuan).val()) == '') {
            //    if (isNaN(parseFloat($(txtGvQuan).val()))) {
            //        $(txtGvQuan).val('0.00');
            //    }
            //}

            //if (!jQuery.trim($(txtGvPrice).val()) == '') {
            //    if (isNaN(parseFloat($(txtGvPrice).val()))) {
            //        $(txtGvPrice).val('');
            //    }
            //}

            //if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
            //    if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
            //        valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
            //        $(txtGvAmount).val(valAmount.toFixed(2));

            //    }
            //} else if (!jQuery.trim($(txtGvQuan).val()) == '' && $(txtGvAmount).val() != '' && jQuery.trim($(txtGvPrice).val()) == '') {
            //    if (!isNaN(parseFloat($(txtGvQuan).val())) && parseFloat($(txtGvQuan).val()) != 0 && !isNaN(parseFloat($(txtGvAmount).val()))) {
            //        var valPrice = parseFloat($(txtGvAmount).val()) / parseFloat($(txtGvQuan).val());
            //        $(txtGvPrice).val(valPrice.toFixed(2));

            //    }
            //}




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
            var hdnchkGTaxable;
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
            hdnchkGTaxable = document.getElementById(checkbox.replace('chkGTaxable', 'hdnchkGTaxable'));
            hdnSTaxGL = document.getElementById(checkbox.replace('chkGTaxable', 'hdnSTaxGL'));
            hdnGSTTaxGL = document.getElementById(checkbox.replace('chkGTaxable', 'hdnGSTTaxGL'));
            hdnSTaxAm = document.getElementById(checkbox.replace('chkGTaxable', 'hdnSTaxAm'));
            txtGvStaxAmount = document.getElementById(checkbox.replace('chkGTaxable', 'txtGvStaxAmount'));
            hdnGSTTaxAm = document.getElementById(checkbox.replace('chkGTaxable', 'hdnGSTTaxAm'));
            hdnchkTaxable = document.getElementById(checkbox.replace('chkGTaxable', 'hdnchkTaxable'));

            var cb = document.getElementById(checkbox);

            staxAmt = parseFloat($(hdnSTaxAm).val());

            valAmount = parseFloat($(txtGvAmount).val()) || 0;

            if (cb.checked == true) {
                $(hdnchkGTaxable).val('1');
            } else {
                $(hdnchkGTaxable).val('0');
            }

            isGst = 1;


            //if (!jQuery.trim($(txtGvQuan).val()) == '') {
            //    if (isNaN(parseFloat($(txtGvQuan).val()))) {
            //        $(txtGvQuan).val('0.00');
            //    }
            //}

            //if (!jQuery.trim($(txtGvPrice).val()) == '') {
            //    if (isNaN(parseFloat($(txtGvPrice).val()))) {
            //        $(txtGvPrice).val('');
            //    }
            //}

            //if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
            //    if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
            //        valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
            //        $(txtGvAmount).val(valAmount.toFixed(2));

            //    }
            //} else if (!jQuery.trim($(txtGvQuan).val()) == '' && $(txtGvAmount).val() != '' && jQuery.trim($(txtGvPrice).val()) == '') {
            //    if (!isNaN(parseFloat($(txtGvQuan).val())) && parseFloat($(txtGvQuan).val()) != 0 && !isNaN(parseFloat($(txtGvAmount).val()))) {
            //        var valPrice = parseFloat($(txtGvAmount).val()) / parseFloat($(txtGvQuan).val());
            //        $(txtGvPrice).val(valPrice.toFixed(2));

            //    }
            //}


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


                    if ($(hdnchkTaxable).val() == '1') {
                        if (parseInt(staxType.value) == 1) {
                            var oldvalAmount = valAmount;
                            if (cb.checked == true) {
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
                }
                else {
                    staxAmt = 0;
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
                //$(hdnchkTaxable).val('0');
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

                valAmount = parseFloat($(txtGvAmount).val()) || 0;

                if (cb.checked == true) {
                    $(hdnchkTaxable).val('1');
                } else {
                    $(hdnchkTaxable).val('0');
                }

                isGst = 1;

                //if (!jQuery.trim($(txtGvQuan).val()) == '') {
                //    if (isNaN(parseFloat($(txtGvQuan).val()))) {
                //        $(txtGvQuan).val('0.00');
                //    }
                //}

                //if (!jQuery.trim($(txtGvPrice).val()) == '') {
                //    if (isNaN(parseFloat($(txtGvPrice).val()))) {
                //        $(txtGvPrice).val('');
                //    }
                //}

                //if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
                //    if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
                //        valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
                //        $(txtGvAmount).val(valAmount.toFixed(2));

                //    }
                //} else if (!jQuery.trim($(txtGvQuan).val()) == '' && $(txtGvAmount).val() != '' && jQuery.trim($(txtGvPrice).val()) == '') {
                //    if (!isNaN(parseFloat($(txtGvQuan).val())) && parseFloat($(txtGvQuan).val()) != 0 && !isNaN(parseFloat($(txtGvAmount).val()))) {
                //        var valPrice = parseFloat($(txtGvAmount).val()) / parseFloat($(txtGvQuan).val());
                //        $(txtGvPrice).val(valPrice.toFixed(2));

                //    }
                //}

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

        function TotalwithGTax(lblGstTax) {

            var cb = lblGstTax.id;
            var stax = document.getElementById('<%=hdnQST.ClientID%>');
            var gtax = document.getElementById('<%=hdnGST.ClientID%>');
            var lblGstTax = document.getElementById(cb.replace('lblGstTax', 'lblGstTax'));
            var cbs = document.getElementById(cb.replace('lblGstTax', 'chkTaxable'));
            //var ch_id = chk.attr('id');
            //var cbs = document.getElementById(ch_id);

            var cbg = document.getElementById(cb.replace('lblGstTax', 'chkGTaxable'));
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
                $(lblGstTax).val(0);
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

                txtGvPrice = document.getElementById(cb.replace('lblGstTax', 'txtGvPrice'));
                txtGvQuan = document.getElementById(cb.replace('lblGstTax', 'txtGvQuan'));
                txtGvAmount = document.getElementById(cb.replace('lblGstTax', 'txtGvAmount'));
                //lblSalesTax = document.getElementById(cb.replace('chkTaxable', 'lblSalesTax'));
                lblGstTax = document.getElementById(cb.replace('lblGstTax', 'lblGstTax'));
                lblAmountWithTax = document.getElementById(cb.replace('lblGstTax', 'lblAmountWithTax'));
                hdnAmountWithTax = document.getElementById(cb.replace('lblGstTax', 'hdnAmountWithTax'));
                hdnchkTaxable = document.getElementById(cb.replace('lblGstTax', 'hdnchkTaxable'));
                hdnSTaxGL = document.getElementById(cb.replace('lblGstTax', 'hdnSTaxGL'));
                hdnGSTTaxGL = document.getElementById(cb.replace('lblGstTax', 'hdnGSTTaxGL'));
                hdnSTaxAm = document.getElementById(cb.replace('lblGstTax', 'hdnSTaxAm'));
                txtGvStaxAmount = document.getElementById(cb.replace('lblGstTax', 'txtGvStaxAmount'));
                hdnGSTTaxAm = document.getElementById(cb.replace('lblGstTax', 'hdnGSTTaxAm'));

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

                //gtaxAmt = parseFloat($(hdnGSTTaxAm).val());
                gtaxAmt = parseFloat($(lblGstTax).val());
                gtaxAmt = parseFloat(gtaxAmt) || 0;


                //gtaxAmtGL = parseInt(gtaxGL.value);


                if (gtax == null) {

                    gtaxAmt = 0.00;
                    gtaxAmtGL = 0;
                    $(lblGstTax).val(gtaxAmt.toFixed(2));

                    $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                    $(hdnGSTTaxGL).val(gtaxAmtGL);
                }
                else if (gtax.value != '') {
                    if (cbg.checked == true) {
                        gtaxAmtGL = parseInt(gtaxGL.value);
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

        function TotalwithTax(txtGvStaxAmount) {

            var cb = txtGvStaxAmount.id;
            var stax = document.getElementById('<%=hdnQST.ClientID%>');
            var gtax = document.getElementById('<%=hdnGST.ClientID%>');
            var txtGvStaxAmounts = document.getElementById(cb.replace('txtGvStaxAmount', 'txtGvStaxAmount'));
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
                $(txtGvStaxAmounts).val(0);
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
                gtaxAmt = parseFloat(gtaxAmt) || 0;


                //gtaxAmtGL = parseInt(gtaxGL.value);


                if (gtax == null) {

                    gtaxAmt = 0.00;
                    gtaxAmtGL = 0;
                    $(lblGstTax).val(gtaxAmt.toFixed(2));

                    $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                    $(hdnGSTTaxGL).val(gtaxAmtGL);
                }
                else if (gtax.value != '') {
                    if (cbg.checked == true) {
                        gtaxAmtGL = parseInt(gtaxGL.value);
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


        function TotalwithTax1(txtGvStaxAmount) {

            //          if(checkbox.checked == true){
            //     alert('checked');
            // }else{
            //     alert('unchecked');
            //}


            var cb = txtGvStaxAmount.id;
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
                //checkbox.checked = false;

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
                //txtGvStaxAmount = document.getElementById(cb.replace('chkTaxable', 'txtGvStaxAmount'));
                hdnGSTTaxAm = document.getElementById(cb.replace('chkTaxable', 'hdnGSTTaxAm'));


                //if (checkbox.checked == true) {
                //    $(hdnchkTaxable).val('1');
                //} else {
                //    $(hdnchkTaxable).val('0');
                //}


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





                if (isGst == 1) {
                    if (gtax == null) {
                        gtaxAmt = 0.00;
                        gtaxAmtGL = 0;
                        $(lblGstTax).val(gtaxAmt.toFixed(2));

                        $(hdnGSTTaxAm).val(gtaxAmt.toFixed(2));
                        $(hdnGSTTaxGL).val(gtaxAmtGL);
                    }
                    else if (gtax.value != '') {
                        if ($(hdnchkTaxable).val() == '1') {
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

                    }

                }

                if ($(hdnchkTaxable).val() == '1') {
                    if (parseInt(staxType.value) == 0 || parseInt(staxType.value) == 2) {
                        if (parseFloat(valAmount) < 0) {

                            //staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmt = parseFloat(txtGvStaxAmount.val());
                            staxAmt = staxAmt * (-1);
                            staxAmtGL = parseInt(staxGL.value);

                        } else {
                            //staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmt = parseFloat(txtGvStaxAmount.val());
                            staxAmtGL = parseInt(staxGL.value);
                        }
                    }
                    else if (parseInt(staxType.value) == 1) {
                        var oldvalAmount = valAmount;
                        if (isGst == 1) {
                            valAmount = parseFloat(valAmount) + gtaxAmt;
                        }
                        if (parseFloat(valAmount) < 0) {

                            //staxAmt = Math.round(((parseFloat(valAmount * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmt = parseFloat(txtGvStaxAmount.val());
                            staxAmt = staxAmt * (-1);
                            staxAmtGL = parseInt(staxGL.value);
                            valAmount = oldvalAmount;

                        } else {
                            //staxAmt = Math.round(((parseFloat(valAmount) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            staxAmt = parseFloat(txtGvStaxAmount.val());
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


        function CalTotalVal(obj) {
            debugger;
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
            var txtGvStaxAmount;
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
                hdnchkGTaxable = document.getElementById(txt.replace('txtGvQuan', 'hdnchkGTaxable'));
                chkGTaxable = document.getElementById(txt.replace('txtGvQuan', 'chkGTaxable'));

                //lblSalesTax = document.getElementById(txt.replace('txtGvQuan', 'lblSalesTax'));
                hdnSTaxAm = document.getElementById(txt.replace('txtGvQuan', 'hdnSTaxAm'));
                txtGvStaxAmount = document.getElementById(txt.replace('txtGvQuan', 'txtGvStaxAmount'));
                lblGstTax = document.getElementById(txt.replace('txtGvQuan', 'lblGstTax'));
                hdnGSTTaxAm = document.getElementById(txt.replace('txtGvQuan', 'hdnGSTTaxAm'));
                hdnIsPO = document.getElementById(txt.replace('txtGvQuan', 'hdnIsPO'));
                hdnOutstandQuan = document.getElementById(txt.replace('txtGvQuan', 'hdnOutstandQuan'));
                hdnOutstandBalance = document.getElementById(txt.replace('txtGvQuan', 'hdnOutstandBalance'));
            }
            else if (txt.indexOf("Price") >= 0) {
                txtGvPrice = document.getElementById(txt);
                txtGvQuan = document.getElementById(txt.replace('txtGvPrice', 'txtGvQuan'));
                txtGvAmount = document.getElementById(txt.replace('txtGvPrice', 'txtGvAmount'));
                lblAmountWithTax = document.getElementById(txt.replace('txtGvPrice', 'lblAmountWithTax'));
                hdnAmountWithTax = document.getElementById(txt.replace('txtGvPrice', 'hdnAmountWithTax'));
                hdnchkTaxable = document.getElementById(txt.replace('txtGvPrice', 'hdnchkTaxable'));
                chkTaxable = document.getElementById(txt.replace('txtGvPrice', 'chkTaxable'));
                hdnchkGTaxable = document.getElementById(txt.replace('txtGvPrice', 'hdnchkGTaxable'));
                chkGTaxable = document.getElementById(txt.replace('txtGvPrice', 'chkGTaxable'));
                //lblSalesTax = document.getElementById(txt.replace('txtGvPrice', 'lblSalesTax'));
                hdnSTaxAm = document.getElementById(txt.replace('txtGvPrice', 'hdnSTaxAm'));
                txtGvStaxAmount = document.getElementById(txt.replace('txtGvPrice', 'txtGvStaxAmount'));
                lblGstTax = document.getElementById(txt.replace('txtGvPrice', 'lblGstTax'));
                hdnGSTTaxAm = document.getElementById(txt.replace('txtGvPrice', 'hdnGSTTaxAm'));
                hdnIsPO = document.getElementById(txt.replace('txtGvPrice', 'hdnIsPO'));
                hdnOutstandQuan = document.getElementById(txt.replace('txtGvPrice', 'hdnOutstandQuan'));
                hdnOutstandBalance = document.getElementById(txt.replace('txtGvPrice', 'hdnOutstandBalance'));
            }
            else if (txt.indexOf("Amount") >= 0) {
                txtGvPrice = document.getElementById(txt.replace('txtGvAmount', 'txtGvPrice'));
                txtGvQuan = document.getElementById(txt.replace('txtGvAmount', 'txtGvQuan'));
                lblAmountWithTax = document.getElementById(txt.replace('txtGvAmount', 'lblAmountWithTax'));
                hdnAmountWithTax = document.getElementById(txt.replace('txtGvAmount', 'hdnAmountWithTax'));
                txtGvAmount = document.getElementById(txt);
                hdnchkTaxable = document.getElementById(txt.replace('txtGvAmount', 'hdnchkTaxable'));
                chkTaxable = document.getElementById(txt.replace('txtGvAmount', 'chkTaxable'));
                hdnchkGTaxable = document.getElementById(txt.replace('txtGvAmount', 'hdnchkGTaxable'));
                chkGTaxable = document.getElementById(txt.replace('txtGvAmount', 'chkGTaxable'));
                //lblSalesTax = document.getElementById(txt.replace('txtGvAmount', 'lblSalesTax'));
                hdnSTaxAm = document.getElementById(txt.replace('txtGvAmount', 'hdnSTaxAm'));
                txtGvStaxAmount = document.getElementById(txt.replace('txtGvAmount', 'txtGvStaxAmount'));
                lblGstTax = document.getElementById(txt.replace('txtGvAmount', 'lblGstTax'));
                hdnGSTTaxAm = document.getElementById(txt.replace('txtGvAmount', 'hdnGSTTaxAm'));
                hdnIsPO = document.getElementById(txt.replace('txtGvAmount', 'hdnIsPO'));
                hdnOutstandQuan = document.getElementById(txt.replace('txtGvAmount', 'hdnOutstandQuan'));
                hdnOutstandBalance = document.getElementById(txt.replace('txtGvAmount', 'hdnOutstandBalance'));
            }
            //else if (txt.indexOf("AmountTot") >= 0) {
            //    txtGvPrice = document.getElementById(txt.replace('lblAmountWithTax', 'txtGvPrice'));
            //    txtGvQuan = document.getElementById(txt.replace('lblAmountWithTax', 'txtGvQuan'));
            //    txtGvAmount = document.getElementById(txt.replace('lblAmountWithTax', 'txtGvAmount'));
            //    lblAmountWithTax = document.getElementById(txt);
            //}

            //////////// PO QTY /AMOUNT CALCULATION //////////////////


            if (jQuery.trim($(hdnIsPO).val()) == '2') {
                var pobalqty = parseFloat($(hdnOutstandQuan).val());
                var pobalamt = parseFloat($(hdnOutstandBalance).val());
                var temTotal = 0;
                var receiveQuan = parseFloat($(txtGvQuan).val());
                var receiveAmnt = parseFloat($(txtGvAmount).val());
                var Price = parseFloat($(txtGvPrice).val());

                if (parseFloat(pobalqty) == 0) { pobalqty = receiveQuan; }

                if ($(txtGvQuan).val() == '') {
                    $(txtGvQuan).val('0.00');
                }
                else {
                    if (receiveQuan > pobalqty) {
                        $(txtGvQuan).val(pobalqty);
                    }
                    else {
                        $(txtGvQuan).val(receiveQuan);
                    }
                }
                var receiveQuanf = parseFloat($(txtGvQuan).val());
                temTotal = temTotal + (receiveQuanf * Price);
                //$(txtGvAmount).val(temTotal.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                $(txtGvAmount).val(parseFloat(temTotal));

            }
            //////////// PO QTY /AMOUNT CALCULATION //////////////////



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

                    if ($(hdnchkTaxable).val() == "0") {
                        $(hdnchkTaxable).val("0");
                        if (chkTaxable != null) {
                            chkTaxable.checked = false;
                        }

                        //$(lblSalesTax).val("0.00");
                        $(lblGstTax).val("0.00");
                        $(hdnSTaxAm).val("0.00");
                        $(txtGvStaxAmount).val("0.00");
                        $(hdnGSTTaxAm).val("0.00");
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
                    if (jQuery.trim($(hdnIsPO).val()) != '2') {
                        $(txtGvPrice).val(valPrice.toFixed(2));
                    }
                    $(lblAmountWithTax).text($(txtGvAmount).val());
                    $(hdnAmountWithTax).val($(txtGvAmount).val());

                    if ($(hdnchkTaxable).val() == "0") {
                        $(hdnchkTaxable).val("0");
                        if (chkTaxable != null) {
                            chkTaxable.checked = false;
                        }

                        //$(lblSalesTax).val("0.00");
                        $(lblGstTax).val("0.00");
                        $(hdnSTaxAm).val("0.00");
                        $(txtGvStaxAmount).val("0.00");

                        $(hdnGSTTaxAm).val("0.00");
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
            CalculateTotalAmt();

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                //document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
                if (jQuery.trim($(hdnIsPO).val()) != '2') {
                    document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
                }
                else {
                    if (txt.indexOf("Quan") >= 0) {
                        document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(4);
                    }
                }
            }
            if (chkTaxable != null) {
                CalTotalValStax(chkTaxable);
            }
            if (chkGTaxable != null) {
                CalTotalValGtax(chkGTaxable);
            }
        }
        function scrollToAnchor() {


            var aTag = $("#accrdPayment");
            if (aTag.hasClass("active") == false) {
                $("#accrdPayment").click();
            }

            $('html,body').animate({ scrollTop: aTag.offset().top }, 'slow');
            aTag.focus();
            return false;
        }
        function showfreq() {
            if (document.getElementById('<%=chkIsRecurr.ClientID%>').checked) {
                document.getElementById('dvfreq').style.display = 'block';
                document.getElementById('<%=lnkQuickCheck.ClientID%>').style.visibility = 'hidden';
            } else {
                document.getElementById('dvfreq').style.display = 'none';
                document.getElementById('<%=lnkQuickCheck.ClientID%>').style.visibility = 'visible';
            }
        }

        function OpenApplyCreditModal(sdate) {
            //alert(sdate);
            <%--$('#<%=txtVendorType.ClientID%>').val("");
            $('#<%=txtremarksvendor.ClientID%>').val("");
            $('#<%=txtVendorType.ClientID%>').prop("readonly", false);--%>
            $('#<%=hdnolddate.ClientID%>').val(sdate);
            $('#<%=txtaplyDate.ClientID%>').val(sdate);

            var wnd = $find('<%=ReprintCheckRange.ClientID %>');
            wnd.set_title("Apply Credit");
            wnd.Show();
        }
        function CloseApplyCreditModal() {
            var wnd = $find('<%=ReprintCheckRange.ClientID %>');
            wnd.Close();

        }

    </script>
</asp:Content>
