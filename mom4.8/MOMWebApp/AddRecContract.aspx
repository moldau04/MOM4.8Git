<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddRecContract" CodeBehind="AddRecContract.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />

    <script type="text/javascript" src="js/jquery.formatCurrency.js"></script>


    <script type="text/javascript">



        $(document).ready(function () {

           


            if ($("#txtUnitExpiration").val() == '') {
                $('#txtUnitExpiration').hide();
            }
            else {
                $('#txtUnitExpiration').show();
            }

            if ($("#txtLastrenew").val() != "") {

                var dt = new Date($("#txtLastrenew").val());

                dt.setMonth(dt.getMonth() + parseInt($("#txtContractLength").val()));

                var formattedDate = dt.format('MM/dd/yyyy');

                $("#txtUnitExpiration").val(formattedDate);
            }

            $("#txtContractLength").change(function () {
                debugger

                if ($("#txtContractLength").val() != "") {

                    var dt = new Date($("#txtLastrenew").val());

                    dt.setMonth(dt.getMonth() + parseInt($("#txtContractLength").val()));

                    var formattedDate = dt.format('MM/dd/yyyy');

                    $("#txtUnitExpiration").val(formattedDate);
                }
                else {
                    $("#txtUnitExpiration").val("");
                }

            });


            var hdnContract = $('#txtContractLength').val();
            $('#ddlExpiration').change(function () {

                if ($('#ddlExpiration').val() == 1) {
                    $('#txtUnitExpiration').show();
                    $('#txtNumFreq').hide();
                    $('#RequiredFieldValidator4').prop('disabled', false);
                    $('#RequiredFieldValidator3').prop('disabled', true);
                    $('#txtContractLength').val(hdnContract);
                }
                else if ($('#ddlExpiration').val() == 2) {
                    $('#txtUnitExpiration').hide();
                    $('#txtNumFreq').show();
                    $('#RequiredFieldValidator4').prop('disabled', true);
                    $('#RequiredFieldValidator3').prop('disabled', false);
                    $('#txtContractLength').val(hdnContract);
                }
                else {
                    $('#txtContractLength').val('999');
                    $('#txtUnitExpiration').hide();
                    $('#txtNumFreq').hide();
                    $('#RequiredFieldValidator4').prop('disabled', false);
                    $('#RequiredFieldValidator3').prop('disabled', false);
                }
            });

            var billing = $("#<%=ddlBilling.ClientID%>").val();
            if (billing == 1) {
                $("#divCentral").show();
            }

            ///////////// Ajax call for customer auto search ////////////////////    s                     
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = document.getElementById('<%=hdnCon.ClientID%>').value;
                this.custID = null;
            }

            $("#<%=txtCustomer.ClientID%>").autocomplete({
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
                            try {

                            } catch (e) {
                                $("#<%=txtCustomer.ClientID%>").val('');
                                $("#<%=hdnPatientId.ClientID%>").val('');
                                $("#<%=txtLocation.ClientID%>").val('');
                                $("#<%=hdnLocId.ClientID%>").val('');
                            }
                            //  alert("Due to unexpected errors we were unable to load customers");
                        }

                    });
                },
                select: function (event, ui) {
                    try {
                        $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                        $("#<%=hdnCustID.ClientID%>").val(ui.item.value);
                        $("#<%=hdnPatientId.ClientID%>").val(ui.item.value);
                        $("#<%=txtLocation.ClientID%>").focus();
                        $("#<%=txtLocation.ClientID%>").val('');
                        $("#<%=hdnLocId.ClientID%>").val('');
                        document.getElementById('<%=btnSelectCustomer.ClientID%>').click();
                    } catch (e) { }
                    return false;
                },
                focus: function (event, ui) {
                    try {
                        $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                    } catch (e) { }
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.label;
                    var result_desc = item.desc;
                    try {
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });
                        }
                    } catch (e) { }
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                        .appendTo(ul);
                };


            ///////////// Ajax call for location auto search ////////////////////
            var queryloc = "";
            $("#<%=txtLocation.ClientID%>").autocomplete(
                {
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.custID = 0;
                        try {
                            if (document.getElementById('<%=hdnPatientId.ClientID%>').value != '') {
                                dtaaa.custID = document.getElementById('<%=hdnPatientId.ClientID%>').value;
                            }
                        } catch (e) { }
                        queryloc = request.term;
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
                                try {

                                } catch (e) {
                                    $("#<%=txtLocation.ClientID%>").val('');
                                    $("#<%=hdnLocId.ClientID%>").val('');
                                }
                                //   alert("Due to unexpected errors we were unable to load location");
                            }

                        });

                    },
                    select: function (event, ui) {
                        try {
                            $("#<%=txtLocation.ClientID%>").val(ui.item.label);
                            $("#<%=hdnLocId.ClientID%>").val(ui.item.value);
                            document.getElementById('<%=btnSelectLoc.ClientID%>').click();
                        } catch (e) { }
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%=txtLocation.ClientID%>").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.label;
                    var result_desc = item.desc;
                    var x = new RegExp('\\b' + queryloc, 'ig'); // notice the escape \ here...  

                    try {
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });
                        }
                    } catch (e) { }

                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                        .appendTo(ul);
                };

            ///////////// Validations for auto search ////////////////////

            $("#<%=txtCustomer.ClientID%>").keyup(function (event) {
                try {
                    var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
                    if (document.getElementById('<%=txtCustomer.ClientID%>').value == '') {
                        hdnPatientId.value = '';
                    }
                } catch (e) { }
            });

            $("#<%=txtLocation.ClientID%>").keyup(function (event) {
                try {

                    var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
                    if (document.getElementById('<%=txtLocation.ClientID%>').value == '') {
                        hdnLocId.value = '';
                    }
                } catch (e) { }
            });

            ///////////// Unit dropdown control handling ////////////////////

            <%--$("#<%=txtUnit.ClientID%>").click(function () {
                $("#divEquip").slideToggle();
                return false;
            });--%>


            /////////////// Format textboxes ///////////////////////////////

            $("#<%=txtBillAmt.ClientID%>").blur(function () {
                $("#<%=txtBillAmt.ClientID%>").formatCurrency();
            });

           <%-- $("#<%=txtDay.ClientID%>").keyup(function(event) {
                if (this.value == '') {
                    this.value = '1';
                }
            });--%>




            if ($('#<%=gvEquip.ClientID%>').length > 0) {

                SelectRowsEq('<%=gvEquip.ClientID%>', '<%=txtUnit.ClientID%>', '<%=hdnUnit.ClientID%>');
            }
        });

        function CheckUncheckAllCheckBoxAsNeeded() {

            var totalCheckboxes = $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").size();

            var checkedCheckboxes = $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox:checked").size();

            if (totalCheckboxes == checkedCheckboxes) {

                $('#<%=gvEquip.ClientID %>').find("input[id*='chkAll']:checkbox").each(function () { this.checked = true; });
            }
            else {
                $("#<%=gvEquip.ClientID%> input[id*='chkAll']:checkbox").attr('checked', false);
            }
        }

        function CalculateAmount() {
            //debugger;
            //If Bill Detail Level is Detailed with price T‌hen we calculate Billing amount according to Unit Price. Detailed with price amount is the total of all units price. 
            //In case of summary and Detailed, we can't calculate. In that case, User put manually Billing amount

            var selected = document.getElementById("<%=ddlBillDetailLevel.ClientID%>").value;

            if (selected == "2") {

                var grid = document.getElementById('<%=gvEquip.ClientID%>');
                var txtAmount = document.getElementById('<%=txtBillAmt.ClientID%>');
                var cell;
                var total = 0;

                if (grid.rows.length > 0) {
                    for (i = 1; i < grid.rows.length; i++) {
                        cell = grid.rows[i].cells[10];
                        cell1 = grid.rows[i].cells[0];

                        if (cell1.childNodes[3].checked == true) {
                            $(cell.childNodes[1]).formatCurrency();
                            if (cell.childNodes[1].value != '') {
                                var text = parseFloat(cell.childNodes[1].value.replace("$", "").replace(/,/g, ""));
                                //                        alert(text);                        
                                total = total + text;
                            }
                        }
                    }


                    if (total == 0) {
                        noty({
                            text: 'Please select at least one equipment with a price.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 1000,
                            theme: 'noty_theme_default',
                            closable: true
                        });

                    }
                    txtAmount.value = total.toFixed(2);
                    $("#<%=txtBillAmt.ClientID%>").formatCurrency();

                }

                Materialize.updateTextFields();
            }
        }

        function CalculateHours() {
            //debugger;
            var grid = document.getElementById('<%=gvEquip.ClientID%>');
            var txtHours = document.getElementById('<%=txtBillHours.ClientID%>');
            if (txtHours != null) {
                var cell;
                var total = 0;

                if (grid.rows.length > 0) {
                    for (i = 1; i < grid.rows.length; i++) {
                        cell = grid.rows[i].cells[11];
                        cell1 = grid.rows[i].cells[0];

                        if (cell1.childNodes[3].checked == true) {
                            if (cell.childNodes[1].value != '') {
                                var text = parseFloat(cell.childNodes[1].value);
                                total = total + text;
                            }
                        }
                    }
                    if (total != 0) {
                        txtHours.value = total.toFixed(2);
                    }
                }
            }

            Materialize.updateTextFields();

        }

        function ChkGL(sender, args) {
            //debugger;
            var hdnGLAcct = document.getElementById('<%=hdnGLAcct.ClientID%>');
            if (hdnGLAcct.value == '') {
                args.IsValid = false;
            }
            if (hdnGLAcct.value == '0') {
                args.IsValid = false;
            }
        }

        ///////////// Custom validator function for customer auto search  ////////////////////
        function ChkCustomer(sender, args) {
            var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
            if (hdnPatientId.value == '') {
                args.IsValid = false;
            }
        }

        ///////////// Custom validator function for location auto search  ////////////////////
        function ChkLocation(sender, args) {
            var hdnLocId = document.getElementById('<%=hdnPatientId.ClientID%>');
            if (hdnLocId.value == '') {
                args.IsValid = false;
            }
        }
        ////////////// Display warning message while there is none contract exist /////////////////
        function dispWarningContract() {

            noty({
                text: 'You cannot select the \'Combined on One Invoice\', As there are No Contracts added yet.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }

        function ChkSelectEqup(element, EqupStatus) {
            var EStatus = document.getElementById(EqupStatus).textContent;

            if (EStatus == "Inactive") {
                document.getElementById(element).checked = false;
                noty({ text: 'Equipment is inactive!.', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: false, theme: 'noty_theme_default', closable: true });
            }
            else {

                SelectRowsEq('<%=gvEquip.ClientID%>', '<%=txtUnit.ClientID%>', '<%=hdnUnit.ClientID%>');
                CalculateAmount();
                CalculateHours();
            }

        }

        function onDdlBillingChange(val) {
            if (val == 1) {

                $("#divCentral").show();
                var countLocations = document.getElementById("<%=ddlSpecifiedLocation.ClientID%>").length
                if ((countLocations - 1) <= 0) {

                    noty({
                        text: 'You cannot select the \'Combined Billing\', As there are No Locations added yet.',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 5000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                }

            }
            else {
                $("#divCentral").hide();
            }
        }

        function onDdlBillDetailLevelChange() {

            var selected = document.getElementById("<%=ddlBillDetailLevel.ClientID%>").value;

            if (selected == "2") {

                CalculateAmount();

            }
            else {

                var hdnValue = document.getElementById("<%=hdnBillAmt.ClientID%>").value;

                document.getElementById('<%=txtBillAmt.ClientID %>').value = hdnValue;

            }

        }

        function validateRecContr(val) {
            if (val == 1) {
                noty({
                    text: 'You cannot select the \'Combined on One Invoice\', As there are No Contracts added yet.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
            }
            else if (val == 2) {
                noty({
                    text: 'You cannot select the \'Combined Billing\', As there are No Locations added yet.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
            }
            else if (val == 3) {
                noty({
                    text: 'Please select specify location!',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
            }

        }
    </script>

    <script type="text/javascript">


        $(document).ready(function () {
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
            //// Renewal Notes show hide   

            RenewalNotesShowHide();
            $("#<%= chkRenewalNotes.ClientID %>").click(function (event) {
                RenewalNotesShowHide();
            });

            ///////////// Ajax call for GL acct auto search ////////////////////                
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.Acct = null;
            }

            $("#<%=txtGLAcct.ClientID%>").autocomplete({

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
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load vendor name");
                        }
                    });
                },
                select: function (event, ui) {

                    $("#<%=txtGLAcct.ClientID%>").val(ui.item.label);
                    $("#<%=hdnGLAcct.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtGLAcct.ClientID%>").val(ui.item.label);

                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    //debugger;
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
                            .append("<a>" + result_item + ", <span>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
        });
        ////Function For Renewal Notes show hide

        function RenewalNotesShowHide() {
            if ($("#<%= chkRenewalNotes.ClientID %>").attr("checked") == "checked")
                $(".inst1").show();
            else {
                $(".inst1").hide();

                $("#<%=txtRenewalNotes.ClientID%>").val('');
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
    </script>
 <style>
.left-css{padding-left: 11px;}
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
                                    <div class="page-title">
                                        <i class="mdi-action-trending-up"></i>&nbsp;
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Contract</asp:Label>
                                        <asp:Label CssClass="title_text_Name" ID="lblContrName" runat="server" Style="display: none;"></asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" OnClientClick="return ValidateAmt();" ValidationGroup="rec">Save</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ToolTip="Close" ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>

                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label runat="server" ID="lblHeaderLabel"></asp:Label>
                                        </div>

                                    </div>

                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label runat="server" ID="lblHyperlinklabel"></asp:Label>
                                            <asp:HyperLink ID="lblHeaderLabeldf" runat="server"></asp:HyperLink>
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
                                    <li><a href="#accrdgeneral" class="link-slide">General</a></li>
                                    <li><a href="#accrdcustom" class="link-slide">Custom</a></li>
                                    <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlNext" runat="server" Visible="False">
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False"
                                                OnClick="lnkFirst_Click">  <i class="fa fa-angle-double-left"></i></asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False"
                                                OnClick="lnkPrevious_Click"> <i class="fa fa-angle-left"></i></asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False"
                                                OnClick="lnkNext_Click"> <i class="fa fa-angle-right"></i></asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False"
                                                OnClick="lnkLast_Click"> <i class="fa fa-angle-double-right"></i></asp:LinkButton>
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
        <input id="hdnCon" runat="server" type="hidden" />
        <input id="hdnPatientId" runat="server" type="hidden" />
        <input id="hdnLocId" runat="server" type="hidden" />
        <input id="hdnUnit" runat="server" type="hidden" />
        <input id="hdnContractLength" runat="server" type="hidden" />
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                        <li class="active">
                            <div id="accrdgeneral" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>General</div>
                            <div class="collapsible-body" style="display: block;">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">

                                        <div class="form-section-row">

                                            <div class="section-ttle">Basic Info.</div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtCustomer" ValidationGroup="rec"
                                                            ErrorMessage="Please select the customer" ClientValidationFunction="ChkCustomer"
                                                            Display="None" SetFocusOnError="True"></asp:CustomValidator>
                                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                                            PopupPosition="TopLeft" TargetControlID="CustomValidator1">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtCustomer" ValidationGroup="rec"
                                                            Display="None" ErrorMessage="Please select the customer" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator19_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator19">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:HiddenField ID="hdnCustID" runat="server" />
                                                        <asp:FilteredTextBoxExtender ID="txtCustomer_FilteredTextBoxExtender" runat="server"
                                                            Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtCustomer">
                                                        </asp:FilteredTextBoxExtender>

                                                        <asp:TextBox ID="txtCustomer" onkeydown="return (event.keyCode!=13);" runat="server" CssClass="searchinputloc" autocomplete="nope" placeholder="Search by customer name, phone#, address etc."
                                                            AutoPostBack="false"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtCustomer" ID="lblCustomer">Customer Name <span class="reqd">*</span></asp:Label>

                                                        <asp:Button CausesValidation="false" ID="btnSelectCustomer" runat="server" Text="Button"
                                                            Style="display: none;" OnClick="btnSelectCustomer_Click" />
                                                    </div>
                                                </div>
                                                <div class="srchclr btnlinksicon rowbtn">
                                                    <asp:HyperLink for="txtCustomer" ID="lnkCustomerID" Visible="true" Target="_blank" runat="server"><i class="mdi-social-people" style="margin-left:0px !important;"></i></asp:HyperLink>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">

                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLocation"
                                                            Display="None" ErrorMessage="Location Name Required" SetFocusOnError="True" ValidationGroup="rec"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtLocation"
                                                            ErrorMessage="Please select the location" ClientValidationFunction="ChkLocation"
                                                            Display="None" SetFocusOnError="True" ValidationGroup="rec"></asp:CustomValidator>
                                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                            PopupPosition="Right" TargetControlID="CustomValidator2">
                                                        </asp:ValidatorCalloutExtender>

                                                        <asp:TextBox ID="txtLocation" onkeydown="return (event.keyCode!=13);" runat="server" CssClass="searchinputloc" autocomplete="off" placeholder="Search by location name, phone#, address etc."></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                                                            Enabled="false" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtLocation">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:Label runat="server" AssociatedControlID="txtLocation" ID="lblLocation">Location Name <span class="reqd">*</span></asp:Label>
                                                        <asp:HiddenField ID="hdnLocRolID" runat="server" />
                                                        <asp:Button CausesValidation="false" ID="btnSelectLoc" runat="server" Text="Button"
                                                            Style="display: none;" OnClick="btnSelectLoc_Click" />

                                                    </div>
                                                </div>

                                                <div class="srchclr btnlinksicon rowbtn">
                                                    <asp:HyperLink for="txtLocation" ID="lnkLocationID" Visible="true" Target="_blank" runat="server"><i class="mdi-communication-location-on ml" ></i></asp:HyperLink>
                                                </div>

                                                <div class="srchclr multirowbtn multirowbtnone">
                                                    <img id="imgCreditHold" visible="false" runat="server" title="Credit Hold" src="images/MSCreditHold.png" style="width: 20px;">
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="255"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtAddress" ID="lbltxtAddress">Location Address</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtPO" runat="server" MaxLength="25"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtPO" ID="lblPO"> PO#</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDescription" ValidationGroup="rec"
                                                            Display="None" ErrorMessage="Please enter the description" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7"
                                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator5">
                                                        </asp:ValidatorCalloutExtender>
                                                        <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label3" AssociatedControlID="txtDescription">Contract Description</asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div>
                                                        <label class="drpdwn-label w-100p">
                                                            Equipment
                                                                        <%--<a href="#" style="float: right; padding: 0 3px 0 15px;"><i class="mdi-content-add-box" style="font-size: 2.2em; color: #1565c0; line-height: 24px;"></i></a>--%>
                                                            <%-- <asp:LinkButton OnClick="lblEqGrid_Click" Style="float: right; padding: 0 3px 0 15px;" runat="server" ID="lblEqGrid">
                                                                <i class="mdi-content-add-box" style="font-size: 2.2em; color: #1565c0; line-height: 24px;"></i>
                                                            </asp:LinkButton>--%>
                                                        </label>
                                                    </div>
                                                    <div class="row">

                                                        <div class="tag-div" id="eqtag">
                                                        </div>

                                                        <div id="DivEqup" class="popup_div" style="z-index: 15!important; display: none; width: 800px!important; overflow: auto; max-height: 500px;">

                                                            <div class="grid_container">
                                                                <div class="RadGrid RadGrid_Material RadGrid">
                                                                    <asp:GridView ID="gvEquip" runat="server" AutoGenerateColumns="False"
                                                                        DataKeyNames="ID" OnDataBound="gvEquip_DataBound">
                                                                        <RowStyle CssClass="evenrowcolor" />
                                                                        <Columns>
                                                                            <asp:TemplateField ItemStyle-Width="0px">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblID" Style="display: none;" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                </ItemTemplate>
                                                                                <HeaderTemplate>
                                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                                </HeaderTemplate>
                                                                                <ItemStyle Width="0px"></ItemStyle>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Name" SortExpression="unit">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("unit") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Unique #" SortExpression="state">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUID" runat="server"><%#Eval("state")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Description" SortExpression="fdesc">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Type" SortExpression="Type">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblType" runat="server"><%#Eval("Type")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Category" SortExpression="category">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblcat" runat="server"><%#Eval("category")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Svc. type" SortExpression="cat">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblServiceType" runat="server"><%#Eval("cat")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Building" SortExpression="building">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblbuilding" runat="server"><%#Eval("building")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Shut Down" SortExpression="shut_down">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblShutdown" runat="server"><%#Eval("shut_down")%></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Price">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtPrice" runat="server" Width="90px" MaxLength="20"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Hours">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtHours" runat="server" Width="50px" MaxLength="20"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <asp:TextBox ID="txtUnit" Style="display: none;" runat="server" MaxLength="400" autocomplete="off" onfocus="this.blur();"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Location Type</label>
                                                        <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default" Enabled="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label" id="lblDefaultWorker" runat="server"></label>
                                                        <asp:DropDownList ID="ddlRoute" runat="server" CssClass="browser-default" Enabled="false">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Recurring ticket category</label>
                                                        <asp:DropDownList ID="ddlTicketCat" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtCompany" runat="server" Enabled="false"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtCompany" ID="lbl">Company</asp:Label>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Status</label>
                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default">
                                                            <asp:ListItem Value="0">Active</asp:ListItem>
                                                            <asp:ListItem Value="1">Closed</asp:ListItem>
                                                            <asp:ListItem Value="2">Hold</asp:ListItem>
                                                           
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hdnddlJobStatus" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdnClosePermission" runat="server" Value="Y" />
                                                        <asp:HiddenField ID="hdnCompletedJObPermission" runat="server" Value="Y" />
                                                        <asp:HiddenField ID="hdnJobReopenPermission" runat="server" Value="Y" />
                                                    </div>
                                                </div>
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Service Type</label>
                                                                <asp:DropDownList ID="ddlServiceType" CssClass="browser-default" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlServiceType_SelectedIndexChanged" onKeyUp="this.blur();">
                                                                </asp:DropDownList>

                                                                <asp:RequiredFieldValidator ID="RFVddlServiceType" ValidationGroup="rec"
                                                                    runat="server" ControlToValidate="ddlServiceType"
                                                                    InitialValue=""
                                                                    Display="None" ErrorMessage="Service Type Required"
                                                                    SetFocusOnError="True">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender PopupPosition="BottomLeft"
                                                                    ID="ValidatorCalloutExtender8" runat="server" Enabled="True"
                                                                    TargetControlID="RFVddlServiceType">
                                                                </asp:ValidatorCalloutExtender>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:HiddenField ID="hdnGLAcct" runat="server" />
                                                                <asp:RequiredFieldValidator ID="rfvGLAcct" runat="server" ValidationGroup="rec" ControlToValidate="txtGLAcct"
                                                                    Display="None" ErrorMessage="Please enter GL account" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="vceGLAcct" runat="server"
                                                                    TargetControlID="rfvGLAcct" PopupPosition="Right">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:CustomValidator ID="cvGLAcct" runat="server" ControlToValidate="txtGLAcct" ValidationGroup="rec"
                                                                    ErrorMessage="Please select the GL account" ClientValidationFunction="ChkGL"
                                                                    Display="None" SetFocusOnError="True"></asp:CustomValidator>
                                                                <asp:ValidatorCalloutExtender ID="vceGLAcct1" runat="server" Enabled="True"
                                                                    PopupPosition="TopLeft" TargetControlID="cvGLAcct">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtGLAcct" runat="server" placeholder="Search by acct#, name"></asp:TextBox>
                                                                <asp:Label runat="server" AssociatedControlID="txtGLAcct" ID="lblGL">GL Account</asp:Label>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlServiceType" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <div class="input-field col s12" id="DivTaskCategory" runat="server" visible="false">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Task Category</label>
                                                        <asp:DropDownList ID="ddlCodeCat" Enabled="false" CssClass="browser-default" runat="server" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Salesperson</label>
                                                        <asp:DropDownList ID="ddlTerr" runat="server" CssClass="browser-default" >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Salesperson 2</label>
                                                        <asp:DropDownList ID="ddlTerr2" runat="server" CssClass="browser-default" >
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="form-section-row">

                                            <div class="section-ttle">Billing Recurring</div>
                                            <div class="form-section3">
                                                <div class="input-field2 col s12">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="rfvBillStartDt" ValidationGroup="rec"
                                                            runat="server" ControlToValidate="txtBillStartDt" Display="None" ErrorMessage="Billing Start Date Required"
                                                            SetFocusOnError="True">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender
                                                            ID="rfvBillStartDt_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                            TargetControlID="rfvBillStartDt">
                                                        </asp:ValidatorCalloutExtender>

                                                        <asp:TextBox ID="txtBillStartDt" runat="server" CssClass="datepicker_mom" MaxLength="28"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtBillStartDt" ID="Label4">Billing Start Date</asp:Label>
                                                        <%--  <asp:CalendarExtender ID="txtBillStartDt_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtBillStartDt">
                                                        </asp:CalendarExtender>--%>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Billing Frequency</label>
                                                        <asp:DropDownList ID="ddlBillFreq" runat="server" CssClass="browser-default">
                                                            <asp:ListItem Value="0">Monthly</asp:ListItem>
                                                            <asp:ListItem Value="1">Bi-Monthly</asp:ListItem>
                                                            <asp:ListItem Value="2">Quarterly</asp:ListItem>
                                                            <asp:ListItem Value="3">3 Times/Year</asp:ListItem>
                                                            <asp:ListItem Value="4">Semi-Annually </asp:ListItem>
                                                            <asp:ListItem Value="5">Annually </asp:ListItem>
                                                            <asp:ListItem Value="6">Never </asp:ListItem>
                                                            <asp:ListItem Value="7">3 Years</asp:ListItem>
                                                            <asp:ListItem Value="8">5 Years</asp:ListItem>
                                                            <asp:ListItem Value="9">2 Years</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Bill Detail Level</label>
                                                        <asp:DropDownList ID="ddlBillDetailLevel" runat="server" CssClass="browser-default" onchange="javascript:onDdlBillDetailLevelChange();">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ValidationGroup="rec"
                                                            ControlToValidate="txtBillAmt" Display="None" ErrorMessage="Billing Amount Required"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                ID="RequiredFieldValidator25_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                TargetControlID="RequiredFieldValidator25" PopupPosition="Left">
                                                            </asp:ValidatorCalloutExtender>
                                                        <asp:HiddenField runat="server" ID="hdnBillAmt" />
                                                        <asp:TextBox ID="txtBillAmt" runat="server" MaxLength="20"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtBillAmt" ID="Label5">Billing Amount</asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Customer Recurring Billing</label>
                                                        <asp:DropDownList ID="ddlBilling" CssClass="browser-default" runat="server" onChange="onDdlBillingChange(this.options[this.selectedIndex].value);">
                                                            <asp:ListItem Value="0">Individual</asp:ListItem>
                                                            <asp:ListItem Value="1">Combined</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Location for Customer Combined Billing</label>
                                                        <asp:DropDownList ID="ddlSpecifiedLocation" runat="server" CssClass="browser-default">
                                                        </asp:DropDownList>

                                                        <asp:CustomValidator ID="cvSpecLoc" runat="server" ErrorMessage="Please Specify location"
                                                            OnServerValidate="cvSpecLoc_ServerValidate" SetFocusOnError="true"
                                                            Display="None" ValidationGroup="rec" ControlToValidate="ddlSpecifiedLocation"></asp:CustomValidator>

                                                        <asp:ValidatorCalloutExtender ID="vceSpecLoc" runat="server" Enabled="True"
                                                            TargetControlID="cvSpecLoc" PopupPosition="Right">
                                                        </asp:ValidatorCalloutExtender>

                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Location Recurring Billing</label>
                                                        <asp:DropDownList ID="ddlContractBill" runat="server" CssClass="browser-default" OnSelectedIndexChanged="ddlContractBill_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="card-css">
                                                    <div class="input-field col s12">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkCredit" CssClass="filled-in" runat="server" TabIndex="20" />
                                                            <asp:Label runat="server" ID="lblchkCredit" AssociatedControlID="chkCredit">Credit Card</asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="input-field col s6">
                                                    <div class="checkrow">
                                                        <asp:CheckBox ID="chkspnotes" CssClass="filled-in" runat="server" />
                                                        <asp:Label runat="server" ID="Label15" AssociatedControlID="chkspnotes">Special Notes</asp:Label>
                                                    </div>
                                                </div>

                                                <div class="input-field col s6">
                                                    <div class="row">
                                                        <asp:Label runat="server" ID="Label16" AssociatedControlID="txtSpecialInstructions">Notes</asp:Label>
                                                        <asp:TextBox ID="txtSpecialInstructions" CssClass="materialize-textarea" runat="server" TextMode="MultiLine"></asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="form-section-row">

                                            <div class="section-ttle">Recurring Ticket Schedule</div>
                                            <div class="form-section3">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSchFreq" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <div class="input-field2 col s12" id="Div_ScheStartDate" runat="server">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" ValidationGroup="rec"
                                                                    runat="server" ControlToValidate="txtScheduleStartDt" Display="None" ErrorMessage="Schedule Start Date Required"
                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender
                                                                    ID="RequiredFieldValidator26_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                    TargetControlID="RequiredFieldValidator26" PopupPosition="Left">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtScheduleStartDt" CssClass="datepicker_mom" runat="server" MaxLength="28"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lblSchedule" AssociatedControlID="txtScheduleStartDt">Schedule Start Date</asp:Label>
                                                                <%--  <asp:CalendarExtender ID="txtScheduleStartDt_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtScheduleStartDt">
                                                        </asp:CalendarExtender>--%>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Schedule Frequency</label>
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlSchFreq" AutoPostBack="true" OnSelectedIndexChanged="ddlSchFreq_SelectedIndexChanged" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Value="-1">Never</asp:ListItem>
                                                                    <asp:ListItem Value="0">Monthly</asp:ListItem>
                                                                    <asp:ListItem Value="1">Bi-Monthly</asp:ListItem>
                                                                    <asp:ListItem Value="2">Quarterly</asp:ListItem>
                                                                    <asp:ListItem Value="15">3 Times/Year</asp:ListItem>
                                                                    <asp:ListItem Value="3">Semi-Annually </asp:ListItem>
                                                                    <asp:ListItem Value="4">Annually</asp:ListItem>
                                                                    <asp:ListItem Value="5">Weekly</asp:ListItem>
                                                                    <asp:ListItem Value="6">Bi-Weekly</asp:ListItem>
                                                                    <asp:ListItem Value="7">Every 13 Weeks</asp:ListItem>
                                                                    <asp:ListItem Value="10">Every 2 Years</asp:ListItem>
                                                                    <asp:ListItem Value="8">Every 3 Years</asp:ListItem>
                                                                    <asp:ListItem Value="9">Every 5 Years</asp:ListItem>
                                                                    <asp:ListItem Value="11">Every 7 Years</asp:ListItem>
                                                                    <asp:ListItem Value="12">On-Demand</asp:ListItem>
                                                                    <asp:ListItem Value="13">Daily</asp:ListItem>
                                                                    <asp:ListItem Value="14">Twice a Month</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="weeke-css">
                                                    <div class="input-field col s5">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkWeekends" runat="server" Enabled="true" />
                                                            <asp:Label runat="server" ID="Label12" AssociatedControlID="chkWeekends">Weekends</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s2">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ddlSchFreq" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <div class="input-field col s5" id="Div_ScheduledTime" runat="server">
                                                                <div class="row">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtsTime" ValidationGroup="rec"
                                                                        Display="None" ErrorMessage="Scheduled Time Required" SetFocusOnError="false"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                                                        TargetControlID="RequiredFieldValidator27" PopupPosition="Left">
                                                                    </asp:ValidatorCalloutExtender>

                                                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99:99" MaskType="Time"
                                                                        AcceptAMPM="True" TargetControlID="txtsTime">
                                                                    </asp:MaskedEditExtender>
                                                                    <asp:TextBox ID="txtsTime" runat="server" MaxLength="10"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="Label13" AssociatedControlID="txtsTime">Scheduled Time</asp:Label>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                </div>
                                                <div class="input-field col s12" style="display: none">

                                                    <div class="row">
                                                        <label class="drpdwn-label">Day</label>
                                                        <asp:TextBox ID="txtDay" runat="server" MaxLength="3" TabIndex="15"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtDay_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtDay" ValidChars="1234567890">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:DropDownList ID="ddlDay" CssClass="browser-default" runat="server">
                                                            <asp:ListItem Value="0">Day</asp:ListItem>
                                                            <asp:ListItem Value="1">Monday</asp:ListItem>
                                                            <asp:ListItem Value="2">Tuesday</asp:ListItem>
                                                            <asp:ListItem Value="3">Wednesday</asp:ListItem>
                                                            <asp:ListItem Value="4">Thursday</asp:ListItem>
                                                            <asp:ListItem Value="5">Friday</asp:ListItem>
                                                            <asp:ListItem Value="6">Saturday</asp:ListItem>
                                                            <asp:ListItem Value="7">Sunday</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSchFreq" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <div class="input-field col s12" id="Div_ScheTotalHours" runat="server">

                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBillHours" ValidationGroup="rec"
                                                                    Display="None" ErrorMessage="Total Hours Required" SetFocusOnError="false"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True"
                                                                    TargetControlID="RequiredFieldValidator2" PopupPosition="BottomLeft">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtBillHours" runat="server" MaxLength="20"></asp:TextBox>
                                                                <asp:Label runat="server" ID="Label14" AssociatedControlID="txtBillHours">Total Hours</asp:Label>
                                                            </div>
                                                        </div>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">


                                                <div style="margin-bottom: 50px;">
                                                    <div class="input-field col s6">
                                                        <div class="checkrow">
                                                            <asp:CheckBox ID="chkRenewalNotes" CssClass="filled-in" runat="server" />
                                                            <asp:Label runat="server" ID="lblchkRenewalNotes" AssociatedControlID="chkRenewalNotes">Renewal Notes</asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s6">
                                                        <div class="row">
                                                            <asp:Label runat="server" ID="Label17" AssociatedControlID="txtRenewalNotes">Notes</asp:Label>
                                                            <asp:TextBox ID="txtRenewalNotes" CssClass="materialize-textarea" runat="server" TextMode="MultiLine"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                        <div class="form-section-row">
                                            <div class="form-section3">
                                                <div class="section-ttle">Billing Rate</div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtBillRate" runat="server" AutoCompleteType="None"
                                                            MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtBillRate" ID="Label6">Billing rate</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtOt" runat="server" AutoCompleteType="None"
                                                            MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtOt" ID="Label7">OT Rate</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtNt" runat="server" AutoCompleteType="None"
                                                            MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtNt" ID="Label8">1.7 Rate</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtDt" runat="server" AutoCompleteType="None"
                                                            MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtDt" ID="Label9">DT Rate</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtTravel" runat="server" AutoCompleteType="None"
                                                            MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtTravel" ID="Label10">Travel Rate</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s2">
                                                    <div class="row">
                                                        &nbsp;
                                                    </div>
                                                </div>
                                                <div class="input-field col s5">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtMileage" runat="server" AutoCompleteType="None"
                                                            MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        <asp:Label runat="server" AssociatedControlID="txtMileage" ID="Label11">Mileage</asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="section-ttle">Escalation</div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <label class="drpdwn-label">Escalation Type</label>
                                                        <asp:DropDownList ID="ddlEscType" runat="server" CssClass="browser-default">
                                                            <asp:ListItem Value="3">Manual</asp:ListItem>
                                                            <asp:ListItem Value="0">Commodity Index</asp:ListItem>
                                                            <asp:ListItem Value="1">Escalation</asp:ListItem>
                                                            <asp:ListItem Value="2">Return</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            TargetControlID="txtEscCycle" ValidChars="1234567890">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="txtEscCycle" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblEscCycle" AssociatedControlID="txtEscCycle">Escalation Cycle</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                            TargetControlID="txtEscFactor" ValidChars="1234567890.">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="txtEscFactor" runat="server"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label1" AssociatedControlID="txtEscCycle">Escalation Factor</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field2 col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtEscdue" runat="server" onblur="validateDatetime();" CssClass="datepicker_mom" MaxLength="28"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label2" AssociatedControlID="txtEscdue">Escalated Last</asp:Label>
                                                        <asp:RequiredFieldValidator ID="rfEscalatedLast" runat="server" ControlToValidate="txtEscdue" ValidationGroup="rec"
                                                            Display="None" ErrorMessage="Escalated Last Required" SetFocusOnError="false"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vcEscalatedLast" runat="server" Enabled="True"
                                                            TargetControlID="rfEscalatedLast" PopupPosition="BottomLeft">
                                                        </asp:ValidatorCalloutExtender>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">
                                                <div class="section-ttle">Contract Renewal</div>

                                                <div class="input-field2 col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtOriginalContract" CssClass="datepicker_mom" runat="server" MaxLength="10"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Labe44l18" AssociatedControlID="txtOriginalContract">Original Contract</asp:Label>

                                                    </div>
                                                </div>
                                                <div class="input-field2 col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtLastrenew" CssClass="datepicker_mom" runat="server" MaxLength="10" ClientIDMode="Static"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label19" AssociatedControlID="txtLastrenew">Last Renew</asp:Label>

                                                    </div>
                                                </div>
                                                <div class="input-field2 col s12">
                                                    <div class="row">
                                                        <asp:FilteredTextBoxExtender ID="fesdsds" runat="server" Enabled="True"
                                                            TargetControlID="txtContractLength" ValidChars="1234567890.">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="txtContractLength" runat="server" MaxLength="28" Text="" ClientIDMode="Static"></asp:TextBox>
                                                        <asp:Label runat="server" ID="Label20" AssociatedControlID="txtContractLength">Contract Length</asp:Label>

                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <div>
                                                                    <label class="drpdwn-label">Expiration</label>
                                                                    <asp:DropDownList ID="ddlExpiration" runat="server" CssClass="browser-default" ClientIDMode="Static">
                                                                        <asp:ListItem Value="">Indefinitely</asp:ListItem>
                                                                        <asp:ListItem Value="1">Contract expiration date</asp:ListItem>
                                                                        <asp:ListItem Value="2" style="display: none">Number of frequencies</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <asp:TextBox ID="txtUnitExpiration" runat="server" CssClass="datepicker_mom" MaxLength="28" ClientIDMode="Static"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Enabled="false" ControlToValidate="txtUnitExpiration" ValidationGroup="rec"
                                                                    Display="None" ErrorMessage="Expiration Date Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server"
                                                                    TargetControlID="RequiredFieldValidator3" PopupPosition="BottomLeft">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtNumFreq" runat="server" MaxLength="28" Visible="False"></asp:TextBox>

                                                                <asp:RequiredFieldValidator Enabled="false" ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNumFreq" ValidationGroup="rec"
                                                                    Display="None" ErrorMessage="Expiration Freq Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server"
                                                                    TargetControlID="RequiredFieldValidator4" PopupPosition="Right">
                                                                </asp:ValidatorCalloutExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-section-row left-css">
                                        <div class="section-ttle">Additional Fields</div>
                                        <div class="input-field col s12">
                                            <div class="row">
                                                <asp:Label runat="server" ID="lblRemarks" AssociatedControlID="txtRemarks">Remarks</asp:Label>
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="materialize-textarea" TextMode="MultiLine" MaxLength="8000"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="cf"></div>
                                </div>
                            </div>

                        </li>
                        <li>
                            <div id="accrdcustom" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Custom</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="col s12 m12 l12">
                                            <div class="grid_container">
                                                <div class="RadGrid RadGrid_Material">
                                                    <asp:GridView ID="gvCustom" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        PageSize="20" ShowFooter="true"
                                                        ShowHeaderWhenEmpty="True" EmptyDataText="No records Found">
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="SNO" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFormat" runat="server" Text='<%# Eval("FieldControl") %>' Visible="false"
                                                                        Width="300px"></asp:Label>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblIndex" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                                                    <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblFormatID" runat="server" Text='<%# Eval("Format") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Desc">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("Label") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Value">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlFormat" runat="server" CssClass="form-control" Visible="false">
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtValue" MaxLength="50" runat="server" Text='<%# Eval("Value") %>'
                                                                        Visible="false" CssClass="form-control"></asp:TextBox>
                                                                    <asp:CheckBox ID="chkValue" runat="server" Visible="false" Checked='<%# Eval("FieldControl").ToString().Equals("Checkbox") ? (Eval("Value") == DBNull.Value ? false :(Eval("Value").Equals("1") ? true : false)) : false %>' />
                                                                    <asp:MaskedEditExtender Enabled='<%# Session["MSM"].ToString() == "TS" ? false : (Eval("FieldControl").ToString()=="Date" ? true : false) %>'
                                                                        TargetControlID="txtValue" ID="MaskedEditDate" runat="server" Mask="99/99/9999"
                                                                        MaskType="Date" UserDateFormat="MonthDayYear">
                                                                    </asp:MaskedEditExtender>
                                                                    <asp:FilteredTextBoxExtender Enabled='<%# Session["MSM"].ToString() == "TS" ? false : ( Eval("FieldControl").ToString()=="Currency" ? true : false) %>'
                                                                        ID="FilteredTextBoxExtender1" TargetControlID="txtValue" runat="server" ValidChars="0123456789.-+">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <div style="float: left; margin-top: 5px; margin-left: 10px;">
                                                                        <asp:Label ID="lblRowCount" runat="server" Text=""></asp:Label>
                                                                    </div>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle CssClass="footer" />
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="input-field col s12">
                                            <div class="row">
                                                &nbsp;
                                            </div>
                                        </div>
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

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">

    <script type="text/javascript"> 

        $(document).ready(function () {

            $("#eqtag").click(function () {

                $("#DivEqup").toggle();
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

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });


            $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").click(CheckUncheckAllCheckBoxAsNeeded);

            $("#<%=gvEquip.ClientID%> input[id*='chkAll']:checkbox").click(function () {

                if ($(this).is(':checked')) {
                    $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").attr('checked', true);
                }
                else {
                    $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").attr('checked', false);
                }

                SelectRows('<%=gvEquip.ClientID%>', '<%=txtUnit.ClientID%>', '<%=hdnUnit.ClientID%>');

                CalculateAmount();

                CalculateHours();

            });

            if ($('#<%=gvEquip.ClientID%>').length > 0) {

                SelectRows('<%=gvEquip.ClientID%>', '<%=txtUnit.ClientID%>', '<%=hdnUnit.ClientID%>');
            }

            ///////////// Quick Codes //////////////
            $("#<%=txtSpecialInstructions.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtSpecialInstructions.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });
            $("#<%=txtRenewalNotes.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtRenewalNotes.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });
            $("#<%=txtRemarks.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtRemarks.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });

            Materialize.updateTextFields();
        });


        function ValidateAmt() {


            var selected = document.getElementById("<%=ddlBillDetailLevel.ClientID%>").value;

            if (selected == "2") {

                var grid = document.getElementById('<%=gvEquip.ClientID%>');
                var txtAmount = document.getElementById('<%=txtBillAmt.ClientID%>');
                var cell;
                var total = 0;

                if (grid.rows.length > 0) {
                    for (i = 1; i < grid.rows.length; i++) {
                        cell = grid.rows[i].cells[10];
                        cell1 = grid.rows[i].cells[0];

                        if (cell1.childNodes[3].checked == true) {
                            $(cell.childNodes[1]).formatCurrency();
                            if (cell.childNodes[1].value != '') {
                                var text = parseFloat(cell.childNodes[1].value.replace("$", "").replace(/,/g, ""));
                                //                        alert(text);                        
                                total = total + text;
                            }
                        }
                    }


                    if (total == 0) {
                        noty({
                            text: 'Please select at least one equipment with a price.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                        });

                        return false;

                    } 
                }

                Materialize.updateTextFields();
            }


            return true;
        }

        function pageLoad(sender, args) {

            $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").click(CheckUncheckAllCheckBoxAsNeeded);

            $("#<%=gvEquip.ClientID%> input[id*='chkAll']:checkbox").click(function () {


                if ($(this).is(':checked')) {
                    $('#<%=gvEquip.ClientID %>').find("input:checkbox").each(function () {
                        //this.checked = true;
                        var txt = this.id;
                        var lblStatus = document.getElementById(txt.replace('chkSelect', 'lblStatus'));
                        if (lblStatus == "Inactive") {
                            this.checked = false;
                        }
                        else {
                            this.checked = true;
                        }


                    });
                }
                else {
                    $('#<%=gvEquip.ClientID %>').find("input:checkbox").each(function () { this.checked = false; });
                }

                SelectRowsEq('<%=gvEquip.ClientID%>', '<%=txtUnit.ClientID%>', '<%=hdnUnit.ClientID%>');

                CalculateAmount();

                CalculateHours();
            });
        }
    </script>

    <script type="text/javascript">

        $("#<%=ddlStatus.ClientID%>").change(function () {

            var ddlval = $('option:selected', $(this)).text();

            var JobClosePermission = document.getElementById('<%= hdnClosePermission.ClientID%>').value;

            var CompletedJObPermission = document.getElementById('<%= hdnCompletedJObPermission.ClientID%>').value;

            var JobReopenPermission = document.getElementById('<%= hdnJobReopenPermission.ClientID%>').value;

            var _hdnddlJobStatus = document.getElementById('<%= hdnddlJobStatus.ClientID%>').value;


            if (JobClosePermission == 'N') {

                if (ddlval == 'Closed' && _hdnddlJobStatus != '1') {
                    noty({
                        text: 'You do not have permission to close project!', type: 'warning', layout: 'topCenter',
                        closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true
                    });
                    $("#<%=ddlStatus.ClientID%>").val(_hdnddlJobStatus);
                }

            }

            if (JobReopenPermission == 'N') {
                if (ddlval == 'Active' && _hdnddlJobStatus != '0') {
                    noty({
                        text: 'You do not have permission to Reopen project!', type: 'warning', layout: 'topCenter',
                        closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true
                    });
                    $("#<%=ddlStatus.ClientID%>").val(_hdnddlJobStatus);
                }
            }
            if (CompletedJObPermission == 'N') {
                if (ddlval == 'Completed' && _hdnddlJobStatus != '3') {
                    noty({
                        text: 'You do not have permission to complete project!', type: 'warning', layout: 'topCenter',
                        closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true
                    });
                    $("#<%=ddlStatus.ClientID%>").val(_hdnddlJobStatus);
                }
            }

        });

    </script>

</asp:Content>
