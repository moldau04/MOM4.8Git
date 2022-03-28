<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddJournalEntry" CodeBehind="AddJournalEntry.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%--<%@ Register Src="uc_CustomerSearch.ascx" TagName="uc_customersearch" TagPrefix="uc1" %>--%>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <link href="Design/css/pikaday.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    <script src="js/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>
    <style>
        input[readonly="readonly"] {
            background-color: #fff !important;
        }

        a[disabled=disabled] {
            color: gray;
        }

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

        .RadGrid_Bootstrap .rgFooter > td:first-child {
            text-align: -webkit-left !important;
        }

        .RadGrid_Bootstrap .rgFooterWrapper tr.rgFooter td {
            text-align: right;
            padding-right: 12px !important;
        }

        .RadWindow label {
            margin-left: 15px;
        }

        .top-fix {
            height: 34px;
            position: absolute;
            top: 39px;
            right: 20px;
            left: 0;
            background: #f3f3f3;
            padding: 2px 15px;
            z-index: 9999;
        }

        .form-section3 .checkrow{
            height: 3.5rem;
            margin-bottom: 20px;
        }

        .form-section3 .checkrow label{
            position: inherit;
        }
    </style>

    <script type="text/javascript"> 
        $(document).ready(function () {
            if ($("#ctl00_ContentPlaceHolder1_RadAddAccountWindow_C_ddlType option:selected").text() != 'Bank') {
                $(".accrd-bank-info").hide();
            }
        });

        function ShowBankPartial() {
            if ($("#ctl00_ContentPlaceHolder1_RadAddAccountWindow_C_ddlType option:selected").text() != 'Bank') {
                $(".accrd-bank-info").hide();
            }
            else {
                $(".accrd-bank-info").show();
            }
        }

        function InitGridData() {
            $(function () {
                $("[id*=txtGvAcctNo]").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;

                        var str = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetAccountNameJE",
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
                        console.log(ui.item);
                        if (ui.item.value == 0 || ui.item.value == ' < Add New > ') {
                            itemJSON();
                            var oWnd = $find("<%=RadAddAccountWindow.ClientID%>");
                            oWnd.show();
                        }
                        else {
                            var txtGvAcctName = this.id;
                            var hdnAcctID = document.getElementById(txtGvAcctName.replace('txtGvAcctNo', 'hdnAcctID'));
                            var txtGvCompanyName = document.getElementById(txtGvAcctName.replace('txtGvAcctNo', 'txtGvCompanyName'));
                            var txtGvAcctName = document.getElementById(txtGvAcctName.replace('txtGvAcctNo', 'txtGvAcctName'));

                            $(txtGvAcctName).val(ui.item.label);
                            $(txtGvCompanyName).val(ui.item.Company);
                            $(hdnAcctID).val(ui.item.value);
                            $(this).val(ui.item.acct);

                            CopyDescToMomo();
                        }

                        return false;
                    },
                    focus: function (event, ui) {
                        $(this).val(ui.item.acct);
                        return false;
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
                                .data("item.ui-autocomplete", item)
                                .append("<a>" + result_item + "</a>")
                                .appendTo(ul);
                        }
                        else {
                            return $("<li></li>")
                                .data("item.ui-autocomplete", item)
                                .append("<a>" + result_item + ", <span>" + result_desc + "</span></a>")
                                .appendTo(ul);
                        }
                    };
                });

                $("[id*=txtGvPhase]").autocomplete({
                    source: function (request, response) {
                        var curr_control = this.element.attr('id');
                        var job = document.getElementById(curr_control.replace('txtGvPhase', 'hdnJobID'));

                        var dtaaa = new dtaa();
                        dtaaa.jobID = document.getElementById(job.id).value;
                        dtaaa.prefixText = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetAllPhase",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load phase details");
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
                        var hdnPID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnPID'));
                        var txtGvLoc = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvLoc'));
                        var txtGvJob = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvJob'));
                        var hdnJobID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnJobID'));

                        $(hdnPID).val(ui.item.Line);
                        $(this).val(ui.item.Desc);
                        $(txtGvLoc).val(ui.item.LocName);
                        $(txtGvJob).val(ui.item.Job+' '+ui.item.JobName);
                        $(hdnJobID).val(ui.item.Job);

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
                })
                $.each($(".psearchinput"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        //var ula = ul;
                        //var itema = item;
                        //var result_value = item.Line;
                        //var result_item = item.Desc;
                        //var result_desc = item.Line;
                        //var result_type = item.PhaseType;
                        //var result_bomType = item.bomType;

                        //if (result_value == 0) {
                        //    return $("<li></li>")
                        //        .data("item.ui-autocomplete", item)
                        //        .append("<a>" + result_item + "</a>")
                        //        .appendTo(ul);
                        //}
                        //else {
                        //    if (result_type == "0")
                        //        result_type = "Revenue";
                        //    else if (result_type == "1" || result_type == "2")
                        //        result_type = "Expense";

                        //    return $("<li></li>")
                        //        .data("item.ui-autocomplete", item)
                        //        .append("<a><b> Code: </b> " + result_bomType + ", <b>Desc:</b> " + result_item + ", <span style='color:Gray;'>" + result_type + "</span></a>")
                        //        .appendTo(ul);
                        //}

                        var ula = ul;
                        var itema = item;
                        var result_value = item.Type;
                        var result_item = item.PhaseType;
                        var result_GroupName = item.GroupName;
                        var result_Code = item.Code;
                        var result_CodeDesc = item.Desc;
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

               






                $("[id*=txtGvLoc]").autocomplete({
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
                                .data("item.ui-autocomplete", item)
                                .append("<a>" + result_item + "</a>")
                                .appendTo(ul);
                        }
                        else {
                            return $("<li></li>")
                                .data("item.ui-autocomplete", item)
                                .append("<a><b> Project: </b> " + result_value + ", " + result_item + ", <span style='color:Gray;'><b> Loc: </b>" + result_desc + "</span></a>")
                                .appendTo(ul);
                        }
                    };
                });

            });
        }

        $(document).ready(function () {

            InitializeGrids('<%=RadGrid_Journal.ClientID%>');
            BalanceProof();
            InitGridData();
            GridRowSelected();
        });

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
            var rawData = $('#<%=RadGrid_Journal.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            BalanceProof();
            $('#<%=hdnJournalJSON.ClientID%>').val(formData);
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

        function clearEntry() {
            var chkIsRecur = $('#<%= chkIsRecurr.ClientID %>').is(':checked');
            if (chkIsRecur == false) {
                var hdnTransID = $("#<%= hdnTransID.ClientID %>").val();
                $("#<%= txtEntryNo.ClientID %>").val(hdnTransID);
                $("#<%= hdnIsRecurr.ClientID %>").val("false");
            }
            else {
                $("#<%= txtEntryNo.ClientID %>").val('');
                $("#<%= hdnIsRecurr.ClientID %>").val("true");

            }
        }

        function toggleRecurring() {
            var chkIsRecur = $('#<%= chkIsRecurr.ClientID %>').is(':checked');
            if (chkIsRecur == false) {
                document.getElementById("devRec").style.display = "none";
                document.getElementById("devRecEntry1").style.display = "none";
                document.getElementById("devRecEntry").style.display = "block";
            }
            else {
                document.getElementById("devRec").style.display = "block";
                document.getElementById("devRecEntry1").style.display = "block";
                document.getElementById("devRecEntry").style.display = "none";
            }
        }

        function BalanceProof() {

            var totalDebit = 0.00;
            var totalCredit = 0.00;
            var balance = 0;

            $("[id*=txtGvDebit]").each(function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totalDebit = totalDebit + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            $("[id*=txtGvCredit]").each(function () {
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totalCredit = totalCredit + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                    }
                    else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            $("[id*=lblTotalDebit]").text(totalDebit.toLocaleString("en-US", { style: 'currency', currency: 'USD', minimumFractionDigits: 2 }));
            $("[id*=lblTotalCredit]").text(totalCredit.toLocaleString("en-US", { style: 'currency', currency: 'USD', minimumFractionDigits: 2 }));
            balance = totalDebit - totalCredit;
            if (balance == 0) {
                $("[id*=txtProof]").css('color', 'Black');
                $('#<%=hdnIsPositive.ClientID %>').val("true");
                $("[id*=txtProof]").val("0.00");
            }
            else {
                if (balance < 0) {
                    balance = balance * -1;
                    $("[id*=txtProof]").val(totalDebit.toFixed(2).toString());
                    $("[id*=txtProof]").css('color', 'Red');
                    $('#<%=hdnIsPositive.ClientID %>').val("false");
                }
                else {
                    $("[id*=txtProof]").css('color', 'Black');
                    $('#<%=hdnIsPositive.ClientID %>').val("true");
                }
                $("[id*=txtProof]").val(balance.toFixed(2).toString());
            }
            if (typeof (Materialize) != 'undefined' && typeof (Materialize.updateTextFields) == 'function') {
                Materialize.updateTextFields();
            }
        }

        $(document).ready(function () {
            $('#<%=hdnIsPositive.ClientID %>').val("true");
            var config = {
                '.chosen-select': {},
                '.chosen-select-deselect': { allow_single_deselect: true },
                '.chosen-select-no-single': { disable_search_threshold: 10 },
                '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chosen-select-width': { width: "95%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

            if ($(window).width() > 767) {
                $('#<%=txtDescription.ClientID%>').focus(function () {
                    $(this).animate({
                        width: '520px',
                        height: '75px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtDescription.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%',
                        height: '46px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });
            }

            $("[id*=txtGvAcctNo]").change(function () {
                var txtGvAcctNo = $(this);
                var strAcctNo = $(this).val();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "AccountAutoFill.asmx/GetChartByAcct",
                    data: '{"prefixText": "' + strAcctNo + '"}',
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var ui = $.parseJSON(data.d);

                        if (ui.length == 0 && strAcctNo != "") {
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
                            var txtGvAcctNo1 = $(txtGvAcctNo).attr('id');
                            var hdnAcctID = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'hdnAcctID'));
                            var txtGvAcctName = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'txtGvAcctName'));
                            var txtGvCompanyName = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'txtGvCompanyName'));

                            $(hdnAcctID).val(ui[0].ID);
                            $(txtGvAcctName).val(ui[0].fDesc);
                            $(txtGvCompanyName).val(ui[0].Company);
                        }
                    },
                    error: function (result) {
                        alert("Due to unexpected errors we were unable to load Acct#");
                    }
                });
            });

        })

        function validate() {
            var valisRecuring = $("#<%= chkIsRecurr.ClientID %>").val();
            var valProof = $("#<%= txtProof.ClientID %>").val();
            if (parseFloat(valProof) > 0) {
                noty({ text: 'Your adjustment is out of balance', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: false, theme: 'noty_theme_default', closable: true });
                $('#MOMloading').hide();
                return false;
            }
            else {
                var field = 'id';
                var url = window.location.href;
                itemJSON();
                if (url.indexOf('&c=' != -1)) {
                    var con = confirm('Do you want to save this Journal entry ?');
                    if (con == true) {
                    } else {
                        $('#MOMloading').hide();
                    }
                    itemJSON();
                    return con;
                }
                else if (url.indexOf('?' + field + '=') != -1)
                    return true;
                else {
                    var con = confirm('Do you want to save this Journal entry ?');
                    if (con == true) {
                    } else {
                        $('#MOMloading').hide();
                    }
                    itemJSON();
                    return con;
                }

            }
        }

        function Balance(obj) {
            var objId = document.getElementById(obj.id);
            var isDebit = obj.id.search("txtGvDebit");
            if (isDebit == -1) {
                //on credit textbox change   
                var valCredit = document.getElementById(obj.id).value;
                if (isInt(valCredit) == true) {
                    document.getElementById(obj.id).value = parseFloat(valCredit).toFixed(2);
                }
                var txtGvDebit = document.getElementById(obj.id.replace('txtGvCredit', 'txtGvDebit'));
                $(txtGvDebit).val('0.00');
            }
            else {
                //on debit textbox change
                var valDebit = document.getElementById(obj.id).value;
                if (isInt(valDebit) == true) {
                    document.getElementById(obj.id).value = parseFloat(valDebit).toFixed(2);
                }
                var txtGvCredit = document.getElementById(obj.id.replace('txtGvDebit', 'txtGvCredit'));
                $(txtGvCredit).val('0.00');
            }
            BalanceProof();
        }

        function isInt(value) {
            var x = parseFloat(value);
            return !isNaN(value) && (x | 0) === x;
        }

        function VisibleRow(row, txt, gridview, event) {
            var rowst = document.getElementById(row)

            var grid = document.getElementById(gridview);
            $('#<%=RadGrid_Journal.ClientID %> input:text.form-control1').each(function () {

                $(this).removeClass("form-control1");
                $(this).addClass("texttransparent");
            });
            $('#<%=RadGrid_Journal.ClientID %> select.form-control1').each(function () {

                $(this).removeClass("form-control1");
                $(this).addClass("texttransparent");

            });

            var txtGvAcctNo = document.getElementById(txt);
            $(txtGvAcctNo).removeClass("texttransparent");
            $(txtGvAcctNo).addClass("form-control1");

            var txtGvtransDes = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvtransDes'));
            $(txtGvtransDes).removeClass("texttransparent");
            $(txtGvtransDes).addClass("form-control1");

            var txtGvDebit = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvDebit'));
            $(txtGvDebit).removeClass("texttransparent");
            $(txtGvDebit).addClass("form-control1");

            var txtGvCredit = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvCredit'));
            $(txtGvCredit).removeClass("texttransparent");
            $(txtGvCredit).addClass("form-control1");

            if ($("[id*=txtGvLoc]").length) {
                var txtGvLoc = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvLoc'));
                $(txtGvLoc).removeClass("texttransparent");
                $(txtGvLoc).addClass("form-control1");

                var txtGvPhase = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvPhase'));
                $(txtGvPhase).removeClass("texttransparent");
                $(txtGvPhase).addClass("form-control1");
            }
        }

        function VisibleRowOnFocus(txt) {
            $('#<%=RadGrid_Journal.ClientID %> input:text.form-control1').each(function () {

                $(this).removeClass("form-control1");
                $(this).addClass("texttransparent");
            });
            $('#<%=RadGrid_Journal.ClientID %> select.form-control1').each(function () {

                $(this).removeClass("form-control1");
                $(this).addClass("texttransparent");

            });

            var txtGvAcctNo = document.getElementById(txt.id);
            $(txtGvAcctNo).removeClass("texttransparent");
            $(txtGvAcctNo).addClass("form-control1");

            var txtGvtransDes = document.getElementById(txt.id.replace('txtGvAcctNo', 'txtGvtransDes'));
            $(txtGvtransDes).removeClass("texttransparent");
            $(txtGvtransDes).addClass("form-control1");

            var txtGvDebit = document.getElementById(txt.id.replace('txtGvAcctNo', 'txtGvDebit'));
            $(txtGvDebit).removeClass("texttransparent");
            $(txtGvDebit).addClass("form-control1");

            var txtGvCredit = document.getElementById(txt.id.replace('txtGvAcctNo', 'txtGvCredit'));
            $(txtGvCredit).removeClass("texttransparent");
            $(txtGvCredit).addClass("form-control1");
            if ($("[id*=txtGvLoc]").length) {

                var txtGvLoc = document.getElementById(txt.id.replace('txtGvAcctNo', 'txtGvLoc'));
                $(txtGvLoc).removeClass("texttransparent");
                $(txtGvLoc).addClass("form-control1");

                var txtGvPhase = document.getElementById(txt.id.replace('txtGvAcctNo', 'txtGvPhase'));
                $(txtGvPhase).removeClass("texttransparent");
                $(txtGvPhase).addClass("form-control1");
            }
        }

        function ClearRow(row) // To clear GridView Row on delete button click
        {
            $("#" + row + " input:text").each(function () {
                $(this).val("");

            });
            $("#" + row + " select").each(function () {
                $(this).val("");
            });
        }

        function clearJob(txt) {
            if ($(txt).val() == '') {
                var txtJobID = document.getElementById(txt.id.replace('txtGvJob', 'txtGvLoc'));
                $(txtJobID).val('');
            }
        }

        function clearPhase(txt) {
            if ($(txt).val() == '') {
                var hdnPID = document.getElementById(txt.id.replace('txtGvPhase', 'hdnPID'));
                $(hdnPID).val('');
            }
        }
    </script>
    <script type="text/javascript" lang="javascript">
        function dtaa() {
            this.prefixText = null;
            this.con = null;
        }

        $("[id*=txtGvLoc]").on("keydown", function () {
            $("#" + this.id).prop('readonly', true);
        });

        $("[id*=txtGvAcctName]").on("keydown", function () {
            $("#" + this.id).prop('readonly', true);
        });

        $("[id*=txtGvJob]").on("keydown", function () {
            $("#" + this.id).prop('readonly', true);
        });

        $("[id*=txtGvPhase]").on("keydown", function () {
            var txtGvPhase = this.id;
            var hdnJobID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnJobID'));
            if ($(hdnJobID).val() == "")
                return false;
        });

        function CopyDescToMomo() {
            var _mainDesc = $("#<%=txtDescription.ClientID %>").val();

            $("#<%=RadGrid_Journal.ClientID %>").find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);
                var txtGvAcctNo = $tr.find('input[id*=txtGvAcctNo]').val();
                if (txtGvAcctNo != "") {
                    var txtGvtransDes = $tr.find('input[id*=txtGvtransDes]').val();
                    if (txtGvtransDes == "" || txtGvtransDes == _mainDesc) {
                        $tr.find('input[id*=txtGvtransDes]').val(_mainDesc);
                    }
                }
            });
        }

        function GridRowSelected() {
            $telerik.$(".textbox").focus(function (e) {
                var rowIndex = $telerik.$(e.currentTarget).parent().parent().get(0).rowIndex;
                $find("<%= RadGrid_Journal.ClientID %>").get_masterTableView().selectItem(rowIndex - 1);
            });
        }

        function UpdateTextbox() {
            if (typeof (Materialize) != 'undefined' && typeof (Materialize.updateTextFields) == 'function') {
                Materialize.updateTextFields();
            }
        }

        function CheckAddRowGrid(sender, args) {

            itemJSON();
        }

        //Journal Entry part
        function RowContextMenuJournalGrid(sender, eventArgs) {
            var menu = $find("<%=RadMenuJournalEntryGrid.ClientID %>");
            var evt = eventArgs.get_domEvent();

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

        function openfileDialog() {
            $("#<%=FileUploadControl.ClientID%>").click();
        }

        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=lnkFileUploaded.ClientID %>").click();
            }
        }

        function OpenErrorModal() {
            window.radopen(null, "errorWindow");
        }

        function CloseModal() {
            var radwindow = $find('<%=RadAddAccountWindow.ClientID %>');
            radwindow.close();
            $(".accrd-bank-info").hide();
            document.getElementById("<%=btnUpdateGridData.ClientID %>").click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdnBatchID" runat="server" />
    <asp:HiddenField ID="hdnIsRecurr" runat="server" />
    <asp:HiddenField ID="hdnIsPositive" runat="server" />
    <asp:HiddenField ID="hdnTransID" runat="server" />
    <asp:HiddenField ID="hdnAddRowValue" runat="server" Value="4" />
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-editor-format-align-left"></i>&nbsp;
                                        <asp:Label runat="server" ID="lblHeader" Text="Add Journal Entries"></asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <%--<a href="#">Save
                                            </a>--%>
                                            <asp:LinkButton CssClass="icon-save" ID="btnSaveNew" ToolTip="Add New" runat="server" OnClientClick="disableButton(this,'Journal'); return validate();" OnClick="btnSaveNew_Click" ValidationGroup="Journal"
                                                TabIndex="24" Text="Save"></asp:LinkButton>
                                            <asp:LinkButton CssClass="icon-print" ID="lnkPrint" runat="server" ToolTip="Print" OnClientClick="itemJSON();" OnClick="lnkPrint_Click" ValidationGroup="Journal"
                                                TabIndex="25" Text="Print">
                                            </asp:LinkButton>

                                            <asp:LinkButton ID="btnReverse" runat="server" OnClick="btnReverse_Click" ToolTip="Reverse" CausesValidation="false" Text="Reverse" Visible="false"></asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <a id="aImport" runat="server" class="dropdown-button" data-beloworigin="true" href="javascript:void(0)" data-activates="dynamicUI">Import</a>
                                            <asp:LinkButton ID="btnExportExcel" runat="server" Text="Export to Excel" CausesValidation="false" OnClick="btnExportExcel_Click" Visible="false" />
                                            <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="hidden" onchange="UploadFile(this);" />
                                            <asp:LinkButton ID="lnkFileUploaded" runat="server" Text="Import Items" CssClass="hidden" CausesValidation="false" OnClick="lnkFileUploaded_Click" />
                                        </div>
                                        <ul id="dynamicUI" class="dropdown-content">
                                            <li><a id="btnImportItems" title="Import Items" href="javascript:void(0)" onclick="openfileDialog();">Upload File</a></li>
                                            <li>
                                                <asp:LinkButton Text="CSV Template" runat="server" ID="btnDownloadCSV" CausesValidation="false" OnClick="btnDownloadCSV_Click" />
                                            </li>
                                            <li>
                                                <asp:LinkButton Text="Excel Template" runat="server" ID="btnDownloadExcel" CausesValidation="false" OnClick="btnDownloadExcel_Click" />
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="btnclosewrap">
                                        <%--<a href="#"><i class="mdi-content-clear"></i></a>--%>
                                        <asp:LinkButton CssClass="mdi-content-clear" ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click" TabIndex="26"></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label ID="lblEntryNo" runat="server" Visible="false"></asp:Label>
                                        </div>
                                        <%--<div class="editlabel">
                                            <a id="aOriginalJE" runat="server" class="dropdown-button" data-beloworigin="true" href="javascript:void(0)" data-activates="dynamicUI">Import123</a>
                                        </div>--%>
                                        <span class="tro trost">
                                            <asp:LinkButton ID="lnkOriginal" runat="server" OnClick="lnkOriginal_Click" Text=""></asp:LinkButton>
                                            <asp:HiddenField ID="hdnOriginalJE" runat="server" Value ="0"/>                                            
                                        </span>
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
                                        <li id="liDocuments"><a href="#accrdDocuments">Documents</a></li>
                                        <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                    </ul>
                                </div>
                            <div class="tblnksright divNavigateLnk">
                                <div class="nextprev" id="divNavigate" runat="server">
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
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="srchpane" style="margin-top: 10px;">
                <telerik:RadAjaxManager ID="RadAjaxManager_Journal" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="chkJobSpecific">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Journal" LoadingPanelID="RadAjaxLoadingPanel_Journal" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="btnCopyPrevious">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Journal" LoadingPanelID="RadAjaxLoadingPanel_Journal" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="btnAddNewLines">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Journal" LoadingPanelID="RadAjaxLoadingPanel_Journal" />
                                <telerik:AjaxUpdatedControl ControlID="hdnAddRowValue" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="btnAddNewLinesToCal">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Journal" LoadingPanelID="RadAjaxLoadingPanel_Journal" />
                                <telerik:AjaxUpdatedControl ControlID="hdnAddRowValue" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="btnUpdateGridData">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_Journal" LoadingPanelID="RadAjaxLoadingPanel_Journal" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>

                <div class="form-section-row">
                    <div class="section-ttle">Journal Details</div>
                    <div class="form-section3">
                        <div class="input-field col s12">
                            <div class="row">
                                <label for="txtDate" class="">Date</label>
                                <asp:RequiredFieldValidator runat="server" ID="rfvTransDate" ErrorMessage="Please select Date" ControlToValidate="txtTransDate"
                                    ValidationGroup="Journal" Display="None"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vceTransDate" runat="server" Enabled="True" PopupPosition="Right"
                                    TargetControlID="rfvTransDate" />

                                <asp:CustomValidator ID="cvfDate" runat="server" ErrorMessage="These month/year period is closed out. Please select another date!"
                                    OnServerValidate="cvfDate_ServerValidate" ControlToValidate="txtTransDate" Display="None" SetFocusOnError="true" ValidationGroup="Journal">
                                </asp:CustomValidator>
                                <asp:ValidatorCalloutExtender ID="vcoefDate" runat="server" Enabled="True"
                                    PopupPosition="Right" TargetControlID="cvfDate" />

                                <asp:RegularExpressionValidator ID="revTransDate" ControlToValidate="txtTransDate" ValidationGroup="Journal"
                                    ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                    runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                </asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="vceTransDate1" runat="server" Enabled="True" PopupPosition="Right"
                                    TargetControlID="revTransDate" />

                                <asp:TextBox ID="txtTransDate" runat="server" CssClass="datepicker_mom" TabIndex="2" MaxLength="15" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="input-field col s12">
                            <div class="row">
                                <label for="txtDesc" class="">Description</label>
                                <asp:RequiredFieldValidator runat="server" ID="rfvDescription" ErrorMessage="Please enter Description"
                                    Display="None" ControlToValidate="txtDescription" ValidationGroup="Journal"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vceDescription" runat="server" Enabled="True" PopupPosition="Right"
                                    TargetControlID="rfvDescription" />
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="materialize-textarea" TabIndex="6"
                                    TextMode="MultiLine" MaxLength="75"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-section3-blank">
                        &nbsp;
                    </div>
                    <div class="form-section3">
                        <div class="input-field col s12">
                            <div class="row">
                                <label for="txtProof" class="">Proof</label>
                                <asp:TextBox ID="txtProof" runat="server" TabIndex="2" align="right" MaxLength="15"></asp:TextBox>

                            </div>
                        </div>
                        <div class="input-field col s5 chkmgn">
                            <div class="checkrow">
                                <asp:CheckBox ID="chkJobSpecific" CssClass="css-checkbox" Text="Project Specific" runat="server" OnCheckedChanged="chkJobSpecific_CheckedChanged" AutoPostBack="true" />
                            </div>
                        </div>
                        <div class="input-field col s2 chkmgn">
                            <div class="row">
                                &nbsp;
                            </div>
                        </div>
                        <div class="input-field col s5">
                            <div class="checkrow">
                                <asp:CheckBox ID="chkIsRecurr" runat="server" Text="Is Recurring" CssClass="css-checkbox" OnCheckedChanged="chkIsRecurr_CheckedChanged" AutoPostBack="true" />
                            </div>
                        </div>
                        <asp:UpdatePanel ID="updPnlFrequency" runat="server">
                            <ContentTemplate>
                                <div class="input-field col s12">
                                    <div class="row">
                                        <label id="lblEntry" runat="server" for="txtEntry">Entry Number</label>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvEntryNo" ControlToValidate="txtEntryNo"
                                            ErrorMessage="Please enter Entry no." Display="None"
                                            ValidationGroup="Journal"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceEntryNo" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvEntryNo" />

                                        <asp:CustomValidator ID="cvEntryNo" runat="server" ErrorMessage="The Entry number is already in use, please use another number!"
                                            OnServerValidate="cvEntryNo_ServerValidate" ControlToValidate="txtEntryNo" Display="None" SetFocusOnError="true" ValidationGroup="Journal">
                                        </asp:CustomValidator>
                                        <asp:ValidatorCalloutExtender ID="vceEntryNum" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="cvEntryNo" />

                                        <asp:TextBox ID="txtEntryNo" runat="server" TabIndex="2" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="input-field col s5">
                                    <div class="row">
                                        <label id="lblEntryNo1" runat="server" for="txtEntryNo1">Entry Number</label>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvEntryNo1" ControlToValidate="txtEntryNo1"
                                            ErrorMessage="Please enter Entry no." Display="None"
                                            ValidationGroup="Journal"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceEntryNo1" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvEntryNo1" />

                                        <asp:CustomValidator ID="cvEntryNo1" runat="server" ErrorMessage="The Entry number is already in use, please use another number!"
                                            OnServerValidate="cvEntryNo1_ServerValidate" ControlToValidate="txtEntryNo1" Display="None" SetFocusOnError="true" ValidationGroup="Journal">
                                        </asp:CustomValidator>
                                        <asp:ValidatorCalloutExtender ID="vceEntryNum1" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="cvEntryNo1" />

                                        <asp:TextBox ID="txtEntryNo1" runat="server" TabIndex="2" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="input-field col s2">
                                    <div class="row">
                                        &nbsp;
                                    </div>
                                </div>
                                <div class="input-field col s5">
                                    <div class="row">
                                        <%--<asp:UpdatePanel ID="updPnlFrequency" runat="server">
                                    <ContentTemplate>  --%>
                                        <label id="lblFrequency" runat="server" class="drpdwn-label">Frequency</label>
                                        <asp:DropDownList ID="ddlFrequency" runat="server" CssClass="browser-default"></asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvFrequency" ErrorMessage="Please select Frequency"
                                            Display="None" ControlToValidate="ddlFrequency" ValidationGroup="Journal"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceFrequency" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvFrequency" />
                                        <%--</ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="chkIsRecurr" />
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="chkIsRecurr" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-section3-blank">
                        &nbsp;
                    </div>
                    <div class="form-section3">
                        <div class="input-field col s5">
                            <div class="row">
                                <%--<label class="drpdwn-label">Frequency</label>
                                <select class="browser-default">
                                    <option>Weekly</option>
                                    <option>Monthly</option>
                                </select>--%>
                                <asp:Image ID="imgCleared" runat="server" ImageUrl="~/images/icons/Cleared.png" />
                            </div>
                        </div>
                    </div>
                </div>

                
            </div>

            <div class="grid_container">
                <div class="form-section-row" style="margin-bottom: 0 !important;">
                    <div class="RadGrid RadGrid_Material FormGrid">
                        <telerik:RadCodeBlock ID="RadCodeBlock_Journal" runat="server">
                            <script type="text/javascript">
                                function pageLoad() {
                                    var grid = $find("<%= RadGrid_Journal.ClientID %>");
                                    var columns = grid.get_masterTableView().get_columns();
                                    for (var i = 0; i < columns.length; i++) {
                                        columns[i].resizeToFit(false, true);
                                    }
                                }

                                var requestInitiator = null;
                                var selectionStart = null;

                                function requestStart(sender, args) {
                                    try {
                                        requestInitiator = document.activeElement.id;
                                        if (document.activeElement.tagName == "INPUT") {
                                            selectionStart = document.activeElement.selectionStart;
                                        }
                                    } catch (e) {

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
                                }
                            </script>
                        </telerik:RadCodeBlock>
                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Journal" runat="server">
                        </telerik:RadAjaxLoadingPanel>
                        <asp:HiddenField ID="hdnJournalJSON" runat="server" />
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Journal" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Journal" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Journal" CssClass="RadGrid_Journal" AllowFilteringByColumn="false" ShowFooter="true" PageSize="1000"
                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" FilterType="CheckList" OnPreRender="RadGrid_Journal_PreRender"
                                EnableHeaderContextMenu="false" OnItemCommand="RadGrid_Journal_ItemCommand" onblur="resetIndexF6()">
                                <CommandItemStyle />
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" Selecting-AllowRowSelect="true">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    <ClientEvents OnRowContextMenu="RowContextMenuJournalGrid" />

                                    <ClientEvents OnKeyPress="AddNewRows" />
                                </ClientSettings>
                                <HeaderStyle Width="100px"></HeaderStyle>
                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True" AllowPaging="false" CommandItemDisplay="None" EnableHierarchyExpandAll="false">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="ID" DataField="ID" UniqueName="AccountID" Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID") %>' />
                                                <asp:Label ID="lblId" runat="server" Text='<%# Container.ItemIndex %>'></asp:Label>
                                                <asp:Label ID="lblTID" runat="server" Text='<%# Eval("ID") == DBNull.Value ? "" : Eval("ID") %>'></asp:Label>

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Acct No." DataField="AcctNo" UniqueName="AcctNo" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <%--<asp:TextBox ID="txtMonth1" Width="100px" runat="server" Text='<%# Eval("Jan") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>--%>
                                                <asp:TextBox ID="txtGvAcctNo" runat="server" CssClass="textbox texttransparent searchinput" AutoCompleteType="Disabled" autocomplete="*"
                                                    MaxLength="15" Width="100%" Text='<%# Eval("AcctNo") == DBNull.Value ? "" : Eval("AcctNo") %>'></asp:TextBox>
                                                <asp:HiddenField ID="hdnIndex" runat="server" Value='<%# Container.ItemIndex %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" Text="Total" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Description" DataField="Account" UniqueName="Account" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvAcctName" runat="server" Width="100%" MaxLength="15"
                                                    autocomplete="off" Text='<%# Bind("Account") %>' onkeypress="return false;" CssClass="textbox"></asp:TextBox>
                                                <asp:HiddenField ID="hdnAcctID" Value='<%# Eval("AcctID") == DBNull.Value ? "" : Eval("AcctID") %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Company" DataField="Company" UniqueName="Company" Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvCompanyName" runat="server" Width="100%" MaxLength="15" autocomplete="off" CssClass="texttransparent" Text='<%# Bind("Company") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Memo" DataField="fDesc" UniqueName="fDesc" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvtransDes" runat="server" Width="100%" autocomplete="off" Text='<%# Bind("fDesc") %>' CssClass="textbox"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="$ Debit" DataField="Debit" UniqueName="Debit" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvDebit" runat="server" CssClass="texttransparent clsDebit textbox" autocomplete="off"
                                                    MaxLength="15" Width="100%" onchange="Balance(this)" Text='<%# DataBinder.Eval(Container.DataItem, "Debit", "{0:n}") %>' Style="text-align: right"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalDebit" runat="server" Style="text-align: right;"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="$ Credit" DataField="Credit" UniqueName="Credit" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvCredit" runat="server" CssClass="texttransparent clsCredit textbox" autocomplete="off"
                                                    MaxLength="15" Width="100%" onchange="Balance(this)" Text='<%# DataBinder.Eval(Container.DataItem, "Credit", "{0:n}") %>' Style="text-align: right"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalCredit" runat="server" Style="text-align: right;"></asp:Label>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Location Name" DataField="Loc" UniqueName="Loc" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvLoc" runat="server" CssClass="texttransparent jsearchinput textbox" AutoCompleteType="Disabled"
                                                    MaxLength="15" Width="100%" Text='<%# Eval("Loc") == DBNull.Value ? "" : Eval("Loc") %>' onchange="clearJob(this)"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Project" DataField="JobName" UniqueName="JobName" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvJob" runat="server" CssClass="texttransparent textbox"
                                                    MaxLength="15" Width="100%" Text='<%# Eval("JobName") == DBNull.Value ? "" : Eval("JobName") %>' onkeypress="return false;"></asp:TextBox>
                                                <asp:HiddenField ID="hdnJobID" Value='<%# Eval("JobID") == DBNull.Value ? "" : Eval("JobID") %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Code" DataField="Phase" UniqueName="Phase" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvPhase" runat="server" CssClass="texttransparent psearchinput textbox"
                                                    MaxLength="15" Width="100%" Text='<%# Eval("Phase") == DBNull.Value ? "" : Eval("Phase") %>' onchange="clearPhase(this)"></asp:TextBox>
                                                <asp:HiddenField ID="hdnPID" Value='<%# Eval("PhaseID") == DBNull.Value ? "" : Eval("PhaseID") %>' runat="server" />
                                                <asp:HiddenField ID="hdntypeID" Value='<%# Eval("TypeID") == DBNull.Value ? "" : Eval("TypeID") %>' runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="" AutoPostBackOnFilter="true" Visible="True" AllowFiltering="false" ShowFilterIcon="false" ItemStyle-Width="25px" HeaderStyle-Width="25px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibDelete" runat="server" CommandName="DeleteTransaction"
                                                    CommandArgument="<%# Container.ItemIndex %>" ImageUrl="~/images/glyphicons-17-bin.png" Width="13px" OnClientClick="itemJSON();" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <%--<FilterMenu CssClass="RadFilterMenu_CheckList">
                                </FilterMenu>--%>
                            </telerik:RadGrid>

                            <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none" CausesValidation="False" />
                            <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="errorWindow" Skin="Material" VisibleTitlebar="true" Title="Error List" Behaviors="Default" CenterIfModal="true"
                                        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                        runat="server" Modal="true" Width="1000" Height="600">
                                        <ContentTemplate>
                                            <div style="margin-top: 15px;">
                                                <div class="col-lg-12 col-md-12 form-section-row">
                                                    <div style="float: right;">
                                                        <span>Total Rows :
                                                <asp:Label ID="lblTotalRows" runat="server" />
                                                            |</span>
                                                        <span style="color: green">Valid Rows :
                                                <asp:Label ID="lblValidRows" runat="server" />
                                                            |</span>
                                                        <span style="color: red">Invalid Rows :
                                                <asp:Label ID="lblInvalidRows" runat="server" /></span>
                                                    </div>
                                                </div>
                                                <div style="clear: both;"></div>
                                                <div class="RadGrid" style="max-height: 380px !important; overflow: auto;">
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
                                                                <telerik:GridTemplateColumn DataField="AccNo" SortExpression="AccNo" AutoPostBackOnFilter="true"
                                                                    HeaderText="Acc No." ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAccNo" Text='<%# Bind("AccNo") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn DataField="Memo" SortExpression="Memo" AutoPostBackOnFilter="true"
                                                                    HeaderText="Memo" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMemo" Text='<%# Bind("Memo") %>' runat="server" />
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
                                                    <div class="btnlinks" style="float: right;">
                                                        <asp:LinkButton Text="Continue" runat="server" ID="btnContinue" OnClick="btnContinue_Click" />
                                                        <asp:LinkButton Text="Cancel" runat="server" ID="btnCancel" OnClick="btnCancel_Click" />
                                                    </div>
                                                </footer>
                                            </div>
                                        </ContentTemplate>
                                    </telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>

                            <input type="hidden" runat="server" id="radGridClickedRowIndex" name="radGridClickedRowIndex" />
                            <telerik:RadContextMenu ID="RadMenuJournalEntryGrid" runat="server" OnItemClick="RadMenuJournalEntryGrid_ItemClick" OnClientItemClicking="CheckAddRowGrid"
                                EnableRoundedCorners="true" EnableShadows="true" EnableScreenBoundaryDetection="false">
                                <Items>
                                    <telerik:RadMenuItem Text="Add Row Above">
                                    </telerik:RadMenuItem>
                                    <telerik:RadMenuItem Text="Add Row Below">
                                    </telerik:RadMenuItem>
                                </Items>
                            </telerik:RadContextMenu>
                        </telerik:RadAjaxPanel>
                    </div>
                </div>
            </div>
            <div class="btnlinks">
                <asp:LinkButton ID="btnAddNewLines" runat="server" CausesValidation="false"
                    OnClick="lbtnAddNewLines_Click" Text="Add New Lines"
                    OnClientClick="itemJSON();" />
                <asp:LinkButton ID="btnCopyPrevious" runat="server" CausesValidation="false" OnClientClick="itemJSON();"
                    OnClick="btnCopyPrevious_Click" Text="Copy Previous" Style="display: none;"></asp:LinkButton>
                <asp:LinkButton ID="btnUpdateGridData" runat="server" CausesValidation="false"
                    OnClick="lbtnUpdateGridData_Click" Text=""
                    OnClientClick="itemJSON();" Style="display: none;" />
            </div>

            
        </div>

        <telerik:RadWindow ID="RadAddAccountWindow" Skin="Material" runat="server" Modal="true" Width="800" Height="600" Title="Add New Account">
            <ContentTemplate>
                <asp:UpdatePanel ID="Updatepanel1" runat="server">
                    <ContentTemplate>
                        <div class="row top-fix">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkSaveAccount" OnClick="lnkSaveAccount_Click" runat="server" ValidationGroup="addNewAccount">Save</asp:LinkButton>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 35px">
                            <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                                <li class="active">
                                    <div id="accrdAccount" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>Account Info</div>
                                    <div class="collapsible-body" style="display: block;">
                                        <div class="form-content-wrap">
                                            <div class="form-content-pd">
                                                <div class="form-section-row">
                                                    <div class="section-ttle">Account Details</div>
                                                    <div class="form-section2">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Account Type</label>
                                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="browser-default" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvAccountNum"
                                                                    runat="server" ControlToValidate="txtAcctNum" Display="None" ErrorMessage="Account # is Required"
                                                                    SetFocusOnError="True" ValidationGroup="addNewAccount">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender
                                                                    ID="vceAcctNum" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="rfvAccountNum" />
                                                                <asp:CustomValidator ID="cvAccountNum" runat="server" ErrorMessage="Acct# already exists, please use different Acct# !"
                                                                    OnServerValidate="cvAccountNum_ServerValidate" ControlToValidate="txtAcctNum" Display="None" SetFocusOnError="true" ValidationGroup="addNewAccount">
                                                                </asp:CustomValidator>
                                                                <asp:ValidatorCalloutExtender ID="vceAcctNum1" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="cvAccountNum" />
                                                                <asp:TextBox ID="txtAcctNum" runat="server" MaxLength="15" autocomplete="off"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtAcctNum" AssociatedControlID="txtAcctNum" for="txtAcctNum">Account Number</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvAcName"
                                                                    runat="server" ControlToValidate="txtAcName" Display="None" ErrorMessage="Account Name is Required"
                                                                    SetFocusOnError="True" ValidationGroup="addNewAccount">
                                                                </asp:RequiredFieldValidator>
                                                                 <asp:ValidatorCalloutExtender
                                                                    ID="vceAcctName" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="rfvAcName" />
                                                                <asp:CustomValidator ID="cvAcName" runat="server" ErrorMessage="Account Name already exists, please use different Account Name !"
                                                                    OnServerValidate="cvAcName_ServerValidate" ControlToValidate="txtAcName" Display="None" SetFocusOnError="true" ValidationGroup="addNewAccount">
                                                                </asp:CustomValidator>
                                                                <asp:ValidatorCalloutExtender
                                                                    ID="vceAcName" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="cvAcName" />
                                                                <asp:TextBox ID="txtAcName" runat="server" MaxLength="75" autocomplete="off"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtAcName" AssociatedControlID="txtAcName" for="txtAcName">Account Name</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" ID="lbltxtDescription" AssociatedControlID="txtAcctDescription">Description</asp:Label>
                                                                <asp:TextBox ID="txtAcctDescription" runat="server" CssClass="materialize-textarea" TextMode="MultiLine" MaxLength="75"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section2-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section2">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Status</label>
                                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Center</label>
                                                                <asp:DropDownList ID="ddlCentral" runat="server" CssClass="browser-default" TabIndex="5">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Sub Category</label>
                                                                <asp:DropDownList ID="ddlSubAcCategory" runat="server" CssClass="browser-default"
                                                                    AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12" id="dvCompanyPermission" runat="server">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Company</label>
                                                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="browser-default">
                                                                </asp:DropDownList>
                                                                <%-- <asp:LinkButton runat="server" ID="btnCompanyPopUp" OnClick="btnCompanyPopUp_Click">Change Company</asp:LinkButton>--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                        </div>
                                    </div>
                                </li>
                                <li class="accrd-bank-info active" style="display:none;">
                                    <div id="accrdgeneral" class="collapsible-header accrd active accordian-text-custom"><i class="mdi-action-perm-data-setting"></i>Bank Info</div>
                                    <div class="collapsible-body" style="display: block;">
                                        <div class="form-content-wrap">
                                            <div class="form-content-pd">
                                                <div class="form-section-row" id="pnlBankAccount" runat="server">
                                                    <div class="section-ttle">Bank Details</div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox runat="server" ID="txtBankName"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtBankName" AssociatedControlID="txtBankName">Bank Name</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:Label runat="server" ID="lbltxtAddress" AssociatedControlID="txtAddress">Address</asp:Label>
                                                                <asp:TextBox ID="txtAddress" placeholder="" CssClass="materialize-textarea" runat="server" MaxLength="15" TextMode="MultiLine"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCity" runat="server" MaxLength="250"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtCity" AssociatedControlID="txtCity">City</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label class="drpdwn-label">State</label>
                                                                <asp:DropDownList ID="ddlState" runat="server" CssClass="browser-default">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtZip" runat="server" MaxLength="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtZip" AssociatedControlID="txtZip">Zip</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Country</label>
                                                                <asp:DropDownList ID="ddlCountry" runat="server" ToolTip="Country" CssClass="browser-default">
                                                                    <asp:ListItem Value="Select">Select</asp:ListItem>
                                                                    <asp:ListItem Value="US">United States</asp:ListItem>
                                                                    <asp:ListItem Value="CA">Canada</asp:ListItem>
                                                                    <asp:ListItem Value="AF">Afghanistan</asp:ListItem>
                                                                    <asp:ListItem Value="AL">Albania</asp:ListItem>
                                                                    <asp:ListItem Value="DZ">Algeria</asp:ListItem>
                                                                    <asp:ListItem Value="AS">American Samoa</asp:ListItem>
                                                                    <asp:ListItem Value="AD">Andorra</asp:ListItem>
                                                                    <asp:ListItem Value="AO">Angola</asp:ListItem>
                                                                    <asp:ListItem Value="AI">Anguilla</asp:ListItem>
                                                                    <asp:ListItem Value="AQ">Antarctica</asp:ListItem>
                                                                    <asp:ListItem Value="AG">Antigua And Barbuda</asp:ListItem>
                                                                    <asp:ListItem Value="AR">Argentina</asp:ListItem>
                                                                    <asp:ListItem Value="AM">Armenia</asp:ListItem>
                                                                    <asp:ListItem Value="AW">Aruba</asp:ListItem>
                                                                    <asp:ListItem Value="AU">Australia</asp:ListItem>
                                                                    <asp:ListItem Value="AT">Austria</asp:ListItem>
                                                                    <asp:ListItem Value="AZ">Azerbaijan</asp:ListItem>
                                                                    <asp:ListItem Value="BS">Bahamas</asp:ListItem>
                                                                    <asp:ListItem Value="BH">Bahrain</asp:ListItem>
                                                                    <asp:ListItem Value="BD">Bangladesh</asp:ListItem>
                                                                    <asp:ListItem Value="BB">Barbados</asp:ListItem>
                                                                    <asp:ListItem Value="BY">Belarus</asp:ListItem>
                                                                    <asp:ListItem Value="BE">Belgium</asp:ListItem>
                                                                    <asp:ListItem Value="BZ">Belize</asp:ListItem>
                                                                    <asp:ListItem Value="BJ">Benin</asp:ListItem>
                                                                    <asp:ListItem Value="BM">Bermuda</asp:ListItem>
                                                                    <asp:ListItem Value="BT">Bhutan</asp:ListItem>
                                                                    <asp:ListItem Value="BO">Bolivia</asp:ListItem>
                                                                    <asp:ListItem Value="BA">Bosnia And Herzegowina</asp:ListItem>
                                                                    <asp:ListItem Value="BW">Botswana</asp:ListItem>
                                                                    <asp:ListItem Value="BV">Bouvet Island</asp:ListItem>
                                                                    <asp:ListItem Value="BR">Brazil</asp:ListItem>
                                                                    <asp:ListItem Value="IO">British Indian Ocean Territory</asp:ListItem>
                                                                    <asp:ListItem Value="BN">Brunei Darussalam</asp:ListItem>
                                                                    <asp:ListItem Value="BG">Bulgaria</asp:ListItem>
                                                                    <asp:ListItem Value="BF">Burkina Faso</asp:ListItem>
                                                                    <asp:ListItem Value="BI">Burundi</asp:ListItem>
                                                                    <asp:ListItem Value="KH">Cambodia</asp:ListItem>
                                                                    <asp:ListItem Value="CM">Cameroon</asp:ListItem>
                                                                    <asp:ListItem Value="CV">Cape Verde</asp:ListItem>
                                                                    <asp:ListItem Value="KY">Cayman Islands</asp:ListItem>
                                                                    <asp:ListItem Value="CF">Central African Republic</asp:ListItem>
                                                                    <asp:ListItem Value="TD">Chad</asp:ListItem>
                                                                    <asp:ListItem Value="CL">Chile</asp:ListItem>
                                                                    <asp:ListItem Value="CN">China</asp:ListItem>
                                                                    <asp:ListItem Value="CX">Christmas Island</asp:ListItem>
                                                                    <asp:ListItem Value="CC">Cocos (Keeling) Islands</asp:ListItem>
                                                                    <asp:ListItem Value="CO">Colombia</asp:ListItem>
                                                                    <asp:ListItem Value="KM">Comoros</asp:ListItem>
                                                                    <asp:ListItem Value="CG">Congo</asp:ListItem>
                                                                    <asp:ListItem Value="CK">Cook Islands</asp:ListItem>
                                                                    <asp:ListItem Value="CR">Costa Rica</asp:ListItem>
                                                                    <asp:ListItem Value="CI">Cote D'Ivoire</asp:ListItem>
                                                                    <asp:ListItem Value="HR">Croatia (Local Name: Hrvatska)</asp:ListItem>
                                                                    <asp:ListItem Value="CU">Cuba</asp:ListItem>
                                                                    <asp:ListItem Value="CY">Cyprus</asp:ListItem>
                                                                    <asp:ListItem Value="CZ">Czech Republic</asp:ListItem>
                                                                    <asp:ListItem Value="DK">Denmark</asp:ListItem>
                                                                    <asp:ListItem Value="DJ">Djibouti</asp:ListItem>
                                                                    <asp:ListItem Value="DM">Dominica</asp:ListItem>
                                                                    <asp:ListItem Value="DO">Dominican Republic</asp:ListItem>
                                                                    <asp:ListItem Value="TP">East Timor</asp:ListItem>
                                                                    <asp:ListItem Value="EC">Ecuador</asp:ListItem>
                                                                    <asp:ListItem Value="EG">Egypt</asp:ListItem>
                                                                    <asp:ListItem Value="SV">El Salvador</asp:ListItem>
                                                                    <asp:ListItem Value="GQ">Equatorial Guinea</asp:ListItem>
                                                                    <asp:ListItem Value="ER">Eritrea</asp:ListItem>
                                                                    <asp:ListItem Value="EE">Estonia</asp:ListItem>
                                                                    <asp:ListItem Value="ET">Ethiopia</asp:ListItem>
                                                                    <asp:ListItem Value="FK">Falkland Islands (Malvinas)</asp:ListItem>
                                                                    <asp:ListItem Value="FO">Faroe Islands</asp:ListItem>
                                                                    <asp:ListItem Value="FJ">Fiji</asp:ListItem>
                                                                    <asp:ListItem Value="FI">Finland</asp:ListItem>
                                                                    <asp:ListItem Value="FR">France</asp:ListItem>
                                                                    <asp:ListItem Value="GF">French Guiana</asp:ListItem>
                                                                    <asp:ListItem Value="PF">French Polynesia</asp:ListItem>
                                                                    <asp:ListItem Value="TF">French Southern Territories</asp:ListItem>
                                                                    <asp:ListItem Value="GA">Gabon</asp:ListItem>
                                                                    <asp:ListItem Value="GM">Gambia</asp:ListItem>
                                                                    <asp:ListItem Value="GE">Georgia</asp:ListItem>
                                                                    <asp:ListItem Value="DE">Germany</asp:ListItem>
                                                                    <asp:ListItem Value="GH">Ghana</asp:ListItem>
                                                                    <asp:ListItem Value="GI">Gibraltar</asp:ListItem>
                                                                    <asp:ListItem Value="GR">Greece</asp:ListItem>
                                                                    <asp:ListItem Value="GL">Greenland</asp:ListItem>
                                                                    <asp:ListItem Value="GD">Grenada</asp:ListItem>
                                                                    <asp:ListItem Value="GP">Guadeloupe</asp:ListItem>
                                                                    <asp:ListItem Value="GU">Guam</asp:ListItem>
                                                                    <asp:ListItem Value="GT">Guatemala</asp:ListItem>
                                                                    <asp:ListItem Value="GN">Guinea</asp:ListItem>
                                                                    <asp:ListItem Value="GW">Guinea-Bissau</asp:ListItem>
                                                                    <asp:ListItem Value="GY">Guyana</asp:ListItem>
                                                                    <asp:ListItem Value="HT">Haiti</asp:ListItem>
                                                                    <asp:ListItem Value="HM">Heard And Mc Donald Islands</asp:ListItem>
                                                                    <asp:ListItem Value="VA">Holy See (Vatican City State)</asp:ListItem>
                                                                    <asp:ListItem Value="HN">Honduras</asp:ListItem>
                                                                    <asp:ListItem Value="HK">Hong Kong</asp:ListItem>
                                                                    <asp:ListItem Value="HU">Hungary</asp:ListItem>
                                                                    <asp:ListItem Value="IS">Icel And</asp:ListItem>
                                                                    <asp:ListItem Value="IN">India</asp:ListItem>
                                                                    <asp:ListItem Value="ID">Indonesia</asp:ListItem>
                                                                    <asp:ListItem Value="IR">Iran (Islamic Republic Of)</asp:ListItem>
                                                                    <asp:ListItem Value="IQ">Iraq</asp:ListItem>
                                                                    <asp:ListItem Value="IE">Ireland</asp:ListItem>
                                                                    <asp:ListItem Value="IL">Israel</asp:ListItem>
                                                                    <asp:ListItem Value="IT">Italy</asp:ListItem>
                                                                    <asp:ListItem Value="JM">Jamaica</asp:ListItem>
                                                                    <asp:ListItem Value="JP">Japan</asp:ListItem>
                                                                    <asp:ListItem Value="JO">Jordan</asp:ListItem>
                                                                    <asp:ListItem Value="KZ">Kazakhstan</asp:ListItem>
                                                                    <asp:ListItem Value="KE">Kenya</asp:ListItem>
                                                                    <asp:ListItem Value="KI">Kiribati</asp:ListItem>
                                                                    <asp:ListItem Value="KP">Korea, Dem People'S Republic</asp:ListItem>
                                                                    <asp:ListItem Value="KR">Korea, Republic Of</asp:ListItem>
                                                                    <asp:ListItem Value="KW">Kuwait</asp:ListItem>
                                                                    <asp:ListItem Value="KG">Kyrgyzstan</asp:ListItem>
                                                                    <asp:ListItem Value="LA">Lao People'S Dem Republic</asp:ListItem>
                                                                    <asp:ListItem Value="LV">Latvia</asp:ListItem>
                                                                    <asp:ListItem Value="LB">Lebanon</asp:ListItem>
                                                                    <asp:ListItem Value="LS">Lesotho</asp:ListItem>
                                                                    <asp:ListItem Value="LR">Liberia</asp:ListItem>
                                                                    <asp:ListItem Value="LY">Libyan Arab Jamahiriya</asp:ListItem>
                                                                    <asp:ListItem Value="LI">Liechtenstein</asp:ListItem>
                                                                    <asp:ListItem Value="LT">Lithuania</asp:ListItem>
                                                                    <asp:ListItem Value="LU">Luxembourg</asp:ListItem>
                                                                    <asp:ListItem Value="MO">Macau</asp:ListItem>
                                                                    <asp:ListItem Value="MK">Macedonia</asp:ListItem>
                                                                    <asp:ListItem Value="MG">Madagascar</asp:ListItem>
                                                                    <asp:ListItem Value="MW">Malawi</asp:ListItem>
                                                                    <asp:ListItem Value="MY">Malaysia</asp:ListItem>
                                                                    <asp:ListItem Value="MV">Maldives</asp:ListItem>
                                                                    <asp:ListItem Value="ML">Mali</asp:ListItem>
                                                                    <asp:ListItem Value="MT">Malta</asp:ListItem>
                                                                    <asp:ListItem Value="MH">Marshall Islands</asp:ListItem>
                                                                    <asp:ListItem Value="MQ">Martinique</asp:ListItem>
                                                                    <asp:ListItem Value="MR">Mauritania</asp:ListItem>
                                                                    <asp:ListItem Value="MU">Mauritius</asp:ListItem>
                                                                    <asp:ListItem Value="YT">Mayotte</asp:ListItem>
                                                                    <asp:ListItem Value="MX">Mexico</asp:ListItem>
                                                                    <asp:ListItem Value="FM">Micronesia, Federated States</asp:ListItem>
                                                                    <asp:ListItem Value="MD">Moldova, Republic Of</asp:ListItem>
                                                                    <asp:ListItem Value="MC">Monaco</asp:ListItem>
                                                                    <asp:ListItem Value="MN">Mongolia</asp:ListItem>
                                                                    <asp:ListItem Value="MS">Montserrat</asp:ListItem>
                                                                    <asp:ListItem Value="MA">Morocco</asp:ListItem>
                                                                    <asp:ListItem Value="MZ">Mozambique</asp:ListItem>
                                                                    <asp:ListItem Value="MM">Myanmar</asp:ListItem>
                                                                    <asp:ListItem Value="NA">Namibia</asp:ListItem>
                                                                    <asp:ListItem Value="NR">Nauru</asp:ListItem>
                                                                    <asp:ListItem Value="NP">Nepal</asp:ListItem>
                                                                    <asp:ListItem Value="NL">Netherlands</asp:ListItem>
                                                                    <asp:ListItem Value="AN">Netherlands Ant Illes</asp:ListItem>
                                                                    <asp:ListItem Value="NC">New Caledonia</asp:ListItem>
                                                                    <asp:ListItem Value="NZ">New Zealand</asp:ListItem>
                                                                    <asp:ListItem Value="NI">Nicaragua</asp:ListItem>
                                                                    <asp:ListItem Value="NE">Niger</asp:ListItem>
                                                                    <asp:ListItem Value="NG">Nigeria</asp:ListItem>
                                                                    <asp:ListItem Value="NU">Niue</asp:ListItem>
                                                                    <asp:ListItem Value="NF">Norfolk Island</asp:ListItem>
                                                                    <asp:ListItem Value="MP">Northern Mariana Islands</asp:ListItem>
                                                                    <asp:ListItem Value="NO">Norway</asp:ListItem>
                                                                    <asp:ListItem Value="OM">Oman</asp:ListItem>
                                                                    <asp:ListItem Value="PK">Pakistan</asp:ListItem>
                                                                    <asp:ListItem Value="PW">Palau</asp:ListItem>
                                                                    <asp:ListItem Value="PA">Panama</asp:ListItem>
                                                                    <asp:ListItem Value="PG">Papua New Guinea</asp:ListItem>
                                                                    <asp:ListItem Value="PY">Paraguay</asp:ListItem>
                                                                    <asp:ListItem Value="PE">Peru</asp:ListItem>
                                                                    <asp:ListItem Value="PH">Philippines</asp:ListItem>
                                                                    <asp:ListItem Value="PN">Pitcairn</asp:ListItem>
                                                                    <asp:ListItem Value="PL">Poland</asp:ListItem>
                                                                    <asp:ListItem Value="PT">Portugal</asp:ListItem>
                                                                    <asp:ListItem Value="PR">Puerto Rico</asp:ListItem>
                                                                    <asp:ListItem Value="QA">Qatar</asp:ListItem>
                                                                    <asp:ListItem Value="RE">Reunion</asp:ListItem>
                                                                    <asp:ListItem Value="RO">Romania</asp:ListItem>
                                                                    <asp:ListItem Value="RU">Russian Federation</asp:ListItem>
                                                                    <asp:ListItem Value="RW">Rwanda</asp:ListItem>
                                                                    <asp:ListItem Value="KN">Saint K Itts And Nevis</asp:ListItem>
                                                                    <asp:ListItem Value="LC">Saint Lucia</asp:ListItem>
                                                                    <asp:ListItem Value="VC">Saint Vincent, The Grenadines</asp:ListItem>
                                                                    <asp:ListItem Value="WS">Samoa</asp:ListItem>
                                                                    <asp:ListItem Value="SM">San Marino</asp:ListItem>
                                                                    <asp:ListItem Value="ST">Sao Tome And Principe</asp:ListItem>
                                                                    <asp:ListItem Value="SA">Saudi Arabia</asp:ListItem>
                                                                    <asp:ListItem Value="SN">Senegal</asp:ListItem>
                                                                    <asp:ListItem Value="SC">Seychelles</asp:ListItem>
                                                                    <asp:ListItem Value="SL">Sierra Leone</asp:ListItem>
                                                                    <asp:ListItem Value="SG">Singapore</asp:ListItem>
                                                                    <asp:ListItem Value="SK">Slovakia (Slovak Republic)</asp:ListItem>
                                                                    <asp:ListItem Value="SI">Slovenia</asp:ListItem>
                                                                    <asp:ListItem Value="SB">Solomon Islands</asp:ListItem>
                                                                    <asp:ListItem Value="SO">Somalia</asp:ListItem>
                                                                    <asp:ListItem Value="ZA">South Africa</asp:ListItem>
                                                                    <asp:ListItem Value="GS">South Georgia , S Sandwich Is.</asp:ListItem>
                                                                    <asp:ListItem Value="ES">Spain</asp:ListItem>
                                                                    <asp:ListItem Value="LK">Sri Lanka</asp:ListItem>
                                                                    <asp:ListItem Value="SH">St. Helena</asp:ListItem>
                                                                    <asp:ListItem Value="PM">St. Pierre And Miquelon</asp:ListItem>
                                                                    <asp:ListItem Value="SD">Sudan</asp:ListItem>
                                                                    <asp:ListItem Value="SR">Suriname</asp:ListItem>
                                                                    <asp:ListItem Value="SJ">Svalbard, Jan Mayen Islands</asp:ListItem>
                                                                    <asp:ListItem Value="SZ">Sw Aziland</asp:ListItem>
                                                                    <asp:ListItem Value="SE">Sweden</asp:ListItem>
                                                                    <asp:ListItem Value="CH">Switzerland</asp:ListItem>
                                                                    <asp:ListItem Value="SY">Syrian Arab Republic</asp:ListItem>
                                                                    <asp:ListItem Value="TW">Taiwan</asp:ListItem>
                                                                    <asp:ListItem Value="TJ">Tajikistan</asp:ListItem>
                                                                    <asp:ListItem Value="TZ">Tanzania, United Republic Of</asp:ListItem>
                                                                    <asp:ListItem Value="TH">Thailand</asp:ListItem>
                                                                    <asp:ListItem Value="TG">Togo</asp:ListItem>
                                                                    <asp:ListItem Value="TK">Tokelau</asp:ListItem>
                                                                    <asp:ListItem Value="TO">Tonga</asp:ListItem>
                                                                    <asp:ListItem Value="TT">Trinidad And Tobago</asp:ListItem>
                                                                    <asp:ListItem Value="TN">Tunisia</asp:ListItem>
                                                                    <asp:ListItem Value="TR">Turkey</asp:ListItem>
                                                                    <asp:ListItem Value="TM">Turkmenistan</asp:ListItem>
                                                                    <asp:ListItem Value="TC">Turks And Caicos Islands</asp:ListItem>
                                                                    <asp:ListItem Value="TV">Tuvalu</asp:ListItem>
                                                                    <asp:ListItem Value="UG">Uganda</asp:ListItem>
                                                                    <asp:ListItem Value="UA">Ukraine</asp:ListItem>
                                                                    <asp:ListItem Value="AE">United Arab Emirates</asp:ListItem>
                                                                    <asp:ListItem Value="GB">United Kingdom</asp:ListItem>
                                                                    <asp:ListItem Value="UM">United States Minor Is.</asp:ListItem>
                                                                    <asp:ListItem Value="UY">Uruguay</asp:ListItem>
                                                                    <asp:ListItem Value="UZ">Uzbekistan</asp:ListItem>
                                                                    <asp:ListItem Value="VU">Vanuatu</asp:ListItem>
                                                                    <asp:ListItem Value="VE">Venezuela</asp:ListItem>
                                                                    <asp:ListItem Value="VN">Viet Nam</asp:ListItem>
                                                                    <asp:ListItem Value="VG">Virgin Islands (British)</asp:ListItem>
                                                                    <asp:ListItem Value="VI">Virgin Islands (U.S.)</asp:ListItem>
                                                                    <asp:ListItem Value="WF">Wallis And Futuna Islands</asp:ListItem>
                                                                    <asp:ListItem Value="EH">Western Sahara</asp:ListItem>
                                                                    <asp:ListItem Value="YE">Yemen</asp:ListItem>
                                                                    <asp:ListItem Value="YU">Yugoslavia</asp:ListItem>
                                                                    <asp:ListItem Value="ZR">Zaire</asp:ListItem>
                                                                    <asp:ListItem Value="ZM">Zambia</asp:ListItem>
                                                                    <asp:ListItem Value="ZW">Zimbabwe</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="lat">Latitude <span class="reqd">*</span></asp:Label>
                                                                <asp:TextBox runat="server" ID="lat"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            &nbsp;
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:Label runat="server" AssociatedControlID="lng">Longitude <span class="reqd">*</span></asp:Label>
                                                                <asp:TextBox runat="server" ID="lng"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <div id="map" style="overflow: hidden !important; height: 170px!important;">
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section-row" runat="server" id="pnlBankAccount2">
                                                    <div class="section-ttle">Main Contact Details</div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtContact" runat="server" MaxLength="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtContact" AssociatedControlID="txtContact"> Contact Name</asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:MaskedEditExtender ID="txtPhoneCust_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999999999"
                                                                    MaskType="Number" TargetControlID="txtPhone">
                                                                </asp:MaskedEditExtender>
                                                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtPhone" AssociatedControlID="txtPhone">Phone</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:MaskedEditExtender ID="txtFax_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999999999"
                                                                    MaskType="Number" TargetControlID="txtFax">
                                                                </asp:MaskedEditExtender>
                                                                <asp:TextBox ID="txtFax" runat="server" MaxLength="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtFax" AssociatedControlID="txtFax">Fax</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                 <asp:MaskedEditExtender ID="txtCellular_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999999999"
                                                                    MaskType="Number" TargetControlID="txtCellular">
                                                                </asp:MaskedEditExtender>
                                                                <asp:TextBox ID="txtCellular" runat="server" MaxLength="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtCellular" AssociatedControlID="txtCellular">Cellular</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                                                    ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="addNewAccount">
                                                                </asp:RegularExpressionValidator>
                                                                <asp:ValidatorCalloutExtender ID="vceEmail" runat="server" Enabled="True"
                                                                    TargetControlID="revEmail">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="150"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtEmail" AssociatedControlID="txtEmail">Email</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtWebsite" runat="server" MaxLength="1000"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtWebsite" AssociatedControlID="txtWebsite"> Web address</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section-row">
                                                    <div class="section-ttle">Bank Account Details</div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtBranch" runat="server" MaxLength="15"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtBranch" AssociatedControlID="txtBranch">Branch Number</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtAcct" runat="server" MaxLength="15" autocomplete="off"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtAcct" AssociatedControlID="txtAcct">Account Number</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtRoute" runat="server" MaxLength="9"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtRoute" AssociatedControlID="txtRoute"> Route Number</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCreditLimit" runat="server" MaxLength="13" onkeypress="return isDecimalKey(event,this)"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtCreditLimit" AssociatedControlID="txtCreditLimit">Credit Limit</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="checkrow">
                                                                <asp:CheckBox ID="chkWarn" runat="server" TabIndex="18" Text="Warn on Overdraft" />
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtNCheck" runat="server" MaxLength="9" onkeypress="return isNumberKey(event,this)" autocomplete="off"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtNCheck" AssociatedControlID="txtNCheck">Start Check Number</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtNDeposit" runat="server" MaxLength="9" onkeypress="return isNumberKey(event,this)" autocomplete="off"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtNDeposit" AssociatedControlID="txtNDeposit">Start Deposit Number</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtNEPay" runat="server" MaxLength="9" onkeypress="return isNumberKey(event,this)" autocomplete="off"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtNEPay" AssociatedControlID="txtNEPay">Start EPay Number</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99.99" ClearMaskOnLostFocus="true"
                                                                    MaskType="Number" TargetControlID="txtRate" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="True">
                                                                </asp:MaskedEditExtender>
                                                                <asp:TextBox ID="txtRate" runat="server" MaxLength="5"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtRate" AssociatedControlID="txtRate">Int Rate</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtReconciled" runat="server" MaxLength="15" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lbltxtReconciled" AssociatedControlID="txtReconciled">Reconciled</asp:Label>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlType" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </ContentTemplate>
        </telerik:RadWindow>
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

                                                            <%--<telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="Contains" HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>--%>

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
                                                            try {
                                                                requestInitiator = document.activeElement.id;
                                                                if (document.activeElement.tagName == "INPUT") {
                                                                    selectionStart = document.activeElement.selectionStart;
                                                                }
                                                            } catch (e) {

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
    <asp:HiddenField runat="server" ID="hdnSelectPOIndex" />
    <asp:HiddenField runat="server" ID="hdnCon" />

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />
    <asp:HiddenField ID="hdnIsAutoCompleteSelected" ClientIDMode="Static" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        ///-Document permission

        function AddDocumentClick(hyperlink) {

            var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
            if (IsAdd == "Y") {
                debugger;
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
        <%--function ConfirmUpload(value) {
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

            Materialize.updateTextFields();
        });
    </script>
    <script>
        function AddNewRows(sender, eventArgs) {

            var $focused = $(':focus');
            var flag = 0
            try {
                if ($focused[0].id.indexOf("txtGvAcctNo") !== -1) {
                    flag = 1
                }
            } catch (ex) {
                flag = 0;
            }

            if (eventArgs.get_keyCode() == 40) {
                if (flag == 0) {
                    $("#<%= hdnAddRowValue.ClientID %>").val(1);
                    document.getElementById('<%=btnAddNewLines.ClientID%>').click();
                }

            }

        }

        function pageLoad(sender, args) {
            Materialize.updateTextFields();
            $(window.document).keydown(function (event) {

                var $focused = $(':focus');
                var flag = 0

                if ($focused.length > 0 && $focused[0].id.indexOf('<%=RadGrid_Journal.ClientID%>') !== -1) {
                    flag = 1
                }

                if (event.which == 117 && flag == 1) {
                    document.getElementById('<%=btnCopyPrevious.ClientID%>').click();
                    return false;
                }
            })

            $("[id*=txtGvAcctNo]").focus(function () {

                var txtGvAcctNo = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvAcctNo.replace('txtGvAcctNo', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
                $(hdnSelectPOIndex).val(hdnIndex.value);

            });
            $("[id*=txtGvAcctName]").focus(function () {
                var txtGvAcctName = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvAcctName.replace('txtGvAcctName', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
                $(hdnSelectPOIndex).val(hdnIndex.value);

            });
            $("[id*=txtGvtransDes]").focus(function () {
                var txtGvtransDes = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvtransDes.replace('txtGvtransDes', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
                $(hdnSelectPOIndex).val(hdnIndex.value);

            });
            $("[id*=txtGvDebit]").focus(function () {
                var txtGvDebit = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvDebit.replace('txtGvDebit', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
                $(hdnSelectPOIndex).val(hdnIndex.value);

            });
            $("[id*=txtGvCredit]").focus(function () {
                var txtGvCredit = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvCredit.replace('txtGvCredit', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
                $(hdnSelectPOIndex).val(hdnIndex.value);

            });
            $("[id*=txtGvLoc]").focus(function () {
                var txtGvLoc = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvLoc.replace('txtGvLoc', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
                $(hdnSelectPOIndex).val(hdnIndex.value);
            });
            $("[id*=txtGvJob]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
                $(hdnSelectPOIndex).val(hdnIndex.value);

            });

            BalanceProof();
            function dtaa() {
                this.prefixText = null;
                this.con = null;
            }
            //txtGvAcctName
            $("[id*=txtGvAcctNo]").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetAccountNameJE",
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
                    console.log(ui.item);
                    if (ui.item.value == 0 || ui.item.value == ' < Add New > ') {
                        itemJSON();
                        var oWnd = $find("<%=RadAddAccountWindow.ClientID%>");
                        oWnd.show();
                    }
                    else {
                        var txtGvAcctName = this.id;
                        var hdnAcctID = document.getElementById(txtGvAcctName.replace('txtGvAcctNo', 'hdnAcctID'));
                        var txtGvCompanyName = document.getElementById(txtGvAcctName.replace('txtGvAcctNo', 'txtGvCompanyName'));
                        var txtGvAcctName = document.getElementById(txtGvAcctName.replace('txtGvAcctNo', 'txtGvAcctName'));

                        $(txtGvAcctName).val(ui.item.label);
                        $(txtGvCompanyName).val(ui.item.Company);
                        $(hdnAcctID).val(ui.item.value);
                        $(this).val(ui.item.acct);

                        CopyDescToMomo();
                    }

                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.acct);
                    return false;
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
                            .data("item.ui-autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.ui-autocomplete", item)
                            .append("<a>" + result_item + ", <span>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
            });

            $("[id*=txtGvPhase]").autocomplete({
                source: function (request, response) {
                    var curr_control = this.element.attr('id');
                    var job = document.getElementById(curr_control.replace('txtGvPhase', 'hdnJobID'));

                    var dtaaa = new dtaa();
                    dtaaa.jobID = document.getElementById(job.id).value;
                    dtaaa.prefixText = request.term;
                    
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetAllPhase",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load phase details");
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
                    var hdnPID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnPID'));
                    var hdntypeID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdntypeID'));
                    var txtGvLoc = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvLoc'));
                    var txtGvJob = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvJob'));
                    var hdnJobID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnJobID'));

                    $(hdnPID).val(ui.item.Line);
                    $(this).val(ui.item.Desc);
                    $(txtGvLoc).val(ui.item.LocName);
                    $(txtGvJob).val(ui.item.Job+' '+ui.item.JobName);
                    $(hdnJobID).val(ui.item.Job);
                    $(hdntypeID).val(ui.item.PhaseType);
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
            })
            $.each($(".psearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    //var ula = ul;
                    //var itema = item;
                    //var result_value = item.Line;
                    //var result_item = item.Desc;
                    //var result_desc = item.Line;
                    //var result_type = item.PhaseType;
                    //var result_bomType = item.bomType;
                    //var result_TypeID = item.PhaseType;

                    //if (result_value == 0) {
                    //    return $("<li></li>")
                    //        .data("item.ui-autocomplete", item)
                    //        .append("<a>" + result_item + "</a>")

                    //        .appendTo(ul);
                    //}
                    //else {
                    //    if (result_type == "0")
                    //        result_type = "Revenue";
                    //    else if (result_type == "1" || result_type == "2")
                    //        result_type = "Expense";

                    //    return $("<li></li>")
                    //        .data("item.ui-autocomplete", item)
                    //        .append("<a><b> Code: </b> " + result_bomType + ", <b>Desc:</b> " + result_item + ", <span style='color:Gray;'>" + result_type + "</span></a>")
                    //        .appendTo(ul);
                    //}


                    var ula = ul;
                    var itema = item;
                    var result_value = item.Type;
                    var result_item = item.PhaseType;
                    var result_GroupName = item.GroupName;
                    var result_Code = item.Code;
                    var result_CodeDesc = item.Desc;
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

            $("[id*=txtGvLoc]").autocomplete({

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

                    var jobStr = ui.item.ID + ", " + ui.item.fDesc;
                    $(txtGvJob).val(jobStr);
                    $(this).val(ui.item.Tag);
                    $('#hdnIsAutoCompleteSelected').val('1');
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
                            .data("item.ui-autocomplete", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("item.ui-autocomplete", item)
                            .append("<a><b> Project: </b> " + result_value + ", " + result_item + ", <span style='color:Gray;'><b> Loc: </b>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
            });


            $("[id*=txtGvLoc]").change(function () {
                //debugger
                var isAutoCompleteSelected = $('#hdnIsAutoCompleteSelected').val();
                $('#hdnIsAutoCompleteSelected').val('0');
                if (isAutoCompleteSelected != '1') {
                    //debugger
                    //var txtGvJob = ;
                    var strItem = $(this).val();
                    var txtGvLoc = $(this).attr('id');
                    var txtGvJob = document.getElementById(txtGvLoc.replace('txtGvLoc', 'txtGvJob'));
                    var hdnJobID = document.getElementById(txtGvLoc.replace('txtGvLoc', 'hdnJobID'));
                    $(txtGvJob).val('');
                    $(hdnJobID).val('');
                    $(this).val('')
                }
                else {
                    
                }
            });










            $("[id*=txtGvDebit]").change(function () {
                BalanceProof();

                var txtGvDebit = $(this).attr('id');
                if (!isNaN(parseFloat($(this).val())) == 0) {
                    $(this).val('0.00');
                }

                var txtGvCredit = document.getElementById(txtGvDebit.replace('txtGvDebit', 'txtGvCredit'));
                $(txtGvCredit).val('0.00');

                var grid = $find("<%= RadGrid_Journal.ClientID %>");
                var rowCount = grid.get_masterTableView().get_dataItems().length;

                BalanceProof();
            });

            $("[id*=txtGvCredit]").change(function () {
                BalanceProof();

                var txtGvCredit = $(this).attr('id');
                if (!isNaN(parseFloat($(this).val())) == 0) {
                    $(this).val('0.00');
                }

                var txtGvDebit = document.getElementById(txtGvCredit.replace('txtGvCredit', 'txtGvDebit'));
                $(txtGvDebit).val('0.00');

                var grid = $find("<%= RadGrid_Journal.ClientID %>");
                var rowCount = grid.get_masterTableView().get_dataItems().length;

                BalanceProof();
            });  

            ///////////// Quick Codes //////////////
            $("#<%=txtDescription.ClientID%>").keyup(function (event) {
                debugger
                replaceQuickCodes(event, '<%=txtDescription.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });

        }

        $("[id*=txtDescription]").change(function () {
            var masterTable = $find("<%=RadGrid_Journal.ClientID%>").get_masterTableView();
            var isUpdate = false;

            if (masterTable.get_dataItems().length > 1) {
                isUpdate = true;
            }
            else if (masterTable.get_dataItems().length == 1) {
                var acctNo = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[0], "AcctNo").firstElementChild.value;
                if (acctNo != null && acctNo != "") {
                    isUpdate = true;
                }
            }

            if (isUpdate) {
                if (confirm("Do you want to update all the Memo fields?")) {
                    var _mainDesc = $("#<%=txtDescription.ClientID %>").val();

                    for (var row = 0; row < masterTable.get_dataItems().length; row++) {
                        masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[row], "fDesc").firstElementChild.value = _mainDesc;
                    }
                }
            }
        });

        function resetIndexF6() {
            var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
            $(hdnSelectPOIndex).val(-1);
        }

    </script>
    <script>
        $(document).ready(function () {
            $('form').attr('autocomplete', 'off');
        });
    </script>
</asp:Content>

