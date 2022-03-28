<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddInventoryAdjustment" Codebehind="AddInventoryAdjustment.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
  <%--<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">--%>
     <style>
        /*input[readonly="readonly"] {
            background-color: #fff !important;            
        }*/
        textarea.materialize-textarea {
            padding: 1.2rem 0;         
        }        
        a[disabled=disabled] i.fa{
            color:gray;
        }    
        input[type=text][readonly=readonly]{
            background-color:#ccc
        }
        input[type=text], input[type=password], input[type=email], input[type=url], input[type=time], input[type=date], input[type=datetime-local], input[type=tel], input[type=number], input[type=search], textarea.materialize-textarea
        {
            margin: 0 0 4px 0;
           height: 1.5rem;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            //$('#ui-active-menuitem').click(function () {
            //    $('.ui-menu-item').removeClass('ui-autocomplete-loading');
            //})
            InitializeGrids('<%=gvGLItems.ClientID%>');
            var query = "";
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
        function InitializeGrids(Gridview) {

            var rowone = $("#" + Gridview).find('tr').eq(1);
            $("input", rowone).each(function () {
                $(this).blur();
            });
        }
        function itemJSON() {
            var rawData = $('#<%=gvGLItems.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            $('#<%=hdnItemJSON.ClientID%>').val(formData);
        }
        ///////////// Custom validator function for vendor auto search  ////////////////////

        function clearPhase(txt) {
            if ($(txt).val() == '') {
                var hdnPID = document.getElementById(txt.id.replace('txtGvPhase', 'hdnPID'));
                $(hdnPID).val('0');
            }
        }

        function makeReadonly(txt) {
            $("#" + txt.id).prop('readonly', true);
        }
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
            ResetBom();
            //window.parent.document.getElementById('btnCancel').click();
        }
        function dtaa() {
            this.term = '';
            this.prefixText = null;
            this.con = null;

        }       
       

        function addAutocomplete() {
            $("[id*=txtGvItem]").autocomplete({

                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.term = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetInventory",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load item details");
                        }
                    });
                },
                messages: {
                    noResults: '',
                    results: function () { }
                },
                select: function (event, ui) {

                    var txtGvItem = this.id;
                    //var txtGvActualOnHand = document.getElementById(txtGvItem.replace('txtGvItem', 'txtGvActualOnHand'));
                    //var txtGvActualValue = document.getElementById(txtGvItem.replace('txtGvItem', 'txtGvActualValue'));
                    //var txtLastCost = document.getElementById(txtGvItem.replace('txtGvItem', 'txtLastCost'));
                    var txtSellPrice = document.getElementById(txtGvItem.replace('txtGvItem', 'txtSellPrice'));
                    var hdnInvID = document.getElementById(txtGvItem.replace('txtGvItem', 'hdnInvID'));

                    var txtGvAcccount = document.getElementById(txtGvItem.replace('txtGvItem', 'txtGvAcccount'));
                    var hdnAccount = document.getElementById(txtGvItem.replace('txtGvItem', 'hdnAccount'));
                    

                    //alert(hdnInvID);
                    // $(hdnInvID).val(ui.item.ID);
                    var jobStr = ui.item.ID + ", " + ui.item.Name + ", " + ui.item.fDesc;
                    $(this).val(jobStr);
                    $(txtGvItem).val(jobStr);
                    //$(txtGvActualOnHand).val(ui.item.Hand);
                    //$(txtGvActualValue).val(ui.item.Balance);
                    //$(txtLastCost).val("0.00");
                    $(txtSellPrice).val(ui.item.SPrice);
                    $(hdnInvID).val(ui.item.ID);
                    
                    var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                    var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                    if (document.getElementById('<%=hdnIsInvTrackingOn.ClientID%>').value == 'False') {
                        $(txtGvAcccount).val(InvDefaultAcctName);
                        $(hdnAccount).val(InvDefaultAcctID);
                        $(txtGvAcccount).prop("disabled", true); 
                        $(txtGvAcccount).attr('readonly', 'true'); // mark it as read only
                        $(txtGvAcccount).attr('autocomplete', 'off');
                        $(txtGvAcccount).css('background-color', '#DEDEDE'); // change the background color
                    }
                    

                    return false;
                },

                change: function (event, ui) {
                    var txtGvJob = this.id;
                    // var hdnInvID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnInvID'));
                    var strJob = document.getElementById(txtGvJob).value;

                    //if (strJob == '') {
                    //    $(hdnInvID).val('')
                    //}
                },
                focus: function (event, ui) {
                    $(this).val(ui.fDesc);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            });
            $.each($(".Itemsearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ID;
                    var result_item = item.Name;
                    var result_desc = item.fDesc;
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
                            .data("ui-autocomplete-item", item)
                            .append("<a>" + result_item + "</a>")
                            .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                            .data("ui-autocomplete-item", item)
                            //.append("<a><b> Project: </b> " + result_value + ", " + result_item + ", <span style='color:Gray;'><b> Loc: </b>" + result_desc + "</span></a>")
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
            });

                //txtGvAcctName
            $("[id*=txtGvAcccount]").autocomplete({

                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    dtaaa.con = "";
                    var str = request.term;
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
                            alert("Due to unexpected errors we were unable to load account name");
                        }
                    });
                },
                messages: {
                    noResults: '',
                    results: function () { }
                },
                select: function (event, ui) {

                    var txtGvAcccount = this.id;
                    var hdnAccount = document.getElementById(txtGvAcccount.replace('txtGvAcccount', 'hdnAccount'));

                    //$(txtGvAcctName).val(ui.item.label);
                    //$(hdnAcctID).val(ui.item.value);
                    //$(this).val(ui.item.acct);

                    var Str = ui.item.acct + ", " + ui.item.label;

                    $(this).val(Str);
                    $(txtGvAcccount).val(Str);
                    $(hdnAccount).val(ui.item.value);


                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.acct);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            });

            $.each($(".Accountsearchinput"), function (index, item) {
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
            });

            //txtGvWarehouse
            $("[id*=txtGvWarehouse]").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;

                    var str = request.term;

                    var txtGvWarehouse_GetID = $(this.element).attr("id");
                    var hdnInvID = document.getElementById(txtGvWarehouse_GetID.replace('txtGvWarehouse', 'hdnInvID'));
                    var ID = $(hdnInvID).val();


                    dtaaa.InvID = ID;
                    dtaaa.isShowAll = "yes";
                    dtaaa.con = "";
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
                messages: {
                    noResults: '',
                    results: function() {}
                },
                select: function (event, ui) {

                    var txtGvWarehouse = this.id;
                    var hdnWarehouse = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnWarehouse'));
                    var hdnInvID = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnInvID'));
                    var txtGvCompany = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'txtGvCompany'));
                    var hdnCompanyEN = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnCompanyEN'));

                        
                    var Str = ui.item.WarehouseID + ", " + ui.item.WarehouseName;

                    $(this).val(Str);
                    $(txtGvWarehouse).val(Str);
                    $(hdnWarehouse).val(ui.item.WarehouseID);
                    $(txtGvCompany).val(ui.item.Company);
                    $(hdnCompanyEN).val(ui.item.EN);

                    var locationID = 0;
                    var warehouseID = $(hdnWarehouse).val();
                    var invID = $(hdnInvID).val();

                    //Ajax Call
                    var dtaaa = new dtaa();
                    dtaaa.InvID = invID;
                    dtaaa.WarehouseID = warehouseID;
                    dtaaa.locationID = locationID;


                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetLocationOnHand",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {

                            var ui = $.parseJSON(data.d);

                            var txtGvActualOnHand = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'txtGvActualOnHand'));
                            var txtGvActualValue = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'txtGvActualValue'));
                            var txtLastCost = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'txtLastCost'));
                            $(txtGvActualOnHand).val(ui[0].Hand);
                            $(txtGvActualValue).val(ui[0].Balance);
                            $(txtLastCost).val("0.00");

                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load account name");
                        }
                    });

                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.WarehouseID);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".Warehousesearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ID;
                    var result_item = item.WarehouseName;
                    var result_desc = item.WarehouseID;
                    var result_Company = item.Company;
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
                        .data("ui-autocomplete-item", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                        .data("ui-autocomplete-item", item)
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

                    dtaaa.WarehouseID = ID;
                    dtaaa.con = "";
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
                messages: {
                    noResults: '',
                    results: function () { }
                },
                select: function (event, ui) {

                    var txtGvWarehouseLocation = this.id;
                    var hdnWarehouseLocationID = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnWarehouseLocationID'));
                    var hdnInvID = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnInvID'));
                    var hdnWarehouse = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'hdnWarehouse'));

                    var Str = ui.item.ID + ", " + ui.item.Name;
                    $(this).val(Str);
                    $(txtGvWarehouseLocation).val(Str);
                    $(hdnWarehouseLocationID).val(ui.item.ID);

                    var locationID = $(hdnWarehouseLocationID).val();
                    var warehouseID = $(hdnWarehouse).val();
                    var invID = $(hdnInvID).val();

                    //Ajax Call
                    var dtaaa = new dtaa();
                    dtaaa.InvID = invID;
                    dtaaa.WarehouseID = warehouseID;
                    dtaaa.locationID = locationID;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetLocationOnHand",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {

                            var ui = $.parseJSON(data.d);

                            var txtGvActualOnHand = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'txtGvActualOnHand'));
                            var txtGvActualValue = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'txtGvActualValue'));
                            var txtLastCost = document.getElementById(txtGvWarehouseLocation.replace('txtGvWarehouseLocation', 'txtLastCost'));
                            $(txtGvActualOnHand).val(ui[0].Hand);
                            $(txtGvActualValue).val(ui[0].Balance);
                            $(txtLastCost).val("0.00");

                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load account name");
                        }
                    });
                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.ID);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            });
            $.each($(".WarehouseLocationsearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ID;
                    var result_item = item.Name;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });


                    if (result_value == 0) {
                        return $("<li></li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a style='color:blue;'>" + result_item + "</a>")
                        .appendTo(ul);
                    }
                };
            });
        }
    </script>
    <script type="text/javascript">

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

        /////////////////// To calculate Total and to make Gridview Amount Value to 2 decimal ////////////
        function CalTotalVal(obj) {


            var txt = obj.id;

            var txtGvQuan;
            var txtGvPrice;
            var txtGvAmount;

            if (txt.indexOf("Quan") >= 0) {
                txtGvQuan = document.getElementById(txt);
                txtGvPrice = document.getElementById(txt.replace('txtGvQuan', 'txtGvPrice'));
                txtGvAmount = document.getElementById(txt.replace('txtGvQuan', 'txtGvAmount'));
            }
            else if (txt.indexOf("Price") >= 0) {
                txtGvPrice = document.getElementById(txt);
                txtGvQuan = document.getElementById(txt.replace('txtGvPrice', 'txtGvQuan'));
                txtGvAmount = document.getElementById(txt.replace('txtGvPrice', 'txtGvAmount'));
            }
            else if (txt.indexOf("Amount") >= 0) {
                txtGvPrice = document.getElementById(txt.replace('txtGvAmount', 'txtGvPrice'));
                txtGvQuan = document.getElementById(txt.replace('txtGvAmount', 'txtGvQuan'));
                txtGvAmount = document.getElementById(txt);
            }

            if (!jQuery.trim($(txtGvQuan).val()) == '') {
                if (isNaN(parseFloat($(txtGvQuan).val()))) {
                    $(txtGvQuan).val('0.00');
                }
            }

            if (!jQuery.trim($(txtGvPrice).val()) == '') {
                if (isNaN(parseFloat($(txtGvPrice).val()))) {
                    $(txtGvPrice).val('0.00');
                }
            }

            if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
                if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
                    var valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
                    $(txtGvAmount).val(valAmount.toFixed(2));
                }
            }
            CalculateTotalAmt();

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }
        function CalculateTotal(obj) {

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
                CalTotalVal(obj);
            }
            else {
                CalTotalVal(obj);
            }

            CalculateTotalAmt();
        }
        function CalculateTotalAmt() {

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


        }

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

        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }

        //////////////////////To make row's textbox visible///////////////////////////////////////////
        function VisibleRow(row, txt, gridview, event) {  //

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

            var txt = document.getElementById(txt);
            $(txt).removeClass("texttransparent");
            $(txt).addClass("non-trans");

        }

        function CalculateHold(obj) {
            var txt = obj.id;

            var txtActualOnHand;
            var txtNewOnHand;
            var txtAdj;
            var chkadjust;

            var txtNewValue;
            var txtGvActualValue;
            var txtAdjAmount;

            if (txt.indexOf("NewOnHand") >= 0) {

                txtNewOnHand = document.getElementById(txt);
                txtActualOnHand = document.getElementById(txt.replace('txtNewOnHand', 'txtGvActualOnHand'));
                txtAdj = document.getElementById(txt.replace('txtNewOnHand', 'txtAdj'));
                chkadjust = document.getElementById(txt.replace('txtNewOnHand', 'chkadjust'));

                txtGvActualValue = document.getElementById(txt.replace('txtNewOnHand', 'txtGvActualValue'));
                txtNewValue = document.getElementById(txt.replace('txtNewOnHand', 'txtNewValue'));
                txtAdjAmount = document.getElementById(txt.replace('txtNewOnHand', 'txtAdjAmount'));

                var newonhandqnty = isNaN(parseFloat($(txtNewOnHand).val())) ? 0.00 : parseFloat($(txtNewOnHand).val());

                var oldonhandqnty = isNaN(parseFloat($(txtActualOnHand).val())) ? 0.00 : parseFloat($(txtActualOnHand).val());

                var adjustedonhandqnty = newonhandqnty - oldonhandqnty;
                var actualvalue = isNaN(parseFloat($(txtGvActualValue).val())) ? 0.00 : parseFloat($(txtGvActualValue).val());

                //if (adjustedonhandqnty < 0)
                //{
                //    $(txtAdj).attr("style", "border-color:red");
                //}

                $(txtAdj).val(adjustedonhandqnty);

                //    if ($(chkadjust).is(":checked")) {
                var unitprice = isNaN(parseFloat(actualvalue / oldonhandqnty)) ? 0.00 : parseFloat(actualvalue / oldonhandqnty);
                var newvalue = unitprice * newonhandqnty;

                $(txtNewValue).val(newvalue);
                var newonhandval = isNaN(parseFloat($(txtNewValue).val())) ? 0.00 : parseFloat($(txtNewValue).val());

                var adjustedholdval = newonhandval - actualvalue;
                $(txtAdjAmount).val(adjustedholdval);
            }


            if (txt.indexOf("NewValue") >= 0) {

                txtNewValue = document.getElementById(txt);
                txtGvActualValue = document.getElementById(txt.replace('txtNewValue', 'txtGvActualValue'));
                txtAdjAmount = document.getElementById(txt.replace('txtNewValue', 'txtAdjAmount'));
                txtNewOnHand = document.getElementById(txt.replace('txtNewValue', 'txtNewOnHand'));


                var newonhandval = $(txtNewValue).val();
                var oldonholdval = $(txtGvActualValue).val();
                var Newonholdqty = $(txtNewOnHand).val();
                var adjustedholdval = newonhandval - oldonholdval;
                // var adjustedholdval = newonhandval * Newonholdqty;               

                $(txtAdjAmount).val(adjustedholdval);

            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <div style="height: 65px !important;">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-action-settings-remote"></i>
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Inventory Adjustment</asp:Label>
                                        <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label>
                                    </div>
                                    <div class="btnlinks">
                                         <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" ToolTip="Save" TabIndex="38" OnClick="btnSubmit_Click">Save</asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <asp:Label runat="server" ID="lblRefNumber"  CssClass="editlabel"></asp:Label>

                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                                OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                                &nbsp;
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlSave" runat="server">
                                        <asp:Panel ID="pnlNext" runat="server">
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
    <div class="container">
        <div class="row">
             <div class="alert alert-warning" runat="server" id="divMessage" >
                            <button type="button" class="close" data-dismiss="alert">×</button>
                            These month/year period is closed out. You do not have permission to add/update this record.
                        </div>
             <div class="grid_container">
                <div class="form-section-row m-b-0">
                    
                     <asp:HiddenField ID="hdnBatch" runat="server" />
                        <asp:HiddenField ID="hdnTransID" runat="server" />
                        <asp:HiddenField ID="hdnStatus" runat="server" />
                        <asp:HiddenField ID="hdnItemJSON" runat="server" />
                        <asp:HiddenField ID="hdnTotal" runat="server" />
                        <asp:HiddenField ID="hdnRowField" runat="server" />
                    <asp:HiddenField runat="server" ID="hdnInvDefaultAcctID" Value="" />
                    <asp:HiddenField runat="server" ID="hdnInvDefaultAcctName" Value="" />
                    <asp:HiddenField runat="server" ID="hdnIsInvTrackingOn" Value="" />
                    <asp:UpdatePanel ID="updPnlGLItems" runat="server">
                        <ContentTemplate>

                            
                                <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                    <script type="text/javascript">
                                        function pageLoad() {
                                            var grid = $find("<%= gvGLItems.ClientID %>");
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
                                <telerik:RadAjaxPanel ID="RadAjaxPanel_AddInventoryAdjustment" runat="server" LoadingPanelID="RadAjaxLoadingPanel_AddInventoryAdjustment" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">

                                    <telerik:RadPersistenceManager ID="RadPersistence_AddInventoryAdjustment" runat="server">
                                        <PersistenceSettings>
                                            <telerik:PersistenceSetting ControlID="gvGLItems" />
                                        </PersistenceSettings>
                                    </telerik:RadPersistenceManager>

                                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                                        <script type="text/javascript">
                                            function pageLoad() {
                                                var grid = $find("<%= gvGLItems.ClientID %>");
                                                var columns = grid.get_masterTableView().get_columns();
                                                for (var i = 0; i < columns.length; i++) {
                                                    columns[i].resizeToFit(false, true);
                                                }
                                                addAutocomplete();

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

                                            }


                                        </script>
                                    </telerik:RadCodeBlock>
                                    <div class="RadGrid RadGrid_Material FormGrid">
                                        <telerik:RadGrid RenderMode="Auto" ID="gvGLItems" ShowFooter="True" PageSize="50"
                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="false" Width="100%" PagerStyle-AlwaysVisible="true"
                                            AllowCustomPaging="false" OnItemCommand="gvGLItems_ItemCommand" OnPreRender="gvGLItems_PreRender">
                                            <CommandItemStyle />
                                            <%--OnNeedDataSource="gvGLItems_NeedDataSource" OnPreRender="gvGLItems_PreRender" OnItemEvent="gvGLItems_ItemEvent">--%>

                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="True"></Selecting>
                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                
                                            </ClientSettings>
                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="false" ShowFooter="True">

                                                <ColumnGroups>
                                                    <telerik:GridColumnGroup HeaderText="Actual Level" Name="ActualLevel" HeaderStyle-HorizontalAlign="Center">
                                                    </telerik:GridColumnGroup>

                                                    <telerik:GridColumnGroup HeaderText="New Level" Name="NewLevel" HeaderStyle-HorizontalAlign="Center">
                                                    </telerik:GridColumnGroup>

                                                </ColumnGroups>
                                                <Columns>

                                                    <telerik:GridTemplateColumn DataField="fDate" SortExpression="fDate" AutoPostBackOnFilter="true"
                                                        HeaderText="Date" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAdjDate" runat="server" CssClass="datepicker_mom" Text='<%# Bind("fDate") %>'></asp:TextBox>
                                                        </ItemTemplate>

                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn DataField="ItemName" SortExpression="ItemName" AutoPostBackOnFilter="true"
                                                        HeaderText="Item" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvItem" runat="server" Text='<%# Bind("ItemName") %>' CssClass="texttransparent Itemsearchinput"></asp:TextBox>
                                                            <asp:HiddenField ID="hdnInvID" Value='<%# Bind("InvId") %>' runat="server" />
                                                            <asp:HiddenField ID="hdnIndex" Value='<%# ((GridItem) Container).RowIndex %>' runat="server" />
                                                            <%--<asp:HiddenField ID="hdnRowID" runat="server" Value='<%# Bind("RowID") %>'></asp:HiddenField>--%>
                                                            <asp:HiddenField ID="hdnAdjID" runat="server" Value='<%# Bind("AdjID") %>'></asp:HiddenField>
                                                            <asp:HiddenField ID="hdnTransID" runat="server" Value='<%# Bind("TransID") %>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn DataField="WarehouseName" SortExpression="WarehouseName" AutoPostBackOnFilter="true"
                                                        HeaderText="Warehouse" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvWarehouse" runat="server" CssClass="texttransparent Warehousesearchinput"
                                                                Text='<%# Bind("WarehouseName") %>' Width="100%" ></asp:TextBox>
                                                            <asp:HiddenField ID="hdnWarehouse" runat="server" Value='<%# Bind("WarehouseID") %>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn DataField="LocationName" SortExpression="LocationName" AutoPostBackOnFilter="true"
                                                        HeaderText="Location" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvWarehouseLocation" runat="server" Text='<%# Bind("LocationName") %>'
                                                                CssClass="texttransparent WarehouseLocationsearchinput" Width="100%"></asp:TextBox>
                                                            <asp:HiddenField ID="hdnWarehouseLocationID" runat="server" Value='<%# Bind("LocationID") %>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn DataField="Company" UniqueName="Company" SortExpression="Company" AutoPostBackOnFilter="true" 
                                                        HeaderText="Company" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvCompany" runat="server" Text='<%# Bind("Company") %>'></asp:TextBox>
                                                            <asp:HiddenField ID="hdnCompanyEN" runat="server" Value='<%# Bind("EN") %>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn DataField="Chart" SortExpression="Chart" AutoPostBackOnFilter="true" 
                                                        HeaderText="Account" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvAcccount" runat="server" Text='<%# Bind("Chart") %>'
                                                                CssClass="texttransparent Accountsearchinput" Width="100%" ></asp:TextBox>
                                                            <asp:HiddenField ID="hdnAccount" runat="server" Value='<%# Bind("ChartID") %>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" ColumnGroupName="ActualLevel"
                                                        HeaderText="On Hand" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvActualOnHand" runat="server" autocomplete="off" ReadOnly="true"
                                                                Width="100%"
                                                                onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" ColumnGroupName="ActualLevel"
                                                        HeaderText="Value" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGvActualValue" runat="server" ReadOnly="true"
                                                                Width="100%" ></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" ColumnGroupName="ActualLevel"
                                                        HeaderText="Last Cost" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtLastCost" runat="server" ReadOnly="true"
                                                                Width="100%" 
                                                                ></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" ColumnGroupName="ActualLevel"
                                                        HeaderText="Sell Price" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSellPrice" runat="server" ReadOnly="true" 
                                                                Width="100%" ></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" ColumnGroupName="NewLevel"

                                                        HeaderText="On Hand" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtNewOnHand" runat="server" Text='<%# Bind("Quantity") %>'
                                                                Width="100%" 
                                                                onkeypress="return isDecimalKey(this,event)" onchange="CalculateHold(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" ColumnGroupName="NewLevel"
                                                        HeaderText="Total Value" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtNewValue" runat="server" Text='<%# Bind("Amount") %>'
                                                                Width="100%" 
                                                                onkeypress="return isDecimalKey(this,event)" onchange="CalculateHold(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" ColumnGroupName="NewLevel"
                                                        HeaderText="Adj Qty" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAdj" runat="server" Text='<%# Bind("Quantity") %>' 
                                                                Width="100%" ></asp:TextBox>

                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" ColumnGroupName="NewLevel"
                                                        HeaderText="Adjust value to reflect changes" ShowFilterIcon="false" Visible="false">
                                                        <ItemTemplate>
                                                            <%-- <asp:TextBox ID="txtAdj" runat="server" CssClass="form-control" ReadOnly="true"
                                                                        Width="100%" ></asp:TextBox>--%>
                                                            <asp:CheckBox ID="chkadjust" runat="server" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>


                                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true"
                                                        HeaderText="Adj Amount" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAdjAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                     


                                                   <%-- <telerik:GridTemplateColumn SortExpression="delete" AutoPostBackOnFilter="true"
                                                        HeaderText="Action" ShowFilterIcon="false" HeaderStyle-Width="70">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibDelete" runat="server" CommandName="DeleteTransaction"
                                                                CommandArgument='<%# Container.DataSetIndex %>'
                                                                ImageUrl="~/images/glyphicons-17-bin.png" Width="13px" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>--%>

                                                </Columns>
                                            </MasterTableView>
                                            <FilterMenu CssClass="RadFilterMenu_CheckList">
                                            </FilterMenu>
                                        </telerik:RadGrid>
                                    </div>
                                </telerik:RadAjaxPanel>
                         
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAddNewLines" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                 
            </div>
            <div class="btnlinks">
                <asp:LinkButton ID="btnAddNewLines" runat="server" CausesValidation="false" OnClientClick="itemJSON();"
                    OnClick="btnAddNewLines_Click" Text="Add New Lines" />
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
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

            addAutocomplete();
        });        
    </script>
    <script>
      
    </script>
</asp:Content>

 