<%@ Page Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" EnableEventValidation="false" Inherits="AddEstimate" Title="Add Estimate || MOM" CodeBehind="AddEstimate.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="js/jquery.ns-autogrow.js"></script>
    <script type="text/javascript" src="js/quickcodes.js"></script>
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <style>
        div.row1.checkbox {
            border-bottom: none !important;
        }

        .highlight {
            background-color: Yellow;
        }

        .highlighted {
            background-color: Yellow;
        }

        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

        .input-small-num {
            min-width: 100% !important;
            text-align: right;
            font-size: 0.9rem !important;
        }

        .input-small {
            font-size: 0.9rem;
            min-width: 100% !important;
        }

        .style-mat-grid {
            background-color: #d5e8fb;
        }

        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
            max-width: 600px;
        }

        .input-small-num {
            text-align: center !important;
        }

        .overgrid {
            overflow-x: scroll;
        }

        .chkgrids {
            padding-left: 10px;
            height: 30px;
            line-height: 30px;
            border-bottom: 2px solid #1c5fb1;
            width: 30%;
        }

        .btnlinksRev {
            border: 0.5px solid #1C5FB1;
            color: #1C5FB1;
            padding: 5px 20px 5px 20px;
            border-radius: 3px;
            font-size: 0.9em;
            background-image: url(../images/accrd.gif);
            background-repeat: repeat-x;
        }



        .selected {
            width: 100% !important;
            background-color: gray !important;
            -webkit-transition: all 0.1s ease;
            -moz-transition: all 0.1s ease;
            -o-transition: all 0.1s ease;
            transition: all 0.1s ease;
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

        .display-block {
            display: block;
        }

        .display-none {
            display: none !important;
        }

        div.row.checkbox {
            border-bottom: 1px solid #9e9e9e;
            padding-bottom: 17px;
            margin-bottom: 10px !important;
        }

        .MatPart {
            background-color: #FFFFC2 !important;
        }

        .qtyRequired {
            background-color: #FFFFC2 !important;
        }

        .LabPart {
            background-color: #ADD8E6 !important;
        }

        .TotalExt {
            background-color: #98FB98 !important;
        }

        .bomAutoSizeColumn {
            width: 10% !important;
        }

        div.rmSlide {
            top: 0 !important;
        }

            div.rmSlide > ul.rmVertical, div.rmSlide > div.rmScrollWrap > ul.rmVertical {
                padding-left: 30px !important;
                padding-right: 5px !important;
            }

            div.rmSlide input[type=checkbox] {
                vertical-align: middle !important;
                display: inline-block !important;
            }

        tr.rgNoRecords > td {
            text-align: left !important;
        }

        .RadGrid_Popup > div > div.rgDataDiv {
            height: 450px !important;
        }

        .BomGrid > .rgDataDiv {
            height: auto !important;
            max-height: 400px;
        }

        .BomGrid .rgFilterRow td input[type=text] {
            padding-top: 0 !important;
        }

        .ProjectGrid > .rgDataDiv {
            height: auto !important;
            max-height: 455px;
        }
    </style>
    <style>
        .RadGrid_Popup > div > div.rgDataDiv {
            height: 450px !important;
        }

        input[class*='txtMembers_'] {
            cursor: pointer;
        }

        .chipUsers {
            width: auto !important;
            padding-left: 5px !important;
            padding-right: 5px !important;
            margin-left: 2px !important;
            margin-right: 2px !important;
            margin-top: 3px !important;
        }

        .chipUserRoles {
            background-color: #2bab54 !important;
            width: auto !important;
            padding-left: 5px !important;
            padding-right: 5px !important;
            margin-left: 2px !important;
            margin-right: 2px !important;
            margin-top: 3px !important;
        }

        .chip > label {
            font-size: 13px;
            font-weight: normal;
            color: #fff;
            line-height: 24px;
        }

        /* The container */
        .cusCheckContainer {
            display: block;
            position: relative;
            padding-left: 22px;
            /* margin-bottom: 12px; */
            cursor: pointer;
            font-size: 15px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

            /* Hide the browser's default checkbox */
            .cusCheckContainer input {
                position: absolute;
                opacity: 0;
                cursor: pointer;
                height: 0;
                width: 0;
            }

        /* Create a custom checkbox */
        .checkmark {
            position: absolute;
            top: 4px;
            left: 0;
            height: 15px;
            width: 15px;
            border-radius: 3px;
            background-color: black;
        }

        /* On mouse-over, add a grey background color */
        .cusCheckContainer:hover input ~ .checkmark {
            background-color: black;
        }

        /* When the checkbox is checked, add a blue background */
        .cusCheckContainer input:checked ~ .checkmark {
            background-color: black;
        }

        /* Create the checkmark/indicator (hidden when not checked) */
        .checkmark:after {
            content: "";
            position: absolute;
            display: none;
        }

        /* Show the checkmark when checked */
        .cusCheckContainer input:checked ~ .checkmark:after {
            display: block;
        }

        /* Style the checkmark/indicator */
        .cusCheckContainer .checkmark:after {
            left: 5px;
            top: 1px;
            width: 6px;
            height: 10px;
            border: solid white;
            border-width: 0 2px 2px 0;
            -webkit-transform: rotate(45deg);
            -ms-transform: rotate(45deg);
            transform: rotate(45deg);
        }

        .textarea-border1 {
            border: 1px solid #9e9e9e !important;
            border-radius: 6px !important;
            position: relative !important;
            z-index: 2 !important;
            background-color: #fff !important;
            max-height: none !important;
            resize: vertical !important;
        }


        .leftContend .RadAjaxPanel {
            float: left !important;
        }

        ul.anchor-links .RadAjaxPanel {
            float: right !important;
        }

        .EstimateTooltip {
            background: #000 none repeat scroll 0 0;
            filter: alpha(opacity=80);
            -moz-opacity: 0.80;
            opacity: 0.80;
            border-radius: 0px !important;
            color: #fff;
            display: none;
            padding: 10px;
            position: absolute;
            width: 300px;
            z-index: 1000;
            margin-bottom: 20px;
        }

            .EstimateTooltip:after {
                top: 0%;
                left: 0%;
                border: solid transparent;
                content: " ";
                height: 0;
                width: 0;
                position: absolute;
                pointer-events: none;
                border-color: rgba(0, 0, 0, 0);
                border-top-color: #000;
                border-width: 10px;
                margin-left: -10px;
                margin-bottom: 20px;
            }
    </style>
    <script type="text/javascript">

        function ShowHideDiscountNotes() {
            var valName = document.getElementById("<%=rfvDiscountedNotes.ClientID%>");

            var isDiscount = $('#<%=chkDiscounted.ClientID%>').prop('checked');
            if (isDiscount == true) {
                $('#divDiscountedNotes').show();
                ValidatorEnable(valName, true);
            } else {
                ValidatorEnable(valName, false);
                $('#divDiscountedNotes').hide();
            }

        }

        function ShowHideGroupEquipments(isChecked) {
            if (isChecked == true) {
                $('#divGroupEquipments').show();
            } else {
                $('#divGroupEquipments').hide();
            }
        }

        function ShowHideEquipments(isChecked) {
            if (isChecked == true) {
                $('#divEquipments').show();
            } else {
                $('#divEquipments').hide();
            }
        }

        function ReloadPage() {
            return false;
        }
        function cancel() {

            window.parent.document.getElementById('btnCancel').click();
        }
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
        function showDecimalVal(obj) {
            if (!isNaN(parseFloat(document.getElementById(obj.id).value.toString().replace(/[\$\(\),]/g, '')))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value.toString().replace(/[\$\(\),]/g, '')).toLocaleString("en-US", { minimumFractionDigits: 2 });
            }
        }
        function showDecimalOrEmptyVal(obj) {
            if (document.getElementById(obj.id).value != '' && document.getElementById(obj.id).value != null) {
                if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                    document.getElementById(obj.id).value = cleanUpCurrency("$" + parseFloat(document.getElementById(obj.id).value.toString().replace(/[\$\(\),]/g, '')).toLocaleString("en-US", { minimumFractionDigits: 2 }));
                }
            }
        }

        function QuotedAmountToFinalBidPrice() {
            var ddlPTypeData = $('#<%=ddlPType.ClientID%>').find("option:selected").text();
            if (ddlPTypeData == "Quoted") {
                var bidpr = GetNumberFromStringFormated($("#<%=hdnBidPrice.ClientID %>").val());//$("#<%=lblBidPrice.ClientID %>").val().replace(/[\$\(\),]/g, '');
                var amt = GetNumberFromStringFormated($('#<%=txtAmount.ClientID%>').val());
                if (amt == "" || amt == "0.00") {
                    $('#<%=txtAmount.ClientID%>').val(bidpr);
                }
                else {
                    if (bidpr != amt) {
                        $("[id*=txtOverride]:input.override-amt").val("$" + amt);
                    }

                }

            }

            UpdateBillingFinalBidPrice();
            Materialize.updateTextFields();
        }

        function CalculateFinalBidPrice() {
            var estType = $("#<%=ddlEstimateType.ClientID%>").val();
            if (estType == 'bid') {
                var finalBidPrice = GetNumberFromStringFormated($("[id*=txtOverride]:input.override-amt").val());
                if (finalBidPrice == "" || finalBidPrice == "0.00") {
                    var bidpr = GetNumberFromStringFormated($("#<%=hdnBidPrice.ClientID %>").val());//GetNumberFromStringFormated($("#<%=lblBidPrice.ClientID %>").val());
                    finalBidPrice = bidpr;
                }
                finalBidPrice = parseFloat(finalBidPrice);
                if (isNaN(finalBidPrice)) {
                    finalBidPrice = 0;
                }

                var milestoneCount = 0;
                $("[id$='gvMilestones_GridData']").find('tbody tr').each(function () {
                    try {
                        var $tr = $(this);
                        if ($tr.find('input[id*=txtAmount]').attr('id') != "" && typeof $tr.find('input[id*=txtAmount]').attr('id') != 'undefined') {
                            var bidAmt = $tr.find('input[id*=txtAmount]').val().replace(/[\$\(\),]/g, '');
                            if (bidAmt != "0" && bidAmt != "0.00" && bidAmt != "0.0" && bidAmt != "") {
                                milestoneCount = 1;
                            }
                        }
                    } catch (e) {

                    }
                });

                if (milestoneCount == 0) {
                    $("[id$='gvMilestones_GridData']").find('tbody tr').each(function () {
                        var $tr = $(this);
                        if ($tr.find('input[id*=txtAmount]').attr('id') != "" && typeof $tr.find('input[id*=txtAmount]').attr('id') != 'undefined') {
                            $tr.find('input[id*=txtAmount]').val(cleanUpCurrency(parseFloat(finalBidPrice).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                            $tr.find('input[id*=txtPerAmount]').val("100");
                            return false;
                        }
                    });
                }
                UpdateBillingFinalBidPrice();
            } else {
                var tAmount = 0;
                $("[id$='gvMilestones_GridData']").find('tbody tr').each(function () {
                    var $tr = $(this);

                    if ($tr.find('input[id*=txtAmount]').attr('id') != "" && typeof $tr.find('input[id*=txtAmount]').attr('id') != 'undefined') {
                        var amt = $tr.find('input[id*=txtAmount]').val().replace(/[\$\(\),]/g, '');

                        if (!isNaN(parseFloat(amt))) {
                            tAmount += parseFloat(amt);
                        }
                    }
                });
                var salestax = GetNumberFromStringFormated($('#hd_salestax').val());
                if (!isNaN(parseFloat(salestax))) {
                    tAmount += parseFloat(salestax);
                }
                $("#<%=txtOverride.ClientID%>").val(tAmount.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                $("#<%=lblFinalBid.ClientID%>").html(tAmount.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            }
        }

        function CalculateEstimateMilestone() {
            var tBid = 0;
            var tOr = 0;
            var tProfit = 0;
            var tSub = 0;
            var tOh = 0;

            var bid = GetNumberFromStringFormated($('.bid-price').val());
            var Or = GetNumberFromStringFormated($("[id*=txtOverride]:input.override-amt").val());
            var Sub = GetNumberFromStringFormated($('#hd_subtotal').val());
            var Oh = GetNumberFromStringFormated($('#hd_oh').val());//.replace(/[\$\(\),]/g, '');
            //console.log('.oh: ' + Oh);
            if (bid != "" && typeof bid != 'undefined') {
                if (!isNaN(parseFloat(bid))) {
                    tBid = parseFloat(bid);
                }
            }
            if (Or != "" && typeof Or != 'undefined') {
                if (!isNaN(parseFloat(Or))) {
                    tOr = parseFloat(Or);
                }
            }
            if (Sub != "" && typeof Sub != 'undefined') {
                if (!isNaN(parseFloat(Sub))) {
                    tSub = parseFloat(Sub);
                }
            }
            if (Oh != "" && typeof Oh != 'undefined') {
                if (!isNaN(parseFloat(Oh))) {
                    tOh = parseFloat(Oh);
                }
            }

            if (tBid == 0 && tOr == 0) {
                $("[id$='gvMilestones_GridData']").find('tbody tr').each(function () {
                    var $tr = $(this);

                    if ($tr.find('input[id*=txtAmount]').attr('id') != "" && typeof $tr.find('input[id*=txtAmount]').attr('id') != 'undefined') {
                        var bidAmt = $tr.find('input[id*=txtAmount]').val().replace(/[\$\(\),]/g, '');

                        if (!isNaN(parseFloat(bidAmt))) {
                            tBid += parseFloat(bidAmt);
                        }
                    }
                });

                tOr = tBid;
            }

            if (tSub != 0 || tOh != 0 || tOr != 0) {
                tProfit = ((1 - ((tSub + tOh) / tOr)) * 100) //1 - ((Subtota + OH) / Override)
            }
            else {
                tProfit = 0;
            }

            $('.bid-price').val(cleanUpCurrency("$" + parseFloat(tBid).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            //$("[id*=txtOverride]:input.override-amt").val(cleanUpCurrency("$" + parseFloat(tOr).toLocaleString("en-US", { minimumFractionDigits: 2 })));


        }

        function SetDefaultVaules() {
            var tMat = 0;
            var tLb = 0;
            var tsub = 0;
            var tcont = 0;
            var tOther = 0;
            var tOh = 0;
            var tMarkup = 0;
            var tPretax = 0;
            var tStax = 0;
            var tHour = 0;
            var tTotal = 0;
            var tProfit = 0;
            var tNonTaxable = 0;
            var tTaxable = 0;
            
            var tOr = 0;
            var saleTaxableValue = 0;


            $("[id$='gvBOM_GridData'] tbody").find("tr").each(function () {
                var $tr = $(this);

                if ($tr.find('select[id*=ddlBType]').attr('id') != "" && typeof $tr.find('select[id*=ddlBType]').attr('id') != 'undefined') {
                    var type = $tr.find('select[id*=ddlBType]').val().replace(/[\$\(\),]/g, '');

                    if (!isNaN(parseInt(type))) {
                        if ($tr.find('input[id*=hdnMatExt]').attr('id') != "" && typeof $tr.find('input[id*=hdnMatExt]').attr('id') != 'undefined') {
                            if (type == '1' || type == '8') {//Materials or Inventory
                                var mat = $tr.find('input[id*=hdnMatExt]').val().replace(/[\$\(\),]/g, '');
                                if (!isNaN(parseFloat(mat))) {
                                    tMat += parseFloat(mat);
                                }
                            } else {
                                var otherexp = $tr.find('input[id*=hdnMatExt]').val().replace(/[\$\(\),]/g, '');

                                if (!isNaN(parseFloat(otherexp))) {
                                    tOther += parseFloat(otherexp);
                                }
                            }
                        }

                       
                        if ($tr.find('input[id*=hdnLabExt]').attr('id') != "" && typeof $tr.find('input[id*=hdnLabExt]').attr('id') != 'undefined') {
                            var Lb = $tr.find('input[id*=hdnLabExt]').val().replace(/[\$\(\),]/g, '');

                            if (!isNaN(parseFloat(Lb))) {
                                tLb += parseFloat(Lb);
                            }
                        }
                    }

                    //Calculate the Material & Labor Markup
                    var materialQty = $tr.find('input[id*=txtQtyReq]').val();
                    //console.log("materialQty: " + materialQty);
                    if (materialQty == "") {
                        materialQty = "0";
                    }
                    var materialUnit = $tr.find('input[id*=txtBudgetUnit]').val();
                    //console.log("materialUnit: " + materialUnit);
                    if (materialUnit == "") {
                        materialUnit = "0";
                    }
                    var materialMatPrice = $tr.find('input[id*=txtMatPrice]').val();
                    //console.log("materialMatPrice: " + materialMatPrice);
                    if (materialMatPrice == "") {
                        materialMatPrice = "0";
                    }

                    var laborHours = $tr.find('input[id*=txtHours]').val();
                    //console.log("laborHours: " + laborHours);
                    if (laborHours == "") {
                        laborHours = "0";
                    }
                    var laborRate = $tr.find('input[id*=txtLabRate]').val();
                    //console.log("laborRate: " + laborRate);
                    if (laborRate == "") {
                        laborRate = "0";
                    }
                    var laborLabPrice = $tr.find('input[id*=txtLabPrice]').val();
                    //console.log("laborLabPrice: " + laborLabPrice);
                    if (laborLabPrice == "") {
                        laborLabPrice = "0";
                    }
                    //;
                    var materialMatPrice = materialMatPrice.replace(",", "");
                    var laborLabPrice = laborLabPrice.replace(",", "");
                    materialUnit = materialUnit.replace(",", "");
                    materialQty = materialQty.replace(",", "");
                    laborHours = laborHours.replace(",", "");
                    laborRate = laborRate.replace(",", "");
                    var matcal = materialMatPrice - (materialQty * materialUnit);
                    var labcal = laborLabPrice - (laborHours * laborRate);
                
                    var materialTaxable = 0;
                    var laborTaxable = 0;
                    var chkMatSalestax = $tr.find('input[id*=chkMatSalestax]').is(":checked");
                    if (chkMatSalestax == true) {
                        
                        materialTaxable = materialQty * materialUnit;
                    }

                    var chkLabSalestax = $tr.find('input[id*=chkLabSalestax]').is(":checked");
                    if (chkLabSalestax == true) {
                       
                        laborTaxable = laborHours * laborRate;
                    }
                    saleTaxableValue = saleTaxableValue + materialTaxable + laborTaxable;
                }

                //Calculate the Total Hour
                if ($tr.find("[id$='txtHours']").attr('id') != "" && typeof $tr.find("[id$='txtHours']").attr('id') != 'undefined') {
                    var strHour = $tr.find("[id$='txtHours']").val().replace(/[\$\(\),]/g, '');
                    var hour = parseFloat(strHour);
                    if (!isNaN(hour)) {
                        tHour += hour;
                    }
                }
            });

            if (isNaN(parseFloat(tMarkup))) {
                // alert("Hello");
                tMarkup = 0;
            }

            var contper = GetNumberFromStringFormated($("[id*=txtPerContingencies]:input").val());
            var ohper = GetNumberFromStringFormated($("[id*=txtHOPercentAge]:input").val());
            var or = GetNumberFromStringFormated($("[id*=txtOverride]:input.override-amt").val());

            if (or == "" || or == "0.00") {
                var bidpr = GetNumberFromStringFormated($("#<%=hdnBidPrice.ClientID %>").val());//GetNumberFromStringFormated($("#<%=lblBidPrice.ClientID %>").val());
                 if (!isNaN(parseFloat(bidpr))) {
                     tOr = parseFloat(bidpr);
                 }
             }
             else {
                 if (!isNaN(parseFloat(or))) {
                     tOr = parseFloat(or);
                 }
             }

             if (contper != "" && typeof contper != 'undefined') {
                 if (!isNaN(parseFloat(contper))) {
                     tcont = parseFloat(contper) * (tMat + tLb + tOther) / 100;
                 }
             }

             tsub = tMat + tLb + tOther + tcont;

             if (ohper != "" && typeof ohper != 'undefined') {
                 if (!isNaN(parseFloat(ohper))) {
                     tOh = parseFloat(ohper) * tsub / 100;
                 }
             }

             //Calculate OH 
             var ohPer = $("[id*=txtHOPercentAge]:input").val();
             if (ohPer == "") {
                 var def_ohper = $("#<%=hdnDefOHPer.ClientID%>").val();
                 if (def_ohper != "") {
                     ohPer = def_ohper;
                 } else {
                     ohPer = "0";
                 }
             }

             var finalCal = parseFloat(ohPer) * parseFloat(tsub) / 100;
             $("[id*=txtOH]:input").val(cleanUpCurrency("$" + finalCal.toLocaleString("en-US", { minimumFractionDigits: 2 })));
             $("[id*=txtHOPercentAge]:input").val(ohPer); 
             $('#hd_oh').val(finalCal);
             tOh = finalCal;



             var markupPer = $("#ctl00_ContentPlaceHolder1_txtMarkupPercentAge").val();
             if (!isNaN(parseFloat(markupPer)) && parseFloat(markupPer) != 0) {
                 var currMarkup = $('#hd_markup').val();
                 var currMarkupPer = 0;
                 if (!isNaN(parseFloat(currMarkup))) {
                     currMarkupPer = parseFloat(currMarkup) * 100 / parseFloat(parseFloat(tsub) + parseFloat(tOh));
                     currMarkupPer = parseFloat(currMarkupPer.toFixed(4));
                 } else {
                     currMarkup = 0;
                 }
                 if (currMarkupPer != markupPer) {
                     tMarkup = parseFloat(markupPer) * parseFloat(parseFloat(tsub) + parseFloat(tOh)) / 100;
                 } else {
                     tMarkup = parseFloat(currMarkup);
                 }

             }

             tPretax = tsub + tOh + tMarkup;
             tTaxable = tOh + tMarkup + tcont + saleTaxableValue;
             tNonTaxable = tPretax - tTaxable;
             if (tNonTaxable.toFixed(2) == 0) {// to fixed error ($0.00)
                 tNonTaxable = 0;
             }

             //tStax = tTaxable * staxRate;
             tTotal = tsub + tOh + tMarkup + tStax;
             tProfit = (1 - (tsub + tOh) / tOr) * 100; //1 - ((Subtota + OH) / Override)

             $('.mat-ext').html(cleanUpCurrency("$" + parseFloat(tMat).toLocaleString("en-US", { minimumFractionDigits: 2 })));
             $('#hd_materialexp').val(tMat);
             $('.lb-ext').html(cleanUpCurrency("$" + parseFloat(tLb).toLocaleString("en-US", { minimumFractionDigits: 2 })));
             $('#hd_laborexp').val(tLb);
             $('.cont').val(cleanUpCurrency("$" + parseFloat(tcont).toLocaleString("en-US", { minimumFractionDigits: 2 })));
             $('.other-ext').html(cleanUpCurrency("$" + parseFloat(tOther).toLocaleString("en-US", { minimumFractionDigits: 2 })));
             $('#hd_otherexp').val(tOther);

             $('.subtotal').html(cleanUpCurrency("$" + parseFloat(tsub).toLocaleString("en-US", { minimumFractionDigits: 2 })));
             $('#hd_subtotal').val(tsub);
            
            $('.pretax').html(cleanUpCurrency("$" + parseFloat($('#<%=hdnBidPrice.ClientID%>').val()).toLocaleString("en-US", { minimumFractionDigits: 2 })));
         
            $('.nontaxable').html(cleanUpCurrency("$" + parseFloat(tsub + tOh).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('.taxable').html(cleanUpCurrency("$" + parseFloat($('#hd_markup').val()).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('#hd_pretax').val($('#hd_total').val());
           var ttt= $('#<%=hdnBidPrice.ClientID%>').val();
            $('.total').html(cleanUpCurrency("$" + parseFloat(ttt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('#hd_total').val(ttt);
             //Calculate the Total Cost 
            $('.totalcost').html(cleanUpCurrency("$" + parseFloat(tsub + tOh).toLocaleString("en-US", { minimumFractionDigits: 2 })));

           $('#hd_totalcost').val(parseFloat(tsub + tOh)); 



             //Calculate Commission
             var tcommission = 0;
             var comPer = $("[id*=txtPercentAgeCommission]:input").val();
             if (comPer == "") {
                 var def_comPer = $("#<%=hdnDefCOMMSPer.ClientID%>").val();
                if (def_comPer != "") {
                    comPer = def_comPer;
                } else {
                    comPer = "0";
                }
            }

            var finalCal = parseFloat(comPer) * (parseFloat(tsub) + parseFloat(tOh) + parseFloat(tMarkup)) / 100;
            $("[id*=txtCommission]:input").val(cleanUpCurrency("$" + finalCal.toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $("[id*=txtPercentAgeCommission]").val(comPer);
            tcommission = finalCal;
            $('#hd_commission').val(finalCal);
             
           
          <%--  $('.stax').html(cleanUpCurrency("$" + parseFloat($('#<%=hdnBidPrice.ClientID%>').val()).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('#hd_salestax').val($('#<%=hdnBidPrice.ClientID%>').val());--%>


            $('.totalHour').html((cleanUpCurrency(parseFloat(tHour.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 }))));
            $('#hd_totalHour').val((cleanUpCurrency(parseFloat(tHour.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 }))));
             
             
            
            //$("[id*=lblBidPrice]").val("$" + tBidPrice);
            $("[id*=lblBidPrice]").html("$" + $('#<%=hdnBidPrice.ClientID%>').val());
         <%--   $('#<%=hdnBidPrice.ClientID%>').val(tBidPrice);--%>

            

            $('.markup').html("$" + $('#hd_markup').val());

           /* $('#hd_markup').val(ProfitAmount);*/

             Materialize.updateTextFields();
         }

        function CalculateEstimateBOM() {
            var tMat = 0;
            var tLb = 0;
            var tsub = 0;
            var tcont = 0;
            var tOther = 0;
            var tOh = 0;
            var tMarkup = 0;
            var tPretax = 0;
            var tStax = 0;
            var tHour = 0;
            var tTotal = 0;
            var tProfit = 0;
            var tNonTaxable = 0;
            var tTaxable = 0;
            //var tProfitAmount = 0;
            //var ttype = 0;
            var tOr = 0;
            var saleTaxableValue = 0;


            $("[id$='gvBOM_GridData'] tbody").find("tr").each(function () {
                var $tr = $(this);

                if ($tr.find('select[id*=ddlBType]').attr('id') != "" && typeof $tr.find('select[id*=ddlBType]').attr('id') != 'undefined') {
                    var type = $tr.find('select[id*=ddlBType]').val().replace(/[\$\(\),]/g, '');

                    if (!isNaN(parseInt(type))) {
                        if ($tr.find('input[id*=hdnMatExt]').attr('id') != "" && typeof $tr.find('input[id*=hdnMatExt]').attr('id') != 'undefined') {
                            if (type == '1' || type == '8') {//Materials or Inventory
                                var mat = $tr.find('input[id*=hdnMatExt]').val().replace(/[\$\(\),]/g, '');
                                if (!isNaN(parseFloat(mat))) {
                                    tMat += parseFloat(mat);
                                }
                            } else {
                                var otherexp = $tr.find('input[id*=hdnMatExt]').val().replace(/[\$\(\),]/g, '');

                                if (!isNaN(parseFloat(otherexp))) {
                                    tOther += parseFloat(otherexp);
                                }
                            }
                        }

                        //The below conditon get commented because we want to calculate the all Labour Data.
                        // if (type == '2') {
                        if ($tr.find('input[id*=hdnLabExt]').attr('id') != "" && typeof $tr.find('input[id*=hdnLabExt]').attr('id') != 'undefined') {
                            var Lb = $tr.find('input[id*=hdnLabExt]').val().replace(/[\$\(\),]/g, '');

                            if (!isNaN(parseFloat(Lb))) {
                                tLb += parseFloat(Lb);
                            }
                        }
                    }

                    //Calculate the Material & Labor Markup
                    var materialQty = $tr.find('input[id*=txtQtyReq]').val();
                    //console.log("materialQty: " + materialQty);
                    if (materialQty == "") {
                        materialQty = "0";
                    }
                    var materialUnit = $tr.find('input[id*=txtBudgetUnit]').val();
                    //console.log("materialUnit: " + materialUnit);
                    if (materialUnit == "") {
                        materialUnit = "0";
                    }
                    var materialMatPrice = $tr.find('input[id*=txtMatPrice]').val();
                    //console.log("materialMatPrice: " + materialMatPrice);
                    if (materialMatPrice == "") {
                        materialMatPrice = "0";
                    }

                    var laborHours = $tr.find('input[id*=txtHours]').val();
                    //console.log("laborHours: " + laborHours);
                    if (laborHours == "") {
                        laborHours = "0";
                    }
                    var laborRate = $tr.find('input[id*=txtLabRate]').val();
                    //console.log("laborRate: " + laborRate);
                    if (laborRate == "") {
                        laborRate = "0";
                    }
                    var laborLabPrice = $tr.find('input[id*=txtLabPrice]').val();
                    //console.log("laborLabPrice: " + laborLabPrice);
                    if (laborLabPrice == "") {
                        laborLabPrice = "0";
                    }
                    //;
                    var materialMatPrice = materialMatPrice.replace(",", "");
                    var laborLabPrice = laborLabPrice.replace(",", "");
                    materialUnit = materialUnit.replace(",", "");
                    materialQty = materialQty.replace(",", "");
                    laborHours = laborHours.replace(",", "");
                    laborRate = laborRate.replace(",", "");
                    var matcal = materialMatPrice - (materialQty * materialUnit);
                    var labcal = laborLabPrice - (laborHours * laborRate);
                    //console.log("matcal = materialMatPrice - (materialQty * materialUnit): " + matcal);

                    //console.log("labcal = laborLabPrice - (laborHours * laborRate): " + labcal);

                    //tMarkup = tMarkup + matcal + labcal;

                    //Adding the Values for Sales Tax
                    var materialTaxable = 0;
                    var laborTaxable = 0;
                    var chkMatSalestax = $tr.find('input[id*=chkMatSalestax]').is(":checked");
                    if (chkMatSalestax == true) {
                        //var _materialExt = materialQty * materialUnit;
                        //if (materialMatPrice > _materialExt) {
                        //    materialTaxable = _materialExt + (materialMatPrice - _materialExt);
                        //}
                        //else {
                        //    materialTaxable = _materialExt;
                        //}
                        materialTaxable = materialQty * materialUnit;
                    }

                    var chkLabSalestax = $tr.find('input[id*=chkLabSalestax]').is(":checked");
                    if (chkLabSalestax == true) {
                        //var _laborExt = laborHours * laborRate;
                        //if (laborLabPrice > _laborExt) {
                        //    laborTaxable = _laborExt + (laborLabPrice - _laborExt);
                        //}
                        //else {
                        //    laborTaxable = _laborExt;
                        //}
                        laborTaxable = laborHours * laborRate;
                    }
                    saleTaxableValue = saleTaxableValue + materialTaxable + laborTaxable;
                }

                //Calculate the Total Hour
                if ($tr.find("[id$='txtHours']").attr('id') != "" && typeof $tr.find("[id$='txtHours']").attr('id') != 'undefined') {
                    var strHour = $tr.find("[id$='txtHours']").val().replace(/[\$\(\),]/g, '');
                    var hour = parseFloat(strHour);
                    if (!isNaN(hour)) {
                        tHour += hour;
                    }
                }
            });

            if (isNaN(parseFloat(tMarkup))) {
                // alert("Hello");
                tMarkup = 0;
            }

            var contper = GetNumberFromStringFormated($("[id*=txtPerContingencies]:input").val());
            var ohper = GetNumberFromStringFormated($("[id*=txtHOPercentAge]:input").val());
            var or = GetNumberFromStringFormated($("[id*=txtOverride]:input.override-amt").val());

            if (or == "" || or == "0.00") {
                var bidpr = GetNumberFromStringFormated($("#<%=hdnBidPrice.ClientID %>").val());//GetNumberFromStringFormated($("#<%=lblBidPrice.ClientID %>").val());
                if (!isNaN(parseFloat(bidpr))) {
                    tOr = parseFloat(bidpr);
                }
            }
            else {
                if (!isNaN(parseFloat(or))) {
                    tOr = parseFloat(or);
                }
            }

            if (contper != "" && typeof contper != 'undefined') {
                if (!isNaN(parseFloat(contper))) {
                    tcont = parseFloat(contper) * (tMat + tLb + tOther) / 100;
                }
            }

            tsub = tMat + tLb + tOther + tcont;

            if (ohper != "" && typeof ohper != 'undefined') {
                if (!isNaN(parseFloat(ohper))) {
                    tOh = parseFloat(ohper) * tsub / 100;
                }
            }

            //Calculate OH 
            var ohPer = $("[id*=txtHOPercentAge]:input").val();
            if (ohPer == "") {
                var def_ohper = $("#<%=hdnDefOHPer.ClientID%>").val();
                if (def_ohper != "") {
                    ohPer = def_ohper;
                } else {
                    ohPer = "0";
                }
            }

            var finalCal = parseFloat(ohPer) * parseFloat(tsub) / 100;
            $("[id*=txtOH]:input").val(cleanUpCurrency("$" + finalCal.toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $("[id*=txtHOPercentAge]:input").val(ohPer);
            //console.log('finalCal: ' + finalCal);
            $('#hd_oh').val(finalCal);
            tOh = finalCal;



            var markupPer = $("#ctl00_ContentPlaceHolder1_txtMarkupPercentAge").val();
            if (!isNaN(parseFloat(markupPer)) && parseFloat(markupPer) != 0) {
                var currMarkup = $('#hd_markup').val();
                var currMarkupPer = 0;
                if (!isNaN(parseFloat(currMarkup))) {
                    currMarkupPer = parseFloat(currMarkup) * 100 / parseFloat(parseFloat(tsub) + parseFloat(tOh));
                    currMarkupPer = parseFloat(currMarkupPer.toFixed(4));
                } else {
                    currMarkup = 0;
                }
                if (currMarkupPer != markupPer) {
                    tMarkup = parseFloat(markupPer) * parseFloat(parseFloat(tsub) + parseFloat(tOh)) / 100;
                } else {
                    tMarkup = parseFloat(currMarkup);
                }

            }

            tPretax = tsub + tOh + tMarkup;
            tTaxable = tOh + tMarkup + tcont + saleTaxableValue;
            tNonTaxable = tPretax - tTaxable;
            if (tNonTaxable.toFixed(2) == 0) {// to fixed error ($0.00)
                tNonTaxable = 0;
            }

            //tStax = tTaxable * staxRate;
            tTotal = tsub + tOh + tMarkup + tStax;
            tProfit = (1 - (tsub + tOh) / tOr) * 100; //1 - ((Subtota + OH) / Override)

            $('.mat-ext').html(cleanUpCurrency("$" + parseFloat(tMat).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('#hd_materialexp').val(tMat);
            $('.lb-ext').html(cleanUpCurrency("$" + parseFloat(tLb).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('#hd_laborexp').val(tLb);
            $('.cont').val(cleanUpCurrency("$" + parseFloat(tcont).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('.other-ext').html(cleanUpCurrency("$" + parseFloat(tOther).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('#hd_otherexp').val(tOther);

            $('.subtotal').html(cleanUpCurrency("$" + parseFloat(tsub).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('#hd_subtotal').val(tsub);

            $('.pretax').html(cleanUpCurrency("$" + parseFloat(tPretax).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('.nontaxable').html(cleanUpCurrency("$" + parseFloat(tNonTaxable).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('.taxable').html(cleanUpCurrency("$" + parseFloat(tTaxable).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('#hd_pretax').val(tPretax);
            //$('.stax').html(cleanUpCurrency("$" + parseFloat(tStax).toLocaleString("en-US", { minimumFractionDigits: 2 })));



            //Calculate the Total Cost 
            $('.totalcost').html(cleanUpCurrency("$" + parseFloat(tsub + tOh).toLocaleString("en-US", { minimumFractionDigits: 2 })));

            $('#hd_totalcost').val(parseFloat(tsub + tOh));



            //Calculate Commission
            var tcommission = 0;
            var comPer = $("[id*=txtPercentAgeCommission]:input").val();
            if (comPer == "") {
                var def_comPer = $("#<%=hdnDefCOMMSPer.ClientID%>").val();
                if (def_comPer != "") {
                    comPer = def_comPer;
                } else {
                    comPer = "0";
                }
            }

            var finalCal = parseFloat(comPer) * (parseFloat(tsub) + parseFloat(tOh) + parseFloat(tMarkup)) / 100;
            $("[id*=txtCommission]:input").val(cleanUpCurrency("$" + finalCal.toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $("[id*=txtPercentAgeCommission]").val(comPer);
            tcommission = finalCal;
            $('#hd_commission').val(finalCal);

            //Calculate the Sales Tax
            //var saleTax = $("[id*=drpSaleTax]").val();
            var _saleTaxText = $("[id*=drpSaleTax] option:selected").text();
            var saleTax = 0;
            if (_saleTaxText == "Select Sales Tax") {
                saleTax = 0;
            }
            else {
                var saleTaxVal = _saleTaxText.split('/');
                saleTax = saleTaxVal[saleTaxVal.length - 1];
            }

            //var stax_salesTax = parseFloat(saleTaxableValue) * parseFloat(saleTax) / 100;
            var stax_salesTax = parseFloat(tTaxable) * parseFloat(saleTax) / 100;
            var _stax_salesTax = stax_salesTax.toFixed(2);
            $('.stax').html(cleanUpCurrency("$" + parseFloat(_stax_salesTax).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('#hd_salestax').val(stax_salesTax);


            $('.totalHour').html((cleanUpCurrency(parseFloat(tHour.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 }))));
            $('#hd_totalHour').val((cleanUpCurrency(parseFloat(tHour.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 }))));

            //Total 
            tTotal = tTotal + tcommission + stax_salesTax;
            var _tTotal = tTotal.toFixed(2);
            $('.total').html(cleanUpCurrency("$" + parseFloat(_tTotal).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('#hd_total').val(_tTotal);
            var tBidPrice = parseFloat(tTotal.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 });
            //$("[id*=lblBidPrice]").val("$" + tBidPrice);
            $("[id*=lblBidPrice]").html("$" + tBidPrice);
            $('#<%=hdnBidPrice.ClientID%>').val(tBidPrice);

            var ddlPTypeData = $('#<%=ddlPType.ClientID%>').find("option:selected").text();
            if (ddlPTypeData == "Quoted") {
                $('#<%=txtAmount.ClientID%>').val(tBidPrice);
            }

            CalculateFinalBidPrice();
            CalculateAndFillBOMFooter();
            Materialize.updateTextFields();
        }

        function CalculateAndFillBOMFooter() {
            var bomTotMatQty = 0;
            var bomTotMatPrice = 0;
            var bomTotMatExt = 0;
            var bomTotLabHours = 0;
            var bomTotLabExt = 0;
            var bomTotLabPrice = 0;
            var bomTotTotalExt = 0;
            var bomTotMatMod = 0;
            var bomTotLabMod = 0;
            var bomTotMatBudgetUnit = 0;

            //debugger
            $("[id$='gvBOM_GridData'] tbody").find("tr").each(function () {
                var $tr = $(this);

                var materialQty = $tr.find('input[id*=txtQtyReq]').val();
                if (materialQty == null || materialQty == "") {
                    materialQty = "0";
                }
                materialQty = materialQty.replace(",", "");

                var matExt = $tr.find('input[id*=hdnMatExt]').val();
                if (matExt == null || matExt == "") {
                    matExt = "0";
                }
                matExt = matExt.replace(/[\$\(\),]/g, '');

                var materialMatPrice = $tr.find('input[id*=txtMatPrice]').val();
                if (materialMatPrice == null || materialMatPrice == "") {
                    materialMatPrice = "0";
                }
                materialMatPrice = materialMatPrice.replace(",", "");

                var laborHours = $tr.find('input[id*=txtHours]').val();
                //console.log("laborHours: " + laborHours);
                if (laborHours == null || laborHours == "") {
                    laborHours = "0";
                }
                laborHours = laborHours.replace(",", "");

                var laborLabPrice = $tr.find('input[id*=txtLabPrice]').val();
                //console.log("laborLabPrice: " + laborLabPrice);
                if (laborLabPrice == null || laborLabPrice == "") {
                    laborLabPrice = "0";
                }
                laborLabPrice = laborLabPrice.replace(",", "");

                var labExt = $tr.find('input[id*=hdnLabExt]').val();
                if (labExt == null || labExt == "") {
                    labExt = "0";
                }
                labExt = labExt.replace(/[\$\(\),]/g, '');

                var totalExt = $tr.find('[id*=lblTotalExt]').text().replace(/[\$\(\),]/g, '');
                if (totalExt == null || totalExt == "") {
                    totalExt = "0";
                }
                totalExt = totalExt.replace(/[\$\(\),]/g, '');

                //txtBudgetUnit
                var matBudgetUnit = $tr.find('input[id*=txtBudgetUnit]').val();
                if (matBudgetUnit == null || matBudgetUnit == "") {
                    matBudgetUnit = "0";
                }
                matBudgetUnit = matBudgetUnit.replace(",", "");

                //txtMatMod
                var matMod = $tr.find('input[id*=txtMatMod]').val();
                if (matMod == null || matMod == "") {
                    matMod = "0";
                }
                matMod = matMod.replace(",", "");
                //txtLabMod
                var labMod = $tr.find('input[id*=txtLabMod]').val();
                if (labMod == null || labMod == "") {
                    labMod = "0";
                }
                labMod = labMod.replace(",", "");

                bomTotMatQty = bomTotMatQty + parseFloat(materialQty);
                bomTotMatPrice = bomTotMatPrice + parseFloat(materialMatPrice);
                bomTotMatExt = bomTotMatExt + parseFloat(matExt);
                bomTotLabHours = bomTotLabHours + parseFloat(laborHours);
                bomTotLabExt = bomTotLabExt + parseFloat(labExt);
                bomTotLabPrice = bomTotLabPrice + parseFloat(laborLabPrice);
                bomTotTotalExt = bomTotTotalExt + parseFloat(totalExt);

                bomTotMatBudgetUnit = bomTotMatBudgetUnit + parseFloat(matBudgetUnit);
                bomTotMatMod = bomTotMatMod + parseFloat(matMod);
                bomTotLabMod = bomTotLabMod + parseFloat(labMod);

            });


            $('[id*=lblTotalQtyReq]').html(bomTotMatQty.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $('[id*=lblTotalMatExt]').html("$" + bomTotMatExt.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $('[id*=lblTotalMatPrice]').html("$" + bomTotMatPrice.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $('[id*=lblTotalHours]').html(bomTotLabHours.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $('[id*=lblTotalLabExt]').html("$" + bomTotLabExt.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $('[id*=lblTotalLabPrice]').html("$" + bomTotLabPrice.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $('[id*=lblTotalTotalExt]').html("$" + bomTotTotalExt.toLocaleString("en-US", { minimumFractionDigits: 2 }));

            $('[id*=lblTotalMatBudgetUnit]').html("$" + bomTotMatBudgetUnit.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $('[id*=lblTotalMatMod]').html("$" + bomTotMatMod.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $('[id*=lblTotalLabMod]').html("$" + bomTotLabMod.toLocaleString("en-US", { minimumFractionDigits: 2 }));
        }

        function GetNumberFromStringFormated(strNum) {
            strNum = strNum.replace(/[\$,]/g, '');
            if (strNum.indexOf('(') == 0) {
                strNum = strNum.replace(/[\(\),]/g, '');
                strNum = '-' + strNum;
            }
            return strNum;
        }

        function removeLine(Gridview) {
            $("#" + Gridview).find('tr').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function () {
                    if ($("#" + Gridview).find('tr').length > 3) {
                        $(this).closest('tr').remove();
                    }
                    else {
                        $(this).closest('tr').find('input:text').val('');
                    }
                });
            });
        }

        $(document).ready(function () {
            console.log("$(document).ready 1"); 

               SetDefaultVaules(); 
           
            $("#dvestimateleftpanel").attr("style", "display:none");
            $("#dvestimatemainpanel").attr("style", "width:100% !important");

            GetSidePanel($("#<%=hdnestimateid.ClientID%>").val(), '0', '0');

            //BindContactChange();

            //$(".factor").live("focusout", function () {
            $(".factor").focusout(function () {
                var id = $(this).parent().parent("div").find("#hdnEstimateCalculationTemplateId").val();
                GetSidePanel($("#<%=hdnestimateid.ClientID%>").val(), id, $(this).val());

            });

            if ($("[id*=hdnChkMat]").val() == '' && $("[id*=hdnChkLb]").val() == '') {
                $("[id*=chkMaterial]").prop('checked', true);
                $("[id*=hdnChkMat]").val('1');
                $("[id*=chkLabor]").prop('checked', true);
                $("[id*=hdnChkLb]").val('1');
            }
        });

      

        function GetSidePanel(id, tempId, factor) {
            $.ajax({
                type: "POST",
                url: "CustomerAuto.asmx/GetEstimateExpenseItems",
                data: '{id:"' + id + '",tempId:"' + tempId + '",factor:"' + factor + '"}',
                //data: '{searchTerm: "", column: "", page:"' + pageindex + '",stdate:"",enddate:"",Department:"' + tabindex + '"}',
                //LableName:'Materials',CalculationType:'1',ShowClaculatedValue:'undefined',InputBasedCalculationOperation:'0',Sequence:'1'
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: BindSidePanel,
                failure: function (response) {
                    //$('#iframeloading').hide();
                },
                error: function (response) {
                    // $('#iframeloading').hide();
                }
            });
        }

        function BindSidePanel(response) {
            var result = response.d.ReponseObject;
            var dvHTML = '';
            $("#dvestdetils").find("#dvSidePanel").remove();
            if (response.d.Header.HasError == false) {

                dvHTML += '<div id="dvSidePanel" style="float: right; width: 35%; background-color: aliceblue; -moz-border-radius: 4px; -webkit-border-radius: 4px; border-radius: 4px;">'

                for (var i = 0; i < result.length; i++) {
                    dvHTML += '<div class="form-col" style="padding: 3px;"><input name="hdnEstimateCalculationTemplateId" id="hdnEstimateCalculationTemplateId" value="' + result[i].ID + '" type="hidden"><div class="fc-label">' + result[i].EstimateCalculationTemplateLableName;
                    //Precalculated fileds
                    if (!result[i].EstimateCalculationInputBasedCalculation) {
                        dvHTML += '</div>'
                        dvHTML += '<div class="fc-input">'
                        if (result[i].EstimateCalculationTemplateInputDataDerived || result[i].EstimateCalculationTemplateIsBOM)
                            dvHTML += '<input id="TextBox2" name="TextBox2" type="text" class="form-control" disabled value="' + result[i].EstimateCalculationValue + '" ></input>'
                        else
                            dvHTML += '<input id="TextBox2" name="TextBox2" type="text" class="form-control" value="' + result[i].EstimateCalculationValue + '"  ></input>'
                        dvHTML += '</div></div>'
                    }
                    //User Input Fields
                    else {
                        dvHTML += '</div><div class="fc-input" style="width: 10%;"><input id="TextBox8" name="TextBox2" type="text" class="form-control factor" TabIndex="1" value="' + result[i].EstimateCalculationTemplateInputBasedCalculationfactor + '" ></input></div>'
                        dvHTML += '<div class="fc-input" style="width: 40%; padding-left: 5px;"><input id="TextBox2" name="TextBox2" type="text" class="form-control" value="' + result[i].EstimateCalculationValue + '" ></input></div></div>'
                    }

                }
                dvHTML += '</div>';

                $("#dvestdetils").append(dvHTML);
            }
        }
    </script>

    <script type="text/javascript">

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

        function ConvertDigit(obj) {
            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }

        function HideShowOnBillingTypeChange(value) {
            if (value == "0") {
                $('#txtAmount').val("");
                $('.hideShowOnBillingType').hide();
            }
            else
                $('.hideShowOnBillingType').show();

            QuotedAmountToFinalBidPrice();
        }
        function AmountValidate(s, args) {
            if ($('#<%= ddlPType.ClientID %>').val() == "0") {
                args.IsValid = true;
            }
            else {
                if ($('#<%= txtAmount.ClientID %>').val() == "" || parseFloat($('#<%= txtAmount.ClientID %>').val()) == 0) {
                    args.IsValid = false;
                }
                else
                    args.IsValid = true;
            }
        }

        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.dtxtScrapFactor = 0;
            }
        }

        function CalculateLb(obj, isOverrideProfitPer) {
            var row = obj.parentNode.parentNode;
            //var rowIndex = row.rowIndex - 1;
            var hr = $(row).find("[id$='txtHours']").val().replace(/[\$\(\),]/g, '');              //Labor Hour
            var rate = $(row).find("[id$='txtLabRate']").val().replace(/[\$\(\),]/g, '');            //Labor Rate
            //var LmuAmt = $(row).find("[id$='txtLabPrice']").val().replace(/[\$\(\),]/g, '');         //Labor Price$
            var Lmu = $(row).find("[id$='txtLabMarkup']").val().replace(/[\$\(\),]/g, '');            //Labor Markup
            var lbtax = $(row).find("[id$='chkLabSalestax']").is(":checked");                                  //Labor taxable

            //var lbext = $(row).find("[id$='lblLabExt']").text().replace(/[\$\(\),]/g, '');      //Labor Ext 
            //var hdLbExt = $(row).find("[id$='hdnLabExt']").val().replace(/[\$\(\),]/g, '');       //hidden Labor Ext
            //var matext = $(row).find("[id$='lblMatExt']").text().replace(/[\$\(\),]/g, '');     //Material Ext
            var hmatext = $(row).find("[id$='hdnMatExt']").val().replace(/[\$\(\),]/g, '');       //hidden material ext
            //var totalext = $(row).find("[id$='lblTotalExt']").text().replace(/[\$\(\),]/g, '');       //Total Ext

            var NHr = 0;
            var NRate = 0;
            var NTotalExt = 0;
            var NStax = 0;
            var NLmu = 0;
            var NLPrice = 0;
            var NLbExt = 0;
            var NBgtExt = 0;
            var stax = "";
            if (Lmu != '' && Lmu != 'undefined') {
                NLmu = parseFloat(Lmu);
            }
            if (hr != '' && rate != '' && hr != 'undefined' && rate != 'undefined') {
                NLbExt = parseFloat(hr) * parseFloat(rate);
            }
            if (hmatext != '') {
                NBgtExt = parseFloat(hmatext);
            }

            NTotalExt = NBgtExt + NLbExt;

            if (lbtax) {

                if (stax != "" && typeof stax != 'undefined' && stax != "0") {
                    NStax = parseFloat(stax);
                    NLPrice = (NLbExt + (NLbExt * (NLmu / 100))) * (NStax / 100); //((Labor EXT + (Labor Ext * LMU%) ) * sales tax%)
                } else {
                    NLPrice = (NLbExt + (NLbExt * (NLmu / 100)));            //((Labor EXT + (Labor Ext * LMU%) ))
                }
            }
            else {
                NLPrice = (NLbExt + (NLbExt * (NLmu / 100)));                //((Labor EXT + (Labor Ext * LMU%) ))
            }

            $(row).find("[id$='lblLabExt']").text((NLbExt).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='hdnLabExt']").val((NLbExt).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='lblTotalExt']").text((NTotalExt).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='txtLabPrice']").val((NLPrice).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='hdnLbChk']").val(lbtax); //Labor taxable

            if (NLbExt == 0) {
                $(row).find("[id$='txtLabMarkup']").val((0).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            }
            //UpdateProfit();
            CalculateEstimateBOM();

            var isTaxUpdating = $(obj).find("[id$='chkLabSalestax']").length == 0;
            UpdateProfit(isOverrideProfitPer, isTaxUpdating);
        }

        function CalculateLbOnly(obj) {
            var row = obj.parentNode.parentNode;
            var hr = $(row).find("[id$='txtHours']").val().replace(/[\$\(\),]/g, '');              //Labor Hour
            var rate = $(row).find("[id$='txtLabRate']").val().replace(/[\$\(\),]/g, '');            //Labor Rate
            var Lmu = $(row).find("[id$='txtLabMarkup']").val().replace(/[\$\(\),]/g, '');            //Labor Markup
            var lbtax = $(row).find("[id$='chkLabSalestax']").is(":checked");                                  //Labor taxable
            var hmatext = $(row).find("[id$='hdnMatExt']").val().replace(/[\$\(\),]/g, '');       //hidden material ext
            var NTotalExt = 0;
            var NStax = 0;
            var NLmu = 0;
            var NLPrice = 0;
            var NLbExt = 0;
            var NBgtExt = 0;
            var stax = "";
            if (Lmu != '' && Lmu != 'undefined') {
                NLmu = parseFloat(Lmu);
            }
            if (hr != '' && rate != '' && hr != 'undefined' && rate != 'undefined') {
                NLbExt = parseFloat(hr) * parseFloat(rate);
            }
            if (hmatext != '') {
                NBgtExt = parseFloat(hmatext);
            }

            NTotalExt = NBgtExt + NLbExt;

            if (lbtax) {

                if (stax != "" && typeof stax != 'undefined' && stax != "0") {
                    NStax = parseFloat(stax);
                    NLPrice = (NLbExt + (NLbExt * (NLmu / 100))) * (NStax / 100); //((Labor EXT + (Labor Ext * LMU%) ) * sales tax%)
                } else {
                    NLPrice = (NLbExt + (NLbExt * (NLmu / 100)));            //((Labor EXT + (Labor Ext * LMU%) ))
                }
            }
            else {
                NLPrice = (NLbExt + (NLbExt * (NLmu / 100)));                //((Labor EXT + (Labor Ext * LMU%) ))
            }

            $(row).find("[id$='lblLabExt']").text((NLbExt).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='hdnLabExt']").val((NLbExt).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='lblTotalExt']").text((NTotalExt).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='txtLabPrice']").val((NLPrice).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='hdnLbChk']").val(lbtax); //Labor taxable
            if (NLbExt == 0) {
                $(row).find("[id$='txtLabMarkup']").val((0).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            }
        }

        function CalculateMat(obj, isOverrideProfitPer) {

            debugger;
            var row = obj.parentNode.parentNode;
            //var rowIndex = row.rowIndex - 1;

            var qty = $(row).find("[id$='txtQtyReq']").val().replace(/[\$\(\),]/g, '');            //Qty Required
            var bgtunit = $(row).find("[id$='txtBudgetUnit']").val().replace(/[\$\(\),]/g, '');        //Budget Unit$
            //var mmuAmt = $(row).find("[id$='txtMatPrice']").val().replace(/[\$\(\),]/g, '');        //Material Price$

            var mmu = $(row).find("[id$='txtMatMarkup']").val().replace(/[\$\(\),]/g, '');           //Material Markup
            var mattax = $(row).find("[id$='chkMatSalestax']").is(":checked");        //Material taxable

            var lbext = $(row).find("[id$='lblLabExt']").text().replace(/[\$\(\),]/g, '');      //Labor Ext 

             
            if (mmu == "") {
                mmu = "0";
            }

            var NBgtExt = 0;
            var NLbExt = 0;
            var NTotalExt = 0;
            var NStax = 0;
            var NMmu = parseFloat(mmu);
            var NMPrice = 0;
            var stax = "";
            if (qty != '' && bgtunit != '') {
                NBgtExt = parseFloat(qty) * parseFloat(bgtunit);
            }
            if (lbext != '') {
                NLbExt = parseFloat(lbext);
            }
            NTotalExt = NBgtExt + NLbExt;

            if (mattax) {

                if (stax != "" && typeof stax != 'undefined' && stax != "0") {
                    NStax = parseFloat(stax);
                    NMPrice = (NBgtExt + (NBgtExt * (NMmu / 100))) * (NStax / 100); //((Mat EXT + (Mat Ext * MMU%) ) * sales tax%)
                } else {
                    NMPrice = (NBgtExt + (NBgtExt * (NMmu / 100)));            //((Mat EXT + (Mat Ext * MMU%) ))
                }
            }
            else {
                NMPrice = (NBgtExt + (NBgtExt * (NMmu / 100)));                //((Mat EXT + (Mat Ext * MMU%) ))
            }

            $(row).find("[id$='lblMatExt']").text((NBgtExt).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='hdnMatExt']").val((NBgtExt).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='lblTotalExt']").text((NTotalExt).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            console.log('txtMatPrice' + NMPrice);
            $(row).find("[id$='txtMatPrice']").val((NMPrice).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='hdnMatChk']").val(mattax); //Mat taxable

            CalculateEstimateBOM();
            var isTaxUpdating = $(obj).find("[id$='chkMatSalestax']").length == 0;
            UpdateProfit(isOverrideProfitPer, isTaxUpdating);
            TxtCalculateProfit();
        }


        function CalculateMaterialMarkupPer(obj, isOverrideProfitPer) {

            var row = obj.parentNode.parentNode; 

            var qty = row.cells[9].getElementsByTagName("input")[0].value.toString().replace(/[\$\(\),]/g, '');            //Qty Required
            var bgtunit = row.cells[11].getElementsByTagName("input")[0].value.toString().replace(/[\$\(\),]/g, '');        //Budget Unit$
            var mmuAmt = row.cells[14].getElementsByTagName("input")[0].value.toString().replace(/[\$\(\),]/g, '');        //Material Markup Price$

            //Calculate the Material Markup %
            var NBgtExt = parseFloat(qty) * parseFloat(bgtunit);
             


            var per = 0;
            if (NBgtExt != 0) {
                per = (mmuAmt - NBgtExt) * 100 / NBgtExt;
            }
            if (!isNaN(per)) {
                row.cells[15].getElementsByTagName("input")[0].value = parseFloat(per).toFixed(2);
            }

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value.toString().replace(/[\$\(\),]/g, '')).toLocaleString("en-US", { minimumFractionDigits: 2 });
            }
            //}
            //UpdateProfit();

            CalculateEstimateBOM();

            //Update % Profit and 
            UpdateProfit(isOverrideProfitPer);
        }

        function CalculateLaborMarkupPer(obj, isOverrideProfitPer) {
            var row = obj.parentNode.parentNode;
            var hr = row.cells[20].getElementsByTagName("input")[0].value.toString().replace(/[\$\(\),]/g, '');              //Labor Hour
            var rate = row.cells[21].getElementsByTagName("input")[0].value.toString().replace(/[\$\(\),]/g, '');            //Labor Rate
            var LmuAmt = row.cells[25].getElementsByTagName("input")[0].value.toString().replace(/[\$\(\),]/g, '');         //Labor Price$

            //Calculate the Material Markup %
            var NBgtExt = parseFloat(hr) * parseFloat(rate);


            var per = 0;
            if (NBgtExt != 0) {
                per = (LmuAmt - NBgtExt) * 100 / NBgtExt;
            }
            if (!isNaN(per)) {

                row.cells[26].getElementsByTagName("input")[0].value = parseFloat(per).toFixed(2);//per.toLocaleString("en-US", { minimumFractionDigits: 2 });
            }

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value.toString().replace(/[\$\(\),]/g, '')).toLocaleString("en-US", { minimumFractionDigits: 2 })
            }
            //}
            //UpdateProfit();
            CalculateEstimateBOM();
            UpdateProfit(isOverrideProfitPer);
        }

        //Update % Profit, Profit Amount and Bid Price
        function UpdateProfit(isOverrideProfitPer, isOverrideProfitPerOnAddnew = true) {

            var strTotalCost = GetNumberFromStringFormated($("#hd_totalcost").val());

            var totalCost = parseFloat(strTotalCost);
            var strPretax = GetNumberFromStringFormated($("#hd_pretax").val());//$('#hd_pretax').val(tPretax);

            var strComPer = $("[id*=txtPercentAgeCommission]:input").val();
            var comPer = parseFloat(strComPer);
            //var total = parseFloat(strTotal);
            var tMarkup = 0;
            var saleTaxableValue = 0;
            $("[id$='gvBOM_GridData'] tbody").find("tr").each(function () {
                var $tr = $(this);

                if ($tr.find('select[id*=ddlBType]').attr('id') != "" && typeof $tr.find('select[id*=ddlBType]').attr('id') != 'undefined') {
                    var type = $tr.find('select[id*=ddlBType]').val().replace(/[\$\(\),]/g, '');

                    if (!isNaN(parseInt(type))) {
                        //Calculate total markup 
                        var markup;
                        if ($tr.find('input[id*=txtMatPrice]').attr('id') != "" && typeof $tr.find('input[id*=txtMatPrice]').attr('id') != 'undefined') {
                            markup = $tr.find('input[id*=txtMatPrice]').val().replace(/[\$\(\),]/g, '');

                            if (!isNaN(parseFloat(markup))) {
                                tMarkup += parseFloat(markup);
                            }
                        }
                        if ($tr.find('input[id*=txtLabPrice]').attr('id') != "" && typeof $tr.find('input[id*=txtLabPrice]').attr('id') != 'undefined') {
                            markup = $tr.find('input[id*=txtLabPrice]').val().replace(/[\$\(\),]/g, '');

                            if (!isNaN(parseFloat(markup))) {
                                tMarkup += parseFloat(markup);
                            }
                        }

                        //Adding the Values for Sales Tax
                        var materialQty = $tr.find('input[id*=txtQtyReq]').val().replace(",", "");
                        if (materialQty == "") {
                            materialQty = "0";
                        }
                        var materialUnit = $tr.find('input[id*=txtBudgetUnit]').val().replace(",", "");
                        if (materialUnit == "") {
                            materialUnit = "0";
                        }
                        //
                        var materialMatPrice = $tr.find('input[id*=txtMatPrice]').val().replace(",", "");
                        if (materialMatPrice == "") {
                            materialMatPrice = "0";
                        } else {
                            materialMatPrice = GetNumberFromStringFormated(materialMatPrice);
                        }

                        var laborHours = $tr.find('input[id*=txtHours]').val();
                        if (laborHours == "") {
                            laborHours = "0";
                        }
                        var laborRate = $tr.find('input[id*=txtLabRate]').val();
                        if (laborRate == "") {
                            laborRate = "0";
                        }
                        var laborLabPrice = $tr.find('input[id*=txtLabPrice]').val();
                        if (laborLabPrice == "") {
                            laborLabPrice = "0";
                        } else {
                            laborLabPrice = GetNumberFromStringFormated(laborLabPrice);
                        }

                        var materialTaxable = 0;
                        var laborTaxable = 0;
                        var chkMatSalestax = $tr.find('input[id*=chkMatSalestax]').is(":checked");
                        if (chkMatSalestax == true) {

                            materialTaxable = materialQty * materialUnit;
                        }

                        var chkLabSalestax = $tr.find('input[id*=chkLabSalestax]').is(":checked");
                        if (chkLabSalestax == true) {

                            laborTaxable = laborHours * laborRate;
                        }
                        saleTaxableValue = saleTaxableValue + materialTaxable + laborTaxable;
                    }
                }
            });

            console.log('tMarkup' + tMarkup);
            //Calculate the Sales Tax
            var _saleTaxText = $("[id*=drpSaleTax] option:selected").text();
            var saleTax = 0;
            if (_saleTaxText == "Select Sales Tax") {
                saleTax = 0;
            }
            else {
                var saleTaxVal = _saleTaxText.split('/');
                saleTax = saleTaxVal[saleTaxVal.length - 1];
            }

            //var profit = tMarkup - totalCost;
            var materialexp = $('#hd_materialexp').val();
            var laborexp = $('#hd_laborexp').val();
            var otherexp = $('#hd_otherexp').val();
            var totalexp = parseFloat(GetNumberFromStringFormated(materialexp))
                + parseFloat(GetNumberFromStringFormated(laborexp))
                + parseFloat(GetNumberFromStringFormated(otherexp));
            
            var strMarkupPer = $("#ctl00_ContentPlaceHolder1_txtMarkupPercentAge").val();
            //var isOverrideProfitPer = $("#<%=hdnEstimateMode.ClientID%>").val() == 'Edit' && strMarkupPer != '';
            var isEditCopyEstimate = $("#<%=hdnEstimateMode.ClientID%>").val() != '' && $("#<%=hdnEstimateMode.ClientID%>").val() != 'Add';
            var profit = 0;
            var perProfit = 0;

            if (isEditCopyEstimate) {
                if (strMarkupPer != '') {
                    if (isOverrideProfitPer == true) {
                        profit = tMarkup - totalexp;
                        if (totalCost != 0) {
                            perProfit = profit * 100 / totalCost;
                        }
                    } else {
                        perProfit = parseFloat(strMarkupPer);
                        profit = perProfit * totalCost / 100;
                    }
                } else {
                    profit = tMarkup - totalexp;
                    if (totalCost != 0) {
                        perProfit = profit * 100 / totalCost;
                    }
                }
            } else {
                var defMarkupPer = $("#<%=hdnDefMarkupPer.ClientID%>").val();
                if (isOverrideProfitPerOnAddnew == true) {
                    if (strMarkupPer != '' && defMarkupPer != null && defMarkupPer != '') {
                        // Check if we have default profit% from template
                        var pardefMarkupPer = parseFloat(defMarkupPer);
                        var parUserMarkupPer = parseFloat(strMarkupPer);
                        if (pardefMarkupPer == parUserMarkupPer) {

                            perProfit = pardefMarkupPer;
                            profit = perProfit * totalCost / 100;
                        }
                        else {
                            profit = tMarkup - totalexp;
                            if (totalCost != 0) {
                                perProfit = profit * 100 / totalCost;
                            }
                        }
                    } else {
                        profit = tMarkup - totalexp;
                        if (totalCost != 0) {
                            perProfit = profit * 100 / totalCost;
                        }
                    }
                } else {
                    if (strMarkupPer != '') {
                        perProfit = parseFloat(strMarkupPer);
                        profit = perProfit * totalCost / 100;
                    } else {
                        if (defMarkupPer != null && defMarkupPer != '') {
                            var pardefMarkupPer = parseFloat(defMarkupPer);
                            perProfit = pardefMarkupPer;
                            profit = perProfit * totalCost / 100;
                        } else {
                            profit = tMarkup - totalexp;
                            if (totalCost != 0) {
                                perProfit = profit * 100 / totalCost;
                            }
                        }
                    }
                }
            }



            var tPretax = totalCost + profit;
            var tCommission = tPretax * comPer / 100;
            var tOh = parseFloat($('#hd_oh').val());
            var tcont = parseFloat(GetNumberFromStringFormated($('.cont').val()));

            var tTaxable = tOh + profit + tcont + saleTaxableValue;
            var tNonTaxable = tPretax - tTaxable;
            if (tNonTaxable.toFixed(2) == 0) {// to fixed error ($0.00)
                tNonTaxable = 0;
            }

            var stax_salesTax = parseFloat(tTaxable) * parseFloat(saleTax) / 100;
            var _stax_salesTax = stax_salesTax.toFixed(2);
            var tTotal = tCommission + stax_salesTax + tPretax;
            // Update ui 




            $("#hd_pretax").val(tPretax);
            $('#hd_salestax').val(stax_salesTax);
            $("#hd_commission").val(tCommission);
            $("#hd_total").val(tTotal);
            $('.pretax').html(cleanUpCurrency("$" + parseFloat(tPretax.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('.nontaxable').html(cleanUpCurrency("$" + parseFloat(tNonTaxable.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('.taxable').html(cleanUpCurrency("$" + parseFloat(tTaxable.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('.stax').html(cleanUpCurrency("$" + parseFloat(_stax_salesTax).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $("[id*=txtCommission]:input").val(cleanUpCurrency("$" + parseFloat(tCommission.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('.total').html(cleanUpCurrency("$" + parseFloat(tTotal.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $("[id*=lblBidPrice]").html(cleanUpCurrency("$" + parseFloat(tTotal.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            $('#<%=hdnBidPrice.ClientID%>').val(tTotal);
           
            
            if (isOverrideProfitPer == true) { NKCalProfit(totalCost, false, true); }
        }

        function TxtCalculateProfit() {
            debugger;             
            var ProfitPer = $("#ctl00_ContentPlaceHolder1_txtMarkupPercentAge").val();
            var oldProfitPer = $("#hd_markupper").val();           
            if (ProfitPer != oldProfitPer)
            {
                if (isNaN(ProfitPer)) { ProfitPer = 0; }
                var tsub = $('#hd_subtotal').val();
                var tOh = $("#hd_oh").val();
                var TotalCost = parseFloat(tsub) + parseFloat(tOh);
                if (ProfitPer != '' & ProfitPer != '0') { 
                    debugger; 
                    NKCalProfit(TotalCost, true, false)
                }
                else {

                    $('.markup').html("$" + '0'); 
                    $('#hd_markup').val('0');
                    NKCalProfit(TotalCost, false, true);
                }
            }
        }

        function NKCalProfit(TotalCost, IsProfitPer, isMarkupPer) {
            debugger;
            var SellPrice = 0.0;
            var ProfitAmount = 0.0;
            var ProfitPer = 0.0;
            if (isMarkupPer) {

                SellPrice = $("#hd_total").val();
                ProfitAmount = SellPrice - TotalCost;
                ProfitAmount = ProfitAmount.toFixed(2);
                ProfitPer = (ProfitAmount / SellPrice) * 100;
                ProfitPer = ProfitPer.toFixed(2);
                $("#ctl00_ContentPlaceHolder1_txtMarkupPercentAge").val(ProfitPer);
                $("#hd_markupper").val(ProfitPer);
            }
            else if (IsProfitPer) {
                ProfitPer = $("#ctl00_ContentPlaceHolder1_txtMarkupPercentAge").val();
                var nkper = 1 - (ProfitPer / 100);
                if (nkper == 0) { nkper = 1; }
                nkper = nkper.toFixed(2);
                SellPrice = TotalCost / nkper;
                SellPrice = SellPrice.toFixed(2);
                $("#hd_total").val(SellPrice); 
                $("[id*=lblBidPrice]").html("$" + SellPrice);
                $('#<%=hdnBidPrice.ClientID%>').val(SellPrice);
                ProfitAmount = SellPrice - TotalCost;
                ProfitAmount = ProfitAmount.toFixed(2);
            }


            $('.markup').html("$" + ProfitAmount);

            $('#hd_markup').val(ProfitAmount);

            //////////////////

        }

        function CalculateOHPer(val) {
            var subTotal = $('#hd_subtotal').val();
            if (val == "1") {
                var ohper = $("[id*=txtHOPercentAge]:input").val();
                // check 
                var finalCal = 0;
                if (ohper == "") {
                    var def_ohper = $("#<%=hdnDefOHPer.ClientID%>").val();
                    if (def_ohper != "") {
                        ohper = def_ohper;
                    } else {
                        ohper = "0";
                    }
                }

                finalCal = parseFloat(ohper) * parseFloat(subTotal) / 100;
                var formatOH = parseFloat(finalCal).toLocaleString("en-US", { minimumFractionDigits: 2 });
                //$("[id*=txtOH]").val("$" + formatOH);
                $("[id*=txtHOPercentAge]:input").val(ohper);
                $("[id*=txtOH]").val(cleanUpCurrency("$" + formatOH));

                $('#hd_oh').val(finalCal);

            }
            else {
                var ohVal = $("[id*=txtOH]:input").val().replace(",", "");
                if (ohVal == "") {
                    ohVal = "0";
                }
                var foH = parseFloat(ohVal) + parseFloat(subTotal);
                var per = (foH - subTotal) * 100 / subTotal;
                var formatVal = parseFloat(per.toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 });
                $("[id*=txtHOPercentAge]:input").val(formatVal);
                $("[id*=txtOH]").val(cleanUpCurrency("$" + ohVal));
                $('#hd_oh').val(ohVal);
            }
            //debugger

        }

        function CalculateContiPer(val) {
            try {
                var materialEx = $('#hd_materialexp').val();
                var laborEx = $('#hd_laborexp').val();
                var otherEx = $('#hd_otherexp').val();
                var subTotal = parseFloat(materialEx) + parseFloat(laborEx) + parseFloat(otherEx);
                if (val == "1") {
                    //if (subTotal > 0) {
                    var contper = $("[id*=txtPerContingencies]:input").val();
                    if (contper) {
                        var finalCal = parseFloat(contper) * parseFloat(subTotal) / 100;
                        var formatOH = parseFloat(finalCal).toLocaleString("en-US", { minimumFractionDigits: 2 });
                        //$("[id*=txtContingencies]").val("$" + formatOH);
                        $("[id*=txtContingencies]:input").val(cleanUpCurrency("$" + formatOH));
                    }
                    //}
                }
                else {
                    //
                    //var ohCont = $("[id*=txtContingencies]").val().replace(/[\$\(\),]/g, '');
                    var ohCont = GetNumberFromStringFormated($("[id*=txtContingencies]:input").val());
                    var foC = parseFloat(ohCont) + parseFloat(subTotal);
                    var per = (foC - subTotal) * 100 / subTotal;
                    var formatVal = parseFloat(per).toLocaleString("en-US", { minimumFractionDigits: 2 });
                    $("[id*=txtPerContingencies]:input").val(formatVal);
                }
            } catch (e) {
                //console.log("Error: " + e.mes)
            }
        }

        function UpdateBillingFinalBidPrice() {
            var bidPrice = 0;
            //var finalBidPrice = $("[id*=txtOverride]:input.override-amt").val().replace(/[\$\(\),]/g, '');
            var finalBidPrice = GetNumberFromStringFormated($("[id*=txtOverride]:input.override-amt").val());
            //if (finalBidPrice == "0.00" || finalBidPrice == "") {
            if (finalBidPrice == "" || finalBidPrice == "0.00") {
                //var bidpr = $("#<%=lblBidPrice.ClientID %>").val().replace(/[\$\(\),]/g, '');
                //var bidpr = GetNumberFromStringFormated($("#<%=lblBidPrice.ClientID %>").val());
                var bidpr = GetNumberFromStringFormated($("#<%=hdnBidPrice.ClientID %>").val());
                bidPrice = bidpr;
            }
            else {
                bidPrice = finalBidPrice;
            }

            var salesTax = GetNumberFromStringFormated($('#hd_salestax').val());
            if (salesTax == null || salesTax == '') {
                salesTax = 0;
            }

            var estType = $("#<%=ddlEstimateType.ClientID%>").val();
            if (estType == 'bid') {
                var totalBillingAmount = 0;
                $("[id$='gvMilestones_GridData']").find('tbody tr').each(function () {
                    //debugger
                    var $tr = $(this);
                    if ($tr.find('input[id*=txtPerAmount]').attr('id') != "" && typeof $tr.find('input[id*=txtPerAmount]').attr('id') != 'undefined') {
                        var per = $tr.find('input[id*=txtPerAmount]').val().replace(/[\$\(\),]/g, '');
                        if (bidPrice != "") {
                            var value = (per * (bidPrice - salesTax)) / 100;
                            var finalValue = value.toFixed(2);
                            var formatValue = parseFloat(finalValue.replace(/[\$\(\),]/g, '')).toLocaleString("en-US", { minimumFractionDigits: 2 });
                            if ($tr.find('input[id*=txtAmount]').attr('id') != "" && typeof $tr.find('input[id*=txtAmount]').attr('id') != 'undefined') {
                                $tr.find('input[id*=txtAmount]').val(formatValue);
                            }
                            totalBillingAmount += value;
                        }
                    }

                });
                $('[id*=lblTotalBillAmt]').text(totalBillingAmount.toFixed(2));
            } else {

                $("[id$='gvMilestones_GridData']").find('tbody tr').not('.rgNoRecords').each(function () {
                    var $tr = $(this);

                    var txtPrice = $tr.find('input[id*=txtPrice]').val();
                    var txtQuantity = $tr.find('input[id*=txtQuantity]').val();

                    if (txtPrice != "" && txtQuantity != "") {
                        var value = txtPrice * txtQuantity;
                        $tr.find('input[id*=txtAmount]').val(value);
                    }
                    else {
                        var txtAmount = $tr.find('input[id*=txtAmount]').val();
                        if (txtAmount != "") {
                            $tr.find('input[id*=txtPrice]').val(txtAmount);
                            $tr.find('input[id*=txtQuantity]').val(1);
                        }
                    }
                });
            }
            CalculateEstimateMilestone();
        }



        function CalculateCommission(val) {
            var subTotal = $('#hd_subtotal').val();
            var ohVal = $("#hd_oh").val();
            var markup = $('#hd_markup').val();
            var salesTax = $('#hd_salestax').val();

            if (val == "1") {

                var comPer = GetNumberFromStringFormated($("[id*=txtPercentAgeCommission]:input").val());
                if (comPer == "") {
                    var def_comPer = $("#<%=hdnDefCOMMSPer.ClientID%>").val();
                    if (def_comPer != "") {
                        comPer = def_comPer;
                    } else {
                        comPer = "0";
                    }
                }
                var finalCal = parseFloat(comPer) * (parseFloat(subTotal) + parseFloat(ohVal) + parseFloat(markup) + parseFloat(salesTax)) / 100;

                var formatVal = parseFloat(finalCal).toLocaleString("en-US", { minimumFractionDigits: 2 });
                $("[id*=txtPercentAgeCommission]").val(comPer);
                $("[id*=txtCommission]:input").val("$" + formatVal);

                $('#hd_commission').val(finalCal);
            }
            else {
                var comVal = GetNumberFromStringFormated($("[id*=txtCommission]:input").val());
                if (ohVal == "") {
                    ohVal = "0";
                }
                if (comVal == "") {
                    comVal = "0";
                }

                var fCom = parseFloat(ohVal) + parseFloat(subTotal) + parseFloat(markup) + parseFloat(comVal) + parseFloat(salesTax);

                var sub = parseFloat(ohVal) + parseFloat(subTotal) + parseFloat(markup) + parseFloat(salesTax);

                var per = (fCom - sub) * 100 / sub;
                var formatVal = parseFloat(per).toLocaleString("en-US", { minimumFractionDigits: 2 });
                $("[id*=txtPercentAgeCommission]").val(formatVal);
                $("[id*=txtCommission]:input").val("$" + comVal);
                $('#hd_commission').val(comVal);
            }
        }

        function fillDesc(obj) {
            var ddl = $(obj).attr('id');
            var val = $(obj).find("option:selected").text();
            document.getElementById(ddl.replace('ddlMatItem', 'txtScope')).value = val;
        }

        function isInt(value) {
            var x = parseFloat(value);
            return !isNaN(value) && (x | 0) === x;
        }

        function CalculatePercentage(gridview) {

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
                        var value = values.join("=");
                        parms[key] = decodeURIComponent(value.replace(/\+/g, '%20'));
                    }
                    return (parms);
                }
            });
        })(jQuery);

        (function ($) {
            $.fn.serializeFormJSON = function () {
                var o = [];
                $(this).find('tr:not(:first, :last)').each(function () {
                    var elements = $(this).find('input, textarea, select, checkbox')
                    if (elements.size() > 0) {
                        var serialized = $(this).find('input, textarea, select').serialize();
                        var item = $.toDictionary(serialized);
                        o.push(item);
                    }
                });
                return o;
            };
        })(jQuery);

        function DelRow(Gridview) {
            if ($("#" + Gridview).find('input[type="checkbox"]:checked').length == 0) {
                //alert('Please select items to delete.');
                noty({ text: 'Please select items to delete.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return;
            }
            var con = confirm('Are you sure you want to delete the items?');
            if (con == true) {
                $("#" + Gridview).find('tr').each(function () {
                    var $tr = $(this);
                    $tr.find('input[type="checkbox"]:checked').each(function () {
                        if ($("#" + Gridview).find('tr').length > 3) {
                            $(this).closest('tr').remove();
                        }
                        else {
                            $(this).closest('tr').find('input:text').val('');
                        }
                    });
                });
            }
        }

        function itemJSON() {
            debugger

            var rawData = $('#<%=gvBOM.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);

            $('#<%=hdnItemJSON.ClientID%>').val(formData);

            var rawMileData = $('#<%=gvMilestones.ClientID%>').serializeFormJSON();
            var formMileData = JSON.stringify(rawMileData);

            $('#<%=hdnMilestone.ClientID%>').val(formMileData);

            var rawCustomData = $('#<%=RadGrid_EstTags.ClientID%>').serializeFormJSON();
            var formCustomData = JSON.stringify(rawCustomData);
            $('#<%=hdnCustomJSON.ClientID%>').val(formCustomData);
        }

        function AddTemplate(dropdown) {

            if (dropdown.selectedIndex == 0) {
                return false;
            }
            else {

                itemJSON();
                __doPostBack(dropdown.id, '');
            }
        }

        function CheckAddRowGrid(sender, args) {

            itemJSON();
        }

        function ActiveLocationName(sender, args) {
            $("[id$='Label5']").addClass("active");
        }

    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            console.log("$(document).ready 2");
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
            }


            $("[id*=chkMaterial]").click(function () {

                var isChecked = $(this).is(":checked");

                var thMatPart = $(".MatPart");
                if (!isChecked) {
                    thMatPart.addClass("display-none");
                }
                else {
                    thMatPart.removeClass("display-none");
                }

                if (isChecked) {
                    $("[id*=hdnChkMat]").val('1');
                }
                else {
                    $("[id*=hdnChkMat]").val('0');
                }

                var isCheckedLb = $("[id*=chkLabor]").is(":checked");
                var qtyRequired = $(".qtyRequired");
                if (!isChecked || !isCheckedLb) {
                    qtyRequired.addClass("display-none");
                }
                else if (isChecked && isCheckedLb) {
                    qtyRequired.removeClass("display-none");
                }
            });

            $("[id*=chkLabor]").click(function () {

                var isChecked = $(this).is(":checked");
                var thLabPart = $(".LabPart");
                if (!isChecked) {
                    thLabPart.addClass("display-none");
                }
                else {
                    thLabPart.removeClass("display-none");
                }
                if (isChecked) {
                    $("[id*=hdnChkLb]").val('1');
                }
                else {
                    $("[id*=hdnChkLb]").val('0');
                }

                var isCheckedMat = $("[id*=chkMaterial]").is(":checked");
                var qtyRequired = $(".qtyRequired");
                if (!isChecked || !isCheckedMat) {
                    qtyRequired.addClass("display-none");
                }
                else if (isChecked && isCheckedMat) {
                    qtyRequired.removeClass("display-none");
                }
            });



            $(function () {
                // On page load reset BOM Material Item by checkbox value //////////////////////
                var isCheckedMat = $("[id*=chkMaterial]").is(":checked");
                var thMatPart = $(".MatPart");
                if (!isCheckedMat) {
                    thMatPart.addClass("display-none");
                }
                else {
                    thMatPart.removeClass("display-none");
                }

                // On page load reset BOM Labor Item by checkbox value //////////////////////
                var isCheckedLb = $("[id*=chkLabor]").is(":checked");
                var thLabPart = $(".LabPart");
                if (!isCheckedLb) {
                    thLabPart.addClass("display-none");
                }
                else {
                    thLabPart.removeClass("display-none");
                }

                var qtyRequired = $(".qtyRequired");
                if (!isCheckedMat || !isCheckedLb) {
                    qtyRequired.addClass("display-none");
                }
                else if (isCheckedMat && isCheckedLb) {
                    qtyRequired.removeClass("display-none");
                }



                //
                $("[id*=ddlCurrencyEst]").change(function () {
                    //var ddlCurrencyEst = $(this);
                    var strCurrencyEst = $(this).val();

                    //var ddlCurrencyEst_id = $(ddlCurrencyEst).attr('id');
                    $("[id*=ddlCurrencyEstbind]").val(strCurrencyEst);
                });
                $("[id*=ddlCurrencyEstbind]").change(function () {
                    //var ddlCurrencyEst = $(this);
                    var strCurrencyEst = $(this).val();

                    //var ddlCurrencyEst_id = $(ddlCurrencyEst).attr('id');
                    $("[id*=ddlCurrencyEst]").val(strCurrencyEst);
                });
            });
        });




        function BindAddress(RolId) {
            $.ajax({
                type: "POST",
                url: "CustomerAuto.asmx/GetEstimateRoleSpecificDetails",
                data: '{RoleId: "' + RolId + '"}',
                //data: '{searchTerm: "", column: "", page:"' + pageindex + '",stdate:"",enddate:"",Department:"' + tabindex + '"}',

                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: OnSuccess,
                failure: function (response) {
                    //alert(response.d);
                    $('#iframeloading').hide();


                },
                error: function (response) {
                    $('#iframeloading').hide();



                }
            });
            Materialize.updateTextFields();
        }
        function OnSuccess(response) {
            var result = response.d.ReponseObject;
            <%--$('#<%=ddlContact.ClientID%>').html('');--%>
            if (response.d.Header.HasError == false) {
                $("#<%=hdnLocID.ClientID%>").val(result.LocId);


            }


            <%--var _id = $('#<%=ddlContact.ClientID%>').val();--%>
            <%--var _estimateNo = $('#<%=TxtEstimateNo.ClientID%>').val();
            BindPhoneContactInfo(_id, _estimateNo);--%>
            Materialize.updateTextFields();
        }


        function OnCustomerAndEmployeeNameSuccess(response) {
            var result = response.d.ReponseObject;

            if (response.d.Header.HasError === false) {

                $("#<%=txtCompany.ClientID%>").val(result.CustomerName);
                var index = 0;
                $("[id$='ddlEmployees'] option").each(function () {
                    var ddlEmpValue = $("[id$='ddlEmployees'] option").eq(index).val();
                    if (ddlEmpValue === result.EmployeeValue) {
                        $("[id$='ddlEmployees'] option[value='" + result.EmployeeValue + "']").attr('selected', 'selected');
                    }
                    index++;
                });

                Materialize.updateTextFields();
            }
        }


        function OnLocationNameSuccess(response) {
            var result = response.d.ReponseObject;

            if (response.d.Header.HasError === false) {

                $("#<%=hdnLocID.ClientID%>").val(result.LocId);
                $("#<%=hdnROLId.ClientID%>").val(result.RolId);
                $("#<%=txtCont.ClientID%>").val(result.Location);

                var checkUniqueRow = result.CheckUniqueRow;

                if (checkUniqueRow === "0") {

                    $("#<%=txtEmail.ClientID%>").val("");
                    $("#<%=txtPhone.ClientID%>").val("");
                    $("#<%=txtCellNew.ClientID%>").val("");
                    $("#<%=txtFax.ClientID%>").val("");
                    $("[id$='ddlEmployees'] option[value='0']").attr('selected', 'selected');
                }

                Materialize.updateTextFields();
            }
        }



        function BindAddressAutoComplete(RolId) {
            $.ajax({
                type: "POST",
                url: "CustomerAuto.asmx/GetEstimateRoleSpecificDetails",
                data: '{RoleId: "' + RolId + '"}',
                //data: '{searchTerm: "", column: "", page:"' + pageindex + '",stdate:"",enddate:"",Department:"' + tabindex + '"}',

                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: OnSuccessBindAddressAutoComplete,
                failure: function (response) {
                    //alert(response.d);
                    $('#iframeloading').hide();
                },
                error: function (response) {
                    $('#iframeloading').hide();
                }
            });
            Materialize.updateTextFields();
        }

        function OnLocationByIdSuccess(response) {
            var result = response.d.ReponseObject;

            if (response.d.Header.HasError === false) {

                $("#<%=txtCompany.ClientID%>").val(result.CustomerName);
                var index = 0;
                $("[id$='ddlEmployees'] option").each(function () {
                    var ddlEmpValue = $("[id$='ddlEmployees'] option").eq(index).val();
                    if (ddlEmpValue === result.EmployeeValue) {
                        $("[id$='ddlEmployees'] option[value='" + result.EmployeeValue + "']").attr('selected', 'selected');
                    }
                    index++;
                });

                Materialize.updateTextFields();
            }
        }
        //End Contact and Address Ajax Calls function
        function calTotalPrice(obj) {

            var objId = document.getElementById(obj.id);
            var currentVal = objId.value;
            var txtTotalPriceObject, txtHdnTotalPrice;
            var TotalPrice;
            if (objId.value) {
                var isPercentage = obj.id.search("txtPercntge");
                if (isPercentage != -1) {

                    var totalAmount = document.getElementById(obj.id.replace('txtPercntge', 'txtBudgetUnit')).textContent | document.getElementById(obj.id.replace('txtPercntge', 'txtBudgetUnit')).innerHTML;
                    //var totalAmount = document.getElementById(obj.id.replace('txtPercntge', 'lblBudgetExt')).textContent | document.getElementById(obj.id.replace('txtPercntge', 'lblBudgetExt')).innerHTML;
                    TotalPrice = (totalAmount * currentVal) / 100 + totalAmount;
                    TotalPrice = TotalPrice.toFixed(2);
                    txtTotalPriceObject = document.getElementById(obj.id.replace('txtPercntge', 'lblTotalPrice'));
                    txtHdnTotalPrice = document.getElementById(obj.id.replace('txtPercntge', 'hdnTotalPrice'));
                    var amountTxtBox = document.getElementById(obj.id.replace('txtPercntge', 'txtAmt'));
                    amountTxtBox.disabled = true;
                }
                else {
                    var totalAmount = document.getElementById(obj.id.replace('txtAmt', 'lblBudgetExt')).textContent | document.getElementById(obj.id.replace('txtAmt', 'lblBudgetExt')).innerHTML;
                    TotalPrice = parseInt(currentVal) + parseInt(totalAmount);
                    TotalPrice = TotalPrice.toFixed(2);
                    txtTotalPriceObject = document.getElementById(obj.id.replace('txtAmt', 'lblTotalPrice'));
                    txtHdnTotalPrice = document.getElementById(obj.id.replace('txtAmt', 'hdnTotalPrice'));
                    var percentageTxtBox = document.getElementById(obj.id.replace('txtAmt', 'txtPercntge'));
                    percentageTxtBox.disabled = true;
                }
                $(txtTotalPriceObject).text(TotalPrice);
                $(txtHdnTotalPrice).val(TotalPrice);
            }
            else {
                var isPercentage = obj.id.search("txtPercntge");
                if (isPercentage == -1) {
                    txtTotalPriceObject = document.getElementById(obj.id.replace('txtAmt', 'lblTotalPrice'));
                    txtHdnTotalPrice = document.getElementById(obj.id.replace('txtAmt', 'hdnTotalPrice'));
                }
                else {
                    txtTotalPriceObject = document.getElementById(obj.id.replace('txtPercntge', 'lblTotalPrice'));
                    txtHdnTotalPrice = document.getElementById(obj.id.replace('txtPercntge', 'hdnTotalPrice'));
                }
                $(txtTotalPriceObject).text(" ");
                $(txtHdnTotalPrice).val(" ");
                document.getElementById(obj.id.replace('txtPercntge', 'txtAmt')).disabled = false;
                document.getElementById(obj.id.replace('txtAmt', 'txtPercntge')).disabled = false;
            }

        }

        function OtherCategory() {

            var cat = $('[id*=drpCategory] option:selected').text();

            if (cat == 'Other') {
                $('[id*=txtCategory]').css("display", "block");
            }
            else {
                $('[id*=txtCategory]').css("display", "none");
            }
        }

        $(document).ready(function () {
            console.log("$(document).ready 3");

            $('#<%=txtREPdesc.ClientID%>').change(function () {
                var istxtOppNameDisabled = $('#<%=txtOppName.ClientID%>').prop('disabled');
                if (istxtOppNameDisabled == false) {
                    var valData = $('#<%=txtREPdesc.ClientID%>').val();
                    $('#<%=txtOppName.ClientID%>').val(valData);
                    Materialize.updateTextFields();
                }
            });


            var cat = $('[id*=drpCategory] option:selected').text();

            if (cat == 'Other') {
                $('[id*=txtCategory]').css("display", "block");
            }
            else {
                $('[id*=txtCategory]').css("display", "none");
            }

            var _rrol = $("#<%=hdnROLId.ClientID%>").val();
            if (isNaN(_rrol) == false && _rrol != "") {
                BindAddress(_rrol);
                <%--$("#<%=lnkAddnew.ClientID%>").css("display", "block");--%>
            }
            else {
                <%--$("#<%=lnkAddnew.ClientID%>").css("display", "none");--%>
            }

            ///////////// Quick Codes //////////////
            $("#<%=txtREPdesc.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtREPdesc.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });
            $("#<%=txtREPremarks.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtREPremarks.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });

            // Show hide discount notes
            ShowHideDiscountNotes();
            // Show equipment gridview
            $("#eqtag").click(function () {
                $("#DivEqup").show();
            });
        });

        //Bom part
        function RowContextMenuBomGrid(sender, eventArgs) {
            var menu = $find("<%=RadMenuBomGrid.ClientID %>");
            var evt = eventArgs.get_domEvent();

            //if (evt.target.tagName === "INPUT" || evt.target.tagName === "A") {
            //    return;
            //}

            var index = eventArgs.get_itemIndexHierarchical();
            $("[id$='radGridClickedRowIndex']").val(index);

            sender.get_masterTableView().selectItem(sender.get_masterTableView().get_dataItems()[index].get_element(), true);

            menu.show(evt);

            evt.cancelBubble = true;
            evt.returnValue = false;

            if (evt.stopPropagation) {
                evt.stopPropagation();
                evt.preventDefault();
            }
        }
        //Milestones part
        function RowContextMenuMilesonteGrid(sender, eventArgs) {
            var menu = $find("<%=RadMenuMilGrid.ClientID %>");
            var evt = eventArgs.get_domEvent();

            //if (evt.target.tagName === "INPUT" || evt.target.tagName === "A") {
            //    return;
            //}

            var index = eventArgs.get_itemIndexHierarchical();
            $("[id$='radMilGridClickedRowIndex']").val(index);

            sender.get_masterTableView().selectItem(sender.get_masterTableView().get_dataItems()[index].get_element(), true);

            menu.show(evt);

            evt.cancelBubble = true;
            evt.returnValue = false;

            if (evt.stopPropagation) {
                evt.stopPropagation();
                evt.preventDefault();
            }
        }

        //function select all checkbox
        function selectAllCheckbox(control, text) {
            var chkSelectAll = $(control);
            var grid = chkSelectAll.closest(".BomGrid");
            if (text === 'BomItem' || text === 'MilItem') {
                var chkbItem = grid.find(".chkSelect input");
                chkbItem.prop('checked', $("[id$='chkSelectAll']").is(':checked'));
            }
            else if (text === 'MatTx') {
                var chkbMat = grid.find(".chkMatSalestax input");
                var isMatTxCheckAll = $("[id$='chkSelectAllMatTx']").is(':checked');
                //;
                chkbMat.each(function () {
                    var isCheck = $(this).is(':checked');
                    if (isCheck != isMatTxCheckAll) {
                        $(this).prop('checked', isMatTxCheckAll);
                        chkMatSalestaxChange(this);
                    }
                });

                CalculateEstimateBOM();
                UpdateProfit(false, false);
                UpdateBillingFinalBidPrice();
            }
            else if (text === 'LaborTx') {
                var chkbLabor = grid.find(".chkLabSalestax input");
                var isLaborTxCheckAll = $("[id$='chkSelectAllLaborTx']").is(':checked');
                //;
                chkbLabor.each(function () {
                    var isCheck = $(this).is(':checked');
                    if (isCheck != isLaborTxCheckAll) {
                        $(this).prop('checked', isLaborTxCheckAll);
                        chkLaborTxChange(this);
                    }
                });
                CalculateEstimateBOM();
                UpdateProfit(false, false);
                UpdateBillingFinalBidPrice();
            }

        }

        function chkMatSalestaxChange(obj) {
            var row = obj.parentNode.parentNode.parentNode;
            var qty = $(row).find("[id$='txtQtyReq']").val().replace(/[\$\(\),]/g, '');                 //Qty Required
            var bgtunit = $(row).find("[id$='txtBudgetUnit']").val().replace(/[\$\(\),]/g, '');         //Budget Unit$
            var mmu = $(row).find("[id$='txtMatMarkup']").val().replace(/[\$\(\),]/g, '');              //Material Markup
            var mattax = $(row).find("[id$='chkMatSalestax']").is(":checked");                          //Material taxable
            var lbext = $(row).find("[id$='lblLabExt']").text().replace(/[\$\(\),]/g, '');              //Labor Ext 
            if (mmu == "") {
                mmu = "0";
            }

            var NBgtExt = 0;
            var NLbExt = 0;
            var NTotalExt = 0;
            var NStax = 0;
            var NMmu = parseFloat(mmu);
            var NMPrice = 0;
            var stax = "";
            if (qty != '' && bgtunit != '') {
                NBgtExt = parseFloat(qty) * parseFloat(bgtunit);
            }
            if (lbext != '') {
                NLbExt = parseFloat(lbext);
            }
            NTotalExt = NBgtExt + NLbExt;

            if (mattax) {

                if (stax != "" && typeof stax != 'undefined' && stax != "0") {
                    NStax = parseFloat(stax);
                    NMPrice = (NBgtExt + (NBgtExt * (NMmu / 100))) * (NStax / 100); //((Mat EXT + (Mat Ext * MMU%) ) * sales tax%)
                } else {
                    NMPrice = (NBgtExt + (NBgtExt * (NMmu / 100)));                 //((Mat EXT + (Mat Ext * MMU%) ))
                }
            }
            else {
                NMPrice = (NBgtExt + (NBgtExt * (NMmu / 100)));                     //((Mat EXT + (Mat Ext * MMU%) ))
            }

            $(row).find("[id$='lblMatExt']").text(NBgtExt.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='hdnMatExt']").val(NBgtExt.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='lblTotalExt']").text(NTotalExt.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='txtMatPrice']").val(NMPrice.toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='hdnMatChk']").val(mattax); //Mat taxable
        }

        function chkLaborTxChange(obj) {
            //
            var row = obj.parentNode.parentNode.parentNode;
            var hr = $(row).find("[id$='txtHours']").val().replace(/[\$\(\),]/g, '');              //Labor Hour
            var rate = $(row).find("[id$='txtLabRate']").val().replace(/[\$\(\),]/g, '');            //Labor Rate
            var Lmu = $(row).find("[id$='txtLabMarkup']").val().replace(/[\$\(\),]/g, '');            //Labor Markup
            var lbtax = $(row).find("[id$='chkLabSalestax']").is(":checked");                                  //Labor taxable
            var hmatext = $(row).find("[id$='hdnMatExt']").val().replace(/[\$\(\),]/g, '');       //hidden material ext

            var NHr = 0;
            var NRate = 0;
            var NTotalExt = 0;
            var NStax = 0;
            var NLmu = 0;
            var NLPrice = 0;
            var NLbExt = 0;
            var NBgtExt = 0;
            var stax = "";
            if (Lmu != '' && Lmu != 'undefined') {
                NLmu = parseFloat(Lmu);
            }
            if (hr != '' && rate != '' && hr != 'undefined' && rate != 'undefined') {
                NLbExt = parseFloat(hr) * parseFloat(rate);
            }
            if (hmatext != '') {
                NBgtExt = parseFloat(hmatext);
            }

            NTotalExt = NBgtExt + NLbExt;

            if (lbtax) {

                if (stax != "" && typeof stax != 'undefined' && stax != "0") {
                    NStax = parseFloat(stax);
                    NLPrice = (NLbExt + (NLbExt * (NLmu / 100))) * (NStax / 100); //((Labor EXT + (Labor Ext * LMU%) ) * sales tax%)
                } else {
                    NLPrice = (NLbExt + (NLbExt * (NLmu / 100)));            //((Labor EXT + (Labor Ext * LMU%) ))
                }
            }
            else {
                NLPrice = (NLbExt + (NLbExt * (NLmu / 100)));                //((Labor EXT + (Labor Ext * LMU%) ))
            }

            $(row).find("[id$='lblLabExt']").text((NLbExt).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='hdnLabExt']").val((NLbExt).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='lblTotalExt']").text((NTotalExt).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='txtLabPrice']").val((NLPrice).toLocaleString("en-US", { minimumFractionDigits: 2 }));
            $(row).find("[id$='hdnLbChk']").val(lbtax); //Labor taxable
            //CalculateEstimateBOM();
            //UpdateProfit();
        }

        function HideME() {
            $("#DivEqup").hide();
        }

        function SelectRowsEq1() {
            var Name = document.getElementById("<%=txtUnit.ClientID %>");
            var div = document.getElementById('eqtag');
            div.innerHTML = '';
            Name.value = '';

            var grid = $find("<%=RadgvEquip.ClientID %>");
            var masterTable = grid.get_masterTableView();
            if (masterTable != null) {
                for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                    var gridItemElement = masterTable.get_dataItems()[i].findElement("chkSelect");
                    var lblUnit = masterTable.get_dataItems()[i].findElement("lblUnit");
                    var lblID = masterTable.get_dataItems()[i].findElement("lblID");
                    if (gridItemElement.checked) {
                        if (Name.value != '') {
                            Name.value = Name.value + ', ' + lblUnit.innerHTML;
                        }
                        else {
                            Name.value = lblUnit.innerHTML;
                        }

                        var tag = "<div class='chip' style='width:auto !important;padding-left:5px !important;padding-right:5px !important ;margin-left:2px !important ;margin-right:2px !important ;margin-top:3px !important ;'><a href='addequipment.aspx?uid=" + lblID.innerHTML + "' target='_blank' style='color:white'>" + lblUnit.innerHTML + "</a></div>"

                        div.innerHTML += tag;
                    }
                }
            }
        }


        function EqCheckBOX(checked) {
            var grid = $find("<%=RadgvEquip.ClientID %>");
            var masterTable = grid.get_masterTableView();
            if (masterTable != null) {
                for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                    var gridItemElement = masterTable.get_dataItems()[i].findElement("chkSelect");
                    var lblUnit = masterTable.get_dataItems()[i].findElement("lblUnit");
                    gridItemElement.checked = checked;
                }
            }
        }

        function CloseRadWindowGroup() {
            var valName = document.getElementById("<%=rfvGroupName.ClientID%>");
            ValidatorEnable(valName, false);
            var wnd = $find('<%=RadWindowGroup.ClientID %>');
            wnd.Close();
        }

        function OpenRadWindowGroup() {
            var groupID = $('#<%=ddlEstimateGroup.ClientID%>').val();
            if (groupID == "0") {
                $('#<%=txtGroupName.ClientID%>').val("");
                $('#<%=hdnGroupUpdateMode.ClientID%>').val("0"); // Addnew mode

                var wnd = $find('<%=RadWindowGroup.ClientID %>');
                wnd.set_title("Add Estimate Group Name");
                wnd.Show();
            }
            else {
                $('#<%=hdnGroupUpdateMode.ClientID%>').val("1"); // Edit mode
                var groupName = $('#<%=ddlEstimateGroup.ClientID%> option:selected').text();

                $('#<%=txtGroupName.ClientID%>').val(groupName);

                var wnd = $find('<%=RadWindowGroup.ClientID %>');
                wnd.set_title("Edit Estimate Group Name");
                wnd.Show();
            }
            Materialize.updateTextFields();
        }

        function EnablePopupValidation() {
            var valName = document.getElementById("<%=rfvGroupName.ClientID%>");
            ValidatorEnable(valName, true);
        }

        function CloseRadWindowLaborRate() {
            var valName = document.getElementById("<%=rfvBOMLaborRate.ClientID%>");
            ValidatorEnable(valName, false);
            var wnd = $find('<%=RadWindowBOMLaborRate.ClientID %>');
            wnd.Close();
        }

        function OpenRadWindowLaborRate(e) {
            //alert($(e).val());
            var txtCurrentLaborRate = $(e).val();
            $('#<%=txtPopupLaborRate.ClientID%>').val(txtCurrentLaborRate);
            var wnd = $find('<%=RadWindowBOMLaborRate.ClientID %>');
            wnd.set_title('Updating Labor Rate');
            wnd.Show();
            Materialize.updateTextFields();
        }

        function lnkAllLaborRate_Yes_ClientClick() {
            var valName = document.getElementById("<%=rfvBOMLaborRate.ClientID%>");
            ValidatorEnable(valName, true);
            var newLaborRate = $('#<%=txtPopupLaborRate.ClientID%>').val();
            $("[id*=txtLabRate].txtLabRate").each(function (index, item) {
                var txtLabRateId = $(item).attr('id');
                var txtHoursId = txtLabRateId.replace('txtLabRate', 'txtHours');
                var hoursVal = $('#' + txtHoursId).val();
                if (!isNaN(parseFloat(hoursVal)) && parseFloat(hoursVal) != 0) {
                    $(item).val(newLaborRate);
                    CalculateLbOnly(item);
                }
            });

            CalculateEstimateBOM();
            UpdateProfit();

            CloseRadWindowLaborRate();
        }

        function lnkAllLaborRate_No_ClientClick() {
            CloseRadWindowLaborRate();
        }

        function OnGridCreated(sender, args) {
            var frozenScroll = $get(sender.get_id() + "_Frozen");
            var allColumns = sender.get_masterTableView().get_columns();
            var scrollLeftOffset = 0;
            var allColumnsWidth = new Array;
            var grid = sender.get_element();
            for (var i = 0; i < allColumns.length; i++) {
                allColumnsWidth[i] = allColumns[i].get_element().offsetWidth;
            }

            $get(sender.get_id() + "_GridData").onscroll = function (e) {
                for (var i = 0; i < allColumns.length; i++) {
                    if (!allColumns[i].get_visible()) {
                        scrollLeftOffset += allColumnsWidth[i];
                    }
                    if ($telerik.isIE7) {
                        var thisColumn = grid.getElementsByTagName("colgroup")[0].getElementsByTagName("col")[i];
                        if (thisColumn.style.display == "none") {
                            scrollLeftOffset += parseInt(thisColumn.style.width);
                        }
                    }
                }
                var thisScrollLeft = this.scrollLeft;
                if (frozenScroll != null) {
                    if (thisScrollLeft > 0)
                        frozenScroll.scrollLeft = thisScrollLeft + scrollLeftOffset + 300;
                    this.scrollLeft = 0;
                }

                scrollLeftOffset = 0;
            };
        }

        function headerMenuShowing(sender, args) {
            var menu = args.get_menu();

            for (var i = 0; i < menu.get_items().get_count(); i++) {
                var item = menu.get_items().getItem(i);
                if (item.get_value() != 'ColumnsContainer') {
                    item.get_element().style.display = 'none';
                }
            }

            var columnsItem = menu.findItemByText("Columns");
            columnsItem.get_items().getItem(0).get_element().style.display = "none";
            columnsItem.get_items().getItem(1).get_element().style.display = "none";
            columnsItem.get_items().getItem(2).get_element().style.display = "none";
        }

        function DeleteContactClick(hyperlink) {
            var IsDelete = document.getElementById('<%= hdnDeleteContact.ClientID%>').value;
            if (IsDelete == "Y") {
                return SelectedRowDelete('<%= RadGrid_Contacts.ClientID%>', 'contact');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
    </script>

    <script type="text/javascript">
        function BindClickEventForGridCheckBox() {
            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', function () {
                CheckUncheckAllCheckBoxAsNeeded();
            });

            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkTask']:checkbox").unbind('click').bind('click', function () {
                OnCheck_TaskCheckBox('<%=RadGrid_Emails.ClientID%>');
            });

            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                if ($(this).is(':checked')) {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                }
                else {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>');
            });

            UpdatedivDisplayTeamMember();
            var line = $("#<%= hdnLineOpenned.ClientID%>").val();
            if (line != null && line != '') {
                var hdnMembersID = $(".txtMembers_" + line).attr("id").replace("txtMembers", "hdnMembers");
                var teamMembers = $("#" + hdnMembersID).val();

                // Update selected for grid
                if (teamMembers != null && teamMembers != "") {
                    var teamArr = teamMembers.toString().split(';');
                    // trim value of teamArr
                    $.each(teamArr, function (index, value) {
                        teamArr[index] = value.trim();
                    });

                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                        var userId = $(this).closest('tr').find('td:eq(0)').find('span').html();
                        var taskCheckID = this.id.replace("chkSelect", "chkTask");
                        var intUserType = userId.split('_')[0];

                        if (teamArr.indexOf(userId) >= 0) {
                            $(this).prop('checked', true);
                            if (intUserType == 0 || intUserType == 1 || intUserType == 6) {
                                $("#" + taskCheckID).prop('disabled', false);
                            }
                            else {
                                $("#" + taskCheckID).prop('disabled', true);
                            }
                        } else {
                            $(this).prop('checked', false);
                            $("#" + taskCheckID).prop('disabled', true);
                        }
                    });
                } else {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
            }
        }

        function UpdatedivDisplayTeamMember() {
            var txtMembers = $("#<%=RadGrid_EstTags.ClientID %> input[id*='txtMembers']");
            $.each(txtMembers, function (index, item) {
                var txtId = $(item).attr("id");
                var div = document.getElementById(txtId.replace("txtMembers", "cusLabelTag"));
                var hdnMembersID = txtId.replace("txtMembers", "hdnMembers");
                var hdnMembersValue = $("#" + hdnMembersID).val();
                div.innerHTML = '';
                var disTeamMembers = $(item).val();
                // Update selected for grid
                if (disTeamMembers != null && disTeamMembers != "") {
                    var teamArr = disTeamMembers.toString().split(';');
                    var teamKeyArr = hdnMembersValue.toString().split(';');
                    // trim value of teamArr
                    $.each(teamArr, function (index, value) {
                        teamArr[index] = value.trim();
                    });

                    if (teamArr != null && teamArr.length > 0) {
                        for (var i = 0; i < teamArr.length; i++) {
                            var tempTeamKeyArr = teamKeyArr[i].toString().split('_');
                            var tag = "";
                            if (teamKeyArr[i].indexOf('6') == 0) {
                                if (tempTeamKeyArr.length == 3 && tempTeamKeyArr[2] == "1") {
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                }
                                else
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                div.innerHTML += tag;
                            } else {
                                if (tempTeamKeyArr.length == 3 && (tempTeamKeyArr[0] == "0" || tempTeamKeyArr[0] == "1") && tempTeamKeyArr[2] == "1")
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";

                                div.innerHTML += tag;
                            }
                        }
                    }
                }
            });
        }

        function CloseTeamMemberWindow() {

            var line = $("#<%= hdnLineOpenned.ClientID%>").val();
            if (line != null && line != '') {
                var hdnMembersValue = $("#<%= hdnOrgMemberKey.ClientID%>").val();
                var txtMembersValue = $("#<%= hdnOrgMemberDisp.ClientID%>").val();

                var txtMembersID = $(".txtMembers_" + line).attr("id");
                $("#" + txtMembersID).val(txtMembersValue);
                var hdnMembersID = txtMembersID.replace("txtMembers", "hdnMembers");
                $("#" + hdnMembersID).val(hdnMembersValue);

                var div = document.getElementById(txtMembersID.replace("txtMembers", "cusLabelTag"));
                div.innerHTML = '';
                var disTeamMembers = txtMembersValue;
                // Update selected for grid
                if (disTeamMembers != null && disTeamMembers != "") {
                    var teamArr = disTeamMembers.toString().split(';');
                    var teamKeyArr = hdnMembersValue.toString().split(';');
                    // trim value of teamArr
                    $.each(teamArr, function (index, value) {
                        teamArr[index] = value.trim();
                    });

                    if (teamArr != null && teamArr.length > 0) {
                        for (var i = 0; i < teamArr.length; i++) {
                            var tempTeamKeyArr = teamKeyArr[i].toString().split('_');
                            var tag = "";
                            if (teamKeyArr[i].indexOf('6') == 0) {
                                if (tempTeamKeyArr.length == 3 && tempTeamKeyArr[2] == "1")
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                div.innerHTML += tag;
                            } else {
                                if (tempTeamKeyArr.length == 3 && (tempTeamKeyArr[0] == "0" || tempTeamKeyArr[0] == "1") && tempTeamKeyArr[2] == "1")
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                div.innerHTML += tag;
                            }
                        }
                    }
                }
            }
            var wnd = $find('<%=TeamMembersWindow.ClientID %>');
            wnd.Close();
        }

        function ShowTeamMemberWindow(txtTeamMember) {

            var line = $(txtTeamMember).closest('tr').find('td:eq(0) > span.customline').text();
            var txtTeamMembersId = $(txtTeamMember).attr("id");
            var hdnTeamMembersId = txtTeamMembersId.replace("cusLabelTag", "hdnMembers");
            var teamMembers = $("#" + hdnTeamMembersId).val();
            var txtCustomLabelId = txtTeamMembersId.replace("cusLabelTag", "lblDesc");
            var txtCustomLabelVal = $("#" + txtCustomLabelId).val();
            var txtTeamMemberDispId = txtTeamMembersId.replace("cusLabelTag", "txtMembers");
            var txtTeamMemberDispVal = $("#" + txtTeamMemberDispId).val();

            $('#<%= hdnLineOpenned.ClientID%>').val(line);
            $('#<%= hdnOrgMemberKey.ClientID%>').val(teamMembers);
            $('#<%= hdnOrgMemberDisp.ClientID%>').val(txtTeamMemberDispVal);

            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', function () {
                CheckUncheckAllCheckBoxAsNeeded();
            });

            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkTask']:checkbox").unbind('click').bind('click', function () {
                OnCheck_TaskCheckBox('<%=RadGrid_Emails.ClientID%>');
            });

            $("#<%=RadGrid_Emails.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                if ($(this).is(':checked')) {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                }
                else {
                    $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>');
            });

            // Update selected for grid
            if (teamMembers != null && teamMembers != "") {
                var teamArr = teamMembers.toString().split(';');
                var teamArrWithTask = teamMembers.toString().split(';');
                // trim value of teamArr
                $.each(teamArr, function (index, value) {
                    var temp = value.trim().split('_');

                    if (temp.length == 3) {
                        temp.splice(2, 1);
                        teamArr[index] = temp.join("_");
                    } else {
                        teamArr[index] = value.trim();
                    }

                });

                $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                    var userId = $(this).closest('tr').find('td:eq(0)').find('span').html();
                    var taskCheckID = this.id.replace("chkSelect", "chkTask");
                    var intUserType = userId.split('_')[0];

                    var idx = teamArr.indexOf(userId);
                    if (idx >= 0) {
                        $(this).prop('checked', true);
                        var memberkeywithTask = teamArrWithTask[idx].split('_');
                        if (memberkeywithTask.length == 3 && memberkeywithTask[2] == 1) {
                            $("#" + taskCheckID).prop('checked', true);
                        } else {
                            $("#" + taskCheckID).prop('checked', false);
                        }

                        if (intUserType == 0 || intUserType == 1 || intUserType == 6) {
                            $("#" + taskCheckID).prop('disabled', false);
                        }
                        else {
                            $("#" + taskCheckID).prop('disabled', true);
                        }
                    } else {
                        $(this).prop('checked', false);
                        $("#" + taskCheckID).prop('disabled', true);
                        $("#" + taskCheckID).prop('checked', false);
                    }
                });
            } else {
                $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                $("#<%=RadGrid_Emails.ClientID%> input[id*='chkTask']:checkbox").prop('checked', false);
                $("#<%=RadGrid_Emails.ClientID%> input[id*='chkTask']:checkbox").prop('disabled', true);
            }

            var wnd = $find('<%=TeamMembersWindow.ClientID %>');
            wnd.set_title("Team Member: " + txtCustomLabelVal);
            wnd.Show();
        }

        function CheckEmailsCheckBox(gridview) {

            var line = $("#<%= hdnLineOpenned.ClientID%>").val();
            var hdnMembersID = $(".txtMembers_" + line).attr("id").replace("txtMembers", "hdnMembers");
            var hdnMembersValue = $("#<%= hdnOrgMemberKey.ClientID%>").val();
            var txtMembersValue = $("#<%= hdnOrgMemberDisp.ClientID%>").val();

            var tempArrayKey = [];
            tempArrayKey.length = 0;
            var tempArrayDisplay = [];
            tempArrayDisplay.length = 0;

            $("#" + gridview + " input[id*='chkSelect']:checkbox").each(function (index) {
                var tempMemberKey = $(this).closest('tr').find('td:eq(0)').find('span').html().trim();
                var teamMemberDisp = $(this).closest('tr').find('td:eq(1)').find('span').html().trim();
                if (teamMemberDisp == "")
                    teamMemberDisp = $(this).closest('tr').find('td:eq(2)').find('span').html().trim();

                var intUserType = tempMemberKey.split('_')[0];

                var temp = tempMemberKey.trim().split('_');
                if (temp.length == 3) {
                    temp.splice(2, 1);
                }

                var taskCheckID = this.id.replace("chkSelect", "chkTask");

                if ($(this).is(":checked")) {
                    if (intUserType == 0 || intUserType == 1 || intUserType == 6) {
                        $("#" + taskCheckID).prop('disabled', false);
                    }
                    else {
                        $("#" + taskCheckID).prop('disabled', true);
                    }

                    if (jQuery.inArray(tempMemberKey, tempArrayKey) < 0) {
                        if ($("#" + taskCheckID).is(":checked")) {
                            temp.push(1);
                        } else {
                            temp.push(0);
                        }
                        tempMemberKey = temp.join("_")
                        tempArrayKey.push(tempMemberKey);
                        tempArrayDisplay.push(teamMemberDisp);
                    }
                } else {
                    if (jQuery.inArray(tempMemberKey, tempArrayKey) >= 0) {
                        tempArrayKey = jQuery.grep(tempArrayKey, function (value) {
                            return value != tempMemberKey;
                        });
                        tempArrayDisplay = jQuery.grep(tempArrayDisplay, function (value) {
                            return value != teamMemberDisp;
                        });
                    }
                    $("#" + taskCheckID).prop('checked', false);
                    $("#" + taskCheckID).prop('disabled', true);
                }
            });

            $("#<%= hdnOrgMemberKey.ClientID%>").val(tempArrayKey.join(";"));
            $("#<%= hdnOrgMemberDisp.ClientID%>").val(tempArrayDisplay.join(";"));
        }

        function CheckUncheckAllCheckBoxAsNeeded() {
            var totalCheckboxes = $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox").size();

            var checkedCheckboxes = $("#<%=RadGrid_Emails.ClientID%> input[id*='chkSelect']:checkbox:checked").size();

            if (totalCheckboxes == checkedCheckboxes) {

                $("#<%=RadGrid_Emails.ClientID %> input[id*='chkAll']:checkbox").prop('checked', true);//.each(function () { this.checked = true; });
            }
            else {
                $("#<%=RadGrid_Emails.ClientID %> input[id*='chkAll']:checkbox").prop('checked', false);//.attr('checked', false);
            }

            if ($('#<%=RadGrid_Emails.ClientID%>').length > 0) {
                CheckEmailsCheckBox('<%=RadGrid_Emails.ClientID%>');
            }
        }

        function OnCheck_TaskCheckBox(gridview) {
            var line = $("#<%= hdnLineOpenned.ClientID%>").val();
            var hdnMembersID = $(".txtMembers_" + line).attr("id").replace("txtMembers", "hdnMembers");
            var hdnMembersValue = $("#<%= hdnOrgMemberKey.ClientID%>").val();

            var tempArrayKey = [];
            tempArrayKey.length = 0;

            $("#" + gridview + " input[id*='chkSelect']:checkbox").each(function (index) {
                var tempMemberKey = $(this).closest('tr').find('td:eq(0)').find('span').html().trim();

                var temp = tempMemberKey.trim().split('_');
                if (temp.length == 3) {
                    temp.splice(2, 1);
                }

                var taskCheckID = this.id.replace("chkSelect", "chkTask");

                if ($(this).is(":checked")) {
                    if (jQuery.inArray(tempMemberKey, tempArrayKey) < 0) {
                        if ($("#" + taskCheckID).is(":checked")) {
                            temp.push(1);
                        } else {
                            temp.push(0);
                        }
                        tempMemberKey = temp.join("_")
                        tempArrayKey.push(tempMemberKey);
                    }
                } else {
                    if (jQuery.inArray(tempMemberKey, tempArrayKey) >= 0) {
                        tempArrayKey = jQuery.grep(tempArrayKey, function (value) {
                            return value != tempMemberKey;
                        });
                    }
                }
            });

            $("#<%= hdnOrgMemberKey.ClientID%>").val(tempArrayKey.join(";"));
        }

        function UndoConfirm() {
            var estimateId = $("#<%= hdnestimateid.ClientID%>").val();
            if (estimateId != null && estimateId != "") {
                var label = $("#<%= lnkUndoConvert.ClientID%>").html();
                if (label == 'Undo Convert')
                    return confirm('Are you sure you want to undo the convert for Estimate #' + estimateId + '?');
                else
                    return confirm('Are you sure you want to unlink the Project for this Estimate #' + estimateId + '? Please review the budgeted amounts under the Project.');

            }
            else {
                return false;
            }
        }

        function chkChangeOrder_onchange(control) {
            var chkChangeOrder = $(control).find("input[type='checkbox']")[0];
            if (chkChangeOrder != null) {
                var hdnChangeOrderChk = chkChangeOrder.id.replace("chkChangeOrder", "hdnChangeOrderChk");
                $("#" + hdnChangeOrderChk).val($(chkChangeOrder).prop("checked"));
            }
        }

        function chkBilFrmBOM_ChangeConfirm(obj) {
            <%--debugger
            var isChecked = $(obj).prop("checked");
            if (isChecked) {
                var a = confirm("This action will replace all your Billing Items by BOM Items?<br/>Are you sure you want to continue?");
                if (a) {
                    var rawData = $('#<%=gvBOM.ClientID%>').serializeFormJSON();
                    var formData = JSON.stringify(rawData);

                    $('#<%=hdnItemJSON.ClientID%>').val(formData);
                    setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder1$chkBilFrmBOM\',\'\')', 0)
                    setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder1$chkBilFrmBOM\',\'\')', 0)
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }--%>
            var isChecked = $(obj).prop("checked");
            var rawMileData = $('#<%=gvMilestones.ClientID%>').serializeFormJSON();
            var formMileData = JSON.stringify(rawMileData);
            $('#<%=hdnMilestone.ClientID%>').val(formMileData);
            if (isChecked) {
                return noty({
                    dismissQueue: true,
                    layout: 'topCenter',
                    theme: 'noty_theme_default',
                    animateOpen: { height: 'toggle' },
                    animateClose: { height: 'toggle' },
                    easing: 'swing',
                    text: 'This action will replace all your Billing Items by BOM Items?<br/>Are you sure you want to continue?',
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



                                //__doPostBack("ctl00$ContentPlaceHolder1$chkBilFrmBOM", "chkchkBilFrmBOMclick");
                                setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder1$chkBilFrmBOM\',\'\')', 0);
                                return true;
                            }
                        },
                        {
                            type: 'btn-danger', text: 'No', click: function ($noty) {

                                $noty.close();
                                return false;
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
            setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder1$chkBilFrmBOM\',\'\')', 0);
            return true;
        }

    </script>
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
    </script>
    <div id="overlay">
        <img src="images/wheel.GIF" alt="Be patient..." class="lodder" />
    </div>

    <telerik:RadAjaxManager ID="RadAjaxManager_Esimate" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnGenerate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowTemplate" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkRevisionSave">
                <UpdatedControls>
                    <%--  <telerik:AjaxUpdatedControl ControlID="RadGrid_Revision" />--%>
                    <telerik:AjaxUpdatedControl ControlID="RevisionNotes_1" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnSelectLoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdnLocID" />
                    <telerik:AjaxUpdatedControl ControlID="hdnROLId" />
                    <telerik:AjaxUpdatedControl ControlID="hdnOpportunity" />
                    <telerik:AjaxUpdatedControl ControlID="ddlEmployees" />
                    <telerik:AjaxUpdatedControl ControlID="ddlTemplate" />
                    <telerik:AjaxUpdatedControl ControlID="ddlEstimateGroup" />
                    <telerik:AjaxUpdatedControl ControlID="divLocationInfo" />
                    <telerik:AjaxUpdatedControl ControlID="panelCompany" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelRadgvEquip" />
                    <telerik:AjaxUpdatedControl ControlID="ddlOpportunity" />
                    <telerik:AjaxUpdatedControl ControlID="divOppName" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Contacts" />
                    <telerik:AjaxUpdatedControl ControlID="lnkAddnewContact" />
                    <telerik:AjaxUpdatedControl ControlID="btnEditContact" />
                    <telerik:AjaxUpdatedControl ControlID="btnDeleteContact" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSelectLocCus">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdnLocID" />
                    <telerik:AjaxUpdatedControl ControlID="hdnROLId" />
                    <telerik:AjaxUpdatedControl ControlID="hdnOpportunity" />
                    <telerik:AjaxUpdatedControl ControlID="ddlEmployees" />
                    <telerik:AjaxUpdatedControl ControlID="ddlTemplate" />
                    <telerik:AjaxUpdatedControl ControlID="ddlEstimateGroup" />
                    <telerik:AjaxUpdatedControl ControlID="divLocationInfo" />
                    <telerik:AjaxUpdatedControl ControlID="panelCompany" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelRadgvEquip" />
                    <telerik:AjaxUpdatedControl ControlID="ddlOpportunity" />
                    <telerik:AjaxUpdatedControl ControlID="divOppName" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Contacts" />
                    <telerik:AjaxUpdatedControl ControlID="lnkAddnewContact" />
                    <telerik:AjaxUpdatedControl ControlID="btnEditContact" />
                    <telerik:AjaxUpdatedControl ControlID="btnDeleteContact" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlTemplate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtOverride" />
                    <telerik:AjaxUpdatedControl ControlID="txtJobType" />
                    <telerik:AjaxUpdatedControl ControlID="txtHOPercentAge" />
                    <telerik:AjaxUpdatedControl ControlID="txtPercentAgeCommission" />
                    <telerik:AjaxUpdatedControl ControlID="txtMarkupPercentAge" />
                    <telerik:AjaxUpdatedControl ControlID="drpSaleTax" />
                    <telerik:AjaxUpdatedControl ControlID="txtContingencies" />
                    <telerik:AjaxUpdatedControl ControlID="txtOH" />
                    <telerik:AjaxUpdatedControl ControlID="txtCommission" />
                    <telerik:AjaxUpdatedControl ControlID="hdnDefOHPer" />
                    <telerik:AjaxUpdatedControl ControlID="hdnDefCOMMSPer" />
                    <telerik:AjaxUpdatedControl ControlID="hdnDefMarkupPer" />
                    <telerik:AjaxUpdatedControl ControlID="hdnDefSTaxName" />
                    <telerik:AjaxUpdatedControl ControlID="ddlEstimateType" />
                    <telerik:AjaxUpdatedControl ControlID="lblFinalBid" />
                    <telerik:AjaxUpdatedControl ControlID="chkSglBilAmt" />
                    <telerik:AjaxUpdatedControl ControlID="chkBilFrmBOM" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlOpportunity">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdnLocID" />
                    <telerik:AjaxUpdatedControl ControlID="hdnROLId" />
                    <telerik:AjaxUpdatedControl ControlID="hdnOpportunity" />
                    <telerik:AjaxUpdatedControl ControlID="txtOverride" />
                    <telerik:AjaxUpdatedControl ControlID="ddlTemplate" />
                    <telerik:AjaxUpdatedControl ControlID="ddlEmployees" />
                    <telerik:AjaxUpdatedControl ControlID="divOppName" />
                    <telerik:AjaxUpdatedControl ControlID="ddlOppStage" />
                    <telerik:AjaxUpdatedControl ControlID="divLocationInfo" />
                    <telerik:AjaxUpdatedControl ControlID="panelCompany" />
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelRadgvEquip" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="ddlEstimateGroup">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelRadgvEquip" />
                    <telerik:AjaxUpdatedControl ControlID="lnkAddGroupName" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkPopupUpdateGroup">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlEstimateGroup" />
                    <telerik:AjaxUpdatedControl ControlID="lnkAddGroupName" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSaveEstimate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkSaveEstimate" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkAddnewContact">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContact" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnEditContact">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowContact" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkContactSave">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Contacts" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnDeleteContact">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Contacts" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="chkShowAllTasks">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Tasks" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%--<telerik:AjaxSetting AjaxControlID="RadGrid_Project">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ProjectWindow" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
            <telerik:AjaxSetting AjaxControlID="btnSaveProject">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="trProj" />
                    <telerik:AjaxUpdatedControl ControlID="liLinkProject" />
                    <telerik:AjaxUpdatedControl ControlID="hdnLinkedProject" />
                    <telerik:AjaxUpdatedControl ControlID="lnkConvert" />
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" />
                    <telerik:AjaxUpdatedControl ControlID="txtBidDate" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" />
                    <telerik:AjaxUpdatedControl ControlID="lnkUndoConvert" />
                    <telerik:AjaxUpdatedControl ControlID="lnkSaveEstimate" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlEstimateType">
                <UpdatedControls>
                    <%--<telerik:AjaxUpdatedControl ControlID="gvMilestones"/>--%>
                    <telerik:AjaxUpdatedControl ControlID="txtOverride" />
                    <telerik:AjaxUpdatedControl ControlID="lblFinalBid" />
                    <telerik:AjaxUpdatedControl ControlID="txtJobType" />
                    <telerik:AjaxUpdatedControl ControlID="txtHOPercentAge" />
                    <telerik:AjaxUpdatedControl ControlID="txtPercentAgeCommission" />
                    <telerik:AjaxUpdatedControl ControlID="txtMarkupPercentAge" />
                    <telerik:AjaxUpdatedControl ControlID="drpSaleTax" />
                    <telerik:AjaxUpdatedControl ControlID="txtContingencies" />
                    <telerik:AjaxUpdatedControl ControlID="txtOH" />
                    <telerik:AjaxUpdatedControl ControlID="txtCommission" />
                    <telerik:AjaxUpdatedControl ControlID="hdnDefOHPer" />
                    <telerik:AjaxUpdatedControl ControlID="hdnDefCOMMSPer" />
                    <telerik:AjaxUpdatedControl ControlID="hdnDefMarkupPer" />
                    <telerik:AjaxUpdatedControl ControlID="hdnDefSTaxName" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkConvert">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="trProj" />
                    <telerik:AjaxUpdatedControl ControlID="liLinkProject" />
                    <telerik:AjaxUpdatedControl ControlID="lnkUndoConvert" />
                    <telerik:AjaxUpdatedControl ControlID="lnkSaveEstimate" />
                    <telerik:AjaxUpdatedControl ControlID="hdnLinkedProject" />
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" />
                    <telerik:AjaxUpdatedControl ControlID="txtBidDate" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkUndoConvert">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="trProj" />
                    <telerik:AjaxUpdatedControl ControlID="liLinkProject" />
                    <telerik:AjaxUpdatedControl ControlID="lnkConvert" />
                    <telerik:AjaxUpdatedControl ControlID="lnkSaveEstimate" />
                    <telerik:AjaxUpdatedControl ControlID="hdnLinkedProject" />
                    <telerik:AjaxUpdatedControl ControlID="ddlStatus" />
                    <telerik:AjaxUpdatedControl ControlID="txtBidDate" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Project" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvLogs" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkApplyApproveStatus">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ApprovedStatusHistory" />
                    <telerik:AjaxUpdatedControl ControlID="btnSendEmail" />
                    <telerik:AjaxUpdatedControl ControlID="ddlApprovalStatus" />
                    <telerik:AjaxUpdatedControl ControlID="pnlApproveStatusComment" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid_ApprovedStatusHistory">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel_Forms" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="chkBilFrmBOM">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlEstimateType" />
                    <telerik:AjaxUpdatedControl ControlID="hdnMilestone" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Estimate" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-communication-contacts"></i>&nbsp;
                                            <asp:Label ID="Label13" runat="server" Text="Add Estimate"></asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkSaveEstimate" runat="server" OnClick="lnkSaveEstimate_Click" OnClientClick="itemJSON(); return ValidBomMileStone();"
                                                CausesValidation="true" ValidationGroup="search">Save</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks" id="pnlReportButton" runat="server">
                                            <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dropdown1">Reports
                                            </a>
                                            <ul id="dropdown1" class="dropdown-content">
                                                <li>
                                                    <asp:LinkButton ID="lnkExportEstimateProfile" runat="server" OnClick="lnkExportEstimateProfile_Click"
                                                        CausesValidation="true" ValidationGroup="search">Estimate Profile</asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkConvert" runat="server" OnClick="lnkConvert_Click"
                                                OnClientClick="return confirm('Do you really want to convert this estimate to project?');"
                                                CausesValidation="true" ValidationGroup="search">Convert to Project</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkUndoConvert" runat="server" OnClick="lnkUndoConvert_Click"
                                                OnClientClick="return UndoConfirm();"
                                                CausesValidation="true" ValidationGroup="search" Text="Undo Convert"></asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkCloseEstimate" runat="server" CausesValidation="False" OnClick="lnkCloseEstimate_Click" ToolTip="Close"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    <div class="rght-content leftContend">
                                        <div class="editlabel" id="trProj" runat="server">
                                            <%-- Project&nbsp;<asp:HyperLink ID="lnkProject" runat="server" Target="_self"></asp:HyperLink>--%>
                                        </div>
                                        <div class="editlabel">
                                            <asp:Label runat="server" ID="lblHeaderLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>

                <div class="container breadcrumbs-bg-custom">
                    <div class="row">
                        <div class="col s12 m12 l12">
                            <div class="row">
                                <div class="tblnks">
                                    <ul class="anchor-links">
                                        <li><a href="#accrdestimate">Estimate Info</a></li>
                                        <li runat="server" id="liProposals"><a href="#accrdProposals">Proposals</a></li>
                                        <li runat="server" id="liContacts"><a href="#accrdcontacts">Contacts</a></li>
                                        <%--<li runat="server" id="liEstimateTags"><a href="#accrdEstimateTags">Custom</a></li>--%>
                                        <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                        <li id="liLinkProject" runat="server"><a href="#" onclick="ShowProjectWindow();">Link Project</a></li>
                                    </ul>
                                </div>
                                <div class="tblnksright">
                                    <div class="nextprev">
                                        <asp:Panel ID="pnlNext" runat="server">
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkFirst" ToolTip="First" OnClick="lnkFirst_Click" runat="server" CausesValidation="False">
                                                        <i class="fa fa-angle-double-left"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" OnClick="lnkPrevious_Click" runat="server" CausesValidation="False">
                                                        <i class="fa fa-angle-left"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkNext" ToolTip="Next" OnClick="lnkNext_Click" runat="server" CausesValidation="False">
                                                        <i class="fa fa-angle-right"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkLast" ToolTip="Last" OnClick="lnkLast_Click" runat="server" CssClass="icon-last" CausesValidation="False">
                                                        <i class="fa fa-angle-double-right"></i>
                                                </asp:LinkButton>
                                            </span>
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

                        <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                            <li>
                                <%--<telerik:RadAjaxPanel ID="RadAjaxPanelEstimateInfo" runat="server">--%>
                                <div id="accrdestimate" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-maps-map"></i>Estimate Info</div>
                                <div class="collapsible-body">
                                    <div class="form-content-wrap">
                                        <div class="form-content-pd">
                                            <div class="form-section-row">

                                                <div class="section-ttle">Estimate Info</div>
                                                <asp:HiddenField ID="hdnOpportunity" runat="server" />
                                                <asp:HiddenField ID="hdnItemJSON" runat="server" />
                                                <asp:HiddenField ID="hdnROLId" runat="server" />
                                                <asp:HiddenField ID="hdProspect" runat="server" />
                                                <asp:HiddenField ID="hdnLocID" runat="server" />
                                                <asp:HiddenField ID="hdnItemJSONPerc" runat="server" />
                                                <asp:HiddenField ID="hdnBOMItemJSON" runat="server" />
                                                <asp:HiddenField ID="hdnMilestone" runat="server" />
                                                <asp:HiddenField ID="hdnestimateid" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdStatus" runat="server" />
                                                <asp:HiddenField ID="hdnEstimateMode" runat="server" />
                                                <asp:HiddenField ID="hdnChangeTemplateConfirmStatus" runat="server" />
                                                <%--<asp:HiddenField ID="hdnSelectedTemplate" runat="server" />--%>
                                                <asp:HiddenField ID="hdnDefOHPer" runat="server" />
                                                <asp:HiddenField ID="hdnDefCOMMSPer" runat="server" />
                                                <asp:HiddenField ID="hdnDefMarkupPer" runat="server" />
                                                <asp:HiddenField ID="hdnDefSTaxName" runat="server" />
                                                <asp:HiddenField ID="hdnLinkedProject" runat="server" />
                                                <asp:HiddenField ID="hdnCustomerID" runat="server" />
                                                <%--<asp:HiddenField ID="hdnContactChange" runat="server" Value="0" />--%>
                                                <telerik:RadAjaxPanel runat="server">


                                                    <asp:Button CausesValidation="false" ID="btnSelectLoc" runat="server" Text="Button" Style="display: none;" OnClick="btnSelectLoc_Click" />
                                                    <telerik:RadButton CausesValidation="false" ID="btnSelectLocCus" runat="server" CssClass="display-none" OnClick="btnSelectCustomer_Click" OnClientClicked="ActiveLocationName" Text="Button"></telerik:RadButton>
                                                    <%--<asp:Button CausesValidation="false" ID="btnSelectCustomer" runat="server" Text="Button" Style="display: none;" OnClick="btnSelectCustomer_Click" />--%>
                                                </telerik:RadAjaxPanel>

                                                <div class="form-section3">
                                                    <div class="col s12 m12 l12">
                                                        <div class="row">
                                                            <div class="input-field col s3">
                                                                <div class="row">

                                                                    <asp:TextBox ID="TxtEstimateNo" runat="server" AutoCompleteType="None" ReadOnly="true" MaxLength="255"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lblEstimateNo" AssociatedControlID="TxtEstimateNo"> Estimate No</asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s1">
                                                                <div class="row">
                                                                    &nbsp;
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s8">
                                                                <div class="row">
                                                                    <asp:TextBox ID="TxtDate" runat="server" AutoCompleteType="None" autocomplete="off" CssClass="datepicker_mom" MaxLength="255"></asp:TextBox>
                                                                    <%-- <asp:CalendarExtender ID="TxtDate_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="TxtDate">
                                                                </asp:CalendarExtender>--%>
                                                                    <asp:RequiredFieldValidator ID="rfvDate"
                                                                        runat="server" ControlToValidate="TxtDate" Display="None" ErrorMessage="Date is Required" ValidationGroup="search"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceDateRf" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvDate" />
                                                                    <asp:RegularExpressionValidator ID="rfvDate1" ControlToValidate="TxtDate" ValidationGroup="search"
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="rfvDate1" />
                                                                    <asp:Label runat="server" ID="lblDate" AssociatedControlID="TxtDate">Estimate Date</asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:Label runat="server" ID="lblBidDate" AssociatedControlID="txtBidDate" CssClass="active">Bid Close Date</asp:Label>
                                                            <asp:TextBox ID="txtBidDate" runat="server" AutoCompleteType="None" autocomplete="off" CssClass="datepicker_mom" MaxLength="255"></asp:TextBox>
                                                            <%--  <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                                TargetControlID="txtBidDate">
                                                            </asp:CalendarExtender>--%>
                                                            <asp:RequiredFieldValidator ID="rfvBidDt1"
                                                                runat="server" ControlToValidate="txtBidDate" Display="None" ErrorMessage="Date is Required" ValidationGroup="search"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="vceBidDt1" runat="server" Enabled="True"
                                                                PopupPosition="Right" TargetControlID="rfvBidDt1" />
                                                            <asp:RegularExpressionValidator ID="revBidDt" ControlToValidate="txtBidDate" ValidationGroup="search"
                                                                ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                            </asp:RegularExpressionValidator>
                                                            <asp:ValidatorCalloutExtender ID="vceBidDt2" runat="server" Enabled="True" PopupPosition="Right"
                                                                TargetControlID="revBidDt" />

                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:Label runat="server" ID="Label3" AssociatedControlID="txtREPdesc">Description</asp:Label>
                                                            <asp:RequiredFieldValidator ID="rfvDesc" runat="server" ControlToValidate="txtREPdesc"
                                                                Display="None" ErrorMessage="Description Required" SetFocusOnError="True" ValidationGroup="search">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="vceDesc" runat="server" Enabled="True"
                                                                TargetControlID="rfvDesc">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:TextBox ID="txtREPdesc" runat="server" CssClass="materialize-textarea" MaxLength="255" AutoCompleteType="None" TextMode="MultiLine"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Category</label>
                                                            <asp:DropDownList ID="drpCategory" onchange="OtherCategory();" CssClass="browser-default" runat="server">
                                                            </asp:DropDownList>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="drpCategory"
                                                                Display="None" ErrorMessage="Category Required" SetFocusOnError="True" InitialValue="Select" ValidationGroup="search">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator1">
                                                            </asp:ValidatorCalloutExtender>

                                                        </div>
                                                        <div class="row">
                                                            <asp:TextBox runat="server" Style="display: none; margin-top: 5px;" ID="txtCategory"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Estimate Status</label>
                                                            <asp:DropDownList ID="ddlStatus" TabIndex="6" CssClass="browser-default" runat="server"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlStatus"
                                                                Display="None" ErrorMessage="Estimate Status is Required" SetFocusOnError="True" InitialValue="" ValidationGroup="search">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator5">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:Label runat="server" ID="lblSoldDate" AssociatedControlID="txtSoldDate" CssClass="active">Sold Date</asp:Label>
                                                            <asp:TextBox ID="txtSoldDate" runat="server" AutoCompleteType="None" autocomplete="off" CssClass="datepicker_mom" MaxLength="255"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div id="pnlApproveProposal" runat="server">
                                                        <div class="section-ttle">Approval</div>
                                                        <div class="input-field col s11">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Approval Status</label>
                                                                <asp:DropDownList ID="ddlApprovalStatus" CssClass="browser-default" runat="server" onchange="ApprovalStatusOnChange();">
                                                                    <asp:ListItem Value="0">Pending</asp:ListItem>
                                                                    <asp:ListItem Value="1">Approved</asp:ListItem>
                                                                    <asp:ListItem Value="2">Changes Required</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1 m-t-5">
                                                            <div class="row">
                                                                <div class="btnlinksicon">
                                                                    <asp:LinkButton ID="lnkLatestProposal" Visible="false" runat="server" Style="padding: 0px!important;" CausesValidation="False"
                                                                        OnClick="lnkLatestProposal_Click"><i class="mdi-file-file-download"></i></asp:LinkButton>
                                                                    <asp:Label ID="lblLatestProposalTooltip" runat="server" CssClass="EstimateTooltip"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row" id="pnlApproveStatusComment" runat="server">
                                                                <asp:Label runat="server" ID="Label22" AssociatedControlID="txtApproveStatusComment">Approved Status Comment</asp:Label>
                                                                <asp:TextBox ID="txtApproveStatusComment" runat="server" CssClass="materialize-textarea" MaxLength="255" AutoCompleteType="None" TextMode="MultiLine"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkApplyApproveStatus" OnClick="lnkApplyApproveStatus_Click" runat="server" Text="Apply"></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <%--<telerik:RadAjaxPanel runat="server">--%>

                                                    <div class="input-field col s12 p-0" id="divLocationInfo" runat="server">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCompanyName" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtCompanyName"
                                                                    Display="None" ErrorMessage="Customer Name Required" SetFocusOnError="True" ValidationGroup="search">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True"
                                                                    TargetControlID="RequiredFieldValidator4">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:Label runat="server" ID="Label4" AssociatedControlID="txtCompanyName">Customer Name</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="srchclr btnlinksicon rowbtn">
                                                            <asp:HyperLink for="txtCompanyName" ID="lnkCustomerID" Visible="true" Target="_blank" runat="server"><i class="mdi-social-people" style="margin-left:0px !important;"></i></asp:HyperLink>
                                                        </div>

                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCont" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCont"
                                                                    Display="None" ErrorMessage="Location Name Required" SetFocusOnError="True" ValidationGroup="search">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                                                    TargetControlID="RequiredFieldValidator2">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:Label runat="server" ID="Label5" AssociatedControlID="txtCont">Location Name</asp:Label>
                                                                <asp:HiddenField ID="hdnLocRolID" runat="server" />
                                                                <asp:Button ID="Button1" runat="server" CausesValidation="False" OnClick="btnSelectLoc_Click"
                                                                    Style="display: none;" Text="Button" />
                                                            </div>
                                                        </div>
                                                        <div class="srchclr btnlinksicon rowbtn">
                                                            <asp:HyperLink for="txtCont" ID="lnkLocationID" Visible="true" Target="_blank" runat="server"><i class="mdi-communication-location-on" style="margin-left:0px !important;"></i></asp:HyperLink>
                                                        </div>

                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <%--<label class="drpdwn-label">Contact  Name</label>--%>
                                                                <%--<asp:DropDownList ID="ddlContact" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                                <asp:HiddenField ID="hdContactSelected" runat="server" />--%>
                                                                <asp:Label runat="server" ID="Label21" AssociatedControlID="txtContact">Contact  Name</asp:Label>
                                                                <asp:TextBox ID="txtContact" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <%--<div class="input-field col s1" style="margin-top: 5px;">
                                                            <div class="row">
                                                                <div class="btnlinksicon">
                                                                    <asp:LinkButton ID="lnkAddnew" runat="server" Style="padding: 0px!important;" CausesValidation="False" ToolTip="Add New Contact" OnClick="lnkAddnew_Click"><i class="mdi-social-person-add"></i></asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>--%>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="255" AutoCompleteType="None"></asp:TextBox>
                                                                <asp:Label runat="server" ID="Label7" AssociatedControlID="txtEmail">Email</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="255" AutoCompleteType="None" TextMode="Phone"></asp:TextBox>
                                                                <asp:Label runat="server" ID="Label8" AssociatedControlID="txtPhone">Phone</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCellNew" runat="server" MaxLength="255" AutoCompleteType="None" TextMode="Phone"></asp:TextBox>
                                                                <asp:Label runat="server" ID="Label9" AssociatedControlID="txtCellNew">Cell / Mobile</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtFax" runat="server" MaxLength="255" AutoCompleteType="None" TextMode="Phone"></asp:TextBox>
                                                                <asp:Label runat="server" ID="Label10" AssociatedControlID="txtFax">Fax</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row row1 checkbox">
                                                            <%--<telerik:RadAjaxPanel runat="server">--%>
                                                            <asp:CheckBox ID="chkPayCertified" runat="server" CssClass="css-checkbox" Text="Certified Project" />
                                                            <%--</telerik:RadAjaxPanel>--%>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row checkbox">
                                                            <%--<telerik:RadAjaxPanel runat="server">--%>
                                                            <asp:CheckBox ID="chkDiscounted" runat="server" CssClass="css-checkbox" Text="Discounted"
                                                                OnClick="ShowHideDiscountNotes();" />
                                                            <%--</telerik:RadAjaxPanel>--%>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row" id="divDiscountedNotes">
                                                            <asp:Label runat="server" ID="Label19" AssociatedControlID="txtDiscountedNotes">Discounted Notes</asp:Label>
                                                            <asp:TextBox ID="txtDiscountedNotes" runat="server" CssClass="materialize-textarea" MaxLength="255" AutoCompleteType="None" TextMode="MultiLine"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvDiscountedNotes" runat="server" ControlToValidate="txtDiscountedNotes"
                                                                Display="None" ErrorMessage="Discounted Notes Required" SetFocusOnError="True" ValidationGroup="search">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" Enabled="True"
                                                                TargetControlID="rfvDiscountedNotes" PopupPosition="BottomLeft">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>
                                                    <%--    <div class="input-field col s12">
                                                        <div class="row" id="divComment">
                                                            <asp:Label runat="server" ID="Label23" AssociatedControlID="txtDiscountedNotes">Comment</asp:Label>
                                                            <asp:TextBox ID="txtComment" runat="server" CssClass="materialize-textarea" MaxLength="255" AutoCompleteType="None" TextMode="MultiLine"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtDiscountedNotes"
                                                                Display="None" ErrorMessage="Comment Required" SetFocusOnError="True" ValidationGroup="search">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender9" runat="server" Enabled="True"
                                                                TargetControlID="rfvDiscountedNotes" PopupPosition="BottomLeft">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>--%>



                                                    <%--</telerik:RadAjaxPanel>--%>
                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <asp:Panel runat="server" ID="panelCompany">
                                                        <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCompany" runat="server" Enabled="false"></asp:TextBox>
                                                                <asp:Label runat="server" ID="Label6" AssociatedControlID="txtCompany">Company</asp:Label>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Assigned To</label>
                                                            <asp:DropDownList ID="ddlEmployees" CssClass="browser-default" runat="server"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlEmployees"
                                                                Display="None" ErrorMessage="Assigned To is Required" SetFocusOnError="True" InitialValue="0" ValidationGroup="search">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator6">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Estimate Type</label>
                                                            <asp:DropDownList ID="ddlEstimateType" AutoPostBack="true" OnSelectedIndexChanged="ddlEstimateType_SelectedIndexChanged" CssClass="browser-default" runat="server">
                                                                <asp:ListItem Value="bid">Bid</asp:ListItem>
                                                                <asp:ListItem Value="quote">T&M</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Template</label>
                                                            <telerik:RadAjaxPanel runat="server">
                                                                <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="browser-default" onchange="javascript:return EsTemplateChangeConfirmation();" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                            </telerik:RadAjaxPanel>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlTemplate"
                                                                Display="None" ErrorMessage="Template Required" SetFocusOnError="True" InitialValue="0" ValidationGroup="search">
                                                            </asp:RequiredFieldValidator>

                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator3" PopupPosition="BottomLeft">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>
                                                    <%--<telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel_Opportunity">--%>
                                                    <div class="input-field col s12" runat="server" id="txtJobTypediv">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtJobType" runat="server"></asp:TextBox>
                                                            <asp:Label runat="server" ID="Label11" AssociatedControlID="txtJobType">Department</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s11">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Opportunity #</label>
                                                            <telerik:RadAjaxPanel runat="server">
                                                                <asp:DropDownList ID="ddlOpportunity" runat="server" CssClass="browser-default"
                                                                    OnSelectedIndexChanged="ddlOpportunity_SelectedIndexChanged" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </telerik:RadAjaxPanel>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s1 m-t-5">
                                                        <div class="row">
                                                            <div class="btnlinksicon">
                                                                <asp:LinkButton ID="lnkAddEditOpportunity" runat="server" Style="padding: 0px!important;" CausesValidation="False"
                                                                    ToolTip="Add Opportunity" OnClick="lnkAddEditOpportunity_Click"><i class="mdi-social-person-add"></i></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12" runat="server" id="divOppName">
                                                        <div class="row">
                                                            <asp:TextBox ID="txtOppName" runat="server" AutoCompleteType="None" MaxLength="255"></asp:TextBox>
                                                            <asp:Label runat="server" ID="Label12" AssociatedControlID="txtOppName">Opportunity Name</asp:Label>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <label class="drpdwn-label">Opportunity Stage</label>
                                                            <asp:DropDownList ID="ddlOppStage" CssClass="browser-default" runat="server"></asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <%--<div class="input-field col s3" style="padding-left:0;">
                                                        <div class="checkbox">
                                                            <telerik:RadAjaxPanel runat="server">
                                                                <asp:CheckBox ID="chkAddGroup" runat="server" CssClass="css-checkbox" Text="Add Group" 
                                                                    OnClick="ShowHideGroupEquipments();"/>
                                                            </telerik:RadAjaxPanel>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s9">
                                                        <div class="row" id="divGroupName">
                                                            <asp:Label runat="server" ID="lblGroupName" AssociatedControlID="txtGroupName">Group Name</asp:Label>
                                                            <asp:TextBox ID="txtGroupName" runat="server" MaxLength="255" AutoCompleteType="None"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvGroupName" runat="server" ControlToValidate="txtGroupName"
                                                                Display="None" ErrorMessage="Group Name Required" SetFocusOnError="True" ValidationGroup="search">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender8" runat="server" Enabled="True"
                                                                TargetControlID="rfvGroupName" PopupPosition="BottomLeft">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>--%>
                                                    <div class="col s12">
                                                        <div class="row" id="divGroupEquipments">
                                                            <div class="input-field col s11">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Group Name</label>
                                                                    <telerik:RadAjaxPanel runat="server">
                                                                        <asp:DropDownList ID="ddlEstimateGroup" CssClass="browser-default" OnSelectedIndexChanged="ddlEstimateGroup_SelectedIndexChanged" runat="server" AutoPostBack="true"></asp:DropDownList>
                                                                    </telerik:RadAjaxPanel>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s1 m-t-5">
                                                                <div class="row">
                                                                    <div class="btnlinksicon">
                                                                        <asp:LinkButton ID="lnkAddGroupName" runat="server" Style="padding: 0px!important;" CausesValidation="False" ToolTip="Add New Group" OnClientClick="OpenRadWindowGroup();return false"><i class="mdi-social-person-add"></i></asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <%--</telerik:RadAjaxPanel>--%>

                                                            <div class="input-field col s12" id="divEquipments">
                                                                <div class="row">
                                                                    <label class="drpdwn-label" style="transform: none;">Equipment</label>
                                                                    <div class="tag-div materialize-textarea textarea-border" id="eqtag">
                                                                    </div>
                                                                    <div id="DivEqup" class="popup_div popup-css">
                                                                        <%--<div class="btnlinks" style="margin-bottom: 5px;">
                                                                            <asp:HyperLink ID="HyperLinkAddEquip" Visible="true" Target="_blank" runat="server">Add New</asp:HyperLink>
                                                                        </div>--%>
                                                                        <div class="btnlinks close-css">
                                                                            <%--<asp:LinkButton CausesValidation="false" ID="HideMELinkButton1" runat="server" OnClientClick="HideME();">Close</asp:LinkButton>--%>
                                                                            <a href="#" onclick="HideME();">Close</a>
                                                                        </div>
                                                                        <div class="grid_container">
                                                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                                                <telerik:RadAjaxPanel ID="RadAjaxPanelRadgvEquip" runat="server">
                                                                                    <telerik:RadGrid ID="RadgvEquip"
                                                                                        RenderMode="Auto" AllowFilteringByColumn="True" ShowFooter="false" PageSize="10"
                                                                                        ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" Width="100%" PagerStyle-AlwaysVisible="true"
                                                                                        AllowCustomPaging="True">
                                                                                        <CommandItemStyle />
                                                                                        <GroupingSettings CaseSensitive="false" />
                                                                                        <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                            <Selecting AllowRowSelect="True"></Selecting>
                                                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                        </ClientSettings>
                                                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                                            <Columns>
                                                                                                <telerik:GridTemplateColumn DataField="id" AutoPostBackOnFilter="false" AllowFiltering="false" SortExpression="id"
                                                                                                    CurrentFilterFunction="Contains" UniqueName="id" HeaderText="" ShowFilterIcon="false" HeaderStyle-Width="30">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblID" runat="server" Style="display: none;" Text='<%# Bind("id") %>'></asp:Label>
                                                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:CheckBox ID="chkAll" runat="server" />
                                                                                                    </HeaderTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="unit" AutoPostBackOnFilter="true" SortExpression="unit"
                                                                                                    CurrentFilterFunction="Contains" UniqueName="unit" HeaderText="Name" ShowFilterIcon="false" HeaderStyle-Width="150">

                                                                                                    <ItemTemplate>
                                                                                                        <a href="addequipment.aspx?uid=<%# Eval("id") %>" target="_blank" style="color: white">
                                                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("unit") %>'></asp:Label></a>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>

                                                                                                <telerik:GridTemplateColumn DataField="state" AutoPostBackOnFilter="true" SortExpression="state"
                                                                                                    CurrentFilterFunction="Contains" UniqueName="state" HeaderText="Unique#" ShowFilterIcon="false" HeaderStyle-Width="150">

                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblUID" runat="server"><%#Eval("state")%></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>

                                                                                                <telerik:GridTemplateColumn DataField="fdesc" AutoPostBackOnFilter="true" SortExpression="fdesc"
                                                                                                    CurrentFilterFunction="Contains" UniqueName="fdesc" HeaderText="Description" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>

                                                                                                <telerik:GridTemplateColumn DataField="Type" AutoPostBackOnFilter="true" SortExpression="Type"
                                                                                                    CurrentFilterFunction="Contains" UniqueName="Type" HeaderText="Type" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblType" runat="server"><%#Eval("Type")%></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>

                                                                                                <telerik:GridTemplateColumn DataField="category" AutoPostBackOnFilter="true" SortExpression="id"
                                                                                                    CurrentFilterFunction="Contains" UniqueName="category" HeaderText="Category" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblcat" runat="server"><%#Eval("category")%></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="cat" AutoPostBackOnFilter="true" AllowFiltering="false" SortExpression="cat"
                                                                                                    CurrentFilterFunction="Contains" UniqueName="cat" HeaderText="ServiceType" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblServiceType" runat="server"><%#Eval("cat")%></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="Building" AutoPostBackOnFilter="true" AllowFiltering="false" SortExpression="Building"
                                                                                                    CurrentFilterFunction="Contains" UniqueName="Building" HeaderText="Building" ShowFilterIcon="false" HeaderStyle-Width="150">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblbuilding" runat="server"><%#Eval("building")%></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="status" AutoPostBackOnFilter="true" AllowFiltering="false" SortExpression="status"
                                                                                                    CurrentFilterFunction="Contains" UniqueName="status" HeaderText="Status" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="shut_down" AutoPostBackOnFilter="true" AllowFiltering="false" SortExpression="shut_down"
                                                                                                    CurrentFilterFunction="Contains" UniqueName="shut_down" HeaderText="Shut Down" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblShutdown" runat="server"><%#Eval("shut_down")%></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn DataField="ShutdownReason" AutoPostBackOnFilter="true" AllowFiltering="false" SortExpression="ShutdownReason"
                                                                                                    CurrentFilterFunction="Contains" UniqueName="ShutdownReason" HeaderText="Shut Down Description" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblShutdownReason" runat="server"><%#Eval("ShutdownReason")%></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false"
                                                                                                    CurrentFilterFunction="Contains" HeaderText="%Hours" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtHours" runat="server" Width="50px" MaxLength="20"></asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate><%= RadgvEquip.VirtualItemCount  %>  Record(s) Found.   </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                            </Columns>
                                                                                            <NoRecordsTemplate>No Record Found.   </NoRecordsTemplate>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                </telerik:RadAjaxPanel>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <asp:TextBox ID="txtUnit" runat="server" Style="display: none;"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:Label runat="server" ID="Label23" AssociatedControlID="txtComment">Internal Comment</asp:Label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtREPdesc"
                                                                Display="None" ErrorMessage="Comment Required" SetFocusOnError="True" ValidationGroup="search">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender9" runat="server" Enabled="True"
                                                                TargetControlID="rfvDesc">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:TextBox ID="txtComment" runat="server" CssClass="materialize-textarea" MaxLength="255" AutoCompleteType="None" TextMode="MultiLine"></asp:TextBox>

                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                    </div>
                                </div>
                                <%--</telerik:RadAjaxPanel>   --%>
                            </li>

                        </ul>

                    </div>
                </div>
            </div>
        </div>

        <div class="container accordian-wrap">
            <div class="row">
                <div class="col s12 m12 l12">
                    <div class="row">
                        <div class="card">
                            <div class="card-content">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="section-ttle">Estimate Totals</div>
                                        <div class="grid_container">
                                            <div class="form-section-row pmd-card">
                                                <div class="RadGrid RadGrid_Material" style="overflow-y: auto;">
                                                    <table class="radliketable">
                                                        <thead>
                                                            <tr>
                                                                <th class="Bid">Bid Price</th>
                                                                <th class="bid-l" colspan="2">
                                                                    <%--<asp:TextBox ID="txtBidPrice" TabIndex="15" runat="server"
                                                                        onkeypress="return isDecimalKey(this,event)"
                                                                        onchange="showDecimalVal(this);CalculateEstimateMilestone();"
                                                                        CssClass="estheader-text bid-price" MaxLength="45"
                                                                        AutoCompleteType="None"></asp:TextBox>--%>
                                                                    <asp:Label ID="lblBidPrice" runat="server" CssClass="estheader-text bid-price"></asp:Label>
                                                                    <asp:HiddenField ID="hdnBidPrice" runat="server" />
                                                                </th>
                                                                <%--<th  ></th>--%>
                                                                <th class="Bid">Final Bid Price</th>
                                                                <th class="bid-r" colspan="2">
                                                                    <asp:TextBox ID="txtOverride" TabIndex="15" runat="server"
                                                                        onkeypress="return isDecimalKey(this,event)"
                                                                        onchange="showDecimalOrEmptyVal(this);CalculateFinalBidPrice();"
                                                                        CssClass="estheader-text override-amt" MaxLength="45"
                                                                        AutoCompleteType="None" Style="display: none;"></asp:TextBox>
                                                                    <asp:Label ID="lblFinalBid" runat="server" CssClass="estheader-text final-bid" Style="display: none;"></asp:Label>
                                                                    <asp:HiddenField ID="hdnFinalBid" runat="server" />
                                                                    <%--    onchange="showDecimalVal(this);CalculateEstimateMilestone();"--%>
                                                                </th>
                                                                <%--<th style="width: 7%"></th>--%>
                                                                <%--  <td style="width: 7%; text-align: right;" class="profit">0.00%</td>--%>

                                                                <th class="w-8"></th>
                                                                <th class="w-8"></th>
                                                                <th class="w-8"></th>
                                                                <th class="w-8"></th>
                                                                <th class="w-8"></th>
                                                                <th class="w-9" style="width: 9%;"></th>
                                                                <th class="w-8"></th>
                                                                <%--<th style="width: 9%;"></th>--%>
                                                                <th class="w-7"></th>
                                                                <th class="w-10"></th>
                                                            </tr>
                                                        </thead>
                                                        <tr style="font-weight: normal; color: #1565c0;">
                                                            <td class="row-fort">Material Exp
                                                            </td>
                                                            <td class="row-fort">Labor Exp
                                                            </td>
                                                            <td class="row-fort">Other Exp
                                                            </td>
                                                            <td class="row-fort">% Cont
                                                            </td>
                                                            <td class="row-fort">Subtotal
                                                            </td>
                                                            <td class="row-fort">% OH
                                                            </td>
                                                            <td class="row-fort">Total Cost
                                                            </td>
                                                            <td class="row-fort">% Profit
                                                            </td>
                                                            <%--<td class="row-fort">Profit amount / revenue
                                                            </td>--%>
                                                            <td class="row-fort">Pretax total
                                                            </td>
                                                            <td class="row-fort">Non Taxable
                                                            </td>
                                                            <td class="row-fort">Taxable
                                                            </td>
                                                            <td class="row-fort">Sales Tax
                                                            </td>
                                                            <td class="row-fort">% Commission
                                                            </td>
                                                            <td class="row-fort">Total Hour
                                                            </td>
                                                            <td class="row-fort">Total Price
                                                            </td>
                                                        </tr>
                                                        <tr style="font-weight: normal;">
                                                            <td class="row-fort1">---
                                                                <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_materialexp" Value="" />
                                                            </td>
                                                            <td class="row-fort1">---
                                                                <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_laborexp" Value="" />
                                                            </td>
                                                            <td class="row-fort1">---
                                                                <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_otherexp" Value="" />
                                                            </td>
                                                            <td class="row-fort1">

                                                                <div class="input-field col s12" style="margin-top: 4px;">
                                                                    <div style="float: left; clear: both;">
                                                                        <asp:TextBox ID="txtPerContingencies" runat="server" MaxLength="45" CssClass="estheader-text"
                                                                            onchange="CalculateContiPer(1);CalculateEstimateBOM();UpdateProfit(true);" AutoCompleteType="None" Text="0.00" Style="text-align: center; min-width: 65px;"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td class="row-fort1">---</td>

                                                            <td>
                                                                <div class="input-field col s12" style="margin-top: 4px;">
                                                                    <div style="float: left; clear: both;">
                                                                        <asp:TextBox ID="txtHOPercentAge" runat="server" MaxLength="45"
                                                                            onchange="CalculateOHPer(1);CalculateEstimateBOM();UpdateProfit(true);" CssClass="estheader-text"
                                                                            AutoCompleteType="None" Style="text-align: center; min-width: 65px;"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td style="text-align: center; padding-top: 0; padding-bottom: 0;">---
                                                                <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_totalcost" Value="" />
                                                            </td>
                                                            <td>
                                                                <div style="text-align: center; width: 100%; margin-top: 4px;">
                                                                    <div class="input-field col s12">
                                                                        <asp:TextBox ID="txtMarkupPercentAge" runat="server" MaxLength="45" onchange="TxtCalculateProfit();" CssClass="estheader-text"
                                                                            AutoCompleteType="None" Style="text-align: center;"></asp:TextBox>
                                                                        <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_markup" Value="" />
                                                                        <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_markupper" Value="" /> 
                                                                        <asp:HiddenField ClientIDMode="Static" runat="server" ID="Gridhd_markupper" Value="" /> 
                                                                    </div>
                                                                </div>
                                                            </td>


                                                            <td style="text-align: center; padding-top: 0; padding-bottom: 0;">---
                                                                <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_pretax" Value="" />
                                                            </td>
                                                            <td style="text-align: center; padding-top: 0; padding-bottom: 0;">---</td>
                                                            <td style="text-align: center; padding-top: 0; padding-bottom: 0;">---</td>
                                                            <td>
                                                                <div style="text-align: center; width: 100%; margin-top: -2px;">

                                                                    <div style="float: left; clear: both;">
                                                                        <asp:DropDownList runat="server" CssClass="browser-default" onchange="CalculateEstimateBOM();" Style="min-width: 100px!important; height: 28px!important; font-size: 0.9rem !important; margin-top: 0; margin-bottom: 0;" ID="drpSaleTax">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_salestax" Value="" />
                                                                    </div>
                                                                </div>

                                                            </td>
                                                            <td>
                                                                <div style="text-align: center; width: 100%;">

                                                                    <div class="input-field col s12" style="margin-top: 4px; padding-left: 0 !important; padding-right: 0 !important;">
                                                                        <asp:TextBox ID="txtPercentAgeCommission" runat="server"
                                                                            CssClass="estheader-text" MaxLength="45" Text="0.00"
                                                                            onchange="CalculateCommission(1);CalculateEstimateBOM();"
                                                                            AutoCompleteType="None" Style="text-align: center; min-width: 65px;"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td class="row-fort">---</td>
                                                        </tr>
                                                        <tr style="text-align: right;">
                                                            <td class="mat-ext" style="text-align: center;"></td>
                                                            <td class="lb-ext" style="text-align: center;"></td>
                                                            <td class="other-ext" style="text-align: center;"></td>
                                                            <td style="text-align: center;">
                                                                <asp:TextBox ID="txtContingencies" TabIndex="15" runat="server"
                                                                    onkeypress="return isDecimalKey(this,event)"
                                                                    onchange="showDecimalVal(this);CalculateContiPer(2);CalculateEstimateBOM();UpdateProfit(true);"
                                                                    CssClass="estheader-text cont" MaxLength="45"
                                                                    AutoCompleteType="None" Style="text-align: center; width: 100%!important; border-bottom: none !important; color: #1565c0 !important;"></asp:TextBox>
                                                                <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_subtotal" Value="" />
                                                            </td>
                                                            <td class="subtotal" style="text-align: center;"></td>
                                                            <td style="text-align: center;">
                                                                <asp:TextBox ID="txtOH" TabIndex="15" runat="server"
                                                                    onkeypress="return isDecimalKey(this,event)"
                                                                    CssClass="estheader-text oh" MaxLength="45"
                                                                    onchange="showDecimalVal(this);CalculateOHPer(2);CalculateEstimateBOM();UpdateProfit(true);"
                                                                    AutoCompleteType="None" Style="text-align: center; width: 100%!important; border-bottom: none !important; color: #1565c0 !important;"></asp:TextBox>
                                                                <%--<input type="hidden" name="hd_oh" id="hd_oh" value="" />--%>
                                                                <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_oh" Value="" />
                                                            </td>
                                                            <td class="totalcost" style="text-align: center;"></td>
                                                            <td class="markup" style="text-align: center;">$0.00</td>
                                                            <%--<td class="profitAmount" style="text-align: center;"></td>--%>
                                                            <td class="pretax" style="text-align: center;"></td>
                                                            <td class="nontaxable" style="text-align: center;"></td>
                                                            <td class="taxable" style="text-align: center;"></td>
                                                            <td class="stax" style="text-align: center;"></td>
                                                            <td style="text-align: center;">
                                                                <asp:TextBox ID="txtCommission" TabIndex="15" runat="server"
                                                                    CssClass="estheader-text" MaxLength="45"
                                                                    onchange="CalculateCommission(2);CalculateEstimateBOM();"
                                                                    AutoCompleteType="None" Style="text-align: center; width: 100% !important; border-bottom: none !important; color: #1565c0 !important;"></asp:TextBox>
                                                                <%--<input type="hidden" name="hd_commission" id="hd_commission" value="" />--%>
                                                                <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_commission" Value="" />
                                                            </td>
                                                            <td class="totalHour" style="text-align: center;"></td>
                                                            <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_totalHour" Value="" />
                                                            <td class="total" style="text-align: center;"></td>
                                                            <asp:HiddenField ClientIDMode="Static" runat="server" ID="hd_total" Value="" />

                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="form-section-row">

                                            <div class="row">


                                                <div class="col s12 m12 l12" style="padding-right: 0px;">
                                                    <div class="row">
                                                        <%-- <asp:UpdatePanel ID="updPnlEstimateTemp" runat="server">
                                                            <ContentTemplate>--%>
                                                        <asp:HiddenField ID="hdnExchangeRate" runat="server" />
                                                        <div class="est-tabs" style="margin-top: 20px;">
                                                            <div class="col s12">
                                                                <ul class="tabs tab-demo-active white" style="width: 100%; margin-bottom: 15px;">
                                                                    <li class="tab col s2">
                                                                        <a class="white-text waves-effect waves-light active" href="#activeone"><i class="mdi-action-done"></i>&nbsp;BOM</a>
                                                                    </li>
                                                                    <li class="tab col s2">
                                                                        <a class="white-text waves-effect waves-light" href="#two"><i class="mdi-content-flag"></i>&nbsp;Billing</a>
                                                                    </li>
                                                                    <li class="tab col s2">
                                                                        <a class="white-text waves-effect waves-light" href="#three"><i class="mdi-content-content-paste"></i>&nbsp;Notes</a>
                                                                    </li>
                                                                    <li class="tab col s2">
                                                                        <a class="white-text waves-effect waves-light" href="#four"><i class="mdi-file-attachment"></i>&nbsp;Documents</a>
                                                                    </li>
                                                                    <%--<li runat="server" id="liForms" class="tab col s2">
                                                                        <a class="white-text waves-effect waves-light" href="#ctl00_ContentPlaceHolder1_five"><i class="mdi-av-web"></i>&nbsp;Forms</a>
                                                                    </li>--%>
                                                                    <li runat="server" id="li2" class="tab col s2">
                                                                        <a class="white-text waves-effect waves-light" href="#six"><i class="mdi-action-trending-up"></i>&nbsp;Custom</a>
                                                                    </li>
                                                                    <li runat="server" id="li3" class="tab col s2">
                                                                        <a class="white-text waves-effect waves-light" href="#seven"><i class="mdi-action-payment"></i>&nbsp;Tasks</a>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                            <div class="col s12">
                                                                <div id="activeone" class="col s12 tab-container-border lighten-4" style="display: block;">
                                                                    <div class="row">
                                                                        <div class="tab-container-content" style="overflow: auto;">
                                                                            <div class="tabs-custom-mgn1 mn-ht">
                                                                                <div class="form-section-row">
                                                                                    <div>
                                                                                        <asp:CheckBox ID="chkMaterial" CssClass="css-checkbox" runat="server" Text="Material" Style="margin-right: 15px;" />
                                                                                        <asp:HiddenField ID="hdnChkMat" runat="server"></asp:HiddenField>
                                                                                        <asp:CheckBox ID="chkLabor" CssClass="css-checkbox" runat="server" Text="Labor" Style="margin-right: 15px;" />
                                                                                        <asp:HiddenField ID="hdnChkLb" runat="server"></asp:HiddenField>
                                                                                        <%--<asp:HiddenField ID="hdnDtmat" runat="server" />--%>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="grid_container">
                                                                                    <div class="form-section-row" style="margin-bottom: 0 !important;">

                                                                                        <asp:UpdatePanel ID="UpdPnlBOM" runat="server">
                                                                                            <ContentTemplate>
                                                                                                <div class="RadGrid RadGrid_Material BomGrid">
                                                                                                    <telerik:RadGrid ID="gvBOM" runat="server" CssClass="BomGrid" AutoGenerateColumns="False" Width="100%"
                                                                                                        RenderMode="Auto" AllowFilteringByColumn="true" ShowGroupPanel="false" ShowStatusBar="true" AllowPaging="True"
                                                                                                        PageSize="20" ShowFooter="true"
                                                                                                        OnNeedDataSource="gvBOM_NeedDataSource"
                                                                                                        OnPreRender="gvBOM_PreRender"
                                                                                                        ClientSettings-Scrolling-UseStaticHeaders="true"
                                                                                                        ClientSettings-Scrolling-AllowScroll="true">
                                                                                                        <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                                        <GroupingSettings CaseSensitive="false" />
                                                                                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                            <ClientEvents OnRowContextMenu="RowContextMenuBomGrid"
                                                                                                                OnGridCreated="OnGridCreated"
                                                                                                                OnHeaderMenuShowing="headerMenuShowing"
                                                                                                                OnKeyPress="AddNewRowsBOM" />
                                                                                                            <%--<Resizing AllowColumnResize="True" AllowRowResize="false" ResizeGridOnColumnResize="false"
                                                                                                                ClipCellContentOnResize="true" EnableRealTimeResize="false" AllowResizeToFit="true" />--%>
                                                                                                        </ClientSettings>
                                                                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="true" ShowFooter="true" AllowPaging="false" EnableHeaderContextMenu="true">
                                                                                                            <Columns>
                                                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="50" AllowFiltering="false" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <HeaderTemplate>
                                                                                                                        <asp:LinkButton ID="ibDeleteBom" runat="server" CausesValidation="false" Style="color: #000; font-size: 1.5em;"
                                                                                                                            OnClientClick="return  CheckDelete('gvBOM.ClientID');" ToolTip="Delete Selected"
                                                                                                                            OnClick="ibDeleteBom_Click"
                                                                                                                            Width="20px"><i class="mdi-navigation-cancel" style="color: #f00;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>
                                                                                                                        <asp:CheckBox ID="chkSelectAll" onchange="selectAllCheckbox(this, 'BomItem');" runat="server" CssClass="css-checkbox chkSelectAll" Text=" " />
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox ID="chkSelect" runat="server" CssClass="css-checkbox chkSelect" Text=" " />
                                                                                                                        <asp:HiddenField ID="hdnOrderNo" runat="server" Value='<%# Eval("OrderNo") %>'></asp:HiddenField>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:LinkButton ID="imgBtnAdd" runat="server" CausesValidation="False" Style="color: #000; font-size: 1.5em;"
                                                                                                                            Width="20px" OnClientClick="return imgBtnAddBOM_ClientClick();">
                                                                                                                        <i class="mdi-content-add-circle" style="color: #2bab54;font-size: 1.2em; font-weight: bold;"></i>
                                                                                                                        </asp:LinkButton>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="50" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false" AutoPostBackOnFilter="false"
                                                                                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div class="handle" style="cursor: move;" title="Move Up/Down">
                                                                                                                            <img src="images/Dragdrop.png" width="20" />
                                                                                                                        </div>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Line No." HeaderStyle-Width="100" UniqueName="Line"
                                                                                                                    AllowFiltering="false" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("JobTItemID") %>'></asp:HiddenField>
                                                                                                                        <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>'></asp:Label>
                                                                                                                        <asp:Label ID="lblIndex" Width="100" runat="server" Text='<%# Container.ItemIndex +1 %>' Style="display: none;"></asp:Label>
                                                                                                                        <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>
                                                                                                                        <asp:HiddenField ID="hdnEstimateItemID" runat="server" Value='<%# Eval("EstimateItemID") %>'></asp:HiddenField>
                                                                                                                        <asp:HiddenField ID="hdnIndex" runat="server" Value='<%# Container.ItemIndex +1 %>'></asp:HiddenField>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Op Sequence" HeaderStyle-Width="110" UniqueName="txtCode"
                                                                                                                    DataField="Code" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtCode" runat="server" Text='<%# Eval("Code") %>'
                                                                                                                            CssClass="input-sm input-small-num txtCodeAutoComplate preventdownrow"></asp:TextBox>
                                                                                                                        <asp:HiddenField ID="hdnCode" runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="CodeDesc" HeaderStyle-Width="100" UniqueName="lblCodeDesc"
                                                                                                                    DataField="CodeDesc" AllowFiltering="true" AllowSorting="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ReadOnly="true" ID="lblCodeDesc" runat="server" Text='<%# Eval("CodeDesc") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="180" UniqueName="ddlBType"
                                                                                                                    DataField="BTypeName" AllowFiltering="true" AllowSorting="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:DropDownList ID="ddlBType" runat="server" DataTextField="Type"
                                                                                                                            SelectedValue='<%# Eval("BType") == DBNull.Value ? "-1" : Eval("BType") %>'
                                                                                                                            CssClass="form-control input-sm input-small-num browser-default preventdownrow" AppendDataBoundItems="true"
                                                                                                                            DataValueField="ID" DataSource='<%#dtBomType%>' onchange="CalculateEstimateBOM();">
                                                                                                                            <asp:ListItem Value="-1">Select Type</asp:ListItem>
                                                                                                                            <asp:ListItem Value="0"> < Add New > </asp:ListItem>
                                                                                                                        </asp:DropDownList>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Item" HeaderStyle-Width="200" UniqueName="txtMatItem"
                                                                                                                    DataField="MatName" AllowFiltering="true" AllowSorting="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox runat="server" ID="txtMatItem" CssClass="form-control input-sm input-small-num browser-default txtMatItemSearch preventdownrow"
                                                                                                                            Style="font-size: 0.9rem !important; text-align: center;"
                                                                                                                            Text='<%# Eval("MatName")%>'></asp:TextBox>
                                                                                                                        <asp:HiddenField runat="server" ID="hdnMatItem" Value='<%# Eval("MatItem")%>' />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="200" UniqueName="txtScope"
                                                                                                                    DataField="fDesc" AllowFiltering="true" AllowSorting="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtScope" runat="server" MaxLength="100" Text='<%# Eval("fDesc") %>' CssClass="form-control input-sm input-small"
                                                                                                                            Style="font-size: 0.9rem !important; text-align: center;"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>

                                                                                                                <telerik:GridTemplateColumn HeaderText="Change Order" HeaderStyle-Width="120" DataField="ChangeOrder" AllowSorting="true" UniqueName="ChangeOrder" AllowFiltering="false" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox ID="chkChangeOrder" onchange="chkChangeOrder_onchange(this);" Checked='<%# (Convert.ToString(Eval("ChangeOrder"))=="1") ? true : false %>'
                                                                                                                            runat="server" />
                                                                                                                        <asp:HiddenField ID="hdnChangeOrderChk" runat="server" Value='<%# Convert.ToString(Eval("ChangeOrder")) == "1" ? true : false %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>

                                                                                                                <telerik:GridTemplateColumn HeaderText="Qty Required" UniqueName="txtQtyReq" HeaderStyle-Width="120" HeaderStyle-CssClass="qtyRequired"
                                                                                                                    ItemStyle-CssClass="qtyRequired" AllowFiltering="true" AllowSorting="true" AutoPostBackOnFilter="true"
                                                                                                                    DataField="QtyReq" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtQtyReq" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("QtyReq").ToString()) ? Eval("QtyReq","{0:n}") : "0.00"  %>'
                                                                                                                            onchange="CalculateMat(this);" CssClass="form-control input-sm input-small-num"
                                                                                                                            onkeypress="return isDecimalKey(this,event)">
                                                                                                                        </asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label ID="lblTotalQtyReq" runat="server" Style="text-align: left;"></asp:Label>

                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="U/M" UniqueName="txtUM" HeaderStyle-Width="80" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart"
                                                                                                                    DataField="UM" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtUM" runat="server" Text='<%# Eval("UM") %>'
                                                                                                                            CssClass="form-control input-sm input-small-num preventdownrow" Style="text-align: center !important; font-size: 0.9rem;">
                                                                                                                        </asp:TextBox>
                                                                                                                        <asp:HiddenField ID="hdnUMID" runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Budget $" UniqueName="txtBudgetUnit" HeaderStyle-Width="100" HeaderStyle-CssClass="MatPart"
                                                                                                                    DataField="BudgetUnit" ItemStyle-CssClass="MatPart" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtBudgetUnit" runat="server" Text='<%# !string.IsNullOrEmpty(Eval("BudgetUnit").ToString()) ? Eval("BudgetUnit","{0:n}") : "0.00"  %>'
                                                                                                                            onchange="CalculateMat(this);" CssClass="form-control input-sm input-small-num" Width="100"
                                                                                                                            Style="text-align: center !important; font-size: 0.9rem;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label ID="lblTotalMatBudgetUnit" runat="server" Style="text-align: left;"></asp:Label>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Mat. Mod" UniqueName="txtMatMod" HeaderStyle-Width="100" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart"
                                                                                                                    DataField="MatMod" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtMatMod" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("MatMod").ToString()) ? Eval("MatMod","{0:n}") : "0.00"  %>'
                                                                                                                            Style="text-align: center !important; font-size: 0.9rem;"
                                                                                                                            CssClass="form-control input-sm input-small-num"
                                                                                                                            onchange="CalculateAndFillBOMFooter();showDecimalVal(this);"
                                                                                                                            onkeypress="return isDecimalKey(this,event)">
                                                                                                                        </asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label ID="lblTotalMatMod" runat="server" Style="text-align: left;"></asp:Label>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Mat. Ext $" UniqueName="lblMatExt" HeaderStyle-Width="150" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart"
                                                                                                                    DataField="BudgetExt" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblMatExt" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("BudgetExt").ToString()) ? Eval("BudgetExt","{0:n}") : "0.00"  %>'
                                                                                                                            Style="text-align: center; font-size: 0.9rem;">
                                                                                                                        </asp:Label>
                                                                                                                        <asp:HiddenField ID="hdnMatExt" runat="server" Value='<%# Eval("BudgetExt","{0:n}") %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label ID="lblTotalMatExt" runat="server" Style="text-align: left;"></asp:Label>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Mat. Markup Price $" UniqueName="txtMatPrice" HeaderStyle-Width="160" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart"
                                                                                                                    DataField="MatPrice" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtMatPrice" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("MatPrice").ToString()) ? Eval("MatPrice","{0:n}") : "0.00"  %>'
                                                                                                                            CssClass="form-control input-sm input-small-num" onchange="CalculateMaterialMarkupPer(this,true);UpdateBillingFinalBidPrice();"
                                                                                                                            onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label ID="lblTotalMatPrice" runat="server" Style="text-align: left;"></asp:Label>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Mat. Markup %" UniqueName="txtMatMarkup" HeaderStyle-Width="150" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart"
                                                                                                                    DataField="MatMarkup" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtMatMarkup" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("MatMarkup").ToString()) ? Eval("MatMarkup","{0:n}") : "0.00"  %>'
                                                                                                                            onchange="CalculateMat(this,true);UpdateBillingFinalBidPrice();" onkeypress="return isDecimalKey(this,event)"
                                                                                                                            CssClass="form-control input-sm input-small-num"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Mat. Tx." UniqueName="chkMatSalestax" HeaderStyle-Width="80" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart"
                                                                                                                    AllowSorting="false" AllowFiltering="false" AutoPostBackOnFilter="false" ShowFilterIcon="false">
                                                                                                                    <HeaderTemplate>
                                                                                                                        <asp:Label runat="server" CssClass="display-block">Mat. Tx.</asp:Label>
                                                                                                                        <asp:CheckBox ID="chkSelectAllMatTx" onchange="selectAllCheckbox(this, 'MatTx');"
                                                                                                                            runat="server" CssClass="css-checkbox chkSelectAllMatTx" Text=" " />
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox ID="chkMatSalestax" CssClass="chkMatSalestax" Checked='<%# (Convert.ToString(Eval("STax")) == "1") ? true : false %>'
                                                                                                                            runat="server" onchange="CalculateMat(this);UpdateBillingFinalBidPrice();" />
                                                                                                                        <asp:HiddenField ID="hdnMatChk" runat="server" Value='<%# Convert.ToString(Eval("STax")) == "1" ? true : false %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Currency" UniqueName="ddlCurrencyEstbind" HeaderStyle-Width="120" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart"
                                                                                                                    DataField="Name" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <HeaderTemplate>
                                                                                                                        <asp:DropDownList ID="ddlCurrencyEst" DataSource='<%#dtCurrency%>' DataTextField="Name"
                                                                                                                            CssClass="browser-default currency-header" Style="font-size: 0.9rem !important; margin-top: 0; margin-bottom: 1px; border-bottom: none !important; text-align: center;"
                                                                                                                            DataValueField="ID"
                                                                                                                            runat="server">
                                                                                                                        </asp:DropDownList>

                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:DropDownList ID="ddlCurrencyEstbind" DataSource='<%#dtCurrency%>' DataTextField="Name"
                                                                                                                            CssClass="input-sm input-small-num browser-default currency-header preventdownrow"
                                                                                                                            DataValueField="ID" runat="server">
                                                                                                                        </asp:DropDownList>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Vendor" UniqueName="txtVendor" HeaderStyle-Width="200" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart"
                                                                                                                    DataField="Vendor" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtVendor" runat="server" Text='<%# Eval("Vendor") %>' Style="font-size: 0.9rem !important; text-align: center !important;"
                                                                                                                            CssClass="form-control input-sm input-small vendorS preventdownrow" placeholder="Search by vendor"></asp:TextBox>
                                                                                                                        <asp:HiddenField ID="hdnVendorId" runat="server" Value='<%# Eval("VendorId") %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Labor Item" UniqueName="ddlLabItem" HeaderStyle-Width="120" HeaderStyle-CssClass="LabPart" ItemStyle-CssClass="LabPart"
                                                                                                                    DataField="LabItemName" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <%--<asp:HiddenField ID="hdntxtLabItem" runat="server" Value='<%# Eval("LabItem") %>' />--%>
                                                                                                                        <asp:DropDownList ID="ddlLabItem" runat="server" DataTextField="LabDesc" DataValueField="LabItem"
                                                                                                                            Style="font-size: 0.9rem !important;"
                                                                                                                            SelectedValue='<%# Eval("LabItem") == DBNull.Value ? "0" : Eval("LabItem") %>'
                                                                                                                            CssClass="form-control input-sm input-small browser-default preventdownrow"
                                                                                                                            DataSource='<%#dtLab%>'>
                                                                                                                        </asp:DropDownList>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Hours" UniqueName="txtHours" HeaderStyle-Width="100" HeaderStyle-CssClass="LabPart" ItemStyle-CssClass="LabPart"
                                                                                                                    DataField="LabHours" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtHours" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("LabHours").ToString()) ? Eval("LabHours","{0:n}") : "0.00"  %>'
                                                                                                                            CssClass="form-control input-sm input-small-num" onchange="CalculateLb(this)"
                                                                                                                            Style="min-width: 100%!important;"
                                                                                                                            onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label ID="lblTotalHours" runat="server" Style="text-align: left;"></asp:Label>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Rate" UniqueName="txtLabRate" HeaderStyle-Width="100" HeaderStyle-CssClass="LabPart" ItemStyle-CssClass="LabPart"
                                                                                                                    DataField="LabRate" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtLabRate" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("LabRate").ToString()) ? Eval("LabRate","{0:n}") : "0.00"  %>'
                                                                                                                            CssClass="form-control input-sm input-small-num txtLabRate"
                                                                                                                            onchange="CalculateLb(this)"
                                                                                                                            Style="min-width: 100%!important;"
                                                                                                                            onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Labor Mod" UniqueName="txtLabMod" HeaderStyle-Width="100" HeaderStyle-CssClass="LabPart" ItemStyle-CssClass="LabPart"
                                                                                                                    DataField="LabMod" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtLabMod" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("LabMod").ToString()) ? Eval("LabMod","{0:n}") : "0.00"  %>'
                                                                                                                            CssClass="form-control input-sm input-small-num"
                                                                                                                            onchange="CalculateAndFillBOMFooter();showDecimalVal(this);"
                                                                                                                            Style="min-width: 100%!important;"
                                                                                                                            onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label ID="lblTotalLabMod" runat="server" Style="text-align: left;"></asp:Label>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Labor Ext $" UniqueName="lblLabExt" HeaderStyle-Width="100" HeaderStyle-CssClass="LabPart" ItemStyle-CssClass="LabPart"
                                                                                                                    DataField="LabExt" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblLabExt" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("LabExt").ToString()) ? Eval("LabExt","{0:n}") : "0.00"  %>'
                                                                                                                            Style="min-width: 100%!important; font-size: 0.9rem;"></asp:Label>
                                                                                                                        <asp:HiddenField ID="hdnLabExt" runat="server" Value='<%# Eval("LabExt","{0:n}") %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label ID="lblTotalLabExt" runat="server" Style="text-align: left;"></asp:Label>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Date" UniqueName="txtSDate" HeaderStyle-Width="90" HeaderStyle-CssClass="LabPart" ItemStyle-CssClass="LabPart"
                                                                                                                    DataField="SDate" AllowSorting="true" AllowFiltering="false" AutoPostBackOnFilter="false" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtSDate" runat="server" Text='<%# Eval("SDate")!=DBNull.Value? (!(Eval("SDate").Equals(DateTime.MinValue)) ? (String.Format("{0:MM/dd/yyyy}", Eval("SDate"))) : "" ) : "" %>'
                                                                                                                            CssClass="input-sm input-small-num datepicker_mom"
                                                                                                                            Style="min-width: 100%!important;"></asp:TextBox>
                                                                                                                        <%-- <asp:CalendarExtender ID="txtSDate_CalendarExtender" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtSDate">
                                                                                                                    </asp:CalendarExtender>--%>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Labor Markup Price $" UniqueName="txtLabPrice" HeaderStyle-Width="180" HeaderStyle-CssClass="LabPart" ItemStyle-CssClass="LabPart"
                                                                                                                    DataField="LabPrice" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtLabPrice" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("LabPrice").ToString()) ? Eval("LabPrice","{0:n}") : "0.00"  %>'
                                                                                                                            CssClass="form-control input-sm input-small-num" onchange="CalculateLaborMarkupPer(this,true);UpdateBillingFinalBidPrice();"
                                                                                                                            onkeypress="return isDecimalKey(this,event)" Style="min-width: 100%!important;">
                                                                                                                        </asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label ID="lblTotalLabPrice" runat="server" Style="text-align: left;"></asp:Label>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Labor Markup %" UniqueName="txtLabMarkup" HeaderStyle-Width="150" HeaderStyle-CssClass="LabPart" ItemStyle-CssClass="LabPart"
                                                                                                                    DataField="LabMarkup" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtLabMarkup" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("LabMarkup").ToString()) ? Eval("LabMarkup","{0:n}") : "0.00"  %>'
                                                                                                                            onkeypress="return isDecimalKey(this,event)" onchange="CalculateLb(this,true);UpdateBillingFinalBidPrice();"
                                                                                                                            CssClass="form-control input-sm input-small-num">
                                                                                                                        </asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Labor Tx." UniqueName="chkLabSalestax" HeaderStyle-Width="100" HeaderStyle-CssClass="LabPart" ItemStyle-CssClass="LabPart"
                                                                                                                    AllowSorting="false" AllowFiltering="false" AutoPostBackOnFilter="false" ShowFilterIcon="false">
                                                                                                                    <HeaderTemplate>
                                                                                                                        <asp:Label runat="server" CssClass="display-block">Labor Tx.</asp:Label>
                                                                                                                        <asp:CheckBox ID="chkSelectAllLaborTx" onchange="selectAllCheckbox(this, 'LaborTx');"
                                                                                                                            runat="server" CssClass="css-checkbox chkSelectAllLaborTx" Text=" " />
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox ID="chkLabSalestax" CssClass="chkLabSalestax" Checked='<%# (Convert.ToString(Eval("LStax"))=="1") ? true : false %>'
                                                                                                                            runat="server" onchange="CalculateLb(this);UpdateBillingFinalBidPrice();" />
                                                                                                                        <asp:HiddenField ID="hdnLbChk" runat="server" Value='<%# Convert.ToString(Eval("LSTax")) == "1" ? true : false %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Total Ext $" UniqueName="lblTotalExt" HeaderStyle-Width="160" HeaderStyle-CssClass="TotalExt" ItemStyle-CssClass="TotalExt"
                                                                                                                    DataField="TotalExt" AllowSorting="true" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblTotalExt" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("TotalExt").ToString()) ? Eval("TotalExt","{0:n}") : "0.00"  %>'
                                                                                                                            Style="min-width: 100%!important; font-size: 0.9rem; float: right;"></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label ID="lblTotalTotalExt" runat="server" Style="text-align: left;"></asp:Label>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                            </Columns>
                                                                                                        </MasterTableView>
                                                                                                    </telerik:RadGrid>
                                                                                                    <asp:LinkButton ID="btnAddNewLinesBOM" runat="server" CausesValidation="false"
                                                                                                        OnClick="btnAddNewLinesBOM_Click" Text="Add New Lines" Style="display: none;"
                                                                                                        OnClientClick="itemJSON();" />
                                                                                                    <asp:LinkButton ID="btnCopyPreviousBOM" runat="server" CausesValidation="false" OnClientClick="itemJSON();"
                                                                                                        OnClick="btnCopyPreviousBOM_Click" Text="Copy Previous" Style="display: none;">
                                                                                                    </asp:LinkButton>
                                                                                                    <input type="hidden" runat="server" id="radGridClickedRowIndex" name="radGridClickedRowIndex" />
                                                                                                    <telerik:RadContextMenu ID="RadMenuBomGrid" runat="server" OnItemClick="RadMenuBomGrid_ItemClick" OnClientItemClicking="CheckAddRowGrid"
                                                                                                        EnableRoundedCorners="true" EnableShadows="true" EnableScreenBoundaryDetection="false">
                                                                                                        <Items>
                                                                                                            <telerik:RadMenuItem Text="Add Row Above">
                                                                                                            </telerik:RadMenuItem>
                                                                                                            <telerik:RadMenuItem Text="Add Row Below">
                                                                                                            </telerik:RadMenuItem>
                                                                                                        </Items>
                                                                                                    </telerik:RadContextMenu>
                                                                                                </div>
                                                                                            </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:AsyncPostBackTrigger ControlID="gvBOM" EventName="ItemCommand" />
                                                                                                <%--<asp:AsyncPostBackTrigger ControlID="lbtnTypeSubmit" />--%>
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div id="two" class="col s12 tab-container-border lighten-4" style="display: none;">
                                                                    <div class="tab-container-content">
                                                                        <div class="tabs-custom-mgn1">
                                                                            <div class="row">
                                                                                <div class="form-section-row" style="border: 1px solid #9e9e9e; border-radius: 8px; padding: 15px; box-shadow: 0 0 5px #ccc; clear: both;">


                                                                                    <div class="form-section4">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <asp:Label runat="server" ID="Label16" AssociatedControlID="txtBillRate">Billing Rate</asp:Label>
                                                                                                <asp:TextBox ID="txtBillRate" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)" runat="server" AutoCompleteType="None" MaxLength="15"></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <asp:Label runat="server" ID="Label1" AssociatedControlID="txtRateNT">1.7 Rate</asp:Label>
                                                                                                <asp:TextBox ID="txtRateNT" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)" runat="server" AutoCompleteType="None" MaxLength="15"></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <asp:Label runat="server" ID="Label15" AssociatedControlID="txtTravelRate">Travel Rate</asp:Label>
                                                                                                <asp:TextBox ID="txtTravelRate" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)" runat="server" AutoCompleteType="None" MaxLength="15"></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>

                                                                                    <div class="form-section4-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section4">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <asp:Label runat="server" ID="Label2" AssociatedControlID="txtOTRate">OT Rate</asp:Label>
                                                                                                <asp:TextBox ID="txtOTRate" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)" runat="server" AutoCompleteType="None" MaxLength="15"></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <asp:Label runat="server" ID="Label17" AssociatedControlID="txtDTRate">DT Rate</asp:Label>
                                                                                                <asp:TextBox ID="txtDTRate" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)" runat="server" AutoCompleteType="None" MaxLength="15"></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <asp:Label runat="server" ID="Label18" AssociatedControlID="txtMileageRate">Mileage</asp:Label>
                                                                                                <asp:TextBox ID="txtMileageRate" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)" runat="server" AutoCompleteType="None" MaxLength="15"></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section4-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section4">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label class="drpdwn-label">Billing Type</label>
                                                                                                <asp:DropDownList ID="ddlPType" CssClass="browser-default" onChange="HideShowOnBillingTypeChange(this.value);" runat="server">
                                                                                                    <asp:ListItem Value="0" Selected="True">None</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Quoted</asp:ListItem>
                                                                                                    <asp:ListItem Value="2">Maximum</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12 hideShowOnBillingType">
                                                                                            <div class="row">
                                                                                                <asp:Label runat="server" ID="lbltxtAmount" AssociatedControlID="txtAmount">Amount</asp:Label>
                                                                                                <asp:TextBox ID="txtAmount" runat="server" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this);QuotedAmountToFinalBidPrice();" CssClass="validate"></asp:TextBox>
                                                                                                <asp:CustomValidator ID="cvtxtAmount"
                                                                                                    SetFocusOnError="True"
                                                                                                    runat="server" Display="Dynamic"
                                                                                                    ErrorMessage="Amount is required"
                                                                                                    ControlToValidate="txtAmount"
                                                                                                    ClientValidationFunction="AmountValidate"
                                                                                                    ValidateEmptyText="True" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section4-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section4">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row checkbox">
                                                                                                <asp:CheckBox ID="chkSglBilAmt" runat="server" CssClass="css-checkbox" Text="Single Billing Amount" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row checkbox">
                                                                                                <asp:CheckBox ID="chkBilFrmBOM" OnCheckedChanged="chkBilFrmBOM_CheckedChanged" AutoPostBack="true" runat="server" CssClass="css-checkbox" Text="Billing From BOM"
                                                                                                    onclick="javascript:return chkBilFrmBOM_ChangeConfirm(this);" /><%--onclick="javascript:return chkBilFrmBOM_ChangeConfirm(this);"--%>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="grid_container">
                                                                                    <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                                                        <asp:UpdatePanel ID="UpdatePnlMilestone" runat="server">
                                                                                            <ContentTemplate>
                                                                                                <div class="RadGrid RadGrid_Material">
                                                                                                    <telerik:RadGrid ID="gvMilestones" runat="server" AutoGenerateColumns="False" CssClass="BomGrid" Width="100%"
                                                                                                        RenderMode="Auto" AllowFilteringByColumn="true" ShowStatusBar="true" AllowPaging="True"
                                                                                                        PageSize="20" ShowFooter="true" OnPreRender="gvMilestones_PreRender"
                                                                                                        ClientSettings-Scrolling-UseStaticHeaders="true"
                                                                                                        ClientSettings-Scrolling-AllowScroll="true">
                                                                                                        <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="3" />
                                                                                                            <ClientEvents OnRowContextMenu="RowContextMenuMilesonteGrid" OnKeyPress="AddNewRowsMilestones" />
                                                                                                        </ClientSettings>
                                                                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                                            <Columns>
                                                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="50">
                                                                                                                    <HeaderTemplate>
                                                                                                                        <asp:LinkButton ID="ibDeleteMilestone" runat="server" CausesValidation="false" Style="color: #000; font-size: 1.5em;"
                                                                                                                            OnClientClick="return CheckDelete('gvMilestones.ClientID');"
                                                                                                                            OnClick="ibDeleteMilestone_Click"
                                                                                                                            Width="20px">
                                                                                                                                    <i class="mdi-navigation-cancel" style="color: #f00;font-size: 1.2em; font-weight: bold;"></i>
                                                                                                                        </asp:LinkButton>
                                                                                                                        <asp:CheckBox ID="chkSelectAll" onchange="selectAllCheckbox(this, 'MilItem');" runat="server" CssClass="css-checkbox chkSelectAll" Text=" " />
                                                                                                                    </HeaderTemplate>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox ID="chkSelect" CssClass="css-checkbox chkSelect" Text=" " runat="server" />
                                                                                                                        <asp:HiddenField ID="hdnOrderNoMil" runat="server" Value='<%# Eval("OrderNo") %>'></asp:HiddenField>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:LinkButton ID="imgBtnAdd" runat="server" CausesValidation="False" Style="color: #000; font-size: 1.5em;"
                                                                                                                            Width="20px" OnClientClick="return imgBtnAddMistone_ClientClick();">
                                                                                                                        <i class="mdi-content-add-circle" style="color: #2bab54;font-size: 1.2em; font-weight: bold;"></i>
                                                                                                                        </asp:LinkButton>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Line No." ItemStyle-Width="5%" Display="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblIndex" runat="server" Text='<%# Container.ItemIndex +1 %>'></asp:Label>
                                                                                                                        <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' Width="65px" Style="display: none;"></asp:Label>
                                                                                                                        <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>
                                                                                                                        <asp:HiddenField ID="hdnIndex" runat="server" Value='<%# Container.ItemIndex +1 %>'></asp:HiddenField>
                                                                                                                        <asp:HiddenField ID="hdnEstimateItemId" runat="server" Value='<%# Eval("EstimateItemId") %>'></asp:HiddenField>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="40" HeaderStyle-HorizontalAlign="Center">
                                                                                                                    <ItemTemplate>
                                                                                                                        <div class="handle" style="cursor: move;" title="Move Up/Down">
                                                                                                                            <img src="images/Dragdrop.png" width="20" />
                                                                                                                        </div>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Op Sequence">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtCode" runat="server" Text='<%# Eval("jcode") %>'
                                                                                                                            Style="width: 100%!important;" CssClass="form-control input-sm input-small txtCodeAutoComplate preventdownrow"></asp:TextBox>
                                                                                                                        <asp:HiddenField ID="hdnCode" runat="server" />
                                                                                                                    </ItemTemplate>

                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="CodeDesc" HeaderStyle-Width="100">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ReadOnly="true" ID="lblCodeDesc" runat="server" Text='<%# Eval("CodeDesc") %>'></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Center" HeaderText="Type">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:DropDownList ID="ddlType" runat="server" SelectedValue='<%# Eval("jtype") == DBNull.Value ? 0 : Eval("jtype") %>'
                                                                                                                            CssClass="form-control input-sm input-small browser-default preventdownrow" Style="width: 100%!important;">
                                                                                                                            <asp:ListItem Value="0">Revenue</asp:ListItem>
                                                                                                                        </asp:DropDownList>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Function">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtSType" runat="server" Text='<%# Eval("Department") %>' placeholder="Select Function"
                                                                                                                            CssClass="form-control input-sm input-small preventdownrow" Style="width: 100%!important;"></asp:TextBox>
                                                                                                                        <asp:HiddenField ID="hdnType" Value='<%# Eval("type") %>' runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Change Order">
                                                                                                                    <%--<HeaderTemplate>
                                                                                                                        <asp:Label runat="server" CssClass="display-block">Labor Tx.</asp:Label>
                                                                                                                        <asp:CheckBox ID="chkSelectAllLaborTx" onchange="selectAllCheckbox(this, 'LaborTx');"
                                                                                                                            runat="server" CssClass="css-checkbox chkSelectAllLaborTx" Text=" " />
                                                                                                                    </HeaderTemplate>--%>
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox ID="chkChangeOrder" onchange="chkChangeOrder_onchange(this);" Checked='<%# (Convert.ToString(Eval("ChangeOrder"))=="1") ? true : false %>'
                                                                                                                            runat="server" />
                                                                                                                        <asp:HiddenField ID="hdnChangeOrderChk" runat="server" Value='<%# Convert.ToString(Eval("ChangeOrder")) == "1" ? true : false %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Name">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtName" runat="server" MaxLength="100" Text='<%# Eval("MilesName") %>' CssClass="form-control input-sm input-small"
                                                                                                                            Style="width: 100%!important;"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Description">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtScope" runat="server" MaxLength="100" Text='<%# Eval("fdesc") %>' CssClass="form-control input-sm input-small"
                                                                                                                            Style="width: 100%!important;"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="%" UniqueName="AmountPer">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtPerAmount" runat="server" MaxLength="100"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("AmountPer").ToString()) ? Eval("AmountPer") : ""  %>'
                                                                                                                            onchange="CalBillingAmount(this,1);" CssClass="form-control input-sm input-small"
                                                                                                                            Style="width: 100%!important;"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="Quantity">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtQuantity" runat="server" MaxLength="100"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("Quantity").ToString()) ? Eval("Quantity") : ""  %>'
                                                                                                                            onchange="CalBillingAmount(this,3);" CssClass="form-control input-sm input-small"
                                                                                                                            Style="width: 100%!important;"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Price" UniqueName="Price">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtPrice" runat="server" MaxLength="100"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("Price").ToString()) ? Eval("Price") : ""  %>'
                                                                                                                            onchange="CalBillingAmount(this,4);" CssClass="form-control input-sm input-small"
                                                                                                                            Style="width: 100%!important;"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Amount" FooterAggregateFormatString="{0:c}" FooterStyle-HorizontalAlign="Right">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtAmount" runat="server"
                                                                                                                            Text='<%# !string.IsNullOrEmpty(Eval("Amount").ToString()) ? Eval("Amount","{0:n}") : "0.00"  %>'
                                                                                                                            onkeypress="return isDecimalKey(this,event)"
                                                                                                                            CssClass="form-control input-sm input-small"
                                                                                                                            onchange="CalculateEstimateMilestone();CalBillingAmount(this,2);" Style="width: 100%!important;"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Label ID="lblTotalBillAmt" runat="server" Style="text-align: left;"></asp:Label>
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Required by">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtRequiredBy" runat="server" Text='<%# Eval("RequiredBy")!=DBNull.Value? (!(Eval("RequiredBy").Equals(DateTime.MinValue)) ? (String.Format("{0:MM/dd/yyyy}", Eval("RequiredBy"))) : "" ) : "" %>'
                                                                                                                            CssClass="form-control input-sm input-small datepicker_mom" Style="width: 200px!important;">
                                                                                                                        </asp:TextBox>
                                                                                                                        <%--  <asp:CalendarExtender ID="txtRequiredBy_CalendarExtender" runat="server" Enabled="True"
                                                                                                                                    TargetControlID="txtRequiredBy">
                                                                                                                                </asp:CalendarExtender>--%>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                            </Columns>
                                                                                                        </MasterTableView>
                                                                                                    </telerik:RadGrid>
                                                                                                    <asp:LinkButton ID="btnAddNewLinesMilestones" runat="server" CausesValidation="false"
                                                                                                        OnClick="btnAddNewLinesMilestones_Click" Text="Add New Lines" Style="display: none;"
                                                                                                        OnClientClick="itemJSON();" />
                                                                                                    <asp:LinkButton ID="btnCopyPreviousMilestones" runat="server" CausesValidation="false" OnClientClick="itemJSON();"
                                                                                                        OnClick="btnCopyPreviousMilestones_Click" Text="Copy Previous" Style="display: none;">
                                                                                                    </asp:LinkButton>
                                                                                                    <input type="hidden" runat="server" id="radMilGridClickedRowIndex" name="radMilGridClickedRowIndex" />
                                                                                                    <telerik:RadContextMenu ID="RadMenuMilGrid" runat="server" OnItemClick="RadMenuMilGrid_ItemClick" OnClientItemClicking="CheckAddRowGrid"
                                                                                                        EnableRoundedCorners="true" EnableShadows="true">
                                                                                                        <Items>
                                                                                                            <telerik:RadMenuItem Text="Add Row Above">
                                                                                                            </telerik:RadMenuItem>
                                                                                                            <telerik:RadMenuItem Text="Add Row Below">
                                                                                                            </telerik:RadMenuItem>
                                                                                                        </Items>
                                                                                                    </telerik:RadContextMenu>
                                                                                                </div>
                                                                                            </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:AsyncPostBackTrigger ControlID="gvMilestones" EventName="ItemCommand" />
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div id="three" class="col s12 tab-container-border lighten-4" style="display: none;">
                                                                    <div class="tabs-custom-mgn1">
                                                                        <div class="row">
                                                                            <div class="input-field col s12" style="margin-bottom: 10px;">
                                                                                <div class="row" style="margin-bottom: 10px;">
                                                                                    <asp:Label runat="server" ID="Label20" AssociatedControlID="txtREPremarks" CssClass="txtbrdlbl">Remarks</asp:Label>
                                                                                    <asp:TextBox ID="txtREPremarks" runat="server" CssClass="materialize-textarea textarea-border midtxtarea" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                                                                </div>
                                                                                <div class="row" runat="server" id="RevisionNotes_1" visible="false">
                                                                                    <div class="input-field col s11" style="padding: 0;">
                                                                                        <asp:Label runat="server" ID="lblREPremarks" AssociatedControlID="txtREPremarks">Revision Notes</asp:Label>
                                                                                        <asp:TextBox ID="txtRevisionNotes" ValidationGroup="RevisionNotes" runat="server" MaxLength="200"></asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtRevisionNotes"
                                                                                            Display="Dynamic" ForeColor="Red" ErrorMessage="Revision Notes is Required!" SetFocusOnError="True" ValidationGroup="RevisionNotes">
                                                                                        </asp:RequiredFieldValidator>
                                                                                    </div>
                                                                                    <div class="input-field col s1">
                                                                                        <div class="btnlinks">
                                                                                            <asp:LinkButton ID="lnkRevisionSave" runat="server" ValidationGroup="RevisionNotes" OnClick="lnkRevisionSave_Click" Visible="false" Text="Save"></asp:LinkButton>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="grid_container">
                                                                                        <div class="RadGrid RadGrid_Material  FormGrid">
                                                                                            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                                                                                <script type="text/javascript">
                                                                                                    function pageLoad() {
                                                                                                        var grid = $find("<%= RadGrid_Revision.ClientID %>");
                                                                                                        try {
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

                                                                                            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Estimate" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Revision" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                                                    PagerStyle-AlwaysVisible="true" OnNeedDataSource="RadGrid_Revision_NeedDataSource"
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

                                                                                                            <telerik:GridBoundColumn FilterDelay="5" DataField="Version" HeaderText="Revision #" HeaderStyle-Width="140"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Version" AllowFiltering="false"
                                                                                                                ShowFilterIcon="false">
                                                                                                            </telerik:GridBoundColumn>

                                                                                                            <telerik:GridBoundColumn FilterDelay="5" DataField="Notes" HeaderText="Revision Notes" HeaderStyle-Width="140"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Notes" AllowFiltering="false"
                                                                                                                ShowFilterIcon="false">
                                                                                                            </telerik:GridBoundColumn>

                                                                                                            <telerik:GridBoundColumn FilterDelay="5" DataField="CreatedDate" HeaderText="Date/Time" HeaderStyle-Width="140"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="CreatedDate" AllowFiltering="false"
                                                                                                                ShowFilterIcon="false">
                                                                                                            </telerik:GridBoundColumn>

                                                                                                            <telerik:GridBoundColumn FilterDelay="5" DataField="CreatedBy" HeaderText="User" HeaderStyle-Width="140"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="CreatedBy" AllowFiltering="false"
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


                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div id="four" class="col s12 tab-container-border lighten-4" style="display: none;">
                                                                    <div class="tabs-custom-mgn1">
                                                                        <div class="row">
                                                                            <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                                <Triggers>
                                                                                    <asp:PostBackTrigger ControlID="lnkUploadDoc" />
                                                                                    <asp:PostBackTrigger ControlID="lnkPostback" />
                                                                                    <asp:PostBackTrigger ControlID="RadGrid_Documents" />
                                                                                </Triggers>
                                                                                <ContentTemplate>--%>
                                                                            <div class="form-section-row">
                                                                                <div class="col s12 m12 l12">
                                                                                    <div class="row">
                                                                                        <asp:FileUpload ID="FileUpload1" runat="server" class="dropify" AllowMultiple="true" onchange="ConfirmUpload(this.value);" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="btncontainer">
                                                                                <asp:Panel ID="pnlDocumentButtons" runat="server">
                                                                                    <div class="btnlinks">
                                                                                        <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" OnClientClick="return checkdelete();">Delete</asp:LinkButton>
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
                                                                                                3.
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

                                                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Documents" PostBackControls="lblName" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Estimate" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Documents" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                                                PagerStyle-AlwaysVisible="true" OnNeedDataSource="RadGrid_Documents_NeedDataSource"
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

                                                                                                        <%-- <telerik:GridTemplateColumn SortExpression="filename" HeaderText="File Name" DataField="filename" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <%--<asp:LinkButton ID="lblName" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Path") %>'
                                                                                                                    Text='<%# Eval("filename") %>' CommandName="D"> </asp:LinkButton>   --comment here
                                                                                                                <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                                                                                    CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                                                                    OnClientClick="return ViewDocumentClick(this);" OnClick="lblName_Click" Text='<%# Eval("filename") %>'>
                                                                                                                </asp:LinkButton>
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>--%>

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

                                                                                                        <telerik:GridTemplateColumn SortExpression="portal" HeaderText="Portal" DataField="portal" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# (Eval("portal")!=DBNull.Value) ? Convert.ToBoolean(Eval("portal")): false %>' />
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>

                                                                                                        <%-- <telerik:GridTemplateColumn SortExpression="MSVisible" HeaderText="Mobile Service" DataField="MSVisible" ShowFilterIcon="false">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>--%>

                                                                                                        <telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                                                                                            HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="100"
                                                                                                            DataType="System.Int16" UniqueName='MSVisible'>
                                                                                                            <FilterTemplate>
                                                                                                                <telerik:RadComboBox RenderMode="Auto" ID="ImportedFilter" runat="server" OnClientSelectedIndexChanged="ImportedFilterSelectedIndexChanged"
                                                                                                                    SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("MSVisible").CurrentFilterValue %>'
                                                                                                                    Width="100px">
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
                                                                            <%--</ContentTemplate>
                                                                            </asp:UpdatePanel>--%>
                                                                        </div>
                                                                        <div style="clear: both;"></div>
                                                                    </div>
                                                                </div>
                                                                <%--<div id="five" runat="server" class="col s12 tab-container-border lighten-4" style="display: none;">
                                                                    <div class="row">
                                                                        

                                                                    </div>
                                                                </div>--%>
                                                                <div id="six" class="col s12 tab-container-border lighten-4" style="display: none;">
                                                                    <div class="row">
                                                                        <div class="tab-container-content" style="overflow: auto;">
                                                                            <div class="tabs-custom-mgn1 mn-ht">
                                                                                <div class="grid_container">
                                                                                    <asp:HiddenField ID="hdnLineOpenned" runat="server" />
                                                                                    <asp:HiddenField ID="hdnOrgMemberKey" runat="server" />
                                                                                    <asp:HiddenField ID="hdnOrgMemberDisp" runat="server" />
                                                                                    <asp:HiddenField ID="hdnOrgUserRoleID" runat="server" />
                                                                                    <asp:HiddenField ID="hdnOrgUserRoleDisp" runat="server" />
                                                                                    <asp:Panel ID="pnlEstTags" runat="server">
                                                                                        <%--<div class="grid_container" style="margin-bottom: 0 !important;">--%>
                                                                                        <div class="RadGrid RadGrid_Material FormGrid gridHeaderCustom">
                                                                                            <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_EstTags" ShowFooter="false" PageSize="50"
                                                                                                    PagerStyle-AlwaysVisible="false" OnItemDataBound="RadGrid_EstTags_ItemDataBound"
                                                                                                    ShowStatusBar="true" runat="server" AllowPaging="True" Width="100%" AllowCustomPaging="false">
                                                                                                    <CommandItemStyle />
                                                                                                    <GroupingSettings CaseSensitive="false" />
                                                                                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false">
                                                                                                        <Selecting AllowRowSelect="false"></Selecting>
                                                                                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                                    </ClientSettings>
                                                                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">
                                                                                                        <Columns>

                                                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="10" AllowFiltering="false" ItemStyle-Width="0.5%" FooterStyle-Width="0.5%">

                                                                                                                <ItemTemplate>
                                                                                                                    <%--<asp:HiddenField ID="txtOrderNo" Value='<%# Eval("OrderNo") %>' runat="server"></asp:HiddenField>--%>
                                                                                                                    <asp:Label ID="lblIndex" Visible="true" runat="server" Text="<%# Container.ItemIndex +1 %>"></asp:Label>
                                                                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Style="display: none;"></asp:Label>
                                                                                                                    <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' Style="display: none;" class="customline"></asp:Label>
                                                                                                                    <asp:Label ID="lblFormat" runat="server" Text='<%# Eval("Format") %>' Visible="false"></asp:Label>
                                                                                                                    <asp:Label ID="lblCustom" runat="server" Text='<%# Eval("Label") %>' Style="display: none;"></asp:Label>
                                                                                                                </ItemTemplate>

                                                                                                            </telerik:GridTemplateColumn>

                                                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="100" AllowFiltering="false" HeaderText="Name" HeaderStyle-CssClass="itemHeader">
                                                                                                                <ItemTemplate>
                                                                                                                    <div style="text-align: left">
                                                                                                                        <asp:HiddenField ID="hdnTestItemValueID" Value='' runat="server"></asp:HiddenField>
                                                                                                                        <asp:Panel ID="divFormatText" runat="server" Visible="false">
                                                                                                                            <label class="fontHeader" for="txtFormatText"><%# Eval("Label") %></label>
                                                                                                                            <asp:TextBox ID="txtFormatText" runat="server" Text='' MaxLength="255"></asp:TextBox>
                                                                                                                        </asp:Panel>

                                                                                                                        <asp:Panel ID="divFormatDrop" runat="server" Visible="false">
                                                                                                                            <label class="fontHeader" for="drpdwnCustom"><%# Eval("Label") %></label>
                                                                                                                            <asp:DropDownList ID="drpdwnCustom" runat="server" CssClass="browser-default fontHeader">
                                                                                                                            </asp:DropDownList>
                                                                                                                        </asp:Panel>
                                                                                                                        <asp:Panel ID="divFormatCurrent" runat="server" Visible="false">
                                                                                                                            <label class="fontHeader" for="txtFormatCurrent"><%# Eval("Label") %></label>
                                                                                                                            <asp:TextBox ID="txtFormatCurrent" runat="server" Text=''
                                                                                                                                CssClass="custom currency"></asp:TextBox>
                                                                                                                        </asp:Panel>
                                                                                                                        <asp:Panel ID="divFormatDate" runat="server" Visible="false">
                                                                                                                            <label class="fontHeader" for="txtFormatDate"><%# Eval("Label") %></label>
                                                                                                                            <asp:TextBox ID="txtFormatDate" runat="server" Text=''
                                                                                                                                CssClass="custom datepicker_mom"></asp:TextBox>
                                                                                                                        </asp:Panel>
                                                                                                                        <asp:Panel ID="divFormatCheckbox" runat="server" Visible="false">
                                                                                                                            <asp:CheckBox ID="chkCustomFormat" runat="server" Text='&nbsp'
                                                                                                                                CssClass="css-checkbox custom"></asp:CheckBox>
                                                                                                                            <label class="fontHeader" for="chkCustomFormat"><%# Eval("Label") %></label>
                                                                                                                        </asp:Panel>
                                                                                                                        <asp:Panel ID="divFormatNotes" runat="server" Visible="false">
                                                                                                                            <label class="fontHeader" for="txtFormatNotes"><%# Eval("Label") %></label>
                                                                                                                            <asp:TextBox ID="txtFormatNotes" runat="server" Text='' MaxLength="255" TextMode="MultiLine"
                                                                                                                                Style="padding: 0.4rem 0 !important" CssClass="materialize-textarea textarea-border1">
                                                                                                                            </asp:TextBox>
                                                                                                                        </asp:Panel>
                                                                                                                        <asp:Panel ID="divFormatChkWithComment" runat="server" Visible="false">
                                                                                                                            <asp:CheckBox ID="chkWithComment" runat="server" Text='&nbsp'
                                                                                                                                CssClass="css-checkbox custom"></asp:CheckBox>
                                                                                                                            <label class="fontHeader" for="chkWithComment"><%# Eval("Label") %></label>
                                                                                                                            <asp:TextBox ID="txtChkComment" placeholder="Checkbox comment" runat="server" Text='' MaxLength="255"></asp:TextBox>

                                                                                                                        </asp:Panel>
                                                                                                                    </div>

                                                                                                                </ItemTemplate>
                                                                                                                <FooterStyle VerticalAlign="Middle" />

                                                                                                            </telerik:GridTemplateColumn>

                                                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="20" AllowFiltering="false" HeaderText="Alert " HeaderStyle-CssClass="itemHeader">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:CheckBox ID="chkSelectAlert" CssClass="css-checkbox" Text=" " runat="server" />
                                                                                                                </ItemTemplate>
                                                                                                                <FooterStyle VerticalAlign="Middle" />

                                                                                                            </telerik:GridTemplateColumn>
                                                                                                            <%--<telerik:GridTemplateColumn HeaderStyle-Width="20" AllowFiltering="false" HeaderText="Task " HeaderStyle-CssClass="itemHeader">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:CheckBox ID="chkSelectTask" CssClass="css-checkbox" Text=" " runat="server" />
                                                                                                                            </ItemTemplate>
                                                                                                                            <FooterStyle VerticalAlign="Middle" />

                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="100" AllowFiltering="false"  HeaderText="User Role" HeaderStyle-CssClass="itemHeader">
                                                                                                                            <ItemTemplate>
                                                                                                                                <div class="tag-div materialize-textarea textarea-border" id="cusLabelTagUR" style="text-align: left !important; cursor: pointer;" onclick="ShowUserRoleWindow(this);" runat="server"></div>
                                                                                                                                <asp:HiddenField ID="hdnUserRoles" runat="server" Value='<%# Eval("UserRole") %>' />
                                                                                                                                <asp:TextBox ID="txtUserRoles" class='<%# "txtUserRoles_" + Eval("Line") %>' runat="server" Style='min-width: 100px !important; display: none;'
                                                                                                                                    Text='<%# Eval("UserRoleDisplay") %>'></asp:TextBox>
                                                                                                                            </ItemTemplate>
                                                                                                                            <FooterStyle VerticalAlign="Middle" />
                                                                                                                        </telerik:GridTemplateColumn>--%>
                                                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="100" AllowFiltering="false" HeaderText="Team Member" HeaderStyle-CssClass="itemHeader">

                                                                                                                <ItemTemplate>
                                                                                                                    <div class="tag-div materialize-textarea textarea-border" id="cusLabelTag" style="text-align: left !important; cursor: pointer;" onclick="ShowTeamMemberWindow(this);" runat="server"></div>
                                                                                                                    <asp:HiddenField ID="hdnMembers" runat="server" Value='<%# Eval("TeamMember") %>' />
                                                                                                                    <asp:TextBox ID="txtMembers" class='<%# "txtMembers_" + Eval("Line") %>' runat="server" Style='min-width: 100px !important; display: none;'
                                                                                                                        Text='<%# Eval("TeamMemberDisplay") %>'></asp:TextBox>
                                                                                                                </ItemTemplate>

                                                                                                            </telerik:GridTemplateColumn>

                                                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="50" AllowFiltering="false" HeaderText="Updated By" HeaderStyle-CssClass="itemHeader">
                                                                                                                <ItemTemplate>
                                                                                                                    <%--<asp:Label ID="lbUpdatedBy" runat="server"><%# Eval("Username") %></asp:Label>--%>
                                                                                                                    <asp:Label ID="lbUpdatedBy" runat="server" Text='<%# Eval("Username") %>'></asp:Label>
                                                                                                                </ItemTemplate>


                                                                                                            </telerik:GridTemplateColumn>
                                                                                                            <telerik:GridTemplateColumn HeaderStyle-Width="50" AllowFiltering="false" HeaderText="Updated Date" HeaderStyle-CssClass="itemHeader">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lbUpdatedDate" runat="server"><%# Eval("UpdatedDate", "{0:MM/dd/yyyy HH:mm tt}") %></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                        </Columns>
                                                                                                    </MasterTableView>
                                                                                                </telerik:RadGrid>
                                                                                            </telerik:RadAjaxPanel>

                                                                                        </div>
                                                                                        <%--</div>--%>
                                                                                        <div class="cf"></div>
                                                                                    </asp:Panel>

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div id="seven" class="col s12 tab-container-border lighten-4" style="display: none;">
                                                                    <div class="row">
                                                                        <div class="tab-container-content" style="overflow: auto;">
                                                                            <div class="tabs-custom-mgn1 mn-ht">
                                                                                <div class="form-section-row">
                                                                                    <div>
                                                                                        <asp:CheckBox ID="chkShowAllTasks" AutoPostBack="true" OnCheckedChanged="chkShowAllTasks_CheckedChanged" CssClass="css-checkbox" runat="server" Text="Show All Tasks" />
                                                                                        <asp:HiddenField ID="hdnShowAllTasks" runat="server"></asp:HiddenField>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="grid_container">
                                                                                    <asp:Panel ID="Panel5" runat="server">
                                                                                        <div class="RadGrid RadGrid_Material FormGrid">
                                                                                            <telerik:RadCodeBlock ID="RadCodeBlock_Tasks" runat="server">
                                                                                                <script type="text/javascript">
                                                                                                    function pageLoad() {
                                                                                                        var grid = $find("<%= RadGrid_Tasks.ClientID %>");
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
                                                                                            <telerik:RadAjaxPanel ID="RadAjaxPanel_Tasks" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Task" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Tasks" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                                                    PagerStyle-AlwaysVisible="true"
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

                                                                                                            <telerik:GridBoundColumn DataField="Contact" HeaderText="Contact Name" SortExpression="Contact"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                                                                ShowFilterIcon="false">
                                                                                                            </telerik:GridBoundColumn>

                                                                                                            <telerik:GridTemplateColumn SortExpression="Subject" HeaderText="Subject" HeaderStyle-Width="200" DataField="Subject" ShowFilterIcon="false">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:HyperLink ID="lnkSubject" NavigateUrl='<%# "addtask.aspx?uid=" + Eval("id") + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl) %>'
                                                                                                                        Target="_self" runat="server" Text='<%# Eval("Subject") %>'></asp:HyperLink>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>

                                                                                                            <telerik:GridTemplateColumn SortExpression="duedate" HeaderText="Due Date/Date Done" HeaderStyle-Width="150" DataField="duedate" ShowFilterIcon="false">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblDuedate" runat="server" Style='<%#  Eval("statusid").ToString()!="1" ?( string.Format("color:{0}",Convert.ToDateTime( Eval("duedate") )<= System.DateTime.Now ? "RED": "BLACK")) : "Black" %>'
                                                                                                                        Text='<%# (String.IsNullOrEmpty(Eval("duedate").ToString())) ? "No Date Available" : Eval("duedate", "{0:MM/dd/yyyy h:mm tt}") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>

                                                                                                            <telerik:GridTemplateColumn SortExpression="days" HeaderText="# Days" HeaderStyle-Width="80" DataField="days" ShowFilterIcon="false">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lbldays" runat="server" Text='<%#   Eval("days").ToString().Replace("-",String.Empty) %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>

                                                                                                            <telerik:GridBoundColumn DataField="Remarks" HeaderText="Desc" HeaderStyle-Width="200"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Remarks"
                                                                                                                ShowFilterIcon="false">
                                                                                                            </telerik:GridBoundColumn>

                                                                                                            <telerik:GridBoundColumn DataField="result" HeaderText="Resolution" HeaderStyle-Width="200"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="result"
                                                                                                                ShowFilterIcon="false">
                                                                                                            </telerik:GridBoundColumn>

                                                                                                            <telerik:GridBoundColumn DataField="fUser" HeaderText="Assigned to" HeaderStyle-Width="140"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="fUser"
                                                                                                                ShowFilterIcon="false">
                                                                                                            </telerik:GridBoundColumn>

                                                                                                            <telerik:GridBoundColumn DataField="status" HeaderText="Status" HeaderStyle-Width="120"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="status"
                                                                                                                ShowFilterIcon="false">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="keyword" HeaderText="Category" HeaderStyle-Width="120"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="keyword"
                                                                                                                ShowFilterIcon="false">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="CreatedBy" HeaderText="Created By" HeaderStyle-Width="120"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="CreatedBy"
                                                                                                                ShowFilterIcon="false">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridTemplateColumn SortExpression="CreatedDate" HeaderText="Created Date" HeaderStyle-Width="150" DataField="CreatedDate" ShowFilterIcon="false">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblCreatedDate" runat="server"
                                                                                                                        Text='<%# (String.IsNullOrEmpty(Eval("CreatedDate").ToString())) ? "" : Eval("CreatedDate", "{0:MM/dd/yyyy h:mm tt}") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                            <telerik:GridBoundColumn DataField="screen" HeaderText="Screen" HeaderStyle-Width="120"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="screen"
                                                                                                                ShowFilterIcon="false">
                                                                                                            </telerik:GridBoundColumn>
                                                                                                            <telerik:GridBoundColumn DataField="ref" HeaderText="Ref ID" HeaderStyle-Width="100"
                                                                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="ref"
                                                                                                                ShowFilterIcon="false">
                                                                                                            </telerik:GridBoundColumn>

                                                                                                        </Columns>
                                                                                                    </MasterTableView>
                                                                                                </telerik:RadGrid>
                                                                                            </telerik:RadAjaxPanel>
                                                                                        </div>
                                                                                    </asp:Panel>
                                                                                </div>
                                                                                <div class="cf"></div>

                                                                            </div>
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
                </div>
            </div>
        </div>

        <div class="container accordian-wrap">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li id="tbProposals" runat="server">
                            <div id="accrdProposals" class="collapsible-header accrd accordian-text-custom"><i class="mdi-av-web"></i>Proposals</div>
                            <div class="collapsible-body" id="divProposals">
                                <div class="tab-container-content">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="section-ttle">Proposals</div>
                                            <div class="form-section-row">
                                                <div class="col s12 m12 l12">
                                                    <div class="row">
                                                        <asp:FileUpload ID="FileUpload2" runat="server" class="dropify" AllowMultiple="false" onchange="ConfirmUploadProposal(this.value);" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="btncontainer">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnGenerate" OnClick="btnGenerate_Click" runat="server" Text="Generate" Visible="false"></asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnSendEmail" CausesValidation="false" runat="server" OnClick="btnSendEmail_Click" Text="Send Email" Visible="false"></asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkDeleteForms" runat="server" CausesValidation="False" OnClick="lnkDeleteForms_Click" OnClientClick="return SelectedRowDelete('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvDocuments','form');">Delete</asp:LinkButton>
                                                </div>
                                                <asp:LinkButton ID="lnkUploadProposal" runat="server" CausesValidation="False" OnClick="lnkUploadProposal_Click" Style="display: none"></asp:LinkButton>
                                                <asp:LinkButton ID="lnkPostbackUploadProposal" runat="server" CausesValidation="False" Style="display: none"></asp:LinkButton>
                                                <%--<div class="form-section-row">
                                                    <asp:Label runat="server" ID="lblError" ForeColor="Red"></asp:Label>
                                                </div>--%>
                                            </div>
                                            <div class="grid_container">
                                                <div class="RadGrid RadGrid_Material  FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock_Forms" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%= RadGrid_Forms.ClientID %>");
                                                                try {
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

                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Forms" PostBackControls="lnkFileName,lnkPdfFileName" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Estimate" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Forms" AllowFilteringByColumn="false" ShowFooter="True" PageSize="100"
                                                            PagerStyle-AlwaysVisible="true"
                                                            ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="false" Width="100%" AllowCustomPaging="True">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">
                                                                <Columns>
                                                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="28">
                                                                    </telerik:GridClientSelectColumn>

                                                                    <telerik:GridTemplateColumn AllowFiltering="false" Visible="false" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField runat="server" ID="hdID" Value='<%# Eval("id") %>' />
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridTemplateColumn ShowFilterIcon="false" AllowFiltering="false" HeaderStyle-Width="50">
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink runat="server" ID="Image2" ImageWidth="15px" Visible='<%# Eval("IsLatest").ToString().ToLower() == "true"%>' ToolTip="Latest Proposal"
                                                                                onclick='DownloadLatestProposal();'
                                                                                ImageUrl='<%# Eval("IsLatest").ToString().ToLower() == "true"  ? "images/Document.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>'>
                                                                            </asp:HyperLink>

                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="name" HeaderText="Name" HeaderStyle-Width="140"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Name"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridTemplateColumn FilterDelay="5" SortExpression="filename" HeaderText="File Name" DataField="filename" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkFileName" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ID")%>'
                                                                                OnClick="lnkFileName_Click" Text='<%# Eval("filename") %>'> 
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <%-- <telerik:GridTemplateColumn DataField="filename" SortExpression="filename" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                        HeaderText="File Name" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                                                CommandArgument='<%#Eval("filename")%>'
                                                                                OnClick="lnkFileName_Click" Text='<%# Eval("filename") %>'>
                                                                            </asp:LinkButton>

                                                                        </ItemTemplate>

                                                                    </telerik:GridTemplateColumn>--%>

                                                                    <telerik:GridTemplateColumn FilterDelay="5" SortExpression="filename" HeaderText="PDF File" DataField="filename" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkPdfFileName" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ID")%>'
                                                                                OnClick="lnkPdfFileName_Click" Text='<%# Eval("JobTID").ToString() == "0" ? "" : Eval("filename").ToString().Replace(".docx",".pdf") %>'>
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="AddedOn" HeaderText="Created Date" HeaderStyle-Width="140"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="AddedOn"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="AddedBy" HeaderText="Created By" HeaderStyle-Width="140"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="AddedBy"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="SendTo" HeaderText="Send To" HeaderStyle-Width="140"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="SendTo"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="SendFrom" HeaderText="Send By" HeaderStyle-Width="140"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="SendFrom"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="SendOn" HeaderText="Send On" HeaderStyle-Width="140"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="SendOn"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </telerik:RadAjaxPanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-section-row">
                                            <div class="section-ttle">Approved Status History</div>
                                            <div class="grid_container">
                                                <div class="RadGrid RadGrid_Material  FormGrid">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoad() {
                                                                var grid = $find("<%= RadGrid_ApprovedStatusHistory.ClientID %>");
                                                                try {
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

                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel5" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Estimate" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_ApprovedStatusHistory" AllowFilteringByColumn="true" ShowFooter="True" PageSize="100"
                                                            OnNeedDataSource="RadGrid_ApprovedStatusHistory_NeedDataSource"
                                                            PagerStyle-AlwaysVisible="true"
                                                            ShowStatusBar="true" runat="server" AllowPaging="true" AllowSorting="false" Width="100%" AllowCustomPaging="True">
                                                            <CommandItemStyle />
                                                            <GroupingSettings CaseSensitive="false" />
                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                            </ClientSettings>
                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="OldStatusName" HeaderText="Old Status" HeaderStyle-Width="160"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="OldStatus"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="NewStatusName" HeaderText="New Status" HeaderStyle-Width="160"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="NewStatus"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="UpdatedBy" HeaderText="Approved By" HeaderStyle-Width="160"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="UpdatedBy"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="UpdatedDate" HeaderText="Approved Date" HeaderStyle-Width="200"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="UpdatedDate"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                    <telerik:GridBoundColumn FilterDelay="5" DataField="ApprovedComment" HeaderText="Approved Comment"
                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="ApprovedComment"
                                                                        ShowFilterIcon="false">
                                                                    </telerik:GridBoundColumn>

                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </telerik:RadAjaxPanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-section-row">
                                            <div class="section-ttle">Email History Log</div>
                                            <div class="grid_container">
                                                <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                        <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
                                                            <script type="text/javascript">
                                                                function pageLoadLog() {
                                                                    try {
                                                                        var grid = $find("<%= RadGrid_EmailLogs.ClientID %>");
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
                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel6" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Estimate" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_EmailLogs" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true" OnItemCreated="RadGrid_EmailLogs_ItemCreated"
                                                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" AllowCustomPaging="True" OnNeedDataSource="RadGrid_EmailLogs_NeedDataSource">
                                                                <CommandItemStyle />
                                                                <GroupingSettings CaseSensitive="false" />
                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                </ClientSettings>
                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" AllowNaturalSort="false">
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn DataField="EmailDate" SortExpression="EmailDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                            CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbldate" runat="server" Text='<%# Eval("EmailDate", "{0:M/d/yyyy}")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="EmailDate" SortExpression="EmailDate" AutoPostBackOnFilter="true" DataType="System.String"
                                                                            CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbltime" runat="server" Text='<%# Eval("EmailDate","{0: hh:mm tt}") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="Username" SortExpression="Username" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUsername" runat="server" Text='<%# Eval("Username") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <%--<telerik:GridTemplateColumn DataField="Ref" SortExpression="Ref" AutoPostBackOnFilter="true" DataType="System.String"
                                                                            CurrentFilterFunction="Contains" HeaderText="Ticket #" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>--%>
                                                                        <telerik:GridTemplateColumn DataField="EmailFunction" SortExpression="EmailFunction" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Function" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblEmailFunction" runat="server" Text='<%# Eval("EmailFunction") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="MailTo" SortExpression="MailTo" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Mail To" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblEmailTo" runat="server" Text='<%# Eval("MailTo") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="Status" SortExpression="Status" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Status" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="UsrErrMessage" SortExpression="UsrErrMessage" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Error Message" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblUsrErrMessage" runat="server" Text='<%# Eval("UsrErrMessage") %>'></asp:Label>
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
                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li runat="server" id="adContacts">
                            <div id="accrdcontacts" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-account-circle"></i>Contacts</div>
                            <div class="collapsible-body" id="divContacts">
                                <div class="tab-container-content">
                                    <div class="form-content-pd">
                                        <div class="btncontainer">
                                            <asp:Panel ID="pnlContactButtons" runat="server">
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkAddnewContact" Visible="false" runat="server" CausesValidation="False" OnClick="lnkAddnewContact_Click">Add</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnEditContact" Visible="false" runat="server" CausesValidation="False"
                                                        OnClick="btnEditContact_Click">Edit</asp:LinkButton>
                                                </div>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="btnDeleteContact" Visible="false" runat="server"
                                                        OnClientClick="return DeleteContactClick(this);"
                                                        CausesValidation="False"
                                                        OnClick="btnDeleteContact_Click">Delete</asp:LinkButton>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <div class="grid_container">
                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                <telerik:RadCodeBlock ID="RadCodeBlock_Prospect" runat="server">
                                                    <script type="text/javascript">
                                                        function pageLoad() {
                                                            var grid = $find("<%= RadGrid_Contacts.ClientID %>");
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
                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_Contacts" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Task" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Contacts" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                        OnNeedDataSource="RadGrid_Contacts_NeedDataSource" PagerStyle-AlwaysVisible="true"
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

                                                                <telerik:GridTemplateColumn DataField="Name" HeaderText="Name" SortExpression="Name"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="140"
                                                                    ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactName" Text='<%# Eval("Name")%>' runat="server"></asp:Label>
                                                                        <asp:HiddenField ID="hdnContactID" Value='<%# Bind("ContactId") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Title" HeaderText="Title" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Title"
                                                                    ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactTitle" Text='<%# Eval("Title")%>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Phone" HeaderText="Phone" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Phone"
                                                                    ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactPhone" Text='<%#Eval("Phone")%>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn DataField="Fax" HeaderText="Fax" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Fax"
                                                                    ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactFax" Text='<%#Eval("Fax")%>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn DataField="Cell" HeaderText="Cell" HeaderStyle-Width="140"
                                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Cell"
                                                                    ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactCell" Text='<%#Eval("Cell")%>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderStyle-Width="140" HeaderText="Email" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Email" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <a href='<%# "email.aspx?to=" + Eval("Email") +"&rol="+hdnROLId.Value %>' target="_self">
                                                                            <asp:Label ID="lblEmail" Text='<%#Eval("Email")%>' runat="server"></asp:Label></a>
                                                                    </ItemTemplate>
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
                        </li>

                        <%--<li id="Li1" runat="server">
                            <div id="accrdEstimateTags" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Custom</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">

                                            
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>--%>

                        <li id="tbLogs" runat="server" style="display: none">
                            <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Logs</div>
                            <div class="collapsible-body">
                                <div class="tab-container-content">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                <div class="RadGrid RadGrid_Material">
                                                    <telerik:RadCodeBlock ID="RadCodeBlock6" runat="server">
                                                        <script type="text/javascript">
                                                            function pageLoadLog() {
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
    <telerik:RadWindow ID="ProjectWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
        runat="server" Modal="true" Width="1050" Height="635">
        <ContentTemplate>
            <telerik:RadAjaxPanel ID="RadAjaxPanel4" runat="server">
                <div style="margin-top: 15px;">
                    <div class="form-section-row">
                        <div class="form-section">
                            <div class="row" style="margin-bottom: 0;">
                                <div class="grid_container" id="divEstimateGrid" runat="server">
                                    <div class="RadGrid RadGrid_Material RadGrid_Popup">
                                        <telerik:RadGrid RenderMode="Auto" CssClass="ProjectGrid" ID="RadGrid_Project" AllowFilteringByColumn="true" ShowFooter="True" PageSize="5000"
                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%"
                                            PagerStyle-AlwaysVisible="true"
                                            OnNeedDataSource="RadGrid_Project_NeedDataSource"
                                            EmptyDataText="No Projects Found..."
                                            ClientSettings-Scrolling-UseStaticHeaders="true"
                                            ClientSettings-Scrolling-AllowScroll="true">

                                            <CommandItemStyle />
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="True"></Selecting>

                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                            </ClientSettings>
                                            <MasterTableView DataKeyNames="id" UseAllDataFields="true" AutoGenerateColumns="false" AllowFilteringByColumn="true" ShowFooter="false">
                                                <Columns>
                                                    <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                                    </telerik:GridClientSelectColumn>

                                                    <telerik:GridTemplateColumn DataField="ID"
                                                        UniqueName="ID"
                                                        SortExpression="ID"
                                                        AutoPostBackOnFilter="true"
                                                        AllowFiltering="true"
                                                        DataType="System.String"
                                                        CurrentFilterFunction="Contains" HeaderText="Project#" HeaderStyle-Width="90" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkName" runat="server" Text='<%# Bind("ID") %>'></asp:HyperLink>
                                                            <asp:HiddenField ID="hdnProjectID" runat="server" Value='<%# Eval("ID") %>'></asp:HiddenField>
                                                        </ItemTemplate>

                                                    </telerik:GridTemplateColumn>


                                                    <telerik:GridTemplateColumn DataField="fDate" SortExpression="fDate" AutoPostBackOnFilter="false" DataType="System.String"
                                                        CurrentFilterFunction="Contains" HeaderText="Date" ShowFilterIcon="false" HeaderStyle-Width="100" AllowFiltering="false"
                                                        UniqueName="fDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate" runat="server" Text='<%# (String.IsNullOrEmpty(Eval("fDate").ToString())) ? "" : Eval("fDate", "{0:M/d/yyyy}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="fdesc" HeaderText="Desc" SortExpression="fdesc" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                        ShowFilterIcon="false" HeaderStyle-Width="200">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="CloseDate" SortExpression="CloseDate" AutoPostBackOnFilter="false" DataType="System.String"
                                                        CurrentFilterFunction="Contains" HeaderText="Close Date" ShowFilterIcon="false" HeaderStyle-Width="100" AllowFiltering="false"
                                                        UniqueName="CloseDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCloseDate" runat="server" Text='<%# (String.IsNullOrEmpty(Eval("CloseDate").ToString())) ? "" : Eval("CloseDate", "{0:M/d/yyyy}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                </Columns>
                                            </MasterTableView>

                                        </telerik:RadGrid>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="margin-top: 10px!important;">
                    <div class="btnlinks">

                        <asp:LinkButton ID="btnSaveProject" runat="server" OnClick="btnSaveProject_Click">Save</asp:LinkButton>

                    </div>
                </div>
            </telerik:RadAjaxPanel>
        </ContentTemplate>
    </telerik:RadWindow>
    <telerik:RadWindowManager ID="RadWindowManagerEstimate" runat="server">
        <Windows>

            <telerik:RadWindow ID="RadWindowTemplate" Skin="Material" VisibleTitlebar="true" Title="Choose Template" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="400">
                <ContentTemplate>
                    <div class="form-section-row" style="margin-top: 10px!important;">
                        <div class="RadGrid RadGrid_Material  FormGrid">
                            <telerik:RadCodeBlock ID="RadCodeBlock_Contacts" runat="server">
                                <script type="text/javascript">
                                    function pageLoad() {
                                        var grid = $find("<%= gvEstimateTemplate.ClientID %>");
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

                            <telerik:RadAjaxPanel ID="RadAjaxPanel_gvEstimate" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Estimate" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                <telerik:RadGrid RenderMode="Auto" ID="gvEstimateTemplate" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                    PagerStyle-AlwaysVisible="true"
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
                                            <telerik:GridTemplateColumn HeaderStyle-Width="50" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" SortExpression="ID" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                    <asp:HiddenField runat="server" ID="hdID" Value='<%# Eval("ID") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Name" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="File Name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="FileName" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFileName" runat="server" Text='<%# Eval("FileName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Added By" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="AddedBy" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddedBy" runat="server" Text='<%# Eval("AddedBy") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </telerik:RadAjaxPanel>

                        </div>
                    </div>
                    <div style="margin-top: 10px!important;">
                        <div class="btnlinks">
                            <asp:LinkButton ID="btnGenerateTemplete" runat="server" OnClick="btnGenerateTemplete_Click" Text="Generate Template"></asp:LinkButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowContact" Skin="Material" VisibleTitlebar="true" Title="Add Contact" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="800" Height="255">
                <ContentTemplate>
                    <div>

                        <div class="form-section-row">
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtContcName"
                                            Display="None" ErrorMessage="Contact Name Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator12">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtContcName" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblContcName" AssociatedControlID="txtContcName">Contact Name</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtTitle" runat="server" ControlToValidate="txtTitle"
                                            Display="None" ErrorMessage="Title  Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtendertxtTitle"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidatortxtTitle">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblTitle" AssociatedControlID="txtTitle">Title</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtContPhone"
                                            Display="None" ErrorMessage="Phone Required" SetFocusOnError="True" ValidationGroup="cont"
                                            Enabled="False"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator13_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator13">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtContPhone" runat="server" MaxLength="22"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblPhone" AssociatedControlID="txtContPhone">Phone</asp:Label>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="form-section-row">
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtContFax" runat="server" MaxLength="22"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblFax" AssociatedControlID="txtContFax">Fax</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:TextBox ID="txtContCell" runat="server" MaxLength="22"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblCell" AssociatedControlID="txtContCell">Cell</asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-section3-blank">
                                &nbsp;
                            </div>
                            <div class="form-section3">
                                <div class="input-field col s12">
                                    <div class="row">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtContEmail"
                                            Display="None" ErrorMessage="Email Required" SetFocusOnError="True" ValidationGroup="cont"
                                            Enabled="False"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator16_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator16">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtContEmail"
                                            Display="None" ErrorMessage="Invalid Email" ValidationGroup="cont" SetFocusOnError="True"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:TextBox ID="txtContEmail" runat="server" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblEmail" AssociatedControlID="txtContEmail">Email</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>
                        <div class="btnlinks">
                            <asp:LinkButton ID="lnkContactSave" runat="server" OnClick="lnkContactSave_Click" ValidationGroup="cont">Save</asp:LinkButton>
                        </div>

                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowBOMType" Skin="Material" VisibleTitlebar="true" Title="Add BOM Type" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="600" Height="200">
                <ContentTemplate>
                    <div>
                        <div class="form-section-row">
                            <div class="input-field col s12">
                                <div class="row">
                                    <asp:RequiredFieldValidator ID="rfvBomType"
                                        runat="server" ControlToValidate="txtBomType" Display="None" ErrorMessage="Type is required"
                                        SetFocusOnError="True" ValidationGroup="Type"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender
                                        ID="vceBomType" runat="server" Enabled="True"
                                        PopupPosition="BottomLeft" TargetControlID="rfvBomType" />
                                    <asp:TextBox ID="txtBomType" runat="server" MaxLength="150"></asp:TextBox>
                                    <asp:Label runat="server" ID="lbltxtBomType" AssociatedControlID="txtBomType">BOM Type</asp:Label>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>
                        <div class="btnlinks">
                            <asp:LinkButton ID="lbtnTypeSubmit" runat="server" OnClick="lbtnTypeSubmit_Click" CausesValidation="true" ValidationGroup="Type">Save</asp:LinkButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>


            <telerik:RadWindow ID="RadWindowGroup" Skin="Material" VisibleTitlebar="true" Title="Add Group" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="460" Height="170">
                <ContentTemplate>
                    <div>
                        <div class="form-section-row">
                            <div class="input-field col s12">
                                <div class="row">
                                    <asp:RequiredFieldValidator ID="rfvGroupName"
                                        runat="server" ControlToValidate="txtGroupName" Display="None" ErrorMessage="Group Name is required"
                                        SetFocusOnError="True" ValidationGroup="popupGroupName"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender
                                        ID="vceGroupName" runat="server" Enabled="True"
                                        PopupPosition="BottomLeft" TargetControlID="rfvGroupName" />
                                    <asp:TextBox ID="txtGroupName" runat="server" MaxLength="150"></asp:TextBox>
                                    <asp:Label runat="server" ID="Label14" AssociatedControlID="txtGroupName">Group Name</asp:Label>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>
                        <div class="btnlinks">
                            <asp:LinkButton ID="lnkPopupUpdateGroup" runat="server" OnClick="lnkPopupUpdateGroup_Click" OnClientClick="EnablePopupValidation(); return true;" CausesValidation="true" ValidationGroup="popupGroupName">Save</asp:LinkButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowBOMLaborRate" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
                runat="server" Modal="true" Width="430" Height="210">
                <ContentTemplate>
                    <div style="margin-top: 15px;">
                        <div class="form-section-row">
                            <div class="form-section">
                                <div class="input-field col s12">
                                    <div class="row" style="margin-bottom: 5px;">
                                        <asp:RequiredFieldValidator ID="rfvBOMLaborRate"
                                            runat="server" ControlToValidate="txtPopupLaborRate" Display="None" ErrorMessage="Labor Rate is required"
                                            SetFocusOnError="True" ValidationGroup="popupBOMLaborRate"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender
                                            ID="ValidatorCalloutExtender8" runat="server" Enabled="True"
                                            PopupPosition="BottomLeft" TargetControlID="rfvBOMLaborRate" />
                                        <asp:TextBox ID="txtPopupLaborRate" runat="server" MaxLength="150" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                        <label for="txtPopupLaborRate">Labor Rate</label>
                                    </div>

                                </div>
                                <div class="row" style="margin-bottom: 10px;">
                                    <label>Are you sure you want to update all labor rate columns?</label>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;"></div>
                        <footer style="float: left; padding-left: 0 !important;">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkAllLaborRate_Yes" runat="server" CausesValidation="true" ValidationGroup="popupBOMLaborRate"
                                    OnClientClick="lnkAllLaborRate_Yes_ClientClick(); return false;">Yes</asp:LinkButton>
                                <asp:LinkButton ID="lnkAllLaborRate_No" runat="server" OnClientClick="lnkAllLaborRate_No_ClientClick(); return false;">No</asp:LinkButton>
                            </div>
                        </footer>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <%--   --%>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadWindow ID="TeamMembersWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
        runat="server" Modal="true" Width="1050" Height="635">
        <ContentTemplate>
            <telerik:RadAjaxPanel ID="RadAjaxPanel3" runat="server">
                <div style="margin-top: 15px;">
                    <div class="form-section-row">
                        <div class="form-section">
                            <div class="row" style="margin-bottom: 0;">
                                <div class="grid_container" id="divMemberGrid" runat="server">
                                    <div class="RadGrid RadGrid_Material RadGrid RadGrid_Popup">

                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Emails" AllowFilteringByColumn="true" ShowFooter="false" PageSize="1000"
                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                            AllowCustomPaging="false" Width="100%" Height="516px" OnPreRender="RadGrid_Emails_PreRender"
                                            OnNeedDataSource="RadGrid_Emails_NeedDataSource">
                                            <CommandItemStyle />
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="True"></Selecting>

                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True">
                                                <Columns>
                                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="28" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" Style="display: none;"><%#Eval("memberkey")%></asp:Label>
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemStyle Width="0px"></ItemStyle>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn
                                                        DataField="fUser" SortExpression="fUser" AutoPostBackOnFilter="true" DataType="System.String"
                                                        CurrentFilterFunction="Contains" HeaderText="User Name" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserName" runat="server"><%#Eval("fUser")%></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn
                                                        DataField="RoleName" SortExpression="RoleName" AutoPostBackOnFilter="true" DataType="System.String"
                                                        CurrentFilterFunction="Contains" HeaderText="User Role" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserRole" runat="server"><%#Eval("RoleName")%></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn SortExpression="IsTask" AllowFiltering="false" HeaderStyle-Width="100" ShowFilterIcon="false" DataField="IsTask" HeaderText="Task">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkTask" Checked='<%# (Convert.ToString(Eval("IsTask")) == "1") ? true : false %>' runat="server" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn
                                                        DataField="email" SortExpression="email" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" DataType="System.String" HeaderText="Email" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEmail" runat="server"><%#Eval("email")%></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn
                                                        DataField="usertype" SortExpression="usertype" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" HeaderText="Type" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblType" runat="server"><%#Eval("usertype")%></asp:Label>
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
                    </div>
                    <div style="clear: both;"></div>
                    <footer style="float: left; padding-left: 0 !important;">
                        <div class="btnlinks">
                            <a id="lnkPopupOK" onclick="CloseTeamMemberWindow();" style="cursor: pointer;">OK</a>
                        </div>
                    </footer>
                </div>
            </telerik:RadAjaxPanel>
        </ContentTemplate>
    </telerik:RadWindow>

    <input id="hdnCustId" runat="server" type="hidden" />
    <asp:HiddenField ID="hdnCon" runat="server" />
    <asp:HiddenField ID="hdnGroupUpdateMode" runat="server" />
    <asp:HiddenField ID="hdnBOMPrevSelectedRowIndex" runat="server" />
    <asp:HiddenField ID="hdnBillingPrevSelectedRowIndex" runat="server" />
    <asp:HiddenField ID="hdnSelectedGrid" runat="server" />
    <asp:HiddenField ID="hdnCustomJSON" runat="server" />

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewContact" Value="Y" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <style>
        [id$='gvBOM_GridData'], [id$='gvMilestones_GridData'] {
            height: 401px !important;
        }

            [id$='gvBOM_GridData'] .rgAltRow > td:not(.MatPart.qtyRequired.LabPart.TotalExt), [id$='gvBOM_GridData'] .rgRow > td:not(.MatPart.qtyRequired.LabPart.TotalExt), [id$='gvMilestones_GridData'] .rgAltRow > td:not(.MatPart.qtyRequired.LabPart.TotalExt), [id$='gvMilestones_GridData'] .rgRow > td:not(.MatPart.qtyRequired.LabPart.TotalExt) {
                background-color: transparent !important;
            }
    </style>

    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
    <script defer src="https://use.fontawesome.com/releases/v5.0.10/js/all.js"></script>
    <script type="text/javascript">
        function CloseProjectWindow() {
            var wnd = $find('<%=ProjectWindow.ClientID %>');
            wnd.Close();
        }

        function ShowProjectWindow() {
            var wnd = $find('<%=ProjectWindow.ClientID %>');
            wnd.set_title("Projects");
            wnd.Show();
        }
        ////////////////// Confirm Document Upload ////////////////////
       <%-- function ConfirmUpload(value) {
            //
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

        function ConfirmUploadProposal(value) {
            if (confirm('Are you sure you want to upload?')) { document.getElementById('<%= lnkUploadProposal.ClientID %>').click(); }
            else { document.getElementById('<%= lnkPostbackUploadProposal.ClientID %>').click(); }
        }

        function checkdelete() {

            return true;
            //return SelectedRowDelete('<%= RadGrid_Documents.ClientID %>', 'file');
        }

        function CheckDelete(Gridview) {

            var gv;
            if (Gridview.includes('gvBOM')) {
                gv = $("#<%= gvBOM.ClientID%>")
            }
            else {
                gv = $("#<%= gvMilestones.ClientID%>")
            }

            var len = gv.find('tr').find('input[id*= chkSelect]:checked').length;
            if (gv.find('input[id*= chkSelect]:checked').length == 0) {
                noty({
                    text: 'Please select items to delete.',
                    dismissQueue: true,
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: true,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: false
                });
                return false;
            }
            else if (gv.find('input[type="checkbox"]:checked').length > 0) {
                //itemJSON();
                //return confirm('Do you really want to delete this item ?');
                if (Gridview.includes('gvBOM')) {
                    var gvBomDataRow = $("[id$='gvBOM_GridData'] tbody tr");


                    var r = confirm('Do you really want to delete this item ?');
                    if (r === true) {


                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else if (Gridview.includes('gvMilestones')) {
                    var gvMileStoneDataRow = $("[id$='gvMilestones_GridData'] tbody tr");


                    var checkConfirmMile = confirm('Do you really want to delete this item ?');
                    if (checkConfirmMile === true) {

                        itemJSON();
                        return true;;
                    }
                    else {
                        return false;
                    }
                }
            }
        }


        $(document).ready(function () {
            console.log("$(document).ready 5");
            $("#<%=txtREPdesc.ClientID%>").attr('maxlength', '255');
            Materialize.updateTextFields();
        });

        function ValidBomMileStone() {
            var validation_check = false;
            var validation_message = "";
            $("[id$='gvBOM_GridData']").find('tbody tr').not('.rgNoRecords').each(function () {
                var $tr = $(this);
                var txtCode = $tr.find('input[id*=txtCode]').val();
                var ddlBType = $tr.find("[id$='ddlBType']").val();
                var txtMatItem = $tr.find("[id$='txtMatItem']").val();
                var txtQtyReq = $tr.find('input[id*=txtQtyReq]').val();
                var txtUM = $tr.find("[id$='txtUM']").val();
                var txtBudgetUnit = $tr.find("[id$='txtBudgetUnit']").val();
                var txtMatMod = $tr.find("[id$='txtMatMod']").val();
                var txtScope = $tr.find('input[id*=txtScope]').val();


                if (parseInt(ddlBType) < 1 || txtScope === "") {
                    if (parseInt(ddlBType) >= 1 || txtScope !== "" ||
                        (txtCode !== "" && txtCode !== undefined) || (txtMatItem.length !== 0 && txtMatItem !== undefined) || (txtQtyReq !== "0.00" && txtQtyReq !== undefined) ||
                        (txtUM !== "" && txtUM !== undefined) || (txtBudgetUnit !== "0.00" && txtBudgetUnit !== undefined) || (txtMatMod !== "0.00" && txtMatMod !== undefined)
                    ) {

                        if (parseInt(ddlBType) < 1) {
                            validation_check = true;
                            validation_message = "BOM type cannot be empty<br>";
                            return false;
                        }
                        if (txtScope === "" || txtScope === undefined) {
                            validation_check = true;
                            validation_message = "BOM description cannot be empty<br>";
                            return false;
                        }
                    }
                }
            });
            $("[id$='gvMilestones_GridData']").find('tbody tr').not('.rgNoRecords').each(function () {
                var $tr = $(this);
                var txtCode = $tr.find('input[id*=txtCode]').val();
                var txtAmount = $tr.find('input[id*=txtAmount]').val();
                var txtStype = $tr.find('input[id*=txtSType]').val();
                var txtMilesName = $tr.find('input[id*=txtName]').val();
                var txtRequiredBy = $tr.find('input[id*=txtRequiredBy]').val();
                var txtScope = $tr.find('input[id*=txtScope]').val();

                if (txtScope === "") {
                    if (txtCode !== "" || txtAmount !== "0.00" || txtStype !== "" || txtMilesName !== "" || txtRequiredBy !== "") {
                        validation_check = true;
                        validation_message = validation_message + "Milestones description cannot be empty";
                        return false;
                    }
                }
            });

            var estType = $("#<%=ddlEstimateType.ClientID%>").val();
            if (estType == 'bid') {
                var BillingPer = 0;
                $("[id$='gvMilestones_GridData'] tr").not('.rgNoRecords').each(function () {
                    var $tr = $(this);
                    var txtPerAmount = $tr.find('input[id*=txtPerAmount]').val();
                    if (txtPerAmount) {
                        BillingPer = parseFloat(BillingPer) + parseFloat(txtPerAmount);
                    }
                });

                if ($("[id$='gvMilestones_GridData'] tbody tr").not('.rgNoRecords').length > 0) {
                    if (BillingPer != 100) {
                        validation_check = true;
                        validation_message = validation_message + "Billing amount percentage is not correct.<br>";
                    }
                }
            } else {
                $("[id$='gvMilestones_GridData']").find('tbody tr').not('.rgNoRecords').each(function () {
                    var $tr = $(this);
                    var txtAmount = $tr.find('input[id*=txtAmount]').val();
                    var txtPrice = $tr.find('input[id*=txtPrice]').val();
                    var txtQuantity = $tr.find('input[id*=txtQuantity]').val();

                    if (txtAmount == "" || txtPrice == "" || txtQuantity == "") {
                        validation_check = true;
                        validation_message = validation_message + "Quantity/Price/Amount cannot be empty";
                        return false;
                    }
                });
            }

            if (validation_check == false) {
                return true;
            }
            else {
                //alert(validation_message);
                noty({ text: validation_message, type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
    </script>

    <script type="text/javascript">

        $(document).ready(function () {
            console.log("$(document).ready 6");
            //Materialize.updateTextFields();
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
                    'scrollTop': $target.offset().top - 125
                }, 900, 'swing');
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

            HideShowOnBillingTypeChange($('#<%= ddlPType.ClientID %>').val());

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });

        });

        function updatebomgridindex() {
            var grid1 = 1;
            $("[id$='gvBOM_GridData']  tbody > tr").each(function () {
                var cat = $(this).find('input:hidden[id*="hdnOrderNo"]');
                if (cat !== null && cat.length > 0) {
                    cat.val(grid1);
                    grid1 = grid1 + 1;
                }
            });
        }

        function updatemilgridindex() {
            var grid1 = 1;
            $("[id$='gvMilestones_GridData']  tbody > tr").each(function () {
                var cat = $(this).find('input:hidden[id*="hdnOrderNoMil"]');
                if (cat !== null && cat.length > 0) {
                    cat.val(grid1);
                    grid1 = grid1 + 1;
                }
            });
        }

        function pageLoad(sender, args) {


            console.log("pageLoad");
            //Materialize.updateTextFields();

            var query = "";
            function dtaa() {
                this.prefixText = null;
                // this.con = null;
                this.con = document.getElementById('<%=hdnCon.ClientID%>').value;
                this.custID = null;
            }
            $("[id$='gvBOM_GridData']").sortable({
                items: 'tr',
                handle: ".handle",
                cursor: 'move',
                axis: 'y',
                dropOnEmpty: false,
                start: function (e, ui) {
                    ui.item.addClass("selected");
                },
                stop: function (e, ui) {
                    ui.item.removeClass("selected", 1000, "swing");
                    updatebomgridindex();
                },
                receive: function (e, ui) {
                    $(this).find("tbody").append(ui.item);
                }
            });
            $("[id$='gvMilestones_GridData']").sortable({
                items: 'tr',
                handle: ".handle",
                cursor: 'move',
                axis: 'y',
                dropOnEmpty: false,
                start: function (e, ui) {
                    ui.item.addClass("selected");
                },
                stop: function (e, ui) {
                    ui.item.removeClass("selected", 1000, "swing");
                    updatemilgridindex();
                },
                receive: function (e, ui) {
                    $(this).find("tbody").append(ui.item);
                }
            });

            $("[id*=txtVendor].vendorS").autocomplete({
                source: function (request, response) {
                    //;
                    //var txtVendor = this.id;
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

                            noty({ text: 'Due to unexpected errors we were unable to load vendor name', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        }
                    });
                },
                select: function (event, ui) {
                    var txtVendor = this.id;
                    var hdnVendorID = document.getElementById(txtVendor.replace('txtVendor', 'hdnVendorId'));
                    var str = ui.item.Name;
                    var strId = ui.item.ID;
                    if (str == "No Record Found!") {
                        $(this).val("");
                        $(hdnVendorID).val("");
                    }
                    else {
                        $(this).val(str);
                        $(hdnVendorID).val(strId);
                    }

                    return false;
                },


                focus: function (event, ui) {
                    var txtVendor = this.id;
                    var str = ui.item.Name;
                    var strId = ui.item.ID;
                    if (str == "No Record Found!") {
                        $(this).val("");
                    }
                    else {
                        $(this).val(str);
                    }
                    return false;
                },
                minLength: 0,
                delay: 50


            }).click(function () {
                $(this).autocomplete('search', $(this).val());
            });



            $.each($(".vendorS"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.Name;
                    var result_value = item.ID;


                    var x = new RegExp('\\b' + query, 'ig');
                    try {
                        if (result_item != null) {
                            result_item = result_item.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>';
                            });
                        }


                    }
                    catch { }


                    if (result_item != null && result_item != "")
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>  " + result_item + " </a>")
                            .appendTo(ul);
                    else
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>  " + result_item + "</a>")
                            .appendTo(ul);


                };
            });

            $("td>[id*=txtUM]").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    var str = request.term;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetUnitMeasure",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {

                            response($.parseJSON(data.d));

                        },
                        error: function (result) {
                            //alert("Due to unexpected errors we were unable to load unit measure");
                            noty({ text: 'Due to unexpected errors we were unable to load unit measure', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        }
                    });
                },
                select: function (event, ui) {
                    if (ui.item.value == 0) {
                    }
                    else {
                        var txtUM = this.id;
                        var hdnUMID = document.getElementById(txtUM.replace('txtUM', 'hdnUMID'));
                        $(hdnUMID).val(ui.item.value);
                        $(this).val(ui.item.label);
                    }
                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).bind('click', function () { $(this).autocomplete("search"); });
            //$.each($(".searchinput"), function (index, item) {
            //    console.log("txtUM autocomplete");

            //    $(item)._renderItem = function (ul, item) {

            //        var ula = ul;
            //        var itema = item;
            //        var result_value = item.value;
            //        var result_item = item.label;

            //        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
            //        result_item = result_item.toString().replace(x, function (FullMatch, n) {
            //            return '<span class="highlight">' + FullMatch + '</span>'
            //        });

            //        return $("<li></li>")
            //            .data("item.autocomplete", item)
            //            .append("<a>" + result_item + "</a>")
            //            .appendTo(ul);

            //    };
            //});
            $("td>[id*=txtUM]").change(function () {
                var txtUM = $(this);
                var strUM = $(this).val();

                var txtUM1 = $(txtUM).attr('id');
                var hdnUMID = document.getElementById(txtUM1.replace('txtUM', 'hdnUMID'));

                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "AccountAutoFill.asmx/GetAutofillByUM",
                    data: '{"prefixText": "' + strUM + '"}',
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var ui = $.parseJSON(data.d);
                        if (ui.length == 0) {
                            var strUM = $(txtUM).val();
                            $(txtUM).val('');
                            noty({
                                text: 'UM \'' + strUM + '\' doesn\'t exist!',
                                type: 'warning',
                                layout: 'topCenter',
                                closeOnSelfClick: false,
                                timeout: false,
                                theme: 'noty_theme_default',
                                closable: true
                            });
                        }
                        else {
                            $(txtUM1).val(ui[0].label);
                            $(hdnUMID).val(ui[0].value);
                        }
                    },
                    error: function (result) {
                        //alert("Due to unexpected errors we were unable to load UM");
                        noty({ text: 'Due to unexpected errors we were unable to load UM', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    }
                });
            });

            $("[id*=txtSType]").autocomplete({

                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetServiceType",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            //alert("Due to unexpected errors we were unable to load service type");
                            noty({ text: 'Due to unexpected errors we were unable to load service type', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        }
                    });
                },
                select: function (event, ui) {
                    if (ui.item.value == 'AddNew') {
                        //redirect to another page 
                        //instead open popup to add code detail.
                        Showpopup();
                    }
                    if (ui.item.value == 0) {

                    }
                    else {
                        var txtSType = this.id;
                        var hdnType = document.getElementById(txtSType.replace('txtSType', 'hdnType'));
                        $(hdnType).val(ui.item.value);
                        $(this).val(ui.item.label);
                    }
                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).bind('click', function () { $(this).autocomplete("search"); })
            $.each($(".searchinput"), function (index, item) {
                console.log("txtSType autocomplete");
                $(item)._renderItem = function (ul, item) {
                    var result_value = item.value;
                    var result_item = item.label;
                    //var result_desc = item.acct;
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


            //Quant
            $("[id*=txtMatItem].txtMatItemSearch").autocomplete({

                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetInventoryItemSearch",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {

                            response($.parseJSON(data.d));

                        },
                        error: function (result) {
                            //alert("Due to unexpected errors we were unable to load job code");
                            noty({ text: 'Due to unexpected errors we were unable to load job code', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        }
                    });
                },
                select: function (event, ui) {
                    if (ui.item.value == 'AddNew') {
                        //redirect to another page 
                        //instead open popup to add code detail.
                        Showpopup();
                    }
                    if (ui.item.MatItem == 0) {

                    }
                    else {
                        var txtMatItem = this.id;
                        var hdnMatItem = document.getElementById(txtMatItem.replace('txtMatItem', 'hdnMatItem'));
                        var txtScope = document.getElementById(txtMatItem.replace('txtMatItem', 'txtScope'));
                        $(hdnMatItem).val(ui.item.MatItem);
                        $(txtScope).val(ui.item.fDesc);
                        // $(this).val(ui.item.MatItem + ', ' + ui.item.fDesc);
                        $(this).val(ui.item.Name);
                    }
                    return false;
                },
                focus: function (event, ui) {
                    //$(this).val(ui.item.label);
                    //$(this).val(ui.item.value + ', ' + ui.item.label);
                    //$(this).val(ui.item.MatItem + ', ' + ui.item.fDesc);
                    var txtMatItem = this.id;
                    var txtScope = document.getElementById(txtMatItem.replace('txtMatItem', 'txtScope'));
                    $(txtScope).val(ui.item.fDesc);
                    $(this).val(ui.item.Name);
                    return false;
                    //$(this).autocomplete('search', $(this).val())
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            });

            $.each($(".txtMatItemSearch"), function (index, item) {
                //.bind('click', function () { $(this).autocomplete("search"); })
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_value = item.value;
                    //var result_item = item.value + ', ' + item.label;
                    var result_item = item.Name + ', ' + item.fDesc;
                    //var result_desc = item.acct;
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
            //Materialize.updateTextFields();
            SelectRowsEq1();
            //Materialize.updateTextFields();

            // -----------txtCode-----------
            function objCode() {
                this.prefixText = null;
                this.DeptName = null;
            }

            $("[id*=txtCode]").autocomplete({

                source: function (request, response) {


                    var txtJobType = document.getElementById("<%=txtJobType.ClientID %>");

                    var DeptName = txtJobType.value;

                    var objCode1 = new objCode();
                    objCode1.prefixText = request.term;
                    objCode1.DeptName = DeptName;

                    query = request.term;

                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetJobCodeByDeptName",
                        data: JSON.stringify(objCode1),
                        dataType: "json",
                        async: true,
                        success: function (data) {

                            response($.parseJSON(data.d));

                        },
                        error: function (result) {

                        }
                    });
                },
                select: function (event, ui) {
                    if (ui.item.value == 'AddNew') {
                        Showpopup();
                    }

                    if (ui.item.value == 0) {

                    }
                    else {
                        var txtCode = this.id;
                        var hdnCode = document.getElementById(txtCode.replace('txtCode', 'hdnCode'));
                        $(hdnCode).val(ui.item.value);
                        $(this).val(ui.item.label);
                        var lblCodeDesc = document.getElementById(txtCode.replace('txtCode', 'lblCodeDesc'));
                        $(lblCodeDesc).val(ui.item.CodeDesc);
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
                $(this).autocomplete('search', $(this).val());
            });

            $.each($(".txtCodeAutoComplate"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.label;
                    var result_value = item.value;
                    var result_itemfdesc = item.CodeDesc;

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
                    }
                    catch { }

                    if (result_item != null && result_itemfdesc != "")
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>  " + result_item + ", <span style='color:Gray;'><b>  </b>" + result_itemfdesc + "</span></a>")
                            .appendTo(ul);
                    else
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>  " + result_item + "</a>")
                            .appendTo(ul);


                };
            });

            //Contact autopopulate
            $("#<%=txtCont.ClientID%>").autocomplete({

                source: function (request, response) {

                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    dtaaa.custID = 0;
                    if ($('#<%=hdProspect.ClientID%>').val() != '' && $('#<%=hdProspect.ClientID%>').val() != '0') {
                        dtaaa.isProspect = 1;
                        dtaaa.custID = $('#<%=hdProspect.ClientID%>').val();
                    } else {
                        dtaaa.isProspect = 0;
                        if (document.getElementById('<%=txtCompanyName.ClientID%>').value != '') {
                            if (document.getElementById('<%=hdnCustId.ClientID%>').value != '') {
                                dtaaa.custID = document.getElementById('<%=hdnCustId.ClientID%>').value;
                            }
                        }
                    }


                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        //url: "CustomerAuto.asmx/getTaskContacts",
                        url: "CustomerAuto.asmx/GetLocationProspect",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            // alert("Due to unexpected errors we were unable to load contact name");
                            noty({ text: 'Due to unexpected errors we were unable to load contact name', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        }
                    });
                },
                select: function (event, ui) {
                    var str = ui.item.label;
                    var staxName;
                    if (str == "No Record Found!") {
                        $("#<%=txtCont.ClientID%>").val("");
                        <%--$("#<%=lnkAddnew.ClientID%>").css("display", "none");--%>
                    }
                    else {
                        // case of location
                        if (ui.item.ProspectID == 0) {
                            $("#<%=hdStatus.ClientID%>").val("0");
                            $("#<%=hdnROLId.ClientID%>").val(ui.item.rolid);
                            $("#<%=hdnLocID.ClientID%>").val(ui.item.value);
                            $("#<%=hdnCustId.ClientID%>").val(ui.item.value);
                            // Location
                            $("#<%=txtCont.ClientID%>").val(ui.item.label);
                            if (ui.item.STax != null)
                                $("#<%=drpSaleTax.ClientID%>").val(ui.item.STax);
                            else
                                $("#<%=drpSaleTax.ClientID%>").val(0);

                            // Customer
                            $("#<%=txtCompanyName.ClientID%>").val(ui.item.CompanyName);

                            document.getElementById('<%=btnSelectLoc.ClientID%>').click();
                            // Hide convert button
                            $("#<%=lnkConvert.ClientID%>").css("display", "none");
                        }// case of Prospect (Lead)
                        else {
                            $("#<%=txtCompanyName.ClientID%>").val(ui.item.CompanyName);
                            $("#<%=hdnLocID.ClientID%>").val("0");
                            $("#<%=hdnCustId.ClientID%>").val("0");
                            $("#<%=hdnROLId.ClientID%>").val(ui.item.rolid);
                            $("#<%=hdProspect.ClientID%>").val(ui.item.ProspectID);
                            $("#<%=hdStatus.ClientID%>").val(ui.item.ProspectID);

                            $("#<%=txtCont.ClientID%>").val(ui.item.label);
                            if (ui.item.STax != null)
                                $("#<%=drpSaleTax.ClientID%>").val(ui.item.STax);
                            else
                                $("#<%=drpSaleTax.ClientID%>").val(0);

                            document.getElementById('<%=btnSelectLoc.ClientID%>').click();
                            // Show convert button
                            $("#<%=lnkConvert.ClientID%>").css('visibility', 'visible');
                        }
                    }

                    Materialize.updateTextFields();
                    //alert("sdsdsd");
                    return false;
                },
                change: function (event, ui) {
                    if (!ui.item) {
                        $(this).val("");
                        return false;
                    }
                },
                focus: function (event, ui) {
                    //alert(ui);
                    $("#<%=txtCont.ClientID%>").val(ui.item.label);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    //._renderItem = function (ul, item) {

                    var result_item = item.label;
                    var result_desc = item.desc;
                    var result_type = item.type;
                    var result_Prospect = item.ProspectID;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.toString().replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>';
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.toString().replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>';
                        });
                    }
                    var color = 'gray';
                    if (result_Prospect == 0) {
                        color = 'Black';
                    }
                    else {
                        color = 'brown';
                    }


                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        //.append("<a style='color:" + color + ";'>" + result_item + " <span style='color:Gray;'> " + result_desc + "</span></a>")
                        .append("<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>")
                        .appendTo(ul);
                };


            //Company Name autopopulate
            $("#<%=txtCompanyName.ClientID%>").autocomplete({

                source: function (request, response) {

                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        //url: "CustomerAuto.asmx/getTaskContacts",
                        url: "CustomerAuto.asmx/GetCustomerProspect",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            //alert("Due to unexpected errors we were unable to load contact name");
                            noty({ text: 'Due to unexpected errors we were unable to load contact name', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        }
                    });
                },
                select: function (event, ui) {
                    //var str = ui.item.label;
                    $("#<%=txtCompanyName.ClientID%>").val(ui.item.label);
                    if (ui.item.prospect == 1) {
                        $("#<%=txtCont.ClientID%>").val(ui.item.lName);
                        $("#<%=hdnROLId.ClientID%>").val(ui.item.rolid);
                        $("#<%=hdProspect.ClientID%>").val(ui.item.value);
                        $("#<%=hdStatus.ClientID%>").val(ui.item.value);
                        //BindAddressAutoComplete(ui.item.rolid);
                        $("#<%=hdnCustId.ClientID%>").val("0");
                        $("#<%=hdnCustomerID.ClientID%>").val("0");

                        $("#<%=hdnLocID.ClientID%>").val("0");
                        var staxRate = ui.item.STaxRate;
                        if (staxRate != null)
                            $("#<%=drpSaleTax.ClientID%>").val(staxRate);
                        else
                            $("#<%=drpSaleTax.ClientID%>").val(0);
                        <%--$("#<%=lnkAddnew.ClientID%>").css("display", "block");--%>
                        //BindCustomerAndEmployeeName(ui.item.value);
                        $("#<%=lnkConvert.ClientID%>").css("display", "block");
                        document.getElementById('<%=btnSelectLocCus.ClientID%>').click();
                    }
                    else {
                        $("#<%=hdnCustId.ClientID%>").val(ui.item.value);
                        $("#<%=hdnCustomerID.ClientID%>").val(ui.item.value);
                        $("#<%=hdStatus.ClientID%>").val("0");
                        $("#<%=hdProspect.ClientID%>").val("0");
                        <%--$("#<%=lnkAddnew.ClientID%>").css("display", "none");--%>
                        $("#<%=hdnROLId.ClientID%>").val(ui.item.rolid);
                        //BindAddressAutoComplete(ui.item.rolid);
                        //document.getElementById('<%=btnSelectLocCus.ClientID%>').click();
                        //BindLocationName(ui.item.value);
                        <%--if ($("#<%=hdnLocID.ClientID%>").val() !== undefined) {
                            if ($("#<%=hdnLocID.ClientID%>").val().length > 0) {
                                BindLocationById($("#<%=hdnLocID.ClientID%>").val());
                            }
                        }--%>

                        $("#<%=lnkConvert.ClientID%>").css("display", "none");
                        document.getElementById('<%=btnSelectLocCus.ClientID%>').click();
                    }

                    return false;
                },
                change: function (event, ui) {
                    if (!ui.item) {
                        $(this).val("");
                        return false;
                    }
                },
                focus: function (event, ui) {
                    //alert(ui);
                    $("#<%=txtCompanyName.ClientID%>").val(ui.item.label);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {

                    var result_item = item.label;
                    var result_desc = item.desc;
                    var result_Prospect = item.prospect;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...     


                    if (query != "") {

                        result_item = result_item.toString().replace(x, function (FullMatch, n) {
                            //return '<span>' + FullMatch + '</span>'
                            return '<span class="highlight">' + FullMatch + '</span>';
                        });

                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                //return '<span>' + FullMatch + '</span>'
                                return '<span class="highlight">' + FullMatch + '</span>';
                            });
                        }
                    }
                    var color = '#222';
                    if (result_Prospect != 0) {
                        display = "inline-block";
                    }
                    else {
                        display = "none";
                    }
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<span class='auto_item'><i style='display:" + display + ";margin-right:8px;width:auto;color:#1565C0 !important;' class='fas fa-thumbs-up' title='Prospect'></i>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>")
                        .appendTo(ul);

                };

            $("[id*=ddlBType]").change(function () {
                var ddlBType = $(this);
                var strBType = $(this).val();
                if (strBType == '0') {
                    window.radopen(null, "RadWindowBOMType");
                    $("#<%= txtBomType.ClientID %>").val('');
                    $(this).val('-1');
                }
            });

            $("[id*=txtLabRate].txtLabRate").dblclick(function () {
                OpenRadWindowLaborRate(this);
            });

            Materialize.updateTextFields();

            $(window.document).keydown(function (event) {
                var $focused = $(':focus');
                var flag = 0;
                if ($focused[0] != null) {
                    if ($focused[0].id.indexOf('<%=gvBOM.ClientID%>') !== -1) {
                        var prevSelectedRowIndex = $('#<%=hdnBOMPrevSelectedRowIndex.ClientID%>').val();
                        if (prevSelectedRowIndex > 0) {
                            flag = 1;
                        }
                    } else if ($focused[0].id.indexOf('<%=gvMilestones.ClientID%>') !== -1) {
                        var prevSelectedRowIndexMistone = $('#<%=hdnBillingPrevSelectedRowIndex.ClientID%>').val();
                        if (prevSelectedRowIndexMistone > 0) {
                            flag = 2;
                        }
                    }
                }

                if (event.which == 117) {
                    if (flag == 1) {
                        document.getElementById('<%=btnCopyPreviousBOM.ClientID%>').click();
                    } else if (flag == 2) {
                        document.getElementById('<%=btnCopyPreviousMilestones.ClientID%>').click();
                    }
                    return false;
                }
            });

            $("#<%=gvBOM.ClientID%> tbody tr input:text, #<%=gvBOM.ClientID%> tbody tr input:checkbox, #<%=gvBOM.ClientID%> tbody tr select").on("focus", function (e) {
                // For F6
                var ctr = $(e)[0].target;
                var currRow = $(ctr).closest('tbody>tr');
                var hdnIndexVal = $(currRow).find("[id*=hdnIndex]").val();
                $('#<%=hdnBOMPrevSelectedRowIndex.ClientID%>').val(hdnIndexVal - 1);
                console.log("currRowBOM: " + hdnIndexVal);
                console.log("prevRowBOM: " + $('#<%=hdnBOMPrevSelectedRowIndex.ClientID%>').val());
            });

            $("#<%=gvMilestones.ClientID%> tbody tr input:text, #<%=gvMilestones.ClientID%> tbody tr input:checkbox, #<%=gvMilestones.ClientID%> tbody tr select").on("focus", function (e) {
                // For F6
                var ctr = $(e)[0].target;
                var currRow = $(ctr).closest('tbody>tr');
                var hdnIndexVal = $(currRow).find("[id*=hdnIndex]").val();
                $('#<%=hdnBillingPrevSelectedRowIndex.ClientID%>').val(hdnIndexVal - 1);
                console.log("currRowBilling: " + hdnIndexVal);
                console.log("prevRowBilling: " + $('#<%=hdnBillingPrevSelectedRowIndex.ClientID%>').val());
            });

            $("#ctl00_ContentPlaceHolder1_txtCompanyName").keyup(function (event) {
                var hdnCustID = document.getElementById('ctl00_ContentPlaceHolder1_hdnCustId');
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtCompanyName').value == '') {
                    hdnCustID.value = '';
                    $('#<%= lnkCustomerID.ClientID %>').removeAttr("href");
                }
            });

            $("#ctl00_ContentPlaceHolder1_txtCont").keyup(function (event) {
                var hdnLocId = document.getElementById('ctl00_ContentPlaceHolder1_hdnLocID');
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtCont').value == '') {
                    hdnLocId.value = '';
                    $('#<%= lnkLocationID.ClientID %>').removeAttr("href");
                    <%--    $('#<%=ddlTerr.ClientID %>').val('');
                      $('#<%=ddlTerr2.ClientID %>').val('');--%>
                }
            });

            $("#<%=txtContact.ClientID%>").autocomplete({
                source: function (request, response) {

                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    dtaaa.RoleID = 0;
                    if (document.getElementById('<%=txtCont.ClientID%>').value != '') {
                        if (document.getElementById('<%=hdnROLId.ClientID%>').value != '') {
                            dtaaa.RoleID = document.getElementById('<%=hdnROLId.ClientID%>').value;
                        }
                    }

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetContactsSearchbyRolid",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load contact name");
                        }
                    });
                },
                select: function (event, ui) {
                    var str = ui.item.label;
                    if (str != "No Record Found!") {
                        $("#<%=txtContact.ClientID%>").val(str);
                        $("#<%=txtPhone.ClientID%>").val(ui.item.Phone);
                        $("#<%=txtEmail.ClientID%>").val(ui.item.Email);
                        $("#<%=txtFax.ClientID%>").val(ui.item.Fax);
                        $("#<%=txtCellNew.ClientID%>").val(ui.item.Cell);
                    }
                    Materialize.updateTextFields();
                    return false;
                },
                //change: function (event, ui) {
                //    if (!ui.item) {
                //        $(this).val("");
                //        return false;
                //    }
                //},
                focus: function (event, ui) {
                    var str = ui.item.label;
                    if (str != "No Record Found!") {
                        $("#<%=txtContact.ClientID%>").val(ui.item.label);
                    }

                    return false;
                },
                minLength: 0,
                delay: 250
            }).bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {

                    var result_item = item.label;
                    var result_desc = item.desc;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.toString().replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    var color = 'black';

                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a style='color:" + color + ";'>" + result_item + " <span style='color:Gray;'> " + result_desc + "</span></a>")
                        .appendTo(ul);
                };

            CalculateTotalBillAmt();
        }

        function AddNewRowsBOM(sender, eventArgs) {
            $('#<%=hdnSelectedGrid.ClientID%>').val("gvBOM");
            var $focused = $(':focus');
            //var flag = 0;
            //if ($focused[0] != null && $focused[0].id.indexOf('<%=gvBOM.ClientID%>') !== -1) {
            var flag = 0;
            var currRow = $('#<%=gvBOM.ClientID%>_GridData tbody tr:last');
            var ddlBTypeVal = $(currRow).find("[id*=ddlBType]").val();
            var txtScopeVal = $(currRow).find("[id*=txtScope]").val();
            console.log(ddlBTypeVal);
            console.log(txtScopeVal);
            if (ddlBTypeVal != '' && ddlBTypeVal != '0' && ddlBTypeVal != '-1' && txtScopeVal != '') {
                console.log("flag1 = " + flag);
                //var $focused = $(':focus');
                var temp = $focused[0];
                if ($focused[0] == null) {
                    flag = 1;
                    console.log("flag2 = " + flag);
                } else {
                    var classes = $focused[0].classList;
                    var isPreventDownRow = false;
                    for (var i = 0; i < classes.length; i++) {
                        if (classes[i] == 'preventdownrow') {
                            isPreventDownRow = true;
                        }
                    }
                    if (!isPreventDownRow) {
                        flag = 1;
                    }
                }
            }
            //}  

            if (eventArgs.get_keyCode() == 40) {
                if (flag == 1) {
                    document.getElementById('<%=btnAddNewLinesBOM.ClientID%>').click();
                }
            }
        }

        function AddNewRowsMilestones(sender, eventArgs) {
            $('#<%=hdnSelectedGrid.ClientID%>').val("gvMilestones");
            var $focused = $(':focus');
            var flag = 0;
            var currRow = $('#<%=gvMilestones.ClientID%>_GridData tbody tr:last');
            //var ddlBTypeVal = $(currRow).find("[id*=ddlBType]").val();
            var txtScopeVal = $(currRow).find("[id*=txtScope]").val();
            console.log(txtScopeVal);
            if (txtScopeVal != '') {
                if ($focused[0] == null) {
                    flag = 1;
                } else {
                    var classes = $focused[0].classList;
                    var isPreventDownRow = false;
                    for (var i = 0; i < classes.length; i++) {
                        if (classes[i] == 'preventdownrow') {
                            isPreventDownRow = true;
                        }
                    }
                    if (!isPreventDownRow) {
                        flag = 1;
                    }
                }
            }
            //}  

            if (eventArgs.get_keyCode() == 40) {
                if (flag == 1) {
                    document.getElementById('<%=btnAddNewLinesMilestones.ClientID%>').click();
                }
            }
        }

        function imgBtnAddBOM_ClientClick() {
            var currRow = $('#<%=gvBOM.ClientID%>_GridData tbody tr:last');
            var ddlBTypeVal = $(currRow).find("[id*=ddlBType]").val();
            var txtScopeVal = $(currRow).find("[id*=txtScope]").val();
            $('#<%=hdnSelectedGrid.ClientID%>').val("gvBOM");
            console.log(ddlBTypeVal);
            console.log(txtScopeVal);
            if (ddlBTypeVal != '' && ddlBTypeVal != '0' && ddlBTypeVal != '-1' && txtScopeVal != '') {
                itemJSON();
                document.getElementById('<%=btnAddNewLinesBOM.ClientID%>').click();
            }

            return false;
        }

        function imgBtnAddMistone_ClientClick() {
            $('#<%=hdnSelectedGrid.ClientID%>').val("gvMilestones");
            var currRow = $('#<%=gvMilestones.ClientID%>_GridData tbody tr:last');
            var txtScopeVal = $(currRow).find("[id*=txtScope]").val();
            if (txtScopeVal != '') {
                itemJSON();
                document.getElementById('<%=btnAddNewLinesMilestones.ClientID%>').click();
            }

            return false;
        }

        function CalBillingAmount(ret, type) {
            try {
                var estType = $("#<%=ddlEstimateType.ClientID%>").val();
                if (estType == 'bid') {
                    var id = $(ret).attr('id')

                    // updating percent
                    if (type == "1") {
                        var txtQuantity = id.replace('txtPerAmount', 'txtQuantity');
                        var txtPrice = id.replace('txtPerAmount', 'txtPrice');
                        $("#" + txtQuantity).val('');
                        $("#" + txtPrice).val('');
                        var txtAmount = id.replace('txtPerAmount', 'txtAmount');
                        var bidPrice = 0;
                        var finalBidPrice = GetNumberFromStringFormated($("[id*=txtOverride]:input.override-amt").val());
                        if (finalBidPrice == "0.00" || finalBidPrice == "") {
                            var bidpr = GetNumberFromStringFormated($("#<%=hdnBidPrice.ClientID %>").val());
                            bidPrice = bidpr;
                        }
                        else {
                            bidPrice = finalBidPrice;
                        }
                        var salesTax = GetNumberFromStringFormated($('#hd_salestax').val());
                        if (salesTax == null || salesTax == '') {
                            salesTax = 0;
                        }
                        var per = GetNumberFromStringFormated($("#" + id).val());
                        if (bidPrice != "") {
                            var value = (per * (bidPrice - salesTax)) / 100;
                            var finalValue = value.toFixed(2);
                            var formatValue = parseFloat(finalValue).toLocaleString("en-US", { minimumFractionDigits: 2 })
                            $("#" + txtAmount).val(formatValue);
                        }
                    }
                    // Updating amount
                    else {
                        var txtQuantity = id.replace('txtAmount', 'txtQuantity');
                        var txtPrice = id.replace('txtAmount', 'txtPrice');
                        $("#" + txtQuantity).val('');
                        $("#" + txtPrice).val('');
                        var txtPerAmount = id.replace('txtAmount', 'txtPerAmount');
                        var bidPrice = 0;
                        var finalBidPrice = GetNumberFromStringFormated($("[id*=txtOverride]:input.override-amt").val());
                        if (finalBidPrice == "0.00" || finalBidPrice == "") {
                            var bidpr = GetNumberFromStringFormated($("#<%=hdnBidPrice.ClientID %>").val());
                            bidPrice = bidpr;
                        }
                        else {
                            bidPrice = finalBidPrice;
                        }

                        var salesTax = GetNumberFromStringFormated($('#hd_salestax').val());
                        if (salesTax == null || salesTax == '') {
                            salesTax = 0;
                        }

                        var amount = GetNumberFromStringFormated($("#" + id).val());
                        if (bidPrice != "" && bidPrice != "0.00" && amount != "") {
                            var value = (amount * 100) / (bidPrice - salesTax);
                            var finalValue = value.toFixed(2);
                            $("#" + txtPerAmount).val(finalValue);
                        }
                        else {
                            $("#" + txtPerAmount).val("");
                        }
                    }
                    //debugger
                    CalculateEstimateMilestone();
                } else {
                    // updating quantity
                    if (type == "3") {
                        var id = $(ret).attr('id')
                        var txtAmountId = id.replace('txtQuantity', 'txtAmount');
                        var txtPriceId = id.replace('txtQuantity', 'txtPrice');
                        var txtPerAmountId = id.replace('txtQuantity', 'txtPerAmount');
                        // Reset PerAmount
                        $("#" + txtPerAmountId).val('');
                        var quan = parseFloat(GetNumberFromStringFormated($("#" + id).val()));
                        var price = parseFloat(GetNumberFromStringFormated($("#" + txtPriceId).val()));
                        var amount = parseFloat(GetNumberFromStringFormated($("#" + txtAmountId).val()));
                        if (isNaN(quan) && !isNaN(price) && price != 0 && !isNaN(amount)) {
                            quan = amount / price;
                        } else if (!isNaN(quan) && !isNaN(price)) {
                            amount = quan * price;
                        } else if (!isNaN(quan) && quan != 0 && isNaN(price) && !isNaN(amount)) {
                            price = amount / quan;
                        }

                        if (!isNaN(quan)) {
                            $("#" + id).val(quan.toFixed(2));
                        }

                        if (!isNaN(amount)) {
                            $("#" + txtAmountId).val(amount.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        }

                        if (!isNaN(price)) {
                            $("#" + txtPriceId).val(price.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        }

                    }// Updating price
                    else if (type == "4") {
                        var id = $(ret).attr('id')
                        var txtAmountId = id.replace('txtPrice', 'txtAmount');
                        var txtQuantityId = id.replace('txtPrice', 'txtQuantity');
                        var txtPerAmountId = id.replace('txtPrice', 'txtPerAmount');
                        //Reset PerAmount
                        $("#" + txtPerAmountId).val('');

                        var quan = parseFloat(GetNumberFromStringFormated($("#" + txtQuantityId).val()));
                        var price = parseFloat(GetNumberFromStringFormated($("#" + id).val()));
                        var amount = parseFloat(GetNumberFromStringFormated($("#" + txtAmountId).val()));
                        if (isNaN(price) && !isNaN(quan) && quan != 0 && !isNaN(amount)) {
                            price = amount / quan;
                        } else if (!isNaN(price) && !isNaN(quan)) {
                            amount = quan * price;
                        } else if (!isNaN(price) && price != 0 && isNaN(quan) && !isNaN(amount)) {
                            quan = amount / price;
                        }

                        if (!isNaN(quan)) {
                            $("#" + txtQuantityId).val(quan.toFixed(2));
                        }

                        if (!isNaN(amount)) {
                            $("#" + txtAmountId).val(amount.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        }

                        if (!isNaN(price)) {
                            $("#" + id).val(price.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        }

                    } else {// Updating amount
                        var id = $(ret).attr('id')
                        var txtPriceId = id.replace('txtAmount', 'txtPrice');
                        var txtQuantityId = id.replace('txtAmount', 'txtQuantity');
                        var txtPerAmountId = id.replace('txtAmount', 'txtPerAmount');
                        //Reset PerAmount
                        $("#" + txtPerAmountId).val('');

                        var quan = parseFloat(GetNumberFromStringFormated($("#" + txtQuantityId).val()));
                        var price = parseFloat(GetNumberFromStringFormated($("#" + txtPriceId).val()));
                        var amount = parseFloat(GetNumberFromStringFormated($("#" + id).val()));
                        if (isNaN(amount) && !isNaN(quan) && !isNaN(price)) {
                            amount = quan * price;
                        } else if (!isNaN(amount) && isNaN(quan) && !isNaN(price) && price != 0) {
                            quan = amount / price;
                        } else if (!isNaN(amount) && !isNaN(quan) && quan != 0) {
                            price = amount / quan;
                        } else if (!isNaN(amount) && amount != 0 && !isNaN(quan) && quan == 0) {
                            quan = 1;
                            price = amount;
                        }

                        if (!isNaN(quan)) {
                            $("#" + txtQuantityId).val(quan.toFixed(2));
                        }

                        if (!isNaN(amount)) {
                            $("#" + id).val(amount.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        }

                        if (!isNaN(price)) {
                            $("#" + txtPriceId).val(price.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        }
                    }
                    //debugger
                    CalculateEstimateMilestone();
                }
                CalculateTotalBillAmt();
            } catch (e) {

            }

        }

        function CalculateTotalBillAmt() {
            var tAmount = 0;
            $("[id$='gvMilestones_GridData']").find('tbody tr').each(function () {
                var $tr = $(this);

                if ($tr.find('input[id*=txtAmount]').attr('id') != "" && typeof $tr.find('input[id*=txtAmount]').attr('id') != 'undefined') {
                    var bidAmt = $tr.find('input[id*=txtAmount]').val().replace(/[\$\(\),]/g, '');

                    if (!isNaN(parseFloat(bidAmt))) {
                        tAmount += parseFloat(bidAmt);
                    }
                }
            });
            $('[id*=lblTotalBillAmt]').text(tAmount.toFixed(2));

            CalculateFinalBidPrice();
        }

        function EsTemplateChangeConfirmation() {
            var estimateMode = $("#<%=hdnEstimateMode.ClientID%>").val();
            if (estimateMode == "Copy" || estimateMode == "Edit") { // Copy mode
                return noty({
                    dismissQueue: true,
                    layout: 'topCenter',
                    theme: 'noty_theme_default',
                    animateOpen: { height: 'toggle' },
                    animateClose: { height: 'toggle' },
                    easing: 'swing',
                    text: 'Do you want to replace the current items in BOM and BILLING by the default items from Template?',
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
                                $("#<%=hdnChangeTemplateConfirmStatus.ClientID%>").val("yes");
                                __doPostBack("ctl00$ContentPlaceHolder1$ddlTemplate", "ddlddlTemplatechange");
                                return true;
                            }
                        },
                        {
                            type: 'btn-danger', text: 'No', click: function ($noty) {

                                $noty.close();
                                $("#<%=hdnChangeTemplateConfirmStatus.ClientID%>").val("no");
                                __doPostBack("ctl00$ContentPlaceHolder1$ddlTemplate", "ddlddlTemplatechange");
                                return true;
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
            else {
                __doPostBack("ctl00$ContentPlaceHolder1$ddlTemplate", "ddlddlTemplatechange");
                return true;
            }
        }

        function ViewDocumentClick(hyperlink) {
            var IsView = "Y"<%--document.getElementById('<%= hdnViewDocument.ClientID%>').value;--%>
            if (IsView == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function ShowContactWindow() {
            var wnd = $find('<%=RadWindowContact.ClientID %>');
            wnd.Show();
            //Sys.Application.remove_load(f);
            //Sys.Application.add_load(f);


            $("[id*=txtContPhone]").mask("(999) 999-9999? Ext 99999");
            $("[id*=txtContPhone]").bind('paste', function () { $(this).val(''); });
            $("[id*=txtContCell]").mask("(999) 999-9999");
            $("[id*=txtContFax]").mask("(999) 999-9999");

            Materialize.updateTextFields();
        }

        function ApprovalStatusOnChange() {
            var status = $('[id*=ddlApprovalStatus] option:selected').val();
            var temp = "<%=Session["Username"].ToString()%>" + " " + "<%=DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")%>" + "\n";
            var comment = "";
            if (status == "1") {
                comment = "Approved by " + temp;
            }
            else if (status == "2") {
                comment = "Changes required by " + temp;
            }
            else {
                comment = "Pending by " + temp;
            }

            $("#<%= txtApproveStatusComment.ClientID%>").val(comment);
        }

    </script>


    <script type="text/javascript">

        $("[id*=txtPhone]").mask("(999) 999-9999? Ext 99999");
        $("[id*=txtPhone]").bind('paste', function () { $(this).val(''); });
        $("[id*=txtCellNew]").mask("(999) 999-9999");
        $("[id*=txtFax]").mask("(999) 999-9999");


    </script>

    <script>
        function HoverMenutext(tooltip, event) {
            var left = '55px';
            $('#' + tooltip).css({ left: left }).show();
        }

        function DownloadLatestProposal() {
            var btn = document.getElementById('<%=lnkLatestProposal.ClientID%>');
            btn.click();
        }
    </script>
</asp:Content>
