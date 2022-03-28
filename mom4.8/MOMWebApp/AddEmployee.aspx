<%@ Page Title="Employee || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddEmployee" Codebehind="AddEmployee.aspx.cs" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
   
    <%--Calendar CSS--%>
    <link href="Design/css/pikaday.css" rel="stylesheet" />

    <style>
        .spanStyle {
            color: grey;
            font-weight: normal;
            font-size: 14px;
            font-family: sans-serif;
            text-align: center;
            align-self: center;
        }

        .form-control1, .form-control_2.input-sm {
            border: 1px solid #e0e0e0;
            padding: 5px;
            color: #808080;
            background: #fff;
            width: 100%;
            font-size: small;
            font-weight: 300;
            font-family: sans-serif;
            border-radius: 2px !important;
            -webkit-appearance: none;
            outline: none;
            height: 28px;
            display: initial !important;
        }

        .form-control {
            display: initial !important;
        }

        .btn {
            border: 1px solid #0086b3;
            font-weight: 700;
            letter-spacing: 1px;
            outline: ridge;
            height: 28px;
            margin-top: -4px !important;
        }

            .btn:focus, .btn:active:focus, .btn.active:focus {
                outline: ridge;
            }

        .btn-primary {
            background: #0099cc !important;
            border-color: #208eb3 !important;
            /*color: #ffffff !important;*/
            /*border-right-color:#007399!important;
            border-right:thin!important;*/
            padding: 3px 10px !important;
            /*box-shadow: 0 3px 0 0 #4380b5;*/
        }

            .btn-primary:hover, .btn-primary:focus, .btn-primary:active, .btn-primary.active, .open > .dropdown-toggle.btn-primary {
                background: #33a6cc !important;
                height: 28px;
            }

            .btn-primary:active, .btn-primary.active {
                background: #007299 !important;
                box-shadow: none;
                height: 28px;
            }

        .istyle {
            padding-right: 0px;
            margin-right: -1px;
            margin-top: -4px !important;
            padding: 2px !important;
            margin-left: -1px !important;
        }

        .checkbox-custom, .radio-custom {
            opacity: 0;
            position: absolute;
        }

        .checkbox-custom, .checkbox-custom-label, .radio-custom, .radio-custom-label {
            display: inline-block;
            vertical-align: middle;
            margin: 5px;
            cursor: pointer;
        }

        .checkbox-custom-label, .radio-custom-label {
            position: relative;
        }

        .checkbox-custom + .checkbox-custom-label:before, .radio-custom + .radio-custom-label:before {
            content: '';
            background: #fff;
            border: 2px solid #3EAFE6;
            display: inline-block;
            vertical-align: middle;
            width: 20px;
            height: 20px;
            padding: 2px;
            margin-right: 10px;
            text-align: center;
        }

        .checkbox-custom:checked + .checkbox-custom-label:before {
            background: #3EAFE6;
        }

        .radio-custom + .radio-custom-label:before {
            border-radius: 50%;
        }

        .radio-custom:checked + .radio-custom-label:before {
            background: #3EAFE6;
        }


        .checkbox-custom:focus + .checkbox-custom-label, .radio-custom:focus + .radio-custom-label {
            outline: 1px solid #ddd;
            / focus style /;
        }

        .rd-flt {
            float: left;
            margin-right: 20px;
            margin-bottom: 5px;
        }

        .trost-label2 a {
            color: #1565C0 !important;
        }
        [id$='RadGrid_VendorTran'] .rgHeader > a {
            white-space: nowrap;
            padding-left: 0 !important;
        }
        .hdsPayRate, .hdsWage {
            border-right: 1px solid #bbb !important;
        }

        .hdsPayRate, .hdsBurdenRate {
            color: #2e6b89 !important;
            font-weight: bold !important;
        }
        .PayRate {
            background-color: #98FB98 !important;
        }

        .BurdenRate {
            background-color: #FFFFC2 !important;
        }
        /*[id$='RadGrid_VendorTran'] .rgRow > td {
            white-space: nowrap;
            padding-left: 30px !important;
        }
        [id$='RadGrid_VendorTran'] .rgAltRow > td {
            white-space: nowrap;
            padding-left: 30px !important;
        }*/
        
    </style>

    <script type="text/javascript">
    $(document).ready(function () {
            Materialize.updateTextFields();

            

        InitializeGrids('<%=gvWagePayRate.ClientID%>');
        InitializeGrids('<%=RadGridWageDeduction.ClientID%>');
        
            $("#ctl00_ContentPlaceHolder1_TabContainer2_tbWage_gvWagePayRate input:text").focus(function () { $(this).select(); }); // to update rates

            <%--if ($(window).width() > 767) {
                $('#<%=txtRemarks.ClientID%>').focus(function () {
                    $(this).animate({
                        //right: "+=0",
                        width: '520px',
                        height: '75px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtRemarks.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%',
                        height: '75px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });
            }--%>
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

            var rawData = $('#<%=gvWagePayRate.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);

            $('#<%=hdnWageRate.ClientID%>').val(formData);
        }
        function itemJSOND() {

            var rawData = $('#<%=RadGridWageDeduction.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);

            $('#<%=hdnWageDRate.ClientID%>').val(formData);
        }
        </script>




    <script type="text/javascript">
        function OnClientItemsRequesting(sender, eventArgs) {
            var context = eventArgs.get_context();
            context["FilterString"] = eventArgs.get_text();
        }
        function isNumberKey(evt, txt) {

            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        
        function CheckDelete() {
            var result = false;
            $("#<%=gvWagePayRate.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                itemJSON();
                return confirm('Do you really want to delete this Wage category ?');
            }
            else {
                alert('Please select an wage category to delete.')
                return false;
            }
        }
        function InitializeGrids(Gridview) {

            var rowone = $("#" + Gridview).find('tr').eq(1);
            $("input", rowone).each(function () {
                $(this).blur();
            });
        }
        function CheckCopyRate() {
            var result = 0;
            $("#<%=gvWagePayRate.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result++;
                }
            });

            if (result == 1) {
                itemJSON();
                return confirm('Do you really want to copy the selected rates to all?');
            } if (result > 1) {
                alert('Please select a single wage category to copy.');
                return false;
            }
            else {
                alert('Please select an wage category to copy.');
                return false;
            }
        }
        function CheckDeleteDed() {
            var result = false;
            $("#<%=RadGridWageDeduction.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                itemJSOND();
                return confirm('Do you really want to delete this Wage deduction category ?');
            }
            else {
                alert('Please select an wage deduction category to delete.')
                return false;
            }
        }
        function CheckCopyRateDed() {
            var result = 0;
            $("#<%=RadGridWageDeduction.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result++;
                }
            });

            if (result == 1) {
                itemJSOND();
                return confirm('Do you really want to copy the selected rates to all?');
            } if (result > 1) {
                alert('Please select a single wage deduction category to copy.');
                return false;
            }
            else {
                alert('Please select an wage deduction category to copy.');
                return false;
            }
        }
        <%--function RowContextMenu(sender, eventArgs) {
            var menu = $find("<%=RadGrid_WageCategory.ClientID %>");
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
        }--%>
</script>
    <script type="text/javascript">  
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace  
        function numericOnly(elementRef) {
            var keyCodeEntered = (event.which) ? event.which : (window.event.keyCode) ? window.event.keyCode : -1;
            if ((keyCodeEntered >= 48) && (keyCodeEntered <= 57)) {
                return true;
            }
            else if (keyCodeEntered == 46) {
                if ((elementRef.value) && (elementRef.value.indexOf('.') >= 0))
                    return false;
                else
                    return true;
            }
            return false;
        }
        
            function SetSSN() {
                var ssn = document.getElementById('txtSSN');
                var hdnssn = document.getElementById('txtSSNhide');
                hdnssn.value = ss.value;
                ssn.value = new Array(ssn.value.length - 3).join('x') + ssn.value.substr(ssn.value.length - 4, 4);

            }
        
            function ShowSSN() {
                
                var ssn = document.getElementById('txtSSN');
                var hdnssn = document.getElementById('txtSSNhide');
                ssn.value = hdnssn.value;

             }
       
</script> 

     <script type="text/javascript"> 

         $(document).ready(function () {
             var query = "";
             function dtaa() {
                 this.prefixText = null;
                 this.con = null;
                 this.custID = null;
             }

             

            

             $("#<%=txtPRTaxGL.ClientID%>").autocomplete({
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
                             alert("Due to unexpected errors we were unable to load accounts");
                         }
                     });
                 },
                 select: function (event, ui) {
                     $("#<%=txtPRTaxGL.ClientID%>").val(ui.item.label);
                        $("#<%=hdntxtPRTaxGL.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtPRTaxGL.ClientID%>").val(ui.item.label);
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
                 };

             <%--$("#<%=txtWageCatGLExpAcct.ClientID%>").autocomplete({
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
                             alert("Due to unexpected errors we were unable to load accounts");
                         }
                     });
                 },
                 select: function (event, ui) {
                     
                     $("#<%=txtWageCatGLExpAcct.ClientID%>").val(ui.item.label);
                     $("#<%=hdntxtWageCatGLExpAcct.ClientID%>").val(ui.item.value);
                     return false;
                 },
                 focus: function (event, ui) {
                     $("#<%=txtWageCatGLExpAcct.ClientID%>").val(ui.item.label);
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
                 };--%>

             $("#<%=txtWageCatGLExpAcctOtherIncome.ClientID%>").autocomplete({
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
                             alert("Due to unexpected errors we were unable to load accounts");
                         }
                     });
                 },
                 select: function (event, ui) {
                     $("#<%=txtWageCatGLExpAcctOtherIncome.ClientID%>").val(ui.item.label);
                     $("#<%=hdntxtWageCatGLExpAcctOtherIncome.ClientID%>").val(ui.item.value);
                     return false;
                 },
                 focus: function (event, ui) {
                     $("#<%=txtWageCatGLExpAcctOtherIncome.ClientID%>").val(ui.item.label);
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
                 };

             <%--$("#<%=txtEmployeeGL.ClientID%>").autocomplete({
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
                             alert("Due to unexpected errors we were unable to load accounts");
                         }
                     });
                 },
                 select: function (event, ui) {
                     $("#<%=txtEmployeeGL.ClientID%>").val(ui.item.label);
                     $("#<%=hdntxtEmployeeGL.ClientID%>").val(ui.item.value);
                     return false;
                 },
                 focus: function (event, ui) {
                     $("#<%=txtEmployeeGL.ClientID%>").val(ui.item.label);
                     return false;
                 },
                 minLength: 0,
                 delay: 250
             })
                 .bind('click', function () { $(this).autocomplete("search"); })
                 .data("ui-autocomplete")._renderItem = function (ul, item) {
                     
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
                         
                     }
                     else {
                         return $("<li></li>")
                             .data("item.autocomplete", item)
                             .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                             .appendTo(ul);
                     }
                 };

             $("#<%=txtCompanyGL.ClientID%>").autocomplete({
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
                             alert("Due to unexpected errors we were unable to load accounts");
                         }
                     });
                 },
                 select: function (event, ui) {
                     $("#<%=txtCompanyGL.ClientID%>").val(ui.item.label);
                     $("#<%=hdntxtCompanyGL.ClientID%>").val(ui.item.value);
                     return false;
                 },
                 focus: function (event, ui) {
                     $("#<%=txtCompanyGL.ClientID%>").val(ui.item.label);
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
                         
                     }
                     else {
                         return $("<li></li>")
                             .data("item.autocomplete", item)
                             .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                             .appendTo(ul);
                     }
                 };
             $("#<%=txtCompanyExpGL.ClientID%>").autocomplete({
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
                             alert("Due to unexpected errors we were unable to load accounts");
                         }
                     });
                 },
                 select: function (event, ui) {
                     $("#<%=txtCompanyExpGL.ClientID%>").val(ui.item.label);
                     $("#<%=hdntxtCompanyExpGL.ClientID%>").val(ui.item.value);
                     return false;
                 },
                 focus: function (event, ui) {
                     $("#<%=txtCompanyExpGL.ClientID%>").val(ui.item.label);
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
                         
                     }
                     else {
                         return $("<li></li>")
                             .data("item.autocomplete", item)
                             .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                             .appendTo(ul);
                     }
                 };--%>
            <%-- $("#<%=txtNTGL.ClientID%>").autocomplete({
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
                             alert("Due to unexpected errors we were unable to load accounts");
                         }
                     });
                 },
                 select: function (event, ui) {
                     $("#<%=txtNTGL.ClientID%>").val(ui.item.label);
                     $("#<%=hdnNTGL.ClientID%>").val(ui.item.value);
                     return false;
                 },
                 focus: function (event, ui) {
                     $("#<%=txtNTGL.ClientID%>").val(ui.item.label);
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
                 };
             $("#<%=txtDTGL.ClientID%>").autocomplete({
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
                             alert("Due to unexpected errors we were unable to load accounts");
                         }
                     });
                 },
                 select: function (event, ui) {
                     $("#<%=txtDTGL.ClientID%>").val(ui.item.label);
                     $("#<%=hdnDTGL.ClientID%>").val(ui.item.value);
                     return false;
                 },
                 focus: function (event, ui) {
                     $("#<%=txtDTGL.ClientID%>").val(ui.item.label);
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
                 };
             $("#<%=txtTTGL.ClientID%>").autocomplete({
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
                             alert("Due to unexpected errors we were unable to load accounts");
                         }
                     });
                 },
                 select: function (event, ui) {
                     $("#<%=txtTTGL.ClientID%>").val(ui.item.label);
                     $("#<%=hdnTTGL.ClientID%>").val(ui.item.value);
                     return false;
                 },
                 focus: function (event, ui) {
                     $("#<%=txtTTGL.ClientID%>").val(ui.item.label);
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
                 };--%>
             
             

         });

         $(function () {
             <%--$("[id*=checkColumnWageCategory]").change(function () {
                 try {
                     
                     var chk = $(this).attr('id');
                     
                     var hdnWageGL = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageGL'));
                     var hdnWageGLName = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageGLName'));
                     var hdnWageReg = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageReg'));
                     var hdnWageOT = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageOT'));
                     var hdnWageDT = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageDT'));
                     var hdnWageTT = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageTT'));
                     var hdnWageFIT = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageFIT'));
                     var hdnWageFICA = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageFICA'));

                     var hdnWageMEDI = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageMEDI'));
                     var hdnWageFUTA = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageFUTA'));
                     var hdnWageSIT = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageSIT'));
                     var hdnWageVac = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageVac'));
                     var hdnWageWC = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageWC'));
                     var hdnWageUni = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageUni'));

                     var hdnWageNT = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageNT'));
                     var hdnWageCReg = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageCReg'));
                     var hdnWageCOT = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageCOT'));
                     var hdnWageCDT = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageCDT'));
                     var hdnWageCNT = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageCNT'));
                     var hdnWageCTT = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageCTT'));
                     var hdnWageStatus = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageStatus'));
                     var hdnWageSick = document.getElementById(chk.replace('checkColumnWageCategory', 'hdnWageSick'));
                    
                         
                     $("#<%=hdntxtWageCatGLExpAcct.ClientID%>").val($(hdnWageGL).val());
                     $("#<%=txtWageCatGLExpAcct.ClientID%>").val($(hdnWageGLName).val());

                         $("#<%=txtRegularRate.ClientID%>").val($(hdnWageReg).val());
                         $("#<%=txtRegularWardanRate.ClientID%>").val($(hdnWageCReg).val());
                         $("#<%=txtOvertimeRate.ClientID%>").val($(hdnWageOT).val());
                         $("#<%=txtOvertimeWardanRate.ClientID%>").val($(hdnWageCOT).val());
                         $("#<%=txtDoubleTime.ClientID%>").val($(hdnWageDT).val());
                         $("#<%=txtDoubleWardanTime.ClientID%>").val($(hdnWageCDT).val());
                         $("#<%=txtTravelTime.ClientID%>").val($(hdnWageTT).val());
                         $("#<%=txtTravelWardanTime.ClientID%>").val($(hdnWageCTT).val());
                         $("#<%=txtTime.ClientID%>").val($(hdnWageNT).val());
                     $("#<%=txtWardanTime.ClientID%>").val($(hdnWageCNT).val());
                     $("#<%=ddlWageCategoryStatus.ClientID%>").val($(hdnWageStatus).val());
                     
                         if ($(hdnWageFIT).val() == "1") {
                             $("#<%=chkFIT.ClientID%>").attr('checked', true);                             
                         }
                         else {
                             $("#<%=chkFIT.ClientID%>").attr('checked', false);
                         }
                     if ($(hdnWageFICA).val() == "1") {
                         $("#<%=chkFICA.ClientID%>").attr('checked', true);
                         }
                         else {
                             $("#<%=chkFICA.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnWageMEDI).val() == "1") {
                         $("#<%=chkMEDI.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=chkMEDI.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnWageFUTA).val() == "1") {
                         $("#<%=chkFUTA.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=chkFUTA.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnWageSIT).val() == "1") {
                         $("#<%=chkSIT.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=chkSIT.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnWageVac).val() == "1") {
                         $("#<%=chkVacation.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=chkVacation.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnWageWC).val() == "1") {
                         $("#<%=chkWorkComp.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=chkWorkComp.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnWageUni).val() == "1") {
                         $("#<%=chkUnion.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=chkUnion.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnWageSick).val() == "1") {
                         $("#<%=chkSick.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=chkSick.ClientID%>").attr('checked', false);
                     }
                         Materialize.updateTextFields();
                    
                    
                 } catch (e) {
                     alert(e);
                 }

             });--%>
             <%--$("[id*=checkColumnWageDeduction]").change(function () {
                 try {

                     var chk = $(this).attr('id');
                     
                     var hdndeduBasedOn = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduBasedOn'));
                     var hdndeduAccruedOn = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduAccruedOn'));
                     var hdndeduByW = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduByW'));
                     var hdndeduEmpRate = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduEmpRate'));
                     var hdndeduEmpTop = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduEmpTop'));
                     var hdndeduEmpGL = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduEmpGL'));
                     var hdndeduEmpGLName = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduEmpGLName'));
                     var hdndeduCompRate = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduCompRate'));
                     var hdndeduCompTop = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduCompTop'));
                     var hdndeduCompGL = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduCompGL'));
                     var hdndeduCompGLName = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduCompGLName'));
                     var hdndeduCompGLE = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduCompGLE'));
                     var hdndeduCompGLEName = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduCompGLEName'));
                     var hdndeduInUse = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduInUse'));
                     var hdndeduYTD = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduYTD'));
                     var hdndeduYTDC = document.getElementById(chk.replace('checkColumnWageDeduction', 'hdndeduYTDC'));
                     

                     $("#<%=txtEmployeeRate.ClientID%>").val($(hdndeduEmpRate).val());
                     $("#<%=txtEmployeeCeiling.ClientID%>").val($(hdndeduEmpTop).val());
                     $("#<%=hdntxtEmployeeGL.ClientID%>").val($(hdndeduEmpGL).val());
                     $("#<%=txtEmployeeGL.ClientID%>").val($(hdndeduEmpGLName).val());
                     $("#<%=txtCompanyRate.ClientID%>").val($(hdndeduCompRate).val());
                     $("#<%=txtCompanyCeiling.ClientID%>").val($(hdndeduCompTop).val());
                     $("#<%=hdntxtCompanyGL.ClientID%>").val($(hdndeduCompGL).val());
                     $("#<%=txtCompanyGL.ClientID%>").val($(hdndeduCompGLName).val());
                     $("#<%=hdntxtCompanyExpGL.ClientID%>").val($(hdndeduCompGLE).val());
                     $("#<%=txtCompanyExpGL.ClientID%>").val($(hdndeduCompGLEName).val());
                     $("#<%=ddlPaidBy.ClientID%>").val($(hdndeduByW).val());
                     $("#<%=ddlBasedOn.ClientID%>").val($(hdndeduBasedOn).val());
                     $("#<%=ddlAccruedOn.ClientID%>").val($(hdndeduAccruedOn).val());
                     
                     Materialize.updateTextFields();


                 } catch (e) {
                     alert(e);
                 }

             });--%>
             $("[id*=checkColumnWageCategoryOtherIncome]").change(function () {
                 try {

                     var chk = $(this).attr('id');
                     // debugger;
                     /////////////////////
                     var hdnotherGL = document.getElementById(chk.replace('checkColumnWageCategoryOtherIncome', 'hdnotherGL'));
                     var hdnotherGLName = document.getElementById(chk.replace('checkColumnWageCategoryOtherIncome', 'hdnotherGLName'));
                     var hdnotherRate = document.getElementById(chk.replace('checkColumnWageCategoryOtherIncome', 'hdnotherRate'));
                     var hdnotherFIT = document.getElementById(chk.replace('checkColumnWageCategoryOtherIncome', 'hdnotherFIT'));
                     var hdnotherFICA = document.getElementById(chk.replace('checkColumnWageCategoryOtherIncome', 'hdnotherFICA'));
                     var hdnotherMEDI = document.getElementById(chk.replace('checkColumnWageCategoryOtherIncome', 'hdnotherMEDI'));
                     var hdnotherFUTA = document.getElementById(chk.replace('checkColumnWageCategoryOtherIncome', 'hdnotherFUTA'));

                     var hdnotherSIT = document.getElementById(chk.replace('checkColumnWageCategoryOtherIncome', 'hdnotherSIT'));
                     var hdnotherVac = document.getElementById(chk.replace('checkColumnWageCategoryOtherIncome', 'hdnotherVac'));
                     var hdnotherWc = document.getElementById(chk.replace('checkColumnWageCategoryOtherIncome', 'hdnotherWc'));
                     var hdnotherUni = document.getElementById(chk.replace('checkColumnWageCategoryOtherIncome', 'hdnotherUni'));
                     var hdnotherSick = document.getElementById(chk.replace('checkColumnWageCategoryOtherIncome', 'hdnotherSick'));
                     
                     //var lblDue = document.getElementById(chk.replace('checkColumnWageCategory', 'lblBalance'))
                     //////////////////////
                     $("#<%=txtWageCatRateOtherIncome.ClientID%>").val($(hdnotherRate).val());
                     $("#<%=txtWageCatGLExpAcctOtherIncome.ClientID%>").val($(hdnotherGLName).val());
                     $("#<%=hdntxtWageCatGLExpAcctOtherIncome.ClientID%>").val($(hdnotherGL).val());
                     
                     
                     if ($(hdnotherFIT).val() == "1") {
                         $("#<%=cbFit.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=cbFit.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnotherFICA).val() == "1") {
                         $("#<%=cbFica.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=cbFica.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnotherMEDI).val() == "1") {
                         $("#<%=cbMedi.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=cbMedi.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnotherFUTA).val() == "1") {
                         $("#<%=cbFuta.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=cbFuta.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnotherSIT).val() == "1") {
                         $("#<%=cbSit.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=cbSit.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnotherVac).val() == "1") {
                         $("#<%=cbVaction.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=cbVaction.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnotherWc).val() == "1") {
                         $("#<%=cbWc.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=cbWc.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnotherUni).val() == "1") {
                         $("#<%=cbUnion.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=cbUnion.ClientID%>").attr('checked', false);
                     }
                     if ($(hdnotherSick).val() == "1") {
                         $("#<%=cbSick.ClientID%>").attr('checked', true);
                     }
                     else {
                         $("#<%=cbSick.ClientID%>").attr('checked', false);
                     }

                     Materialize.updateTextFields();


                 } catch (e) {
                     alert(e);
                 }

             });
         });
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
         function uncheckWageCategoryOtherIncome(chk) {
             var status = chk.checked;

             var checkBoxes = $("input[id*='checkColumnWageCategoryOtherIncome']");

             $.each(checkBoxes, function () {
                 $(this).attr('checked', false);
             });

             chk.checked = status;
         }
         function uncheckWageCategory(chk) {
             var status = chk.checked;

             var checkBoxes = $("input[id*='checkColumnWageCategory']");

             $.each(checkBoxes, function () {
                 $(this).attr('checked', false);
             });

             chk.checked = status;
         }
         function uncheckWageDeduction(chk) {
             var status = chk.checked;

             var checkBoxes = $("input[id*='checkColumnWageDeduction']");

             $.each(checkBoxes, function () {
                 $(this).attr('checked', false);
             });

             chk.checked = status;
         }
         <%--function OpenWageCategoryModal() {
            
             var wnd = $find('<%=WageCategoryListWindow.ClientID %>');
             wnd.set_title("Wage Category");
             wnd.Show();
         }
         function CloseWageCategoryModal() {
             var wnd = $find('<%=WageCategoryListWindow.ClientID %>');
            wnd.Close();
        }
         function CheckUncheckAllCheckBoxAsNeeded() {
             var totalCheckboxes = $("#<%=RadGrid_WageCategoryList.ClientID%> input[id*='chkSelect']:checkbox").size();

             var checkedCheckboxes = $("#<%=RadGrid_WageCategoryList.ClientID%> input[id*='chkSelect']:checkbox:checked").size();

             if (totalCheckboxes == checkedCheckboxes) {

                 $("#<%=RadGrid_WageCategoryList.ClientID %> input[id*='chkAll']:checkbox").prop('checked', true);//.each(function () { this.checked = true; });
            }
            else {
                $("#<%=RadGrid_WageCategoryList.ClientID %> input[id*='chkAll']:checkbox").prop('checked', false);//.attr('checked', false);
             }

            
         }
         function BindClickEventForGridCheckBox() {
             $("#<%=RadGrid_WageCategoryList.ClientID%> input[id*='chkSelect']:checkbox").unbind('click').bind('click', function () {
                 CheckUncheckAllCheckBoxAsNeeded();
             });

             

             $("#<%=RadGrid_WageCategoryList.ClientID%> input[id*='chkAll']:checkbox").unbind('click').bind('click', function () {
                 if ($(this).is(':checked')) {
                     $("#<%=RadGrid_WageCategoryList.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', true);
                }
                else {
                    $("#<%=RadGrid_WageCategoryList.ClientID%> input[id*='chkSelect']:checkbox").prop('checked', false);
                }
                
            });

            
         }
         function OpenWageDeductionModal() {

             var wnd = $find('<%=WageDeductionListWindow.ClientID %>');
             wnd.set_title("Wage Category");
             wnd.Show();
         }
         function CloseWageDeductionModal() {
             var wnd = $find('<%=WageDeductionListWindow.ClientID %>');
             wnd.Close();
         }
         function CheckUncheckAllCheckBoxAsNeededDeduction() {
             var totalCheckboxes = $("#<%=RadGrid_WageDeductionList.ClientID%> input[id*='chkSelectD']:checkbox").size();

             var checkedCheckboxes = $("#<%=RadGrid_WageDeductionList.ClientID%> input[id*='chkSelectD']:checkbox:checked").size();

             if (totalCheckboxes == checkedCheckboxes) {

                 $("#<%=RadGrid_WageDeductionList.ClientID %> input[id*='chkAllD']:checkbox").prop('checked', true);//.each(function () { this.checked = true; });
             }
             else {
                 $("#<%=RadGrid_WageDeductionList.ClientID %> input[id*='chkAllD']:checkbox").prop('checked', false);//.attr('checked', false);
             }

             
         }
         function BindClickEventForGridCheckBoxDeduction() {
             $("#<%=RadGrid_WageDeductionList.ClientID%> input[id*='chkSelectD']:checkbox").unbind('click').bind('click', function () {
                 CheckUncheckAllCheckBoxAsNeededDeduction();
             });



             $("#<%=RadGrid_WageDeductionList.ClientID%> input[id*='chkAllD']:checkbox").unbind('click').bind('click', function () {
                 if ($(this).is(':checked')) {
                     $("#<%=RadGrid_WageDeductionList.ClientID%> input[id*='chkSelectD']:checkbox").prop('checked', true);
                 }
                 else {
                     $("#<%=RadGrid_WageDeductionList.ClientID%> input[id*='chkSelectD']:checkbox").prop('checked', false);
                 }

             });


         }--%>




         function myFunction() {
             var addeditcase = $('#<%=hdnAddEdit.ClientID%>').val(); 
             if (addeditcase != '0') {
                 var choice = window.confirm('Would you like to update all projects with these changes?');
                 if (choice == true) {
                     $('#<%=hdnFlage.ClientID%>').val('1'); 
                     return true;
                 }
                 else {
                     $('#<%=hdnFlage.ClientID%>').val('0'); 
                     return true;
                 }
             }
             else {
                 return true;
             }
             
         }
         
         function OpenWageDeductionWindowEditDoubleclick(ID) {

            <%-- $("#<%=RadGrid_WageDeduction.ClientID %>").find('tr:not(:first,:last)').each(function () {
                 var $tr = $(this);
                 $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                     ID = $tr.find('span[id*=lblId]').text();


                 });
             });--%>
             if (ID != "") {
                <%-- $('#<%=txtServiceType.ClientID%>').val(ID);
                $('#<%=hdnAddEdit.ClientID%>').val(ID);
                var wnd = $find('<%=WageDeductionWindow.ClientID %>');
                 wnd.set_title("Edit Wage Deduction");
                 Materialize.updateTextFields();
                 document.getElementById('<%=lnkEditWageDeduction.ClientID%>').click();--%>
             }
             else {
                 ChkWarning();
             }
         }
      

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <%-------$$$$$$$$$$$$$$$ RAD AJAX MANAGER  $$$$$$$$$$$$$$$-----%>

     <telerik:RadAjaxManager ID="RadAjaxManager_SerType" runat="server"  >
        <AjaxSettings>  
            <telerik:AjaxSetting AjaxControlID="btnAddWages">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="WageCategoryListWindow" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_WageCategory" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            </AjaxSettings>
        
        <AjaxSettings>  
            <telerik:AjaxSetting AjaxControlID="btndeductionAdd">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="WageDeductionListWindow" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            </AjaxSettings>
         <%--<AjaxSettings>
                        
                        <telerik:AjaxSetting AjaxControlID="RadGrid_WageCategoryList">
                            <UpdatedControls>   
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_WageCategoryList" LoadingPanelID="RadAjaxLoadingPanel_WageCategoryList" />
                                
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                       
                    </AjaxSettings>--%>
        </telerik:RadAjaxManager>
    <%--<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_WageCategoryList" runat="server">
                            </telerik:RadAjaxLoadingPanel>--%>
     
   
      <script type="text/javascript">
    function OnClientItemsRequesting(sender, eventArgs) {
        var context = eventArgs.get_context();
        context["FilterString"] = eventArgs.get_text();
    }
</script>
    <script type="text/javascript">
        

        function hideDropDown(selector, combo, e) {
            var tgt = e.relatedTarget;
            var parent = $telerik.$(selector)[0];
            var parents = $telerik.$(tgt).parents(selector);

            if (tgt != parent && parents.length == 0) {
                if (combo.get_dropDownVisible())
                    combo.hideDropDown();
            }
        }
</script>
     
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Vendor" runat="server">
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
                                        <i class="mdi-social-person-outline"></i>&nbsp;
                                         <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Employee</asp:Label>
                                        <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label>
                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton CssClass="icon-save" ID="btnSubmit" ValidationGroup="wage" runat="server" OnClientClick="itemJSON(); itemJSOND();" OnClick="btnSubmit_Click">Save</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                            OnClick="lnkClose_Click" PostBackUrl="~/EmployeeList.aspx?AddEmployee=Y"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                                    <li runat="server" id="liGeneral"><a href="#accrdGeneral" style="border-bottom-color: #00000024;"> General</a></li>
                                    <li runat="server" id="liControl" style="display: none"><a href="#accrdControl" style="border-bottom-color: #00000024;"> Field Worker Options</a></li>
                                    <li runat="server" id="liFinancial"><a href="#accrdFinancial"style="border-bottom-color: #00000024;"> Financial</a></li>
                                    <li runat="server" id="liWages"><a href="#accrdWages" style="border-bottom-color: #00000024;"> Wages</a></li>
                                    <li runat="server" id="liOtherIncome"><a href="#accrdOtherIncome" style="border-bottom-color: #00000024;"> Other Income</a></li>
                                    <li runat="server" id="liDeductions"><a href="#accrdDeductions" style="border-bottom-color: #00000024;"> Deductions</a></li>
                                    <li runat="server" id="liMiscCustom"><a href="#accrdMiscCustom" style="border-bottom-color: #00000024;"> Misc/Custom</a></li>
                                    <li runat="server" id="liDirectDeposit"><a href="#accrdDirectDeposit" style="border-bottom-color: #00000024;"> Direct Deposit</a></li>
                                    <li runat="server" id="liYTD"><a href="#accrdYTD" style="border-bottom-color: #00000024;"> YTD</a></li>
                                    <li runat="server" id="liBenchmark"><a href="#accrdBenchmark" style="border-bottom-color: #00000024;"> Benchmark/Remark</a></li>
                                    
                                    <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">

                                    <asp:Panel ID="pnlNext" runat="server">
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" OnClick="lnkFirst_Click" CausesValidation="False">
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
                            <div id="accrdGeneral" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>General</div>
                            <div class="collapsible-body">
                                                <div class="form-content-wrap">
                                                    <div class="form-content-pd">
                                                        <div class="form-section-row">
                                                            <div class="col s12">
                                                                <div class="form-section-row">
                                                                    <div class="section-ttle">User Details</div>
                                                                    <div class="form-section3">
                                                                        <div class="col s4">
                                                                            <asp:Image ID="ProfileImage" Style="border: none; background-color: none; border-radius: 50%; margin-left: auto; margin-right: auto; display: block;" runat="server" Height="100px" Width="100px" ImageUrl="images/User.png"></asp:Image>
                                                                        </div>
                                                                        <div class="col s8">
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <asp:TextBox ID="txtFName" TextMode="SingleLine" runat="server" CssClass="txtFName validate" MaxLength="15">
                                                                                    </asp:TextBox>
                                                                                    <label for="txtFName">First Name</label>

                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                                        ControlToValidate="txtFName" Display="None" ErrorMessage="First Name Required"
                                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                    <asp:ValidatorCalloutExtender
                                                                                        ID="RequiredFieldValidator1_ValidatorCalloutExtender" CssClass="valiateField" runat="server" Enabled="True" PopupPosition="TopLeft"
                                                                                        TargetControlID="RequiredFieldValidator1">
                                                                                    </asp:ValidatorCalloutExtender>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <label for="txtMName">Middle Name</label>

                                                                                    <asp:TextBox ID="txtMName" runat="server" MaxLength="5" CssClass="validate" ToolTip="Middle Name">
                                                                                    </asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <asp:TextBox ID="txtLName" runat="server" CssClass="txtLName validate" MaxLength="15"></asp:TextBox>
                                                                                    <label for="txtLName">Last Name</label>
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                                        ControlToValidate="txtLName" Display="None" ErrorMessage="Last Name Required"
                                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                    <asp:ValidatorCalloutExtender
                                                                                        ID="RequiredFieldValidator2_ValidatorCalloutExtender" CssClass="valiateField" runat="server" Enabled="True" PopupPosition="TopLeft"
                                                                                        TargetControlID="RequiredFieldValidator2">
                                                                                    </asp:ValidatorCalloutExtender>
                                                                                </div>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <%--<input type="text" class="validate" txt/>--%>
                                                                                <asp:TextBox ID="txtCustName" CssClass="validate" runat="server"></asp:TextBox>
                                                                                <label>Customer Name</label>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s12" style="display: none;">
                                                                            <div class="row">
                                                                                <%--<input type="text" class="validate" txt/>--%>
                                                                                <asp:TextBox ID="txtUserTitle" Text="" CssClass="validate" runat="server"></asp:TextBox>
                                                                                <label>User Title</label>
                                                                            </div>
                                                                        </div>


                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="txtAddress">Address</label>
                                                                                <textarea id="txtAddress" runat="server" class="txtAddress materialize-textarea validate" placeholder="" maxlength="100"></textarea>

                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_Address" runat="server"
                                                                                    ControlToValidate="txtAddress" Display="None" ErrorMessage="Address Required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="ValidatorCalloutExtender_Address" CssClass="valiateField" runat="server"
                                                                                    Enabled="True" TargetControlID="RequiredFieldValidator_Address" PopupPosition="TopLeft">
                                                                                </asp:ValidatorCalloutExtender>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtCity" runat="server" MaxLength="50" CssClass="txtCity validate"></asp:TextBox>
                                                                                <label for="txtCity">City</label>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_City" runat="server"
                                                                                    ControlToValidate="txtCity" Display="None" ErrorMessage="City Required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="ValidatorCalloutExtender_City" runat="server" CssClass="valiateField"
                                                                                    Enabled="True" TargetControlID="RequiredFieldValidator_City" PopupPosition="TopLeft">
                                                                                </asp:ValidatorCalloutExtender>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <%--       <div class="row">
                                                                                <asp:TextBox ID="txtState" MaxLength="2" runat="server" CssClass="txtState validate"></asp:TextBox>
                                                                                <label for="txtState">State</label>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_State" runat="server"
                                                                                    ControlToValidate="txtState" Display="None" ErrorMessage="State Required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="ValidatorCalloutExtender_State" runat="server" CssClass="valiateField"
                                                                                    Enabled="True" TargetControlID="RequiredFieldValidator_State"  PopupPosition="TopLeft">
                                                                                </asp:ValidatorCalloutExtender>
                                                                            </div>--%>

                                                                            <div class="row">
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_State" runat="server"
                                                                                    ControlToValidate="ddlState" Display="None" ErrorMessage="State Required"
                                                                                    SetFocusOnError="True">
                                                                                </asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="ValidatorCalloutExtender_State" CssClass="valiateField"
                                                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator_State" PopupPosition="TopLeft">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <label>State/Province</label>
                                                                                <asp:TextBox ID="ddlState" runat="server" CssClass="txtState validate"></asp:TextBox>
                                                                                <%--<asp:DropDownList ID="ddlState" runat="server" ToolTip="State" CssClass="browser-default validate">
                                                                                    <asp:ListItem Value="State">State</asp:ListItem>
                                                                                    <asp:ListItem Value="AL">Alabama</asp:ListItem>
                                                                                    <asp:ListItem Value="AK">Alaska</asp:ListItem>
                                                                                    <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                                                                                    <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                                                                                    <asp:ListItem Value="CA">California</asp:ListItem>
                                                                                    <asp:ListItem Value="CO">Colorado</asp:ListItem>
                                                                                    <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                                                                    <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                                                                                    <asp:ListItem Value="DE">Delaware</asp:ListItem>
                                                                                    <asp:ListItem Value="FL">Florida</asp:ListItem>
                                                                                    <asp:ListItem Value="GA">Georgia</asp:ListItem>
                                                                                    <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                                                                                    <asp:ListItem Value="ID">Idaho</asp:ListItem>
                                                                                    <asp:ListItem Value="IL">Illinois</asp:ListItem>
                                                                                    <asp:ListItem Value="IN">Indiana</asp:ListItem>
                                                                                    <asp:ListItem Value="IA">Iowa</asp:ListItem>
                                                                                    <asp:ListItem Value="KS">Kansas</asp:ListItem>
                                                                                    <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                                                                                    <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                                                                                    <asp:ListItem Value="ME">Maine</asp:ListItem>
                                                                                    <asp:ListItem Value="MD">Maryland</asp:ListItem>
                                                                                    <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                                                                                    <asp:ListItem Value="MI">Michigan</asp:ListItem>
                                                                                    <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                                                                                    <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                                                                                    <asp:ListItem Value="MO">Missouri</asp:ListItem>
                                                                                    <asp:ListItem Value="MT">Montana</asp:ListItem>
                                                                                    <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                                                                                    <asp:ListItem Value="NV">Nevada</asp:ListItem>
                                                                                    <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                                                                                    <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                                                                                    <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                                                                                    <asp:ListItem Value="NY">New York</asp:ListItem>
                                                                                    <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                                                                                    <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                                                                                    <asp:ListItem Value="OH">Ohio</asp:ListItem>
                                                                                    <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                                                                                    <asp:ListItem Value="OR">Oregon</asp:ListItem>
                                                                                    <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                                                                                    <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                                                                                    <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                                                                                    <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                                                                                    <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                                                                                    <asp:ListItem Value="TX">Texas</asp:ListItem>
                                                                                    <asp:ListItem Value="UT">Utah</asp:ListItem>
                                                                                    <asp:ListItem Value="VT">Vermont</asp:ListItem>
                                                                                    <asp:ListItem Value="VA">Virginia</asp:ListItem>
                                                                                    <asp:ListItem Value="WA">Washington</asp:ListItem>
                                                                                    <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                                                                                    <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                                                                                    <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                                                                                    <asp:ListItem Value="AB">Alberta</asp:ListItem>
                                                                                    <asp:ListItem Value="BC">British Columbia</asp:ListItem>
                                                                                    <asp:ListItem Value="MB">Manitoba</asp:ListItem>
                                                                                    <asp:ListItem Value="NB">New Brunswick</asp:ListItem>
                                                                                    <asp:ListItem Value="NL">Newfoundland and Labrador</asp:ListItem>
                                                                                    <asp:ListItem Value="NT">Northwest Territories</asp:ListItem>
                                                                                    <asp:ListItem Value="NS">Nova Scotia</asp:ListItem>
                                                                                    <asp:ListItem Value="NU">Nunavut</asp:ListItem>
                                                                                    <asp:ListItem Value="PE">Prince Edward Island</asp:ListItem>
                                                                                    <asp:ListItem Value="SK">Saskatchewan</asp:ListItem>
                                                                                    <asp:ListItem Value="ON">Ontario</asp:ListItem>
                                                                                    <asp:ListItem Value="QC">Quebec</asp:ListItem>
                                                                                    <asp:ListItem Value="YT">Yukon</asp:ListItem>
                                                                                </asp:DropDownList>--%>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtZip" MaxLength="10" runat="server"></asp:TextBox>
                                                                                <label for="txtZip">Zip</label>
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
                                                                                <asp:CompareValidator
                                                                                    ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlCountry"
                                                                                    ErrorMessage="Country Required." Operator="NotEqual"
                                                                                    ValueToCompare="Select"
                                                                                    Display="None" SetFocusOnError="true" />
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="ValidatorCalloutExtender3" CssClass="valiateField"
                                                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator3" PopupPosition="TopLeft">
                                                                                </asp:ValidatorCalloutExtender>
                                                                                <asp:DropDownList ID="ddlCountry" runat="server" ToolTip="Country" CssClass="ddlCountry validate browser-default">
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
                                                                                <asp:TextBox ID="txtLatitude" runat="server" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtLatitude">Latitude</label>
                                                                            </div>
                                                                        </div>


                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtLongitude" runat="server" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtLongitude">Longitude</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <div id="map" style="overflow: hidden !important; height: 110px!important;"></div>
                                                                            </div>
                                                                        </div>


                                                                    </div>
                                                                </div>
                                                                <div class="form-section-row">
                                                                    <div class="section-ttle">Settings</div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="txtUserName">Username</label>
                                                                                <%-- <input type="text" id="uname" autocomplete="off" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                // fix for mobile safari to show virtual keyboard
                                                                                this.blur();    this.focus();  }" />--%>
                                                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="txtUserName form-control validate" MaxLength="50" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                // fix for mobile safari to show virtual keyboard
                                                                                this.blur();    this.focus();  }"></asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtUserName_FilteredTextBoxExtender" runat="server"
                                                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                                                                    TargetControlID="txtUserName">
                                                                                </asp:FilteredTextBoxExtender>

                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtUserName"
                                                                                    Display="None" ErrorMessage="Username Required" SetFocusOnError="True">
                                                                                </asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="RequiredFieldValidator4_ValidatorCalloutExtender" PopupPosition="TopLeft" runat="server" Enabled="True" CssClass="valiateField" TargetControlID="RequiredFieldValidator4">
                                                                                </asp:ValidatorCalloutExtender>

                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <label for="txtPassword">Password</label>

                                                                                <%--         <input type="password" id="pass" autocomplete="off" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                // fix for mobile safari to show virtual keyboard
                                                                                this.blur();    this.focus();  }" />--%>
                                                                                <asp:TextBox ID="txtPassword" runat="server" CssClass="txtPassword form-control validate" MaxLength="10" AutoCompleteType="Disabled" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                // fix for mobile safari to show virtual keyboard
                                                                                this.blur();    this.focus();  }">

                                                                                </asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtPassword_FilteredTextBoxExtender" runat="server"
                                                                                    Enabled="False" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                                                                    TargetControlID="txtPassword">
                                                                                </asp:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPassword"
                                                                                    Display="None" ErrorMessage="Password Required" SetFocusOnError="True">
                                                                                </asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="RequiredFieldValidator5_ValidatorCalloutExtender" PopupPosition="TopLeft" CssClass="valiateField" runat="server" Enabled="True" TargetControlID="RequiredFieldValidator5">
                                                                                </asp:ValidatorCalloutExtender>

                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Status</label>
                                                                                <asp:DropDownList ID="rbStatus" runat="server" CssClass="browser-default">
                                                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
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
                                                                                <label class="drpdwn-label">User Type</label>
                                                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                                    <ContentTemplate>
                                                                                        <asp:DropDownList ID="ddlUserType" runat="server" CssClass="browser-default" >
                                                                                            <asp:ListItem Value="0">Office</asp:ListItem>
                                                                                            <asp:ListItem Value="1">Field</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <%--<input id="dateoT" type="text" class="datepicker_mom">--%>
                                                                                <asp:TextBox ID="txtStartDate" runat="server" autocomplete="off"
                                                                                    CssClass="datepicker_mom" onkeypress="return false;"></asp:TextBox>
                                                                                <label for="dateoT">Start Date</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <%--<input id="dateoH" type="text" class="datepicker_mom">--%>
                                                                                <asp:TextBox ID="txtEndDate" runat="server" autocomplete="off"
                                                                                    CssClass="datepicker_mom" onkeypress="return false;"></asp:TextBox>
                                                                                <label for="dateoH">End Date</label>
                                                                            </div>
                                                                        </div>


                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">

                                                                        <div class="input-field col s12">
                                                                            <div class="row" style="margin-top: 8px;margin-left: -26px;margin-right: -31px;">
                                                                                <%--Multiselect Dropdown with Checkboxes, Use Telerik--%>
                                                                                <label class="drpdwn-label">Department</label>
                                                                                <telerik:RadComboBox ID="ddlDepartment" runat="server" Enabled="true" CheckBoxes="true"
                                                                                    EmptyMessage="Select Department" CssClass="browser-default col s12"
                                                                                    Sort="Ascending">
                                                                                </telerik:RadComboBox>
                                                                            </div>
                                                                        </div>



                                                                        <div class="input-field col s12" id="dvCompanyPermission" runat="server" style="margin-top: 14px;">
                                                                            <div class="row">
                                                                                <%--Multiselect Dropdown with Checkboxes, Use Telerik--%>
                                                                                <label class="drpdwn-label">Company</label>
                                                                                <telerik:RadComboBox ID="lstSelectCompany" runat="server" Enabled="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                                                                    Localization-CheckAllString="Select all" EmptyMessage="Select Company" CssClass="browser-default col s12"
                                                                                    Sort="Ascending">
                                                                                </telerik:RadComboBox>
                                                                                <asp:Label ID="lblSelectedTech" runat="server" Visible="false"></asp:Label>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s5" style="margin-top: 4px;">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">User Role</label>
                                                                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">UserRoleChange
                                                                                    <ContentTemplate>--%>
                                                                                        <asp:DropDownList ID="ddlUserRole" runat="server" CssClass="browser-default"
                                                                                            onchange="javascript:return UserRoleChange(this);">
                                                                                        </asp:DropDownList>
                                                                                    <%--</ContentTemplate>
                                                                                </asp:UpdatePanel>--%>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5" id="divApplyUserRolePermission" runat="server" style="margin-top: 4px;">
                                                                            <div class="row">
                                                                                <label class="drpdwn-label">Applying User Role Permission</label>
                                                                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                                    <ContentTemplate>--%>
                                                                                        <asp:DropDownList ID="ddlApplyUserRolePermission" runat="server" CssClass="browser-default"
                                                                                            onchange="javascript:return ApplyUserRolePermissionChange(this);"
                                                                                            >
                                                                                            <asp:ListItem Value="0">None</asp:ListItem>
                                                                                            <asp:ListItem Value="1">Merge</asp:ListItem>
                                                                                            <asp:ListItem Value="2">Override</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    <%--</ContentTemplate>
                                                                                </asp:UpdatePanel>--%>
                                                                            </div>
                                                                        </div>
                                                                        <%--<div class="input-field col s12">
                                                                            <div class="row">
                                                                                <div class="form-section12">
                                                                                    <div class="input-field col s6">
                                                                                        <asp:CheckBox ID="chkProjectManager" runat="server" CssClass="filled-in" />
                                                                                        <asp:Label ID="lbProjectManager" runat="server" Text="Project Manager"></asp:Label>
                                                                                    </div>
                                                                                    <div class="input-field col s6">
                                                                                        <asp:CheckBox ID="chkAssignedProject" runat="server" CssClass="filled-in" />
                                                                                        <asp:Label ID="lbAssignProject" runat="server" Text="Assigned Projects"></asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>--%>
                                                                    </div>
                                                                </div>

                                                                <div class="form-section-row">
                                                                    <div class="section-ttle">Other Info</div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <%--    <input id="telephone" type="text" class="validate">--%>
                                                                                <asp:TextBox ID="txtTelephone" runat="server" CssClass="validate txtTelephone" MaxLength="22"
                                                                                    placeholder="(xxx)xxx-xxxx"></asp:TextBox>
                                                                                <label for="txtTelephone">Phone</label>

                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                                                                    ControlToValidate="txtTelephone" Display="None" ErrorMessage="Telephone Required"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                <asp:ValidatorCalloutExtender
                                                                                    ID="RequiredFieldValidator8_ValidatorCalloutExtender" runat="server"
                                                                                    Enabled="True" TargetControlID="RequiredFieldValidator8" PopupPosition="TopLeft" CssClass="valiateField">
                                                                                </asp:ValidatorCalloutExtender>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <%--<input id="cellular" type="text" class="">--%>
                                                                                <asp:TextBox ID="txtCell" runat="server" CssClass="validate" MaxLength="22"
                                                                                    placeholder="(xxx)xxx-xxxx"></asp:TextBox>
                                                                                <label for="txtCell">Cell</label>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <%--<input id="email" type="text" class="validate">--%>
                                                                                <%--<asp:TextBox ID="txtEmail" OnTextChanged="txtEmail_TextChanged" AutoPostBack="True" runat="server" CssClass="form-control" MaxLength="50"
                                                                                    TabIndex="10"></asp:TextBox>--%>
                                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                                    <ContentTemplate>
                                                                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="50"
                                                                                            TabIndex="10"></asp:TextBox>

                                                                                        <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender" runat="server"
                                                                                            Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtEmail">
                                                                                        </asp:FilteredTextBoxExtender>
                                                                                        <label for="email">Email</label>

                                                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                                                            ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                        </asp:RegularExpressionValidator>
                                                                                        <asp:ValidatorCalloutExtender
                                                                                            ID="RegularExpressionValidator1_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                            TargetControlID="RegularExpressionValidator1">
                                                                                        </asp:ValidatorCalloutExtender>
                                                                                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                                                            Display="None" ErrorMessage="Email Required" SetFocusOnError="True" Enabled="False">
                                                                                        </asp:RequiredFieldValidator>
                                                                                        <asp:ValidatorCalloutExtender ID="rfvEmail_ValidatorCalloutExtender"
                                                                                            runat="server" Enabled="True" TargetControlID="rfvEmail">
                                                                                        </asp:ValidatorCalloutExtender>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>



                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <%--<input id="textmessage" type="text" class="validate">--%>
                                                                                <asp:TextBox ID="txtMsg" runat="server" CssClass="validate"></asp:TextBox>
                                                                                <label for="txtMsg">Text Message</label>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s12">
                                                                            <div class="row">

                                                                                <%--<asp:Label ID="lblMultiLang" runat="server" Text="Language"></asp:Label>--%>
                                                                                <label id="lblMultiLang" class="drpdwn-label" runat="server">Language</label>
                                                                                <asp:DropDownList ID="ddlLang" runat="server" CssClass="browser-default">
                                                                                    <asp:ListItem Value="select">-- Select --</asp:ListItem>
                                                                                    <asp:ListItem Value="english">English</asp:ListItem>
                                                                                    <asp:ListItem Value="spanish">Spanish</asp:ListItem>
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
                                                                                <label for="EmName">Emergency Contact Name</label>
                                                                                <%--<input type="text" id="EmName" />--%>
                                                                                <asp:TextBox ID="txtEmName" CssClass="validate" runat="server"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <label for="EmNum">Emergency Number</label>
                                                                                <%--<input type="text" id="EmNum" />--%>
                                                                                <asp:TextBox ID="txtEmNum" CssClass="validate" runat="server"></asp:TextBox>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s6 mgntp10">
                                                                            <div class="checkrow">
                                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                                    <ContentTemplate>
                                                                                        <asp:CheckBox ID="chkEmailAcc" runat="server" 
                                                                                             CssClass="filled-in" />
                                                                                        <asp:Label runat="server" Text="Email Account"></asp:Label>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">


                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <%--<textarea id="remarks" class="materialize-textarea textarea-border smalltxtarea"></textarea>--%>
                                                                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"
                                                                                    MaxLength="8000" CssClass="txtRemarks formmaterialize-textarea textarea-border smalltxtarea pnlAccounts"></asp:TextBox>
                                                                                <label for="txtRemarks" class="txtbrdlbl">Remarks</label>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <asp:UpdatePanel runat="server" ID="updatepnlemail">
                                                                    <ContentTemplate>
                                                                        <asp:Panel ID="pnlEmailAccount" runat="server" Visible="False">

                                                                            <div class="form-section-row">
                                                                                <div class="form-section3half">
                                                                                    <div class="section-ttle">
                                                                                        Incoming Mail Settings
                                                                                    </div>
                                                                                    <div class="form-section-row">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtInServer">Incoming Mail Server (IMAP)</label>
                                                                                                <%--<input type="text" id="imap" />--%>
                                                                                                <asp:TextBox ID="txtInServer" runat="server"
                                                                                                    MaxLength="100"></asp:TextBox>

                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19"
                                                                                                    runat="server" ControlToValidate="txtInServer" Display="None" ErrorMessage="Required"
                                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender
                                                                                                    ID="RequiredFieldValidator19_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                    PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator19">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5" style="border-bottom: 1px solid #9e9e9e">
                                                                                            <div class="checkrow">
                                                                                                <%--input id="ssl" type="checkbox" class="filled-in">--%>
                                                                                                <asp:CheckBox ID="chkSSL" CssClass="filled-in" runat="server" />
                                                                                                <label for="chkSSL" class="margin-top-3px">SSL</label>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="portin">Port</label>
                                                                                                <%--<input type="text" id="portin" />--%>
                                                                                                <asp:TextBox ID="txtinPort" runat="server"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                                    FilterMode="ValidChars" FilterType="Numbers" TargetControlID="txtinPort">
                                                                                                </asp:FilteredTextBoxExtender>

                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtinPort"
                                                                                                    Display="None" ErrorMessage="Required" SetFocusOnError="True">
                                                                                                </asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                                    ID="RequiredFieldValidator20_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                    PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator20">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="unamein">
                                                                                                    Email Username
                                                                                                </label>
                                                                                                <%--            <input type="text" id="unamein" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                    // fix for mobile safari to show virtual keyboard
                                                                                    this.blur();    this.focus();  }" />--%>
                                                                                                <asp:TextBox ID="txtInUSername" runat="server" CssClass="validate" AutoCompleteType="Disabled" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                                // fix for mobile safari to show virtual keyboard
                                                                                                this.blur();    this.focus();  }"
                                                                                                    MaxLength="100"></asp:TextBox><%--onfocusout="CopyToBCC()"--%>
                                                                                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                                                                    ControlToValidate="txtInUSername" Display="None" ErrorMessage="Invalid Username"
                                                                                                    SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                                </asp:RegularExpressionValidator>
                                                                                                <asp:ValidatorCalloutExtender
                                                                                                    ID="RegularExpressionValidator2_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                    TargetControlID="RegularExpressionValidator2" PopupPosition="TopLeft">
                                                                                                </asp:ValidatorCalloutExtender>--%>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtInUSername"
                                                                                                    Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator21_ValidatorCalloutExtender"
                                                                                                    runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator21">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="pwdin">Email Password</label>
                                                                                                <%--   <input type="password" id="pwdin" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                    // fix for mobile safari to show virtual keyboard
                                                                                    this.blur();    this.focus();  }" />--%>
                                                                                                <asp:TextBox ID="txtInPassword" TextMode="Password" runat="server" AutoCompleteType="Disabled" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                // fix for mobile safari to show virtual keyboard
                                                                                this.blur();    this.focus();  }"
                                                                                                    MaxLength="50"></asp:TextBox>
                                                                                                <%--<telerik:RadTextBox RenderMode="Auto" runat="server" ID="txtInPassword"  Width="200px" TextMode="Password"></telerik:RadTextBox>--%>

                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server"
                                                                                                    ControlToValidate="txtInPassword" Display="None" ErrorMessage="Required" SetFocusOnError="True">
                                                                                                </asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender
                                                                                                    ID="RequiredFieldValidator22_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                    PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator22">
                                                                                                </asp:ValidatorCalloutExtender>

                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="pwdin">Bcc Email</label>
                                                                                                <asp:TextBox ID="txtBccEmail" runat="server" MaxLength="50" onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                                // fix for mobile safari to show virtual keyboard
                                                                                                this.blur();    this.focus();  }"></asp:TextBox>
                                                                                                <asp:RegularExpressionValidator
                                                                                                    ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtBccEmail"
                                                                                                    Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                                </asp:RegularExpressionValidator>
                                                                                                <asp:ValidatorCalloutExtender
                                                                                                    ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                                                    TargetControlID="RegularExpressionValidator4" PopupPosition="TopLeft">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12 mgntp17">
                                                                                            <div class="row">
                                                                                                <div class="btnlinks">
                                                                                                    <%--<asp:LinkButton ID="btnTestIncoming" runat="server"  OnClick="btnTestIncoming_Click" Text="Test Settings" />--%>
                                                                                                    <asp:LinkButton ID="btnTestIncoming" runat="server" Text="Test Settings"></asp:LinkButton>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>

                                                                                    </div>
                                                                                </div>
                                                                                <div class="form-section3half-blank">
                                                                                    &nbsp;
                                                                                </div>
                                                                                <div class="form-section3half">

                                                                                    <div class="section-ttle">
                                                                                        Outgoing Mail Settings
                                                 
                                                                                    </div>
                                                                                    <div class="form-section-row">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtOutServer">Outgoing Mail Server (SMTP)</label>
                                                                                                <%--<input type="text" id="smtp" />--%>
                                                                                                <asp:TextBox ID="txtOutServer" runat="server" MaxLength="100"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23"
                                                                                                    runat="server" ControlToValidate="txtOutServer" Display="None" ErrorMessage="Required"
                                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender
                                                                                                    ID="RequiredFieldValidator23_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                    PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator23">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5" style="border-bottom: 1px solid #9e9e9e">
                                                                                            <div class="checkrow">
                                                                                                <%--<input id="sslo" type="checkbox" class="filled-in">--%>
                                                                                                <asp:CheckBox ID="chkSame" runat="server" CssClass="filled-in" />
                                                                                                <label for="chkSame" class="margin-top-3px">Same as Incoming</label>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="porto">Port</label>
                                                                                                <%--<input type="text" id="porto" />--%>
                                                                                                <asp:TextBox ID="txtOutPort" runat="server"></asp:TextBox>
                                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                                    FilterMode="ValidChars" FilterType="Numbers" TargetControlID="txtOutPort">
                                                                                                </asp:FilteredTextBoxExtender>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtOutPort"
                                                                                                    Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender
                                                                                                    ID="RequiredFieldValidator24_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                    PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator24">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="unameo">Email Username</label>
                                                                                                <%--<input type="text" id="unameo" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                                // fix for mobile safari to show virtual keyboard
                                                                                                this.blur();    this.focus();  }" />--%>
                                                                                                <asp:TextBox ID="txtOutUsername" runat="server" MaxLength="100">
                                                                                                </asp:TextBox>
                                                                                                <%--onfocus="if (this.hasAttribute('readonly')) 
                                                                                                            { 
                                                                                                                this.removeAttribute('readonly');
                                                                                                                // fix for mobile safari to show virtual keyboard
                                                                                                                this.blur();
                                                                                                                this.focus(); 
                                                                                                            }"--%>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server"
                                                                                                    ControlToValidate="txtOutUsername" Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                                <%--<asp:RegularExpressionValidator
                                                                                                    ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtOutUsername"
                                                                                                    Display="None" ErrorMessage="Invalid Username" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                                                                </asp:RegularExpressionValidator>
                                                                                                <asp:ValidatorCalloutExtender
                                                                                                    ID="RegularExpressionValidator3_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                    TargetControlID="RegularExpressionValidator3" PopupPosition="TopLeft">
                                                                                                </asp:ValidatorCalloutExtender>--%>
                                                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator25_ValidatorCalloutExtender"
                                                                                                    runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator25">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="pwdin">Email Password</label>
                                                                                                <%--<input type="password" id="pwdo" readonly onfocus="if (this.hasAttribute('readonly')) { this.removeAttribute('readonly');
                                                                                                // fix for mobile safari to show virtual keyboard
                                                                                                this.blur();    this.focus();  }" />--%>
                                                                                                <asp:TextBox ID="txtOutPassword" TextMode="Password" runat="server" MaxLength="50">
                                                                                                </asp:TextBox>
                                                                                                <!--onfocus="if (this.hasAttribute('readonly')) 
                                                                                                                { 
                                                                                                                    this.removeAttribute('readonly');
                                                                                                                    // fix for mobile safari to show virtual keyboard
                                                                                                                    this.blur();
                                                                                                                    this.focus();  
                                                                                                                }"-->
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server"
                                                                                                    ControlToValidate="txtOutPassword" Display="None" ErrorMessage="Required" SetFocusOnError="True">
                                                                                                </asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender
                                                                                                    ID="RequiredFieldValidator26_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                    PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator26">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5" style="border-bottom: 1px solid #9e9e9e">
                                                                                            <div class="checkrow">
                                                                                                <%--<input id="sslo" type="checkbox" class="filled-in">--%>
                                                                                                <asp:CheckBox ID="chkTakeASentEmailCopy" runat="server" CssClass="filled-in" />
                                                                                                <label for="chkTakeASentEmailCopy" class="margin-top-3px">Send Copy to "Sent Items"</label>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12 mgntp17">
                                                                                            <div class="row">
                                                                                                <div class="btnlinks">
                                                                                                    <asp:LinkButton ID="btnTestOut" runat="server"  Text="Test Settings" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>

                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                <div class="cf"></div>
                                                            </div>
                                                            <div class="cf"></div>
                                                        </div>
                                                        <div class="cf"></div>
                                                    </div>
                                                </div>
                                            </div>


                            <%--<div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvtxtFirst" runat="server" ControlToValidate="txtFirst"
                                                                Display="None" ErrorMessage="First Name Required" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender166" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtFirst">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtFirst" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtFirst">First</label>
                                                                
                                                            </div>
                                                        </div>
                                                       
                                                      

                                                        <div class="input-field col s6">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvtxtMiddle" runat="server" ControlToValidate="txtMiddle"
                                                                Display="None" ErrorMessage="Middle Name Required" SetFocusOnError="True" ValidationGroup="wage" ></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtMiddle">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtMiddle" runat="server" MaxLength="200" />
                                                                <asp:HiddenField ID="hdntxtMiddle" runat="server" />
                                                                <asp:Label runat="server" ID="lbltxtMiddle" AssociatedControlID="txtMiddle">Middle</asp:Label>
                                                                    
                                                                
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvtxtLast" runat="server" ControlToValidate="txtLast"
                                                                Display="None" ErrorMessage="Last Name Required" SetFocusOnError="True" ValidationGroup="wage" ></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender25" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtLast">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtLast" runat="server" MaxLength="200" />
                                                                <asp:HiddenField ID="hdntxtLast" runat="server" />
                                                                <asp:Label runat="server" ID="lbltxtLast" AssociatedControlID="txtLast">Last</asp:Label>
                                                               
                                                            </div>
                                                        </div>




                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                               <asp:RequiredFieldValidator ID="rfvtxtContact" runat="server" ControlToValidate="txtContact"
                                                                Display="None" ErrorMessage="Contact Required" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="vcerfvtxtContact" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtContact">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtContact" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtContact">Contact</label>
                                                                
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s6">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCellular" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtCellular">Cellular</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                               &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvtxtContactno" runat="server" ControlToValidate="txtContactno"
                                                                Display="None" ErrorMessage="Contact no Required" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="vcetxtContactno" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtContactno">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtContactno" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtContactno">Contact #</label>
                                                            </div>
                                                        </div>

                                                       
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                               
                                                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtEmail">Email</label>
                                                                
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">

                                                                <asp:TextBox ID="txtTextMsgAddress" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtTextMsgAddress">Text Message Address</label>
                                                                
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                

                                                                <asp:RequiredFieldValidator ID="rfvtxtAddress" runat="server" ControlToValidate="txtAddress"
                                                                Display="None" ErrorMessage="Address Required" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtAddress">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" CssClass="materialize-textarea"></asp:TextBox>
                                                                <label for="txtAddress">Address</label>

                                                            </div>
                                                        </div>
                                                        <div class="input-field col s6">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvtxtCity" runat="server" ControlToValidate="txtCity"
                                                                Display="None" ErrorMessage="City Required" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtCity">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtCity" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtCity">City</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvtxtCountry" runat="server" ControlToValidate="txtCountry"
                                                                Display="None" ErrorMessage="Country Required" SetFocusOnError="True" ValidationGroup="wage"></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtCountry">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtCountry" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtCountry">Country</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s6">
                                                            <div class="row">
                                                                <label class="drpdwn-label">State</label>
                                                                <asp:DropDownList ID="ddlState" runat="server" CssClass="browser-default">                                                                    
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                               &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtZip" runat="server" MaxLength="20"></asp:TextBox>
                                                                <label for="txtZip">Zip</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                               
                                                                <div class="input-field col s6">
                                                            <div class="row">
                                                                <asp:RequiredFieldValidator ID="rfvtxtPhone" runat="server" ControlToValidate="txtPhone"
                                                                Display="None" ErrorMessage="Phone Required" SetFocusOnError="True" ValidationGroup="wage" ></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtPhone">
                                                                </asp:ValidatorCalloutExtender>

                                                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtPhone">Phone</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                               &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtFax" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtFax">Fax</label>
                                                            </div>
                                                        </div>
                                                               
                                                                
                                                            </div>
                                                        </div>
                                                        
                                                        

                                                        
                                                        <div class="input-field col s5">
                                                                    <div class="row">
                                                                        <label class="drpdwn-label">Status</label>
                                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default">
                                                                            <asp:ListItem Value="0">Active</asp:ListItem>
                                                                            <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                                            <asp:ListItem Value="2">Hold</asp:ListItem>
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
                                                                        <label class="drpdwn-label">Field</label>
                                                                        <asp:DropDownList ID="ddlField" runat="server" CssClass="browser-default">
                                                                            <asp:ListItem Value="0">Yes</asp:ListItem>
                                                                            <asp:ListItem Value="1">No</asp:ListItem>
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
                                                                <asp:TextBox ID="lat" runat="server" MaxLength="50" ></asp:TextBox>
                                                                <label for="lat">Latitude</label>                                                       
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <asp:TextBox ID="lng" runat="server" MaxLength="50" ></asp:TextBox>
                                                                <label for="lng">Longitude</label>
                                                               
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                               <div id="map" style="overflow: hidden !important; height: 230px!important;">
                                                            </div>
                                                            </div>
                                                        </div>

                                                        
                                                        
                                                        
                                                       
                                                                                      
                                                       


                                                    </div>
                                                    
                                                    


                                                    <div class="form-section-row">
                                                                    <div class="section-ttle">Settings</div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtEmployeeID" runat="server" MaxLength="50"></asp:TextBox>
                                                                                <label for="txtEmployeeID">Employee ID</label>
                                                                                <asp:RequiredFieldValidator ID="rfvtxtEmployeeID" runat="server" ControlToValidate="txtEmployeeID"
                                                                                Display="None" ErrorMessage="Employee ID Required" SetFocusOnError="True" ValidationGroup="wage" ></asp:RequiredFieldValidator>
        
                                                                                <asp:ValidatorCalloutExtender ID="vcetxtEmployeeID" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                                TargetControlID="rfvtxtEmployeeID">
                                                                                </asp:ValidatorCalloutExtender>

                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s6">
                                                            <div class="row">
                                                                

                                                                <asp:MaskedEditExtender ID="msee" runat="server" TargetControlID="txtSSN" Mask="999-999-999"></asp:MaskedEditExtender>
                                                                
                                                                <asp:TextBox ID="txtSSN" runat="server" MaxLength="50" ></asp:TextBox>
                                                                <label for="txtSSN">SSN</label>
                                                                <asp:HiddenField ID="txtSSNhide" runat="server" />
                                                                    
                                                                
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s1">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Filing State</label>
                                                                
                                                                    <telerik:RadComboBox ID="ddlFilingState" BackColor="White" CssClass="browser-default" style="background-color:White;display: block;background-color: transparent;width: 100%;padding: 0;border: none;margin-top: 11px;font-size: 0.9em;" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" MaxHeight="150" >
                                                                    </telerik:RadComboBox> 
                                                               
                                                            </div>
                                                        </div>


                                                                        <div class="input-field col s12">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtCallSign" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtCallSign">Call Sign</label>
                                                                <asp:RequiredFieldValidator ID="rfvtxtCallSign" runat="server" ControlToValidate="txtCallSign"
                                                                Display="None" ErrorMessage="Call Sign Required" SetFocusOnError="True" ValidationGroup="wage" ></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="vcetxtCallSign" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtCallSign">
                                                                </asp:ValidatorCalloutExtender>

                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtTitle" runat="server" ></asp:TextBox>
                                                                <label for="txtTitle">Title</label>
                                                                <asp:RequiredFieldValidator ID="rfvtxtTitle" runat="server" ControlToValidate="txtTitle"
                                                                Display="None" ErrorMessage="Title Required" SetFocusOnError="True" ValidationGroup="wage" InitialValue="0" ></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="vcetxtTitle" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtTitle">
                                                                </asp:ValidatorCalloutExtender>
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                <asp:TextBox ID="txtSupervisor" runat="server" ></asp:TextBox>
                                                                                <label for="txtSupervisor">Supervisor</label>
                                                                            </div>
                                                                        </div>

                                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <div class="input-field col s5">
                                                                    <div class="row">
                                                                        <label class="drpdwn-label">Gender</label>
                                                                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="browser-default">
                                                                            <asp:ListItem Value="0">Male</asp:ListItem>
                                                                            <asp:ListItem Value="1">Female</asp:ListItem>
                                                                            <asp:ListItem Value="2">N/A</asp:ListItem>
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
                                                                        <label class="drpdwn-label">National Origin</label>
                                                                        <asp:DropDownList ID="ddlNationalOrigin" runat="server" CssClass="browser-default">
                                                                            <asp:ListItem Value="0">White</asp:ListItem>
                                                                            <asp:ListItem Value="1">Black (not of hisponic)</asp:ListItem>
                                                                            <asp:ListItem Value="2">Black</asp:ListItem>
                                                                            <asp:ListItem Value="3">Hisponic</asp:ListItem>
                                                                            <asp:ListItem Value="4">Asian or Pecific Island</asp:ListItem>
                                                                            <asp:ListItem Value="5">American Indian</asp:ListItem>
                                                                            <asp:ListItem Value="6">N/A</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>



                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                
                                                                                <asp:TextBox ID="txtHired" runat="server" class="datepicker_mom"
                                                                        MaxLength="15"></asp:TextBox>
                                                                        <label for="txtHired">Hired</label>
                                                                        <asp:RequiredFieldValidator ID="rfvHiredDate" 
                                                                        runat="server" ControlToValidate="txtHired" Display="None" ErrorMessage="Hire date is Required"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vcePostDate" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvHiredDate" />
                                                                    <asp:RegularExpressionValidator ID="revHiredDate" ControlToValidate="txtHired" 
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="vcePostDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="revHiredDate" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s2">
                                                                            <div class="row">
                                                                                &nbsp;
                                                                            </div>
                                                                        </div>
                                                                        <div class="input-field col s5">
                                                                            <div class="row">
                                                                                
                                                                                <asp:TextBox ID="txtTerminated" runat="server" class="datepicker_mom"
                                                                        MaxLength="15"></asp:TextBox>
                                                                        <label for="txtTerminated">Terminated</label>
                                                                        <asp:RegularExpressionValidator ID="revtxtTerminated" ControlToValidate="txtTerminated" 
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender10" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="revtxtTerminated" />
                                                                            </div>
                                                                        </div>


                                                                    </div>
                                                                    <div class="form-section3-blank">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div class="form-section3">
                                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <div class="input-field col s5">
                                                                    <div class="row">
                                                                        <asp:TextBox ID="txtBirthday" runat="server" class="datepicker_mom"
                                                                        MaxLength="15"></asp:TextBox>
                                                                        <label for="txtBirthday">Birthday</label>
                                                                        <asp:RegularExpressionValidator ID="revtxtBirthday" ControlToValidate="txtBirthday" 
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender11" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="revtxtBirthday" />
                                                                         <asp:RequiredFieldValidator ID="rfvtxtBirthday" runat="server" ControlToValidate="txtBirthday"
                                                                Display="None" ErrorMessage="Birthday Required" SetFocusOnError="True" ValidationGroup="wage" ></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="vcetxtBirthday" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtBirthday">
                                                                </asp:ValidatorCalloutExtender>
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s2">
                                                                    <div class="row">
                                                                        &nbsp;
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s5">
                                                                    <div class="row">
                                                                        <asp:TextBox ID="txtReview" runat="server" class="datepicker_mom"
                                                                        MaxLength="15"></asp:TextBox>
                                                                        <label for="txtReview">Seniority/Review</label>
                                                                        <asp:RegularExpressionValidator ID="revtxtReview" ControlToValidate="txtReview" 
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender12" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="revtxtReview" />
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s5">
                                                                    <div class="row">
                                                                        <asp:TextBox ID="txtRehire" runat="server" class="datepicker_mom"
                                                                        MaxLength="15"></asp:TextBox>
                                                                        <label for="txtRehire">Rehire/Anniversary</label>
                                                                        <asp:RequiredFieldValidator ID="rfvReHiredDate" 
                                                                        runat="server" ControlToValidate="txtRehire" Display="None" ErrorMessage="Rehire date is Required"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvReHiredDate" />
                                                                    <asp:RegularExpressionValidator ID="revReHiredDate" ControlToValidate="txtRehire" 
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="revReHiredDate" />
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s2">
                                                                    <div class="row">
                                                                        &nbsp;
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s5">
                                                                    <div class="row">
                                                                        <asp:TextBox ID="txtINSExp" runat="server" class="datepicker_mom"
                                                                        MaxLength="15"></asp:TextBox>
                                                                        <label for="txtINSExp">INS Exp</label>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtINSExp" 
                                                                        runat="server" ControlToValidate="txtINSExp" Display="None" ErrorMessage="INS Exp is Required"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender8" runat="server" Enabled="True"
                                                                        PopupPosition="Right" TargetControlID="rfvtxtINSExp" />
                                                                    <asp:RegularExpressionValidator ID="revrfvtxtINSExp" ControlToValidate="txtINSExp" 
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format MM/dd/yyyy" Display="None">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender9" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="revrfvtxtINSExp" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <div class="input-field col s5" style="padding-left: 0px;">
                                                                    <div class="checkrow">
                                                                        <asp:CheckBox ID="chkSales" runat="server" CssClass="css-checkbox" Text="Sales" />
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s2">
                                                                    <div class="row">
                                                                        &nbsp;
                                                                    </div>
                                                                </div>
                                                                <div class="input-field col s5" style="padding-left: 0px;">
                                                                    <div class="checkrow">
                                                                        <asp:CheckBox ID="chkDBoard" runat="server" CssClass="css-checkbox" Text="DBoard" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>                                                                       
                                                                       
                                                                    </div>
                                                                </div>


                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>--%>
                        </li>

                        <li>
                            <div id="accrdControl" class="collapsible-header accrd active accordian-text-custom " ><i class="mdi-social-poll"></i>Field Worker Options</div>
                            <div class="collapsible-body">
                                                <div class="form-content-wrap form-content-wrapwd">
                                                    <div class="form-content-pd">
                                                        <div class="form-section-row">
                                                            <div class="section-ttle">Options</div>
                                                            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                                <%--  <telerik:RadAjaxPanel  ID="rapWorker"  runat="server">--%>
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnlWorker" runat="server" Enabled="False">

                                                                        <div class="form-section3">
                                                                            <div class="input-field col s4">
                                                                                <div class="checkrow">
                                                                                    <%--<input id="scdlboard" type="checkbox" class="filled-in">--%>
                                                                                    <asp:CheckBox ID="chkScheduleBrd" runat="server" CssClass="filled-in" />
                                                                                    <label for="lblSchbrd">Schedule Board</label>
                                                                                </div>
                                                                            </div>

                                                                            <div class="input-field col s5 ">
                                                                                <div class="checkrow">
                                                                                    <%--<input id="athDevice" type="checkbox" class="filled-in">--%>
                                                                                    <asp:CheckBox ID="chkMSAuthorisedDeviceOnly" runat="server" OnCheckedChanged="chkMSAuthorisedDeviceOnly_CheckedChanged" AutoPostBack="true" CssClass="chkMSAuthorisedDeviceOnly filled-in" />
                                                                                    <label for="chkMSAuthorisedDeviceOnly">Authorized Device</label>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s3 ">
                                                                                <div class="checkrow">
                                                                                    <%--<input id="maps" type="checkbox" class="filled-in">--%>
                                                                                    <asp:CheckBox ID="chkMap" CssClass="filled-in" runat="server" AutoPostBack="True"
                                                                                        OnCheckedChanged="chkMap_CheckedChanged" />
                                                                                    <%--<asp:Label ID="lblMap" runat="server" Text="Maps"></asp:Label>--%>
                                                                                    <label id="lblMap" runat="server">Maps</label>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s11" style="margin-top: 5px;">
                                                                                <div class="row">
                                                                                    <label class="drpdwn-label">Merchant ID</label>
                                                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <asp:DropDownList ID="ddlMerchantID" runat="server" onchange="AddNewMerchant(this);"
                                                                                                CssClass="browser-default" Style="float: left;" TabIndex="12">
                                                                                            </asp:DropDownList>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s1" style="margin-top: 5px;">
                                                                                <div class="row">
                                                                                    <div class="btnlinksicon" style="margin-left: -5px; padding-top: 12px;">
                                                                                        <%--<a href="#"><i class="mdi-editor-border-color"></i></a>--%>
                                                                                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                                                            <ContentTemplate>
                                                                                                <asp:LinkButton ID="imgbtnMerchant" runat="server" CausesValidation="False"
                                                                                                    OnClick="imgbtnMerchant_Click" ToolTip="Edit" Height="30px">
                                                                                        <i class="mdi-editor-border-color"></i>
                                                                                                </asp:LinkButton>
                                                                                            </ContentTemplate>
                                                                                        </asp:UpdatePanel>
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
                                                                                    <%--<input id="devID" type="text">--%>
                                                                                    <%--<asp:Label ID="lblDeviceID" runat="server" Text="Device ID"></asp:Label>--%>
                                                                                    <label id="lblDeviceID" runat="server">Device ID</label>
                                                                                    <asp:TextBox ID="txtDeviceID" CssClass="txtDeviceID" runat="server" MaxLength="50"></asp:TextBox>
                                                                                    <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtDeviceID"
                                                                                            Display="None" Enabled="False" ErrorMessage="Device ID Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator11_ValidatorCalloutExtender"
                                                                                            runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator11">
                                                                                        </asp:ValidatorCalloutExtender>--%>
                                                                                </div>
                                                                            </div>

                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <%--<input id="authdevID" type="text">--%>
                                                                                    <asp:TextBox ID="txtAuthdevID" CssClass="txtAuthdevID" runat="server"></asp:TextBox>
                                                                                    <label for="authdevID">Authorized Device ID</label>
                                                                                    <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                                                                        ControlToValidate="txtAuthdevID"  Enabled="false" Display="None" ErrorMessage="Authorized Device ID Required"
                                                                                        SetFocusOnError="True">

                                                                                    </asp:RequiredFieldValidator>
                                                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                                                                        runat="server" CssClass="valiateField" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator12">
                                                                                    </asp:ValidatorCalloutExtender>--%>
                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                        <div class="form-section3-blank">
                                                                            &nbsp;
                                                                        </div>
                                                                        <div class="form-section3">
                                                                            <div class="input-field col s12">
                                                                                <div class="row">
                                                                                    <asp:Label ID="lblSuper" runat="server" CssClass="drpdwn-label" Text="Supervisor"></asp:Label>
                                                                                    <%--<label id="lblSuper"  runat="server" class="drpdwn-label">Supervisor</label>--%>
                                                                                    <asp:DropDownList ID="ddlSuper" runat="server" AutoPostBack="True" CssClass="ddlSuper browser-default"
                                                                                        OnSelectedIndexChanged="ddlSuper_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s6">
                                                                                <div class="checkrow">
                                                                                    <asp:CheckBox ID="chkSuper" runat="server" AutoPostBack="True" OnCheckedChanged="chkSuper_CheckedChanged"
                                                                                        CssClass="filled-in" />
                                                                                    <asp:Label ID="lblIsSuper" runat="server" Text="Is Supervisor"></asp:Label>
                                                                                </div>
                                                                            </div>
                                                                            <div class="input-field col s6">
                                                                                <div class="checkrow">
                                                                                    <asp:CheckBox ID="chkDefaultWorker" runat="server" CssClass="filled-in" />
                                                                                    <asp:Label ID="lblDefwork" runat="server" Text="Default Worker"></asp:Label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                                <%--   </telerik:RadAjaxPanel>--%>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                        <div class="form-section-row">
                                                            <div class="section-ttle">
                                                                Workers
                                                            </div>
                                                            <%--<telerik:RadAjaxLoadingPanel runat="server">--%>
                                                            <div class="btnlinks">
                                                                <%--<asp:LinkButton ID="btnEdit" runat="server"  CssClass="btnEdit" OnClick="btnEdit_Click" CausesValidation="False" ToolTip="Edit">Edit</asp:LinkButton>--%>
                                                                <telerik:RadButton ID="btnEdit" runat="server" CssClass="btnEdit" OnClick="btnEdit_Click" CausesValidation="False" ToolTip="Edit" Text="Edit"></telerik:RadButton>
                                                            </div>
                                                            <div class="btnlinks">
                                                                <asp:LinkButton ID="lnkDone" runat="server" CssClass="lnkDone" OnClick="lnkDone_Click" CausesValidation="False">Done</asp:LinkButton>
                                                            </div>
                                                            <%-- </telerik:RadAjaxLoadingPanel>--%>
                                                            <div class="RadGrid RadGrid_Material">
                                                                <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                                                    <script type="text/javascript">
                                                                        function pageLoad() {
                                                                            var grid = $find("<%= gvUsers.ClientID %>");
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
                                                                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_gvUsers" runat="server">
                                                                </telerik:RadAjaxLoadingPanel>
                                                                <telerik:RadAjaxPanel ID="UpdatePanel8" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvUsers" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                    <asp:HiddenField ID="hfCheckEdit" runat="server" Value="false" />
                                                                    <div class="grid_container mgntp10" id="pnlGrid" runat="server" visible="false">
                                                                        <telerik:RadGrid ID="gvUsers" RenderMode="Auto" runat="server" AutoGenerateColumns="False" CssClass=" gvUsers table table-bordered table-striped table-condensed flip-content"
                                                                            AllowSorting="True" AllowPaging="true" OnSorting="gvUsers_Sorting"
                                                                            PageSize="20"
                                                                            ShowFooter="True"
                                                                            FilterType="CheckList"
                                                                            OnNeedDataSource="gvUsers_NeedDataSource"
                                                                            OnItemDataBound="gvUsers_ItemDataBound"
                                                                            OnPageIndexChanged="gvUsers_PageIndexChanged"
                                                                            OnPageSizeChanged="gvUsers_PageSizeChanged">
                                                                            <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ActiveItemStyle CssClass="evenrowcolor" />
                                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="UserID">
                                                                                <Columns>
                                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnSelected" runat="server" />
                                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="ID" Visible="False" UniqueName="UserID" DataField="UserID" AllowFiltering="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("UserID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="TypeID" Visible="false" UniqueName="usertypeid" DataField="usertypeid">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTypeid" runat="server" Text='<%# Bind("usertypeid") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Username" SortExpression="fuser" UniqueName="fuser" DataField="fuser" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("fuser") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="First Name" SortExpression="ffirst" UniqueName="ffirst" DataField="ffirst" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="Label3" runat="server"><%#Eval("ffirst")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Last Name" SortExpression="last" UniqueName="last" DataField="last" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblLN" runat="server"><%#Eval("last")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Type" SortExpression="usertype" UniqueName="usertype" DataField="usertype" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblActive" runat="server"><%#Eval("usertype")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Status" SortExpression="status" UniqueName="status" DataField="status" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Supervisor" SortExpression="super" UniqueName="super" DataField="super" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblsuper" runat="server"><%#Eval("super")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                            <SelectedItemStyle CssClass="selectedrowcolor" />
                                                                            <AlternatingItemStyle CssClass="oddrowcolor" />
                                                                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                            </FilterMenu>
                                                                        </telerik:RadGrid>

                                                                        <%--  <telerik:RadGrid RenderMode="Auto" ID="gvUsers" AutoGenerateColumns="False" CssClass="gvUsers" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                                                            ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" 
                                                                            OnNeedDataSource="gvUsers_NeedDataSource" 
                                                                            OnItemDataBound="gvUsers_ItemDataBound"
                                                                             OnSorting="gvUsers_Sorting" 
                                                                            >
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                <Selecting AllowRowSelect="True"></Selecting>

                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="UserID">
                                                                                <Columns>
                                                                                    <telerik:GridTemplateColumn Visible="false" AllowFiltering="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnSelected" runat="server" />
                                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="ID" Visible="False" UniqueName="UserID" DataField="UserID" AllowFiltering="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("UserID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="TypeID" Visible="false" UniqueName="usertypeid" DataField="usertypeid">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTypeid" runat="server" Text='<%# Bind("usertypeid") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Username" SortExpression="fuser" UniqueName="fuser" DataField="fuser" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("fuser") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="First Name" SortExpression="ffirst" UniqueName="ffirst" DataField="ffirst" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="Label3" runat="server"><%#Eval("ffirst")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Last Name" SortExpression="last" UniqueName="last" DataField="last" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblLN" runat="server"><%#Eval("last")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Type" SortExpression="usertype" UniqueName="usertype" DataField="usertype" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblActive" runat="server"><%#Eval("usertype")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Status" SortExpression="status" UniqueName="status" DataField="status" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="Supervisor" SortExpression="super" UniqueName="super" DataField="super" ShowFilterIcon="false" FilterDelay="5" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblsuper" runat="server"><%#Eval("super")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                </Columns>
                                                                            </MasterTableView>
                                                                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                            </FilterMenu>
                                                                        </telerik:RadGrid>--%>
                                                                    </div>
                                                                </telerik:RadAjaxPanel>
                                                            </div>
                                                            <div class="form-section-row">
                                                                <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                                                                    CausesValidation="False" />
                                                                <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                                                                    TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="pnlMerchant"
                                                                    BackgroundCssClass="ModalPopupBG" DynamicServicePath="" Enabled="True">
                                                                </asp:ModalPopupExtender>
                                                                <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; background: #fff; border: solid;" CssClass="roundCorner shadow custsetup-popup">
                                                                    <div>
                                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                                            <ContentTemplate>
                                                                                <iframe id="iframeTicket" runat="server" width="1024px" height="600px" frameborder="0"
                                                                                    src=""></iframe>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </asp:Panel>
                                                                <asp:Button runat="server" ID="hideModalPopupViaServer" Style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px; display: none;"
                                                                    Text="Close" OnClick="hideModalPopupViaServer_Click"
                                                                    CausesValidation="False" />
                                                            </div>
                                                            <div class="form-section-row">
                                                                <asp:Panel runat="server" ID="pnlMerchant" CssClass="table-merchant" Style="background-color: #fff; border: 1px solid #316b9d">
                                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddlMerchantID" />
                                                                            <asp:AsyncPostBackTrigger ControlID="imgbtnMerchant" />
                                                                        </Triggers>
                                                                        <ContentTemplate>
                                                                            <div id="divInner">
                                                                                <div runat="server" id="titleBar" class="model-popup-body">
                                                                                    <asp:Label CssClass="title_text" ID="Label1" Style="color: white" runat="server">Merchant</asp:Label>
                                                                                    <asp:LinkButton ID="lnkCancelMerchant" runat="server" CausesValidation="False" Style="float: right; color: #fff; margin-left: 10px; height: 16px;"
                                                                                        OnClick="lnkCancelMerchant_Click">Close</asp:LinkButton>
                                                                                    <asp:LinkButton ID="lnkSaveMerchant" runat="server" Height="16px" Style="float: right; color: #fff; margin-left: 10px;"
                                                                                        ValidationGroup="mrt" OnClick="lnkSaveMerchant_Click">Save</asp:LinkButton>
                                                                                    <asp:LinkButton ID="imgbtnDelete" Visible="false" runat="server" ImageUrl="images/delete.png" CausesValidation="false"
                                                                                        ToolTip="Delete Merchant" Style="width: 40px; float: right; color: #fff; margin-right: 15px; height: 20px; width: 20px;"
                                                                                        OnClick="imgbtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the Merchant?')">Delete</asp:LinkButton>
                                                                                </div>
                                                                                <div style="padding: 15px">
                                                                                    <table style="height: 200px; padding: 10px">
                                                                                        <tr>
                                                                                            <td>Merchant ID
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtMerchantID"
                                        Display="None" ErrorMessage="Merchant ID Required" SetFocusOnError="True" ValidationGroup="mrt"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator13_ValidatorCalloutExtender"
                                                                                                    runat="server" Enabled="True" PopupPosition="BottomRight" TargetControlID="RequiredFieldValidator13">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtMerchantID" runat="server" class="form-control" MaxLength="100"></asp:TextBox>
                                                                                                <asp:HiddenField ID="hdnMerchantInfoID" runat="server" Value="0" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Login ID
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtLoginID"
                                        Display="None" ErrorMessage="Login ID Required" SetFocusOnError="True" ValidationGroup="mrt"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator14_ValidatorCalloutExtender"
                                                                                                    runat="server" Enabled="True" PopupPosition="TopRight" TargetControlID="RequiredFieldValidator14">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtLoginID" runat="server" class="form-control" MaxLength="100"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Username<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server"
                                                                                                ControlToValidate="txtMerchantUsername" Display="None" ErrorMessage="Username Required"
                                                                                                SetFocusOnError="True" ValidationGroup="mrt"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                                    ID="RequiredFieldValidator15_ValidatorCalloutExtender" PopupPosition="TopLeft" runat="server" Enabled="True"
                                                                                                    TargetControlID="RequiredFieldValidator15">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtMerchantUsername" runat="server" class="form-control"
                                                                                                    MaxLength="20"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Password<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server"
                                                                                                ControlToValidate="txtMerchantPassword" Display="None" ErrorMessage="Password Required"
                                                                                                SetFocusOnError="True" ValidationGroup="mrt"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                                                    ID="RequiredFieldValidator16_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                                                    TargetControlID="RequiredFieldValidator16" PopupPosition="TopLeft">
                                                                                                </asp:ValidatorCalloutExtender>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtMerchantPassword" runat="server" class="form-control"
                                                                                                    MaxLength="100"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </asp:Panel>
                                                            </div>
                                                            <div class="cf"></div>
                                                        </div>
                                                    </div>

                                                    <div style="clear: both;"></div>
                                                </div>
                                            </div>
                        </li>

                        <li>
                            <div id="accrdFinancial" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-social-poll"></i>Financial</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    
                                                    
                                                    

                                                    <div class="form-section3">
                                                                                        <div class="section-ttle">Method of payment</div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">

                                                                                                <div class="form-section3">
                                                                                                    <div class="input-field col s12">
                                                                                                        <div class="row">
                                                                                                <label class="drpdwn-label">Pay Period</label>
                                                                                                <asp:DropDownList ID="ddlPayPeriod" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">Weekly</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Bi-Weekly</asp:ListItem>
                                                                                                    <asp:ListItem Value="3">Semi-Monthly</asp:ListItem>
                                                                                                    <asp:ListItem Value="4">Monthly</asp:ListItem>
                                                                                                    <asp:ListItem Value="5">Semi-Annually</asp:ListItem>
                                                                                                    <asp:ListItem Value="6">Annually</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                <div class="form-section3">
                                                                                                    <div class="input-field col s12">
                                                                                                        <div class="row">
                                                                                                <label class="drpdwn-label">Method</label>
                                                                                                <asp:DropDownList ID="ddlPayMethod" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">Salaried</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Hourly</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>
                                                                                                <div class="form-section3">
                                                                                                    <div class="input-field col s12">
                                                                                                        <div class="row">
                                                                                                <label class="drpdwn-label">Payroll Hours</label>
                                                                                                <asp:DropDownList ID="ddlPayrollHours" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">Fixed</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Variable</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </div>


                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                               <asp:TextBox ID="txtPRTaxGL" runat="server" ></asp:TextBox>
                                                                <asp:HiddenField ID="hdntxtPRTaxGL" runat="server" />
                                                                <label for="txtPRTaxGL">PR Tax GL</label>
                                                                <asp:RequiredFieldValidator ID="rfvtxtPRTaxGL" runat="server" ControlToValidate="txtPRTaxGL"
                                                                Display="None" ErrorMessage="PR Tax GL Required" SetFocusOnError="True" ValidationGroup="wage" ></asp:RequiredFieldValidator>

                                                                <asp:ValidatorCalloutExtender ID="vcetxtPRTaxGL" runat="server" Enabled="True" PopupPosition="BottomLeft"  
                                                                TargetControlID="rfvtxtPRTaxGL">
                                                                </asp:ValidatorCalloutExtender>
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label class="drpdwn-label">Default Wage</label>
                                                                <asp:DropDownList ID="ddlDefaultWage" runat="server" CssClass="browser-default">                                                                    
                                                                </asp:DropDownList>
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-sectioncustom1">
                                                                                        <div class="col s2">
                                                                                            <div class="section-ttle">Allowances</div>
                                                                                        </div>
                                                                                       
                                                                                        <div class="form-section3-blank" >
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col s3">
                                                                                            <div class="section-ttle">Federal</div>
                                                                                        </div>
                                                                                        <div class="form-section3-blank" >
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col s3">
                                                                                            <div class="section-ttle">State</div>
                                                                                        </div>
                                                                                        <div class="form-section3-blank" >
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col s3">

                                                                                            <div class="section-ttle">Local (Bracket)</div>

                                                                                        </div>
                                                                                        <div class="col s1">

                                                                                        </div>

                                                                                        <div class="col s2" style="font-weight: 600; padding-top: 20px;">
                                                                                            <label style="font-size: 0.9rem;">Status</label> 
                                                                                        </div>
                                                                                       
                                                                                        <div class="form-section3-blank" >
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col s3">
                                                                                            <asp:DropDownList ID="ddlaalownancesFederal" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">Single</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Married</asp:ListItem>
                                                                                                    <asp:ListItem Value="0">Head of Household</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Married filling Seprate</asp:ListItem>
                                                                                                </asp:DropDownList> 
                                                                                        </div>
                                                                                        <div class="form-section3-blank" >
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col s3">
                                                                                            <asp:DropDownList ID="ddlaalownancesState" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">Single</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Married</asp:ListItem>
                                                                                                    <asp:ListItem Value="0">Head of Household</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Married filling Seprate</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                        </div>
                                                                                        <div class="form-section3-blank" >
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col s3">

                                                                                            <asp:DropDownList ID="ddlaalownancesLocal" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">Single</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Married</asp:ListItem>
                                                                                                    <asp:ListItem Value="0">Head of Household</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Married filling Seprate</asp:ListItem>
                                                                                                </asp:DropDownList>

                                                                                        </div>
                                                                                        <div class="col s1">

                                                                                        </div>


                                                                                        <div class="col s2" style="font-weight: 600; padding-top: 20px;">
                                                                                            <label style="font-size: 0.9rem;"># Allowances</label> 
                                                                                        </div>
                                                                                       
                                                                                        <div class="form-section3-blank" >
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col s3">
                                                                                            <asp:TextBox ID="txtAllowncesFedral" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                                        </div>
                                                                                        <div class="form-section3-blank" >
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col s3">
                                                                                            <asp:TextBox ID="txtAllowncesState" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                                        </div>
                                                                                        <div class="form-section3-blank" >
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col s3">
                                                                                            <asp:TextBox ID="txtAllownceslocal" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>

                                                                                        </div>
                                                                                        <div class="col s1">

                                                                                        </div>

                                                                                        <div class="col s2" style="font-weight: 600; padding-top: 20px;">
                                                                                            <label style="font-size: 0.9rem;">Additional</label> 
                                                                                        </div>
                                                                                       
                                                                                        <div class="form-section3-blank" >
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col s3">
                                                                                            <asp:TextBox ID="txtAllowancesAdditonalFedral" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                                        </div>
                                                                                        <div class="form-section3-blank" >
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col s3">
                                                                                            <asp:TextBox ID="txtAllowancesAdditonalState" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                                        </div>
                                                                                        <div class="form-section3-blank" >
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col s3">
                                                                                            <asp:DropDownList ID="ddlAllowancesAdditonalLocal" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">None</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">NY City</asp:ListItem>
                                                                                                    <asp:ListItem Value="2">Yonkers</asp:ListItem>
                                                                                                </asp:DropDownList>

                                                                                        </div>
                                                                                        <div class="col s1">

                                                                                        </div>
                                                                                       
                                                                                    </div>
                                                                                   
                                                                                        
                                                                                </div>





                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            
                        </li>

                        <li>
                            <div id="accrdWages" class="collapsible-header accrd accordian-text-custom "><i class="mdi-social-poll"></i>Wages</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    
                                                    
                                                                                    <%--<div class="form-section3">
                                                                                         <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                &nbsp;
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
                                                                                                <div class="buttonContainer">
                                                                                                    <div class="btnlinks">
                                                                                                        <asp:LinkButton CssClass="icon-save" ID="btnAddWages" CausesValidation="false" runat="server" OnClientClick="OpenWageCategoryModal();return false" OnClick="btnAddWages_Click">Add</asp:LinkButton>
                                                                                                    </div>
                                                                                                    <div class="btnlinks">
                                                                                                        <asp:LinkButton CssClass="icon-save" ID="btnRemoveWages" runat="server" >Remove</asp:LinkButton>
                                                                                                    </div>
                                                                                                    <div class="btnlinks">
                                                                                                        <asp:LinkButton CssClass="icon-save" ID="btnReplicateWages" runat="server" >Replicate Wages</asp:LinkButton>
                                                                                                    </div>
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
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>

                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                
                                                                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_WageCategory"  PageSize="50"  
                                                                                                OnItemDataBound="RadGrid_WageCategory_ItemDataBound" OnNeedDataSource="RadGrid_WageCategory_NeedDataSource" OnPreRender="RadGrid_WageCategory_PreRender"
                                                                                                ShowStatusBar="true" runat="server"  Width="100%" Height="150px" AllowCustomPaging="True">
                                                                                                <CommandItemStyle />
                                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                                </ClientSettings>
                                                                                                <MasterTableView AutoGenerateColumns="false" >
                                                                                                    <Columns>
                                                                                                        
                                                                                                                <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderStyle-Width="30">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox ID="checkColumnWageCategory" runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn UniqueName="lblWageId" FilterDelay="5" DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false"
                                                                                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                                                                        
                                                                                                                        
                                                                                                                        
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="fdesc" HeaderText="Description" UniqueName="fdesc" SortExpression="fdesc"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:HyperLink ID="lblWageFdesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:HyperLink>
                                                                                                                        <asp:HiddenField ID="hdnWageGL" runat="server" Value='<%# Eval("GL") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageGLName" runat="server" Value='<%# Eval("GLName") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageReg" runat="server" Value='<%# Eval("Reg") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageOT" runat="server" Value='<%# Eval("OT") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageDT" runat="server" Value='<%# Eval("DT") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageTT" runat="server" Value='<%# Eval("TT") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageFIT" runat="server" Value='<%# Eval("FIT") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageFICA" runat="server" Value='<%# Eval("FICA") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageMEDI" runat="server" Value='<%# Eval("MEDI") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageFUTA" runat="server" Value='<%# Eval("FUTA") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageSIT" runat="server" Value='<%# Eval("SIT") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageVac" runat="server" Value='<%# Eval("Vac") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageWC" runat="server" Value='<%# Eval("WC") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageUni" runat="server" Value='<%# Eval("Uni") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageInUse" runat="server" Value='<%# Eval("InUse") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageYTD" runat="server" Value='<%# Eval("YTD") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageYTDH" runat="server" Value='<%# Eval("YTDH") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageOYTD" runat="server" Value='<%# Eval("OYTD") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageOYTDH" runat="server" Value='<%# Eval("OYTDH") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageDYTD" runat="server" Value='<%# Eval("DYTD") %>' />
										                                                                                <asp:HiddenField ID="hdnWageDYTDH" runat="server" Value='<%# Eval("DYTDH") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageTYTD" runat="server" Value='<%# Eval("TYTD") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageTYTDH" runat="server" Value='<%# Eval("TYTDH") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageNT" runat="server" Value='<%# Eval("NT") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageNYTD" runat="server" Value='<%# Eval("NYTD") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageNYTDH" runat="server" Value='<%# Eval("NYTDH") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageVacR" runat="server" Value='<%# Eval("VacR") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageCReg" runat="server" Value='<%# Eval("CReg") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageCOT" runat="server" Value='<%# Eval("COT") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageCDT" runat="server" Value='<%# Eval("CDT") %>' />                                        
                                                                                                                        <asp:HiddenField ID="hdnWageCNT" runat="server" Value='<%# Eval("CNT") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageCTT" runat="server" Value='<%# Eval("CTT") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageStatus" runat="server" Value='<%# Eval("Status") %>' />
                                                                                                                        <asp:HiddenField ID="hdnWageSick" runat="server" Value='<%# Eval("Sick") %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                
                                                                                                        
                                                                                                    </Columns>
                                                                                                </MasterTableView>
                                                                                            </telerik:RadGrid>
                                                                                            </div>
                                                                                        </div>
                                                                                       

                                                                                        
                                                                                    
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3">
                                                                                        <div class="input-field col s5">
                                                                                            <div class="section-ttle">Pay Rate</div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="section-ttle">Cost Burden</div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="txtRegularRate">Regular</label>
                                                                                                <asp:TextBox ID="txtRegularRate" runat="server" Style="text-align: right" onkeypress = "return numericOnly(this);"/>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="txtRegularWardanRate">Regular</label>
                                                                                                <asp:TextBox ID="txtRegularWardanRate" runat="server" Style="text-align: right" onkeypress = "return numericOnly(this);"/>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="txtOvertimeRate">Overtime</label>
                                                                                                <asp:TextBox ID="txtOvertimeRate" runat="server" Style="text-align: right" onkeypress = "return numericOnly(this);"/>
                                                                                               
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="txtOvertimeWardanRate">Overtime</label>
                                                                                                <asp:TextBox ID="txtOvertimeWardanRate" runat="server" Style="text-align: right" onkeypress = "return numericOnly(this);"/>
                                                                                               
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="txtTime">1.7 Time</label>
                                                                                                <asp:TextBox ID="txtTime" runat="server" Style="text-align: right" onkeypress = "return numericOnly(this);"/>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                         <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="txtWardanTime">1.7 Time</label>
                                                                                                <asp:TextBox ID="txtWardanTime" runat="server" Style="text-align: right" onkeypress = "return numericOnly(this);"/>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="txtDoubleTime">Double Time</label>
                                                                                                <asp:TextBox ID="txtDoubleTime" runat="server" Style="text-align: right" onkeypress = "return numericOnly(this);" />
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="txtDoubleWardanTime">Double Time</label>
                                                                                                <asp:TextBox ID="txtDoubleWardanTime" runat="server" Style="text-align: right" onkeypress = "return numericOnly(this);"/>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="txtTravelTime">Travel Time</label>
                                                                                                <asp:TextBox ID="txtTravelTime" runat="server" Style="text-align: right" onkeypress = "return numericOnly(this);"/>
                                                                                               
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s5">
                                                                                            <div class="row">
                                                                                                <label for="txtTravelWardanTime">Travel Time</label>
                                                                                                <asp:TextBox ID="txtTravelWardanTime" runat="server" Style="text-align: right" onkeypress = "return numericOnly(this);"/>
                                                                                               
                                                                                            </div>
                                                                                        </div>

                                                                                    </div>
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3">
                                                                                        <div class="section-ttle">Subject To</div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtWageCatGLExpAcct">GL Exp Acct</label>
                                                                                                <asp:TextBox ID="txtWageCatGLExpAcct" runat="server" />
                                                                                                <asp:HiddenField ID="hdntxtWageCatGLExpAcct" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label class="drpdwn-label">Status</label>
                                                                                                <asp:DropDownList ID="ddlWageCategoryStatus" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                         <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkFIT" CssClass="css-checkbox" Text="FIT" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                       
                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkSIT" CssClass="css-checkbox" Text="SIT" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkFICA" CssClass="css-checkbox" Text="FICA" runat="server" />
                                                                                            </div>
                                                                                        </div>

                                                                                        

                                                                                        
                                                                                        
                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkVacation" CssClass="css-checkbox" Text="Vacation" runat="server" />
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkMEDI" CssClass="css-checkbox" Text="MEDI" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                       
                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkWorkComp" CssClass="css-checkbox" Text="Work Comp" runat="server" />
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkFUTA" CssClass="css-checkbox" Text="FUTA" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                       
                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkUnion" CssClass="css-checkbox" Text="UNION" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="chkSick" CssClass="css-checkbox" Text="Sick" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>--%>












                                                                                            <div class="form-section3">
                                                                                                <div class="input-field col s12">
                                                                                                    <div class="row">
                                                                                                       
                                                                                                    </div>
                                                                                                </div>
                                                                                                
                                                                                            </div>
                                                                                            
                                                                                            <div class="form-section3-blank">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                            <div class="form-section3">
                                                                                                <div class="input-field col s12">
                                                                                                    <div class="row">
                                                                                                        <%--<div class="buttonContainer">
										                                                                     <div class="btnlinks">
                                                                                                        <asp:LinkButton CssClass="icon-save" ID="btnAddWages" CausesValidation="false" runat="server" OnClientClick="OpenWageCategoryModal();return false" OnClick="btnAddWages_Click">Add</asp:LinkButton>
                                                                                                    </div>
                                                                                                    <div class="btnlinks">
                                                                                                        <asp:LinkButton CssClass="icon-save" ID="btnRemoveWages" runat="server" >Remove</asp:LinkButton>
                                                                                                    </div>
									                                                                    </div>--%>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="input-field col s12" >
                                                                                                    <div class="checkrow">
                                                                                                        <span class="tro">
                                                                                                        </span>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>



                                                                                            <div class="form-section-row">
														                                        <%--<div class="grid_container">
															                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
																                                        <div class="table-scrollable" style="height: auto; overflow-y: auto;">
																	                                        <div class="RadGrid RadGrid_Material">
																		                                        <telerik:RadCodeBlock ID="RadCodeBlock7" runat="server">
																			                                        <script type="text/javascript">
                                                                                                                        function pageLoad() {
                                                                                                                            // debugger;
                                                                                                                            var grid = $find("<%= RadGrid_WageCategory.ClientID %>");
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




																		                                        <div id="gvWageCatDIV">
																			                                        

																				                                        

																				                                        <telerik:RadGrid ID="RadGrid_WageCategory" runat="server" CssClass="BomGrid" AutoGenerateColumns="False" Width="100%"
																					                                        RenderMode="Auto" AllowFilteringByColumn="true" ShowStatusBar="true" AllowMultiRowSelection="True" AllowPaging="true"
                                                                                                                            OnItemDataBound="RadGrid_WageCategory_ItemDataBound" OnNeedDataSource="RadGrid_WageCategory_NeedDataSource" OnPreRender="RadGrid_WageCategory_PreRender"
																					                                        ShowGroupPanel="false"
																					                                        PagerStyle-AlwaysVisible="true"
																					                                        PageSize="50" ShowFooter="true">
																					                                        <CommandItemStyle />
																					                                        <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
																					                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
																						                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
																						                                        
																					                                        </ClientSettings>
																					                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="true">
																						                                        <GroupByExpressions>
																							
																						                                        </GroupByExpressions>
																						                                        <Columns>
                                                                                                                                    <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderStyle-Width="30">
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <asp:CheckBox ID="checkColumnWageCategory" runat="server" />
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </telerik:GridTemplateColumn>
																							                                        <telerik:GridTemplateColumn HeaderText="Wage Category" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="fdesc" UniqueName="fdesc" SortExpression="fdesc">
																								                                        <ItemTemplate>
																									                                        <asp:Label ID="lblwagecatedesc" runat="server" Text='<%# Eval("fdesc") %>' Width="125" CssClass="left-align"></asp:Label>																									                                        
                                                                                                                                            <asp:HiddenField ID="hdnwagecateid" runat="server" Value='<%# Eval("id") %>' />																								                                        
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
																							                                        <telerik:GridTemplateColumn HeaderText="GL Expences" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="GLName" UniqueName="GLName" SortExpression="GLName">
																								                                        <ItemTemplate>
																									                                        <asp:Label ID="lblwagecategl" runat="server" Text='<%# Eval("GLName") %>' Width="125" CssClass="left-align"></asp:Label>
																									                                        <asp:HiddenField ID="hdnwagecategl" runat="server" Value='<%# Eval("GL") %>' />
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
																							                                        
																							                                        <telerik:GridTemplateColumn HeaderText="Regular Rate(Pay)" HeaderStyle-Width="125" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="Reg" UniqueName="Reg" SortExpression="Reg">
																								                                        <ItemTemplate>
																									                                        <asp:TextBox ID="txtregratepay" runat="server" Text='<%# Eval("Reg","{0:n}") %>' 
																										                                        CssClass="form-control input-sm input-small right-align" Width="100" onkeypress = "return numericOnly(this);"></asp:TextBox>
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Overtime(Pay)" HeaderStyle-Width="125" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="OT" UniqueName="OT" SortExpression="OT">
																								                                        <ItemTemplate>
																									                                        <asp:TextBox ID="txtotratepay" runat="server" Text='<%# Eval("OT","{0:n}") %>' 
																										                                        CssClass="form-control input-sm input-small right-align" Width="100" onkeypress = "return numericOnly(this);"></asp:TextBox>
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="1.7 Time(Pay)" HeaderStyle-Width="125" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="NT" UniqueName="NT" SortExpression="NT">
																								                                        <ItemTemplate>
																									                                        <asp:TextBox ID="txtntratepay" runat="server" Text='<%# Eval("NT","{0:n}") %>' 
																										                                        CssClass="form-control input-sm input-small right-align" Width="100" onkeypress = "return numericOnly(this);"></asp:TextBox>
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Double Time(Pay)" HeaderStyle-Width="125" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="DT" UniqueName="DT" SortExpression="DT">
																								                                        <ItemTemplate>
																									                                        <asp:TextBox ID="txtdtratepay" runat="server" Text='<%# Eval("DT","{0:n}") %>' 
																										                                        CssClass="form-control input-sm input-small right-align" Width="100" onkeypress = "return numericOnly(this);"></asp:TextBox>
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Travel Time(Pay)" HeaderStyle-Width="125" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="TT" UniqueName="TT" SortExpression="TT">
																								                                        <ItemTemplate>
																									                                        <asp:TextBox ID="txtttratepay" runat="server" Text='<%# Eval("TT","{0:n}") %>' 
																										                                        CssClass="form-control input-sm input-small right-align" Width="100" onkeypress = "return numericOnly(this);"></asp:TextBox>
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Regular Rate(Burden)" HeaderStyle-Width="125" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="CReg" UniqueName="CReg" SortExpression="CReg">
																								                                        <ItemTemplate>
																									                                        <asp:TextBox ID="txtregrateburden" runat="server" Text='<%# Eval("CReg","{0:n}") %>' 
																										                                        CssClass="form-control input-sm input-small right-align" Width="100" onkeypress = "return numericOnly(this);"></asp:TextBox>
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Overtime(Burden)" HeaderStyle-Width="125" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="COT" UniqueName="COT" SortExpression="COT">
																								                                        <ItemTemplate>
																									                                        <asp:TextBox ID="txtotrateburden" runat="server" Text='<%# Eval("COT","{0:n}") %>' 
																										                                        CssClass="form-control input-sm input-small right-align" Width="100" onkeypress = "return numericOnly(this);"></asp:TextBox>
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="1.7 Time(Burden)" HeaderStyle-Width="125" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="CNT" UniqueName="CNT" SortExpression="CNT">
																								                                        <ItemTemplate>
																									                                        <asp:TextBox ID="txtntrateburden" runat="server" Text='<%# Eval("CNT","{0:n}") %>' 
																										                                        CssClass="form-control input-sm input-small right-align" Width="100" onkeypress = "return numericOnly(this);"></asp:TextBox>
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Double Time(Burden)" HeaderStyle-Width="125" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="CDT" UniqueName="CDT" SortExpression="CDT">
																								                                        <ItemTemplate>
																									                                        <asp:TextBox ID="txtdtrateburden" runat="server" Text='<%# Eval("CDT","{0:n}") %>' 
																										                                        CssClass="form-control input-sm input-small right-align" Width="100" onkeypress = "return numericOnly(this);"></asp:TextBox>
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Travel Time(Burden)" HeaderStyle-Width="125" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="CTT" UniqueName="CTT" SortExpression="CTT">
																								                                        <ItemTemplate>
																									                                        <asp:TextBox ID="txtttrateburden" runat="server" Text='<%# Eval("CTT","{0:n}") %>' 
																										                                        CssClass="form-control input-sm input-small right-align" Width="100" onkeypress = "return numericOnly(this);"></asp:TextBox>
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
																							                                        
																						                                        </Columns>
																					                                        </MasterTableView>
																				                                        </telerik:RadGrid>
																			                                        
																		                                        </div>

																	                                        </div>
																                                        </div>
															                                        </div>
														                                        </div>--%>




                                                                                                <div class="btnlinks" style="margin-bottom: 10px;">
                                                                    <asp:LinkButton ID="btnCopyRate" runat="server" ToolTip="Copy selected rates to all" Text="Copy Rates"
                                                                        OnClientClick="return CheckCopyRate('gvWagePayRate.ClientID');" OnClick="btnCopyRate_Click">
                                                                    </asp:LinkButton>
                                                                </div>
                                                                <div class="grid_container">
                                                                    <asp:HiddenField ID="hdnWageRate" runat="server" />
                                                                    <div class="RadGrid RadGrid_Material">

                                                                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_gvWagePayRate" runat="server">
                                                                        </telerik:RadAjaxLoadingPanel>
                                                                        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                                                                            <script type="text/javascript">
                                                                                function pageLoad() {
                                                                                    var grid = $find("<%= gvWagePayRate.ClientID %>");
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
                                                                        <telerik:RadAjaxPanel ID="UpdatePanel6" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvWagePayRate" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                            <%--    <ContentTemplate>--%>
                                                                            <telerik:RadGrid ID="gvWagePayRate" runat="server" AutoGenerateColumns="False" FilterType="CheckList"
                                                                                CssClass="gvWagePayRate table table-bordered table-striped table-condensed flip-content" margin-bottom="0px"
                                                                                AllowSorting="true"
                                                                                ShowFooter="true"
                                                                                OnRowCommand="gvWagePayRate_RowCommand"
                                                                                OnItemDataBound="gvWagePayRate_ItemDataBound"
                                                                                EnableViewState="true"
                                                                                OnNeedDataSource="gvWagePayRate_NeedDataSource">
                                                                                <AlternatingItemStyle CssClass="oddrowcolor" />
                                                                                <ActiveItemStyle CssClass="evenrowcolor" />
                                                                                <FooterStyle CssClass="footer" />
                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                </ClientSettings>
                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="Reg">
                                                                                    <ColumnGroups>
                                                                                        <telerik:GridColumnGroup HeaderText="Pay Rate" HeaderStyle-CssClass="hdsPayRate" Name="PayRate" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                                                        <telerik:GridColumnGroup HeaderText="Burden Rate" HeaderStyle-CssClass="hdsBurdenRate" Name="BurdenRate" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                                                        <telerik:GridColumnGroup HeaderText="" Name="Checkbox" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                                                        <telerik:GridColumnGroup HeaderText="" Name="OrderNumber" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                                                        <telerik:GridColumnGroup HeaderText="" Name="Wage" HeaderStyle-CssClass="hdsWage" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                                                    </ColumnGroups>
                                                                                    <Columns>
                                                                                        <telerik:GridTemplateColumn AllowFiltering="false" ColumnGroupName="Checkbox" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                                                                            <HeaderTemplate>
                                                                                                <asp:ImageButton ID="imgBtnWageDelete" runat="server" CausesValidation="false" OnClick="imgBtnWageDelete_Click"
                                                                                                    OnClientClick="return CheckDelete('gvWagePayRate.ClientID');"
                                                                                                    ImageUrl="images/menu_delete.png" Width="13px" />
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:LinkButton ID="imgBtnWageAdd" runat="server" CommandName="AddWage" CausesValidation="False"
                                                                                                    CommandArgument="<%# ((GridItem) Container).ItemIndex %>"
                                                                                                    Width="30" OnClientClick="itemJSON();" OnClick="imgBtnWageAdd_Click" Text="Add"><i class="fa fa-plus-circle iconAdd_gvWagePayRate" ></i></asp:LinkButton>
                                                                                            </FooterTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="No." ColumnGroupName="OrderNumber" SortExpression="id" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblNum" runat="server" Text='<%# ((GridItem) Container).ItemIndex + 1 %>'></asp:Label>

                                                                                                <asp:HiddenField ID="hdnGL" runat="server" Value='<%# Eval("GL") %>'/>
                                                                                                <asp:HiddenField ID="hdnFIT" runat="server" Value='<%# Eval("FIT") %>'/>
                                                                                                <asp:HiddenField ID="hdnFICA" runat="server" Value='<%# Eval("FICA") %>'/>
                                                                                                <asp:HiddenField ID="hdnMEDI" runat="server" Value='<%# Eval("MEDI") %>'/>
                                                                                                <asp:HiddenField ID="hdnFUTA" runat="server" Value='<%# Eval("FUTA") %>'/>
                                                                                                <asp:HiddenField ID="hdnSIT" runat="server" Value='<%# Eval("SIT") %>'/>
                                                                                                <asp:HiddenField ID="hdnVac" runat="server" Value='<%# Eval("Vac") %>'/>
                                                                                                <asp:HiddenField ID="hdnWc" runat="server" Value='<%# Eval("Wc") %>'/>
                                                                                                <asp:HiddenField ID="hdnUni" runat="server" Value='<%# Eval("Uni") %>'/>
                                                                                                <asp:HiddenField ID="hdnInUse" runat="server" Value='<%# Eval("InUse") %>'/>
                                                                                                <asp:HiddenField ID="hdnSick" runat="server" Value='<%# Eval("Sick") %>'/>
                                                                                                <asp:HiddenField ID="hdnStatus" runat="server" Value='<%# Eval("Status") %>'/>
                                                                                                <asp:HiddenField ID="hdnYTD" runat="server" Value='<%# Eval("YTD") %>'/>
                                                                                                <asp:HiddenField ID="hdnYTDH" runat="server" Value='<%# Eval("YTDH") %>'/>
                                                                                                <asp:HiddenField ID="hdnOYTD" runat="server" Value='<%# Eval("OYTD") %>'/>
                                                                                                <asp:HiddenField ID="hdnOYTDH" runat="server" Value='<%# Eval("OYTDH") %>' />
                                                                                                <asp:HiddenField ID="hdnDYTD" runat="server"  Value='<%# Eval("DYTD") %>'/>
                                                                                                <asp:HiddenField ID="hdnDYTDH" runat="server" Value='<%# Eval("DYTDH") %>' />
                                                                                                <asp:HiddenField ID="hdnTYTD" runat="server"  Value='<%# Eval("TYTD") %>'/>
                                                                                                <asp:HiddenField ID="hdnTYTDH" runat="server" Value='<%# Eval("TYTDH") %>' />
                                                                                                <asp:HiddenField ID="hdnNYTD" runat="server"  Value='<%# Eval("NYTD") %>'/>
                                                                                                <asp:HiddenField ID="hdnNYTDH" runat="server" Value='<%# Eval("NYTDH") %>' />
                                                                                                <asp:HiddenField ID="hdnVacR" runat="server"  Value='<%# Eval("VacR") %>'/>
                                                                                                <asp:HiddenField ID="hdnfdesc" runat="server" Value='<%# Eval("fdesc") %>' />
                                                                                                <asp:HiddenField ID="hdnGLName" runat="server"  Value='<%# Eval("GLName") %>'/>
                                                                                                
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Wage" ColumnGroupName="Wage" SortExpression="Wage" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList ID="ddlWage" runat="server" DataValueField="Wage" DataTextField="fDesc" CssClass="browser-default"
                                                                                                    AutoPostBack="true" DataSource='<%#dtWage%>' Width="100%" OnSelectedIndexChanged="ddlWage_SelectedIndexChanged"
                                                                                                    SelectedValue='<%# Eval("Wage") == DBNull.Value ? "0" : Eval("Wage") %>'>
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Regular" SortExpression="Reg" ColumnGroupName="PayRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            DataField="Reg" UniqueName="Reg"
                                                                                            HeaderStyle-CssClass="PayRate" ItemStyle-CssClass="PayRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtReg" runat="server" Text='<%#Eval("Reg","{0:0.00}") %>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                                <%--<asp:Label ID="txtReg" runat="server" Text='<%#Eval("Reg","{0:0.00}") %>' Width="100%" ></asp:Label>--%>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Overtime" SortExpression="OT" ColumnGroupName="PayRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            DataField="OT" UniqueName="OT"
                                                                                            HeaderStyle-CssClass="PayRate" ItemStyle-CssClass="PayRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtOt" runat="server" Text='<%#Eval("OT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="1.7 Time" SortExpression="NT" ColumnGroupName="PayRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            DataField="NT" UniqueName="NT"
                                                                                            HeaderStyle-CssClass="PayRate" ItemStyle-CssClass="PayRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtNt" runat="server" Text='<%#Eval("NT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content"
                                                                                                    onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Double Time" SortExpression="DT" ColumnGroupName="PayRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            DataField="DT" UniqueName="DT"
                                                                                            HeaderStyle-CssClass="PayRate" ItemStyle-CssClass="PayRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtDt" runat="server" Text='<%#Eval("DT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Travel" SortExpression="TT" ColumnGroupName="PayRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            DataField="TT" UniqueName="TT"
                                                                                            HeaderStyle-CssClass="PayRate" ItemStyle-CssClass="PayRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtTt" runat="server" Text='<%#Eval("TT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Regular" SortExpression="CReg" ColumnGroupName="BurdenRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            DataField="CReg" UniqueName="CReg"
                                                                                            HeaderStyle-CssClass="BurdenRate" ItemStyle-CssClass="BurdenRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCReg" runat="server" Text='<%#Eval("CReg","{0:0.00}") %>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Overtime" SortExpression="COT" ColumnGroupName="BurdenRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            DataField="COT" UniqueName="COT"
                                                                                            HeaderStyle-CssClass="BurdenRate" ItemStyle-CssClass="BurdenRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCOt" runat="server" Text='<%#Eval("COT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="1.7 Time" SortExpression="CNT" ColumnGroupName="BurdenRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            DataField="CNT" UniqueName="CNT"
                                                                                            HeaderStyle-CssClass="BurdenRate" ItemStyle-CssClass="BurdenRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCNt" runat="server" Text='<%# Eval("CNT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Double Time" SortExpression="CDT" ColumnGroupName="BurdenRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            DataField="CDT" UniqueName="CDT"
                                                                                            HeaderStyle-CssClass="BurdenRate" ItemStyle-CssClass="BurdenRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCDt" runat="server" Text='<%#Eval("CDT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Travel" SortExpression="CTT" ColumnGroupName="BurdenRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            DataField="CTT" UniqueName="CTT"
                                                                                            HeaderStyle-CssClass="BurdenRate" ItemStyle-CssClass="BurdenRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCTt" runat="server" Text='<%#Eval("CTT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                    </Columns>
                                                                                </MasterTableView>
                                                                                <SelectedItemStyle CssClass="selectedrowcolor" />
                                                                                <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                                </FilterMenu>
                                                                            </telerik:RadGrid>
                                                                            <%--</ContentTemplate>--%>
                                                                        </telerik:RadAjaxPanel>
                                                                    </div>
                                                                    <div class="cf"></div>
                                                                </div>




                                                                                            </div>















                                                                                   
                                                                                        
                                                                                </div>





                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            
                        </li>

                        <li>
                            <div id="accrdOtherIncome" class="collapsible-header accrd accordian-text-custom "><i class="mdi-social-poll"></i>Other Income</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    
                                                    
                                                    

                                                                                    <div class="form-section3">
                                                                                        
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <%--<label class="drpdwn-label">Pay Period</label>
                                                                                                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                                                                </asp:DropDownList>--%>
                                                                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_WageCategoryOtherIncome"  PageSize="50" OnItemDataBound="RadGrid_WageCategoryOtherIncome_ItemDataBound"
                                                                                                OnNeedDataSource="RadGrid_WageCategoryOtherIncome_NeedDataSource" OnPreRender="RadGrid_WageCategoryOtherIncome_PreRender"
                                                                                                ShowStatusBar="true" runat="server"  Width="100%" Height="150px" AllowCustomPaging="True">
                                                                                                <CommandItemStyle />
                                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                                </ClientSettings>
                                                                                                <MasterTableView AutoGenerateColumns="false" >
                                                                                                    <Columns>
                                                                                                        <%--<telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="30">
                                                                                                                </telerik:GridClientSelectColumn>--%>


                                                                                                                <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderStyle-Width="30">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox ID="checkColumnWageCategoryOtherIncome" runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>


                                                                                                                <telerik:GridTemplateColumn UniqueName="lblWageId" FilterDelay="5" DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false"
                                                                                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblID1" runat="server" Text='<%# Eval("Cat") %>'></asp:Label>
                                                                                                                        
                                                                                                                        
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="fdesc" HeaderText="Description" UniqueName="fdesc" SortExpression="fdesc"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:HyperLink ID="lblWageFdesc1" runat="server" Text='<%# Eval("fdesc") %>'></asp:HyperLink>
                                                                                                                        <asp:HiddenField ID="hdnotherGL" runat="server" Value='<%# Eval("GL") %>' />
                                                                                                                        <asp:HiddenField ID="hdnotherGLName" runat="server" Value='<%# Eval("ExpAcctName") %>' />
                                                                                                                        <asp:HiddenField ID="hdnotherRate" runat="server" Value='<%# Eval("Rate") %>' />
                                                                                                                        <asp:HiddenField ID="hdnotherFIT" runat="server" Value='<%# Eval("FIT") %>' />
                                                                                                                        <asp:HiddenField ID="hdnotherFICA" runat="server" Value='<%# Eval("FICA") %>' />
                                                                                                                        <asp:HiddenField ID="hdnotherMEDI" runat="server" Value='<%# Eval("MEDI") %>' />
                                                                                                                        <asp:HiddenField ID="hdnotherFUTA" runat="server" Value='<%# Eval("FUTA") %>' />
                                                                                                                        <asp:HiddenField ID="hdnotherSIT" runat="server" Value='<%# Eval("SIT") %>' />
                                                                                                                        <asp:HiddenField ID="hdnotherVac" runat="server" Value='<%# Eval("Vac") %>' />
                                                                                                                        <asp:HiddenField ID="hdnotherWc" runat="server" Value='<%# Eval("WC") %>' />
                                                                                                                        <asp:HiddenField ID="hdnotherUni" runat="server" Value='<%# Eval("Uni") %>' />
                                                                                                                        <asp:HiddenField ID="hdnotherSick" runat="server" Value='<%# Eval("Sick") %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                
                                                                                                        
                                                                                                    </Columns>
                                                                                                </MasterTableView>
                                                                                            </telerik:RadGrid>
                                                                                            </div>
                                                                                        </div>
                                                                                       

                                                                                        
                                                                                    </div>
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    
                                                                                    <div class="form-section3">
                                                                                        <div class="section-ttle">Subject To</div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtWageCatGLExpAcctOtherIncome">GL Exp Acct</label>
                                                                                                <asp:TextBox ID="txtWageCatGLExpAcctOtherIncome" runat="server" />
                                                                                                <asp:HiddenField ID="hdntxtWageCatGLExpAcctOtherIncome" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtWageCatRateOtherIncome">Rate</label>
                                                                                                <asp:TextBox ID="txtWageCatRateOtherIncome" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                         <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="cbFit" CssClass="css-checkbox" Text="FIT" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <%--<div class="input-field col s2">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>--%>
                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="cbSit" CssClass="css-checkbox" Text="SIT" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="cbFica" CssClass="css-checkbox" Text="FICA" runat="server" />
                                                                                            </div>
                                                                                        </div>

                                                                                        

                                                                                        
                                                                                        
                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="cbVaction" CssClass="css-checkbox" Text="Vacation" runat="server" />
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="cbMedi" CssClass="css-checkbox" Text="MEDI" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                       
                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="cbWc" CssClass="css-checkbox" Text="Work Comp" runat="server" />
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="cbFuta" CssClass="css-checkbox" Text="FUTA" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                       
                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="cbUnion" CssClass="css-checkbox" Text="UNION" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s4 mgntp10">
                                                                                            <div class="checkrow">
                                                                                                <asp:CheckBox ID="cbSick" CssClass="css-checkbox" Text="Sick" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                    
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3">
                                                                                    </div>


                                                                                   
                                                                                        
                                                                                </div>





                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            
                        </li>
                        
                        <li>
                            <div id="accrdDeductions" class="collapsible-header accrd accordian-text-custom "><i class="mdi-social-poll"></i>Deductions</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    
                                                    
                                                    

                                                                                    <%--<div class="form-section3">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <div class="buttonContainer">
                                                                                                    <div class="btnlinks">
                                                                                                        <asp:LinkButton CssClass="icon-save" ID="btndeductionAdd" runat="server" OnClientClick="OpenWageDeductionModal();return false" OnClick="btndeductionAdd_Click">Add</asp:LinkButton>
                                                                                                    </div>
                                                                                                    <div class="btnlinks">
                                                                                                        <asp:LinkButton CssClass="icon-save" ID="btndeductionRemove" runat="server" >Remove</asp:LinkButton>
                                                                                                    </div>
                                                                                                    
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                
                                                                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_WageDeduction"  PageSize="50"  
                                                                                                OnItemDataBound="RadGrid_WageDeduction_ItemDataBound" OnNeedDataSource="RadGrid_WageDeduction_NeedDataSource" OnPreRender="RadGrid_WageDeduction_PreRender"
                                                                                                ShowStatusBar="true" runat="server" Width="100%" Height="150px" AllowCustomPaging="True">
                                                                                                <CommandItemStyle />
                                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                                </ClientSettings>
                                                                                                <MasterTableView AutoGenerateColumns="false" >
                                                                                                    <Columns>
                                                                                                        
                                                                                                        <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderStyle-Width="30">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:CheckBox ID="checkColumnWageDeduction" runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn UniqueName="lblWageId" FilterDelay="5" DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false"
                                                                                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblIDdedu1" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                                                                        
                                                                                                                        


                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="fdesc" HeaderText="Description" UniqueName="fdesc" SortExpression="fdesc"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:HyperLink ID="lblWageFdescdedu1" runat="server" Text='<%# Eval("fdesc") %>'></asp:HyperLink>
                                                                                                                        <asp:HiddenField ID="hdndeduBasedOn" runat="server" Value='<%# Eval("BasedOn") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduAccruedOn" runat="server" Value='<%# Eval("AccruedOn") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduByW" runat="server" Value='<%# Eval("ByW") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduEmpRate" runat="server" Value='<%# Eval("EmpRate") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduEmpTop" runat="server" Value='<%# Eval("EmpTop") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduEmpGL" runat="server" Value='<%# Eval("EmpGL") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduEmpGLName" runat="server" Value='<%# Eval("EmpGLName") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduCompRate" runat="server" Value='<%# Eval("CompRate") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduCompTop" runat="server" Value='<%# Eval("CompTop") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduCompGL" runat="server" Value='<%# Eval("CompGL") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduCompGLName" runat="server" Value='<%# Eval("CompGLName") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduCompGLE" runat="server" Value='<%# Eval("CompGLE") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduCompGLEName" runat="server" Value='<%# Eval("CompGLEName") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduInUse" runat="server" Value='<%# Eval("InUse") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduYTD" runat="server" Value='<%# Eval("YTD") %>' />
                                                                                                                        <asp:HiddenField ID="hdndeduYTDC" runat="server" Value='<%# Eval("YTDC") %>' />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                
                                                                                                        
                                                                                                    </Columns>
                                                                                                </MasterTableView>
                                                                                            </telerik:RadGrid>
                                                                                            </div>
                                                                                        </div>
                                                                                       

                                                                                        
                                                                                    </div>
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    
                                                                                    <div class="form-section9">
                                                                                        
                                                                                        <div class="input-field col s3">
                                                                                            <div class="row">
                                                                                                <label class="drpdwn-label">Paid By</label>
                                                                                                <asp:DropDownList ID="ddlPaidBy" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">Company</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Employee</asp:ListItem>
                                                                                                    <asp:ListItem Value="2">Both</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s3">
                                                                                            <div class="row">
                                                                                                <label class="drpdwn-label">Based On</label>
                                                                                                <asp:DropDownList ID="ddlBasedOn" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">FIT</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">FICA</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">MEDI</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">FUTA</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">SIT</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Vacation</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Work Comp</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Union</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Flate Rate</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s3">
                                                                                            <div class="row">
                                                                                                <label class="drpdwn-label">Accrued On</label>
                                                                                                <asp:DropDownList ID="ddlAccruedOn" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0"># Hours</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Dollar Amount</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Flate Rate</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>

                                                                                         <div class="input-field col s3">
                                                                                            <div class="row">
                                                                                                <label for="txtEmployeeRate">Employee Rate</label>
                                                                                                <asp:TextBox ID="txtEmployeeRate" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s3">
                                                                                            <div class="row">
                                                                                                <label for="txtEmployeeCeiling">Employee Ceiling</label>
                                                                                                <asp:TextBox ID="txtEmployeeCeiling" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s3">
                                                                                            <div class="row">
                                                                                                <label for="txtEmployeeGL">Employee GL</label>
                                                                                                <asp:TextBox ID="txtEmployeeGL" runat="server" />
                                                                                                <asp:HiddenField ID="hdntxtEmployeeGL" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s3">
                                                                                            <div class="row">
                                                                                                <label for="txtCompanyRate">Company Rate</label>
                                                                                                <asp:TextBox ID="txtCompanyRate" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s3">
                                                                                            <div class="row">
                                                                                                <label for="txtCompanyCeiling">Company Ceiling</label>
                                                                                                <asp:TextBox ID="txtCompanyCeiling" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s3">
                                                                                            <div class="row">
                                                                                                <label for="txtCompanyGL">Company GL</label>
                                                                                                <asp:TextBox ID="txtCompanyGL" runat="server" />
                                                                                                <asp:HiddenField ID="hdntxtCompanyGL" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="input-field col s3">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                                
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s3">
                                                                                            <div class="row">
                                                                                                
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s3">
                                                                                            <div class="row">
                                                                                                <label for="txtCompanyExpGL">Company Exp GL</label>
                                                                                                <asp:TextBox ID="txtCompanyExpGL" runat="server" />
                                                                                                <asp:HiddenField ID="hdntxtCompanyExpGL" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s1">
                                                                                            <div class="row">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                        </div>


                                                                                         
                                                                                    </div>--%>
                                                    
                                                                                    








                                                                                            <div class="form-section3">
                                                                                                <div class="input-field col s12">
                                                                                                    <div class="row">
                                                                                                       
                                                                                                    </div>
                                                                                                </div>
                                                                                                
                                                                                            </div>
                                                                                            
                                                                                            <div class="form-section3-blank">
                                                                                                &nbsp;
                                                                                            </div>
                                                                                            <div class="form-section3">
                                                                                                <div class="input-field col s12">
                                                                                                    <div class="row">
                                                                                                        <div class="buttonContainer">
                                                                                                            <%--<div class="btnlinks">
                                                                                                                <asp:LinkButton CssClass="icon-save" ID="btndeductionAdd" runat="server" OnClientClick="OpenWageDeductionModal();return false" OnClick="btndeductionAdd_Click">Add</asp:LinkButton>
                                                                                                            </div>
                                                                                                            <div class="btnlinks">
                                                                                                                <asp:LinkButton CssClass="icon-save" ID="btndeductionRemove" runat="server" >Remove</asp:LinkButton>
                                                                                                            </div>--%>
                                                                                                    
                                                                                                        </div>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <div class="input-field col s12" >
                                                                                                    <div class="checkrow">
                                                                                                        <span class="tro">
                                                                                                        </span>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>



                                                                                            <div class="form-section-row">
                                                                                                <div class="btnlinks" style="margin-bottom: 10px;">
                                                                                                    <asp:LinkButton ID="btnCopyRateD" runat="server" ToolTip="Copy selected rates to all" Text="Copy Rates"
                                                                                                        OnClientClick="return CheckCopyRateDed('RadGridWageDeduction.ClientID');" OnClick="btnCopyRateDed_Click">
                                                                                                    </asp:LinkButton>
                                                                                                </div>





														                                        <div class="grid_container">
                                                                                                    <asp:HiddenField ID="hdnWageDRate" runat="server" />
															                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
																                                        <div class="table-scrollable" style="height: auto; overflow-y: auto;">
																	                                        <div class="RadGrid RadGrid_Material">
																		   <%--                                     <telerik:RadCodeBlock ID="RadCodeBlock10" runat="server">
																			                                        <script type="text/javascript">
                                                                                                                        function pageLoad() {
                                                                                                                            // debugger;
                                                                                                                            var grid = $find("<%= RadGrid_WageDeduction.ClientID %>");
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




																		                                        <div id="gvWageDedDIV">
																			                                        

																				                                        

																				                                        <telerik:RadGrid ID="RadGrid_WageDeduction" runat="server" CssClass="BomGrid" AutoGenerateColumns="False" Width="100%"
																					                                        RenderMode="Auto" AllowFilteringByColumn="true" ShowStatusBar="true" AllowMultiRowSelection="True" AllowPaging="true"
                                                                                                                            OnItemDataBound="RadGrid_WageDeduction_ItemDataBound" OnNeedDataSource="RadGrid_WageDeduction_NeedDataSource" OnPreRender="RadGrid_WageDeduction_PreRender"
																					                                        ShowGroupPanel="false"
																					                                        PagerStyle-AlwaysVisible="true"
																					                                        PageSize="50" ShowFooter="true">
																					                                        <CommandItemStyle />
																					                                        <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
																					                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowGroupExpandCollapse="false">
																						                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
																						                                        
																					                                        </ClientSettings>
																					                                        <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="true" AllowPaging="true">
																						                                        <GroupByExpressions>
																							
																						                                        </GroupByExpressions>
																						                                        <Columns>
                                                                                                                                    <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderStyle-Width="30">
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <asp:CheckBox ID="checkColumnWageDeduction" runat="server" />
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </telerik:GridTemplateColumn>
																							                                        <telerik:GridTemplateColumn HeaderText="Dedcution" HeaderStyle-Width="250" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="fdesc" UniqueName="fdesc" SortExpression="fdesc">
																								                                        <ItemTemplate>
																									                                        <asp:Label ID="lbldeductioncatedesc" runat="server" Text='<%# Eval("fdesc") %>' Width="125" CssClass="left-align"></asp:Label>																									                                        
                                                                                                                                            <asp:HiddenField ID="hdndeductioncateid" runat="server" Value='<%# Eval("id") %>' />																								                                        
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
																							                                        <telerik:GridTemplateColumn HeaderText="Based on" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="BasedOn" UniqueName="BasedOn" SortExpression="BasedOn">
																								                                        <ItemTemplate>
																									                                        <asp:Label ID="lbldeductioncatebasedon" runat="server" Text='<%# Eval("BasedOnDesc") %>' Width="125" CssClass="left-align"></asp:Label>
																									                                        
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
																							                                        <telerik:GridTemplateColumn HeaderText="Accured on" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="AccruedOn" UniqueName="AccruedOn" SortExpression="AccruedOn">
																								                                        <ItemTemplate>
																									                                        <asp:Label ID="lbldeductioncateaccured" runat="server" Text='<%# Eval("AccruedOnDesc") %>' Width="125" CssClass="left-align"></asp:Label>
																									                                        
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Employee" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="EmpRate" UniqueName="EmpRate" SortExpression="EmpRate">
																								                                        <ItemTemplate>
																									                                        <asp:Label ID="lbldeductioncateemployee" runat="server" Text='<%# Eval("EmpRate") %>' Width="125" CssClass="right-align"></asp:Label>
																									                                        
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Employee Ceiling" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="EmpTop" UniqueName="EmpTop" SortExpression="EmpTop">
																								                                        <ItemTemplate>
																									                                        <asp:Label ID="lbldeductioncateemployeeceil" runat="server" Text='<%# Eval("EmpTop") %>' Width="125" CssClass="left-align"></asp:Label>
																									                                        
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Employee GL" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="EmpGL" UniqueName="EmpGL" SortExpression="EmpGL">
																								                                        <ItemTemplate>
																									                                        <asp:Label ID="lbldeductioncateemployeegl" runat="server" Text='<%# Eval("EmpGLName") %>' Width="125" CssClass="left-align"></asp:Label>
																									                                        <asp:HiddenField ID="hdndeductioncateemployeegl" runat="server" Value='<%# Eval("EmpGL") %>' />
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Company" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="CompRate" UniqueName="CompRate" SortExpression="CompRate">
																								                                        <ItemTemplate>
																									                                        <asp:Label ID="lbldeductioncatecompany" runat="server" Text='<%# Eval("CompRate") %>' Width="125" CssClass="left-align"></asp:Label>
																									                                        
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Company Ceiling" HeaderStyle-Width="100" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="CompTop" UniqueName="CompTop" SortExpression="CompTop">
																								                                        <ItemTemplate>
																									                                        <asp:Label ID="lbldeductioncatecompanyceil" runat="server" Text='<%# Eval("CompTop") %>' Width="125" CssClass="left-align"></asp:Label>
																									                                        
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Company GL" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="CompGL" UniqueName="CompGL" SortExpression="CompGL">
																								                                        <ItemTemplate>
																									                                        <asp:Label ID="lbldeductioncatecomapnygl" runat="server" Text='<%# Eval("CompGLName") %>' Width="125" CssClass="left-align"></asp:Label>
																									                                        <asp:HiddenField ID="hdndeductioncatecomapnygl" runat="server" Value='<%# Eval("CompGL") %>' />
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
                                                                                                                                    <telerik:GridTemplateColumn HeaderText="Company Exp GL" HeaderStyle-Width="150" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="MatPart" ItemStyle-CssClass="MatPart" HeaderStyle-HorizontalAlign="Center" DataField="CompGLE" UniqueName="CompGLE" SortExpression="CompGLE">
																								                                        <ItemTemplate>
																									                                        <asp:Label ID="lbldeductioncatecomapnyexpgl" runat="server" Text='<%# Eval("CompGLEName") %>' Width="125" CssClass="left-align"></asp:Label>
																									                                        <asp:HiddenField ID="hdndeductioncatecomapnyexpgl" runat="server" Value='<%# Eval("CompGLE") %>' />
																								                                        </ItemTemplate>
																							                                        </telerik:GridTemplateColumn>
																							                                        
																							                                        
																						                                        </Columns>
																					                                        </MasterTableView>
																				                                        </telerik:RadGrid>
																			                                        
																		                                        </div>--%>

                                                                                                                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_gvWageDedPayRate" runat="server">
                                                                        </telerik:RadAjaxLoadingPanel>
                                                                        <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
                                                                            <script type="text/javascript">
                                                                                function pageLoad() {
                                                                                    var grid = $find("<%= RadGridWageDeduction.ClientID %>");
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
                                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvWageDedPayRate" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                            <%--    <ContentTemplate>--%>
                                                                            <telerik:RadGrid ID="RadGridWageDeduction" runat="server" AutoGenerateColumns="False" FilterType="CheckList"
                                                                                CssClass="gvWagePayRate table table-bordered table-striped table-condensed flip-content" margin-bottom="0px"
                                                                                AllowSorting="true"
                                                                                ShowFooter="true"
                                                                                OnRowCommand="RadGridWageDeduction_RowCommand"
                                                                                OnItemDataBound="RadGridWageDeduction_ItemDataBound"
                                                                                EnableViewState="true"
                                                                                OnNeedDataSource="RadGridWageDeduction_NeedDataSource">
                                                                                <AlternatingItemStyle CssClass="oddrowcolor" />
                                                                                <ActiveItemStyle CssClass="evenrowcolor" />
                                                                                <FooterStyle CssClass="footer" />
                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                </ClientSettings>
                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="Ded">
                                                                                    
                                                                                    <Columns>
                                                                                        <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                                                                            <HeaderTemplate>
                                                                                                <asp:ImageButton ID="imgBtnWageDDelete" runat="server" CausesValidation="false" OnClick="imgBtnWageDDelete_Click"
                                                                                                    OnClientClick="return CheckDeleteDed('RadGridWageDeduction.ClientID');"
                                                                                                    ImageUrl="images/menu_delete.png" Width="13px" />
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chkSelect1" runat="server" />
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:LinkButton ID="imgBtnWageDAdd" runat="server" CommandName="AddWage" CausesValidation="False"
                                                                                                    CommandArgument="<%# ((GridItem) Container).ItemIndex %>"
                                                                                                    Width="30" OnClientClick="itemJSOND();" OnClick="imgBtnWageDAdd_Click" Text="Add"><i class="fa fa-plus-circle iconAdd_gvWagePayRate" ></i></asp:LinkButton>
                                                                                            </FooterTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="No." SortExpression="id" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblNum1" runat="server" Text='<%# ((GridItem) Container).ItemIndex + 1 %>'></asp:Label>
                                                                                                <asp:HiddenField ID="hdndCompGLE" runat="server" Value='<%# Eval("CompGLE") %>' />
                                                                                                <asp:HiddenField ID="hdndByW" runat="server" Value='<%# Eval("ByW") %>' />
                                                                                                <asp:HiddenField ID="hdndEmpGL" runat="server" Value='<%# Eval("EmpGL") %>' />
                                                                                                <asp:HiddenField ID="hdndEmpGLName" runat="server" Value='<%# Eval("EmpGLName") %>' />
                                                                                                <asp:HiddenField ID="hdndCompGL" runat="server" Value='<%# Eval("CompGL") %>' />
                                                                                                <asp:HiddenField ID="hdndCompGLName" runat="server" Value='<%# Eval("CompGLName") %>' />
                                                                                                <asp:HiddenField ID="hdndCompGLEName" runat="server" Value='<%# Eval("CompGLEName") %>' />
                                                                                                <asp:HiddenField ID="hdndInUse" runat="server" Value='<%# Eval("InUse") %>' />
                                                                                                <asp:HiddenField ID="hdndYTD" runat="server" Value='<%# Eval("YTD") %>' />
                                                                                                <asp:HiddenField ID="hdndYTDC" runat="server" Value='<%# Eval("YTDC") %>' />
                                                                                                <asp:HiddenField ID="hdndfdesc" runat="server" Value='<%# Eval("fdesc") %>' />
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Deduction" SortExpression="Ded" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList ID="ddlWageD" runat="server" DataValueField="Ded" DataTextField="fDesc" CssClass="browser-default"
                                                                                                    AutoPostBack="true" DataSource='<%#dtWageDeduction%>' Width="100%" OnSelectedIndexChanged="ddlWageD_SelectedIndexChanged"
                                                                                                    SelectedValue='<%# Eval("Ded") == DBNull.Value ? "0" : Eval("Ded") %>'>
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="BasedOn" SortExpression="BasedOn" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList ID="ddlBasedOnD" runat="server" DataValueField="Wage" DataTextField="fDesc" CssClass="browser-default"
                                                                                                    Width="100%" SelectedValue='<%# Eval("BasedOn") == DBNull.Value ? "0" : Eval("BasedOn") %>'>
                                                                                                     <asp:ListItem Text="FIT Wages" Value="0"></asp:ListItem>
                                                                                                     <asp:ListItem Text="FICA Wages" Value="1"></asp:ListItem>
                                                                                                     <asp:ListItem Text="MEDI Wages" Value="2"></asp:ListItem>
                                                                                                     <asp:ListItem Text="FUTA Wages" Value="3"></asp:ListItem>
                                                                                                     <asp:ListItem Text="SIT Wages" Value="4"></asp:ListItem>
                                                                                                     <asp:ListItem Text="Vacation Wages" Value="5"></asp:ListItem>
                                                                                                     <asp:ListItem Text="Worker's Comp Wages" Value="6"></asp:ListItem>
                                                                                                     <asp:ListItem Text="Union Wages" Value="7"></asp:ListItem>
                                                                                                     <asp:ListItem Text="Flat Amount" Value="8"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="AccuredOn" SortExpression="AccuredOn" HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList ID="ddlAccuredOnD" runat="server" DataValueField="Wage" DataTextField="fDesc" CssClass="browser-default"
                                                                                                     Width="100%" SelectedValue='<%# Eval("AccruedOn") == DBNull.Value ? "0" : Eval("AccruedOn") %>'>
                                                                                                     <asp:ListItem Text="Number of Hours" Value="0"></asp:ListItem>
                                                                                                     <asp:ListItem Text="Dollar Amount" Value="1"></asp:ListItem>
                                                                                                     <asp:ListItem Text="Flat Amount" Value="2"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Employee Rate" SortExpression="EmpRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="EqualTo" DataField="EmpRate" UniqueName="EmpRate"
                                                                                            HeaderStyle-CssClass="PayRate" >
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtEmpRateD" runat="server" Text='<%#Eval("EmpRate","{0:0.00}") %>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>                                                                                                
                                                                                                <%--<asp:Label ID="txtReg" runat="server" Text='<%#Eval("Reg","{0:0.00}") %>' Width="100%" ></asp:Label>--%>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Employee Max Rate" SortExpression="EmpTop" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="EqualTo" DataField="EmpTop" UniqueName="EmpTop"
                                                                                            HeaderStyle-CssClass="PayRate" >
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtEmpMaxRateD" runat="server" Text='<%#Eval("EmpTop","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <%--<telerik:GridTemplateColumn HeaderText="Employee GL" SortExpression="NT" ColumnGroupName="PayRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="5" CurrentFilterFunction="EqualTo" DataField="NT" UniqueName="NT"
                                                                                            HeaderStyle-CssClass="PayRate" ItemStyle-CssClass="PayRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtNt" runat="server" Text='<%#Eval("NT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content"
                                                                                                    onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>--%>
                                                                                        <telerik:GridTemplateColumn HeaderText="Company Rate" SortExpression="CompRate"  HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="EqualTo" DataField="CompRate" UniqueName="CompRate"
                                                                                            HeaderStyle-CssClass="PayRate" >
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCompanyRateD" runat="server" Text='<%#Eval("CompRate","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Company Max Rate" SortExpression="CompTop"  HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="EqualTo" DataField="CompTop" UniqueName="CompTop"
                                                                                            HeaderStyle-CssClass="PayRate" >
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCompanyMaxRateD" runat="server" Text='<%#Eval("CompTop","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <%--<telerik:GridTemplateColumn HeaderText="Company GL" SortExpression="CReg" ColumnGroupName="BurdenRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="5" CurrentFilterFunction="EqualTo" DataField="CReg" UniqueName="CReg"
                                                                                            HeaderStyle-CssClass="BurdenRate" ItemStyle-CssClass="BurdenRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCReg" runat="server" Text='<%#Eval("CReg","{0:0.00}") %>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>
                                                                                        <telerik:GridTemplateColumn HeaderText="Company Exp GL" SortExpression="COT" ColumnGroupName="BurdenRate" HeaderStyle-HorizontalAlign="Center"
                                                                                            AutoPostBackOnFilter="true" ShowFilterIcon="false" FilterDelay="5" CurrentFilterFunction="EqualTo" DataField="COT" UniqueName="COT"
                                                                                            HeaderStyle-CssClass="BurdenRate" ItemStyle-CssClass="BurdenRate">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtCOt" runat="server" Text='<%#Eval("COT","{0:0.00}")%>' Width="100%" onkeypress="return isDecimalKey(this,event)"
                                                                                                    CssClass="form-control rate-content" onchange="showDecimalVal(this)" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </telerik:GridTemplateColumn>--%>
                                                                                        
                                                                                    </Columns>
                                                                                </MasterTableView>
                                                                                <SelectedItemStyle CssClass="selectedrowcolor" />
                                                                                <FilterMenu CssClass="RadFilterMenu_CheckList">
                                                                                </FilterMenu>
                                                                            </telerik:RadGrid>
                                                                            <%--</ContentTemplate>--%>
                                                                        </telerik:RadAjaxPanel>



																	                                        </div>
																                                        </div>
															                                        </div>
														                                        </div>
                                                                                            </div>

















                                                                                   
                                                                                        
                                                                                </div>





                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            
                        </li>



                        <li>
                            <div id="accrdMiscCustom" class="collapsible-header accrd accordian-text-custom "><i class="mdi-social-poll"></i>Misc/Custom</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    
                                                    
                                                    

                                                                                    <div class="form-section4">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtVacationRate">Vacation Rate</label>
                                                                                                <asp:TextBox ID="txtVacationRate" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtSickRate">Sick Rate / Hour</label>
                                                                                                <asp:TextBox ID="txtSickRate" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtBenifits">Benifits/hr.</label>
                                                                                                <asp:TextBox ID="txtBenifits" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtCustom2">Custom2</label>
                                                                                                <asp:TextBox ID="txtCustom2" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section4">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <%--<label for="txtBasedOn">Based On</label>
                                                                                                <asp:TextBox ID="txtBasedOn" runat="server" onkeypress = "return numericOnly(this);"/>--%>
                                                                                                <label class="drpdwn-label">Based On</label>
                                                                                                <asp:DropDownList ID="ddlBasedOnmisc" runat="server" CssClass="browser-default">
                                                                                                    <asp:ListItem Value="0">Wages</asp:ListItem>
                                                                                                    <asp:ListItem Value="1">Hours</asp:ListItem>                                                                                                    
                                                                                                </asp:DropDownList>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtAccuredSickHours">Accured Sick Hours</label>
                                                                                                <asp:TextBox ID="txtAccuredSickHours" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtPDASerial">PDA Serial #</label>
                                                                                                <asp:TextBox ID="txtPDASerial" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtCustom3">Custom3</label>
                                                                                                <asp:TextBox ID="txtCustom3" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section4">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtAccuredVacation">Accured Vacation</label>
                                                                                                <asp:TextBox ID="txtAccuredVacation" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtAvailableSickHours">Available Sick Hours</label>
                                                                                                <asp:TextBox ID="txtAvailableSickHours" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtLastPaid">Last Paid</label>
                                                                                                <asp:TextBox ID="txtLastPaid" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtCustom4">Custom4</label>
                                                                                                <asp:TextBox ID="txtCustom4" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-section4">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtAvailableVacation">Available Vacation</label>
                                                                                                <asp:TextBox ID="txtAvailableVacation" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtSickUnits"># Sick Units</label>
                                                                                                <asp:TextBox ID="txtSickUnits" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtPaidMisc"># Paid</label>
                                                                                                <asp:TextBox ID="txtPaidMisc" runat="server" onkeypress = "return numericOnly(this);"/>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <label for="txtCustom5">Custom5</label>
                                                                                                <asp:TextBox ID="txtCustom5" runat="server" />
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    
                                                                                    <div class="form-section3">
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                                <div class="buttonContainer">
                                                                                                    <div class="btnlinks">
                                                                                                        <asp:LinkButton CssClass="icon-save" ID="btnViewPDF" runat="server" >View PDF</asp:LinkButton>
                                                                                                    </div>
                                                                                                    <div class="btnlinks">
                                                                                                        <asp:LinkButton CssClass="icon-save" ID="btnSetPDF" runat="server" >Set PDF</asp:LinkButton>
                                                                                                    </div>
                                                                                                    <div class="btnlinks">
                                                                                                        <asp:LinkButton CssClass="icon-save" ID="btnRemoveBiofromTech" runat="server" >Remove Bio from Tech</asp:LinkButton>
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                       
                                                                                    </div>
                                                                                      
                                                                                </div>





                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            
                        </li>
                        <li>
                            <div id="accrdDirectDeposit" class="collapsible-header accrd accordian-text-custom "><i class="mdi-social-poll"></i>Direct Deposit</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    <%--<div class="section-ttle">General</div>--%>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <%--<asp:TextBox ID="TextBox1" runat="server" MaxLength="200"></asp:TextBox>
                                                                <label for="txtFirst">First</label>--%>
                                                                <label class="drpdwn-label">Direct Deposit</label>
                                                                <asp:DropDownList ID="ddlDirectDeposit" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                               <label class="drpdwn-label">Split Type</label>
                                                                <asp:DropDownList ID="ddlSplitType" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Value="0">No Split</asp:ListItem>
                                                                    <asp:ListItem Value="1">Percentage</asp:ListItem>
                                                                    <asp:ListItem Value="2">Flat Amount</asp:ListItem>
                                                                </asp:DropDownList>
                                                                
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                               
                                                                
                                                            </div>
                                                        </div>
                                                        

                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="section-ttle">Direct Deposit Bank Account 1</div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <label class="drpdwn-label">Account Type</label>
                                                                <asp:DropDownList ID="ddlAccountType1" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Value="0">Checking</asp:ListItem>
                                                                    <asp:ListItem Value="1">Saving</asp:ListItem>
                                                                    <asp:ListItem Value="2">Pre-Note</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtBankRoute1" runat="server" ></asp:TextBox>
                                                                <label for="txtBankRoute1">Bank Route #</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtBankAcct1" runat="server" ></asp:TextBox>
                                                                <label for="txtBankAcct1">Bank Acct #</label>
                                                            </div>
                                                        </div>
                                                        
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="section-ttle">Direct Deposit Bank Account 2</div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                              <label class="drpdwn-label">Account Type</label>
                                                                <asp:DropDownList ID="ddlAccountType2" runat="server" CssClass="browser-default">
                                                                    <asp:ListItem Value="0">Checking</asp:ListItem>
                                                                    <asp:ListItem Value="1">Saving</asp:ListItem>
                                                                    <asp:ListItem Value="2">Pre-Note</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>


                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtBankRoute2" runat="server" ></asp:TextBox>
                                                                <label for="txtBankRoute2">Bank Route #</label>
                                                            </div>
                                                        </div>
                                                        
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtBankAcct2" runat="server" ></asp:TextBox>
                                                                <label for="txtBankAcct2">Bank Acct #</label>
                                                            </div>
                                                        </div>
                                                        
                                                    </div>
                                                    <%--<div class="section-ttle">General</div>--%>
                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div id="accrdYTD" class="collapsible-header accrd accordian-text-custom "><i class="mdi-social-poll"></i>YTD</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    
                                                    
                                                    

                                                                                    <div class="form-section2">
                                                                                        <div class="section-ttle">Revenue</div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                               
                                                                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Revenue"  PageSize="50"  FilterType="CheckList" 
                                                                                                
                                                                                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                                                <CommandItemStyle />
                                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                                </ClientSettings>
                                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                                                    <Columns>
                                                                                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                                                                                                </telerik:GridClientSelectColumn>
                                                                                                                <telerik:GridTemplateColumn UniqueName="lblWageId" FilterDelay="5" DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false"
                                                                                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblIDdedu1" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="fdesc" HeaderText="Description" UniqueName="fdesc" SortExpression="fdesc"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:HyperLink ID="lblWageFdescdedu1" runat="server" Text='<%# Eval("fdesc") %>'></asp:HyperLink>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                
                                                                                                        
                                                                                                    </Columns>
                                                                                                </MasterTableView>
                                                                                            </telerik:RadGrid>
                                                                                            </div>
                                                                                        </div>
                                                                                       

                                                                                        
                                                                                    </div>
                                                                                    <div class="form-section3-blank">
                                                                                        <div class="row">
                                                                                            &nbsp;
                                                                                        </div>
                                                                                    </div>
                                                                                    
                                                                                    <div class="form-section2">
                                                                                        <div class="section-ttle">Deductions</div>
                                                                                        <div class="input-field col s12">
                                                                                            <div class="row">
                                                                                               
                                                                                                <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Deductions"  PageSize="50"  FilterType="CheckList" 
                                                                                                
                                                                                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                                                <CommandItemStyle />
                                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                                </ClientSettings>
                                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                                                    <Columns>
                                                                                                        <telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                                                                                                </telerik:GridClientSelectColumn>
                                                                                                                <telerik:GridTemplateColumn UniqueName="lblWageId" FilterDelay="5" DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false"
                                                                                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblIDdedu1" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="fdesc" HeaderText="Description" UniqueName="fdesc" SortExpression="fdesc"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:HyperLink ID="lblWageFdescdedu1" runat="server" Text='<%# Eval("fdesc") %>'></asp:HyperLink>
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
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>
                                        <div class="cf"></div>
                                    </div>
                                </div>
                            
                        </li>
                        <li>
                            <div id="accrdBenchmark" class="collapsible-header accrd accordian-text-custom "><i class="mdi-social-poll"></i>Banchmark/Remark</div>
                            <div class="collapsible-body">
                                <div class="form-content-wrap">
                                    <div class="form-content-pd">
                                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    <%--<div class="section-ttle">General</div>--%>
                                                    <div class="form-section3">
                                                        <div class="section-ttle">Benchmark for Tech Efficiency Report</div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtBillRate" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                <label for="txtBillRate">Bill Rate</label>
                                                                
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                               <asp:TextBox ID="txtSales" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                <label for="txtSales">Sales</label>
                                                                
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                               <asp:TextBox ID="txtInvoiceAverage" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                <label for="txtInvoiceAverage">Invoice Average</label>
                                                                
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                               <asp:TextBox ID="txtRemark" runat="server" ></asp:TextBox>
                                                                <label for="txtRemark">Remark</label>
                                                                
                                                            </div>
                                                        </div>

                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="section-ttle">        &nbsp;          </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtClosingPercentage" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                <label for="txtClosingPercentage">Closing %</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtBillableEfficiency" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                <label for="txtBillableEfficiency">Billable Efficiency</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtProdcutionEfficiency" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                <label for="txtProdcutionEfficiency">Prodcution Efficiency</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtAverageTasks" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                <label for="txtAverageTasks">Average Tasks</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        <div class="section-ttle">Ticket Custome Fields 6-10</div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                              <asp:TextBox ID="txtCustom6" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                <label for="txtCustom6">ShutDown</label>
                                                            </div>
                                                        </div>


                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCustom7" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                <label for="txtCustom">Custom7</label>
                                                            </div>
                                                        </div>
                                                        
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCustom8" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                <label for="txtCustom8">Custom8</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCustom9" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                <label for="txtCustom9">Custom9</label>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtCustom10" runat="server" onkeypress = "return numericOnly(this);"></asp:TextBox>
                                                                <label for="txtCustom10">Custom10</label>
                                                            </div>
                                                        </div>
                                                        
                                                    </div>
                                                    <%--<div class="section-ttle">General</div>--%>
                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
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







   


      
                    
                        


                        <div style="clear: both;"></div>
                        <footer style="float: left; padding-left: 0 !important;">
                            <div class="btnlinks">
                                <asp:HiddenField ID="hdnEmpID" runat="server" />
                                <asp:HiddenField ID="hdnAddEdit" runat="server" />
                                <asp:HiddenField ID="hdnFlage" runat="server" />
                                <asp:HiddenField ID="hdnSSN" runat="server" />
                            </div>
                        </footer>
                    
                

     

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(function () {
            $("[id*=txtPhone]").mask("(999) 999-9999? Ext 99999");
            $("[id*=txtPhone]").bind('paste', function () { $(this).val(''); });
            $("[id*=txtCellular]").mask("(999) 999-9999");
            $("[id*=txtFax]").mask("(999) 999-9999");
            $("[id*=txtContactno]").mask("(999) 999-9999");
            
        });
    </script>
     <script type="text/javascript">
         $(document).ready(function () {
             $(function () {
                 $("#<%= txtAddress.ClientID %>").geocomplete({
                    map: false,
                    details: "#divmain",
                    types: ["geocode", "establishment"],
                    address_components:"#<%= txtAddress.ClientID %>",
                    city: "#<%= txtCity.ClientID %>",
                    state: "#<%= ddlState.ClientID %>",
                    zip: "#<%= txtZip.ClientID %>",
                    lat: "#<%= txtLatitude.ClientID %>",
                    lng: "#<%= txtLongitude.ClientID %>"
                }).bind("geocode:result", function (event, result) {
                    var countryCode = "", city = "", cityAlt = "", getCountry = "";

                    for (var i = 0; i < result.address_components.length; i++) {

                        var addr = result.address_components[i];

                        if (addr.types[0] == 'country')
                            getCountry = addr.short_name;
                        if (addr.types[0] == 'locality')
                            city = addr.long_name;
                        if (addr.types[0] == 'administrative_area_level_1')
                            cityAlt = addr.short_name;
                        if (addr.types[0] == 'postal_code')
                            countryCode = addr.long_name;
                    }
                    if (cityAlt.length > 2)
                        for (var i = 0; i < result.address_components.length; i++) {
                            var addr = result.address_components[i];
                            if (addr.types[0] == 'administrative_area_level_2')
                                cityAlt = addr.short_name;
                        }

                    $("#<%=ddlCountry.ClientID%>").val(getCountry);
                    $("#<%=ddlState.ClientID%>").val(cityAlt);
                    $("#<%=txtZip.ClientID%>").val(countryCode);
                    $("#<%=txtCity.ClientID%>").val(city);

                    Materialize.updateTextFields();

                }).bind("set");



                initialize();
            });

            $("#mapLink").click(function () {
                $("#map").toggle();
                initialize();
            });


        });


         function initialize() {
             var address = new google.maps.LatLng(document.getElementById('<%= txtLatitude.ClientID %>').value, document.getElementById('<%= txtLongitude.ClientID %>').value);
             var marker;
             var map;
             var mapOptions = {
                 zoom: 13,
                 mapTypeId: google.maps.MapTypeId.ROADMAP,
                 center: address
             };

             map = new google.maps.Map(document.getElementById('map'),
                 mapOptions);

             marker = new google.maps.Marker({
                 map: map,
                 draggable: false,
                 position: address
             });
         }
    </script>
      
</asp:Content>