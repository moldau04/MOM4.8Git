<%@ Page Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddProjectTemp" CodeBehind="AddProjectTemp.aspx.cs" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="uc_AccountSearch.ascx" TagName="uc_AccountSearch" TagPrefix="uc1" %>
<%@ Register Src="uc_gvChecklist.ascx" TagName="uc_gvChecklist" TagPrefix="uc2" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <style>
      .cardradius{min-height: 307px !important;}
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

        .new_popupCSS {
            top: 50% !important;
            background: #fff;
            max-width: 825px;
            width: 100%;
            left: 50% !important;
            transform: translate(-50%, -50%) !important;
            height: auto;
            max-height: 620px !important;
            border-radius: 6px 6px 0px 0px;
            box-shadow: 0 19px 38px rgba(0,0,0,0.2), 0 15px 12px rgba(0,0,0,0.2);
        }

            .new_popupCSS .title_bar_popup {
                background: #1c5fb1;
                color: #fff;
                padding: 13px 15px;
                border-radius: 6px 6px 0px 0px;
                display: flex;
                align-items: center;
                justify-content: space-between;
            }
            .RadGrid_Material .rgHeader {
                padding: 5px 8px !important;
            }
        .title_bar_popup .rwIcon:before {
            content: "\e137";
            margin: 0;
            width: 1em;
            height: 1em;
            vertical-align: middle;
            font: 2em/1 "WebComponentsIcons";
            display: inline-block;
            padding-right: 40px;
            vertical-align: middle;
        }

        .title_bar_popup .rwCommandButton:before {
            content: "\e11b";
            margin: auto;
            padding: 0;
            display: inline-block;
            font: 16px/1 "WebComponentsIcons";
            text-align: center;
            vertical-align: top;
        }

        .title_bar_popup .rwCommandButton {
            padding: 2px;
            border: 1px solid transparent;
            border-radius: 2px;
            line-height: 1;
            display: inline-block;
            text-decoration: none;
            vertical-align: top;
        }

        .new_popupCSS #content {
            min-height: auto !important;
        }

        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
        }

        .selected {
            width: 100% !important;
            background-color: gray !important;
            -webkit-transition: all 0.1s ease;
            -moz-transition: all 0.1s ease;
            -o-transition: all 0.1s ease;
            transition: all 0.1s ease;
        }

        [id$='gvBOM_GridData'], [id$='gvMilestones_GridData'] {
            height: auto !important;
            max-height: 408px;
        }

        .RadGrid div.rgHeaderWrapper {
            border-radius: 5px 5px 0 0;
        }

        [id$='gvBOM_GridData'] .rgAltRow > td, [id$='gvBOM_GridData'] .rgRow > td, [id$='gvMilestones_GridData'] .rgAltRow > td, [id$='gvMilestones_GridData'] .rgRow > td {
            background-color: transparent !important;
        }

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
        .cardradius{
            min-height: 298px!important;
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
        .chip {
           border-radius: 11px;}
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
            border-radius: 9px;
            background-color: black;
        }

        /* On mouse-over, add a grey background color */
        .cusCheckContainer:hover input ~ .checkmark {
            background-color: black;
        }
        .tag-div{
            margin-bottom: 3px;
            margin-top: 3px;
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

        div.row.checkbox {
            border-bottom: 1px solid #9e9e9e;
            padding-bottom: 17px;
            margin-bottom: 10px !important;
        }
    </style>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
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
        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.dtxtScrapFactor = 0;
            }
        }
        function calBudgetExt(obj) {
            //debugger;
            var txt = $(obj).attr('id');
            var valQtyReq = 0;
            var valBudgetExt = 0;
            var valScrapFact = 0;
            var txtScrapFactor = 0;
            var objId = document.getElementById(txt);
            var valLabExt = 0;
            var isQtyReq = txt.search("txtQtyReq");
            if (isQtyReq == -1) {
                //on txtBudgetUnit textbox change   
                var txtBudgetUnit = (document.getElementById(txt).value).toString().replace(/[,]/g, '');
                if (isInt(txtBudgetUnit) == true) {
                    document.getElementById(txt).value = parseFloat(txtBudgetUnit).toFixed(2);
                    txtBudgetUnit = document.getElementById(txt).value;
                    document.getElementById(txt).value = parseFloat(txtBudgetUnit).toLocaleString("en-US", { minimumFractionDigits: 2 })
                }
                var txtQtyReq = (document.getElementById(txt.replace('txtBudgetUnit', 'txtQtyReq')).value).toString().replace(/[\$\(\),]/g, '')
                var ddlBType = document.getElementById(txt.replace('txtBudgetUnit', 'ddlBType')).value;
                var lblMatExt = document.getElementById(txt.replace('txtBudgetUnit', 'lblMatExt'));
                var hdnMatExt = document.getElementById(txt.replace('txtBudgetUnit', 'hdnMatExt'));
                var lblLabExt = document.getElementById(txt.replace('txtBudgetUnit', 'lblLabExt'))
                valLabExt = $(lblLabExt).text().toString()
                var lblTotalExt = document.getElementById(txt.replace('txtBudgetUnit', 'lblTotalExt'))
            }
            else {
                //on txtQtyReq textbox change
                var txtQtyReq = (document.getElementById(txt).value).toString().replace(/[,]/g, '');
                if (isInt(txtQtyReq) == true) {
                    document.getElementById(txt).value = parseFloat(txtQtyReq).toFixed(2);
                    txtQtyReq = parseFloat(document.getElementById(txt).value)
                    document.getElementById(txt).value = txtQtyReq.toLocaleString("en-US", { minimumFractionDigits: 2 })
                }
                var txtBudgetUnit = (document.getElementById(txt.replace('txtQtyReq', 'txtBudgetUnit')).value).toString().replace(/[\$\(\),]/g, '')
                var ddlBType = document.getElementById(txt.replace('txtQtyReq', 'ddlBType')).value;
                var lblMatExt = document.getElementById(txt.replace('txtQtyReq', 'lblMatExt'));
                var hdnMatExt = document.getElementById(txt.replace('txtQtyReq', 'hdnMatExt'));
                var lblLabExt = document.getElementById(txt.replace('txtQtyReq', 'lblLabExt'))
                valLabExt = $(lblLabExt).text().toString()
                var lblTotalExt = document.getElementById(txt.replace('txtQtyReq', 'lblTotalExt'))
            }
            valBudgetExt = parseFloat(txtQtyReq) * parseFloat(txtBudgetUnit);
            if (isInt(valBudgetExt) == true) {
                valBudgetExt = valBudgetExt.toFixed(2);
                $(lblMatExt).text(parseFloat(valBudgetExt).toLocaleString("en-US", { minimumFractionDigits: 2 }))
                $(hdnMatExt).val(valBudgetExt.toString())
                if (isInt(valLabExt) == false) {
                    valLabExt = 0;
                }
                var total = parseFloat(valBudgetExt) + parseFloat(valLabExt);
                $(lblTotalExt).text(total.toLocaleString("en-US", { minimumFractionDigits: 2 }))
            }
        }
        function calLabExt(obj) {
            var txt = $(obj).attr('id');
            var valHours = 0;
            var valRate = 0;
            var valLabExt = 0;
            var valMatExt = 0;
            var objId = document.getElementById(obj.id);
            var isRate = txt.search("txtLabRate");
            var isHours = txt.search("txtHours");
            if (isRate != -1) {
                //on txtLabRate textbox change   
                var txtLabRate = document.getElementById(txt).value.toString().replace(/[\$\(\),]/g, '');
                if (isInt(txtLabRate) == true) {
                    document.getElementById(txt).value = parseFloat(txtLabRate).toLocaleString("en-US", { minimumFractionDigits: 2 })
                    txtLabRate = document.getElementById(txt).value.toString().replace(/[\$\(\),]/g, '');
                }
                var txtHours = document.getElementById(txt.replace('txtLabRate', 'txtHours')).value.toString().replace(/[\$\(\),]/g, '');
                var lblLabExt = document.getElementById(txt.replace('txtLabRate', 'lblLabExt'));
                var lblTotalExt = document.getElementById(txt.replace('txtLabRate', 'lblTotalExt'))
                var lblMatExt = document.getElementById(txt.replace('txtLabRate', 'lblMatExt'))
                valMatExt = $(lblMatExt).text().toString().replace(/[\$\(\),]/g, '');
            }
            else if (isHours != -1) {
                //on txtHours textbox change
                var txtHours = document.getElementById(txt).value;
                if (isInt(txtHours) == true) {
                    document.getElementById(txt).value = parseFloat(txtHours).toLocaleString("en-US", { minimumFractionDigits: 2 })
                    txtHours = document.getElementById(txt).value;
                }
                var txtLabRate = document.getElementById(txt.replace('txtHours', 'txtLabRate')).value.toString().replace(/[\$\(\),]/g, '');
                var lblLabExt = document.getElementById(txt.replace('txtHours', 'lblLabExt'));
                var lblTotalExt = document.getElementById(txt.replace('txtHours', 'lblTotalExt'))
                var lblMatExt = document.getElementById(txt.replace('txtHours', 'lblMatExt'))
                valMatExt = $(lblMatExt).text().toString().replace(/[\$\(\),]/g, '');
            }
            valLabExt = (parseFloat(txtHours) * parseFloat(txtLabRate));
            if (isInt(valLabExt) == true) {
                valLabExt = parseFloat(valLabExt)
                $(lblLabExt).text(valLabExt.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                if (isInt(valMatExt) == false) {
                    valMatExt = 0;
                }
                var total = parseFloat(valMatExt) + parseFloat(valLabExt);
                $(lblTotalExt).text(total.toLocaleString("en-US", { minimumFractionDigits: 2 }))
            }
        }

        function isInt(value) {
            var x = parseFloat(value);
            return !isNaN(value) && (x | 0) === x;
        }
        //Function to Show ModalPopUp
        function Showpopup() {
            $find('mpeAddCode').show();
        }
        $(document).ready(function () {
        });
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

        function GetNumberFromStringFormated(strNum) {
            strNum = strNum.replace(/[\$,]/g, '');
            if (strNum.indexOf('(') == 0) {
                strNum = strNum.replace(/[\(\),]/g, '');
                strNum = '-' + strNum;
            }
            return strNum;
        }
        function CalBillingAmount(obj, type) {
            var id = $(obj).attr('id');
            if (type == "1") {// Quantity
                var txtAmountId = id.replace('txtQuantity', 'txtAmount');
                var txtPriceId = id.replace('txtQuantity', 'txtPrice');
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
            }
            // Price
            else if (type == "2") {
                var txtAmountId = id.replace('txtPrice', 'txtAmount');
                var txtQuantityId = id.replace('txtPrice', 'txtQuantity');
                var quan = parseFloat(GetNumberFromStringFormated($("#" + txtQuantityId).val()));
                var price = parseFloat(GetNumberFromStringFormated($("#" + id).val()));
                var amount = parseFloat(GetNumberFromStringFormated($("#" + txtAmountId).val()));
                //if (price == "" && quan != "" && quan != 0 && amount != "") {
                if (isNaN(price) && !isNaN(quan) && quan != 0 && !isNaN(amount)) {
                    price = amount / quan;
                    //} else if (price != "" && quan != "") {
                } else if (!isNaN(price) && !isNaN(quan)) {
                    amount = quan * price;
                    //} else if (price != "" && price != 0 && quan == "" && amount != "") {
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
            } else if (type == "3") {
                var txtPriceId = id.replace('txtAmount', 'txtPrice');
                var txtQuantityId = id.replace('txtAmount', 'txtQuantity');
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
        }

        function showDecimalVal(obj) {
            if (!isNaN(parseFloat(document.getElementById(obj.id).value.toString().replace(/[\$\(\),]/g, '')))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value.toString().replace(/[\$\(\),]/g, '')).toLocaleString("en-US", { minimumFractionDigits: 2 })
            }
        }

        function DelRow(Gridview) {
            var selectedlen = $("#" + Gridview).find('tr').find('input[id*= chkSelect]:checked').length;

            if (selectedlen == 0) {
                alert('Please select items to delete.');
                return;
            }
            var con = confirm('Are you sure you want to delete the items?');
            if (con == true) {
                $("#" + Gridview).find('tr').each(function () {
                    var $tr = $(this);
                    $tr.find('input[id*= chkSelect]:checked').each(function () {
                        //if ($("#" + Gridview).find('tr').length > 3) {
                        $(this).closest('tr').remove();
                        //}
                        //else {
                        //    $(this).closest('tr').find('input:text').val('');
                        //}
                    });
                });
            }
        }
        function itemJSON() {
            var rawMileData = $('#<%=gvMilestones.ClientID%>').serializeFormJSON();
            var formMileData = JSON.stringify(rawMileData);
            $('#<%=hdnMilestone.ClientID%>').val(formMileData);

            var rawData = $('#<%=gvBOM.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);

            $('#<%=hdnItemJSON.ClientID%>').val(formData);
        }

        function chkBilFrmBOM_ChangeConfirm(obj) {
            var isChecked = $(obj).prop("checked");
            if (isChecked) {
                var a = confirm("This action will replace all your Billing Items by BOM Items? Are you sure you want to continue?");
                if (a) {
                    var rawData = $('#<%=gvBOM.ClientID%>').serializeFormJSON();
                    var formData = JSON.stringify(rawData);

                    $('#<%=hdnItemJSON.ClientID%>').val(formData);
                    setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder1$chkBilFrmBOM\',\'\')', 0)
                    return true;
                } else {
                    return false;
                }
            } else {
                return true;
            }
            <%--var isChecked = $(obj).prop("checked");
            var rawData = $('#<%=gvBOM.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            $('#<%=hdnItemJSON.ClientID%>').val(formData);

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

                                setTimeout('__doPostBack(\'ctl00$ContentPlaceHolder1$chkBilFrmBOM\',\'\')', 0)
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
            return true;--%>
        }


    </script>
    <script type="text/javascript">

        function RedirectPage() {
            var url = window.location.href;
            if (url != '')
                window.location = url;
        }
        //function pageLoad(sender, args) {
        var query = "";
        function dtaa() {
            this.prefixText = null;
            this.con = null;
            this.custID = null;
        }
        $(document).ready(function () {
            UpdatedivDisplayTeamMember();
            //UpdatedivDisplayUserRole();
            /////////////////////////////////// Wage ////////////////////////////////////
            $("#<%=txtPrevilWage.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    dtaaa.con = "";
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetWage",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load wage");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtPrevilWage.ClientID%>").val(ui.item.label);
                    $("#<%=hdnPrevilWageID.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtPrevilWage.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 50
            }).bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.value;
                    var result_item = item.label;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);
                };
            ///////////////////////////////////// Inventroy service ////////////////////////////////////
            $("#<%=txtInvService.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    dtaaa.con = "";
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetInvService",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load services");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtInvService.ClientID%>").val(ui.item.label);
                    $("#<%=hdnInvServiceID.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtInvService.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 50
            }).bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var result_value = item.value;
                    var result_item = item.label;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);
                };
            ///////////////////////////////////// Unrecognized Revenue ////////////////////////////////////
            $("#<%=txtUnrecognizedRevenue.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    dtaaa.con = "";
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetInvService",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load services");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtUnrecognizedRevenue.ClientID%>").val(ui.item.label);
                    $("#<%=hdnUnrecognizedRevenue.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtUnrecognizedRevenue.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 50
            }).bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var result_value = item.value;
                    var result_item = item.label;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);
                };
            ///////////////////////////////////// Unrecognized Revenue ////////////////////////////////////
            $("#<%=txtUnrecognizedExpense.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    dtaaa.con = "";
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
                            alert("Due to unexpected errors we were unable to load services");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtUnrecognizedExpense.ClientID%>").val(ui.item.label);
                    $("#<%=hdnUnrecognizedExpense.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtUnrecognizedExpense.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 50
            }).bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var result_value = item.value;
                    var result_item = item.acct;
                    var result_desc = item.label;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                        .appendTo(ul);
                };
            ///////////////////////////////////// Unrecognized Revenue ////////////////////////////////////
            $("#<%=txtRetainageReceivable.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    dtaaa.con = "";
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
                            alert("Due to unexpected errors we were unable to load services");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtRetainageReceivable.ClientID%>").val(ui.item.label);
                    $("#<%=hdnRetainageReceivable.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtRetainageReceivable.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 50
            }).bind('click', function () { $(this).autocomplete("search"); })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var result_value = item.value;
                    var result_item = item.acct;
                    var result_desc = item.label;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                        .appendTo(ul);
                };
            HideShowOnPostingTypeChange($('#<%= ddlPostingMethod.ClientID %>').val())
        });
        function dtaa() {
            this.prefixText = null;
            this.con = null;
            this.custID = null;
        }

        function HideShowOnPostingTypeChange(opt) {
            if (opt == 1 || opt == 2) {
                $('.hideShowOnPostingType').show();
            }
            else {
                $('.hideShowOnPostingType').hide();
                $('#<%= hdnUnrecognizedRevenue.ClientID %>').val('');
                $('#<%= hdnUnrecognizedExpense.ClientID %>').val('');
                $('#<%= hdnRetainageReceivable.ClientID %>').val('');
                $('#<%= txtUnrecognizedRevenue.ClientID %>').val('');
                $('#<%= txtUnrecognizedExpense.ClientID %>').val('');
                $('#<%= txtRetainageReceivable.ClientID %>').val('');
            }
        }
    </script>
    <script type="text/javascript">
        function AddBOMClick() {
            var BOMper = document.getElementById('<%= hdnBOMPermission.ClientID%>').value;
            if (BOMper.length >= 1) {
                var BOMvalues = BOMper.substr(0, 1);
                var BOMEditvalues = BOMper.substr(1, 1);
                if (BOMvalues == "N" || BOMEditvalues == "N") {
                    noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    return false;
                }
            }
            itemJSON();
            return true;
        }

        function CheckAddBomGrid(sender, args) {
            var BOMper = document.getElementById('<%= hdnBOMPermission.ClientID%>').value;
            if (BOMper.length >= 1) {
                var BOMvalues = BOMper.substr(0, 1);
                var BOMEditvalues = BOMper.substr(1, 1);
                if (BOMvalues == "N" || BOMEditvalues == "N") {
                    noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    return false;
                }
            }
            itemJSON();
        }

        function DeleteBOMClick() {
            var BOMper = document.getElementById('<%= hdnBOMPermission.ClientID%>').value;
            if (BOMper.length >= 1) {
                var BOMvalues = BOMper.substr(2, 1);
                if (BOMvalues == "N") {
                    noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    return false;
                }
            }
            DelRow('<%=gvBOM.ClientID%>');
            return true;
        }
        function AddMilestonesClick() {
            var Milesper = document.getElementById('<%= hdnMilestonesPermission.ClientID%>').value;
            if (Milesper.length >= 1) {
                var Milesvalues = Milesper.substr(0, 1);
                var MilesEditvalues = Milesper.substr(1, 1);
                if (Milesvalues == "N" || MilesEditvalues == "N") {
                    noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    return false;
                }
            }
            itemJSON();
            return true;
        }
        function CheckAddMilGrid(sender, args) {
            var Milesper = document.getElementById('<%= hdnMilestonesPermission.ClientID%>').value;
            if (Milesper.length >= 1) {
                var Milesvalues = Milesper.substr(0, 1);
                var MilesEditvalues = Milesper.substr(1, 1);
                if (Milesvalues == "N" || MilesEditvalues == "N") {
                    noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    return false;
                }
            }
            itemJSON();
        }
        function DeleteMilestonesClick() {
            var Milesper = document.getElementById('<%= hdnMilestonesPermission.ClientID%>').value;
            if (Milesper.length >= 1) {
                var Milesvalues = Milesper.substr(2, 1);
                if (Milesvalues == "N") {
                    noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    return false;
                }
            }
            DelRow('<%=gvMilestones.ClientID%>');
            return true;
        }
        ///-Document permission
        function AddDocumentClick(hyperlink) {
            var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
            if (IsAdd != "Y") {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteDocumentClick(hyperlink) {
            var IsDelete = document.getElementById('<%= hdnDeleteDocument.ClientID%>').value;
            if (IsDelete != "Y") {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function ViewDocumentClick(hyperlink) {
            var IsView = document.getElementById('<%= hdnEditeDocument.ClientID%>').value;
            if (IsView != "Y") {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

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

        //Validation for Description and Type
        function ConfirmGCAdd() {
            if (checkTempRowBomGrid() === false || checkTempRowMilGrid() === false) {
                return false;
            }
        }
        function checkTempRowBomGrid() {
            var check = true;
            var validation_message = "";
            $("[id$='gvBOM_GridData']").find('tbody tr').not('.rgNoRecords').each(function () {
                var $tr = $(this);
                var ddlBType = $tr.find("[id$='ddlBType']").val();
                var txtCode = $tr.find("[id$='txtCode']").val();
                var txtddlMatItem = $tr.find("[id$='txtddlMatItem']").val();
                var txtQtyReq = $tr.find('[id$=txtQtyReq]').val();
                var txtDes = $tr.find('[id$=txtScope]').val();

                var checkddlBType = (ddlBType === "0" || ddlBType === undefined || ddlBType.length === 0);
                var checktxtCode = (txtCode.length === 0 || txtCode === undefined);
                var checktxtddlMatItem = (txtddlMatItem.length === 0 || txtddlMatItem === undefined);
                var checkQtyReq = (txtQtyReq.length === 0 || txtQtyReq === undefined);
                var checkDes = (txtDes.length === 0 || txtDes === undefined);

                if (checkddlBType && checktxtCode && checktxtddlMatItem && checkQtyReq && checkDes) {
                    check = true;
                }
                else if (checkddlBType) {

                    validation_message = "Bom type cannot be empty";
                    check = false;
                }
                else if (checkDes) {

                    validation_message = "Bom description cannot be empty";
                    check = false;
                }
            });

            if (check) {
                return check;
            }
            else {
                noty({ text: validation_message, type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return check;
            }
        }

        function checkTempRowMilGrid() {
            var check = true;
            var validation_message = "";
            $("[id$='gvMilestones_GridData']").find('tbody tr').not('.rgNoRecords').each(function () {
                var $tr = $(this);
                var ddlType = $tr.find("[id$='ddlType']").val();
                var txtFunction = $tr.find("[id$='txtSType']").val();
                var txtName = $tr.find('[id$=txtName]').val();
                var txtDes = $tr.find('[id$=txtScope]').val();
                var txtAmount = $tr.find('[id$=txtAmount]').val();
                var txtRequireBy = $tr.find('[id$=txtRequiredBy]').val();

                var checkType = (parseInt(ddlType) < 0 || ddlType === undefined);
                var checkFunction = (txtFunction.length === 0 || txtFunction === undefined);
                var checkName = (txtName.length === 0 || txtName === undefined);
                var checkDes = (txtDes.length === 0 || txtDes === undefined);
                var checkAmount = (txtAmount.length === 0 || txtAmount === undefined);
                var checkRequireBy = (txtRequireBy.length === 0 || txtRequireBy === undefined);


                if (checkType && checkFunction && checkName && checkDes && checkAmount && checkRequireBy) {
                    check = true;
                }
                else if (checkDes) {

                    validation_message = "Billing description cannot be empty";
                    check = false;
                }
            });

            if (check) {
                return check;
            }
            else {
                noty({ text: validation_message, type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return check;
            }
        }
        function selectAllCheckbox(control, text) {
            var chkSelectAll = $(control);
            var grid = chkSelectAll.closest(".BomGrid");
            if (text === 'BomItem' || text === 'MilItem') {
                var chkbItem = grid.find(".chkSelect input");
                chkbItem.prop('checked', $("[id$='chkSelectAll']").is(':checked'));
            } else if (text === 'MatTx') {
                var chkbMat = grid.find(".chkMatSalestax input");
                var isMatTxCheckAll = $("[id$='chkSelectAllMatTx']").is(':checked');
                chkbMat.each(function () {
                    var isCheck = $(this).is(':checked');
                    var hdnMatChkID = this.id.replace("chkMatSalestax", "hdnMatChk");

                    if (isCheck != isMatTxCheckAll) {
                        $(this).prop('checked', isMatTxCheckAll);
                        $("#" + hdnMatChkID).val(isMatTxCheckAll);
                    }
                });
            }
            else if (text === 'LaborTx') {
                var chkbLabor = grid.find(".chkLabSalestax input");
                var isLaborTxCheckAll = $("[id$='chkSelectAllLaborTx']").is(':checked');
                chkbLabor.each(function () {
                    var isCheck = $(this).is(':checked');
                    var hdnLbChkID = this.id.replace("chkLabSalestax", "hdnLbChk");

                    if (isCheck != isLaborTxCheckAll) {
                        $(this).prop('checked', isLaborTxCheckAll);
                        $("#" + hdnLbChkID).val(isLaborTxCheckAll);
                    }
                });
            }
        }

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
            var txtMembers = $("#<%=gvCustom.ClientID %> input[id*='txtMembers']");
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
                                    //tag = "<div class='chip chipUserRoles'><input type='checkbox' checked='checked' disabled style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                }
                                else
                                    //tag = "<div class='chip chipUserRoles'><input type='checkbox' disabled  style='margin-right: 5px;' /><label>" + teamArr[i] + "</label></div>";
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                div.innerHTML += tag;
                            } else {
                                if (tempTeamKeyArr.length == 3 && (tempTeamKeyArr[0] == "0" || tempTeamKeyArr[0] == "1") && tempTeamKeyArr[2] == "1")
                                    //tag = "<div class='chip chipUsers' ><input type='checkbox' checked='checked' disabled  style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else
                                    //tag = "<div class='chip chipUsers' ><input type='checkbox' disabled  style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";

                                div.innerHTML += tag;
                            }
                        }
                    }
                }
            });
        }

        function CloseTeamMemberWindow() {
            debugger
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
                                    //tag = "<div class='chip chipUserRoles'><input type='checkbox' checked='checked' disabled style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else
                                    //tag = "<div class='chip chipUserRoles'><input type='checkbox' disabled  style='margin-right: 5px;' /><label>" + teamArr[i] + "</label></div>";
                                    tag = "<div class='chip chipUserRoles'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                div.innerHTML += tag;
                            } else {
                                if (tempTeamKeyArr.length == 3 && (tempTeamKeyArr[0] == "0" || tempTeamKeyArr[0] == "1") && tempTeamKeyArr[2] == "1")
                                    //tag = "<div class='chip chipUsers' ><input type='checkbox' checked='checked' disabled  style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' checked='checked' disabled><span class='checkmark'></span></label></div>";
                                else
                                    tag = "<div class='chip chipUsers'><label  class='cusCheckContainer'>" + teamArr[i] + "<input type='checkbox' disabled><span class='checkmark'></span></label></div>";
                                //tag = "<div class='chip chipUsers' ><input type='checkbox' disabled  style='margin-right: 5px;'/><label>" + teamArr[i] + "</label></div>";
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
            debugger
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
            debugger
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

        /*==============================================*/

        <%--function BindClickEventForGridCheckBox_UR() {
            $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', function () {
                CheckUncheckAllCheckBoxAsNeeded_UR();
            });

            $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                if ($(this).is(':checked')) {
                    $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                }
                else {
                    $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
                CheckUserRolesCheckBox('<%=RadGrid_UserRoles.ClientID%>');
            });

            UpdatedivDisplayUserRole();
            var line = $("#<%= hdnLineOpenned.ClientID%>").val();
            if (line != null && line != '') {
                var hdnUserRolesID = $(".txtUserRoles_" + line).attr("id").replace("txtUserRoles", "hdnUserRoles");
                var userRoles = $("#" + hdnUserRolesID).val();
                
                // Update selected for grid
                if (userRoles != null && userRoles != "") {
                    var teamArr = userRoles.toString().split(';');
                    // trim value of teamArr
                    $.each(teamArr, function (index, value) {
                        teamArr[index] = value.trim();
                    });

                    $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                        var userId = $(this).closest('tr').find('td:eq(0)').find('span').html();
                        if (teamArr.indexOf(userId) >= 0) {
                            $(this).prop('checked', true);
                            
                        } else {
                            $(this).prop('checked', false);
                        }
                    });
                } else {
                    $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
            }
        }

        function UpdatedivDisplayUserRole() {
            var txtUserRoles = $("#<%=gvCustom.ClientID %> input[id*='txtUserRoles']");
            $.each(txtUserRoles, function (index, item) {
                var txtId = $(item).attr("id");
                var div = document.getElementById(txtId.replace("txtUserRoles", "cusLabelTagUR"));
                div.innerHTML = '';
                var disUserRoles = $(item).val();
                // Update selected for grid
                if (disUserRoles != null && disUserRoles != "") {
                    var teamArr = disUserRoles.toString().split(';');
                    // trim value of teamArr
                    $.each(teamArr, function (index, value) {
                        teamArr[index] = value.trim();
                    });

                    teamArr.sort();

                    if (teamArr != null && teamArr.length > 0)
                        for (var i = 0; i < teamArr.length; i++) {
                            var tag = "<div class='chip' style='width:auto !important;padding-left:5px !important;padding-right:5px !important ;margin-left:2px !important ;margin-right:2px !important ;margin-top:3px !important ;'>" + teamArr[i] + "</div>";
                            div.innerHTML += tag;
                        }
                }
            });
        }

        function CloseUserRoleWindow() {
            var line = $("#<%= hdnLineOpenned.ClientID%>").val();
            if (line != null && line != '') {
                var hdnUserRolesValue = $("#<%= hdnOrgUserRoleID.ClientID%>").val();
                var txtUserRolesValue = $("#<%= hdnOrgUserRoleDisp.ClientID%>").val();

                var txtUserRolesID = $(".txtUserRoles_" + line).attr("id");
                $("#" + txtUserRolesID).val(txtUserRolesValue);
                var hdnUserRolesID = txtUserRolesID.replace("txtUserRoles", "hdnUserRoles");
                $("#" + hdnUserRolesID).val(hdnUserRolesValue);

                var div = document.getElementById(txtUserRolesID.replace("txtUserRoles", "cusLabelTagUR"));
                div.innerHTML = '';
                var disUserRoles = txtUserRolesValue;
                // Update selected for grid
                if (disUserRoles != null && disUserRoles != "") {
                    var teamArr = disUserRoles.toString().split(';');
                    // trim value of teamArr
                    $.each(teamArr, function (index, value) {
                        teamArr[index] = value.trim();
                    });

                    teamArr.sort();

                    if (teamArr != null && teamArr.length > 0)
                        for (var i = 0; i < teamArr.length; i++) {
                            var tag = "<div class='chip' style='width:auto !important;padding-left:5px !important;padding-right:5px !important ;margin-left:2px !important ;margin-right:2px !important ;margin-top:3px !important ;'>" + teamArr[i] + "</div>";
                            div.innerHTML += tag;
                        }
                }
            }
            var wnd = $find('<%=UserRolesWindow.ClientID %>');
            wnd.Close();
        }

        function ShowUserRoleWindow(txtUserRole) {
            var line = $(txtUserRole).closest('tr').find('td:eq(0) > span.customline').text();
            var txtUserRolesId = $(txtUserRole).attr("id");
            var hdnUserRolesId = txtUserRolesId.replace("cusLabelTagUR", "hdnUserRoles");
            var userRoles = $("#" + hdnUserRolesId).val();
            var txtCustomLabelId = txtUserRolesId.replace("cusLabelTagUR", "lblDesc");
            var txtCustomLabelVal = $("#" + txtCustomLabelId).val();
            var txtUserRoleDispId = txtUserRolesId.replace("cusLabelTagUR", "txtUserRoles");
            var txtUserRoleDispVal = $("#" + txtUserRoleDispId).val();

            $('#<%= hdnLineOpenned.ClientID%>').val(line);
            $('#<%= hdnOrgUserRoleID.ClientID%>').val(userRoles);
            $('#<%= hdnOrgUserRoleDisp.ClientID%>').val(txtUserRoleDispVal);

            $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', function () {
                CheckUncheckAllCheckBoxAsNeeded_UR();
            });

            $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                if ($(this).is(':checked')) {
                    $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                }
                else {
                    $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
                CheckUserRolesCheckBox('<%=RadGrid_UserRoles.ClientID%>');
            });

            // Update selected for grid
            if (userRoles != null && userRoles != "") {
                var teamArr = userRoles.toString().split(';');
                // trim value of teamArr
                $.each(teamArr, function (index, value) {
                    teamArr[index] = value.trim();
                });

                $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {
                    var userId = $(this).closest('tr').find('td:eq(0)').find('span').html();
                    if (teamArr.indexOf(userId) >= 0) {
                        $(this).prop('checked', true);
                    } else {
                        $(this).prop('checked', false);
                    }
                });
            } else {
                $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
            }

            var wnd = $find('<%=UserRolesWindow.ClientID %>');
            wnd.set_title("User Roles: " + txtCustomLabelVal);
            wnd.Show();

            //document.getElementById('').click();
        }

        function CheckUserRolesCheckBox(gridview) {
            var line = $("#<%= hdnLineOpenned.ClientID%>").val();
            var hdnUserRolesValue = $("#<%= hdnOrgUserRoleID.ClientID%>").val();
            var txtUserRolesValue = $("#<%= hdnOrgUserRoleDisp.ClientID%>").val();

            var tempArrayKey = [];
            tempArrayKey.length = 0;
            var tempArrayDisplay = [];
            tempArrayDisplay.length = 0;

            $("#" + gridview + " input[id*='chkSelect']:checkbox").each(function (index) {
                var tempMemberKey = $(this).closest('tr').find('td:eq(0)').find('span').html().trim();
                var teamMemberDisp = $(this).closest('tr').find('td:eq(1)').find('span').html().trim();
                if ($(this).is(":checked")) {
                    if (jQuery.inArray(tempMemberKey, tempArrayKey) < 0) {
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
                }
            });
            
            $("#<%= hdnOrgUserRoleID.ClientID%>").val(tempArrayKey.join(";"));
            $("#<%= hdnOrgUserRoleDisp.ClientID%>").val(tempArrayDisplay.join(";"));
        }

        function CheckUncheckAllCheckBoxAsNeeded_UR() {
            var totalCheckboxes = $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkSelect']:checkbox").size();

            var checkedCheckboxes = $("#<%=RadGrid_UserRoles.ClientID%> input[id*='chkSelect']:checkbox:checked").size();

            if (totalCheckboxes == checkedCheckboxes) {

                $("#<%=RadGrid_UserRoles.ClientID %> input[id*='chkAll']:checkbox").prop('checked', true);//.each(function () { this.checked = true; });
            }
            else {
                $("#<%=RadGrid_UserRoles.ClientID %> input[id*='chkAll']:checkbox").prop('checked', false);//.attr('checked', false);
            }

            if ($('#<%=RadGrid_UserRoles.ClientID%>').length > 0) {
                CheckUserRolesCheckBox('<%=RadGrid_UserRoles.ClientID%>');
            }
        }--%>
        function chkMatSalestax_onchange(control) {
            var chkMatSalestax = $(control).find("input[type='checkbox']")[0];
            if (chkMatSalestax != null) {
                var hdnMatChkID = chkMatSalestax.id.replace("chkMatSalestax", "hdnMatChk");
                $("#" + hdnMatChkID).val($(chkMatSalestax).prop("checked"));
            }
        }

        function chkLabSalestax_onchange(control) {
            var chkLabSalestax = $(control).find("input[type='checkbox']")[0];
            if (chkLabSalestax != null) {
                var hdnLbChkID = chkLabSalestax.id.replace("chkLabSalestax", "hdnLbChk");
                $("#" + hdnLbChkID).val($(chkLabSalestax).prop("checked"));
            }
        }

        function chkChangeOrder_onchange(control) {
            var chkChangeOrder = $(control).find("input[type='checkbox']")[0];
            if (chkChangeOrder != null) {
                var hdnChangeOrderChk = chkChangeOrder.id.replace("chkChangeOrder", "hdnChangeOrderChk");
                $("#" + hdnChangeOrderChk).val($(chkChangeOrder).prop("checked"));
            }
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
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
        <img src="images/wheel.GIF" alt="Be patient..." class="lodder"  />
    </div>

    <telerik:RadAjaxManager ID="RadAjaxManager_ProjectTemp" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="lnkAddForm">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowForms" LoadingPanelID="RadAjaxLoadingPanel_ProjectTemp" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkEditForm">
                <UpdatedControls>

                    <telerik:AjaxUpdatedControl ControlID="RadWindowForms" LoadingPanelID="RadAjaxLoadingPanel_ProjectTemp" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ReloadGrid">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divMemberGrid" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSaveTemplate">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdnMilestone" />

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="imgBtnAdd">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvBom" />
                    <telerik:AjaxUpdatedControl ControlID="hdnloadBom" />
                    <telerik:AjaxUpdatedControl ControlID="ddlBType" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="imgBtnAddMileston">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdnMilestone" />
                    <telerik:AjaxUpdatedControl ControlID="gvMilestones" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lnkBompostback">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdnloadBom" />
                    <telerik:AjaxUpdatedControl ControlID="gvBom" />

                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="lbtnTypeSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="hdnloadBom" />
                    <telerik:AjaxUpdatedControl ControlID="gvBom" />
                    <telerik:AjaxUpdatedControl ControlID="ddlBType" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="chkBilFrmBOM">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="gvMilestones" />
                    <telerik:AjaxUpdatedControl ControlID="hdnMilestone" />
                    <telerik:AjaxUpdatedControl ControlID="ddlEstimateType" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_ProjectTemp" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <%--<telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            
        </Windows>
    </telerik:RadWindowManager>--%>


    <telerik:RadWindow ID="TeamMembersWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
        runat="server" Modal="true" Width="1050" Height="635">
        <ContentTemplate>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                <div class="m-t-15" >
                    <div class="form-section-row">
                        <div class="form-section">
                            <div class="row pmd-card" >
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
                                                    <%--<telerik:GridTemplateColumn
                                                            DataField="UserID" SortExpression="UserID" AutoPostBackOnFilter="true" 
                                                            CurrentFilterFunction="Contains" DataType="System.String" HeaderText="User ID" ShowFilterIcon="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server"><%#Eval("UserID")%></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>--%>
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
                                                    <%--<telerik:GridTemplateColumn 
                                                            DataField="fFirst" SortExpression="fFirst" AutoPostBackOnFilter="true" 
                                                            CurrentFilterFunction="Contains" DataType="System.String" HeaderText="First Name" ShowFilterIcon="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFirstName" runat="server"><%#Eval("fFirst")%></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn 
                                                            DataField="fLast" SortExpression="fLast" AutoPostBackOnFilter="true"
                                                            CurrentFilterFunction="Contains" DataType="System.String" HeaderText="Last Name" ShowFilterIcon="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLastName" runat="server"><%#Eval("fLast")%></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>--%>
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
                                                    <%--<telerik:GridBoundColumn DataField="MemberEmail" AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="Contains"
                                                            HeaderText="Email" SortExpression="MemberEmail"
                                                            UniqueName="MemberEmail">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="GroupName" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                            HeaderText="Group Name" SortExpression="GroupName" HeaderStyle-Width="200"
                                                            UniqueName="GroupName" ShowFilterIcon="false">
                                                        </telerik:GridBoundColumn>--%>
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
                    <footer class="footer-css-top-btn">
                        <div class="btnlinks">
                            <a id="lnkPopupOK" onclick="CloseTeamMemberWindow();" style="cursor: pointer;">OK</a>
                            <%--<telerik:RadAjaxPanel runat="server" >
                                    <asp:LinkButton ID="lnkPopupOK" OnClientClick="CloseTeamMemberWindow();" runat="server">OK</asp:LinkButton>
                                </telerik:RadAjaxPanel>--%>
                        </div>
                    </footer>
                    <%--<asp:LinkButton ID="ReloadGrid" runat="server" OnClick="ReloadGrid_Click">Clear Filter</asp:LinkButton>--%>
                </div>
            </telerik:RadAjaxPanel>
        </ContentTemplate>
    </telerik:RadWindow>

    <%--<telerik:RadWindow ID="UserRolesWindow" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false"
        runat="server" Modal="true" Width="1050" Height="635">
        <ContentTemplate>
            <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server">
                <div style="margin-top: 15px;">
                    <div class="form-section-row">
                        <div class="form-section">
                            <div class="row" style="margin-bottom: 0;">
                                <div class="grid_container" id="div4" runat="server">
                                    <div class="RadGrid RadGrid_Material RadGrid RadGrid_Popup">

                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_UserRoles" AllowFilteringByColumn="true" ShowFooter="false" PageSize="1000"
                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" PagerStyle-AlwaysVisible="true" FilterType="CheckList"
                                            AllowCustomPaging="false" Width="100%" Height="516px" OnPreRender="RadGrid_UserRoles_PreRender"
                                            OnNeedDataSource="RadGrid_UserRoles_NeedDataSource">
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
                                                            <asp:Label ID="lblID" runat="server" Style="display: none;"><%#Eval("Id")%></asp:Label>
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemStyle Width="0px"></ItemStyle>
                                                    </telerik:GridTemplateColumn>
                                                    
                                                    <telerik:GridTemplateColumn
                                                        DataField="RoleName" SortExpression="RoleName" AutoPostBackOnFilter="true" DataType="System.String"
                                                        CurrentFilterFunction="Contains" HeaderText="User Role" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUserRole" runat="server"><%#Eval("RoleName")%></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    
                                                    <telerik:GridTemplateColumn
                                                        DataField="Desc" SortExpression="Desc" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" DataType="System.String" HeaderText="Description" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDescription" runat="server"><%#Eval("Desc")%></asp:Label>
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
                            <a id="lnkUserRoleOK" onclick="CloseUserRoleWindow();" style="cursor: pointer;">OK</a>
                        </div>
                    </footer>
                </div>
            </telerik:RadAjaxPanel>
        </ContentTemplate>
    </telerik:RadWindow>--%>

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

                                          <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Project Template</asp:Label>

                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkSaveTemplate" runat="server" OnClick="lnkSaveTemplate_Click" OnClientClick="window.btn_clicked =true; itemJSON(); return ConfirmGCAdd();">Save</asp:LinkButton>

                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkCloseTemplate" runat="server" CausesValidation="False" ToolTip="Close" OnClick="lnkCloseTemplate_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    <div class="btnclosewrap-one">
                                        <a class="collapse-expand" data-position="bottom" data-tooltip="Expand/Collapse Accordion">
                                            <i id="expcolp" title="Expand All" class="mdi-action-open-in-browser"></i>
                                        </a>
                                    </div>
                                    <div class="rght-content">

                                        <div class="editlabel">
                                            Project Template ID:
                                            <asp:Label CssClass="title_text_Name" ID="lblProjectNo" runat="server"></asp:Label>
                                            &nbsp;|
                                            <asp:Label runat="server" ID="lblHeaderLabel"></asp:Label>
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
                                    <li><a href="#accrdFinanceTab">Finance</a></li>
                                    <li><a href="#accrdBOM" onclick="LoadBomItems();">BOM</a></li>
                                    <li><a href="#accrdMilestone">Billing</a></li>
                                    <li><a href="#accrdCustom">Workflow</a></li>
                                    <li runat="server" id="liForms"><a href="#accrdForm">Forms</a></li>
                                    <li runat="server" id="liCheckList" visible="false"><a href="#accrdCheckList">Project Checklist</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlNext" runat="server" Visible="False">
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False" OnClick="lnkFirst_Click">
                                                  <i class="fa fa-angle-double-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False" OnClick="lnkPrevious_Click">
                                                 <i class="fa fa-angle-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False" OnClick="lnkNext_Click">
                                                 <i class="fa fa-angle-right"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False" OnClick="lnkLast_Click">  
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
                    <div class="card cardradius">
                        <div class="card-content">
                            <div class="form-content-wrap form-content-wrapwd formpdtop2">
                                <div class="form-content-pd">
                                    <div class="form-section-row">
                                        <div class="col s12 m12 l12 p-r-0" style="padding-right: 0px;">
                                            <div class="row">
                                                <div class="form-section-row">
                                                    <div class="section-ttle">Template Info.</div>
                                                    <div class="form-input-row">
                                                        <div class="form-section3">
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                                                        ControlToValidate="txtREPdesc" Display="None" ErrorMessage="Name Required" SetFocusOnError="True">
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender
                                                                        ID="RequiredFieldValidator9_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                        TargetControlID="RequiredFieldValidator9">
                                                                    </asp:ValidatorCalloutExtender>
                                                                    <asp:TextBox ID="txtREPdesc" runat="server" AutoCompleteType="None" MaxLength="255"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lbltxtREPdesc" AssociatedControlID="txtREPdesc">Template Name</asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Department</label>
                                                                    <asp:DropDownList ID="ddlJobType" runat="server" CssClass="browser-default" OnSelectedIndexChanged="ddlJobType_SelectedIndexChanged" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvJobType" runat="server" InitialValue="Select Department"
                                                                        ControlToValidate="ddlJobType" Display="None" ErrorMessage="Department Required" SetFocusOnError="True">
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceJobType" runat="server" Enabled="True"
                                                                        TargetControlID="rfvJobType">
                                                                    </asp:ValidatorCalloutExtender>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Status</label>
                                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default">
                                                                        <asp:ListItem Value="0">Active</asp:ListItem>
                                                                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:Label runat="server" ID="lbltxtREPremarks" AssociatedControlID="txtREPremarks">Remarks</asp:Label>
                                                                    <asp:TextBox ID="txtREPremarks" runat="server" CssClass="materialize-textarea mtarea" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">&nbsp;</div>
                                                        <div class="form-section3">
                                                            <div class="input-field col s12" id="tempRev" runat="server">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtTempRev" runat="server" AutoCompleteType="None"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lbltxtTempRev" AssociatedControlID="txtTempRev">Template Rev</asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12" id="tempRemarks" runat="server">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtTempRemarks" runat="server" AutoCompleteType="None"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lbltxtTempRemarks" AssociatedControlID="txtTempRemarks">Template Remarks</asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Alert Type</label>
                                                                    <asp:DropDownList ID="ddlAlertType" runat="server" CssClass="browser-default">
                                                                        <asp:ListItem Value="Select Type">Select Type</asp:ListItem>
                                                                        <asp:ListItem Value="0">Email</asp:ListItem>
                                                                        <asp:ListItem Value="1">Text</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row checkbox">
                                                                    <asp:CheckBox ID="chkAlert" CssClass="css-checkbox" runat="server" Text="Alert Manager" />
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s6">
                                                                <div class="row checkbox">
                                                                    <asp:CheckBox ID="chkSglBilAmt" CssClass="css-checkbox" runat="server" Text="Single Billing Amount" />
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s6">
                                                                <div class="row checkbox">
                                                                    <asp:CheckBox ID="chkBilFrmBOM" OnCheckedChanged="chkBilFrmBOM_CheckedChanged" AutoPostBack="true" onclick="return chkBilFrmBOM_ChangeConfirm(this);" CssClass="css-checkbox" runat="server" Text="Billing from BOM" />
                                                                </div>
                                                            </div>
                                                            <%--<div class="input-field col s12">
                                                                <div class="row checkbox">
                                                                    <asp:CheckBox ID="CheckBox1" runat="server" CssClass="css-checkbox" Text="Single Billing Amount" />
                                                                </div>
                                                            </div>--%>
                                                        </div>
                                                        <div class="form-section3-blank">&nbsp;</div>
                                                        <div class="form-section3">
                                                            <div class="input-field col s12" id="Div1" runat="server">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtOHPercentage" onkeypress="return isDecimalKey(this,event)" runat="server" AutoCompleteType="None"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lblOHPercentage" AssociatedControlID="txtOHPercentage">% OH</asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12" id="Div2" runat="server">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtCommissionPercentage" onkeypress="return isDecimalKey(this,event)" runat="server" AutoCompleteType="None"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lblCommissionPercentage" AssociatedControlID="txtCommissionPercentage">% Commission</asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12" id="Div3" runat="server">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtProfitPercentage" onkeypress="return isDecimalKey(this,event)" runat="server" AutoCompleteType="None"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lblProfitPercentage" AssociatedControlID="txtProfitPercentage">% Profit</asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Estimate Type</label>
                                                                    <asp:DropDownList ID="ddlEstimateType" runat="server" CssClass="browser-default">
                                                                        <asp:ListItem Value="bid">Bid</asp:ListItem>
                                                                        <asp:ListItem Value="quote">T&M</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Sales Tax</label>
                                                                    <asp:DropDownList ID="ddlSalesTax" runat="server" CssClass="browser-default">
                                                                        <asp:ListItem Value="0">Select Sales Tax</asp:ListItem>
                                                                    </asp:DropDownList>
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
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li>
                            <div id="accrdFinanceTab" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>Finance</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <uc1:uc_AccountSearch ID="uc_InvExpGL" runat="server" />
                                                        <label for="ctl00_ContentPlaceHolder1_TabContainer2_tbpnlFinance_uc_InvExpGL_txtGLAcct">Expense GL</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <uc1:uc_AccountSearch ID="uc_InterestGL" runat="server" />
                                                        <label for="ctl00_ContentPlaceHolder1_TabContainer2_tbpnlFinance_uc_InterestGL_txtGLAcct">Interest GL</label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtInvService" runat="server" MaxLength="255"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnInvServiceID" runat="server" />
                                                        <asp:Label runat="server" ID="lbltxtInvService" AssociatedControlID="txtInvService">Billing Code</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                            ControlToValidate="txtPrevilWage" Display="None" ErrorMessage="Labor Wage Required for Job Costing" SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender
                                                            ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                            TargetControlID="RequiredFieldValidator2">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtPrevilWage" runat="server" MaxLength="255"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label1" AssociatedControlID="txtPrevilWage">Labor Wage</asp:Label>
                                                        <asp:HiddenField ID="hdnPrevilWageID" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">&nbsp;</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12 hideShowOnPostingType">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtUnrecognizedRevenue" runat="server" MaxLength="255"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtUnrecognizedRevenue" AssociatedControlID="txtUnrecognizedRevenue">Unrecognized Revenue</asp:Label>
                                                        <asp:HiddenField ID="hdnUnrecognizedRevenue" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s12 hideShowOnPostingType">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtUnrecognizedExpense" runat="server" MaxLength="255"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtUnrecognizedExpense" AssociatedControlID="txtUnrecognizedRevenue">Unrecognized Expense</asp:Label>
                                                        <asp:HiddenField ID="hdnUnrecognizedExpense" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s12 hideShowOnPostingType">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtRetainageReceivable" runat="server" MaxLength="255"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtRetainageReceivable" AssociatedControlID="txtRetainageReceivable"> Retainage Receivable</asp:Label>
                                                        <asp:HiddenField ID="hdnRetainageReceivable" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Posting Type</label>
                                                        <asp:DropDownList ID="ddlPostingMethod" runat="server" CssClass="browser-default" onChange="HideShowOnPostingTypeChange(this.value);"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">&nbsp;</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Service Type</label>
                                                        <asp:DropDownList ID="ddlContractType" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row checkbox">
                                                        <asp:CheckBox ID="chkChargeInt" CssClass="css-checkbox" runat="server" Text="Charge Interest" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row checkbox">
                                                        <asp:CheckBox ID="chkInvoicing" CssClass="css-checkbox" Text="Close after Invoicing" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row checkbox">
                                                        <asp:CheckBox ID="chkChargeable" CssClass="css-checkbox" Text="Tickets Chargeable" runat="server" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="cf"></div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdBOM" onclick="LoadBomItems();" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-done"></i>BOM</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap form-content-wrapwd">
                                    <div class="form-content-pd">
                                        <asp:CheckBox ID="chkTargetHours" Style="margin-left: 10px;" CssClass="css-checkbox m-l10" runat="server" Text="Targeted Hours" />
                                        <div class="grid_container">
                                            <%--<div class="table-scrollable" style="margin-bottom: 0 !important;overflow:scroll;height:400px;">--%>
                                            <div class="form-section-row pmd-card" >
                                                <div class="RadGrid RadGrid_Material BomGrid">
                                                    <asp:LinkButton ID="lnkBompostback" runat="server" Style="display: none;" CausesValidation="false" Text="" OnClick="lnkBompostback_Click" />
                                                    <asp:HiddenField ID="hdnloadBom" runat="server" Value="0" />
                                                    <telerik:RadGrid ID="gvBOM" runat="server" CssClass="BomGrid" AutoGenerateColumns="False" Width="100%"
                                                        RenderMode="Auto" AllowFilteringByColumn="true" ShowGroupPanel="false" ShowStatusBar="true" AllowPaging="false"
                                                        PageSize="50" ShowFooter="true"
                                                        OnItemCommand="gvBOM_RowCommand">
                                                        <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                            <ClientEvents OnRowContextMenu="RowContextMenuBomGrid" />
                                                        </ClientSettings>
                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                            <Columns>
                                                                <telerik:GridTemplateColumn HeaderStyle-Width="50">
                                                                    <HeaderTemplate>
                                                                        <a id="Button2" class="delButton" onclick="return DeleteBOMClick();" >
                                                                            <i class="mdi-navigation-cancel cancel-css" style="color: #f00; font-size: 1.2em; font-weight: bold; margin-top: -30px;"></i></a>
                                                                        <asp:CheckBox ID="chkSelectAll" onchange="selectAllCheckbox(this, 'BomItem');" runat="server" CssClass="css-checkbox chkSelectAll" Text=" " />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" CssClass="css-checkbox chkSelect" Text=" " runat="server" />
                                                                        <asp:HiddenField ID="hdnOrderNo" runat="server" Value='<%# Eval("OrderNo") %>'></asp:HiddenField>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="imgBtnAdd" runat="server" CommandName="AddBOMItem" CausesValidation="False" Style="color: #000; font-size: 1.5em;"
                                                                            CommandArgument="<%# ((GridFooterItem) Container).RowIndex %>" Width="19px" OnClientClick="return AddBOMClick();">
                                                                            <i class="mdi-content-add-circle" style="color: #2bab54;font-size: 1.2em; font-weight: bold;margin-left:-15px;"></i>
                                                                        </asp:LinkButton>
                                                                    </FooterTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Line No." Display="false">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("JobTItemID") %>'></asp:HiddenField>
                                                                        <asp:Label ID="lblLine" Width="80" runat="server" Text='<%# Eval("Line") %>' Style="display: none;"></asp:Label>
                                                                        <asp:Label ID="lblIndex" Width="80" runat="server" Text='<%# Container.ItemIndex +1 %>'></asp:Label>
                                                                        <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>
                                                                        <asp:HiddenField ID="hdnIndex" runat="server" Value='<%# Container.ItemIndex +1 %>'></asp:HiddenField>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderStyle-Width="40" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <div class="handle" style="cursor: move;" title="Move Up/Down">
                                                                            <img src="images/Dragdrop.png" width="20" />
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn HeaderText="Op Sequence" HeaderStyle-Width="100">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtGroup" class="Groupsearchinput" runat="server" Text='<%# Eval("GroupName") %>' Style="display: none;" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                        <asp:HiddenField ID="hdnGroupID" Value='<%# Eval("GroupID") %>' runat="server" />
                                                                        <asp:TextBox ID="txtCode" Width="100" runat="server" Text='<%# Eval("Code") %>' CssClass="form-control input-sm input-small txtCodeAutoComplate"></asp:TextBox>
                                                                        <asp:HiddenField ID="hdnCode" runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="CodeDesc" HeaderStyle-Width="90">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ReadOnly="true" ID="lblCodeDesc" runat="server" Text='<%# Eval("CodeDesc") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="180">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlBType" Width="160" runat="server" DataTextField="Type" SelectedValue='<%# Eval("BType") == DBNull.Value ? "0" : Eval("BType") %>'
                                                                            DataValueField="ID" DataSource='<%#dtBomType%>' CssClass="browser-default">
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Material Item" HeaderStyle-Width="180">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtddlMatItem" runat="server" Text='<%# Eval("MatDesc") %>' Width="160"
                                                                            CssClass="form-control input-sm input-small ddlMatItem-search" placeholder="Search by Material Item..."></asp:TextBox>
                                                                        <asp:HiddenField ID="hdnddlMatItemId" runat="server" Value='<%# Eval("MatItem") %>' />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="150">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtScope" runat="server" Width="150" MaxLength="100" Text='<%# Eval("fDesc") %>' CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Qty Req." HeaderStyle-Width="90">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtQtyReq" runat="server" Width="80" Text='<%# Eval("QtyReq","{0:n}") %>' Style="text-align: right;" onchange="calBudgetExt(this)"
                                                                            CssClass="form-control input-sm input-small"
                                                                            onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="U/M" HeaderStyle-Width="60">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtUM" Width="80" runat="server" Text='<%# Eval("UM") %>' CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                        <asp:HiddenField ID="hdnUMID" runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Budget Unit $" HeaderStyle-Width="125">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtBudgetUnit" Width="100" runat="server" Text='<%# Eval("BudgetUnit","{0:n}") %>' onchange="calBudgetExt(this)"
                                                                            CssClass="form-control input-sm input-small" Style="text-align: right; width: 100px!important;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Material Mod" HeaderStyle-Width="125">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMatMod" runat="server" Width="100" Text='<%# Eval("MatMod","{0:n}")%>' CssClass="form-control input-sm input-small"
                                                                            onchange="showDecimalVal(this)" Style="text-align: right; width: 100px!important;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Material Ext $" HeaderStyle-Width="150">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMatExt" runat="server" Width="100" Text='<%# Eval("BudgetExt","{0:n}") %>' Style="text-align: right; width: 100px!important;"></asp:Label>
                                                                        <asp:HiddenField ID="hdnMatExt" runat="server" Value='<%# Eval("BudgetExt","{0:n}") %>' />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn HeaderText="Mat. Tx." UniqueName="chkMatSalestax" HeaderStyle-Width="80">
                                                                    <HeaderTemplate>
                                                                        <asp:Label runat="server" CssClass="display-block">Mat. Tx.</asp:Label>
                                                                        <asp:CheckBox ID="chkSelectAllMatTx" onchange="selectAllCheckbox(this, 'MatTx');"
                                                                            runat="server" CssClass="css-checkbox chkSelectAllMatTx" Text=" " />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkMatSalestax" onchange="chkMatSalestax_onchange(this);" CssClass="chkMatSalestax" Checked='<%# (Convert.ToString(Eval("STax")) == "1") ? true : false %>'
                                                                            runat="server" />
                                                                        <asp:HiddenField ID="hdnMatChk" runat="server" Value='<%# Convert.ToString(Eval("STax")) == "1" ? true : false %>' />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Vendor" HeaderStyle-Width="200">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtVendor" runat="server" Text='<%# Eval("Vendor") %>' Width="160"
                                                                            CssClass="form-control input-sm input-small vendor-search" placeholder="Search by vendor"></asp:TextBox>
                                                                        <asp:HiddenField ID="hdnVendorId" runat="server" Value='<%# Eval("VendorId") %>' />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Labor Item" HeaderStyle-Width="120">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtLabItem" Width="150" runat="server" placeholder="Search by Labor Item..." Text='<%# Eval("txtLabItem") %>' CssClass="labor-search">
                                                                        </asp:TextBox>
                                                                        <asp:HiddenField ID="hdntxtLabItem" Value='<%# Eval("LabItem") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Hours" HeaderStyle-Width="70">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtHours" runat="server" Text='<%# Eval("LabHours","{0:n}") %>' CssClass="form-control input-sm input-small"
                                                                            onchange="calLabExt(this)" Width="80"
                                                                            Style="text-align: right; width: 100px!important;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Rate" HeaderStyle-Width="70">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtLabRate" runat="server" Text='<%# Eval("LabRate","{0:n}")  %>' CssClass="form-control input-sm input-small"
                                                                            onchange="calLabExt(this)" Width="80"
                                                                            Style="text-align: right; width: 100px!important;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Labor Mod" HeaderStyle-Width="90">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtLabMod" runat="server" Text='<%# Eval("LabMod","{0:n}") %>' CssClass="form-control input-sm input-small"
                                                                            onchange="showDecimalVal(this)" Width="80"
                                                                            Style="width: 100px!important; text-align: right;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Labor Ext $" HeaderStyle-Width="90">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLabExt" runat="server" Text='<%# Eval("LabExt","{0:n}")  %>' Width="100"></asp:Label>
                                                                        <asp:HiddenField ID="hdnLabExt" runat="server" Value='<%# Eval("LabExt","{0:n}") %>' />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Labor Tx." UniqueName="chkLabSalestax" HeaderStyle-Width="70">
                                                                    <HeaderTemplate>
                                                                        <asp:Label runat="server" CssClass="display-block">Labor Tx.</asp:Label>
                                                                        <asp:CheckBox ID="chkSelectAllLaborTx" onchange="selectAllCheckbox(this, 'LaborTx');"
                                                                            runat="server" CssClass="css-checkbox chkSelectAllLaborTx" Text=" " />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkLabSalestax" onchange="chkLabSalestax_onchange(this);" CssClass="chkLabSalestax" Checked='<%# (Convert.ToString(Eval("LStax"))=="1") ? true : false %>'
                                                                            runat="server" />
                                                                        <asp:HiddenField ID="hdnLbChk" runat="server" Value='<%# Convert.ToString(Eval("LSTax")) == "1" ? true : false %>' />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Total Ext $" HeaderStyle-Width="90">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotalExt" runat="server" Width="100" Text='<%# Eval("TotalExt","{0:n}")  %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="90">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtSDate" runat="server" Width="100" Text='<%# Eval("SDate")!=DBNull.Value? (!(Eval("SDate").Equals(DateTime.MinValue)) ? (String.Format("{0:MM/dd/yyyy}", Eval("SDate"))) : "" ) : "" %>' CssClass="form-control input-sm input-small datepicker_mom"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                    <input type="hidden" runat="server" id="radGridClickedRowIndex" name="radGridClickedRowIndex" />
                                                    <telerik:RadContextMenu ID="RadMenuBomGrid" runat="server" OnItemClick="RadMenuBomGrid_ItemClick" OnClientItemClicking="CheckAddBomGrid"
                                                        EnableRoundedCorners="true" EnableShadows="true">
                                                        <Items>
                                                            <telerik:RadMenuItem Text="Add Row Above">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem Text="Add Row Below">
                                                            </telerik:RadMenuItem>
                                                        </Items>
                                                    </telerik:RadContextMenu>

                                                </div>
                                            </div>
                                            <%--</div>--%>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdMilestone" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-flag"></i>Billing</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap form-content-wrapwd">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                <div class="RadGrid RadGrid_Material">
                                                    <telerik:RadGrid ID="gvMilestones" runat="server" AutoGenerateColumns="False" CssClass="BomGrid" Width="100%"
                                                        RenderMode="Auto" AllowFilteringByColumn="true" ShowStatusBar="true" AllowPaging="True"
                                                        PageSize="20" ShowFooter="true"
                                                        OnItemDataBound="gvMilestones_ItemDataBound"
                                                        OnItemCommand="gvMilestones_ItemCommand">
                                                        <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="3" />
                                                            <ClientEvents OnRowContextMenu="RowContextMenuMilesonteGrid" />
                                                        </ClientSettings>
                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                            <Columns>
                                                                <telerik:GridTemplateColumn HeaderStyle-Width="50">
                                                                    <HeaderTemplate>
                                                                        <a id="Button2" onclick="return DeleteMilestonesClick();"
                                                                            style="cursor: pointer; font-size: 1.5em !important;">
                                                                            <i class="mdi-navigation-cancel" style="color: #f00; font-size: 1.2em; font-weight: bold; margin-top: -28px;"></i></a>
                                                                        <asp:CheckBox ID="chkSelectAll" onchange="selectAllCheckbox(this, 'BomItem');" runat="server" CssClass="css-checkbox chkSelectAll" Text=" " />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" CssClass="css-checkbox chkSelect" Text=" " runat="server" />
                                                                        <asp:HiddenField ID="hdnOrderNoMil" runat="server" Value='<%# Eval("OrderNo") %>'></asp:HiddenField>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="imgBtnAddMileston" runat="server" CommandName="AddMilestoneItem" CausesValidation="False" Style="color: #000; font-size: 1.5em;"
                                                                            CommandArgument="<%# ((GridFooterItem) Container).RowIndex %>" Width="20px" OnClientClick="return AddMilestonesClick();">
                                                                                             <i class="mdi-content-add-circle" style="color: #2bab54;font-size: 1.2em; font-weight: bold;margin-left:-15px;"></i>
                                                                        </asp:LinkButton>
                                                                    </FooterTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn HeaderText="Line No." HeaderStyle-Width="50" Display="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIndex" runat="server" Text="<%# Container.ItemIndex +1 %>"></asp:Label>
                                                                        <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>
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
                                                                        <asp:TextBox ID="txtGroup" class="Groupsearchinput" runat="server" Text='<%# Eval("GroupName") %>' Style="display: none;" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                        <asp:HiddenField ID="hdnGroupID" Value='<%# Eval("GroupID") %>' runat="server" />
                                                                        <asp:TextBox ID="txtCode" runat="server" Style="width: 100%!important;" Text='<%# Eval("jcode") %>' CssClass="form-control input-sm input-small txtCodeAutoComplate"></asp:TextBox>
                                                                        <asp:HiddenField ID="hdnCode" runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn HeaderText="CodeDesc" HeaderStyle-Width="100">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="lblCodeDesc" ReadOnly="true" runat="server" Text='<%# Eval("CodeDesc") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>

                                                                <telerik:GridTemplateColumn HeaderText="Type">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlType" runat="server" SelectedValue='<%# Eval("jtype") == DBNull.Value ? 0 : Eval("jtype") %>'
                                                                            Style="width: 100%!important;" CssClass="browser-default">
                                                                            <asp:ListItem Value="0">Revenue</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Function">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtSType" runat="server" Text='<%# Eval("Department") %>' Style="width: 100%!important;" CssClass="form-control input-sm input-small"
                                                                            placeholder="Select Function"></asp:TextBox>
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
                                                                <telerik:GridTemplateColumn HeaderText="Name" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtName" runat="server" MaxLength="100" Text='<%# Eval("MilesName") %>'
                                                                            Style="width: 100%!important; text-align: justify !important" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Description" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtScope" runat="server" MaxLength="100" Text='<%# Eval("fdesc") %>'
                                                                            Style="width: 100%!important;" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Quantity" FooterStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity","{0:n}") %>' onkeypress="return isDecimalKey(this,event)"
                                                                            onchange="CalBillingAmount(this,1)"
                                                                            Style="text-align: right; width: 100%!important;" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Price" FooterStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPrice" runat="server" Text='<%# Eval("Price","{0:n}") %>' onkeypress="return isDecimalKey(this,event)"
                                                                            onchange="CalBillingAmount(this,2)"
                                                                            Style="text-align: right; width: 100%!important;" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Amount $" FooterStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAmount" runat="server" Text='<%# Eval("Amount","{0:n}") %>' onkeypress="return isDecimalKey(this,event)"
                                                                            onchange="CalBillingAmount(this,3)"
                                                                            Style="text-align: right; width: 100%!important;" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Required by">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRequiredBy" runat="server" Text='<%# Eval("RequiredBy")!=DBNull.Value? (!(Eval("RequiredBy").Equals(DateTime.MinValue)) ? (String.Format("{0:MM/dd/yyyy}", Eval("RequiredBy"))) : "" ) : "" %>'
                                                                            Style="width: 100%!important;" CssClass="datepicker_mom"></asp:TextBox>
                                                                        <%-- <asp:CalendarExtender ID="txtRequiredBy_CalendarExtender" runat="server" Enabled="True"
                                                                                            TargetControlID="txtRequiredBy">
                                                                                        </asp:CalendarExtender>--%>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                    <input type="hidden" runat="server" id="radMilGridClickedRowIndex" name="radMilGridClickedRowIndex" />
                                                    <telerik:RadContextMenu ID="RadMenuMilGrid" runat="server" OnItemClick="RadMenuMilGrid_ItemClick" OnClientItemClicking="CheckAddMilGrid"
                                                        EnableRoundedCorners="true" EnableShadows="true">
                                                        <Items>
                                                            <telerik:RadMenuItem Text="Add Row Above">
                                                            </telerik:RadMenuItem>
                                                            <telerik:RadMenuItem Text="Add Row Below">
                                                            </telerik:RadMenuItem>
                                                        </Items>
                                                    </telerik:RadContextMenu>
                                                    <%--<Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvMilestones" EventName="RowCommand" />
                                                    </Triggers>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdCustom" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Workflow</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap form-content-wrapwd">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                <div class="RadGrid RadGrid_Material">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:HiddenField ID="hdnLineOpenned" runat="server" />
                                                            <asp:HiddenField ID="hdnOrgMemberKey" runat="server" />
                                                            <asp:HiddenField ID="hdnOrgMemberDisp" runat="server" />
                                                            <asp:HiddenField ID="hdnOrgUserRoleID" runat="server" />
                                                            <asp:HiddenField ID="hdnOrgUserRoleDisp" runat="server" />
                                                            <asp:GridView ID="gvCustom" runat="server" AutoGenerateColumns="False" AlternatingRowStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HorizontalAlign="Center"
                                                                CssClass="BomGrid" Width="100%"
                                                                ShowFooter="true" HeaderStyle-Font-Size="0.9em" OnRowCommand="gvCustom_RowCommand"
                                                                OnRowDataBound="gvCustom_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-Width="0.5%" HeaderStyle-Width="0.5%" FooterStyle-Width="0.5%">
                                                                        <HeaderTemplate>
                                                                            <asp:LinkButton ID="ibtnDeleteCItem" OnClientClick="return confirm('Are you sure you want to delete the items? This will delete the items from Workflow field.')"
                                                                                CausesValidation="false" ToolTip="Delete" runat="server" Style="color: #000; font-size: 1.5em;" Width="20px"
                                                                                OnClick="ibtnDeleteCItem_Click"><i class="mdi-navigation-cancel" style="margin-left: -2px; color: #f00;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>

                                                                            <asp:HiddenField ID="txtRowLine" Value='<%# Eval("OrderNo") %>' runat="server"></asp:HiddenField>
                                                                            <%--<asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>--%>
                                                                            <asp:Label ID="lblIndex" Visible="false" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                                            <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' CssClass="customline" Style="display: none;"></asp:Label>
                                                                            <asp:CheckBox ID="chkSelect" CssClass="css-checkbox" Text=" " runat="server" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:LinkButton ID="lnkAddnewRow" runat="server" CausesValidation="False" Style="color: #000; font-size: 1.5em;" Width="20px"
                                                                                ToolTip="Add New Row" OnClick="lnkAddnewRow_Click"><i class="mdi-content-add-circle" style="margin-left: -9px; color: #2bab54;font-size: 1.2em; font-weight: bold;"></i></asp:LinkButton>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <div class="handle" style="cursor: move" title="Move Up/Down">
                                                                                <img src="images/Dragdrop.png" width="20px" />
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Label"
                                                                        FooterStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblDesc" runat="server" Text='<%# Eval("Label") %>' MaxLength="255" Style='min-width: 100px!important;'
                                                                                CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                            <%--<asp:RequiredFieldValidator ID="rfvDescT" runat="server" ControlToValidate="lblDesc"
                                                                                Display="Dynamic" ErrorMessage="***Required***" SetFocusOnError="True" ValidationGroup="ctempl"></asp:RequiredFieldValidator>--%>
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Tab"
                                                                        FooterStyle-VerticalAlign="Middle">
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlTab" runat="server" Width="100%" CssClass="browser-default" Style='min-width: 80px !important;'
                                                                                DataValueField="ID" DataSource='<%#dtTab%>' DataTextField="tabname"
                                                                                SelectedValue='<%# Eval("tblTabID") == DBNull.Value ? 0 : Eval("tblTabID") %>'>
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Alert " HeaderStyle-CssClass="itemHeader" HeaderStyle-Width="50">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelectAlert" CssClass="css-checkbox" Text=" " runat="server"
                                                                                Checked='<%# Convert.ToBoolean((Eval("IsAlert")==DBNull.Value ? false:Eval("IsAlert"))) %>' />
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                    <%--<asp:TemplateField HeaderText="Task " HeaderStyle-CssClass="itemHeader" HeaderStyle-Width="50">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelectTask" CssClass="css-checkbox" Text=" " runat="server"
                                                                                Checked='<%# Convert.ToBoolean((Eval("IsTask")==DBNull.Value ? false:Eval("IsTask"))) %>' />
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </asp:TemplateField>--%>
                                                                    <%--<asp:TemplateField
                                                                        HeaderText="User Role" HeaderStyle-CssClass="itemHeader">
                                                                        <ItemTemplate>
                                                                            <div class="tag-div materialize-textarea textarea-border" id="cusLabelTagUR" style="text-align: left !important; cursor: pointer;" onclick="ShowUserRoleWindow(this);" runat="server"></div>
                                                                            <asp:HiddenField ID="hdnUserRoles" runat="server" Value='<%# Eval("UserRole") %>' />
                                                                            <asp:TextBox ID="txtUserRoles" class='<%# "txtUserRoles_" + Eval("Line") %>' runat="server" Style='min-width: 100px !important; display: none;'
                                                                                Text='<%# Eval("UserRoleDisplay") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </asp:TemplateField>--%>
                                                                    <asp:TemplateField
                                                                        HeaderText="Team Member" HeaderStyle-CssClass="itemHeader">
                                                                        <ItemTemplate>
                                                                            <div class="tag-div materialize-textarea textarea-border" id="cusLabelTag" style="text-align: left !important; cursor: pointer;" onclick="ShowTeamMemberWindow(this);" runat="server"></div>
                                                                            <asp:HiddenField ID="hdnMembers" runat="server" Value='<%# Eval("TeamMember") %>' />
                                                                            <asp:TextBox ID="txtMembers" class='<%# "txtMembers_" + Eval("Line") %>' runat="server" Style='min-width: 100px !important; display: none;'
                                                                                Text='<%# Eval("TeamMemberDisplay") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <FooterStyle VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Format" Visible="true">
                                                                        <ItemTemplate>
                                                                            <table style="border-spacing: 0; padding: 0;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="ddlFormat" runat="server" Width="100px" AutoPostBack="true" CssClass="browser-default"
                                                                                            OnSelectedIndexChanged="ddlFormat_SelectedIndexChanged" DataValueField="value" DataSource='<%#dtFormat%>' DataTextField="format"
                                                                                            SelectedValue='<%# Eval("format") == DBNull.Value ? 0 : Eval("format") %>'>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Panel ID="pnlCustomValue" runat="server" Visible="false">
                                                                                            <table style="border-spacing: 0px; padding: 0px;">
                                                                                                <tr>
                                                                                                    <td style="padding-left: 10px;">
                                                                                                        <asp:DropDownList ID="ddlCustomValue" Width="100px" runat="server" AutoPostBack="true"
                                                                                                            OnSelectedIndexChanged="ddlCustomValue_SelectedIndexChanged" CssClass="browser-default">
                                                                                                            <asp:ListItem Value="">--Add New--</asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                    <td style="padding-left: 10px;">
                                                                                                        <asp:TextBox ID="txtCustomValue" Width="100px" runat="server" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                                                    </td>
                                                                                                    <td style="padding-left: 10px;">
                                                                                                        <table style="border-spacing: 0; padding: 0;">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <div class="btnlinks">
                                                                                                                        <asp:LinkButton ID="lnkAddCustomValue" CommandName="AddCustomValue" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                                            runat="server" CausesValidation="False">Add</asp:LinkButton>
                                                                                                                    </div>
                                                                                                                    <div class="btnlinks">
                                                                                                                        <asp:LinkButton ID="lnkUpdateCustomValue" CommandName="UpdateCustomValue" Visible="false" runat="server"
                                                                                                                            CommandArgument='<%# Container.DataItemIndex %>' CausesValidation="False">Update</asp:LinkButton>
                                                                                                                    </div>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:LinkButton ID="lnkDelCustomValue" CommandName="DeleteCustomValue" CommandArgument='<%# Container.DataItemIndex %>'
                                                                                                                        CausesValidation="False" Visible="false" runat="server">
                                                                                                                    <img height="12px" alt="Delete" title="Delete" src="images/delete-grid.png" /></asp:LinkButton>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <div style="float: left; margin-top: 5px; margin-left: 10px;">
                                                                                <asp:Label ID="lblRowCount" runat="server" Text=""></asp:Label>

                                                                            </div>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlJobType" />
                                                            <asp:AsyncPostBackTrigger ControlID="hdnLineOpenned" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li runat="server" id="adForms">
                            <div id="accrdForm" class="collapsible-header accrd accordian-text-custom"><i class="mdi-av-web"></i>Forms</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap form-content-wrapwd">
                                    <div class="form-content-pd">
                                        <asp:UpdatePanel ID="updatepnl" runat="server" UpdateMode="Always">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="gvDocuments" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlDocPermission" runat="server">
                                                    <div class="btncontainer">
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkAddForm" runat="server" CausesValidation="False" OnClick="lnkAddForm_Click" OnClientClick="return AddDocumentClick(this);">Add</asp:LinkButton>
                                                        </div>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkEditForm" runat="server" CausesValidation="False" OnClick="lnkEditForm_Click" OnClientClick="return ViewDocumentClick(this);">Edit</asp:LinkButton>
                                                        </div>
                                                        <div class="btnlinks">
                                                            <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" OnClientClick="return DeleteDocumentClick(this); SelectedRowDelete('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvDocuments','form');">Delete</asp:LinkButton>
                                                        </div>
                                                </asp:Panel>
                                                <div class="grid_container">
                                                    <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                        <div class="RadGrid RadGrid_Material">
                                                            <asp:GridView ID="gvDocuments" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                Width="100%">
                                                                <RowStyle CssClass="evenrowcolor" />
                                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="ID" Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'>
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="File Name">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkFileName" runat="server" CausesValidation="false" CommandArgument='<%#Eval("ID")%>'
                                                                                OnClick="lnkFileName_Click" Text='<%# Eval("filename") %>'> 
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Added by">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddedBy" runat="server" Text='<%# Eval("AddedBy") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Added On">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAddedOn" runat="server" Text='<%# Eval("AddedOn") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Updated By">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUpdatedBy" runat="server" Text='<%# Eval("UpdatedBy") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Updated On">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUpdatedOn" runat="server" Text='<%# Eval("UpdatedOn") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </li>
                        <li id="adCheckList" runat="server" visible="false">
                            <div id="accrdCheckList" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Project Checklist</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap form-content-wrapwd">
                                    <div class="form-content-pd">
                                        <div class="grid_container">
                                            <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
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

    <asp:UpdatePanel runat="server" pdateMode="Always">
        <ContentTemplate>
            <div id="popup" class="form-section-row">
                <div class="row">
                    <a href="#" runat="server" value="Add New" id="btnAddNew"></a>
                    <asp:ModalPopupExtender ID="mpeAddBomType" BackgroundCssClass="ModalPopupBG"
                        runat="server" CancelControlID="btnCancel" OkControlID="btnOkay"
                        TargetControlID="btnAddNew" BehaviorID="programmaticModalPopupBehavior"
                        PopupControlID="pnlAddBomType" Drag="true" PopupDragHandleControlID="PopupHeader" OnOkScript="ReloadPage();">
                    </asp:ModalPopupExtender>
                    <div class="popup_Buttons" style="display: none">
                        <input id="btnOkay" value="Done" type="button" />
                        <input id="btnCancel" value="Cancel" type="button" />
                    </div>
                    <div id="pnlAddBomType" class="new_popupCSS" style="display: none; width: 350px !important;">
                        <div>
                            <div class="title_bar_popup">
                                <span class="rwIcon">
                                    <asp:Label CssClass="title_text" Style="color: white" ID="lblAddBomType" runat="server">Add BOM Type</asp:Label>
                                </span>
                                <div style="float: right;">
                                    <a onclick="$find('programmaticModalPopupBehavior').hide()" style="cursor: pointer; float: right; color: #fff; margin-left: 10px; height: 16px;"><span class="rwCommandButton rwCloseButton t-ripple-effect-icon t-ripple-center" title="Close"><span class="t-ripple-container"><span class="t-ripple t-ripple-white" style="background-color: rgb(255, 255, 255); width: 35px; height: 35px; transform: translate(-50%, -50%) translate(11px, 11px);"></span></span></span></a>
                                    <asp:LinkButton runat="server" ID="lbtnTypeSubmit" ValidationGroup="Type" Text="Save" OnClick="lbtnTypeSubmit_Click" Style="float: right; color: #fff; margin-left: 10px;" />
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div id="addBOMBody" style="margin: 10px;">
                                <div class="form-section-row">
                                    <div class="input-field col s12">
                                        <div class="row">
                                            <label for="txtBomType">BOM Type</label>
                                            <asp:TextBox ID="txtBomType" runat="server"
                                                MaxLength="150"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBomType"
                                                runat="server" ControlToValidate="txtBomType" Display="None" ErrorMessage="Type is required"
                                                SetFocusOnError="True" ValidationGroup="Type"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender
                                                ID="vceBomType" runat="server" Enabled="True"
                                                PopupPosition="BottomLeft" TargetControlID="rfvBomType" />
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lbtnTypeSubmit" />
        </Triggers>
    </asp:UpdatePanel>
    <telerik:RadWindowManager ID="RadWindowManagerProjectTemp" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowForms" Skin="Material" VisibleTitlebar="true" Title="Form" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="600" Height="270">
                <ContentTemplate>
                    <div>
                        <div class="input-field col s12">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ForeColor="Red" ValidationGroup="fromTemp" ControlToValidate="txtEstimateName" runat="server" ErrorMessage="Enter Name!"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtEstimateName" ValidationGroup="fromTemp" runat="server" AutoCompleteType="None" MaxLength="50"></asp:TextBox>
                                <asp:Label runat="server" ID="lbltxtEstimateName" AssociatedControlID="txtEstimateName">Name</asp:Label>
                            </div>
                        </div>
                        <div class="input-field col s12">
                            <div class="row">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                <asp:LinkButton ID="lnkPostback" runat="server" CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                <asp:HiddenField ID="hdnEstimateFormId" runat="server" />
                            </div>
                        </div>
                        <div style="clear: both;"></div>
                        <div class="btnlinks">
                            <asp:LinkButton ID="lnkUploadDoc" runat="server" ValidationGroup="fromTemp" OnClick="lnkUploadDoc_Click">Save</asp:LinkButton>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <asp:HiddenField ID="hdnItemJSON" runat="server" />
    <asp:HiddenField runat="server" ID="hdnBOMPermission" Value="YYYY" />
    <asp:HiddenField runat="server" ID="hdnMilestonesPermission" Value="YYYY" />
    <asp:HiddenField ID="hdnMilestone" runat="server" />
    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('a[href^="#accrd"]').on('click', function (e) {
                e.preventDefault();
                $("a.anchorActive").removeClass("anchorActive");
                $(this).addClass("anchorActive");
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
            function collapseAll() {
                $(".accrd").removeClass(function () {
                    return "active";
                });
                $('html, body').stop().animate({
                    'scrollTop': 0
                }, 300, 'swing');
                $(".collapsible-accordion").collapsible({ accordion: true });
                $(".collapsible-accordion").collapsible({ accordion: false });
            }
            function expandAll() {
                $('html, body').stop().animate({
                    'scrollTop': 0
                }, 300, 'swing');
                $(".accrd").addClass("active");
                $(".collapsible-accordion").collapsible({ accordion: false });
            }
            $('.collapse-expand').on('click', function (e) {
                if (this.classList.contains("opened") === true) {
                    this.classList.remove("opened");
                    collapseAll();
                    $("#expcolp").attr("title", "Expand All");
                    $("#expcolp").addClass('mdi-navigation-unfold-more');
                    $("#expcolp").removeClass('mdi-navigation-unfold-less');
                }
                else {
                    this.classList.add("is-active");
                    this.classList.add("opened");
                    expandAll();
                    $("#expcolp").attr("title", "Collapse All");
                    $("#expcolp").addClass('mdi-navigation-unfold-less');
                    $("#expcolp").removeClass('mdi-navigation-unfold-more');
                }
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

            $("[id*=ddlBType]").change(function () {

                if (this instanceof HTMLSelectElement) {
                    var ddlItem = $(this);
                    var strBType = $(this).val();
                    var ddlBType_id = $(ddlItem).attr('id');
                    if (strBType == '') {
                        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
                        $(this).val('0');
                        modalPopupBehavior.show();
                    }
                }

            });

            // -----------txtCode-----------
            function objCode() {
                this.prefixText = null;
                this.DeptID = null;
            }


            $(function () {
                $("[id*=txtCode]").autocomplete({
                    source: function (request, response) {
                        var ddlJobType = document.getElementById("<%=ddlJobType.ClientID %>");
                        var selectedValue = ddlJobType.value;
                        var DeptID = 0;

                        if (selectedValue != 'Select Department') DeptID = parseInt(selectedValue);

                        var objCode1 = new objCode();
                        objCode1.prefixText = request.term;
                        objCode1.DeptID = DeptID;
                        query = request.term;
                        var str = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetJobCodeByDeptID",
                            data: JSON.stringify(objCode1),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load job code");
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
                    delay: 50
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
                        catch{ }


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


                //----------GroupName-----------

                $("[id*=txtGroup]").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();

                        dtaaa.prefixText = request.term;
                        query = request.term;
                        var str = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetGroup",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load Group");
                            }
                        });
                    },
                    select: function (event, ui) {

                        if (ui.item.value == 0) {
                        }
                        else {
                            var txtGroup = this.id;
                            var hdnGroupID = document.getElementById(txtGroup.replace('txtGroup', 'hdnGroupID'));
                            $(hdnGroupID).val(ui.item.value);
                            $(this).val(ui.item.label);
                        }
                        return false;
                    },
                    focus: function (event, ui) {
                        $(this).val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 50
                }).bind('click', function () { $(this).autocomplete("search"); })
                $.each($(".Groupsearchinput"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var ula = ul;
                        var itema = item;
                        var result_value = item.value;
                        var result_item = item.label;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
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

                $("[id*=txtUM]").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.con = "";
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
                                alert("Due to unexpected errors we were unable to load unit measure");
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
                    delay: 50
                }).bind('click', function () { $(this).autocomplete("search"); })
                $.each($(".searchinput"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var ula = ul;
                        var itema = item;
                        var result_value = item.value;
                        var result_item = item.label;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    };
                });

                $("[id*=txtUM]").change(function () {
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
                            alert("Due to unexpected errors we were unable to load UM");
                        }
                    });
                });

                $("[id*=txtSType]").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.con = "";
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
                                alert("Due to unexpected errors we were unable to load service type");
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
                    delay: 50
                }).bind('click', function () { $(this).autocomplete("search"); })
                $.each($(".searchinput"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var result_value = item.value;
                        var result_item = item.label;
                        //var result_desc = item.acct;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
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

                $("[id*=txtVendor]").autocomplete({
                    source: function (request, response) {
                        var txtVendor = this.id;
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.con = "";
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
                        var txtVendor = this.id;
                        var hdnVendorID = document.getElementById(txtVendor.replace('txtVendor', 'hdnVendorId'));
                        var str = ui.item.Name;
                        var strId = ui.item.ID;
                        if (str == "No Record Found!") {
                            $(txtVendor).val("");
                        }
                        else {
                            $(this).val(str);
                            $(hdnVendorID).val(strId);
                        }
                        return false;
                    },
                    focus: function (event, ui) {
                        var txtVendor = this.id;
                        $(txtVendor).val(ui.item.Name);
                        return false;
                    },
                    minLength: 0,
                    delay: 50
                }).bind('click', function () { $(this).autocomplete("search"); })
                $.each($(".vendor-search"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var ula = ul;
                        var itema = item;
                        var result_value = item.ID;
                        var result_item = item.Name;
                        //var result_desc = item.acct;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
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

                //-------Lab Item------

                $("[id*=txtLabItem]").autocomplete({
                    source: function (request, response) {
                        var txtLabItem = this.id;
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetLabor",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load Labor");
                            }
                        });
                    },
                    select: function (event, ui) {
                        var txtLabItem = this.id;
                        var hdntxtLabItem = document.getElementById(txtLabItem.replace('txtLabItem', 'hdntxtLabItem'));
                        var str = ui.item.LabDesc;
                        var strId = ui.item.LabItem;
                        if (str == "No Record Found!") {
                            $(txtLabItem).val("");
                        }
                        else {
                            $(this).val(str);
                            $(hdntxtLabItem).val(strId);
                        }
                        return false;
                    },
                    focus: function (event, ui) {
                        var txtLabItem = this.id;
                        $(txtLabItem).val(ui.item.LabDesc);
                        return false;
                    },
                    minLength: 0,
                    delay: 50
                }).bind('click', function () { $(this).autocomplete("search"); })
                $.each($(".labor-search"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {

                        var ula = ul;
                        var itema = item;

                        var result_value = item.LabItem;
                        var result_item = item.LabDesc;

                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
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

                //-------------                

                $("[id*=txtddlMatItem]").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;
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
                            }
                        });
                    },
                    select: function (event, ui) {
                        var txtddlMatItem = this.id;
                        var hdnddlMatItemId = document.getElementById(txtddlMatItem.replace('txtddlMatItem', 'hdnddlMatItemId'));
                        var txtScope = document.getElementById(txtddlMatItem.replace('txtddlMatItem', 'txtScope'));
                        var str = ui.item.MatDesc;
                        var strId = ui.item.MatItem;
                        if (str == "No Record Found!") {
                            $(txtddlMatItem).val("");
                        }
                        else {
                            $(this).val(str);
                            $(hdnddlMatItemId).val(strId);
                            $(txtScope).val(str);
                        }
                        return false;
                    },
                    focus: function (event, ui) {
                        var txtddlMatItem = this.id;
                        $(txtddlMatItem).val(ui.item.MatDesc);
                        return false;
                    },
                    minLength: 0,
                    delay: 50
                }).bind('click', function () { $(this).autocomplete("search"); })
                $.each($(".ddlMatItem-search"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var ula = ul;
                        var itema = item;
                        var result_value = item.MatItem;
                        var result_item = item.MatDesc;


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

                //----------------


                $('#<%= gvCustom.ClientID %>').sortable({
                    items: 'tr:not(tr:first-child)',
                    handle: ".handle",
                    cursor: 'move',
                    axis: 'y',
                    dropOnEmpty: false,
                    start: function (e, ui) {
                        ui.item.addClass("selected");
                    },
                    stop: function (e, ui) {
                        ui.item.removeClass("selected", 1000, "swing");
                        updategridindex();
                    },
                    receive: function (e, ui) {
                        $(this).find("tbody").append(ui.item);
                    }
                });

                //UpdatedivDisplayUserRole();
            });
            function updategridindex() {
                debugger
                var grid1 = 1;
                $('#<%= gvCustom.ClientID %> > tbody  > tr').not(':first').not(':last').each(function () {
                    var cat = $(this).find('input[id*="txtRowLine"]');
                    cat.val(grid1);
                    grid1 = grid1 + 1;
                });
            }
        }

    </script>
    <script type="text/javascript">
        function LoadBomItems() {
            if (document.getElementById('<%= hdnloadBom.ClientID%>').value == "0") {
                document.getElementById('<%= lnkBompostback.ClientID%>').click();
            }
        }

    </script>
</asp:Content>
