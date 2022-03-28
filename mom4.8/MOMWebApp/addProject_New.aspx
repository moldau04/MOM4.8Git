<%@ Page Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="addProject_New" Codebehind="addProject_New.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="uc_AccountSearch.ascx" TagName="uc_AccountSearch" TagPrefix="uc1" %>
<%@ Register Src="uc_Datepicker.ascx" TagName="uc_Datepicker" TagPrefix="ucd" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/lity/lity.js"></script>
    <link href="js/lity/lity.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/Timepicker/jquery.timepicker.js"></script>
    <link rel="stylesheet" href="Scripts/Timepicker/jquery.timepicker.css" />
    <script type="text/javascript" src="appearance/js/bootstrap-datepicker.min.js"></script>
    <link rel="stylesheet" href="appearance/css/bootstrap-datepicker3.min.css" />

    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
    <!-- dropify -->
    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>

    <style>
        /**/
        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

        .highlight {
            background-color: Yellow;
        }

        .textBoxCCS {
            background: none;
            border: none;
        }

        .WIPGrid {
            height: auto !important;
        }

            .WIPGrid .rgRow > td, .RadGrid_Bootstrap .rgAltRow > td, .RadGrid_Bootstrap .rgEditRow > td, .RadGrid_Bootstrap .rgFooter > td, .RadGrid_Bootstrap .rgFilterRow > td, .RadGrid_Bootstrap .rgHeader, .RadGrid_Bootstrap .rgResizeCol, .RadGrid_Bootstrap .rgGroupHeader td {
                padding-top: 3px !important;
                padding-bottom: 3px !important;
                padding-right: 5px !important;
                padding-left: 5px !important
            }

            .WIPGrid .rgHeader, .RadGrid_Bootstrap th.rgResizeCol, .RadGrid_Bootstrap .rgHeaderWrapper {
                /* background-color: whitesmoke;
            padding: 5px;*/
                font-size: small;
            }

        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
        }

        .EmailBody {
            margin-left: 95px;
        }
    </style>
    <script type="text/javascript"> 

        //////////////// Confirm Mail Send to worker ///////////////////
        function notyConfirm(ticid) {
            noty({
                dismissQueue: true,
                layout: 'topCenter',
                theme: 'noty_theme_default',
                animateOpen: { height: 'toggle' },
                animateClose: { height: 'toggle' },
                easing: 'swing',
                text: 'Do you want to send text message to worker?',
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
                        type: 'btn btn-primary', text: 'Ok', click: function ($noty) {
                            $noty.close();
                            window.open("mailticket.aspx?id=" + ticid + "&c=0", "_blank");
                        }
                    },
                    {
                        type: 'btn btn-danger', text: 'Cancel', click: function ($noty) {
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





        function CheckSingleCheckbox(ob) {
            var grid = ob.parentNode.parentNode.parentNode;
            var inputs = grid.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (ob.checked && inputs[i] != ob && inputs[i].checked && inputs[i].getAttribute('tabindex') == 19) {
                        inputs[i].checked = false;
                    }
                }
            }
        }

        function AddTicketClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddeTicket.ClientID%>').value;
            if (id == "Y") {
                $('#divAddTicket').slideToggle('fast');
                $('#liSaveticket').toggle();
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditTicketClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditeTicket.ClientID%>').value;
            var viewid = document.getElementById('<%= hdnviewTicket.ClientID%>').value;
            if (id == "Y" || viewid == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        ///-Contact permission

        function AddContactClick(hyperlink) {
            var IsAdd = document.getElementById('<%= hdnAddeContact.ClientID%>').value;
            if (IsAdd == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function EditContactClick(hyperlink) {
            var IsEdit = document.getElementById('<%= hdnEditeContact.ClientID%>').value;
            if (IsEdit == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }




        function AddPlannerClick(hyperlink) {

            $('#divAddPlanner').slideToggle('fast');

            return true;

        }
        ///-Document permission

        function AddDocumentClick(hyperlink) {
            var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
            if (IsAdd == "Y") {
                ConfirmProjectUpload(hyperlink.value);
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }
        }

        function DeleteDocumentClick(hyperlink) {
            var IsDelete = document.getElementById('<%= hdnDeleteDocument.ClientID%>').value;
            if (IsDelete == "Y") {
                return confirm('Are you sure you want to delete this Doc?');;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }


        function ViewDocumentClick(hyperlink) {
            var IsView = document.getElementById('<%= hdnViewDocument.ClientID%>').value;
            if (IsView == "Y") {
                window.postback = false; setTimeout(function () { window.postback = true; }, 100);
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }

        function ViewIMGClick(hyperlink) {
            var IsView = document.getElementById('<%= hdnViewDocument.ClientID%>').value;
            if (IsView == "Y") {
                window.postback = false; setTimeout(function () { window.postback = true; }, 100);
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        ////////////-----------------Permission
        function ReloadPage() {
            return false;
        }
        function cancel() {

            window.parent.document.getElementById('btnCancel').click();
        }
        $(document).ready(function () {


            $('input.timepicker').timepicker({
                dropdown: false
            });
            $('input.timepicker').on('click', function () {
                if ($('input.timepicker').val() == "") {
                    $('input.timepicker').timepicker('setTime', new Date());
                    $(this).select();
                }
                else { $(this).select(); }
            });
            $('input.timepicker').on('focus', function () {

                $(this).select();
            });
        });
        function HideShowOnPostingTypeChange(opt) {
            if (opt == 1 || opt == 2) {
                $('.hideShowOnPostingType').show();
            }
            else {
                $('.hideShowOnPostingType').hide();
               <%-- $('#<%= hdnUnrecognizedRevenue.ClientID %>').val('');
                $('#<%= hdnUnrecognizedExpense.ClientID %>').val('');
                $('#<%= hdnRetainageReceivable.ClientID %>').val('');
                $('#<%= txtUnrecognizedRevenue.ClientID %>').val('');
                $('#<%= txtUnrecognizedExpense.ClientID %>').val('');
                $('#<%= txtRetainageReceivable.ClientID %>').val('');--%>
            }
        }

        ////////////////// Confirm Document Upload ////////////////////
        function ConfirmProjectUpload(value) {
            var filename;
            var fullPath = value;
            if (fullPath) {
                var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
                filename = fullPath.substring(startIndex);
                if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
                    filename = filename.substring(1);
                }
            }

            if (confirm('Upload ' + filename + '?')) {
                document.getElementById('<%=lnkUploadProjectDoc.ClientID %>').click();
            }
            else {
                document.getElementById('<%=lnkProjectPostback.ClientID %>').click();
            }
        }
    </script>
    <script type="text/javascript">

        var changes = 0;
        $(document).on("change", ":input[type!='file']", function () {

            changes = 1;

        });

        window.onbeforeunload = function () {
            if (!window.btn_clicked) {
                if (window.postback != false) {
                    if (changes == 1) {
                        return 'Changes not saved.';
                    }
                }
            }
        };

        function divExpandCollapse(divname) {
            //debugger;
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);

            if (div.style.display == "none") {
                div.style.display = "inline";
                img.src = "images/icons/minus.gif";
            } else {
                div.style.display = "none";
                img.src = "images/icons/plus.gif";
            }
        }

        function WarningTemplate() {
            noty({
                text: 'The existing BOM and Milestone are in use. BOM and Milestone items can not be changed.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: false,
                theme: 'noty_theme_default',
                closable: true
            });
        }

        ///////////// Custom validator function for customer auto search  ////////////////////
        function ChkCustomer(sender, args) {
            var hdnCustID = document.getElementById('<%=hdnCustID.ClientID%>');
            if (hdnCustID.value == '') {
                args.IsValid = false;
            }
        }
        function isInt(value) {
            var x = parseFloat(value);
            return !isNaN(value) && (x | 0) === x;
        }
        ///////////// Custom validator function for location auto search  ////////////////////
        function ChkLocation(sender, args) {
            var hdnLocId = document.getElementById('<%=hdnLocID.ClientID%>');
            if (hdnLocId.value == '') {
                args.IsValid = false;
            }
        }
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
        function showDecimalVal(obj) {
            if (!isNaN(parseFloat(document.getElementById(obj.id).value.toString().replace(/[\$\(\),]/g, '')))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value.toString().replace(/[\$\(\),]/g, '')).toLocaleString("en-US", { minimumFractionDigits: 2 })
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
            //alert(txtQtyReq);
            //alert(txtBudgetUnit);
            //alert(valBudgetExt);
            if (!isNaN(valBudgetExt)) {
                valBudgetExt = valBudgetExt.toFixed(2);
                $(lblMatExt).text(parseFloat(valBudgetExt).toLocaleString("en-US", { minimumFractionDigits: 2 }))
                $(hdnMatExt).val(valBudgetExt.toString())
                if (isNaN(valLabExt) || valLabExt == "") {
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
            if (!isNaN(valLabExt)) {
                valLabExt = parseFloat(valLabExt)
                $(lblLabExt).text(valLabExt.toLocaleString("en-US", { minimumFractionDigits: 2 }))

                if (isNaN(valMatExt) || valMatExt == "") {
                    valMatExt = 0;
                }
                var total = parseFloat(valMatExt) + parseFloat(valLabExt);
                $(lblTotalExt).text(total.toLocaleString("en-US", { minimumFractionDigits: 2 }))
            }

        }

        //function fillDesc(obj) {
        //    var ddl = $(obj).attr('id');
        //    var val = $(obj).find("option:selected").text()
        //    document.getElementById(ddl.replace('ddlMatItem', 'txtScope')).value = val;
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
        function CalculateRevTotal() {
            //debugger;;
            var tRev = 0;
            $("#<%=gvMilestones.ClientID %>").find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);

                if ($tr.find('input[id*=txtAmount]').attr('id') != "" && typeof $tr.find('input[id*=txtAmount]').attr('id') != 'undefined') {
                    var amt = $tr.find('input[id*=txtAmount]').val().replace(/[\$\(\),]/g, '');

                    if (!isNaN(parseFloat(amt))) {
                        tRev += parseFloat(amt);
                    }
                }
            });
            $('.totalrev').html(cleanUpCurrency("$" + parseFloat(tRev).toLocaleString("en-US", { minimumFractionDigits: 2 })));
        }
        function DisableControls() {
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                $tr.find('input[class*=DisableControls]').attr("readonly", true);
                $tr.find('input[class*=DisableControls]').focus(function () {
                    var $inp = $('input:text');
                    var nxtIdx = $inp.index(this) + 1;
                    if (this.id.indexOf("txtScheduledValues") > -1)
                        nxtIdx += 1;
                    $(":input:text:eq(" + nxtIdx + ")").focus().select();
                });
            });
        }
        var tAmt = 0;
        function CalculateContractAmountTotal() {
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtContractAmount]');
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    var amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtContractAmount]').val(parseFloat(0).toFixed(2));
                }
            });
            $('.totalContractAmount').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
        }
        var _firstCall = 1;
        function CalculateChangeOrderTotal() {
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtChangeOrder]');
                var amt = 0;
                var amt2 = 0;
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtChangeOrder]').val(parseFloat(0).toFixed(2));
                    //debugger;;
                    if (setTimeout(function () { $tr.find('input').is(":focus") }, 100) || _firstCall == 1) {
                        // calculare Scheduled Values
                        var col2 = $tr.find('input[id*=txtContractAmount]');
                        if (col2.attr('id') != "" && typeof col2.attr('id') != 'undefined') {
                            amt2 = col2.val().replace(/[\$\(\),]/g, '');
                            //if (isNaN(parseFloat(amt2))) {
                            //    amt2 = 0;
                            //}
                        }
                        if (isNaN(parseFloat(amt))) amt = 0;
                        if (isNaN(parseFloat(amt2))) amt2 = 0;
                        $tr.find('input[id*=txtScheduledValues]').val(parseFloat((parseFloat(amt) + parseFloat(amt2))).toFixed(2));
                    }
                }
            });
            $('.totalChangeOrder').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            CalculateScheduledValuesTotal();
        }
        function CalculateScheduledValuesTotal() {
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtScheduledValues]');
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    var amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtScheduledValues]').val(parseFloat(0).toFixed(2));
                }
            });
            $('.totalScheduledValues').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            CalculateTotalCompletedAndStoredTotal();
        }
        function CalculatePreviousBilledTotal() {
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtPreviousBilled]');
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    var amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtPreviousBilled]').val(parseFloat(0).toFixed(2));
                    if (setTimeout(function () { $tr.find('input').is(":focus") }, 100) || _firstCall == 1) {
                        // calculare Total Completed and Stored Start
                        var amt2 = 0;
                        var col2 = $tr.find('input[id*=txtPresentlyStored]');
                        if (col2.attr('id') != "" && typeof col2.attr('id') != 'undefined') {
                            amt2 = col2.val().replace(/[\$\(\),]/g, '');
                        }
                        var amt3 = 0;
                        var col3 = $tr.find('input[id*=txtCompletedThisPeriod]');
                        if (col3.attr('id') != "" && typeof col3.attr('id') != 'undefined') {
                            amt3 = col3.val().replace(/[\$\(\),]/g, '');
                        }
                        var totalCompSto = 0;
                        if (!isNaN(parseFloat(amt))) { totalCompSto += parseFloat(amt); }
                        if (!isNaN(parseFloat(amt2))) { totalCompSto += parseFloat(amt2); }
                        if (!isNaN(parseFloat(amt3))) { totalCompSto += parseFloat(amt3); }
                        $tr.find('input[id*=txtTotalCompletedAndStored]').val(parseFloat(totalCompSto).toFixed(2));
                        // calculare Total Completed and Stored END
                    }
                }
            });
            $('.totalPreviousBilled').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            CalculateTotalCompletedAndStoredTotal();
        }
        function CalculatCompletedThisPeriodTotal() {
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtCompletedThisPeriod]');
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    var amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtCompletedThisPeriod]').val(parseFloat(0).toFixed(2));
                    if (setTimeout(function () { $tr.find('input').is(":focus") }, 100) || _firstCall == 1) {
                        // calculare Scheduled Values Start
                        var amt2 = 0;
                        var col2 = $tr.find('input[id*=txtPresentlyStored]');
                        if (col2.attr('id') != "" && typeof col2.attr('id') != 'undefined') {
                            amt2 = col2.val().replace(/[\$\(\),]/g, '');
                        }
                        var amt3 = 0;
                        var col3 = $tr.find('input[id*=txtPreviousBilled]');
                        if (col3.attr('id') != "" && typeof col3.attr('id') != 'undefined') {
                            amt3 = col3.val().replace(/[\$\(\),]/g, '');
                        }
                        var totalCompSto = 0;
                        if (!isNaN(parseFloat(amt))) { totalCompSto += parseFloat(amt); }
                        if (!isNaN(parseFloat(amt2))) { totalCompSto += parseFloat(amt2); }
                        if (!isNaN(parseFloat(amt3))) { totalCompSto += parseFloat(amt3); }
                        $tr.find('input[id*=txtTotalCompletedAndStored]').val(parseFloat(totalCompSto).toFixed(2));
                        // calculare Scheduled Values END
                    }
                }
            });
            $('.totalCompletedThisPeriod').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            CalculateTotalCompletedAndStoredTotal();
        }
        function CalculatPresentlyStoredTotal() {
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtPresentlyStored]');
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    var amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtPresentlyStored]').val(parseFloat(0).toFixed(2));
                    if (setTimeout(function () { $tr.find('input').is(":focus") }, 100) || _firstCall == 1) {
                        // calculare Scheduled Values Start
                        var amt2 = 0;
                        var col2 = $tr.find('input[id*=txtCompletedThisPeriod]');
                        if (col2.attr('id') != "" && typeof col2.attr('id') != 'undefined') {
                            amt2 = col2.val().replace(/[\$\(\),]/g, '');
                        }
                        var amt3 = 0;
                        var col3 = $tr.find('input[id*=txtPreviousBilled]');
                        if (col3.attr('id') != "" && typeof col3.attr('id') != 'undefined') {
                            amt3 = col3.val().replace(/[\$\(\),]/g, '');
                        }
                        var totalCompSto = 0;
                        if (!isNaN(parseFloat(amt))) { totalCompSto += parseFloat(amt); }
                        if (!isNaN(parseFloat(amt2))) { totalCompSto += parseFloat(amt2); }
                        if (!isNaN(parseFloat(amt3))) { totalCompSto += parseFloat(amt3); }
                        $tr.find('input[id*=txtTotalCompletedAndStored]').val(parseFloat(totalCompSto).toFixed(2));
                        // calculare Scheduled Values END
                    }
                }
            });
            $('.totalPresentlyStored').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            CalculateTotalCompletedAndStoredTotal();
        }
        function CalculateTotalCompletedAndStoredTotal() {
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtTotalCompletedAndStored]');
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    var amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtTotalCompletedAndStored]').val(parseFloat(0).toFixed(2));
                    if (setTimeout(function () { $tr.find('input').is(":focus") }, 100) || _firstCall == 1) {
                        // calculate Pre Complete total Start && 
                        var amt2 = 0;
                        var col2 = $tr.find('input[id*=txtScheduledValues]');
                        if (col2.attr('id') != "" && typeof col2.attr('id') != 'undefined') {
                            amt2 = col2.val().replace(/[\$\(\),]/g, '');
                        }
                        var totalPerComplete = 0;

                        if (!isNaN(parseFloat(amt)) && !isNaN(parseFloat(amt2))) {
                            if (parseFloat(amt2) > 0) {
                                totalPerComplete = parseFloat(amt) / parseFloat(amt2);
                            }
                        }
                        $tr.find('input[id*=txtPerComplete]').val(parseFloat(totalPerComplete * 100).toFixed(2));
                        // calculare Pre Complete total END

                        // Calculate Balance to Finish Start
                        var totalBalanceToFinsh = 0;
                        if (!isNaN(parseFloat(amt)) && !isNaN(parseFloat(amt2))) {
                            totalBalanceToFinsh = parseFloat(amt2) - parseFloat(amt);
                        }
                        $tr.find('input[id*=txtBalanceToFinsh]').val(parseFloat(totalBalanceToFinsh).toFixed(2));
                        // Calculate Balance to Finish End

                        // Calculate Total Billed Start
                        var amt3 = 0;
                        var col3 = $tr.find('input[id*=txtRetainageAmount]');
                        if (col3.attr('id') != "" && typeof col3.attr('id') != 'undefined') {
                            amt3 = col3.val().replace(/[\$\(\),]/g, '');
                        }
                        var totalBilled = 0;
                        if (!isNaN(parseFloat(amt)) && !isNaN(parseFloat(amt3))) {
                            totalBilled = parseFloat(amt) - parseFloat(amt3);
                        }
                        $tr.find('input[id*=txtTotalBilled]').val(parseFloat(totalBilled).toFixed(2));
                        // Calculate Balance to Finish End
                    }
                }
            });
            $('.totalTotalCompletedAndStored').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            CalculatePerCompleteTotal();
            CalculateBalanceToFinshTotal();
            CalculateTotalBilledTotal();
        }
        function CalculatePerCompleteTotal() {
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtPerComplete]');
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    var amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtPerComplete]').val(parseFloat(0).toFixed(2));
                }
            });
            $('.totalPerComplete').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
        }
        function CalculateBalanceToFinshTotal() {
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtBalanceToFinsh]');
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    var amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtBalanceToFinsh]').val(parseFloat(0).toFixed(2));
                }
            });
            $('.totalBalanceToFinsh').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
        }
        function CalculateRetainagePerTotal() {
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtRetainagePer]');
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    var amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtRetainagePer]').val(parseFloat(0).toFixed(2));
                    if (setTimeout(function () { $tr.find('input').is(":focus") }, 100) || _firstCall == 1) {
                        // Calculate Retainage Amount Start
                        var amt1 = 0;
                        var totalTotalBilled = 0;
                        var col1 = $tr.find('input[id*=txtTotalCompletedAndStored]');
                        if (col1.attr('id') != "" && typeof col1.attr('id') != 'undefined') {
                            amt1 = col1.val().replace(/[\$\(\),]/g, '');
                        }
                        if (!isNaN(parseFloat(amt)) && !isNaN(parseFloat(amt1))) {
                            $tr.find('input[id*=txtRetainageAmount]').val(parseFloat(parseFloat(amt1) * (parseFloat(amt) / 100.0)).toFixed(2));
                            // calculate Total Billed Start
                            var totalTotalBilled = parseFloat(parseFloat(amt1) - parseFloat(amt1) * (parseFloat(amt) / 100.0));
                            $tr.find('input[id*=txtTotalBilled]').val(parseFloat(totalTotalBilled).toFixed(2));
                            // calculare Total Billed END
                        }
                        // Calculate Retainage Amount End
                    }
                }
            });
            $('.totalRetainagePer').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
        }
        function CalculateRetainageAmountTotal() {
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtRetainageAmount]');
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    var amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtRetainageAmount]').val(parseFloat(0).toFixed(2));
                }
                if (setTimeout(function () { $tr.find('input').is(":focus") }, 100) || _firstCall == 1) {
                    // calculate Total Billed Start
                    var amt2 = 0;
                    var col2 = $tr.find('input[id*=txtTotalCompletedAndStored]');
                    if (col2.attr('id') != "" && typeof col2.attr('id') != 'undefined') {
                        amt2 = col2.val().replace(/[\$\(\),]/g, '');
                    }
                    var totalTotalBilled = 0;
                    if (!isNaN(parseFloat(amt)) && !isNaN(parseFloat(amt2))) {
                        totalTotalBilled = parseFloat(amt2) - parseFloat(amt);
                    }
                    $tr.find('input[id*=txtTotalBilled]').val(parseFloat(totalTotalBilled).toFixed(2));
                    // calculare Total Billed END
                }

            });
            $('.totalRetainageAmount').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
            CalculateTotalBilledTotal();
        }
        function CalculateTotalBilledTotal() {
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtTotalBilled]');
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    var amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtTotalBilled]').val(parseFloat(0).toFixed(2));
                    if (setTimeout(function () { $tr.find('input').is(":focus") }, 100) || _firstCall == 1) {
                        // calculate Total Billed Start
                        var amt2 = 0;
                        var col2 = $tr.find('input[id*=txtTotalCompletedAndStored]');
                        if (col2.attr('id') != "" && typeof col2.attr('id') != 'undefined') {
                            amt2 = col2.val().replace(/[\$\(\),]/g, '');
                        }
                        var totalRetainageAmount = 0;
                        if (!isNaN(parseFloat(amt)) && !isNaN(parseFloat(amt2))) {
                            totalRetainageAmount = parseFloat(amt2) - parseFloat(amt);
                        }
                        $tr.find('input[id*=txtRetainageAmount]').val(parseFloat(totalRetainageAmount).toFixed(2));
                        // calculare Total Billed END
                    }
                }
            });
            $('.totalTotalBilled').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));

            // Calculate Sum of Retaunage Amnount
            tAmt = 0;
            $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                var $tr = $(this);
                var col = $tr.find('input[id*=txtRetainageAmount]');
                if (col.attr('id') != "" && typeof col.attr('id') != 'undefined') {
                    var amt = col.val().replace(/[\$\(\),]/g, '');
                    if (!isNaN(parseFloat(amt))) {
                        tAmt += parseFloat(amt);
                    }
                    else
                        $tr.find('input[id*=txtRetainageAmount]').val(parseFloat(0).toFixed(2));
                }
            });
            $('.totalRetainageAmount').html(cleanUpCurrency("$" + parseFloat(tAmt).toLocaleString("en-US", { minimumFractionDigits: 2 })));
        }
        function TotalCal() {
            DisableControls();
            CalculateContractAmountTotal();
            CalculateChangeOrderTotal();
            CalculateScheduledValuesTotal();
            CalculatePreviousBilledTotal();
            CalculatPresentlyStoredTotal();
            CalculateTotalCompletedAndStoredTotal();
            CalculatePerCompleteTotal();
            CalculateBalanceToFinshTotal();
            CalculateRetainagePerTotal();
            CalculateRetainageAmountTotal();
            CalculateTotalBilledTotal();
            CalculatCompletedThisPeriodTotal();
        }
        function hideShowBillingDetailsTab(value) {
            if (value) {
                $('#ctl00_ContentPlaceHolder1_liBilling_WIP').show();
                //$('#tbpnlWIP').show();
                $('#ctl00_ContentPlaceHolder1_liBilling_ProgressBillings').show();
                //$('#tbpnlProgressBilling').show();
            }
            else {
                <%--$find('<%=TabContainerBillingDetails.ClientID %>')._tabs[1]._hide();
                $find('<%=TabContainerBillingDetails.ClientID %>')._tabs[2]._hide();--%>
                $('#ctl00_ContentPlaceHolder1_liBilling_WIP').hide();
                $('#tbpnlWIP').hide();
                $('#ctl00_ContentPlaceHolder1_liBilling_ProgressBillings').hide();
                $('#tbpnlProgressBilling').hide();
                $('#tbpnlDetails').show();
            }
        }
        function MoveTab(value) {
            <%--var ctrl = $find('<%=TabContainerBillingDetails.ClientID %>');
            ctrl.set_activeTab(ctrl.get_tabs()[1]);
            return true;--%>
        }
        function ConfirmStatusUpdate(opt) {
            if (confirm('Are you sure you want to update stauts ?')) {
                $(opt).attr('oldIndex', opt.selectedIndex);
            }
            else {
                opt.value = $(opt).attr('oldIndex');
                __doPostback('ddlApplicationStatus', '');
            }
        }
        function ConfirmDelete() {
            return confirm('Are you sure you want to delete record?');
        }
        $(window).load(function () {

            ///////////////////**Select Multiple Equpment In Ticket List tap**////////////////////

            $("#<%=txtUnit.ClientID%>").keyup(function (event) {
                var hdnUnitId = document.getElementById('<%=hdnUnitID.ClientID%>');
                if (document.getElementById('<%=txtUnit.ClientID%>').value == '') {
                    hdnUnitId.value = '';
                }
            });

         <%--   $("#<%=txtUnit.ClientID%>").click(function () {
                $("#divEquip").slideToggle();
                return false;
            });--%>

            $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").click(CheckUncheckAllCheckBoxAsNeeded);

            $("#<%=gvEquip.ClientID%> input[id*='chkAll']:checkbox").click(function () {
                if ($(this).is(':checked')) {
                    $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").attr('checked', true);
                }
                else {
                    $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").attr('checked', false);
                }
                SelectRows('<%=gvEquip.ClientID%>', '<%=txtUnit.ClientID%>', '<%=hdnUnitID.ClientID%>');

            });

            function CheckUncheckAllCheckBoxAsNeeded() {
                var totalCheckboxes = $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").size();
                var checkedCheckboxes = $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox:checked").size();

                if (totalCheckboxes == checkedCheckboxes) {
                    $("#<%=gvEquip.ClientID%> input[id*='chkAll']:checkbox").attr('checked', true);
                }
                else {
                    $("#<%=gvEquip.ClientID%> input[id*='chkAll']:checkbox").attr('checked', false);
                }
            }

            //**********************Select Multiple Equpment In Ticket List tap**************


            $("#<%= chkcatlist.ClientID %> tr").click(function () {
                var text = '';
                (($("#<%= chkcatlist.ClientID %> tr td")).find("input[type='checkbox']:checked")).each(function () {
                    text += jQuery('label[for=' + $(this).attr('id') + ']').html() + ",";
                });
                $("#<%= txtWorkers.ClientID %>").val(text);
            });

            $("img[id*='imgPlus']").click(function () {
                if ($(this).attr('src') == "images/plus.png") {
                    //$(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                    $("<tr style='display:none'><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>").insertAfter($(this).closest("tr")).show('slow');
                    $(this).attr("src", "images/minus.png");
                }
                else {
                    $(this).attr("src", "images/plus.png");
                    $(this).closest("tr").next().remove();
                }
            });

            //$("body").on("click", "[src*=plus]", function () {
            //    alert(this);

            //    $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
            //    //$("<tr style='display:none'><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>").insertAfter($(this).closest("tr")).show('slow');
            //    $(this).attr("src", "images/minus.png");
            //});
            //$("body").on("click", "[src*=minus]", function () {
            //    alert(this);
            //    $(this).attr("src", "images/plus.png");
            //    $(this).closest("tr").next().remove();
            //});

            $(function () {
                TotalCal();
                _firstCall = 0;
                CalculateRevTotal();
                hideShowBillingDetailsTab($('#<%= chkProgressBilling.ClientID %>').is(":checked"));
                HideShowOnPostingTypeChange($('#<%= ddlPostingMethod.ClientID %>').val())
                if ($("#<%= chkspnotes.ClientID %>").attr("checked") == "checked")
                    $(".inst").show();
                else {
                    $(".inst").hide();
                }

                $("#<%= chkspnotes.ClientID %>").click(function (event) {

                    if ($("#<%= chkspnotes.ClientID %>").attr("checked") == "checked")
                        $(".inst").show();
                    else {
                        $(".inst").hide();
                        $("#<%=txtSpecialInstructions.ClientID%>").val('');
                    }
                });

                if ($("#<%= chkRenew.ClientID %>").attr("checked") == "checked")
                    $(".inst1").show();
                else {
                    $(".inst1").hide();
                }

                $("#<%= chkRenew.ClientID %>").click(function (event) {

                    if ($("#<%= chkRenew.ClientID %>").attr("checked") == "checked")
                        $(".inst1").show();
                    else {
                        $(".inst1").hide();
                        $("#<%=txtRenew.ClientID%>").val('');
                    }
                });

                $('#<%=gvBudget.ClientID%>').show();
                $('#divBudgetPager').show();
                $('#<%=gvExpenses.ClientID%>').hide();
                $('#divExpensesPager').hide();

                $('#<%=chkExpense.ClientID%>').change(function () {
                    if ($(this).is(':checked')) {
                        $('#<%=gvBudget.ClientID%>').hide();
                        $('#divBudgetPager').hide();
                        $('#<%=gvExpenses.ClientID%>').show();
                        $('#divExpensesPager').show();
                    }
                    else {
                        $('#<%=gvBudget.ClientID%>').show();
                        $('#divBudgetPager').show();
                        $('#<%=gvExpenses.ClientID%>').hide();
                        $('#divExpensesPager').hide();
                    }
                });
                ///////////////////////////////////// Unrecognized Revenue ////////////////////////////////////
                $("#<%=txtUnrecognizedRevenue.ClientID%>").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetInvService_TypeZero",
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

                        $("#<%=gvWIPs.ClientID %>").find('tbody tr').each(function () {
                            var $tr = $(this);
                            $tr.find('select[id*=ddlBillingCode]').val(ui.item.value);
                        });

                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%=txtUnrecognizedRevenue.ClientID%>").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
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
                ///////////////////////////////////// Unrecognized Expense ////////////////////////////////////
                $("#<%=txtUnrecognizedExpense.ClientID%>").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetInvService_TypeZero",
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
                    delay: 250
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
                ///////////////////////////////////// Retainage Receivable ////////////////////////////////////
                $("#<%=txtRetainageReceivable.ClientID%>").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetInvService_TypeZero",
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
                    delay: 250
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
                $("[id*=txtCode]").autocomplete({

                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;

                        var str = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetJobCode",
                            data: JSON.stringify(dtaaa),
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
                            //redirect to another page 
                            //instead open popup to add code detail.
                            Showpopup();
                        }
                        if (ui.item.value == 0) {

                        }
                        else {

                            var txtCode = this.id;
                            var hdnCode = document.getElementById(txtCode.replace('txtCode', 'hdnCode'));
                            $(hdnCode).val(ui.item.value);
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
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        //debugger;
                        var ula = ul;
                        var itema = item;
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

                $("[id*=txtUM]").autocomplete({
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
                    delay: 250
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
                $("[id*=ddlBType]").change(function () {
                    var ddlBType = $(this);
                    var strBType = $(this).val();

                    var ddlBType_id = $(ddlBType).attr('id');
                    if (strBType == '0') {

                        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
                        $("#<%= txtBomType.ClientID %>").val('');
                        modalPopupBehavior.show();
                        $(this).val('Select Type');
                    }

                })
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


                var query = "";
                function u_dta() {
                    this.prefixText = null;
                    this.con = "";
                }
                $("[id*=txtUserID]").autocomplete({

                    source: function (request, response) {
                        var dta = new u_dta();
                        dta.prefixText = request.term;
                        query = request.term;

                        var str = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetUsername",
                            data: JSON.stringify(dta),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load username");
                            }
                        });
                    },
                    select: function (event, ui) {

                        var txtUserID = this.id;
                        var hdnUserID = document.getElementById(txtUserID.replace('txtUserID', 'hdnUserID'));
                        var txtFirstName = document.getElementById(txtUserID.replace('txtUserID', 'txtFirstName'));
                        var txtLastName = document.getElementById(txtUserID.replace('txtUserID', 'txtLastName'));
                        var txtEmail = document.getElementById(txtUserID.replace('txtUserID', 'txtEmail'));
                        var txtMobile = document.getElementById(txtUserID.replace('txtUserID', 'txtMobile'));
                        $(this.id).val(ui.item.label);
                        $(hdnUserID).val(ui.item.value);
                        $(txtFirstName).val(ui.item.fFirst);
                        $(txtLastName).val(ui.item.fLast);
                        $(txtEmail).val(ui.item.Email);
                        $(txtMobile).val(ui.item.Cellular);

                        return false;
                    },
                    focus: function (event, ui) {
                        $(this).val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).bind('click', function () { $(this).autocomplete("search"); })
                $.each($(".usearchinput"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        //debugger;
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
                    delay: 250
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




                ///////////// Ajax call for customer auto search ////////////////////                
                var query = "";
                function dta() {
                    this.prefixText = null;
                    this.con = "";
                }
                var queryloc = "";
                $('#<%= txtCustomer.ClientID %>').autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dta();
                        dtaaa.prefixText = request.term;
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "CustomerAuto.asmx/GetCustomerProspect",
                            //data: '{"prefixText":' + JSON.stringify(request.term) + ',"con":' + JSON.stringify(document.getElementById('ctl00_ContentPlaceHolder1_hdnCon').value) + '}',
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            //error: function(result) {
                            //    alert("Due to unexpected errors we were unable to load customers");
                            //}
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                var err = eval("(" + XMLHttpRequest.responseText + ")");
                                alert(err.Message);
                            }
                        });
                    },
                    select: function (event, ui) {
                        //debugger;
                        $("#ctl00_ContentPlaceHolder1_txtCustomer").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_hdnCustID").val(ui.item.value);
                        $("#ctl00_ContentPlaceHolder1_txtLocation").focus();
                        $("#ctl00_ContentPlaceHolder1_txtLocation").val('');
                        $("#ctl00_ContentPlaceHolder1_hdnLocID").val('');

                        if (ui.item.prospect == 1) {
                            document.getElementById('ctl00_ContentPlaceHolder1_btnSelectLoc').click();
                        }
                        else {
                            document.getElementById('ctl00_ContentPlaceHolder1_btnSelectCustomer').click();
                        }
                        return false;
                    },
                    focus: function (event, ui) {

                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
                    .data("ui-autocomplete")._renderItem = function (ul, item) {
                        var result_item = item.label;
                        var result_desc = item.desc;
                        var result_Prospect = item.prospect;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...   

                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });
                        }

                        var color = 'Black';
                        if (result_Prospect != 0) {
                            color = 'brown';
                        }
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a style='color:" + color + ";'>" + result_item + ", <span style='color:gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    };


                ////////////////////////////////////////////////////////////////////////////////////////////////


                $("#<%=txtName.ClientID%>").keyup(function (event) {

                    <%--   var hdnGCId = $("#<%=hdnGCID.ClientID%>");  
                        var hdnGCIDtemp = $("#<%=hdnGCIDtemp.ClientID%>");
                        var hdnGCName = $("#<%=hdnGCName.ClientID%>");

                    if (this.value.trim() != hdnGCName.val().trim())
                        hdnGCId.val('');
                    else
                        hdnGCId.val(hdnGCIDtemp.val().trim());--%>

                    var hdnGCName = $("#<%=hdnGCName.ClientID%>");
                    var hdnGContractorID = $("#<%=hdnGContractorID.ClientID%>");
                    var hdnGCNameupdate = $("#<%=hdnGCNameupdate.ClientID%>");

                    if (hdnGContractorID.val() > 0) {
                        if (this.value.trim() != hdnGCName.val().trim() && this.value.trim() != '')
                            hdnGCNameupdate.val('1');
                        hdnGContractorID.val(0);
                    }

                });

                ///////////// Ajax call for GC auto search ////////////////////                
                var query = "";
                function dta() {
                    this.prefixText = null;
                    this.con = "";
                    this.type = "";
                }
                $("#<%= txtName.ClientID%>").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dta();
                        dtaaa.prefixText = request.term;
                        dtaaa.type = "1";
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "CustomerAuto.asmx/GetGCorHomeOwner",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            //error: function(result) {
                            //    alert("Due to unexpected errors we were unable to load customers");
                            //}
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                var err = eval("(" + XMLHttpRequest.responseText + ")");
                                alert(err.Message);
                            }
                        });
                    },
                    select: function (event, ui) {
                        $("#<%= txtName.ClientID%>").val('');
                        $("#<%= txtCity.ClientID%>").val('');
                        $("#<%= GctxtAddress.ClientID%>").val('');
                        $("#<%= ddlState.ClientID%>").val('');
                        $("#<%= txtPostalCode.ClientID%>").val('');
                        $("#<%= txtCountry.ClientID%>").val('');
                        $("#<%= txtContactName.ClientID%>").val('');
                        $("#<%= txtPhone.ClientID%>").val('');
                        $("#<%= txtFax.ClientID%>").val('');
                        $("#<%= txtEmailWeb.ClientID%>").val('');
                        $("#<%= txtMobile.ClientID%>").val('');
                        $("#<%= txtRemarks.ClientID%>").val('');
                        $("#<%= hdnGCID.ClientID%>").val('');
                        $("#<%= hdnGCIDtemp.ClientID%>").val('');
                        $("#<%= hdnGCName.ClientID%>").val('');
                        $("#<%= hdnGContractorID.ClientID%>").val('0');
                        $("#<%= hdnGCNameupdate.ClientID%>").val(0);

                        $("#<%= txtName.ClientID%>").val(ui.item.label);
                        $("#<%= txtCity.ClientID%>").val(ui.item.city);
                        $("#<%= GctxtAddress.ClientID%>").val(ui.item.Address);
                        $("#<%= ddlState.ClientID%>").val(ui.item.state);
                        $("#<%= txtPostalCode.ClientID%>").val(ui.item.zip);
                        $("#<%= txtCountry.ClientID%>").val(ui.item.country);
                        $("#<%= txtContactName.ClientID%>").val(ui.item.contact);
                        $("#<%= txtPhone.ClientID%>").val(ui.item.phone);
                        $("#<%= txtFax.ClientID%>").val(ui.item.fax);
                        $("#<%= txtEmailWeb.ClientID%>").val(ui.item.email);
                        $("#<%= txtMobile.ClientID%>").val(ui.item.cellular);
                        $("#<%= txtRemarks.ClientID%>").val(ui.item.remarks);
                        $("#<%= hdnGCID.ClientID%>").val(ui.item.rolid);
                        $("#<%= hdnGCIDtemp.ClientID%>").val(ui.item.rolid);
                        $("#<%= hdnGContractorID.ClientID%>").val(ui.item.GC_HOid);
                        $("#<%= hdnGCNameupdate.ClientID%>").val(0);
                        $("#<%= hdnGCName.ClientID%>").val(ui.item.label);

                        return false;
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.label;
                    var result_desc = item.desc;
                    var result_Prospect = item.prospect;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...   

                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    var color = 'Black';
                    if (result_Prospect != 0) {
                        color = 'brown';
                    }
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a style='color:" + color + ";'>" + result_item + ", <span style='color:gray;'>" + result_desc + "</span></a>")
                        .appendTo(ul);
                };


                ///////////////////////////////////////

                /////Homeowner////


                $("#<%=hotxtname.ClientID%>").keyup(function (event) {
                    var hdnHOName = $("#<%=hdnHOName.ClientID%>");
                    var hdnHOId = $("#<%=hdnHomeOwnerID.ClientID%>");
                    var hdnHONameupdate = $("#<%=hdnHONameupdate.ClientID%>");

                    if (hdnHOId.val() > 0) {
                        if (this.value.trim() != hdnHOName.val().trim() && this.value.trim() != '')
                            hdnHONameupdate.val('1');
                        hdnHOId.val('0');
                    }
                });




                ///////////// Ajax Call for Homeowner Auto Search ////////////////////                
                var query = "";
                function dta() {
                    this.prefixText = null;
                    this.con = "";
                    this.type = "";
                }

                if ($("#<%= hotxtname.ClientID%>").value == "true") {
                    $("#<%= hotxtname.ClientID%>").autocomplete({
                        source: function (request, response) {
                            var dtaaa = new dta();
                            dtaaa.prefixText = request.term;
                            dtaaa.type = "2";
                            query = request.term;
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "CustomerAuto.asmx/GetGCorHomeOwner",
                                data: JSON.stringify(dtaaa),
                                dataType: "json",
                                async: true,
                                success: function (data) {
                                    response($.parseJSON(data.d));
                                },

                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    var err = eval("(" + XMLHttpRequest.responseText + ")");
                                    alert(err.Message);
                                }
                            });
                        },

                        select: function (event, ui) {
                            $("#<%= hotxtname.ClientID%>").val('');
                            $("#<%= hotxtcity.ClientID%>").val('');
                            $("#<%= HotxtAddress.ClientID%>").val('');
                            $("#<%= hotddlstate.ClientID%>").val('');
                            $("#<%= hotxtZIP.ClientID%>").val('');
                            $("#<%= hotxtCountry.ClientID%>").val('');
                            $("#<%= hotxtContactName.ClientID%>").val('');
                            $("#<%= hotxtPhone.ClientID%>").val('');
                            $("#<%= HotxtFax.ClientID%>").val('');
                            $("#<%= HotxtEmailWeb.ClientID%>").val('');
                            $("#<%= hotxtMobile.ClientID%>").val('');
                            $("#<%= hotxtRemarks.ClientID%>").val('');
                            $("#<%= hdnHomeOwnerID.ClientID%>").val('0');
                            $("#<%= hdnHOName.ClientID%>").val('');
                            $("#<%= hdnHONameupdate.ClientID%>").val(0);

                            $("#<%= hotxtname.ClientID%>").val(ui.item.label);
                            $("#<%= hotxtcity.ClientID%>").val(ui.item.city);
                            $("#<%= HotxtAddress.ClientID%>").val(ui.item.Address);
                            $("#<%= hotddlstate.ClientID%>").val(ui.item.state);
                            $("#<%= hotxtZIP.ClientID%>").val(ui.item.zip);
                            $("#<%= hotxtCountry.ClientID%>").val(ui.item.country);
                            $("#<%= hotxtContactName.ClientID%>").val(ui.item.contact);
                            $("#<%= hotxtPhone.ClientID%>").val(ui.item.phone);
                            $("#<%= HotxtFax.ClientID%>").val(ui.item.fax);
                            $("#<%= HotxtEmailWeb.ClientID%>").val(ui.item.email);
                            $("#<%= hotxtMobile.ClientID%>").val(ui.item.cellular);
                            $("#<%= hotxtRemarks.ClientID%>").val(ui.item.remarks);
                            $("#<%= hdnHomeOwnerID.ClientID%>").val(ui.item.GC_HOid);
                            $("#<%= hdnHOName.ClientID%>").val(ui.item.label);


                            return false;
                        },
                        focus: function (event, ui) {
                            return false;
                        },
                        minLength: 0,
                        delay: 250
                    }).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var result_item = item.label;
                        var result_desc = item.desc;
                        var result_Prospect = item.prospect;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...   

                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });
                        }
                        var color = 'Black';
                        if (result_Prospect != 0) {
                            color = 'brown';
                        }
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a style='color:" + color + ";'>" + result_item + ", <span style='color:gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    };
                }

                ///////////



                function l_dta() {
                    this.prefixText = "";
                    this.con = "";
                    this.custID = null;
                }
                $("#ctl00_ContentPlaceHolder1_txtLocation").autocomplete({

                    source: function (request, response) {
                        var dta = new l_dta();
                        dta.custID = 0;

                        var custID = document.getElementById('ctl00_ContentPlaceHolder1_hdnCustID').value;
                        if (!isNaN(parseInt(custID))) {
                            dta.custID = parseInt(custID);
                        }
                        dta.prefixText = request.term;
                        query = request.term;

                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "CustomerAuto.asmx/GetLocation",
                            data: JSON.stringify(dta),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load locations");
                            }
                        });
                    },
                    select: function (event, ui) {
                        $("#ctl00_ContentPlaceHolder1_txtLocation").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_hdnLocID").val(ui.item.value);
                        <%-- $("#<%=hdnCustID.ClientID%>").val('');--%>
                        document.getElementById('ctl00_ContentPlaceHolder1_btnSelectLoc').click();

                        return false;
                    },
                    focus: function (event, ui) {
                        $("#ctl00_ContentPlaceHolder1_txtLocation").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).bind('click', function () { $(this).autocomplete("search"); })
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
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    };
                $("#ctl00_ContentPlaceHolder1_txtCustomer").keyup(function (event) {
                    var hdnCustID = document.getElementById('ctl00_ContentPlaceHolder1_hdnCustID');
                    if (document.getElementById('ctl00_ContentPlaceHolder1_txtCustomer').value == '') {
                        hdnCustID.value = '';
                        $('#<%= lnkCustomerID.ClientID %>').removeAttr("href");
                    }
                });

                $("#ctl00_ContentPlaceHolder1_txtLocation").keyup(function (event) {
                    var hdnLocId = document.getElementById('ctl00_ContentPlaceHolder1_hdnLocID');
                    if (document.getElementById('ctl00_ContentPlaceHolder1_txtLocation').value == '') {
                        hdnLocId.value = '';
                        $('#<%= lnkLocationID.ClientID %>').removeAttr("href");
                        $('#<%=ddlTerr.ClientID %>').val('');
                        $('#<%=ddlTerr2.ClientID %>').val('');
                    }
                });
                var query = "";
                function dtaa() {
                    this.prefixText = null;
                    this.con = null;
                    this.custID = null;
                }

                $("#<%=txtPrevilWage.ClientID%>").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
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
                    delay: 250
                }).bind('click', function () { $(this).autocomplete("search"); })
                    .data("ui-autocomplete")._renderItem = function (ul, item) {
                        //debugger;
                        var ula = ul;
                        var itema = item;
                        var result_value = item.value;
                        var result_item = item.label;
                        //var result_desc = item.acct;
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
                    delay: 250
                }).bind('click', function () { $(this).autocomplete("search"); })
                    .data("ui-autocomplete")._renderItem = function (ul, item) {
                        //debugger;
                        var ula = ul;
                        //var itema = item;
                        var result_value = item.value;
                        var result_item = item.label;
                        //var result_desc = item.acct;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });

                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    };
                $(".custom").change(function () {
                    getCustomVal();
                });
                $(".currency").change(function () {
                    showDecimalVal(this);
                })
                $(".currency").keypress(function (e) {
                    return isDecimalKey(this, e.target)
                })
                //$(".date-picker").click(function () {
                //    $(this).datepicker();
                //})

                //var datepicker = $.fn.datepicker.noConflict(); // return $.fn.datepicker to previously assigned value
                //$.fn.bootstrapDP = datepicker;
                $(".date-picker").datepicker({
                    autoclose: true,
                    todayHighlight: true
                });

                $("[id*=txtVendor]").autocomplete({

                    source: function (request, response) {
                        var txtVendor = this.id;
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetVendorNameProject",
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
                    delay: 250
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
                        //if (result_desc != null) {
                        //    result_desc = result_desc.replace(x, function (FullMatch, n) {
                        //        return '<span class="highlight">' + FullMatch + '</span>'
                        //    });
                        //}

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


                //*********** Start BOM Grid Material Item Auto populate ************//
                $("[id*=txtMatItem]").autocomplete({

                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;

                        var txtMatItem = $(this.element).prop("id");
                        var ddlBType = document.getElementById(txtMatItem.replace('txtMatItem', 'ddlBType'));
                        var bTypeID = $(ddlBType).val();
                        dtaaa.bTypeID = bTypeID;


                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetProjectMaterialType",
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
                        var txtMatItem = this.id;

                        var hdnMatItem = document.getElementById(txtMatItem.replace('txtMatItem', 'hdnMatItem'));
                        var txtScope = document.getElementById(txtMatItem.replace('txtMatItem', 'txtScope'));

                        var str = ui.item.MatDesc;
                        var strId = ui.item.MatItem;
                        var strDesc = ui.item.fDesc;

                        if (str == "No Record Found!") {
                            $("#" + txtMatItem).val("");
                            $(txtScope).val("");
                            $(hdnMatItem).val("0");
                        }
                        else {
                            $("#" + txtMatItem).val(str);
                            $(txtScope).val(strDesc);
                            $(hdnMatItem).val(strId);
                        }

                        return false;
                    },
                    focus: function (event, ui) {
                        var txtMatItem = this.id;

                        var txtScope = document.getElementById(txtMatItem.replace('txtMatItem', 'txtScope'));

                        $("#" + txtMatItem).val(ui.item.MatDesc);
                        $(txtScope).val(ui.item.fDesc);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).bind('click', function () { $(this).autocomplete("search"); })
                $.each($(".MatItem-Search"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var ula = ul;
                        var itema = item;
                        var result_value = item.MatItem;
                        var result_item = item.MatDesc;
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
                //*********** End BOM Grid Material Item Auto populate ************//

            })



            ///////////// Ajax call for txtContcName auto search //////////////////// 


            var struid = $("#<%=hdnprojectID.ClientID%>").val();
            function dtaaon() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
                this.LocID = null;
                this.JobId = null;
            }
            $("[id*=txtContcName]").autocomplete({

                source: function (request, response) {
                    var dtaaa = new dtaaon();
                    dtaaa.prefixText = request.term;
                    dtaaa.custID = 0;
                    dtaaa.LocID = 0;
                    dtaaa.JobId = 0;
                    if (struid != '') {
                        dtaaa.custID = 0;
                        dtaaa.LocID = 0;
                        dtaaa.JobId = struid;
                    }
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetContactAutojquery",
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
                    $("#<%=txtContcName.ClientID%>").val(ui.item.fDesc);
                    $("#<%=txtTitle.ClientID%>").val(ui.item.Title);
                    $("#<%=txtContPhone.ClientID%>").val(ui.item.Phone);
                    $("#<%=txtContFax.ClientID%>").val(ui.item.Fax);
                    $("#<%=txtContCell.ClientID%>").val(ui.item.Cell);
                    $("#<%=txtContEmail.ClientID%>").val(ui.item.Email);
                    return false;
                },
                focus: function (event, ui) {
                    return false;
                },
                minLength: 0,
                delay: 250
            }).bind('click', function () { $(this).autocomplete("search"); })
            $.each($(".Contact-search"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {

                    var result_item = item.fDesc;
                    var result_desc = item.Title + ',' + item.Phone;

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

                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                        .appendTo(ul);

                };
            });



            //////////////////////////////////

            ////////End
        });

        ////////////////// Confirm Document Upload ////////////////////
        function ConfirmUpload(value) {
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

        }

        $(document).ready(function () {
            InitializeGrids('<%=gvBOM.ClientID%>');
        });
        //function CalculatePercentage(gridview) {
        //}
        function isNumberKey(evt, txt) {

            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
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
                $(this).find('tbody tr').each(function () {
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
        (function ($) {
            $.fn.serializeCustomJSON = function () {
                var o = [];
                $(".custom").each(function () {
                    if ($(this).length) {
                        var serialized = $(this).serialize();

                        if (serialized == '') {

                            var serialized = $(this).find('input, textarea, select').serialize();

                        }
                        var item = $.toDictionary(serialized);
                        o.push(item);
                    }
                    //}
                });
                return o;
            };
        })(jQuery);
        function InitializeGrids(Gridview) {
            var custom = $(".custom");
            $("input", custom).each(function () {
                $(this).blur();
            });

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

        function DelRow(Gridview) {
            if ($("#" + Gridview).find('input[type="checkbox"]:checked').length == 0) {
                alert('Please select items to delete.');
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
        function NumericValid(e) {

            //         $("#txtboxToFilter").keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A
                (e.keyCode == 65 && e.ctrlKey === true) ||
                // Allow: home, end, left, right
                (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
            //            });
        }

        function itemJSON() {

            var rawData = $('#<%=gvBOM.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            $('#<%=hdnItemJSON.ClientID%>').val(formData);

            var rawDataTeam = $('#<%=gvTeamItems.ClientID%>').serializeFormJSON();
            var formDataTeam = JSON.stringify(rawDataTeam);
            $('#<%=hdnItemTeamJSON.ClientID%>').val(formDataTeam);

            var rawMileData = $('#<%=gvMilestones.ClientID%>').serializeFormJSON();
            var formMileData = JSON.stringify(rawMileData);
            $('#<%=hdnMilestone.ClientID%>').val(formMileData);

            var rawCustomData = $(".custom").serializeFormJSON();
            var formCustomData = JSON.stringify(rawCustomData);
            $('#<%=hdnCustomJSON.ClientID%>').val(formCustomData);

        }
        function getCustomVal() {
            var rawCustomData = $(".custom").serializeCustomJSON();
            var formCustomData = JSON.stringify(rawCustomData);
            $('#<%=hdnCustomJSON.ClientID%>').val(formCustomData);
        }

        function Calculate(Gridview) {

            var tquan = 0;
            var tunit = 0;
            var ttotal = 0;

            $("#" + Gridview).find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);
                var quan = $tr.find('input[id*=txtActual]').val();
                var unit = $tr.find('input[id*=txtBudget]').val();
                var total = 0;

                if (!isNaN(parseFloat(quan))) {
                    tquan += parseFloat(quan);
                }
                if (!isNaN(parseFloat(unit))) {
                    tunit += parseFloat(unit);
                }
                if (!isNaN(parseFloat(quan))) {
                    if (!isNaN(parseFloat(unit))) {
                        total = ((parseFloat(unit) - parseFloat(quan)) / parseFloat(quan)) * 100;
                        ttotal += parseFloat(total);
                    }
                }
                $tr.find('input[id*=txtPercent]').val(total.toFixed(2));
            });

            var $footer = $("#" + Gridview).find('tr').eq($("#" + Gridview).find('tr').length - 1)
            $footer.find('input[id*=txtTPercent]').val(ttotal.toFixed(2));
            $footer.find('input[id*=txtTActual]').val(tquan.toFixed(2));
            $footer.find('input[id*=txtTBudget]').val(tunit.toFixed(2));
        }

        function CheckAdd(Gridview) {
            var gv;
            if (Gridview.includes('gvBOM')) {
                gv = $("#<%= gvBOM.ClientID%>")
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
            else {
                gv = $("#<%= gvMilestones.ClientID%>")
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
        }

        function CheckDelete(Gridview) {
            var isTrue = true;
            var gv;
            if (Gridview.includes('gvBOM')) {
                gv = $("#<%= gvBOM.ClientID%>")
                var BOMper = document.getElementById('<%= hdnBOMPermission.ClientID%>').value;
                if (BOMper.length >= 3) {
                    var BOMvalues = BOMper.substr(2, 1);
                    var BOMEditvalues = BOMper.substr(1, 1);
                    if (BOMvalues == "N" || BOMEditvalues == "N") {
                        noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        isTrue = false;
                        return false;
                    }
                }
            }
            else {
                gv = $("#<%= gvMilestones.ClientID%>")
                var Milesper = document.getElementById('<%= hdnMilestonesPermission.ClientID%>').value;
                if (Milesper.length >= 3) {
                    var Milesvalues = Milesper.substr(2, 1);
                    var mileEditvalues = Milesper.substr(1, 1);
                    if (Milesvalues == "N" || mileEditvalues == "N") {
                        noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                        isTrue = false;
                        return false;
                    }
                }
            }
            if (isTrue) {
                var len = gv.find('tr').find('input[type="checkbox"]:checked').length;
                if (len > 1) {
                    noty({
                        text: 'Please select any one items to delete.',
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
                if (gv.find('input[type="checkbox"]:checked').length == 0) {

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
                    itemJSON();
                    return confirm('Do you really want to delete this job item ?');
                }
            }
        }
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
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }
        function ConvertDigit(obj) {

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                //document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }

        function ConfirmGCAdd() {

            var validation = ValidBomMileStone();
            if (validation == false) {
                return false;
            }


            var ret = true;

            <%-- var GCid = $('#<%= hdnGCID.ClientID%>');
            var gcName = $('#<%= txtName.ClientID%>');
            if (GCid.val().trim() == '' && gcName.val().trim() != '') {
                ret = confirm(gcName.val() + ' will be created as a new General Contractor. Do you want to continue?');
            }--%>

            var hdnGCNameupdate = $('#<%= hdnGCNameupdate.ClientID%>');
            var txtName = $('#<%= txtName.ClientID%>');
            if (hdnGCNameupdate.val().trim() == '1') {
                ret = confirm(txtName.val() + ' will be created as a new General Contractor. Do you want to continue?');
            }

            if (ret) {
                //Confirm for Home owner
                var hdnHONameupdate = $('#<%= hdnHONameupdate.ClientID%>');
                var hotxtname = $('#<%= hotxtname.ClientID%>');
                if (hdnHONameupdate.val().trim() == '1') {
                    ret = confirm(hotxtname.val() + ' will be created as a new Homeowner. Do you want to continue?');
                }
            }

            return ret;
        }



        function CheckHold(clickedid) {

            var msg = "";
            if (clickedid.firstChild.checked == false)
                msg = "Are you sure you want to change ticket status?";
            else
                msg = "Are you sure you want to keep the ticket on hold?";

            var box = confirm(msg);
            if (box == true) {
                return true;
            }
            else {
                if (clickedid.firstChild.checked == true)
                    clickedid.firstChild.checked = false;
                else
                    clickedid.firstChild.checked = true;

                return false;
            }
        }

        function WorkersMenu(box) {
            box.blur();
            $("#divCat").css("width", $(box).outerWidth() + "px");
            $("#divCat").toggle();
            $("#divCatSearch").css("width", $(box).outerWidth() + "px");
            $("#divCatSearch").toggle();
            return false;
        }

        //function HoverMenutext(row, tooltip, event) {
        //    var left = event.pageX + (-110) + 'px';
        //    var top = event.pageY + (-510) + 'px';
        //    $('#' + tooltip).css({ top: top, left: left }).show();
        //}

        function SearchWorker(search) {
            $('#<%= chkcatlist.ClientID%> tr').each(function () {
                $(this).removeClass('HighliteBlue');
            });
            if ($(search).val().trim().length > 0) {
                $('#<%= chkcatlist.ClientID%> tr').each(function () {
                    var str = $(this).find('td').eq(0).text().toLocaleLowerCase();
                    var st = $(search).val().trim().toLocaleLowerCase();
                    //if (str.includes(st)) {
                    if (str.substring(0, st.length).indexOf(st) > -1) {
                        $(this).addClass('HighliteBlue');
                        var scrollTo = $(this);
                        var container = $('#divCat');
                        container.animate({
                            scrollTop: scrollTo.offset().top - container.offset().top + container.scrollTop()
                        }, 100);
                        return false;
                    }
                });
            }
            else {
                var container = $('#divCat');
                container.animate({
                    scrollTop: container.scrollTop(0)
                }, 100);
            }
        }

        function SelectCodeByCategory(categorylist) {
            var category = $(categorylist).val();
            $('#tblCodesList > tbody  > tr').not(':first').each(function () {
                var cat = $(this).find('input[type="hidden"]').val();
                if (cat.toLocaleLowerCase() != category.toLocaleLowerCase() && category != 'ALL')
                    $(this).hide();
                else
                    $(this).show();
            });
        }
        function checkedHidden(checkbox) {
            $(checkbox).closest('tr').find($("input[id*='hdnChecked']")).val("1");
        }

        function DeleteAlert() {
            var gvOpenCalls = document.getElementById('<%= gvTickets.ClientID %>');
            var result = false;
            $('input:checkbox[id$=chkSelect]:checked', gvOpenCalls).each(function (index, item) {
                var id = $(this).parent().parent().find('span[id$=lblTicketId]');
                result = confirm('Are you sure you want to delete ticket # ' + id.text() + ' ?');
            });
            return result;
        }




    </script>
    <style>
        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
        }

        .chklist label {
            margin-left: 10px !important;
        }

        .chklist input {
            height: 12px !important;
        }

        .HighliteBlue {
            background-color: #316b9d !important;
        }

        .shadow {
            /* rgba(0, 0, 0, 0.3) rgb(90, 168, 208)*/
            -moz-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            -webkit-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
        }

        .shadowHover:hover {
            -moz-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            -webkit-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
        }

        .hoverGrid {
            display: none;
            position: absolute;
            min-width: 300px;
            max-width: 800px;
            min-height: 20px;
            /*font-weight: bold;*/
            font-size: 14px;
            padding: 5px 5px 5px 5px;
            background: black;
            color: #FFF;
        }

        .transparent {
            zoom: 1;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .roundCorner {
            border: 1px solid #ccc;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
        }

        #scrollbox3 {
            overflow: auto;
            width: 400px;
            height: 360px;
            padding: 0 5px;
            border: 1px solid #b7b7b7;
        }

        .track3 {
            width: 10px;
            background: rgba(0, 0, 0, 0);
            margin-right: 2px;
            border-radius: 10px;
            -webkit-transition: background 250ms linear;
            transition: background 250ms linear;
        }

            .track3:hover,
            .track3.dragging {
                background: #d9d9d9; /* Browsers without rgba support */
                background: rgba(0, 0, 0, 0.15);
            }

        .handle3 {
            width: 7px;
            right: 0;
            background: #999;
            background: rgba(0, 0, 0, 0.4);
            border-radius: 7px;
            -webkit-transition: width 250ms;
            transition: width 250ms;
        }

        .track3:hover .handle3,
        .track3.dragging .handle3 {
            width: 10px;
        }

        .thumbnail {
            margin-bottom: 0;
            padding: 0;
        }

        .thumb {
            margin: 0;
            padding: 5px 5px 0;
        }

        .navbar {
            margin-bottom: 5px;
            border-radius: 0;
        }

        .navbar-inverse {
            /*background-color: #414040;*/
            background-color: #316b9d;
            border: none !important;
        }


        .pager {
            margin: 5px 0;
        }

            .pager a {
                background-color: #fff;
                border: 1px solid #ddd;
                border-radius: 15px;
                display: inline-block;
                padding: 5px 14px;
            }

            .pager span {
                padding: 5px;
            }

            .pager .title {
                text-align: center;
                font-weight: bold;
                color: #fff;
                line-height: 32px;
            }

        .gallery span {
            display: inline-block;
            text-align: center;
            width: 100%;
            word-wrap: break-word;
        }

        .img-responsive {
            max-height: 100px;
            /*width: 50px;*/
            border-width: 0px;
            margin: 0 auto;
            text-align: left;
        }

        .pull-leftslider {
            float: left !important;
            /*width: 50%;*/
            cursor: pointer;
        }

        .dropdown-menu table {
            height: 50px;
            width: 50px;
        }
    </style>

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

            var picker = new Pikaday(
                {
                    field: document.getElementById('datepicker'),
                    firstDay: 1,
                    format: 'MM/DD/YYYY',
                    minDate: new Date(2000, 1, 1),
                    maxDate: new Date(2020, 12, 31),
                    yearRange: [2000, 2020]
                });

            var picker = new Pikaday(
                {
                    field: document.getElementById('datepicker2'),
                    firstDay: 1,
                    format: 'MM/DD/YYYY',
                    minDate: new Date(2000, 1, 1),
                    maxDate: new Date(2020, 12, 31),
                    yearRange: [2000, 2020]
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
    <script>
        $(function () {
            $("#txtSchTime").timepicker({
                explicitMode: true //Set showInputs to false
            });
        })

        $(document).ready(function () {
            if ($("[id*=hdnChkMat]").val() == '' && $("[id*=hdnChkLb]").val() == '') {
                $("[id*=chkMaterial]").prop('checked', true);
                $("[id*=hdnChkMat]").val('1')
                $("[id*=chkLabor]").prop('checked', true);
                $("[id*=hdnChkLb]").val('1')
            }
        });

        $("[id*=chkMaterial]").click(function () {

            var isChecked = $(this).is(":checked");
            var isCheckedLb = $("[id*=chkLabor]").is(":checked");

            var th_qty = $("[id*=gvBOM] th:contains('Qty Required')");
            var th_um = $("[id*=gvBOM] th:contains('U/M')");
            var th_bgt_unit = $("[id*=gvBOM] th:contains('Budget Unit $')");
            var th_mat_mod = $("[id*=gvBOM] th:contains('Material Mod')");
            var th_mat_ext = $("[id*=gvBOM] th:contains('Material Ext $')");
            //var th_mat_price = $("[id*=gvBOM] th:contains('Material Price $')");
            //var th_mat_markup = $("[id*=gvBOM] th:contains('Material Markup %')");
            //var th_mat_tax = $("[id*=gvBOM] th:contains('Material taxable')");
            //var th_curr = $("[id*=gvBOM] th:contains('$:')");
            var th_ven = $("[id*=gvBOM] th:contains('Vendor')");

            th_qty.css("display", isChecked ? "" : "none");
            th_um.css("display", isChecked ? "" : "none");
            th_bgt_unit.css("display", isChecked ? "" : "none");
            th_mat_mod.css("display", isChecked ? "" : "none");
            th_mat_ext.css("display", isChecked ? "" : "none");
            //th_mat_price.css("display", isChecked ? "" : "none");
            //th_mat_markup.css("display", isChecked ? "" : "none");
            //th_mat_tax.css("display", isChecked ? "" : "none");
            //th_curr.css("display", isChecked ? "" : "none");
            th_ven.css("display", isChecked ? "" : "none");

            $("[id*=gvBOM] tr").each(function () {
                $(this).find("td").eq(th_qty.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_um.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_bgt_unit.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_mat_mod.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_mat_ext.index()).css("display", isChecked ? "" : "none");
                //$(this).find("td").eq(th_mat_price.index()).css("display", isChecked ? "" : "none");
                //$(this).find("td").eq(th_mat_markup.index()).css("display", isChecked ? "" : "none");
                //$(this).find("td").eq(th_mat_tax.index()).css("display", isChecked ? "" : "none");
                //$(this).find("td").eq(th_curr.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_ven.index()).css("display", isChecked ? "" : "none");
            });


            if (isChecked) {
                $("[id*=hdnChkMat]").val('1')
            }
            else {
                $("[id*=hdnChkMat]").val('0')
            }

        });

        $("[id*=chkLabor]").click(function () {
            var isChecked = $(this).is(":checked");

            //var th_labor = $("[id*=gvBOM] th:contains('Labor')");
            var th_lb_item = $("[id*=gvBOM] th:contains('Labor Item')");
            var th_hour = $("[id*=gvBOM] th:contains('Hours')");
            var th_rate = $("[id*=gvBOM] th:contains('Rate')");
            var th_mod = $("[id*=gvBOM] th:contains('Labor Mod')");
            var th_lb_ext = $("[id*=gvBOM] th:contains('Labor Ext $')");
            var th_lb_dt = $("[id*=gvBOM] th:contains('Date')");
            var th_lb_price = $("[id*=gvBOM] th:contains('Labor Price $')");
            var th_lb_markup = $("[id*=gvBOM] th:contains('Labor Markup %')");
            var th_lb_tax = $("[id*=gvBOM] th:contains('Labor taxable')");

            //th_labor.css("display", isChecked ? "" : "none");
            th_lb_item.css("display", isChecked ? "" : "none");
            th_hour.css("display", isChecked ? "" : "none");
            th_rate.css("display", isChecked ? "" : "none");
            th_mod.css("display", isChecked ? "" : "none");
            th_lb_ext.css("display", isChecked ? "" : "none");
            th_lb_dt.css("display", isChecked ? "" : "none");
            th_lb_price.css("display", isChecked ? "" : "none");
            th_lb_markup.css("display", isChecked ? "" : "none");
            th_lb_tax.css("display", isChecked ? "" : "none");

            $("[id*=gvBOM] tr").each(function () {
                $(this).find("td").eq(th_lb_item.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_hour.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_rate.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_mod.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_lb_ext.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_lb_dt.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_lb_price.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_lb_markup.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_lb_tax.index()).css("display", isChecked ? "" : "none");
            });
            if (isChecked) {
                $("[id*=hdnChkLb]").val('1')
            }
            else {
                $("[id*=hdnChkLb]").val('0')
            }
        });

        function funMaterialLabor() {
            var isChecked = $("[id*=chkMaterial]").is(":checked");
            var isCheckedLb = $("[id*=chkLabor]").is(":checked");

            var th_qty = $("[id*=gvBOM] th:contains('Qty Required')");
            var th_um = $("[id*=gvBOM] th:contains('U/M')");
            var th_bgt_unit = $("[id*=gvBOM] th:contains('Budget Unit $')");
            var th_mat_mod = $("[id*=gvBOM] th:contains('Material Mod')");
            var th_mat_ext = $("[id*=gvBOM] th:contains('Material Ext $')");
            var th_ven = $("[id*=gvBOM] th:contains('Vendor')");

            th_qty.css("display", isChecked ? "" : "none");
            th_um.css("display", isChecked ? "" : "none");
            th_bgt_unit.css("display", isChecked ? "" : "none");
            th_mat_mod.css("display", isChecked ? "" : "none");
            th_mat_ext.css("display", isChecked ? "" : "none");
            th_ven.css("display", isChecked ? "" : "none");

            $("[id*=gvBOM] tr").each(function () {
                $(this).find("td").eq(th_qty.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_um.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_bgt_unit.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_mat_mod.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_mat_ext.index()).css("display", isChecked ? "" : "none");
                $(this).find("td").eq(th_ven.index()).css("display", isChecked ? "" : "none");
            });

            if (isChecked) {
                $("[id*=hdnChkMat]").val('1')
            }
            else {
                $("[id*=hdnChkMat]").val('0')
            }



            var th_lb_item = $("[id*=gvBOM] th:contains('Labor Item')");
            var th_hour = $("[id*=gvBOM] th:contains('Hours')");
            var th_rate = $("[id*=gvBOM] th:contains('Rate')");
            var th_mod = $("[id*=gvBOM] th:contains('Labor Mod')");
            var th_lb_ext = $("[id*=gvBOM] th:contains('Labor Ext $')");
            var th_lb_dt = $("[id*=gvBOM] th:contains('Date')");
            var th_lb_price = $("[id*=gvBOM] th:contains('Labor Price $')");
            var th_lb_markup = $("[id*=gvBOM] th:contains('Labor Markup %')");
            var th_lb_tax = $("[id*=gvBOM] th:contains('Labor taxable')");

            th_lb_item.css("display", isCheckedLb ? "" : "none");
            th_hour.css("display", isCheckedLb ? "" : "none");
            th_rate.css("display", isCheckedLb ? "" : "none");
            th_mod.css("display", isCheckedLb ? "" : "none");
            th_lb_ext.css("display", isCheckedLb ? "" : "none");
            th_lb_dt.css("display", isCheckedLb ? "" : "none");
            th_lb_price.css("display", isCheckedLb ? "" : "none");
            th_lb_markup.css("display", isCheckedLb ? "" : "none");
            th_lb_tax.css("display", isCheckedLb ? "" : "none");

            $("[id*=gvBOM] tr").each(function () {
                $(this).find("td").eq(th_lb_item.index()).css("display", isCheckedLb ? "" : "none");
                $(this).find("td").eq(th_hour.index()).css("display", isCheckedLb ? "" : "none");
                $(this).find("td").eq(th_rate.index()).css("display", isCheckedLb ? "" : "none");
                $(this).find("td").eq(th_mod.index()).css("display", isCheckedLb ? "" : "none");
                $(this).find("td").eq(th_lb_ext.index()).css("display", isCheckedLb ? "" : "none");
                $(this).find("td").eq(th_lb_dt.index()).css("display", isCheckedLb ? "" : "none");
                $(this).find("td").eq(th_lb_price.index()).css("display", isCheckedLb ? "" : "none");
                $(this).find("td").eq(th_lb_markup.index()).css("display", isCheckedLb ? "" : "none");
                $(this).find("td").eq(th_lb_tax.index()).css("display", isCheckedLb ? "" : "none");
            });
            if (isCheckedLb) {
                $("[id*=hdnChkLb]").val('1')
            }
            else {
                $("[id*=hdnChkLb]").val('0')
            }

        }



        function ValidBomMileStone() {
            var validation_check = false;
            var validation_message = "";

            $("#<%=gvBOM.ClientID %>").find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);
                var txtCode = $tr.find('input[id*=txtCode]').val();
                if (txtCode != "") {
                    var txtScope = $tr.find('input[id*=txtScope]').val();
                    if (txtScope == "") {
                        validation_check = true;
                        validation_message = validation_message + "BOM description cannot be empty<br>";
                        return false;
                    }
                }
            });

            $("#<%=gvMilestones.ClientID %>").find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);
                var txtCode = $tr.find('input[id*=txtCode]').val();
                var txtAmount = $tr.find('input[id*=txtAmount]').val();
                if (txtCode != "" || txtAmount != "") {
                    var txtScope = $tr.find('input[id*=txtScope]').val();
                    if (txtScope == "") {
                        validation_check = true;
                        validation_message = validation_message + "Milestones description cannot be empty";
                        return false;
                    }
                }
            });
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <input type="hidden" id="_ispostback" value="<%=Page.IsPostBack.ToString()%>" />
    <div style="height: 65px !important;">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-communication-contacts"></i>
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Project</asp:Label>
                                        <asp:Label CssClass="title_text_Name" ID="lblProjectNo" Text="-" runat="server"></asp:Label>
                                    </div>
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="lnkSaveTemplate" runat="server" CssClass="icon-save" Style="margin-left: 20px;" ToolTip="Save" Text="Save"
                                            OnClick="lnkSaveTemplate_Click"
                                            OnClientClick="window.btn_clicked =true; itemJSON(); return ConfirmGCAdd();">
                                        </asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <asp:Label CssClass="title_text_Name" ID="lblNetAmount" Text="" runat="server" Visible="false" Style="font-size: 14px; text-align: right; width: 100%"></asp:Label>
                                    </div>
                                    <div class="rght-content">
                                        <div class="btnclosewrap">
                                            <a title="Close" onclick="window.close();">
                                                <i class="mdi-content-clear"></i></a>
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
                                    <li runat="server" id="liaccrdAttributes"><a href="#accrdAttributes">Attributes</a></li>
                                    <li runat="server" id="liaccrdFinance"><a href="#accrdFinance">Finance</a></li>
                                    <li runat="server" id="liaccrdTicket"><a href="#accrdTicket">Tickets</a></li>
                                    <li runat="server" id="liaccrdTicketTask"><a href="#accrdTicketTask">Tickets Task</a></li>
                                    <li runat="server" id="liaccrdBOM"><a href="#accrdBOM">BOM</a></li>
                                    <li runat="server" id="liaccrdMilestone"><a href="#accrdMilestone">Billing Details</a></li>
                                    <li runat="server" id="liaccrdDocument"><a href="#accrdDocument">Documents</a></li>
                                    <li runat="server" id="liaccrdContacts"><a href="#accrdContacts">Contacts</a></li>
                                    <li runat="server" id="liaccrdPlanner"><a href="#accrdPlanner">Planner</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlSave" runat="server">
                                        <asp:Panel ID="Panel10" runat="server">
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False"
                                                    OnClick="lnkFirst_Click"><i class="fa fa-angle-left"></i></asp:LinkButton>
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
                                                <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CssClass="icon-last" CausesValidation="False" OnClick="lnkLast_Click">
                                                        <i class="fa fa-angle-double-right"></i>
                                                </asp:LinkButton>
                                            </span>
                                        </asp:Panel>
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
                            <div class="form-content-wrap form-content-wrapwd">
                                <div class="form-content-pd formpdtop">
                                    <div class="form-section-row">
                                        <div class="col s12 m12 l12" style="padding-right: 0px;">
                                            <div class="row">
                                                <div class="form-section-row">
                                                    <div class="form-input-row">
                                                        <div class="section-ttle">Project Details</div>
                                                        <div class="form-section3half">
                                                            <asp:UpdatePanel ID="updPnlAddress" runat="server">
                                                                <ContentTemplate>
                                                                    <div class="input-field col s12">
                                                                        <div id="trEstimate" class="row" runat="server" visible="false">
                                                                            <div class="pro">
                                                                                <label for="lnkEstimate">Estimate</label>
                                                                                <asp:HyperLink ID="lnkEstimate" runat="server" Target="_blank"></asp:HyperLink>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label for="txtREPdesc">Project Name</label>
                                                                            <asp:TextBox ID="txtREPdesc" runat="server" AutoCompleteType="None" autocomplete="off"
                                                                                MaxLength="75"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvProjectName" runat="server"
                                                                                ControlToValidate="txtREPdesc" Display="None" ErrorMessage="Name Required" SetFocusOnError="True">
                                                                            </asp:RequiredFieldValidator>
                                                                            <asp:ValidatorCalloutExtender
                                                                                ID="vceProjectName" runat="server" Enabled="True"
                                                                                TargetControlID="rfvProjectName">
                                                                            </asp:ValidatorCalloutExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:HyperLink for="txtCustomer" ID="lnkCustomerID" Visible="true" Target="_blank" runat="server">Customer # - Search by Customer Name, Phone #, Address etc</asp:HyperLink>
                                                                            <asp:CustomValidator ID="cvCustomer" runat="server" ClientValidationFunction="ChkCustomer"
                                                                                ControlToValidate="txtCustomer" Display="None" ErrorMessage="Please select the customer"
                                                                                SetFocusOnError="True" Enabled="False"></asp:CustomValidator>
                                                                            <asp:ValidatorCalloutExtender ID="vceCustomer1" runat="server" Enabled="True"
                                                                                TargetControlID="cvCustomer">
                                                                            </asp:ValidatorCalloutExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="txtCustomer"
                                                                                Display="None" ErrorMessage="Please select the customer" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            <asp:ValidatorCalloutExtender ID="vceCustomer"
                                                                                runat="server" Enabled="True" TargetControlID="rfvCustomer">
                                                                            </asp:ValidatorCalloutExtender>
                                                                            <asp:TextBox ID="txtCustomer" CssClass="validate" runat="server"
                                                                                placeholder="Search by customer name, phone#, address etc." autocomplete="off"></asp:TextBox>
                                                                            <asp:HiddenField ID="hdnCustID" runat="server" />
                                                                            <asp:FilteredTextBoxExtender ID="txtCustomer_FilteredTextBoxExtender" runat="server"
                                                                                Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtCustomer">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            <asp:Button ID="btnSelectCustomer" runat="server" CausesValidation="False" OnClick="btnSelectCustomer_Click"
                                                                                Style="display: none;" Text="Button" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:HyperLink for="txtLocation" ID="lnkLocationID" Visible="true" Target="_blank" runat="server">Location # - Search by Location Name, Phone #, Address etc</asp:HyperLink>
                                                                            <asp:RequiredFieldValidator ID="rfvLocation" runat="server"
                                                                                ControlToValidate="txtLocation" Display="None" ErrorMessage="Location Name Required"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            <asp:ValidatorCalloutExtender ID="vceLocation"
                                                                                runat="server" Enabled="True" TargetControlID="rfvLocation">
                                                                            </asp:ValidatorCalloutExtender>
                                                                            <asp:CustomValidator ID="cvLocation" runat="server" ClientValidationFunction="ChkLocation"
                                                                                ControlToValidate="txtLocation" Display="None" ErrorMessage="Please select the location"
                                                                                SetFocusOnError="True"></asp:CustomValidator>
                                                                            <asp:ValidatorCalloutExtender ID="vceLocation1" runat="server" Enabled="True"
                                                                                TargetControlID="cvLocation">
                                                                            </asp:ValidatorCalloutExtender>
                                                                            <div class="fc-input">
                                                                                <asp:TextBox ID="txtLocation" CssClass="validate" runat="server"
                                                                                    placeholder="Search by location name, phone#, address etc." autocomplete="off"></asp:TextBox>
                                                                            </div>
                                                                            <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                                                                                Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtLocation">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            <asp:HiddenField ID="hdnLocID" runat="server" />
                                                                            <asp:Button ID="btnSelectLoc" runat="server" CausesValidation="False" OnClick="btnSelectLoc_Click"
                                                                                Style="display: none;" Text="Button" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label for="txtAddress">Address</label>
                                                                            <asp:TextBox ID="txtAddress" Height="50px" TextMode="MultiLine" class="materialize-textarea" runat="server">
                                                                            </asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                                                        <div class="row">
                                                                            <label for="txtCompany">Company</label>
                                                                            <asp:TextBox ID="txtCompany" runat="server" CssClass="validate" Enabled="false"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label class="drpdwn-label">Default Salesperson</label>
                                                                            <asp:DropDownList ID="ddlTerr" Enabled="false" CssClass="browser-default" runat="server">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label class="drpdwn-label">Salesperson 2</label>
                                                                            <asp:DropDownList ID="ddlTerr2" Enabled="false" CssClass="browser-default" runat="server">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnSelectLoc" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnSelectCustomer" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="form-section3half-blank">
                                                            &nbsp;
                                                        </div>
                                                        <div class="form-section3half">
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Template Type</label>
                                                                    <asp:RequiredFieldValidator ID="rfvTemplateType" runat="server" ControlToValidate="ddlTemplate"
                                                                        Display="None" ErrorMessage="Please select template type" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceTemplateType" runat="server" Enabled="True" TargetControlID="rfvTemplateType" PopupPosition="BottomLeft">
                                                                    </asp:ValidatorCalloutExtender>
                                                                    <div class="fc-input">
                                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                                                            <ContentTemplate>
                                                                                <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="browser-default" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged"
                                                                                    AutoPostBack="true" onchange="itemJSON();">
                                                                                </asp:DropDownList>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Task Category</label>
                                                                    <asp:RequiredFieldValidator ID="rfvTaskCategory" runat="server" ControlToValidate="ddlCodeCat"
                                                                        Display="None" ErrorMessage="Please select Task Category" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server"
                                                                        TargetControlID="rfvTaskCategory" PopupPosition="BottomLeft">
                                                                    </asp:ValidatorCalloutExtender>
                                                                    <div class="fc-input">
                                                                        <asp:UpdatePanel ID="UpdatePanel26" runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:DropDownList ID="ddlCodeCat" CssClass="browser-default" runat="server" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddlCodeCat_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Status</label>
                                                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" InitialValue="Select Status" ControlToValidate="ddlJobStatus"
                                                                        Display="None" ErrorMessage="Please select Status" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vceStatus" runat="server" Enabled="True" TargetControlID="rfvStatus" PopupPosition="BottomLeft">
                                                                    </asp:ValidatorCalloutExtender>
                                                                    <div class="fc-input">
                                                                        <asp:DropDownList ID="ddlJobStatus" CssClass="browser-default" runat="server"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Always">
                                                                <ContentTemplate>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <label for="ddlJobType" class="drpdwn-label">Department</label>
                                                                            <asp:DropDownList ID="ddlJobType" CssClass="browser-default" runat="server"
                                                                                OnSelectedIndexChanged="ddlJobType_SelectedIndexChanged" AutoPostBack="true">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <label for="txtREPremarks">Remarks</label>
                                                                    <asp:TextBox ID="txtREPremarks" runat="server" CssClass="materialize-textarea mtarea"
                                                                        Height="50px" TextMode="MultiLine"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section-row" style="padding-bottom: 15px;">
                                                <div class="form-input-row">
                                                    <%--<div class="section-ttle">Summary</div>--%>
                                                    <div class="grid_container">
                                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                            <telerik:RadGrid ID="gvBudgetGrid" runat="server" AutoGenerateColumns="False"
                                                                CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                PageSize="20" ShowFooter="false">
                                                                <CommandItemStyle />
                                                                <HeaderStyle Font-Bold="true" />
                                                                <ItemStyle BackColor="WhiteSmoke" />
                                                                <AlternatingItemStyle BackColor="White" />
                                                                <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                </ClientSettings>
                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn HeaderText=" " ItemStyle-Width="7%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblHeader" runat="server" Text='<%# Eval("Header") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblHeaderFooter" runat="server" Text="Variance"></asp:Label>
                                                                            </FooterTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Revenue" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRev" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Rev", "{0:c}")%>'
                                                                                    ForeColor='<%# Convert.ToDouble(Eval("Rev"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblRevFooter" runat="server"></asp:Label>
                                                                            </FooterTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Labor Expense" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLaborExp" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Labor", "{0:c}")%>'
                                                                                    ForeColor='<%# Convert.ToDouble(Eval("Labor"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblLaborExpFooter" runat="server"></asp:Label>
                                                                            </FooterTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Material Expense" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblMatExp" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Mat", "{0:c}")%>'
                                                                                    ForeColor='<%# Convert.ToDouble(Eval("Mat"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblMatExpFooter" runat="server"></asp:Label>
                                                                            </FooterTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Other Expense" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblOtherExp" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OtherExp", "{0:c}")%>'
                                                                                    ForeColor='<%# Convert.ToDouble(Eval("OtherExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblOtherExpFooter" runat="server"></asp:Label>
                                                                            </FooterTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Total Expense" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCost" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cost", "{0:c}")%>'
                                                                                    ForeColor='<%# Convert.ToDouble(Eval("Cost"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblCostFooter" runat="server"></asp:Label>
                                                                            </FooterTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Profit" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblProfit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Profit", "{0:c}")%>'
                                                                                    ForeColor='<%# Convert.ToDouble(Eval("Profit"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblProfitFooter" runat="server"></asp:Label>
                                                                            </FooterTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="% in Profit" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRatio" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Ratio", "{0:n}")%>'
                                                                                    ForeColor='<%# Convert.ToDouble(Eval("Ratio"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblRatioFooter" runat="server"></asp:Label>
                                                                            </FooterTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Hours" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblHours" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Hour", "{0:n}")%>'
                                                                                    ForeColor='<%# Convert.ToDouble(Eval("Hour"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblHoursFooter" runat="server"></asp:Label>
                                                                            </FooterTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Total On Order" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblOnOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OnOrder", "{0:c}")%>'
                                                                                    ForeColor='<%# Convert.ToDouble(Eval("OnOrder"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblOnOrderFooter" runat="server"></asp:Label>
                                                                            </FooterTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                            </telerik:RadGrid>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="form-section-row">
                                                <div class="form-input-row">
                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <asp:PlaceHolder ID="PlaceHolderHeader" runat="server"></asp:PlaceHolder>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="cf"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container accordian-wrap">
                        <div class="row">
                            <ul runat="server" id="TabContainerHeader" class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                                <li id="tbpnlAttri" runat="server">
                                    <div id="accrdAttributes" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>Attributes</div>
                                    <div class="collapsible-body">
                                        <div class="form-content-wrap">
                                            <div class="form-content-pd">
                                                <div class="form-section-row">
                                                    <div class="row">
                                                        <div class="col s12 m12 l12" style="padding-right: 0px;">
                                                            <div class="row">
                                                                <div style="float: left; clear: left; width: 100%; padding-top: 15px;">
                                                                    <div class="col s12">
                                                                        <ul class="tabs tab-demo-active white" style="width: 100%; z-index: 0;">
                                                                            <li class="tab col s2">
                                                                                <a class="white-text waves-effect waves-light active" href="#tbpnlGeneral">General</a>
                                                                            </li>
                                                                            <li class="tab col s2">
                                                                                <a class="white-text waves-effect waves-light" href="#tbpnlTeam">Team</a>
                                                                            </li>
                                                                            <li class="tab col s2">
                                                                                <a class="white-text waves-effect waves-light" href="#tbpnlGCInfo">GC Information</a>
                                                                            </li>
                                                                            <li class="tab col s2">
                                                                                <a class="white-text waves-effect waves-light" href="#TabPaneHomeowner">Homeowner</a>
                                                                            </li>
                                                                            <li class="tab col s2">
                                                                                <a class="white-text waves-effect waves-light" href="#tbpnlEquipment">Equipment</a>
                                                                            </li>
                                                                            <li class="tab col s2">
                                                                                <a class="white-text waves-effect waves-light" href="#TabPanel1">Notes</a>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                    <div class="col s12">
                                                                        <div id="tbpnlGeneral" class="col s12 tab-container-border lighten-4" style="display: block;">
                                                                            <div class="form-content-wrap" style="overflow: auto;">
                                                                                <div class="tabs-custom-mgn1" style="padding-top: 20px;">
                                                                                    <div class="form-section-row">
                                                                                        <div class="col s12 m12 l12" style="padding-right: 0px; padding-top: 30px; overflow-x: hidden;">
                                                                                            <div class="row">
                                                                                                <div class="form-section-row">
                                                                                                    <div class="form-input-row">
                                                                                                        <div class="form-section3half">
                                                                                                            <div class="input-field col s12">
                                                                                                                <div class="row">
                                                                                                                    <label for="txtProjCreationDate">Project Creation Date</label>
                                                                                                                    <asp:TextBox ID="txtProjCreationDate" runat="server" CssClass=""></asp:TextBox>
                                                                                                                    <asp:CalendarExtender ID="txtProjCreationDate_CalendarExtender" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtProjCreationDate">
                                                                                                                    </asp:CalendarExtender>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="input-field col s12">
                                                                                                                <div class="row">
                                                                                                                    <label for="txtPO">PO#</label>
                                                                                                                    <asp:TextBox ID="txtPO" runat="server" CssClass=""></asp:TextBox>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="input-field col s12">
                                                                                                                <div class="row">
                                                                                                                    <label for="txtSalesOrder">Sales Order #</label>
                                                                                                                    <asp:TextBox ID="txtSalesOrder" runat="server" CssClass=""></asp:TextBox>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <label class="drpdwn-label">Attach PO</label>
                                                                                                            <div class="input-field col s12">
                                                                                                                <div class="row">
                                                                                                                    <div class="fc-input" style="padding-top: 5px;">
                                                                                                                        <asp:FileUpload ID="fuAttachPO" runat="server" class="dropify" />
                                                                                                                        <asp:LinkButton ID="lnkUploadDoc" runat="server" CausesValidation="False" OnClick="lnkUploadDoc_Click"
                                                                                                                            Style="display: none">Upload</asp:LinkButton>
                                                                                                                        <asp:LinkButton ID="lnkPostback" runat="server"
                                                                                                                            CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="input-field col s12">
                                                                                                                <div class="checkrow">
                                                                                                                    <span class="tro">
                                                                                                                        <asp:CheckBox ID="chkCertifiedJob" Text="Certified Job" runat="server" />
                                                                                                                    </span>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div class="form-section3half-blank">
                                                                                                            &nbsp;
                                                                                                        </div>
                                                                                                        <div class="form-section3half">
                                                                                                            <div class="input-field col s12">
                                                                                                                <div class="row">
                                                                                                                    <label for="txtCustom1">Custom 1</label>
                                                                                                                    <asp:TextBox ID="txtCustom1" runat="server" CssClass=""></asp:TextBox>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="input-field col s12">
                                                                                                                <div class="row">
                                                                                                                    <label for="txtCustom2">Custom 2</label>
                                                                                                                    <asp:TextBox ID="txtCustom2" runat="server" CssClass=""></asp:TextBox>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="input-field col s12">
                                                                                                                <div class="row">
                                                                                                                    <label for="txtCustom3">Custom 3</label>
                                                                                                                    <asp:TextBox ID="txtCustom3" runat="server"></asp:TextBox>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="input-field col s12">
                                                                                                                <div class="row">
                                                                                                                    <label for="txtCustom4">Custom 4</label>
                                                                                                                    <asp:TextBox ID="txtCustom4" runat="server"></asp:TextBox>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="input-field col s12">
                                                                                                                <div class="row">
                                                                                                                    <label for="txtCustom5">Custom 5</label>
                                                                                                                    <asp:TextBox ID="txtCustom5" runat="server"></asp:TextBox>
                                                                                                                    <asp:CalendarExtender ID="txtCustom5_CalendarExtender" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtCustom5">
                                                                                                                    </asp:CalendarExtender>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div class="form-input-row">
                                                                                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                                                                                <ContentTemplate>
                                                                                                                    <asp:PlaceHolder ID="PlaceHolderAttrGeneral" runat="server"></asp:PlaceHolder>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
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
                                                                        <div id="tbpnlTeam" class="col s12 tab-container-border lighten-4" style="display: none;">
                                                                            <div class="tabs-custom-mgn1" style="padding-top: 20px;">
                                                                                <div class="form-section-row">
                                                                                    <div class="section-ttle">Items</div>
                                                                                    <div class="grid_container">
                                                                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                                                <ContentTemplate>
                                                                                                    <telerik:RadGrid ID="gvTeamItems" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                                                        PageSize="20" ShowFooter="true"
                                                                                                        OnRowCommand="gvTeamItems_RowCommand">
                                                                                                        <CommandItemStyle />
                                                                                                        <HeaderStyle Font-Bold="true" />
                                                                                                        <ItemStyle BackColor="WhiteSmoke" />
                                                                                                        <AlternatingItemStyle BackColor="White" />
                                                                                                        <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                                            <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                                                        </ClientSettings>
                                                                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                                            <Columns>
                                                                                                                <telerik:GridTemplateColumn ItemStyle-Width="4%">
                                                                                                                    <HeaderTemplate>
                                                                                                                        <a id="Button2" class="delButton" onclick="DelRow('<%#gvTeamItems.ClientID%>');"
                                                                                                                            style="cursor: pointer;">
                                                                                                                            <img src="images/menu_delete.png" title="Delete" width="18px;" /></a>
                                                                                                                    </HeaderTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:ImageButton ID="imgBtnAdd" runat="server" CommandName="AddTeam" CausesValidation="False"
                                                                                                                            CommandArgument="<%# ((GridFooterItem) Container).RowIndex %>"
                                                                                                                            ImageUrl="~/images/add.png" Width="18px" OnClientClick="itemJSON();" />
                                                                                                                    </FooterTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Line No." ItemStyle-Width="7%">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblIndex" runat="server" Text="<%# Container.ItemIndex +1 %>"></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Title" ItemStyle-Width="7%">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtTitle" CssClass="form-control input-sm input-small" Text='<%# Eval("Title") %>' runat="server"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="MOM User ID" ItemStyle-Width="7%">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtUserID" CssClass="usearchinput form-control input-sm input-small" Text='<%# Eval("UserID") %>'
                                                                                                                            Placeholder="Search" ToolTip="Search by User ID/Firstname/Lastname" runat="server"></asp:TextBox>
                                                                                                                        <asp:HiddenField ID="hdnUserID" runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="First Name" ItemStyle-Width="7%">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtFirstName" CssClass="form-control input-sm input-small" Text='<%# Eval("FirstName") %>' runat="server"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Last Name" ItemStyle-Width="7%">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtLastName" CssClass="form-control input-sm input-small" Text='<%# Eval("LastName") %>' runat="server"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Email" ItemStyle-Width="7%">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtEmail" CssClass="form-control input-sm input-small" Text='<%# Eval("Email") %>' runat="server"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Mobile #" ItemStyle-Width="7%">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtMobile" CssClass="form-control input-sm input-small" Text='<%# Eval("Mobile") %>' runat="server"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                            </Columns>
                                                                                                        </MasterTableView>
                                                                                                    </telerik:RadGrid>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <asp:Panel ID="tbpnlGCInfo" runat="server" ClientIDMode="Static" CssClass="col s12 tab-container-border lighten-4" Style="display: none;">
                                                                            <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Always">
                                                                                <ContentTemplate>
                                                                                    <div class="tabs-custom-mgn1" style="padding-top: 20px;">
                                                                                        <div class="form-section-row">
                                                                                            <div class="form-section3half">
                                                                                                <div class="input-field col s12">
                                                                                                    <div class="row">
                                                                                                        <label for="txtName">Name - Search GC</label>
                                                                                                        <asp:TextBox ID="txtName" placeholder="Search GC" runat="server"></asp:TextBox>
                                                                                                        <asp:HiddenField ID="hdnGCID" runat="server" />
                                                                                                        <asp:HiddenField ID="hdnGCIDtemp" runat="server" />
                                                                                                        <asp:HiddenField ID="hdnGCName" runat="server" />
                                                                                                        <asp:HiddenField ID="hdnGContractorID" Value="0" runat="server" />
                                                                                                        <asp:HiddenField ID="hdnGCNameupdate" Value="0" runat="server" />
                                                                                                    </div>
                                                                                                </div>

                                                                                                <div class="input-field col s12">
                                                                                                    <div class="row">
                                                                                                        <label for="GctxtAddress">Address</label>
                                                                                                        <asp:TextBox class="materialize-textarea mtarea" ID="GctxtAddress" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                                                                    </div>
                                                                                                </div>

                                                                                                <div class="input-field col s5">
                                                                                                    <div class="row">
                                                                                                        <label for="txtCity">City</label>
                                                                                                        <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="input-field col s2">
                                                                                                    <div class="row">
                                                                                                        &nbsp;
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="input-field col s5">
                                                                                                    <div class="row">
                                                                                                        <label class="drpdwn-label">State/Province</label>
                                                                                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="browser-default"
                                                                                                            TabIndex="4">
                                                                                                        </asp:DropDownList>
                                                                                                    </div>
                                                                                                </div>

                                                                                                <div class="input-field col s5">
                                                                                                    <div class="row">
                                                                                                        <label for="txtPostalCode">Zip/Postal Code</label>
                                                                                                        <asp:TextBox ID="txtPostalCode" runat="server"></asp:TextBox>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="input-field col s2">
                                                                                                    <div class="row">
                                                                                                        &nbsp;
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="input-field col s5">
                                                                                                    <div class="row">
                                                                                                        <label for="txtCountry">Country</label>
                                                                                                        <asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="form-section3half-blank">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                            <div class="form-section3half">
                                                                                                <div class="input-field col s12">
                                                                                                    <div class="row">
                                                                                                        <label for="txtContactName">Contact Name</label>
                                                                                                        <asp:TextBox ID="txtContactName" runat="server"></asp:TextBox>
                                                                                                    </div>
                                                                                                </div>

                                                                                                <div class="input-field col s12">
                                                                                                    <div class="row">
                                                                                                        <label for="txtPhone">Phone</label>
                                                                                                        <asp:TextBox ID="txtPhone" Placeholder="(xxx)xxx-xxxx Ext: xxx" runat="server"></asp:TextBox>
                                                                                                    </div>
                                                                                                </div>

                                                                                                <div class="input-field col s12">
                                                                                                    <div class="row">
                                                                                                        <label for="txtFax">Fax</label>
                                                                                                        <asp:TextBox ID="txtFax" Placeholder="(xxx)xxx-xxxx Ext: xxx" runat="server"></asp:TextBox>
                                                                                                    </div>
                                                                                                </div>

                                                                                                <div class="input-field col s12">
                                                                                                    <div class="row">
                                                                                                        <label for="txtEmailWeb">Email</label>
                                                                                                        <asp:TextBox ID="txtEmailWeb" runat="server"></asp:TextBox>
                                                                                                        <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                                                                                            ControlToValidate="txtEmailWeb" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                                                        <asp:ValidatorCalloutExtender ID="vceEmail" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                                                            TargetControlID="revEmail">
                                                                                                        </asp:ValidatorCalloutExtender>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="input-field col s12">
                                                                                                    <div class="row">
                                                                                                        <label for="txtMobile">Cellular</label>
                                                                                                        <asp:TextBox ID="txtMobile" Placeholder="(xxx)xxx-xxxx" runat="server"></asp:TextBox>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="form-section-row">
                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <label for="txtRemarks">Remarks</label>
                                                                                                    <asp:TextBox ID="txtRemarks" runat="server"></asp:TextBox>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                            <div class="tabs-custom-mgn1" style="padding-top: 20px;">
                                                                                <div class="form-section-row">
                                                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                                                                                        <ContentTemplate>
                                                                                            <asp:PlaceHolder ID="PlaceHolderAttriGC" runat="server"></asp:PlaceHolder>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </div>
                                                                        </asp:Panel>
                                                                        <asp:Panel ID="TabPaneHomeowner" runat="server" ClientIDMode="Static" CssClass="col s12 tab-container-border lighten-4" Style="display: block;">
                                                                            <div class="form-content-wrap" style="overflow: auto;">
                                                                                <div class="tabs-custom-mgn1" style="padding-top: 20px;">
                                                                                    <div class="form-section-row">
                                                                                        <div class="col s12 m12 l12" style="padding-right: 0px; padding-top: 30px; overflow-x: hidden;">
                                                                                            <div class="row">
                                                                                                <div class="form-section-row">
                                                                                                    <asp:UpdatePanel ID="UpdatePanel28" runat="server" UpdateMode="Always">
                                                                                                        <ContentTemplate>
                                                                                                            <div class="form-input-row">
                                                                                                                <div class="form-section3half">
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="hotxtname">Name</label>
                                                                                                                            <asp:TextBox ID="hotxtname" placeholder="" runat="server"></asp:TextBox>
                                                                                                                            <asp:HiddenField ID="hdnIsHO" runat="server" />
                                                                                                                            <asp:HiddenField ID="hdnHomeOwnerID" Value="0" runat="server" />
                                                                                                                            <asp:HiddenField ID="hdnHOName" runat="server" />
                                                                                                                            <asp:HiddenField ID="hdnHONameupdate" Value="0" runat="server" />
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="HotxtAddress">Address</label>
                                                                                                                            <asp:TextBox ID="HotxtAddress" CssClass="materialize-textarea" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </div>

                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="hotxtcity">City</label>
                                                                                                                            <asp:TextBox ID="hotxtcity" runat="server"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="hotddlstate" class="drpdwn-label">State/Prov</label>
                                                                                                                            <asp:DropDownList ID="hotddlstate" runat="server" class="browser-default"
                                                                                                                                TabIndex="4">
                                                                                                                            </asp:DropDownList>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="hotxtZIP">ZIP/Postal Code</label>
                                                                                                                            <asp:TextBox ID="hotxtZIP" runat="server"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="hotxtCountry">Country</label>
                                                                                                                            <asp:TextBox ID="hotxtCountry" runat="server"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                                <div class="form-section3half-blank">
                                                                                                                    &nbsp;
                                                                                                                </div>
                                                                                                                <div class="form-section3half">
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="hotxtContactName">Contact Name</label>
                                                                                                                            <asp:TextBox ID="hotxtContactName" runat="server"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="hotxtPhone">Phone</label>
                                                                                                                            <asp:TextBox ID="hotxtPhone" Placeholder="(xxx)xxx-xxxx Ext: xxx" runat="server"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="HotxtFax">Phone</label>
                                                                                                                            <asp:TextBox ID="HotxtFax" Placeholder="(xxx)xxx-xxxx Ext: xxx" runat="server"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="HotxtEmailWeb">Email</label>
                                                                                                                            <div class="fc-input">
                                                                                                                                <asp:TextBox ID="HotxtEmailWeb" runat="server"></asp:TextBox>
                                                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                                                                                                    ControlToValidate="HotxtEmailWeb" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                                                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                                                                                <asp:ValidatorCalloutExtender ID="hoValidatorC" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                                                                                    TargetControlID="RegularExpressionValidator1">
                                                                                                                                </asp:ValidatorCalloutExtender>
                                                                                                                            </div>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="hotxtMobile">Cellular</label>
                                                                                                                            <asp:TextBox ID="hotxtMobile" Placeholder="(xxx)xxx-xxxx" runat="server"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="form-input-row">
                                                                                                                <div class="form-section3half">
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="hotxtRemarks">Remarks</label>
                                                                                                                            <asp:TextBox ID="hotxtRemarks" runat="server"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                    <div class="form-input-row">
                                                                                                        <asp:UpdatePanel ID="UpdatePanel29" runat="server">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="cf"></div>
                                                                            </div>
                                                                        </asp:Panel>
                                                                        <div id="tbpnlEquipment" class="col s12 tab-container-border lighten-4" style="display: none;">
                                                                            <div class="tabs-custom-mgn1" style="padding-top: 20px;">
                                                                                <div class="form-input-row">
                                                                                    <div class="searchpaneinner">
                                                                                        <div class="btnlinks">
                                                                                            <asp:LinkButton CssClass="icon-edit" ID="lnkEditEq" ToolTip="Edit" runat="server" OnClick="lnkEditEq_Click" CausesValidation="False" Text="Edit"></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="btnlinks">
                                                                                            <asp:LinkButton ID="btnCopyEQ" runat="server" CssClass="icon-copy" ToolTip="Copy" OnClick="btnCopyEQ_Click" CausesValidation="False" Text="Copy"></asp:LinkButton>
                                                                                        </div>
                                                                                        <div class="btnlinks">
                                                                                            <asp:LinkButton ID="lnkAddEQ" runat="server" CssClass="icon-addnew" ToolTip="Add New" OnClick="lnkAddEQ_Click" CausesValidation="False" Text="Add"></asp:LinkButton>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="grid_container" style="margin-top: 20px;">
                                                                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                                                <ContentTemplate>
                                                                                                    <asp:Repeater ID="rtEquips" runat="server" OnItemDataBound="rtEquips_ItemDataBound">
                                                                                                        <HeaderTemplate>
                                                                                                            <table class="table table-bordered table-striped table-condensed flip-content">
                                                                                                                <tr>
                                                                                                                    <th></th>
                                                                                                                    <th>Name</th>
                                                                                                                    <th>Manuf.</th>
                                                                                                                    <th>Description</th>
                                                                                                                    <th>Type</th>
                                                                                                                    <th>Service type</th>
                                                                                                                    <th>Status</th>
                                                                                                                    <th>Customer</th>
                                                                                                                    <th>Location ID</th>
                                                                                                                    <th>Location</th>
                                                                                                                    <th>Address</th>
                                                                                                                    <th>Price</th>
                                                                                                                    <th>Last Service</th>
                                                                                                                    <th>Installed</th>
                                                                                                                </tr>
                                                                                                        </HeaderTemplate>
                                                                                                        <ItemTemplate>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                                                    <asp:Image runat="server" AlternateText="" Style="cursor: pointer;" ID="imgPlus" ImageUrl="images/plus.png" />
                                                                                                                    <div id="divCustom" style="display: none">
                                                                                                                        <asp:Repeater ID="rtEquipCustom" runat="server">
                                                                                                                            <HeaderTemplate>
                                                                                                                                <table style="width: 400px;" class="table table-bordered table-condensed flip-content">
                                                                                                                                    <tr>
                                                                                                                                        <th>Desc</th>
                                                                                                                                        <th>Value</th>
                                                                                                                                        <th>Last Updated</th>
                                                                                                                                    </tr>
                                                                                                                            </HeaderTemplate>
                                                                                                                            <ItemTemplate>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                        <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                                                                                                                                    </td>
                                                                                                                                    <td>
                                                                                                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("LastUpdateUser") %>'></asp:Label>
                                                                                                                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("LastUpdated", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </ItemTemplate>
                                                                                                                            <FooterTemplate>
                                                                                                                                </table>
                                                                                                                            </FooterTemplate>
                                                                                                                        </asp:Repeater>
                                                                                                                    </div>
                                                                                                                </td>
                                                                                                                <td>

                                                                                                                    <asp:Label ID="lblID" Visible="false" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("unit") %>'></asp:Label>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblmanuf" runat="server" Text='<%# Bind("manuf") %>'></asp:Label>

                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>

                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblType" runat="server"><%#Eval("Type")%></asp:Label>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblServiceType" runat="server"><%#Eval("cat")%></asp:Label></td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label></td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblCust" runat="server"><%#Eval("name")%></asp:Label></td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblLocid" runat="server"><%#Eval("LocID")%></asp:Label></td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblLocName" runat="server"><%#Eval("tag")%></asp:Label></td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblAddress" runat="server"><%#Eval("Address")%></asp:Label></td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblPrice" runat="server"><%# DataBinder.Eval(Container.DataItem, "Price", "{0:c}")%></asp:Label></td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lbllast" runat="server"><%# Eval("last")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "last"))):""%></asp:Label></td>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lblSince" runat="server"><%# Eval("since") != DBNull.Value ? String.Format("{0:M/d/yyyy}", Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "since"))) : ""%></asp:Label></td>
                                                                                                            </tr>
                                                                                                        </ItemTemplate>
                                                                                                        <FooterTemplate>
                                                                                                            </table>
                                                                                                        </FooterTemplate>
                                                                                                    </asp:Repeater>


                                                                                                </ContentTemplate>
                                                                                                <Triggers>
                                                                                                    <asp:AsyncPostBackTrigger ControlID="btnSelectLoc" />
                                                                                                    <asp:AsyncPostBackTrigger ControlID="btnSelectCustomer" />
                                                                                                </Triggers>
                                                                                            </asp:UpdatePanel>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-input-row">
                                                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                                                            <ContentTemplate>
                                                                                                <asp:PlaceHolder ID="PlaceHolderAttriEquip" runat="server"></asp:PlaceHolder>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div id="TabPanel1" class="col s12 tab-container-border lighten-4" style="display: none; padding-top: 20px;">
                                                                            <div class="tabs-custom-mgn1" style="padding-top: 20px;">
                                                                                <asp:UpdatePanel ID="UpdatePanel24" runat="server" UpdateMode="Always">
                                                                                    <ContentTemplate>
                                                                                        <div class="form-section3half">
                                                                                            <div class="input-field col s12">
                                                                                                <div class="checkrow">
                                                                                                    <asp:CheckBox ID="chkspnotes" runat="server" Text="Special Notes" CssClass="filled-in" />
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <label for="txtSpecialInstructions">Special Note Content</label>
                                                                                                    <asp:TextBox ID="txtSpecialInstructions" CssClass="materialize-textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="form-section3half-blank">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                        <div class="form-section3half">
                                                                                            <div class="input-field col s12">
                                                                                                <div class="checkrow">
                                                                                                    <asp:CheckBox ID="chkRenew" runat="server" Text="Renew Notes" CssClass="filled-in" />
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <label for="txtRenew">Renew Note Content</label>
                                                                                                    <asp:TextBox ID="txtRenew" CssClass="materialize-textarea" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                                <div class="form-input-row">
                                                                                    <asp:UpdatePanel ID="UpdatePanel25" runat="server" UpdateMode="Always">
                                                                                        <ContentTemplate>
                                                                                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
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
                                        <div class="cf"></div>
                                    </div>
                                </li>
                                <li id="tbpnlFinance" runat="server">
                                    <div id="accrdFinance" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Finance</div>
                                    <div class="collapsible-body">
                                        <div class="form-content-wrap">
                                            <div class="form-content-pd">
                                                <div class="form-section-row">
                                                    <div class="row">
                                                        <div class="col s12 m12 l12" style="padding-right: 0px;">
                                                            <div class="row">
                                                                <div style="float: left; clear: left; width: 100%; margin-top: 10px;">
                                                                    <div class="col s12">
                                                                        <ul class="tabs tab-demo-active white" style="width: 100%; height: 18px !important; line-height: 8px !important; z-index: 0;">
                                                                            <li class="tab col s2">
                                                                                <a class="white-text waves-effect waves-light active" href="#tbpnlGeneral1">General</a>
                                                                            </li>
                                                                            <li class="tab col s2">
                                                                                <a class="white-text waves-effect waves-light" href="#tbpnlBudgets">Budgets</a>
                                                                            </li>
                                                                            <li class="tab col s2">
                                                                                <a class="white-text waves-effect waves-light" href="#tbpnlBilling">Billing</a>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                    <div class="col s12">
                                                                        <asp:Panel ID="tbpnlGeneral1" ClientIDMode="Static" runat="server" CssClass="col s12 tab-container-border lighten-4" Style="display: block; padding-top: 20px;">
                                                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                                                                                <ContentTemplate>
                                                                                    <div class="row">
                                                                                        <div class="form-section3">
                                                                                            <div class="col s12 m12 l12">
                                                                                                <div class="row">
                                                                                                    <div class="input-field col s12">
                                                                                                        <div class="row">
                                                                                                            <label for="uc_InvExpGL">Expense GL</label>
                                                                                                            <uc1:uc_AccountSearch ID="uc_InvExpGL" UpdateMode="Conditional" runat="server" />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="input-field col s12">
                                                                                                        <div class="row">
                                                                                                            <label for="uc_InterestGL">Interest GL</label>
                                                                                                            <uc1:uc_AccountSearch ID="uc_InterestGL" UpdateMode="Conditional" runat="server" />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="input-field col s12">
                                                                                                        <div class="row">
                                                                                                            <label for="txtInvService">Billing Code</label>
                                                                                                            <asp:TextBox ID="txtInvService" runat="server" AutoCompleteType="None"
                                                                                                                MaxLength="255"></asp:TextBox>
                                                                                                            <asp:HiddenField ID="hdnInvServiceID" runat="server" />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="input-field col s12">
                                                                                                        <div class="row">
                                                                                                            <label for="txtPrevilWage">Labor Wage</label>
                                                                                                            <asp:TextBox ID="txtPrevilWage" runat="server" AutoCompleteType="None"
                                                                                                                MaxLength="255"></asp:TextBox>
                                                                                                            <asp:HiddenField ID="hdnPrevilWageID" runat="server" />
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>

                                                                                        </div>
                                                                                        <div class="form-section3-blank">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                        <div class="form-section3">
                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <label class="drpdwn-label">Posting Type</label>
                                                                                                    <select class="browser-default" disabled="disabled">
                                                                                                        <option value="" selected>No Data Found</option>
                                                                                                        <option value="1">Alberta</option>
                                                                                                        <option value="2">Alaska</option>
                                                                                                    </select>
                                                                                                </div>
                                                                                            </div>

                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <label for="ddlContractType1" class="drpdwn-label">Service Type</label>
                                                                                                    <asp:DropDownList ID="ddlContractType1" runat="server" CssClass="browser-default"></asp:DropDownList>
                                                                                                </div>
                                                                                            </div>

                                                                                            <div class="input-field col s12" style="margin-bottom: 10px;">
                                                                                                <div class="checkrow">
                                                                                                    <span class="tro">
                                                                                                        <%--<label for="chkChargeInt" class="title-check-text">Charge Interest</label>--%>
                                                                                                        <asp:CheckBox ID="chkChargeInt" Text="Charge Interest" runat="server" CssClass="filled-in" />
                                                                                                    </span>
                                                                                                </div>
                                                                                            </div>

                                                                                            <div class="input-field col s12" style="margin-bottom: 10px;">
                                                                                                <div class="checkrow">
                                                                                                    <span class="tro">
                                                                                                        <%--<label for="chkChargeable" class="title-check-text">Chargeable</label>--%>
                                                                                                        <asp:CheckBox ID="chkChargeable" Text="Chargeable" runat="server" CssClass="filled-in" />
                                                                                                    </span>
                                                                                                </div>
                                                                                            </div>

                                                                                            <div class="input-field col s12" style="margin-bottom: 10px;">
                                                                                                <div class="checkrow">
                                                                                                    <span class="tro">
                                                                                                        <%--<label for="chkInvoicing" class="title-check-text">Close After Invoicing</label>--%>
                                                                                                        <asp:CheckBox ID="chkInvoicing" Text="Close after Invoicing" runat="server" class="filled-in" />
                                                                                                    </span>
                                                                                                </div>
                                                                                            </div>


                                                                                        </div>
                                                                                        <div class="form-section3-blank">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                        <div class="form-section3">
                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <asp:Label for="txtBillRate" ID="lblBillRate" runat="server" Text="Billing Rate"></asp:Label>
                                                                                                    <asp:TextBox ID="txtBillRate" runat="server" AutoCompleteType="None"
                                                                                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <asp:Label for="txtOt" ID="lblOt" runat="server" Text="OT Rate"></asp:Label>
                                                                                                    <asp:TextBox ID="txtOt" runat="server" AutoCompleteType="None"
                                                                                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <asp:Label for="txtNt" ID="lblNt" runat="server" Text="1.7 Rate"></asp:Label>
                                                                                                    <asp:TextBox ID="txtNt" runat="server" AutoCompleteType="None"
                                                                                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <asp:Label for="txtDt" ID="lblDt" runat="server" Text="DT Rate"></asp:Label>
                                                                                                    <asp:TextBox ID="txtDt" runat="server" AutoCompleteType="None"
                                                                                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <asp:Label for="txtTravel" ID="lblTravel" runat="server" Text="Travel Rate"></asp:Label>
                                                                                                    <asp:TextBox ID="txtTravel" runat="server" AutoCompleteType="None"
                                                                                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="input-field col s12">
                                                                                                <div class="row">
                                                                                                    <asp:Label for="txtMileage" ID="lblMileage" runat="server" Text="Mileage"></asp:Label>
                                                                                                    <asp:TextBox ID="txtMileage" runat="server" AutoCompleteType="None"
                                                                                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="row">
                                                                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Always">
                                                                                            <ContentTemplate>
                                                                                                <asp:PlaceHolder ID="PlaceHolderFinceGeneral" runat="server"></asp:PlaceHolder>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
                                                                                    </div>
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="ddlTemplate" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </asp:Panel>
                                                                        <asp:Panel ID="tbpnlBudgets" ClientIDMode="Static" runat="server" CssClass="col s12 tab-container-border lighten-4" Style="display: none;">
                                                                            <div class="form-content-wrap">
                                                                                <div class="tabs-custom-mgn1" style="padding-top: 30px; min-height: 150px;">
                                                                                    <div class="form-section3">
                                                                                        <div class="input-field col s12" style="margin-bottom: 30px;">
                                                                                            <div class="checkrow">
                                                                                                <span class="tro">
                                                                                                    <asp:CheckBox ID="chkExpense" runat="server" Text="Show Breakdown" class="title-check-text" />
                                                                                                </span>
                                                                                            </div>
                                                                                            <telerik:RadGrid RenderMode="Auto" ID="gvBudget" AllowFilteringByColumn="false" ShowFooter="true"
                                                                                                runat="server" Width="1050" CssClass="WIPGrid" PageSize="200" OnItemDataBound="gvBudget_ItemDataBound">
                                                                                                <CommandItemStyle />
                                                                                                <HeaderStyle Font-Bold="true" />
                                                                                                <ItemStyle BackColor="WhiteSmoke" />
                                                                                                <AlternatingItemStyle BackColor="White" />
                                                                                                <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                                                </ClientSettings>
                                                                                                <GroupingSettings ShowUnGroupButton="false"></GroupingSettings>
                                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                                    <GroupByExpressions>
                                                                                                        <telerik:GridGroupByExpression>
                                                                                                            <GroupByFields>
                                                                                                                <telerik:GridGroupByField FieldName="JobType" HeaderText="JobType" SortOrder="Descending" />
                                                                                                            </GroupByFields>
                                                                                                            <SelectFields>
                                                                                                                <telerik:GridGroupByField FieldName="JobType" HeaderText='JobType' />
                                                                                                                <%--<telerik:GridGroupByField FieldName="JobType" HeaderText="JobType" HeaderValueSeparator="Total: " Aggregate="Sum" FormatString="{0:C}" />--%>
                                                                                                            </SelectFields>
                                                                                                        </telerik:GridGroupByExpression>
                                                                                                    </GroupByExpressions>
                                                                                                    <SortExpressions>
                                                                                                        <telerik:GridSortExpression FieldName="fDesc" SortOrder="Ascending" />
                                                                                                    </SortExpressions>
                                                                                                    <Columns>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Description" DataField="fDesc" UniqueName="fDesc"
                                                                                                            HeaderStyle-Width="30%" ItemStyle-Width="30%">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblfDesc" runat="server" Text='<%# Bind("fDesc") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblPhase" runat="server" Text='<%# Bind("Phase") %>' Style="display: none;"></asp:Label>
                                                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("Type") %>' Style="display: none;"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Code" DataField="Code" UniqueName="Code" DataType="System.Int32"
                                                                                                            HeaderStyle-Width="10%" ItemStyle-Width="10%" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblOpSeq" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblNetProfit" runat="server" Text="Net Profit" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Actual" DataField="Actual" UniqueName="Actual"
                                                                                                            HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <a style="text-decoration: none" target="_blank" href="ProjectBudgetDetail.aspx?uid=<%#Request.QueryString["uid"] %>&type=<%#Eval("Type") %>&phase=<%#Eval("Phase") %>">
                                                                                                                    <asp:Label ID="lblActual" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Actual", "{0:c}")%>'
                                                                                                                        ForeColor='<%# Convert.ToDouble(Eval("Actual"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                                                                </a>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterActual" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Committed" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblComm" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Comm", "{0:c}")%>'
                                                                                                                    ForeColor='<%# Convert.ToDouble(Eval("Comm"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterComm" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Total" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblTotalActual" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Total", "{0:c}")%>'
                                                                                                                    ForeColor='<%# Convert.ToDouble(Eval("Total"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterTotalActual" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Budget" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblBudget" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Budget", "{0:c}")%>'
                                                                                                                    ForeColor='<%# Convert.ToDouble(Eval("Budget"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterBudget" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Difference" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblVariance" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Variance", "{0:c}")%>'
                                                                                                                    ForeColor='<%# Convert.ToDouble(Eval("Variance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterVariance" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Ratio" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblRatio" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Ratio", "{0:0.00}%")%>'
                                                                                                                    ForeColor='<%# Convert.ToDouble(Eval("Ratio"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterRatio" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>

                                                                                                        <telerik:GridTemplateColumn DataField="Code" HeaderText="Code" Visible="false" />
                                                                                                        <telerik:GridTemplateColumn DataField="JobType" HeaderText="Job Type" Visible="false" />
                                                                                                        <telerik:GridTemplateColumn HeaderText="Budget Hours" DataField="BHours" UniqueName="BHours" FooterStyle-HorizontalAlign="Right"
                                                                                                            HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblBudgetHr" runat="server" ForeColor='<%# Convert.ToDouble(Eval("BHours"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'
                                                                                                                    Text='<%# DataBinder.Eval(Container.DataItem, "BHours", "{0:n}") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                    </Columns>
                                                                                                </MasterTableView>
                                                                                                <FooterStyle CssClass="footer" />
                                                                                            </telerik:RadGrid>
                                                                                            <br />
                                                                                            <div id="divBudgetPager">
                                                                                                <asp:Repeater ID="rptBudgetPager" runat="server">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton ID="lnkBudgetPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>' Enabled='<%# Eval("Enabled") %>' OnClick="BudgetPage_Changed"></asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                </asp:Repeater>
                                                                                            </div>

                                                                                            <telerik:RadGrid ID="gvExpenses" runat="server" AutoGenerateColumns="False"
                                                                                                DataKeyNames="Phase" Width="100%" ShowFooter="true"
                                                                                                EmptyDataText="No Expense Items Found" ShowHeaderWhenEmpty="True">
                                                                                                <CommandItemStyle />
                                                                                                <HeaderStyle Font-Bold="true" />
                                                                                                <FooterStyle CssClass="footer" />
                                                                                                <ItemStyle BackColor="WhiteSmoke" />
                                                                                                <AlternatingItemStyle BackColor="White" />
                                                                                                <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                                                </ClientSettings>
                                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                                    <Columns>
                                                                                                        <telerik:GridBoundColumn DataField="Code" HeaderText="Op Sequence" ItemStyle-Width="5%" />
                                                                                                        <telerik:GridBoundColumn DataField="fDesc" HeaderText="Description" ItemStyle-Width="200" />
                                                                                                        <telerik:GridBoundColumn DataField="Phase" HeaderText="Phase" ItemStyle-Width="5%" />
                                                                                                        <telerik:GridTemplateColumn HeaderText="Mat. Actual" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblPhase" runat="server" Text='<%# Bind("Phase") %>' Style="display: none;"></asp:Label>
                                                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("Type") %>' Style="display: none;"></asp:Label>
                                                                                                                <a style="text-decoration: none" target="_blank" href="ProjectExpensesDetails.aspx?uid=<%#Request.QueryString["uid"] %>&type=<%#Eval("Type") %>&phase=<%#Eval("Phase") %>">
                                                                                                                    <%# DataBinder.Eval(Container.DataItem, "MatAct", "{0:c}")%>
                                                                                                                </a>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterMatAct" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Mat. Budget" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <%# DataBinder.Eval(Container.DataItem, "MatBgt", "{0:c}")%>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterMatBgt" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Mat. Modifier" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <%# DataBinder.Eval(Container.DataItem, "MatMod", "{0:c}")%>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterMatMod" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Mat. Diff." ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <%# DataBinder.Eval(Container.DataItem, "MatDiff", "{0:c}")%>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterMatDiff" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Hours" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <%# DataBinder.Eval(Container.DataItem, "HourAct", "{0:0.00}")%>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterHours" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Hr Bgt." ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <%# DataBinder.Eval(Container.DataItem, "HourBgt", "{0:0.00}")%>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterHoursBgt" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Lab. Act." ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <%# DataBinder.Eval(Container.DataItem, "LaborAct", "{0:c}")%>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterLabAct" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Lab. Bgt." ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <%# DataBinder.Eval(Container.DataItem, "LaborBgt", "{0:c}")%>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterLabBgt" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Lab. Mod." ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <%# DataBinder.Eval(Container.DataItem, "LaborMod", "{0:c}")%>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterLabMod" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                        <telerik:GridTemplateColumn HeaderText="Lab. Diff." ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                            <ItemTemplate>
                                                                                                                <%# DataBinder.Eval(Container.DataItem, "LabDiff", "{0:c}")%>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblFooterLabDiff" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </telerik:GridTemplateColumn>
                                                                                                    </Columns>
                                                                                                </MasterTableView>
                                                                                            </telerik:RadGrid>

                                                                                            <br />
                                                                                            <div id="divExpensesPager">
                                                                                                <asp:Repeater ID="rptExpensesPager" runat="server">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton ID="lnkExpensesPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>' Enabled='<%# Eval("Enabled") %>' OnClick="ExpensesPage_Changed"></asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                </asp:Repeater>
                                                                                            </div>

                                                                                            <div id="divJC" runat="server">
                                                                                                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="Panel4"
                                                                                                    ExpandControlID="Image4" CollapseControlID="Image4" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                                                                                    CollapsedImage="~/images/arrow_right.png" ImageControlID="Image4" Enabled="True" />
                                                                                                <div id="DivJobC" style="font-weight: bold; font-size: 15px">
                                                                                                    <label for="Image4">Job Cost: </label>
                                                                                                    <asp:Image ID="Image4" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                                                                                </div>
                                                                                                <asp:Panel ID="Panel4" runat="server" Style="padding: 10px 10px 10px 10px;">
                                                                                                    <telerik:RadGrid ID="gvJOBC" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                                                        DataKeyNames="ref" Width="100%" ShowHeaderWhenEmpty="True"
                                                                                                        ShowFooter="True" EmptyDataText="No job cost found...">
                                                                                                        <HeaderStyle Font-Bold="true" />
                                                                                                        <ItemStyle BackColor="WhiteSmoke" />
                                                                                                        <AlternatingItemStyle BackColor="White" />
                                                                                                        <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                                            <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                                                        </ClientSettings>
                                                                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                                            <Columns>
                                                                                                                <telerik:GridTemplateColumn HeaderText="ref" SortExpression="ref" Visible="true">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Description" SortExpression="fdesc">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblfdesc" runat="server" Text='<%# Bind("fdesc") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Invoice date" SortExpression="fdate">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("fdate", "{0:MM/dd/yy}") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Amount" SortExpression="amount">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblamount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn HeaderText="Phase" SortExpression="Phase">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblPhase" runat="server" Text='<%# Bind("Phase") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                            </Columns>
                                                                                                        </MasterTableView>
                                                                                                    </telerik:RadGrid>
                                                                                                    <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px;"
                                                                                                        title="Go to top">
                                                                                                        <img id="imgtop" alt="top" src="images/uptotop.gif" />
                                                                                                        Go To Top
                                                                                                    </div>
                                                                                                </asp:Panel>
                                                                                            </div>
                                                                                            <div class="row">
                                                                                                <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Always">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:PlaceHolder ID="PlaceHolderFinceBudget" runat="server"></asp:PlaceHolder>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </asp:Panel>
                                                                        <asp:Panel ID="tbpnlBilling" ClientIDMode="Static" runat="server" CssClass="col s12 tab-container-border lighten-4" Style="display: none; min-height: 150px;">
                                                                            <div class="form-content-wrap">
                                                                                <div class="tabs-custom-mgn1" style="padding-top: 30px;">
                                                                                    <div class="form-section-row">
                                                                                        <div class="input-field col s12" style="margin-bottom: 30px;">
                                                                                            <div class="row">
                                                                                                <div id="divInvoices" runat="server">
                                                                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="Panel2"
                                                                                                                ExpandControlID="Image2" CollapseControlID="Image2" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                                                                                                CollapsedImage="~/images/arrow_right.png" ImageControlID="Image2" Enabled="True" />
                                                                                                            <div id="Div2" style="font-weight: bold; font-size: 15px">
                                                                                                                AR Invoices:
                                                                                                                <asp:Image ID="Image2" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                                                                                            </div>
                                                                                                            <asp:Panel ID="Panel2" runat="server" Style="padding: 10px 10px 10px 10px;">
                                                                                                                <asp:DropDownList ID="ddlInvoiceStatus" runat="server" CssClass="browser-default"
                                                                                                                    Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlInvoiceStatus_SelectedIndexChanged">
                                                                                                                    <asp:ListItem Value="-1">All</asp:ListItem>
                                                                                                                    <asp:ListItem Value="0">Open</asp:ListItem>
                                                                                                                    <asp:ListItem Value="1">Paid</asp:ListItem>
                                                                                                                    <asp:ListItem Value="2">Voided</asp:ListItem>
                                                                                                                    <asp:ListItem Value="3">Partially Paid</asp:ListItem>
                                                                                                                    <asp:ListItem Value="4">Marked as Pending</asp:ListItem>
                                                                                                                    <asp:ListItem Value="5">Paid by Credit Card</asp:ListItem>
                                                                                                                </asp:DropDownList>
                                                                                                                <asp:Label ID="lblCountInvoice" runat="server" Style="font-style: italic; float: right"></asp:Label>
                                                                                                                <telerik:RadGrid ID="gvInvoice" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                                                                    DataKeyNames="ID" AllowPaging="True" PageSize="5" Width="100%" OnDataBound="gvInvoice_DataBound" OnRowCommand="gvInvoice_RowCommand"
                                                                                                                    ShowFooter="True" EmptyDataText="No invoices found...">
                                                                                                                    <CommandItemStyle />
                                                                                                                    <HeaderStyle Font-Bold="true" />
                                                                                                                    <ItemStyle CssClass="evenrowcolor" />
                                                                                                                    <AlternatingItemStyle CssClass="oddrowcolor" />
                                                                                                                    <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                                                    <FooterStyle CssClass="footer" />
                                                                                                                    <SelectedItemStyle CssClass="selectedrowcolor" />
                                                                                                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                                                        <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                                                                    </ClientSettings>
                                                                                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                                                        <Columns>
                                                                                                                            <telerik:GridTemplateColumn HeaderText="ID" SortExpression="ref" Visible="false">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                            <telerik:GridTemplateColumn HeaderText="Invoice #" SortExpression="ref">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblInv" Visible="false" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                                                                                                                    <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Bind("ref") %>' Target="_blank" NavigateUrl='<%# "addinvoice.aspx?uid=" +Eval("ref")  %>'></asp:HyperLink>
                                                                                                                                </ItemTemplate>
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                            <telerik:GridTemplateColumn HeaderText="Manual Inv. #" SortExpression="manualInv">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblMInv" runat="server" Text='<%# Bind("manualInv") %>'></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                            <telerik:GridTemplateColumn HeaderText="Invoice date" SortExpression="fdate">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("fdate", "{0:MM/dd/yy}") %>'></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                            <telerik:GridTemplateColumn HeaderText="Pretax Amount" SortExpression="amount">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblPretaxAmout" runat="server"><%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                                <FooterTemplate>
                                                                                                                                    <asp:Label ID="lblTotalPretaxAmt" runat="server"></asp:Label>
                                                                                                                                </FooterTemplate>
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                            <telerik:GridTemplateColumn HeaderText="Sales Tax" SortExpression="stax">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblSalesTax" runat="server"><%# DataBinder.Eval(Container.DataItem, "stax", "{0:c}")%></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                                <FooterTemplate>
                                                                                                                                    <asp:Label ID="lblTotalSalesTax" runat="server"></asp:Label>
                                                                                                                                </FooterTemplate>
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                            <telerik:GridTemplateColumn HeaderText="Invoice Total" SortExpression="total">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblInvoiceTotal" runat="server"><%# DataBinder.Eval(Container.DataItem, "total", "{0:c}")%></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                                <FooterTemplate>
                                                                                                                                    <asp:Label ID="InvTotalInvoice" runat="server"></asp:Label>
                                                                                                                                </FooterTemplate>
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                            <telerik:GridTemplateColumn HeaderText="Status" SortExpression="status">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblStatus" runat="server"><%#Eval("status")%></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                            <telerik:GridTemplateColumn HeaderText="PO" SortExpression="po">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblPO" runat="server"><%#Eval("po")%></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                            <telerik:GridTemplateColumn HeaderText="Department Type" SortExpression="type">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblDepartmentType" runat="server"><%#Eval("type")%></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                            <telerik:GridTemplateColumn HeaderText="Amount Due" SortExpression="balance">
                                                                                                                                <ItemTemplate>
                                                                                                                                    <asp:Label ID="lblDue" runat="server"><%# DataBinder.Eval(Container.DataItem, "balance", "{0:c}") %></asp:Label>
                                                                                                                                </ItemTemplate>
                                                                                                                                <FooterTemplate>
                                                                                                                                    <asp:Label ID="InvTotalDue" runat="server"></asp:Label>
                                                                                                                                </FooterTemplate>
                                                                                                                            </telerik:GridTemplateColumn>
                                                                                                                        </Columns>

                                                                                                                        <PagerTemplate>
                                                                                                                            <div align="center">
                                                                                                                                <asp:ImageButton ID="ImageButton1" CausesValidation="false" runat="server" CommandArgument="First" ImageUrl="images/first.png" />
                                                                                                                                &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev" CausesValidation="false"
                                                                                                                                    ImageUrl="~/images/Backward.png" />
                                                                                                                                &nbsp &nbsp <span>Page</span>
                                                                                                                                <asp:DropDownList CssClass="browser-default" ID="ddlPages" runat="server" CausesValidation="false" AutoPostBack="True" OnSelectedIndexChanged="ddlPagesInvoice_SelectedIndexChanged">
                                                                                                                                </asp:DropDownList>
                                                                                                                                <span>of </span>
                                                                                                                                <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                                                                                                &nbsp &nbsp
                                                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="false" CommandArgument="Next" ImageUrl="images/Forward.png" />
                                                                                                                                &nbsp &nbsp
                                                                        <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="false" CommandArgument="Last" ImageUrl="images/last.png" />
                                                                                                                            </div>
                                                                                                                        </PagerTemplate>
                                                                                                                    </MasterTableView>
                                                                                                                </telerik:RadGrid>
                                                                                                                <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px;"
                                                                                                                    title="Go to top">
                                                                                                                    <img id="imgtop" alt="top" src="images/uptotop.gif" />
                                                                                                                    Go To Top
                                                                                                                </div>
                                                                                                            </asp:Panel>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </div>
                                                                                                <div id="divAP" runat="server">
                                                                                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="Panel3"
                                                                                                        ExpandControlID="Image3" CollapseControlID="Image3" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                                                                                        CollapsedImage="~/images/arrow_right.png" ImageControlID="Image3" Enabled="True" />
                                                                                                    <div id="Div3" style="font-weight: bold; font-size: 15px">
                                                                                                        AP Invoices: 
                                                                                                        <asp:Image ID="Image3" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                                                                                    </div>
                                                                                                    <asp:Panel ID="Panel3" runat="server" Style="padding: 10px 10px 10px 10px;">
                                                                                                        <telerik:RadGrid ID="gvAPInvoices" runat="server" AutoGenerateColumns="False"
                                                                                                            CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                                                            DataKeyNames="ID" Width="100%"
                                                                                                            ShowFooter="True" EmptyDataText="No AP invoices found...">
                                                                                                            <CommandItemStyle />
                                                                                                            <HeaderStyle Font-Bold="true" />
                                                                                                            <ItemStyle CssClass="evenrowcolor" />
                                                                                                            <AlternatingItemStyle CssClass="oddrowcolor" />
                                                                                                            <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                                            <FooterStyle CssClass="footer" />

                                                                                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                                                <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                                                            </ClientSettings>
                                                                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                                                <Columns>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Ref" SortExpression="ref" Visible="true">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Bind("ref") %>'></asp:Label>
                                                                                                                            <asp:HyperLink ID="HyperLinkAPBills" runat="server" Text='<%# Bind("ref") %>' Target="_blank" NavigateUrl='<%# "addbills.aspx?id=" +Eval("ID")  %>'></asp:HyperLink>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Description" SortExpression="fdesc">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblfdesc" runat="server" Text='<%# Bind("fdesc") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Invoice date" SortExpression="fdate">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("fdate", "{0:MM/dd/yy}") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Project Amount" SortExpression="amount">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblprojectamount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PAmount", "{0:c}")%>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Total Amount" SortExpression="amount">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblamount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                </Columns>
                                                                                                            </MasterTableView>
                                                                                                        </telerik:RadGrid>
                                                                                                        <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px;"
                                                                                                            title="Go to top">
                                                                                                            <img id="imgtop" alt="top" src="images/uptotop.gif" />
                                                                                                            Go To Top
                                                                                                        </div>

                                                                                                    </asp:Panel>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="row">
                                                                                                <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Always">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:PlaceHolder ID="PlaceHolderFinceBill" runat="server"></asp:PlaceHolder>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="cf"></div>
                                                        </div>
                                                    </div>

                                                    <div class="cf"></div>

                                                </div>
                                            </div>
                                            <div style="clear: both;"></div>
                                        </div>
                                    </div>
                                </li>
                                <li id="tbpnlTicket" runat="server">
                                    <div id="accrdTicket" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-open-in-browser"></i>Ticket List</div>
                                    <div class="collapsible-body">
                                        <div class="form-content-wrap">
                                            <div class="form-content-pd">
                                                <div class="form-section-row">
                                                    <div class="row">
                                                        <div id="divTickets" runat="server">
                                                            <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnSelectLoc" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnSelectCustomer" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <div>
                                                                        <asp:Panel ID="panelTicketAdd" runat="server">
                                                                            <div style="background: #316b9d; width: 100%;">
                                                                                <ul class="lnklist-header lnklist-panel" style="height: 31px">
                                                                                    <li>
                                                                                        <a onclick='return AddTicketClick(this)' id="imgAddTicket">
                                                                                            <img style="width: 15px" src="images/arrow_down.png" />
                                                                                            <span style="color: #fff; font-weight: bold">Add New Ticket </span></a>
                                                                                    </li>
                                                                                    <li id="liSaveticket" style="float: right; display: none">
                                                                                        <asp:LinkButton CssClass="icon-save" ID="lnkSaveTicket"
                                                                                            OnClick="lnkSaveTicket_Click" ValidationGroup="ticket" ToolTip="Save" runat="server"></asp:LinkButton>
                                                                                    </li>
                                                                                </ul>
                                                                            </div>
                                                                            <div id="divAddTicket" class="col s12" style="border: 1px solid #316b9d; display: none">
                                                                                <div class="form-section3half">
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <div class="fc-label">
                                                                                            </div>
                                                                                            <div class="fc-input">
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label for="txtSchDate">Schedule Date</label>
                                                                                            <asp:RequiredFieldValidator ValidationGroup="ticket" ID="RequiredFieldValidator24" runat="server"
                                                                                                ControlToValidate="txtSchDate" Display="None" ErrorMessage="Date Scheduled Required"
                                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender
                                                                                                ID="RequiredFieldValidator24_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                PopupPosition="Right" TargetControlID="RequiredFieldValidator24">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                            <asp:TextBox ID="txtSchDate" runat="server"></asp:TextBox>
                                                                                            <asp:CalendarExtender ID="txtSchDt_CalendarExtender" runat="server" Enabled="True"
                                                                                                TargetControlID="txtSchDate">
                                                                                            </asp:CalendarExtender>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label for="txtSchTime">Schedule Time</label>
                                                                                            <asp:TextBox ID="txtSchTime" Class="form-control timepicker" runat="server" MaxLength="28" Enabled="true"></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtSchTime" ValidationGroup="ticket" runat="server" ControlToValidate="txtSchTime" Display="None" EmptyValueMessage="Time is required" ErrorMessage="Time is Required" InvalidValueMessage="Time is invalid" IsValidEmpty="False" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3"
                                                                                                runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator_txtSchTime">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label for="txtEST">EST</label>
                                                                                            <asp:TextBox ID="txtEST" runat="server">0</asp:TextBox>
                                                                                            <asp:MaskedEditExtender ID="txtEST_MaskedEditExtender" runat="server" Mask="99.99"
                                                                                                MaskType="Number" TargetControlID="txtEST" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                                                CultureTimePlaceholder="" Enabled="false">
                                                                                            </asp:MaskedEditExtender>
                                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                                                Enabled="true" ValidChars="0.123456789" TargetControlID="txtEST">
                                                                                            </asp:FilteredTextBoxExtender>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label for="txtDays">Days</label>
                                                                                            <asp:TextBox ID="txtDays" Text="1" TextMode="Number" runat="server"></asp:TextBox>
                                                                                            <asp:FilteredTextBoxExtender ID="txtDays_FilteredTextBoxExtender" runat="server"
                                                                                                Enabled="true" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtDays">
                                                                                            </asp:FilteredTextBoxExtender>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label class="drpdwn-label">Category</label>
                                                                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="browser-default">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ValidationGroup="ticket" ID="RequiredFieldValidator20" runat="server"
                                                                                                ControlToValidate="ddlCategory" Display="None" ErrorMessage="Category Required"
                                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender PopupPosition="TopLeft"
                                                                                                    ID="RequiredFieldValidator20_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                    TargetControlID="RequiredFieldValidator20">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label for="txtUnit">Equipment</label>
                                                                                            <asp:TextBox ID="txtUnit" onclick="$('#divEquip').slideToggle();" runat="server" Style="position: relative; z-index: 100"
                                                                                                autocomplete="off" TextMode="MultiLine"
                                                                                                TabIndex="20" CssClass="materialize-textarea"></asp:TextBox>
                                                                                            <asp:Button ID="btnEquip" CausesValidation="False" Style="display: none" runat="server" Text="Button" />
                                                                                            <div id="divEquip" class="menu_popup_chklst shadow" style="width: 700px">
                                                                                                <telerik:RadGrid ID="gvEquip" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                                                    DataKeyNames="ID" PageSize="20">
                                                                                                    <CommandItemStyle />
                                                                                                    <HeaderStyle Font-Bold="true" />
                                                                                                    <ItemStyle BackColor="WhiteSmoke" />
                                                                                                    <AlternatingItemStyle BackColor="White" />
                                                                                                    <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                                        <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                                                    </ClientSettings>
                                                                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                                        <Columns>
                                                                                                            <telerik:GridTemplateColumn>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblID" runat="server" Style="display: none;" Text='<%# Bind("id") %>'></asp:Label>
                                                                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderTemplate>
                                                                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemStyle Width="0px" />
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                            <telerik:GridTemplateColumn HeaderText="Name" SortExpression="unit">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("unit") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                            <telerik:GridTemplateColumn HeaderText="Unique #" SortExpression="state">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblUID" runat="server"><%#Eval("state")%></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                            <telerik:GridTemplateColumn HeaderText="Description" SortExpression="fdesc">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                            <telerik:GridTemplateColumn HeaderText="Type" SortExpression="Type">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblType" runat="server"><%#Eval("Type")%></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                            <telerik:GridTemplateColumn HeaderText="Category" SortExpression="category">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblcat" runat="server"><%#Eval("category")%></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                            <telerik:GridTemplateColumn HeaderText="Service type" SortExpression="cat">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblServiceType" runat="server"><%#Eval("cat")%></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                            <telerik:GridTemplateColumn HeaderText="Status" SortExpression="status">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                            <telerik:GridTemplateColumn HeaderText="% Hours">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:TextBox ID="txtHours" runat="server" Width="50px" MaxLength="20"></asp:TextBox>
                                                                                                                </ItemTemplate>
                                                                                                            </telerik:GridTemplateColumn>
                                                                                                        </Columns>
                                                                                                    </MasterTableView>
                                                                                                </telerik:RadGrid>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="form-section3half-blank">
                                                                                    &nbsp;
                                                                                </div>
                                                                                <div class="form-section3half">
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <div class="fc-label">
                                                                                            </div>
                                                                                            <div class="fc-input">
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label for="txtWorkers">Worker</label>
                                                                                            <asp:RequiredFieldValidator Enabled="false" ValidationGroup="ticket" ID="RequiredFieldValidator1" runat="server"
                                                                                                ControlToValidate="txtWorkers" Display="None" ErrorMessage="Worker Required" SetFocusOnError="True">
                                                                                            </asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                                                                                PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator1">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                            <div id="divCategoryContainer">
                                                                                                <asp:TextBox ID="txtWorkers" onfocus="WorkersMenu(this);" runat="server" TextMode="MultiLine" CssClass="materialize-textarea"></asp:TextBox>
                                                                                                <div class="menu_popup_chklst shadow" id="divCatSearch" style="background-color: #fff; max-height: 200px;">
                                                                                                    <asp:TextBox ID="txtWorkerSearch" onkeyup="SearchWorker(this);" runat="server" placeholder="Search"></asp:TextBox>
                                                                                                </div>
                                                                                                <div class="menu_popup_chklst shadow" id="divCat" style="background-color: #fff; max-height: 200px; margin-top: 30px">
                                                                                                    <asp:CheckBoxList ID="chkcatlist" CssClass="chklist table table-bordered table-striped table-condensed flip-content browser-default" runat="server">
                                                                                                    </asp:CheckBoxList>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label for="txtReason">Work Description</label>
                                                                                            <asp:TextBox ID="txtReason" TextMode="MultiLine" Height="64px" runat="server" CssClass="materialize-textarea"></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ValidationGroup="ticket" ID="RequiredFieldValidator30" runat="server" ControlToValidate="txtReason"
                                                                                                Display="None" ErrorMessage="Reason for Service Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator30_ValidatorCalloutExtender"
                                                                                                runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator30">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtCreditReason">Dispatch Alert</label>
                                                                                                <asp:TextBox ID="txtCreditReason" TextMode="MultiLine" Height="64px" runat="server" CssClass="materialize-textarea" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <div class="checkrow">
                                                                                                    <asp:CheckBox ID="chkDispAlert" Text="Dispatch Alert" runat="server" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <div class="checkrow">
                                                                                                    <asp:CheckBox ID="chkHold" Text="Hold" runat="server" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <div id="divTicketList" style="margin-top: 20px">
                                                                <h3>Tickets</h3>
                                                                <ul>
                                                                    <li>
                                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default" AutoPostBack="true"
                                                                            TabIndex="14" Width="200px" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" CausesValidation="false">
                                                                            <asp:ListItem Value="-1">All</asp:ListItem>
                                                                            <asp:ListItem Value="-2">All Open</asp:ListItem>
                                                                            <asp:ListItem Value="0">Un-Assigned</asp:ListItem>
                                                                            <asp:ListItem Value="1">Assigned</asp:ListItem>
                                                                            <asp:ListItem Value="2">Enroute</asp:ListItem>
                                                                            <asp:ListItem Value="3">Onsite</asp:ListItem>
                                                                            <asp:ListItem Value="4">Completed</asp:ListItem>
                                                                            <asp:ListItem Value="5">Hold</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </li>
                                                                    <li>
                                                                        <asp:Label ID="lblTicketCount" runat="server" Style="font-style: italic;"></asp:Label>
                                                                    </li>
                                                                </ul>
                                                                <asp:Panel runat="server" ID="pnlTicketButtons" Style="background: #316b9d; width: 100%;" Visible="false">
                                                                    <ul class="lnklist-header lnklist-panel">
                                                                        <li>
                                                                            <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" OnClick="lnkDelete_Click" ToolTip="Delete" CssClass="icon-delete"
                                                                                OnClientClick="return DeleteAlert();" TabIndex="23"></asp:LinkButton>
                                                                        </li>
                                                                    </ul>
                                                                </asp:Panel>
                                                                <telerik:RadGrid ID="gvTickets" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                                                    OnSorting="gvTickets_Sorting"
                                                                    CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                    Width="100%" EmptyDataText="No tickets to display" PageSize="5" AllowPaging="false" ShowFooter="True" ShowHeaderWhenEmpty="true"
                                                                    OnDataBound="gvOpenCalls_DataBound" OnRowCommand="gvOpenCalls_RowCommand">
                                                                    <CommandItemStyle />
                                                                    <HeaderStyle Font-Bold="true" />
                                                                    <ItemStyle BackColor="WhiteSmoke" />
                                                                    <AlternatingItemStyle BackColor="White" />
                                                                    <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                        <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                    </ClientSettings>
                                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                        <Columns>
                                                                            <telerik:GridTemplateColumn>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Hold">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkHold" Enabled="false" Checked='<%# (Eval("assigned").ToString()=="5")?true:false %>'
                                                                                        runat="server" />
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Ticket #" SortExpression="id">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTicketId" Style="display: none" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                                    <asp:HyperLink ID="HyperLink1" OnClick='return EditTicketClick(this)' runat="server" Text='<%# Bind("id") %>' Target="_blank" NavigateUrl='<%# "addticket.aspx?id=" +Eval("id") +"&comp="+ Eval("comp")+"&pop=1" %>'></asp:HyperLink>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Assigned to" SortExpression="dwork">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblAssdTo" runat="server" Text='<%# Bind("dwork") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Category" SortExpression="cat">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("cat") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Work Complete Desc." ItemStyle-Width="200px">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDesc" Width="200px" Style="word-wrap: break-word;" runat="server" Text='<%# Eval("description")%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Status" SortExpression="assignname">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("assignname") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Schedule Date" SortExpression="edate">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbledate" runat="server" Text='<%# Eval("edate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="EST" SortExpression="est">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblEst" runat="server" Text='<%# Eval("est") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblESTFooter" runat="server"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Total Time" SortExpression="Tottime">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTot" runat="server" Text='<%# Eval("Tottime") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblTotalFooter" runat="server"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="RT" SortExpression="reg">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblRT" runat="server" Text='<%# Eval("reg") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblRTFooter" runat="server"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="OT" SortExpression="OT">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblOT" runat="server" Text='<%# Eval("OT") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblOTFooter" runat="server"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="NT" SortExpression="NT">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblNT" runat="server" Text='<%# Eval("NT") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblNTFooter" runat="server"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="DT" SortExpression="DT">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDT" runat="server" Text='<%# Eval("DT") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblDTFooter" runat="server"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="TT" SortExpression="TT">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTT" runat="server" Text='<%# Eval("TT") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblTTFooter" runat="server"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Labor Expenses" SortExpression="laborexp" ItemStyle-HorizontalAlign="Right">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLAbExpense" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "laborexp", "{0:c}")%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblLabExpenseFooter" runat="server"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Expenses" SortExpression="expenses">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblExpense" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "expenses", "{0:c}")%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblExpenseFooter" runat="server"></asp:Label>
                                                                                </FooterTemplate>
                                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                            </telerik:GridTemplateColumn>
                                                                        </Columns>
                                                                        <PagerTemplate>
                                                                            <div>
                                                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First" CausesValidation="false" ImageUrl="images/first.png" />
                                                                                &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev" CausesValidation="false"
                                                                                    ImageUrl="~/images/Backward.png" />
                                                                                &nbsp &nbsp <span>Page</span>
                                                                                <asp:DropDownList CssClass="browser-default" ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPagesOpenCall_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <span>of </span>
                                                                                <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                                                &nbsp &nbsp
                                                                        <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument="Next" CausesValidation="false" ImageUrl="images/Forward.png" />
                                                                                &nbsp &nbsp
                                                                        <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument="Last" CausesValidation="false" ImageUrl="images/last.png" />
                                                                            </div>
                                                                        </PagerTemplate>
                                                                    </MasterTableView>
                                                                </telerik:RadGrid>
                                                                <asp:HiddenField runat="server" ID="hdTicketListOrderBy" Value=" ASC" />
                                                                <br />
                                                                <div>
                                                                    <asp:Repeater ID="rptTicketsPager" runat="server">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkTicketPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>' Enabled='<%# Eval("Enabled") %>' OnClick="lnkTicketPage_Click"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Always">
                                                            <ContentTemplate>
                                                                <asp:PlaceHolder ID="PlaceHolderTicket" runat="server"></asp:PlaceHolder>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                    </div>
                                </li>
                                <li id="tbpnlTicketTask" runat="server">
                                    <div id="accrdTicketTask" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-cached"></i>Ticket Task</div>
                                    <div class="collapsible-body">
                                        <div class="form-content-wrap form-content-wrapwd">
                                            <div class="form-content-pd">
                                                <div class="form-section-row">
                                                    <div class="grid_container">
                                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                            <asp:Repeater ID="rptTicketTask" runat="server" OnItemDataBound="rptTicketTask_ItemDataBound">
                                                                <HeaderTemplate>
                                                                    <table class="table table-bordered table-striped table-condensed flip-content">
                                                                        <tr>
                                                                            <th>Ticket #</th>
                                                                            <th>Assigned to</th>
                                                                            <th>Category</th>
                                                                            <th>Status</th>
                                                                            <th>Schedule Date</th>
                                                                        </tr>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblAssdTo" runat="server" Text='<%# Bind("dwork") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("cat") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("assignname") %>'></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lbledate" runat="server" Text='<%# Eval("edate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="5">
                                                                            <div id="divCustom">
                                                                                <asp:Repeater ID="rtTasks" runat="server">
                                                                                    <HeaderTemplate>
                                                                                        <table style="width: 400px; margin-left: 170px" class="table table-bordered table-condensed flip-content">
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label4" runat="server" Text='<%# Container.ItemIndex + 1 %>'></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("task_code") %>'></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        </table>
                                                                                    </FooterTemplate>
                                                                                </asp:Repeater>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    </table>
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                            <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnlCodes" runat="server" Width="100%">
                                                                        <div style="background: #316b9d; width: 100%; border: 1px solid #316b9d; height: 43px">
                                                                            <ul class="lnklist-header lnklist-panel" style="height: 31px">
                                                                                <li>
                                                                                    <span style="color: #fff; font-size: 20px;">Resolution Tasks </span>
                                                                                </li>
                                                                                <li style="float: right">
                                                                                    <ul>
                                                                                    </ul>
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                        <div style="width: 100%; border: 1px solid #316b9d">
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <div style="height: 200px; width: 100%; overflow-y: scroll;">
                                                                                            <asp:Repeater ID="rptCodesList" runat="server">
                                                                                                <HeaderTemplate>
                                                                                                    <table id="tblCodesList" style="width: 100%;" class="table table-bordered table-striped table-condensed flip-content ">
                                                                                                        <th>Code</th>
                                                                                                        <th>Updated</th>
                                                                                                        <th>Ticket#</th>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="chkCode" onchange="checkedHidden(this);" runat="server" Text='<%# Eval("fdesc") %>' />
                                                                                                            <asp:HiddenField ID="hdnCodeCat" runat="server" Value='<%# Eval("category") %>' />
                                                                                                            <asp:HiddenField ID="hdnTicket" runat="server" />
                                                                                                            <asp:HiddenField ID="hdnDate" runat="server" />
                                                                                                            <asp:HiddenField ID="hdnUsername" runat="server" />
                                                                                                            <asp:HiddenField ID="hdnChecked" runat="server" />
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblDesc" runat="server"></asp:Label>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:HyperLink ID="lnkTicket" Target="_blank" runat="server"></asp:HyperLink>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    </table>
                                                                                                </FooterTemplate>
                                                                                            </asp:Repeater>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <div class="col-md-12 col-lg-12">
                                                                <asp:PlaceHolder ID="TaskPlaceholder" runat="server"></asp:PlaceHolder>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                    </div>
                                </li>
                                <li id="tbpnlBom" runat="server">
                                    <div id="accrdBOM" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-cached"></i>BOM</div>
                                    <div class="collapsible-body">
                                        <div class="form-content-wrap form-content-wrapwd">
                                            <div class="form-content-pd">
                                                <div class="form-section-row">
                                                    <div class="grid_container">
                                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                            <asp:Panel ID="Panel5" runat="server">
                                                                <div class="clearfix"></div>
                                                                <div class="col-lg-12 col-md-12">
                                                                    <asp:HiddenField ID="hdnItemJSON" runat="server" />
                                                                    <asp:HiddenField ID="hdnItemTeamJSON" runat="server" />
                                                                    <div class="table-scrollable" style="height: 400px; overflow-y: auto; border: none">
                                                                        <asp:CheckBox ID="chkMaterial" runat="server" Text="Material" CssClass="filled-in" />
                                                                        <asp:HiddenField ID="hdnChkMat" runat="server"></asp:HiddenField>
                                                                        <asp:CheckBox ID="chkLabor" runat="server" Text="Labor" CssClass="filled-in" />
                                                                        <asp:HiddenField ID="hdnChkLb" runat="server"></asp:HiddenField>
                                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                            <ContentTemplate>
                                                                                <telerik:RadGrid ID="gvBOM" runat="server" AutoGenerateColumns="False"
                                                                                    CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                                    PageSize="20" ShowFooter="true" OnItemDataBound="gvBOM_RowDataBound"
                                                                                    OnItemCommand="gvBOM_RowCommand">
                                                                                    <CommandItemStyle />
                                                                                    <HeaderStyle Font-Bold="true" />
                                                                                    <ItemStyle BackColor="WhiteSmoke" />
                                                                                    <AlternatingItemStyle BackColor="White" />
                                                                                    <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                        <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                                    </ClientSettings>
                                                                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                        <Columns>
                                                                                            <telerik:GridTemplateColumn ItemStyle-Width="1%">
                                                                                                <HeaderTemplate>
                                                                                                    <asp:ImageButton ID="ibDeleteBom" runat="server" CausesValidation="false"
                                                                                                        OnClientClick="return CheckDelete('gvBOM.ClientID');"
                                                                                                        OnClick="ibDeleteBom_Click"
                                                                                                        ImageUrl="images/menu_delete.png" Width="13px" />
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:ImageButton ID="imgBtnAdd" runat="server" CommandName="AddProject" CausesValidation="False"
                                                                                                        CommandArgument="<%# ((GridFooterItem) Container).RowIndex %>"
                                                                                                        ImageUrl="~/images/add.png" Width="18px" OnClientClick="return CheckAdd('gvBOM.ClientID');" />
                                                                                                </FooterTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Line No." ItemStyle-Width="1%">
                                                                                                <ItemTemplate>
                                                                                                    <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("JobTItemID") %>'></asp:HiddenField>
                                                                                                    <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' Style="display: none;"></asp:Label>
                                                                                                    <asp:Label ID="lblIndex" runat="server" Text='<%# Container.ItemIndex +1 %>'></asp:Label>
                                                                                                    <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>
                                                                                                    <asp:HiddenField ID="hdnIndex" runat="server" Value='<%# Container.ItemIndex +1 %>'></asp:HiddenField>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Op Sequence" ItemStyle-Width="5%">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtCode" runat="server" Text='<%# Eval("Code") %>' Style="width: 100%!important;" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                                                    <asp:HiddenField ID="hdnCode" runat="server" />
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Type">
                                                                                                <ItemTemplate>
                                                                                                    <asp:HiddenField ID="hdnBType" runat="server" Value='<%# Eval("BType") %>'></asp:HiddenField>
                                                                                                    <asp:DropDownList ID="ddlBType" runat="server" CssClass="browser-default input-sm input-small">
                                                                                                    </asp:DropDownList>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Material Item">
                                                                                                <ItemTemplate>
                                                                                                    <asp:HiddenField ID="hdnMatItem" runat="server" Value='<%# Convert.ToString(Eval("MatItem"))==""?"0":Eval("MatItem") %>'></asp:HiddenField>
                                                                                                    <asp:TextBox ID="txtMatItem" placeholder="Material Item" Text='<%# Eval("MatDesc") %>' runat="server" CssClass="form-control input-sm input-small MatItem-Search"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Description">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtScope" runat="server" Text='<%# Eval("fDesc") %>' CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Qty Required" ItemStyle-Width="5%">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtQtyReq" runat="server" Text='<%# Eval("QtyReq","{0:n}") %>' Style="width: 100px!important; text-align: right;" onchange="calBudgetExt(this)"
                                                                                                        CssClass="form-control input-sm input-small"
                                                                                                        onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="U/M" ItemStyle-Width="5%">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtUM" runat="server" Text='<%# Eval("UM") %>' Style="width: 100px!important;" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                                                    <asp:HiddenField ID="hdnUMID" runat="server" />
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Budget Unit $">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtBudgetUnit" runat="server" Text='<%# Eval("BudgetUnit","{0:n}") %>' onchange="calBudgetExt(this)"
                                                                                                        CssClass="form-control input-sm input-small" Style="text-align: right; width: 100px!important;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Material Mod">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtMatMod" runat="server" Text='<%# Eval("MatMod","{0:n}")%>' CssClass="form-control input-sm input-small"
                                                                                                        onchange="showDecimalVal(this)" Style="text-align: right; width: 100px!important;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Material Ext $">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblMatExt" runat="server" Text='<%# Eval("BudgetExt","{0:n}") %>' Style="text-align: right; width: 100px!important;"></asp:Label>
                                                                                                    <asp:HiddenField ID="hdnMatExt" runat="server" Value='<%# Eval("BudgetExt","{0:0.00}") %>' />
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Vendor">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtVendor" runat="server" Text='<%# Eval("Vendor") %>'
                                                                                                        CssClass="form-control input-sm input-small vendor-search" placeholder="Search by vendor"></asp:TextBox>
                                                                                                    <asp:HiddenField ID="hdnVendorId" runat="server" Value='<%# Eval("VendorId") %>' />
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Labor Item">
                                                                                                <ItemTemplate>
                                                                                                    <asp:DropDownList ID="ddlLabItem" runat="server" DataTextField="LabDesc" DataValueField="LabItem"
                                                                                                        SelectedValue='<%# Eval("LabItem") == DBNull.Value ? "0" : Eval("LabItem") %>' CssClass="form-control input-sm input-small"
                                                                                                        DataSource='<%#dtLab%>'>
                                                                                                    </asp:DropDownList>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Hours">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtHours" runat="server" Text='<%# Eval("LabHours","{0:n}") %>' CssClass="input-sm input-small"
                                                                                                        onchange="calLabExt(this)"
                                                                                                        Style="text-align: right; width: 100px!important;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Rate">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtLabRate" runat="server" Text='<%# Eval("LabRate","{0:n}")  %>' CssClass="form-control input-sm input-small"
                                                                                                        onchange="calLabExt(this)"
                                                                                                        Style="text-align: right; width: 100px!important;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Labor Mod">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtLabMod" runat="server" Text='<%# Eval("LabMod","{0:n}") %>' CssClass="form-control input-sm input-small"
                                                                                                        onchange="showDecimalVal(this)"
                                                                                                        Style="width: 100px!important; text-align: right;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Labor Ext $">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblLabExt" runat="server" Text='<%# Eval("LabExt","{0:n}")  %>' Style="width: 150px!important; text-align: right;"></asp:Label>
                                                                                                    <asp:HiddenField ID="hdnLabExt" runat="server" Value='<%# Eval("LabExt","{0:n}") %>' />
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Total Ext $">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblTotalExt" runat="server" Text='<%# Eval("TotalExt","{0:n}")  %>' Style="width: 150px!important; text-align: right;"></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                            <telerik:GridTemplateColumn HeaderText="Date">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtSDate" runat="server" Text='<%# Eval("SDate")!=DBNull.Value? (!(Eval("SDate").Equals(DateTime.MinValue)) ? (String.Format("{0:MM/dd/yyyy}", Eval("SDate"))) : "" ) : "" %>' CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                                                    <asp:CalendarExtender ID="txtSDate_CalendarExtender" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtSDate">
                                                                                                    </asp:CalendarExtender>
                                                                                                </ItemTemplate>
                                                                                            </telerik:GridTemplateColumn>
                                                                                        </Columns>
                                                                                    </MasterTableView>
                                                                                </telerik:RadGrid>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="gvBOM" EventName="ItemCommand" />
                                                                                <asp:AsyncPostBackTrigger ControlID="lbtnTypeSubmit" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-12 col-lg-12">
                                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Always">
                                                                        <ContentTemplate>
                                                                            <asp:PlaceHolder ID="PlaceHolderBOM" runat="server"></asp:PlaceHolder>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                    </div>
                                </li>
                                <li id="tbpnMilestone" runat="server">
                                    <div id="accrdMilestone" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Billing Details</div>
                                    <div class="collapsible-body">
                                        <div class="form-content-wrap form-content-wrapwd">
                                            <div class="form-content-pd">
                                                <div class="form-section-row">
                                                    <div class="grid_container">
                                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnlBillingDetails" runat="server">
                                                                        <div class="form-section4">
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <div class="checkrow">
                                                                                        <asp:CheckBox ID="chkProgressBilling" Checked="true" runat="server" Text="Progress Billing" OnClick="hideShowBillingDetailsTab(this.checked)" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <label class="drpdwn-label">Posting Method</label>
                                                                                    <asp:DropDownList ID="ddlPostingMethod" CssClass="browser-default" runat="server" onChange="HideShowOnPostingTypeChange(this.value);"></asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-section4-blank">
                                                                            &nbsp;
                                                                        </div>
                                                                        <div class="form-section4">
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <div class="form-col hideShowOnPostingType">
                                                                                        <label for="txtUnrecognizedRevenue">
                                                                                            Unrecognized Revenue   
                                                                                        </label>
                                                                                        <asp:TextBox ID="txtUnrecognizedRevenue" runat="server"
                                                                                            MaxLength="255"></asp:TextBox>
                                                                                        <asp:HiddenField ID="hdnUnrecognizedRevenue" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <div class="form-col hideShowOnPostingType">
                                                                                        <label for="txtUnrecognizedExpense">
                                                                                            Unrecognized Expense   
                                                                                        </label>
                                                                                        <asp:TextBox ID="txtUnrecognizedExpense" runat="server"
                                                                                            MaxLength="255"></asp:TextBox>
                                                                                        <asp:HiddenField ID="hdnUnrecognizedExpense" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <div class="form-col hideShowOnPostingType">
                                                                                        <label for="txtRetainageReceivable">
                                                                                            Retainage Receivable
                                                                                        </label>
                                                                                        <asp:TextBox ID="txtRetainageReceivable" runat="server"
                                                                                            MaxLength="255"></asp:TextBox>
                                                                                        <asp:HiddenField ID="hdnRetainageReceivable" runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div style="float: left; clear: left; width: 100%; padding-top: 15px;">
                                                                            <div class="col s12">
                                                                                <ul class="tabs tab-demo-active white" runat="server" style="width: 100%; z-index: 0;">
                                                                                    <li class="tab col s2" runat="server" id="liBilling_Details">
                                                                                        <a class="white-text waves-effect waves-light active" href="#tbpnlDetails">Details</a>
                                                                                    </li>
                                                                                    <li class="tab col s2" runat="server" id="liBilling_WIP">
                                                                                        <a class="white-text waves-effect waves-light" href="#tbpnlWIP">WIP</a>
                                                                                    </li>
                                                                                    <li class="tab col s2" runat="server" id="liBilling_ProgressBillings">
                                                                                        <a class="white-text waves-effect waves-light" href="#tbpnlProgressBilling">Progress Billings</a>
                                                                                    </li>
                                                                                </ul>
                                                                            </div>
                                                                            <div class="col s12">
                                                                                <asp:Panel ID="tbpnlDetails" runat="server" ClientIDMode="Static" CssClass="col s12 tab-container-border lighten-4" Style="display: block;">
                                                                                    <div class="tabs-custom-mgn1" style="padding-top: 20px;">
                                                                                        <div class="row">
                                                                                            <div class="table-scrollable" style="height: 400px; overflow-y: auto; border: none">
                                                                                                <div class="col-md-12 col-lg-12">
                                                                                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                                                                        <ContentTemplate>
                                                                                                            <telerik:RadGrid ID="gvMilestones" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content" OnItemDataBound="gvMilestones_RowDataBound"
                                                                                                                PageSize="20" ShowFooter="true" OnItemCommand="gvMilestones_RowCommand">
                                                                                                                <CommandItemStyle />
                                                                                                                <HeaderStyle Font-Bold="true" />
                                                                                                                <ItemStyle BackColor="WhiteSmoke" />
                                                                                                                <AlternatingItemStyle BackColor="White" />
                                                                                                                <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                                                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                                                                </ClientSettings>
                                                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                                                    <Columns>
                                                                                                                        <telerik:GridTemplateColumn ItemStyle-Width="1%">
                                                                                                                            <HeaderTemplate>
                                                                                                                                <asp:ImageButton ID="ibDeleteMilestone" runat="server" CausesValidation="false"
                                                                                                                                    OnClientClick="return CheckDelete('gvMilestones.ClientID');"
                                                                                                                                    OnClick="ibDeleteMilestone_Click"
                                                                                                                                    ImageUrl="images/menu_delete.png" Width="13px" />
                                                                                                                            </HeaderTemplate>
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:CheckBox ID="chkSelect" runat="server" ItemStyle-Width="1%" />
                                                                                                                            </ItemTemplate>
                                                                                                                            <FooterTemplate>
                                                                                                                                <asp:ImageButton ID="imgBtnAdd" runat="server" CommandName="AddMilestone" CausesValidation="False"
                                                                                                                                    CommandArgument="<%# ((GridFooterItem) Container).RowIndex %>"
                                                                                                                                    ImageUrl="~/images/add.png" Width="18px" OnClientClick="return CheckAdd('gvMilestones.ClientID');" />
                                                                                                                            </FooterTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn HeaderText="Line No." ItemStyle-Width="1%">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:Label ID="lblIndex" runat="server" Text='<%# Container.ItemIndex +1 %>'></asp:Label>
                                                                                                                                <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' Width="65px" Style="display: none;"></asp:Label>
                                                                                                                                <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>
                                                                                                                                <asp:HiddenField ID="hdnIndex" runat="server" Value='<%# Container.ItemIndex +1 %>'></asp:HiddenField>
                                                                                                                                <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID") %>'></asp:HiddenField>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn HeaderText="Op Sequence" ItemStyle-Width="3%">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:TextBox ID="txtCode" runat="server" Text='<%# Eval("jcode") %>' Style="width: 100%!important;" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                                                                                <asp:HiddenField ID="hdnCode" runat="server" />
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn HeaderText="Type" ItemStyle-Width="15%">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:DropDownList ID="ddlType" runat="server" SelectedValue='<%# Eval("jtype") == DBNull.Value ? 0 : Eval("jtype") %>'
                                                                                                                                    CssClass="form-control input-sm input-small" Style="width: 100%!important;">
                                                                                                                                    <asp:ListItem Value="0">Revenue</asp:ListItem>
                                                                                                                                </asp:DropDownList>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn HeaderText="Function" ItemStyle-Width="10%">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:TextBox ID="txtSType" runat="server" Text='<%# Eval("Department") %>' placeholder="Select Function"
                                                                                                                                    CssClass="form-control input-sm input-small" Style="width: 100%!important;"></asp:TextBox>
                                                                                                                                <asp:HiddenField ID="hdnType" Value='<%# Eval("type") %>' runat="server" />
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn HeaderText="Name" ItemStyle-Width="15%">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:TextBox ID="txtWIPName" runat="server" MaxLength="100" Text='<%# Eval("MilesName") %>' CssClass="form-control input-sm input-small"
                                                                                                                                    Style="width: 100%!important;"></asp:TextBox>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn HeaderText="Description" ItemStyle-Width="15%">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:TextBox ID="txtScope" runat="server" MaxLength="100" Text='<%# Eval("fdesc") %>' CssClass="form-control input-sm input-small"
                                                                                                                                    Style="width: 100%!important;"></asp:TextBox>
                                                                                                                            </ItemTemplate>
                                                                                                                            <FooterTemplate>
                                                                                                                            </FooterTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn HeaderText="Amount" ItemStyle-Width="5%" FooterStyle-HorizontalAlign="Right">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:TextBox ID="txtAmount" runat="server" Text='<%# Eval("Amount","{0:n}") %>' onkeypress="return isDecimalKey(this,event)"
                                                                                                                                    CssClass="form-control input-sm input-small"
                                                                                                                                    onchange="showDecimalVal(this);CalculateRevTotal();" Style="width: 100%!important; text-align: right;"></asp:TextBox>
                                                                                                                            </ItemTemplate>
                                                                                                                            <FooterTemplate>
                                                                                                                                <asp:Label ID="lblTotalRev" runat="server" class="totalrev"></asp:Label>
                                                                                                                            </FooterTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                        <telerik:GridTemplateColumn HeaderText="Required by" ItemStyle-Width="8%">
                                                                                                                            <ItemTemplate>
                                                                                                                                <asp:TextBox ID="txtRequiredBy" runat="server" Text='<%# Eval("RequiredBy")!=DBNull.Value? (!(Eval("RequiredBy").Equals(DateTime.MinValue)) ? (String.Format("{0:MM/dd/yyyy}", Eval("RequiredBy"))) : "" ) : "" %>'
                                                                                                                                    CssClass="form-control input-sm input-small" Style="width: 150px!important;">
                                                                                                                                </asp:TextBox>
                                                                                                                                <asp:CalendarExtender ID="txtRequiredBy_CalendarExtender" runat="server" Enabled="True"
                                                                                                                                    TargetControlID="txtRequiredBy">
                                                                                                                                </asp:CalendarExtender>
                                                                                                                            </ItemTemplate>
                                                                                                                        </telerik:GridTemplateColumn>
                                                                                                                    </Columns>
                                                                                                                </MasterTableView>
                                                                                                            </telerik:RadGrid>
                                                                                                        </ContentTemplate>
                                                                                                        <Triggers>
                                                                                                            <asp:AsyncPostBackTrigger ControlID="gvMilestones" EventName="ItemCommand" />
                                                                                                        </Triggers>
                                                                                                    </asp:UpdatePanel>
                                                                                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Always">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:PlaceHolder ID="PlaceHolderMilestone" runat="server"></asp:PlaceHolder>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="clearfix"></div>
                                                                                        </div>
                                                                                    </div>
                                                                                </asp:Panel>
                                                                                <asp:Panel ID="tbpnlWIP" runat="server" ClientIDMode="Static" CssClass="col s12 tab-container-border lighten-4" Style="display: none;">
                                                                                    <div class="tabs-custom-mgn1" style="padding-top: 20px;">
                                                                                        <div class="form-section-row">
                                                                                            <div class="row">
                                                                                                <div class="col s12 m12 l12">
                                                                                                    <div class="row">
                                                                                                        <div class="form-section-row">
                                                                                                            <div class="form-input-row">
                                                                                                                <div class="form-section3">
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="txtProgressBilling">Progress Billing # </label>
                                                                                                                            <asp:TextBox ID="txtProgressBilling" runat="server" MaxLength="25"></asp:TextBox>
                                                                                                                            <asp:HiddenField ID="hdnWIPID" runat="server" />
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="txtInvoiceNO">Invoice #: </label>
                                                                                                                            <asp:TextBox ID="txtInvoiceNO" runat="server" Enabled="false"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="txtBillingDate">Billing Date </label>
                                                                                                                            <asp:TextBox ID="txtBillingDate" runat="server"></asp:TextBox>
                                                                                                                            <asp:CalendarExtender ID="txtBillingDate_CalendarExtender" runat="server" Enabled="True"
                                                                                                                                TargetControlID="txtBillingDate">
                                                                                                                            </asp:CalendarExtender>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                                <div class="form-section3-blank">
                                                                                                                    &nbsp;
                                                                                                                </div>
                                                                                                                <div class="form-section3">
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label class="drpdwn-label">Terms: </label>
                                                                                                                            <asp:DropDownList ID="ddlTerms" Enabled="true" runat="server" CssClass="browser-default">
                                                                                                                            </asp:DropDownList>
                                                                                                                            <asp:HiddenField ID="hdnGlobal_Terms" runat="server"></asp:HiddenField>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="txtSalesTax">Sales Tax (%):</label>
                                                                                                                            <asp:TextBox ID="txtSalesTax" runat="server"></asp:TextBox>
                                                                                                                            <asp:RegularExpressionValidator ID="NumericOnlyValidator" runat="server" ControlToValidate="txtSalesTax" ErrorMessage="Enter a valid number" ValidationExpression="^[0-9]\d*(\.\d+)?$">
                                                                                                                            </asp:RegularExpressionValidator>
                                                                                                                            <br />
                                                                                                                            <asp:HiddenField ID="hdnGlobal_SalesTax" runat="server"></asp:HiddenField>
                                                                                                                            <asp:Label ID="lblSalesTax" runat="server"></asp:Label>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                                <div class="form-section3-blank">
                                                                                                                    &nbsp;
                                                                                                                </div>
                                                                                                                <div class="form-section3">
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="txtArchitectName">
                                                                                                                                Architect Name:
                                                                                                                            </label>
                                                                                                                            <asp:TextBox ID="txtArchitectName" runat="server"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div class="input-field col s12">
                                                                                                                        <div class="row">
                                                                                                                            <label for="txtArchitectAdress">
                                                                                                                                Architect Address:
                                                                                                                            </label>
                                                                                                                            <asp:TextBox ID="txtArchitectAdress" runat="server" Height="70px" TextMode="MultiLine" CssClass="materialize-textarea"></asp:TextBox>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="row">
                                                                                                        <div class="table-scrollable" style="height: auto; overflow-y: auto;">
                                                                                                            <asp:UpdatePanel ID="upWIP" runat="server">
                                                                                                                <ContentTemplate>
                                                                                                                    <telerik:RadGrid RenderMode="Auto" ID="gvWIPs" AllowFilteringByColumn="false" ShowFooter="true"
                                                                                                                        runat="server" Width="1050" AllowSorting="True" CssClass="WIPGrid" PageSize="200">
                                                                                                                        <CommandItemStyle />
                                                                                                                        <HeaderStyle Font-Bold="true" />
                                                                                                                        <ItemStyle BackColor="WhiteSmoke" />
                                                                                                                        <AlternatingItemStyle BackColor="White" />
                                                                                                                        <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                                                            <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                                                                        </ClientSettings>

                                                                                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                                                            <Columns>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Line No." DataField="Line" UniqueName="Line"
                                                                                                                                    HeaderStyle-Width="7px" ItemStyle-Width="7px" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:Label ID="lblLine" runat="server" Text='<%# Eval("RowNo") %>'></asp:Label>
                                                                                                                                        <%--<asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' Width="10px"></asp:Label>--%>
                                                                                                                                        <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>
                                                                                                                                        <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("Id") %>'></asp:HiddenField>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Description" DataField="WIPDesc" UniqueName="WIPDesc"
                                                                                                                                    HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtWIPDesc" runat="server" MaxLength="200" Text='<%# Eval("WIPDesc") %>' CssClass="textBoxCCS"
                                                                                                                                            Style="width: 70px !important;" BorderWidth="0px"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Contract Amount" DataField="ContractAmount" UniqueName="ContractAmount" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtContractAmount" Width="55px" onchange="showDecimalVal(this);CalculateContractAmountTotal();" runat="server"
                                                                                                                                            Text='<%# Eval("ContractAmount","{0:n}") %>' BorderWidth="0px" CssClass="textBoxCCS DisableControls" Style="text-align: right;">
                                                                                                                                        </asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:Label ID="lblTotalContractAmount" runat="server" class="totalContractAmount"></asp:Label>
                                                                                                                                    </FooterTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Change Order" DataField="ChangeOrder" UniqueName="ChangeOrder" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtChangeOrder" Width="55px" onchange="showDecimalVal(this);CalculateChangeOrderTotal();" runat="server" onkeypress="return isDecimalKey(this,event)"
                                                                                                                                            Text='<%# Eval("ChangeOrder","{0:n}") %>' BorderWidth="0px" CssClass="textBoxCCS" Style="text-align: right;"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:Label ID="lblTotalChangeOrder" runat="server" class="totalChangeOrder"></asp:Label>
                                                                                                                                    </FooterTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Scheduled Values" DataField="ScheduledValues" UniqueName="ScheduledValues" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="68px" ItemStyle-Width="68px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtScheduledValues" Width="65px" onchange="showDecimalVal(this);CalculateScheduledValuesTotal();" runat="server" onkeypress="return isDecimalKey(this,event)"
                                                                                                                                            Text='<%# Eval("ScheduledValues","{0:n}") %>' BorderWidth="0px" CssClass="textBoxCCS DisableControls" Style="text-align: right;"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:Label ID="lblTotalScheduledValues" runat="server" class="totalScheduledValues"></asp:Label>
                                                                                                                                    </FooterTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Previous Billed" DataField="PreviousBilled" UniqueName="PreviousBilled" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="58px" ItemStyle-Width="58px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtPreviousBilled" Width="55px" onchange="showDecimalVal(this);CalculatePreviousBilledTotal();" runat="server" onkeypress="return isDecimalKey(this,event)"
                                                                                                                                            Text='<%# Eval("PreviousBilled","{0:n}") %>' BorderWidth="0px" CssClass="textBoxCCS DisableControls" Style="text-align: right;"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:Label ID="lblTotalPreviousBilled" runat="server" class="totalPreviousBilled"></asp:Label>
                                                                                                                                    </FooterTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Completed this Period" DataField="CompletedThisPeriod" UniqueName="CompletedThisPeriod" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="71px" ItemStyle-Width="71px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtCompletedThisPeriod" Width="68px" onchange="showDecimalVal(this);CalculatCompletedThisPeriodTotal();" runat="server" onkeypress="return isDecimalKey(this,event)"
                                                                                                                                            Text='<%# Eval("CompletedThisPeriod","{0:n}") %>' BorderWidth="0px" CssClass="textBoxCCS" Style="text-align: right;"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:Label ID="lblTotalCompletedThisPeriod" runat="server" class="totalCompletedThisPeriod"></asp:Label>
                                                                                                                                    </FooterTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Presently Stored" DataField="PresentlyStored" UniqueName="PresentlyStored" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="58px" ItemStyle-Width="58px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtPresentlyStored" Width="55px" onchange="showDecimalVal(this);CalculatPresentlyStoredTotal();" runat="server" onkeypress="return isDecimalKey(this,event)"
                                                                                                                                            Text='<%# Eval("PresentlyStored","{0:n}") %>' BorderWidth="0px" CssClass="textBoxCCS" Style="text-align: right;"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:Label ID="lblPresentlyStored" runat="server" class="totalPresentlyStored"></asp:Label>
                                                                                                                                    </FooterTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Total Completed & Stored" DataField="TotalCompletedAndStored" UniqueName="TotalCompletedAndStored" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="71px" ItemStyle-Width="71px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtTotalCompletedAndStored" Width="68px" onchange="showDecimalVal(this);CalculateTotalCompletedAndStoredTotal();" runat="server" onkeypress="return isDecimalKey(this,event)"
                                                                                                                                            Text='<%# Eval("PresentlyStored","{0:n}") %>' BorderWidth="0px" CssClass="textBoxCCS DisableControls" Style="text-align: right;"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:Label ID="lblTotalTotalCompletedAndStored" runat="server" class="totalTotalCompletedAndStored"></asp:Label>
                                                                                                                                    </FooterTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="% Comp." DataField="PerComplete" UniqueName="PerComplete" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="53px" ItemStyle-Width="53px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtPerComplete" Width="50px" onchange="showDecimalVal(this);CalculatePerCompleteTotal();" runat="server" onkeypress="return isDecimalKey(this,event)"
                                                                                                                                            Text='<%# Eval("PerComplete","{0:n}") %>' BorderWidth="0px" CssClass="textBoxCCS" Style="text-align: right;"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:Label ID="lblTotalPerComplete" runat="server" class="totalPerComplete"></asp:Label>
                                                                                                                                    </FooterTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Balance to Finish" DataField="BalanceToFinsh" UniqueName="BalanceToFinsh" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtBalanceToFinsh" Width="77px" onchange="showDecimalVal(this);CalculateBalanceToFinshTotal();" runat="server" onkeypress="return isDecimalKey(this,event)"
                                                                                                                                            Text='<%# Eval("BalanceToFinsh","{0:n}") %>' BorderWidth="0px" CssClass="textBoxCCS DisableControls" Style="text-align: right;"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:Label ID="lblTotalBalanceToFinsh" runat="server" class="totalBalanceToFinsh"></asp:Label>
                                                                                                                                    </FooterTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Retainage %" DataField="RetainagePer" UniqueName="RetainagePer" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="65px" ItemStyle-Width="65px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtRetainagePer" Width="62px" onchange="showDecimalVal(this);CalculateRetainagePerTotal();" runat="server" onkeypress="return isDecimalKey(this,event)"
                                                                                                                                            Text='<%# Eval("RetainagePer","{0:n}") %>' BorderWidth="0px" CssClass="textBoxCCS" Style="text-align: right;"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:Label ID="lblTotalRetainagePer" runat="server" class="totalRetainagePer"></asp:Label>
                                                                                                                                    </FooterTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Retainage Amount" DataField="RetainageAmount" UniqueName="RetainageAmount" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="66px" ItemStyle-Width="66px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtRetainageAmount" Width="63px" onchange="showDecimalVal(this);CalculateRetainageAmountTotal();" runat="server" onkeypress="return isDecimalKey(this,event)"
                                                                                                                                            Text='<%# Eval("RetainageAmount","{0:n}") %>' BorderWidth="0px" CssClass="textBoxCCS" Style="text-align: right;"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:Label ID="lblTotalRetainageAmount" runat="server" class="totalRetainageAmount"></asp:Label>
                                                                                                                                    </FooterTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Total Billed" DataField="TotalBilled" UniqueName="TotalBilled" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="71px" ItemStyle-Width="71px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:TextBox ID="txtTotalBilled" Width="68px" onchange="showDecimalVal(this);CalculateTotalBilledTotal();" runat="server" onkeypress="return isDecimalKey(this,event)"
                                                                                                                                            Text='<%# Eval("TotalBilled","{0:n}") %>' BorderWidth="0px" CssClass="textBoxCCS DisableControls" Style="text-align: right;"></asp:TextBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                    <FooterTemplate>
                                                                                                                                        <asp:Label ID="lblTotalTotalBilled" runat="server" class="totalTotalBilled"></asp:Label>
                                                                                                                                    </FooterTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Billing Code" DataField="TotalBilled" UniqueName="TotalBilled" FooterStyle-HorizontalAlign="Right"
                                                                                                                                    HeaderStyle-Width="71px" ItemStyle-Width="71px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:DropDownList ID="ddlBillingCode" runat="server" CssClass="browser-default" DataTextField="BillType"
                                                                                                                                            DataValueField="ID" SelectedValue='<%# Eval("BillingCode") %>' DataSource='<%#dtBillingCodeData%>'
                                                                                                                                            Width="150px">
                                                                                                                                        </asp:DropDownList>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                                <telerik:GridTemplateColumn HeaderText="Tax" DataField="Taxable" UniqueName="Taxable" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                                                                                                    <ItemTemplate>
                                                                                                                                        <asp:CheckBox ID="chkTaxable" runat="server" Checked='<%# Eval("Taxable") %>'></asp:CheckBox>
                                                                                                                                    </ItemTemplate>
                                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                            </Columns>
                                                                                                                        </MasterTableView>
                                                                                                                        <FooterStyle CssClass="footer" />
                                                                                                                    </telerik:RadGrid>
                                                                                                                </ContentTemplate>
                                                                                                            </asp:UpdatePanel>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="row">
                                                                                                        <div class="form-section-row">
                                                                                                            <div class="form-input-row">
                                                                                                                <div class="row">
                                                                                                                    <asp:Button ID="btnSave" runat="server" Text=" Save WIP " OnClick="SaveWIP_Click"
                                                                                                                        OnClientClick="window.btn_clicked =true;"></asp:Button>
                                                                                                                    <asp:Button ID="btnWIPCancel" runat="server" Text=" Cancel " OnClick="btnWIPCancel_Click"
                                                                                                                        OnClientClick="window.btn_clicked =true;"></asp:Button>
                                                                                                                </div>
                                                                                                            </div>

                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="clearfix"></div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </asp:Panel>
                                                                                <asp:Panel ID="tbpnlProgressBilling" runat="server" ClientIDMode="Static" CssClass="col s12 tab-container-border lighten-4" Style="display: none;">
                                                                                    <div class="tabs-custom-mgn1" style="padding-top: 20px;">
                                                                                        <div class="form-section-row">
                                                                                            <div class="row">
                                                                                                <div class="col s12 m12 l12">
                                                                                                    <div class="row">
                                                                                                        <div style="margin-bottom: 5px; float: left; clear: both;">
                                                                                                            <div class="fc-label" style="float: left;">
                                                                                                                <div class="fc-label" style="float: left;">
                                                                                                                    <asp:Button ID="btnGenerate" OnClick="btnGenerate_Click" CssClass="btn btn-primary" runat="server" Text="Generate" />
                                                                                                                </div>
                                                                                                            </div>
                                                                                                            <div class="fc-label" style="float: left; margin-left: 5px;">
                                                                                                                <div class="fc-label" style="float: left;">
                                                                                                                    <asp:Button ID="btnConfirmEmail" CssClass="btn btn-primary" runat="server" Text="Send Email" OnClick="btnConfirmEmail_Click" />
                                                                                                                </div>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                        <div style="float: left; clear: both;">
                                                                                                            <asp:Label runat="server" ID="lblError" ForeColor="Red"></asp:Label>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                    <div class="row">
                                                                                                        <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none" CausesValidation="False" />
                                                                                                        <asp:Panel ID="pnlPop" runat="server" Style="background-color: #fff; width: 800px; border: 1px solid #316b9d;">
                                                                                                            <asp:Panel ID="pnlGenerateReport" runat="server">
                                                                                                                <div class="title_bar_popup">
                                                                                                                    <asp:Label ID="Label2" runat="server" CssClass="title_text" Style="color: white">Choose</asp:Label>
                                                                                                                    <a onclick="$find('GenerateReportBehavior').hide()" style="cursor: pointer; float: right; color: #fff; margin-left: 10px; height: 16px;">Close</a>
                                                                                                                </div>
                                                                                                                <div class="clearfix"></div>
                                                                                                                <div id="content" style="margin: 10px;">
                                                                                                                    <div class="form-section-row">
                                                                                                                        <div class="form-section3">
                                                                                                                            <div class="input-field col s12">
                                                                                                                                <label>
                                                                                                                                    <asp:CheckBox ID="chkPrintInvoice" Text="Invoices" runat="server" Checked="true" />
                                                                                                                                </label>
                                                                                                                            </div>
                                                                                                                        </div>
                                                                                                                        <div class="form-section3">
                                                                                                                            <div class="input-field col s12">
                                                                                                                                <label>
                                                                                                                                    <asp:CheckBox ID="chkInvoiceWithTicket" runat="server" Checked="true" />
                                                                                                                                </label>
                                                                                                                            </div>
                                                                                                                        </div>
                                                                                                                        <div class="form-section3">
                                                                                                                            <div class="input-field col s12">
                                                                                                                                <label>
                                                                                                                                    <asp:CheckBox ID="chkBillingInvoice" Text="Billing Invoice" runat="server" Checked="true" />
                                                                                                                                </label>
                                                                                                                            </div>
                                                                                                                        </div>
                                                                                                                        <div class="form-section3">
                                                                                                                            <div class="input-field col s12">
                                                                                                                                <label>
                                                                                                                                    <asp:CheckBox ID="chkAIAReport" runat="server" Text="AIA Report" Checked="true" />
                                                                                                                                </label>
                                                                                                                            </div>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div style="clear: both;"></div>
                                                                                                                </div>
                                                                                                                <div id="button_template" style="float: right; margin: 10px;">
                                                                                                                    <asp:Button ID="btnGenerateReport" CssClass="btn btn-primary" runat="server" OnClick="btnGenerateReport_Click" Text="Generate" />
                                                                                                                </div>
                                                                                                                <div id="button_PreviewandEmail" style="float: right; margin: 10px;">
                                                                                                                    <asp:Button ID="btnOpenEmail" CssClass="btn btn-primary" runat="server" Text="Preview & Email" OnClick="btnOpenEmail_Click" />
                                                                                                                </div>
                                                                                                            </asp:Panel>
                                                                                                        </asp:Panel>
                                                                                                        <asp:ModalPopupExtender runat="server" ID="ModalPopupGenerateReport" BehaviorID="GenerateReportBehavior" TargetControlID="hiddenTargetControlForModalPopup"
                                                                                                            PopupControlID="pnlPop" RepositionMode="RepositionOnWindowResizeAndScroll">
                                                                                                        </asp:ModalPopupExtender>
                                                                                                    </div>
                                                                                                    <div class="row">
                                                                                                        <asp:Button runat="server" ID="hiddenTargetControlForOpenEmailModalPopup" Style="display: none" CausesValidation="False" />
                                                                                                        <asp:Panel ID="pnlOpenEmailPop" runat="server" Style="background-color: #fff; width: 800px; border: 1px solid #316b9d;">
                                                                                                            <asp:Panel ID="pnlOpenEmail" runat="server">
                                                                                                                <div class="title_bar_popup">
                                                                                                                    <asp:Label ID="Label5" runat="server" CssClass="title_text" Style="color: white">Email Preview</asp:Label>
                                                                                                                    <a onclick="$find('OpenEmailBehavior').hide()" style="cursor: pointer; float: right; color: #fff; margin-left: 10px; height: 16px;">Close</a>
                                                                                                                    <asp:LinkButton runat="server" ID="btnSendEmail" Text="Send" OnClick="btnSendEmail_Click" Style="float: right; color: #fff; margin-left: 10px;" />
                                                                                                                </div>
                                                                                                                <div class="clearfix"></div>
                                                                                                                <div id="OpenEmailcontent" style="margin: 10px;">
                                                                                                                    <div class="form-section-row">
                                                                                                                        <div class="form-section3">
                                                                                                                            <div class="input-field col s12">
                                                                                                                                <iframe style="width: 780px; height: 500px;" id="myframe" runat="server" src="Invoice.pdf"></iframe>
                                                                                                                            </div>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                    <div style="clear: both;"></div>
                                                                                                                </div>
                                                                                                                <div id="button_OpenEmail" style="float: right; margin: 10px;">
                                                                                                                </div>
                                                                                                            </asp:Panel>
                                                                                                        </asp:Panel>
                                                                                                        <asp:ModalPopupExtender runat="server" ID="ModalPopupOpenEmail" BehaviorID="OpenEmailBehavior" TargetControlID="hiddenTargetControlForOpenEmailModalPopup"
                                                                                                            PopupControlID="pnlOpenEmailPop" RepositionMode="RepositionOnWindowResizeAndScroll">
                                                                                                        </asp:ModalPopupExtender>
                                                                                                    </div>
                                                                                                    <div class="row">
                                                                                                        <asp:Button runat="server" ID="hiddenTargetControlForSendEmailModalPopup" Style="display: none" CausesValidation="False" />
                                                                                                        <asp:Panel ID="pnlSendEmailPop" runat="server" Style="background-color: #fff; width: 600px; border: 1px solid #316b9d;">
                                                                                                            <asp:Panel ID="pnlSendEmail" runat="server">
                                                                                                                <div class="title_bar_popup">
                                                                                                                    <asp:Label ID="Label6" runat="server" CssClass="title_text" Style="color: white">Send Email</asp:Label>
                                                                                                                    <a onclick="$find('SendEmailBehavior').hide()" style="cursor: pointer; float: right; color: #fff; margin-left: 10px; height: 16px;">Close</a>
                                                                                                                    <asp:LinkButton runat="server" ID="btnEmailInvoice" Text="Send" OnClick="btnEmailInvoice_Click"
                                                                                                                        Style="float: right; color: #fff; margin-left: 10px;"
                                                                                                                        ValidationGroup="mail" />
                                                                                                                </div>
                                                                                                                <div class="clearfix"></div>
                                                                                                                <div id="SendEmailcontent" style="margin: 10px;">
                                                                                                                    <table style="width: 100%; height: 430px">
                                                                                                                        <tr>
                                                                                                                            <td>From</td>
                                                                                                                            <td>
                                                                                                                                <asp:TextBox ID="txtFrom" runat="server" Width="400px"></asp:TextBox>
                                                                                                                                <asp:FilteredTextBoxExtender ID="txtFrom_FilteredTextBoxExtender"
                                                                                                                                    runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                                                                                                    TargetControlID="txtFrom">
                                                                                                                                </asp:FilteredTextBoxExtender>
                                                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                                                                                                                    ControlToValidate="txtFrom" Display="None"
                                                                                                                                    ErrorMessage="Invalid E-Mail Address"
                                                                                                                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                                                                                                    ValidationGroup="mail"></asp:RegularExpressionValidator>
                                                                                                                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator3_ValidatorCalloutExtender"
                                                                                                                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator3">
                                                                                                                                </asp:ValidatorCalloutExtender>
                                                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                                                                                    ControlToValidate="txtFrom" Display="None"
                                                                                                                                    ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                                                                                                                    ValidationGroup="mail"></asp:RequiredFieldValidator>
                                                                                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                                                                                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                                                                                                                </asp:ValidatorCalloutExtender>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td>To</td>
                                                                                                                            <td>
                                                                                                                                <asp:TextBox ID="txtTo" runat="server" Width="400px"></asp:TextBox>
                                                                                                                                <asp:FilteredTextBoxExtender ID="txtTo_FilteredTextBoxExtender" runat="server"
                                                                                                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                                                                                                    TargetControlID="txtTo">
                                                                                                                                </asp:FilteredTextBoxExtender>
                                                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                                                                                                                    ControlToValidate="txtTo" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                                                                                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                                                                                                    ValidationGroup="mail"></asp:RegularExpressionValidator>
                                                                                                                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator4_ValidatorCalloutExtender"
                                                                                                                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator4">
                                                                                                                                </asp:ValidatorCalloutExtender>
                                                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                                                                                                    ControlToValidate="txtTo" Display="None"
                                                                                                                                    ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                                                                                                                    ValidationGroup="mail"></asp:RequiredFieldValidator>
                                                                                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender"
                                                                                                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3">
                                                                                                                                </asp:ValidatorCalloutExtender>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td>CC</td>
                                                                                                                            <td>
                                                                                                                                <asp:TextBox ID="txtCC" runat="server" Width="400px"></asp:TextBox>
                                                                                                                                <asp:FilteredTextBoxExtender ID="txtCC_FilteredTextBoxExtender" runat="server"
                                                                                                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                                                                                                    TargetControlID="txtCC">
                                                                                                                                </asp:FilteredTextBoxExtender>
                                                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                                                                                                                                    ControlToValidate="txtCC" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                                                                                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,6})*$"
                                                                                                                                    ValidationGroup="mail"></asp:RegularExpressionValidator>
                                                                                                                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator5_ValidatorCalloutExtender"
                                                                                                                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator5">
                                                                                                                                </asp:ValidatorCalloutExtender>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td>Subject</td>
                                                                                                                            <td>
                                                                                                                                <asp:TextBox ID="txtSubject" runat="server" Width="400px"></asp:TextBox>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td>Attachment</td>
                                                                                                                            <td>
                                                                                                                                <asp:Label Text="Invoice.pdf" runat="server" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td colspan="2">
                                                                                                                                <asp:TextBox ID="txtBody" CssClass="form-control EmailBody" runat="server" TextMode="MultiLine"
                                                                                                                                    Height="200px" Width="400px"></asp:TextBox></td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                    <div style="clear: both;"></div>
                                                                                                                </div>
                                                                                                            </asp:Panel>
                                                                                                        </asp:Panel>
                                                                                                        <asp:ModalPopupExtender runat="server" ID="ModalPopupSendEmail" BehaviorID="SendEmailBehavior" TargetControlID="hiddenTargetControlForSendEmailModalPopup"
                                                                                                            PopupControlID="pnlSendEmailPop" RepositionMode="RepositionOnWindowResizeAndScroll">
                                                                                                        </asp:ModalPopupExtender>
                                                                                                    </div>
                                                                                                    <div class="row">
                                                                                                        <telerik:RadGrid ID="gvProgressBilling" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                                                            Width="100%" OnPreRender="gvProgressBilling_PreRender">
                                                                                                            <CommandItemStyle />
                                                                                                            <HeaderStyle Font-Bold="true" />
                                                                                                            <ItemStyle BackColor="WhiteSmoke" />
                                                                                                            <AlternatingItemStyle BackColor="White" />
                                                                                                            <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                                                                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
                                                                                                                <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                                                                            </ClientSettings>
                                                                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                                                <Columns>
                                                                                                                    <telerik:GridTemplateColumn>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                                                            <asp:HiddenField runat="server" ID="hdID" Value='<%# Eval("id") %>' />
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="ID" Visible="False">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'>
                                                                                                                            </asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:ImageButton ID="ibDeleteWIP" runat="server" CausesValidation="false" OnClick="ibDelete_Click" data-id='<%# Eval("id") %>'
                                                                                                                                ImageUrl="images/menu_delete.png" Width="13px" OnClientClick="return ConfirmDelete(this)" />
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Progress Billing #">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:LinkButton ID="lnkProgressBillingNo" runat="server" Text='<%#Eval("ProgressBillingNo").ToString() != "" ? Eval("ProgressBillingNo") : "Edit" %>' data-id='<%# Eval("id") %>' OnClick="lnkProgressBillingNo_Click" OnClientClick="return MoveTab(this)"></asp:LinkButton>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>

                                                                                                                    <telerik:GridTemplateColumn HeaderText="Date">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblBillingDate" runat="server" Text='<%# Eval("BillingDate","{0:d}") %>'></asp:Label>

                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>

                                                                                                                    <telerik:GridTemplateColumn HeaderText="Status">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:UpdatePanel runat="server" ChildrenAsTriggers="true">
                                                                                                                                <ContentTemplate>
                                                                                                                                    <asp:DropDownList ID="ddlApplicationStatus" AutoPostBack="true" runat="server" data-id='<%# Eval("id") %>'
                                                                                                                                        DataSource='<%#dtApplicationStatus%>' SelectedValue='<%# Eval("ApplicationStatusId") %>' DataTextField="StatusName" DataValueField="Id"
                                                                                                                                        onChange="ConfirmStatusUpdate(this);"
                                                                                                                                        oldIndex='<%# Eval("ApplicationStatusId") %>'
                                                                                                                                        OnSelectedIndexChanged="ddlApplicationStatus_SelectedIndexChanged"
                                                                                                                                        CausesValidation="false"
                                                                                                                                        CssClass="form-control input-sm input-small" Style="width: 100%!important;">
                                                                                                                                    </asp:DropDownList>
                                                                                                                                </ContentTemplate>
                                                                                                                            </asp:UpdatePanel>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Amt">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Submitted Date">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblSubmittedDate" runat="server" Text='<%# Eval("SubmittedDate") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Approved by">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblApprovedby" runat="server" Text='<%# Eval("Approvedby") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Invoice #">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblInvoiceId" Visible="false" runat="server" Text='<%# Eval("InvoiceId") %>'></asp:Label>
                                                                                                                            <asp:HyperLink ID="hlnkInvoice" runat="server" Text='<%# Eval("InvoiceId") %>' Target="_blank" NavigateUrl='<%# "addinvoice.aspx?uid=" +Eval("InvoiceId")  %>'></asp:HyperLink>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Form">
                                                                                                                        <ItemTemplate>
                                                                                                                            <%--<asp:Label ID="lblForm" runat="server"  Text='AIA' ></asp:Label>--%>
                                                                                                                            <asp:LinkButton ID="lnkForm" ClientIDMode="Static" runat="server" Text="AIA" data-id='<%# Eval("InvoiceId") %>' OnClick="lnkForm_Click"></asp:LinkButton>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Send To">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblSendTo" runat="server" Text='<%# Eval("SendTo") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Send By">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblSendBy" runat="server" Text='<%# Eval("SendBy") %>'></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                    </telerik:GridTemplateColumn>
                                                                                                                    <telerik:GridTemplateColumn HeaderText="Send On">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblSendOn" runat="server" Text='<%# Eval("SendOn") %>'></asp:Label>
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
                                                                                </asp:Panel>
                                                                            </div>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <%--<asp:PostBackTrigger ControlID="tbpnlProgressBilling$btnGenerateReport" />--%>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                    </div>
                                </li>
                                <li id="tbpnAttachment" runat="server">
                                    <div id="accrdDocument" class="collapsible-header accrd accordian-text-custom"><i class="mdi-av-my-library-books"></i>Documents</div>
                                    <div class="collapsible-body">
                                        <div class="form-content-wrap form-content-wrapwd">
                                            <div class="form-content-pd">
                                                <div class="form-section-row">
                                                    <asp:UpdatePanel ID="UpdatePanel27" runat="server">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="lnkUploadProjectDoc" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:Panel ID="pnlDocPermission" runat="server">
                                                                <div id="dvAttachment" style="font-weight: bold; font-size: 15px">
                                                                    <table style="width: 100%;">

                                                                        <tr>
                                                                            <td style="width: 20%;">Documents Types: </td>
                                                                            <td style="width: 20%;">
                                                                                <asp:Image ID="Image5" runat="server" Style="float: left; height: 15px; cursor: pointer" Visible="false" />
                                                                                <asp:DropDownList ID="ddlAttachment" runat="server" AutoPostBack="true"
                                                                                    TabIndex="14" Width="200px" OnSelectedIndexChanged="ddlAttachment_SelectedIndexChanged" CausesValidation="false">
                                                                                    <asp:ListItem Value="All">All</asp:ListItem>
                                                                                    <asp:ListItem Value="Project">Project</asp:ListItem>
                                                                                    <asp:ListItem Value="Tickets">Tickets</asp:ListItem>
                                                                                    <asp:ListItem Value="Location">Location</asp:ListItem>
                                                                                    <asp:ListItem Value="Customer">Customer</asp:ListItem>

                                                                                </asp:DropDownList>
                                                                            </td>


                                                                            <td style="width: 10%;">Sort By</td>
                                                                            <td style="width: 20%;">
                                                                                <asp:DropDownList ID="ddlSortAttachment" Width="200px" CausesValidation="false" AutoPostBack="true" OnSelectedIndexChanged="ddlSortAttachment_SelectedIndexChanged" runat="server">
                                                                                    <asp:ListItem Value="2"> Type </asp:ListItem>
                                                                                    <asp:ListItem Value="1"> Filename </asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>


                                                                            <td style="width: 20%;">

                                                                                <asp:LinkButton ID="lnkUploadProjectDoc" runat="server"
                                                                                    CausesValidation="False" OnClick="lnkUploadProjectDoc_Click"
                                                                                    Style="display: none">Upload</asp:LinkButton>

                                                                                <asp:LinkButton ID="lnkProjectPostback" runat="server"
                                                                                    CausesValidation="False" Style="display: none">Postback</asp:LinkButton>

                                                                                <span runat="server" id="fuspan">Upload Document </span></td>
                                                                            <td style="width: 20%;">
                                                                                <asp:FileUpload ID="FU_Project" runat="server" Visible="true"
                                                                                    onchange="AddDocumentClick(this);" />
                                                                            </td>

                                                                        </tr>

                                                                    </table>


                                                                </div>
                                                                <asp:Panel ID="Panel8" runat="server" Style="padding: 10px 10px 10px 10px; min-height: 100px;">
                                                                    <asp:Label ID="Label3" runat="server" Style="font-style: italic; float: right"></asp:Label>
                                                                    <br />
                                                                    <asp:Repeater ID="rptattachmenttype" runat="server" OnItemDataBound="rptattachmenttype_ItemDataBound">
                                                                        <ItemTemplate>
                                                                            <!-- Navigation -->
                                                                            <asp:HiddenField ID="hdntype" runat="server" Value='<%# Eval("Value") %>' />
                                                                            <div>
                                                                                <nav class="navbar navbar-inverse ">
                                                                                    <div class="pager">

                                                                                        <span class="col-xs-4">
                                                                                            <asp:LinkButton ID="lnkprevious" Visible="false" runat="server" Text="Previous" CssClass="pull-leftslider" CommandArgument='<%# Eval("Value") %>' CommandName="Previous"></asp:LinkButton>

                                                                                        </span>

                                                                                        <span class="col-xs-5 title"><%# Eval("Value") %></span><span class="col-xs-3">

                                                                                            <asp:LinkButton ID="lnknext" Visible="false" runat="server" Text="Next" CssClass="pull-right" CommandArgument='<%# Eval("Value") %>' CommandName="Next"></asp:LinkButton>
                                                                                        </span>
                                                                                        <asp:HiddenField ID="hdnpages" runat="server"></asp:HiddenField>
                                                                                        <asp:HiddenField ID="hdnpagescount" runat="server"></asp:HiddenField>

                                                                                    </div>
                                                                                </nav>
                                                                                <asp:Repeater ID="rptattachment" runat="server" OnItemCommand="rptattachment_ItemCommand" OnItemDataBound="rptattachment_ItemDataBound">
                                                                                    <ItemTemplate>
                                                                                        <!-- Page Content -->
                                                                                        <div class="gallery">
                                                                                            <div class="col-lg-3 col-md-3 col-xs-3 thumb">
                                                                                                <a class="thumbnail" style="height: 110px; padding: 5px; display: flex; justify-content: center; align-items: center;"
                                                                                                    <%# (Eval("content").ToString() == "image")? "href='"+ Eval("Value")+"'":"" %>
                                                                                                    <%# (Eval("content").ToString() == "image")?" data-lity ":"" %>>

                                                                                                    <asp:ImageButton ImageUrl='<%#  (Eval("content").ToString() == "image")? Eval("Value").ToString() : Eval("Value").ToString() %>'
                                                                                                        ID="imgattachment" runat="server" CssClass="img-responsive"
                                                                                                        OnClientClick="return ViewIMGClick(this);"
                                                                                                        CommandName="OpenAttachment" CommandArgument='<%# Eval("path") %>'></asp:ImageButton>
                                                                                                </a>
                                                                                                <span>
                                                                                                    <div style="float: left">
                                                                                                        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                                                                                            <ContentTemplate>
                                                                                                                <table>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:CheckBox ID="chkMS" runat="server" onclick="$(this).closest('div').find('.dummybutton').click();"
                                                                                                                                Checked='<%# Convert.ToBoolean( Eval("msvisible")) %>'
                                                                                                                                ToolTip="Show on Mobile Service" /></td>

                                                                                                                        <td>
                                                                                                                            <asp:LinkButton OnClientClick="return DeleteDocumentClick(this);"
                                                                                                                                Visible='<%# Eval("screen").ToString() =="Project"?true:false %>'
                                                                                                                                CommandName="DeleteAttachment" CausesValidation="false" ID="btnDelete" CssClass="btn submit" runat="server"
                                                                                                                                ToolTip="Delete" CommandArgument='<%# Eval("ID") %>'><i class="fa fa-trash-o"   ></i></asp:LinkButton></td>

                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                                <asp:Button ID="btnMS" runat="server" CssClass="dummybutton" Style="display: none" CausesValidation="false" CommandName="UpdateMS" CommandArgument='<%# Eval("ID") %>' />
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </div>
                                                                                                    <div style="float: right">

                                                                                                        <table>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandName="RotatedImgleft"
                                                                                                                        Visible='<%# (Eval("content").ToString() == "image")?true:false %>' ToolTip="Rotate Left" CssClass="btn submit"
                                                                                                                        CausesValidation="false"
                                                                                                                        CommandArgument='<%# Eval("path") %>'>
                                                                                  <i class="fa fa-rotate-left"   ></i>
                                                                                                                    </asp:LinkButton>

                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:LinkButton ID="LinkButton1" ToolTip="Rotate Right" runat="server" CommandName="RotatedImgright"
                                                                                                                        Visible='<%# (Eval("content").ToString() == "image")?true:false %>'
                                                                                                                        CausesValidation="false"
                                                                                                                        CssClass="btn submit"
                                                                                                                        CommandArgument='<%# Eval("path") %>'>
                                                                                  <i class="fa fa-rotate-right"   ></i>
                                                                                                                    </asp:LinkButton></td>
                                                                                                            </tr>

                                                                                                        </table>
                                                                                                    </div>
                                                                                                    <center>   <asp:LinkButton ID="lnkDownload" runat="server" CommandName="OpenAttachment"
                                                                                            OnClientClick="return ViewDocumentClick(this);"
                                                                                            CommandArgument='<%# Eval("path") %>'>
                                                                                    <%# Eval("Text") %>
                                                                                        </asp:LinkButton> </center>
                                                                                                </span>
                                                                                            </div>
                                                                                        </div>
                                                                                        <!-- /.container -->
                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>
                                                                            </div>
                                                                            <div class="clearfix"></div>
                                                                            <hr>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </asp:Panel>
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                    </div>
                                </li>
                                <li id="tbpnContact" runat="server">
                                    <div id="accrdContacts" class="collapsible-header accrd accordian-text-custom"><i class="mdi-social-people"></i>Contacts</div>
                                    <div class="collapsible-body">
                                        <div class="form-content-wrap form-content-wrapwd">
                                            <div class="form-content-pd">
                                                <div class="form-section-row">
                                                    <div class="grid_container">
                                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                            <asp:UpdatePanel ID="UpdatePanel33" runat="server" UpdateMode="Conditional">

                                                                <ContentTemplate>
                                                                    <div>
                                                                        <asp:Panel ID="panel1" runat="server">
                                                                            <div style="background: #316b9d; width: 100%;">

                                                                                <ul class="lnklist-header lnklist-panel" style="height: 31px">

                                                                                    <li>
                                                                                        <asp:Label ID="lblconinfo" ForeColor="White" runat="server"></asp:Label>
                                                                                    </li>

                                                                                    <li>
                                                                                        <asp:LinkButton CssClass="icon-addnew" Style="float: right" OnClientClick="return AddContactClick(this);"
                                                                                            ID="imgAddContact" runat="server" CausesValidation="False" OnClick="imgAddContact_Click"
                                                                                            TabIndex="23" ToolTip="Add"></asp:LinkButton>

                                                                                    </li>
                                                                                    <li>
                                                                                        <asp:LinkButton CssClass="icon-edit" Style="float: right" OnClientClick="return EditContactClick(this);"
                                                                                            ID="btnEditcontact" runat="server" CausesValidation="False" OnClick="btnEditcontact_Click"
                                                                                            TabIndex="23" ToolTip="Edit"></asp:LinkButton>


                                                                                    </li>
                                                                                    <li style="float: right;">
                                                                                        <asp:LinkButton CssClass="icon-closed" CausesValidation="False" Visible="false" ID="lnkContactClose" OnClick="LinkButton4_Click"
                                                                                            ToolTip="Close" runat="server"></asp:LinkButton>
                                                                                    </li>
                                                                                    <li style="float: right;">
                                                                                        <asp:LinkButton CssClass="icon-save" Visible="false" ID="lnkContactSave" OnClick="lnkContactSave_Click"
                                                                                            ValidationGroup="Contact" ToolTip="Save" runat="server"></asp:LinkButton>
                                                                                    </li>
                                                                                </ul>
                                                                            </div>
                                                                            <div id="divAddContact" runat="server" visible="false" class="col-lg-12 col-md-12" style="border: 1px solid #316b9d;">
                                                                                <div class="col-md-6 col-lg-6">
                                                                                    <div class="form-group">
                                                                                        <div class="form-col">
                                                                                            <div class="fc-label">
                                                                                            </div>
                                                                                            <div class="fc-input">
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-group">
                                                                                        <div class="form-col">
                                                                                            <div class="fc-label">
                                                                                                Contact Name 
                                                                                            </div>
                                                                                            <div class="fc-input">
                                                                                                <asp:TextBox ID="txtContcName" autocomplete="off" runat="server" CssClass="form-control Contact-search" MaxLength="50"></asp:TextBox>
                                                                                                <asp:HiddenField ID="hdncontactID" Value="0" runat="server" />
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtContcName"
                                                                                                    Display="None" ErrorMessage="Contact Name Required" SetFocusOnError="True" ValidationGroup="Contact"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                                                                                    runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator12">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-group">
                                                                                        <div class="form-col">
                                                                                            <div class="fc-label">
                                                                                                Title
                                                                                            </div>
                                                                                            <div class="fc-input  timepicker">
                                                                                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="50"></asp:TextBox>
                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtContEmail"
                                                                                                    Display="None" ErrorMessage="Invalid Email" ValidationGroup="Contact" SetFocusOnError="True"
                                                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="r45345" PopupPosition="Left"
                                                                                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-group">
                                                                                        <div class="form-col">
                                                                                            <div class="fc-label">
                                                                                                Phone
                                                                                            </div>
                                                                                            <div class="fc-input">
                                                                                                <asp:TextBox ID="txtContPhone" runat="server" MaxLength="22" placeholder="(xxx)xxx-xxxx Ext:xxx"></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-group">
                                                                                        <div class="form-col">
                                                                                            <div class="fc-label">
                                                                                                Fax
                                                                                            </div>
                                                                                            <div class="fc-input">
                                                                                                <asp:TextBox ID="txtContFax" runat="server" MaxLength="22"></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>


                                                                                </div>
                                                                                <div class="col-md-6 col-lg-6">
                                                                                    <div class="form-group">
                                                                                        <div class="form-col">
                                                                                            <div class="fc-label">
                                                                                            </div>
                                                                                            <div class="fc-input">
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>

                                                                                    <div class="form-group">
                                                                                        <div class="form-col">
                                                                                            <div class="fc-label">
                                                                                                Cell
                                                                                            </div>
                                                                                            <div class="fc-input">
                                                                                                <asp:TextBox ID="txtContCell" runat="server" MaxLength="22" placeholder="(xxx)xxx-xxxx"></asp:TextBox>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-group">
                                                                                        <div class="form-col">
                                                                                            <div class="fc-label">
                                                                                                Email
                                                                                
                                                                                            </div>
                                                                                            <div class="fc-input">
                                                                                                <asp:TextBox ID="txtContEmail" runat="server" MaxLength="50"></asp:TextBox>
                                                                                            </div>

                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-group">
                                                                                        <div class="form-col">
                                                                                            <div class="fc-label">
                                                                                                Contact Type
                                                                                            </div>
                                                                                            <div class="fc-input">
                                                                                                <asp:DropDownList ID="ddlContactType" runat="server" Width="100%">
                                                                                                    <asp:ListItem Text="Location" Value="1"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Customer" Value="2"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-group">
                                                                                        <div class="form-col">
                                                                                            <div class="fc-label">
                                                                                                Email Ticket
                                                                                            </div>
                                                                                            <div class="fc-input">
                                                                                                <asp:CheckBox ID="chkEmailTicket" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>


                                                                                </div>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <asp:UpdatePanel ID="UpdatePanel30" runat="server">
                                                                <Triggers>
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <div class="col-md-12 col-lg-12">
                                                                        <div class="row">
                                                                            <div class="table-scrollable" style="border: none">
                                                                                <div style="padding-bottom: 15px">
                                                                                    <telerik:RadGrid ID="gvContacts" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                                        Width="100%" EmptyDataText="Record Not Found">
                                                                                        <AlternatingItemStyle CssClass="oddrowcolor" />
                                                                                        <SelectedItemStyle CssClass="selectedrowcolor" />
                                                                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="false">
                                                                                            <Columns>
                                                                                                <telerik:GridTemplateColumn HeaderText="Edit">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="chkEditcontact" TabIndex="19" onclick="CheckSingleCheckbox(this)" runat="server" />
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="chkBoxIsRecd" runat="server" Checked='<%# Eval("IsHighLighted").ToString()=="0"? false:true %>' OnCheckedChanged="chkBoxIsRecd_OnCheckedChanged" AutoPostBack="true" />
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn HeaderText="IsHighLighted" Visible="False">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="IsHighLighted" runat="server" Text='<%# Bind("IsHighLighted") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn HeaderText="PhoneID" Visible="False">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="PhoneID" runat="server" Text='<%# Bind("PhoneID") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn HeaderText="Name">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn HeaderText="Title">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn HeaderText="Phone">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblPhn" runat="server" Text='<%#Eval("Phone")%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn HeaderText="Fax">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblFx" runat="server" Text='<%#Eval("Fax")%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn HeaderText="Cell">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblCell" runat="server" Text='<%#Eval("Cell")%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn HeaderText="Email">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblEmail" runat="server" Text='<%#Eval("Email")%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn HeaderText="Email Ticket" Visible="false">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblEmailTicket" runat="server" Text='<%#Eval("EmailRecTicket")%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn HeaderText="Contact Type">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblContactType" runat="server" Text='<%#Eval("ContactType")%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                </div>
                                                                                <div class="clearfix"></div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                    </div>
                                </li>
                                <li id="TabPanel3" runat="server">
                                    <div id="accrdPlanner" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-contact-cal"></i>Planner</div>
                                    <div class="collapsible-body">
                                        <div class="form-content-wrap form-content-wrapwd">
                                            <div class="form-content-pd">
                                                <div class="form-section-row">
                                                    <div class="srchpaneinner">
                                                        <asp:LinkButton Text="Add Planner" CausesValidation="false" OnClick="lnkAddPlanner_Click" ID="lnkAddPlanner" runat="server" Style="text-decoration: none; font-size: 14px;" />
                                                    </div>
                                                    <div class="grid_container">
                                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                            <asp:Panel ID="PlannerInfo" runat="server" Style="float: left; clear: both; width: 100%">
                                                                <div style="background: #316b9d; width: 100%;">
                                                                    <ul class="lnklist-header lnklist-panel" style="height: 31px">
                                                                        <li>
                                                                            <a onclick='return AddPlannerClick(this)' runat="server" id="imgAddPlanner">
                                                                                <img style="width: 15px" src="images/arrow_down.png" />
                                                                                <span style="color: #fff; font-weight: bold">Planner Info </span></a>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                                <div id="divAddPlanner" class="col-lg-12 col-md-12" style="border: 1px solid #316b9d; display: block;">
                                                                    <div class="col-lg-12 col-md-12" style="margin-top: 30px!important;">
                                                                        <div class="col-md-4 col-lg-4">
                                                                            <div class="form-group">
                                                                                <div class="form-col">
                                                                                    <div class="fc-label" style="text-align: left;">
                                                                                        Planner ID :- 
                                                                                    </div>
                                                                                    <div class="fc-input" style="margin-top: 5px!important;">
                                                                                        <asp:LinkButton Visible="true" OnClick="lnkPlannerID_Click" runat="server" ID="lnkPlannerID"></asp:LinkButton>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4 col-lg-4">
                                                                            <div class="form-group">
                                                                                <div class="form-col">
                                                                                    <div class="fc-label" style="text-align: left;">
                                                                                        Planner Desc :- 
                                                                                    </div>
                                                                                    <div class="fc-input" style="margin-top: 5px!important;">
                                                                                        <asp:Label runat="server" ID="lblPlannerDescription"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4 col-lg-4">
                                                                            <div class="form-group">
                                                                                <div class="form-col">
                                                                                    <div class="fc-label" style="text-align: left;">
                                                                                        Start Date :-
                                                                                    </div>
                                                                                    <div class="fc-input" style="margin-top: 5px!important;">
                                                                                        <asp:Label runat="server" ID="lblStartDate"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-lg-12 col-md-12">
                                                                        <div class="col-md-4 col-lg-4">
                                                                            <div class="form-group">
                                                                                <div class="form-col">
                                                                                    <div class="fc-label" style="text-align: left;">
                                                                                        Finish Date :-
                                                                                    </div>
                                                                                    <div class="fc-input" style="margin-top: 5px!important;">
                                                                                        <asp:Label runat="server" ID="lblFinishDate"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4 col-lg-4">
                                                                            <div class="form-group">
                                                                                <div class="form-col">
                                                                                    <div class="fc-label" style="text-align: left;">
                                                                                        No. of Tasks :- 
                                                                                    </div>
                                                                                    <div class="fc-input" style="margin-top: 5px!important;">
                                                                                        <asp:Label runat="server" ID="lblNumberofTasks"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4 col-lg-4">
                                                                            <div class="form-group">
                                                                                <div class="form-col">
                                                                                    <div class="fc-label" style="text-align: left;">
                                                                                        Next Due Task :- 
                                                                                    </div>
                                                                                    <div class="fc-input" style="margin-top: 5px!important;">
                                                                                        <asp:Label runat="server" ID="lblNextDueTask"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-lg-12 col-md-12">
                                                                        <div class="col-md-4 col-lg-4">
                                                                            <div class="form-group">
                                                                                <div class="form-col">
                                                                                    <div class="fc-label" style="text-align: left;">
                                                                                        Progress Task :- 
                                                                                    </div>
                                                                                    <div class="fc-input" style="margin-top: 5px!important;">
                                                                                        <asp:Label runat="server" ID="lblInProgressTask"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4 col-lg-4">
                                                                            <div class="form-group">
                                                                                <div class="form-col">
                                                                                    <div class="fc-label" style="text-align: left;">
                                                                                        Total Hours :- 
                                                                                    </div>
                                                                                    <div class="fc-input" style="margin-top: 5px!important;">
                                                                                        <asp:Label runat="server" ID="lblTotalHours"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4 col-lg-4">
                                                                            <div class="form-group">
                                                                                <div class="form-col">
                                                                                    <div class="fc-label" style="text-align: left;">
                                                                                        Total Days :- 
                                                                                    </div>
                                                                                    <div class="fc-input" style="margin-top: 5px!important;">
                                                                                        <asp:Label runat="server" ID="lblTotalDays"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                    <div class="cf"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <a href="#" runat="server" value="Add New" id="btnAddNew"></a><%--onclick="getAccount();"--%>

                    <asp:ModalPopupExtender ID="mpeAddBomType" BackgroundCssClass="ModalPopupBG"
                        runat="server" CancelControlID="btnCancel" OkControlID="btnOkay"
                        TargetControlID="btnAddNew" BehaviorID="programmaticModalPopupBehavior"
                        PopupControlID="pnlAddBomType" Drag="true" PopupDragHandleControlID="PopupHeader" OnOkScript="ReloadPage();">
                    </asp:ModalPopupExtender>

                    <div class="popup_Buttons" style="display: none">
                        <input id="btnOkay" value="Done" type="button" />
                        <input id="btnCancel" value="Cancel" type="button" />
                    </div>

                    <div id="pnlAddBomType" class="table-subcategory" style="display: none;">
                        <div class="popup_Container">
                            <div class="popup_Body">
                                <div class="model-popup-body" style="padding-bottom: 24px;">
                                    <asp:Label CssClass="title_text" Style="float: left; font-size: 13px; font-weight: bold;" ID="lblAddBomType" runat="server">Add BOM Type</asp:Label>

                                    <div style="float: right;">
                                        <asp:LinkButton CssClass="save_button" ID="lbtnTypeSubmit" Style="color: white; padding-right: 10px;" runat="server"
                                            OnClick="lbtnTypeSubmit_Click" TabIndex="38" CausesValidation="true" ValidationGroup="Type">
                                             Save </asp:LinkButton>
                                        <a class="close_button_Form" id="lbtnClose" style="color: white;" onclick="cancel();">Close </a>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-12 col-md-12" style="padding-left: 0px; padding-right: 0px;">
                                <div class="com-cont">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    BOM Type
                                                </div>
                                                <div class="fc-input">
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
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField runat="server" ID="hdnAddeTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeTicket" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnviewTicket" Value="Y" />

    <input id="hdnUnitID" runat="server" type="hidden" />
    <asp:HiddenField runat="server" ID="hdnFinancePermission" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnBOMPermission" Value="YYYY" />
    <asp:HiddenField runat="server" ID="hdnMilestonesPermission" Value="YYYY" />

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteContact" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewContact" Value="Y" />


    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnprojectID" Value="0" />

    <asp:HiddenField ID="hdnMilestone" runat="server" />
    <asp:HiddenField ID="hdnCustomJSON" runat="server" />

</asp:Content>
