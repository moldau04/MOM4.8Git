<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" Inherits="AddInvoiceNew" CodeBehind="AddInvoiceNew.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <script type="text/javascript" src="js/quickcodes.js"></script>
    <style>
        .highlight {
            background-color: Yellow;
        }

        .highlighted {
            background-color: Yellow;
        }


        .ui-menu .ui-menu-item-wrapper {
            position: relative;
            padding: 3px 1em 3px .4em;
        }

        .rgFooter > td {
            text-align: center !important;
        }

        .cent {
            text-align: center !important;
        }

        .RadGrid_Material .rgHeader {
            color: #2e6b89 !important;
            font-weight: bold !important;
        }

        ul.anchor-links li a {
            border-bottom: 1px groove !important;
        }

        .clearable {
            position: relative;
            display: inline-block;
            width: 100%;
        }

            .clearable input[type=text] {
                padding-right: 24px;
                width: 100%;
                box-sizing: border-box;
            }

        .clearable__clear {
            display: none;
            position: absolute;
            right: 0;
            top: 0;
            padding: 6px 40px;
            font-style: normal;
            font-size: 1.2em;
            user-select: none;
            cursor: pointer;
        }

        .mr {
            margin-right: 10px;
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
    </style>

    <telerik:RadScriptBlock ID="RadScriptBlockFormattedName" runat="server">

        <script type="text/javascript">     
            $(document).ready(function () {
                //debugger;
                $("#divProject").slideUp();
                $("#<%=txtProject.ClientID%>").focusin(function () {
                    //debugger;
                    $("#divProject").slideDown();
                });
                $("#<%=txtProject.ClientID%>").focusout(function () {
                    $("#divProject").slideUp();
                    // debugger;
                });

                function dtaa() {
                    this.prefixText = null;
                    this.jdata = null;
                    this.InvID = null;
                    this.con = null;
                    this.Acct = null;
                }

                ///////////////////// Billing Code 


                $("[id*=txtBillingCode]").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.jdata = null;//$("[id*=hdnBillCodeJSON]").val();
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetBillingCode",
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
                        var txtBillingCode = this.id;
                        $(ddlBillingCode).val(ui.item.id);
                        var hdnStatus = $('#' + txtBillingCode.replace('txtBillingCode', 'hdnStatus'));
                        var hdnAStatus = $('#' + txtBillingCode.replace('txtBillingCode', 'hdnAStatus'));
                        var lblDescription = document.getElementById(txtBillingCode.replace('txtBillingCode', 'lblDescription'));
                        var ddlBillingCode = document.getElementById(txtBillingCode.replace('txtBillingCode', 'ddlBillingCode'));
                        var txtBillingCodeID = document.getElementById(txtBillingCode.replace('txtBillingCode', 'txtBCodeID'));
                        var lblPricePer = document.getElementById(txtBillingCode.replace('txtBillingCode', 'lblPricePer'));
                        var txtBCodeType = document.getElementById(txtBillingCode.replace('txtBillingCode', 'txtBCodeType'));

                        $('#<%=hdntxtBCodeID.ClientID%>').val(txtBillingCodeID);

                        if (ui.item.id == null) {

                            $(lblDescription).val('');
                            $(this).val('');
                            $(ddlBillingCode).val('');
                            $(txtBillingCodeID).val('');
                            $(txtBCodeType).val('');
                            $('#<%=hdntxtBCodeID.ClientID%>').val('');
                        }
                        else {

                            try {

                                $(hdnStatus).val(ui.item.Status);
                                $(hdnAStatus).val(ui.item.AStatus);
                                $(lblDescription).val(ui.item.fDesc.replace("<br />", ''));
                                $(this).val(ui.item.BillType);
                                $(ddlBillingCode).val(ui.item.id);
                                $(txtBillingCodeID).val(ui.item.id);
                                $(txtBCodeType).val(ui.item.type);

                                var priceVal = ui.item.Price1 == '0' ? '0.00' : ui.item.Price1;
                                debugger
                                $(this).closest('tr').find('td').eq(6).find('input').val(priceVal);
                                if ($(hdnAStatus).val() == "1" || $(hdnStatus).val() == "1") {
                                    alert("This Account\\Billing code is inactive. Please change account\\billing code name before proceeding.");
                                    // noty({
                                    //    text: 'This Account\\Billing code is inactive. Please change account\\billing code name before proceeding.',
                                    //    type: 'warning',
                                    //    layout: 'topCenter',
                                    //    closeOnSelfClick: false,
                                    //    timeout: 4000,
                                    //    theme: 'noty_theme_default',
                                    //    closable: true
                                    //});
                                }

                                var strBillid = ui.item.id;
                                var PP = lblPricePer;
                                //var hdnBillCode = document.getElementById('<%=hdnBillCodeJSON.ClientID%>');

                                //if (hdnBillCode != '') {
                                //    var json = hdnBillCode.value
                                //    var obj = jQuery.parseJSON(json);
                                //    jQuery.each(obj, function (key, val) {
                                //        var stop = 0;
                                //        jQuery.each(val, function (keyu, valu) {
                                //            if (keyu == "id" && valu == strBillid) {
                                //                stop = 1;
                                //            }
                                //            if (stop == 1 && keyu == "fDesc") {
                                //                lblDescription.value = valu;
                                //            }
                                //            if (stop == 1 && keyu == "Price1") {
                                //                PP.value = valu;
                                //            }
                                //        });
                                //    });
                                //}

                                var billrate = $("#<%=hdnBillRate.ClientID%>").val();
                                if (billrate != '') {
                                    $(lblPricePer).val(billrate);
                                }
                                CalculateGridAmount();

                                /////  $$$$  If Inventory Tracking ON then allow user to select warehouse and location $$$$ ////////

                                if ($('#<%=hdnISInventoryTrackingON.ClientID%>').val() == '1') {
                                    if ($(txtBCodeType).val() == '0') {
                                     //var window = $find('<%= RadWindowWareHouse.ClientID %>');
                                        //window.show();
                                    }
                                }

                            } catch{
                                $(lblDescription).val('');
                                $(this).val('');
                                $(ddlBillingCode).val('');
                                $(txtBillingCodeID).val('');
                                $(txtBCodeType).val('');
                                $('#<%=hdntxtBCodeID.ClientID%>').val('');
                            }
                        }
                        return false;
                    },
                    focus: function (event, ui) {
                        try {
                            $(this).val(ui.item.BillType);
                        } catch{ }
                        return false;
                    },
                    minLength: 0,
                    delay: 10
                }).bind('click', function () { $(this).autocomplete("search"); });

                $.each($(".billingsearch"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var ula = ul;
                        var itema = item;
                        var result_value = item.BillType;
                        var result_fDesc = item.fDesc;
                        var INVTYPE = item.type;
                        var HAND = item.Hand;
                        var HANDDesc = '';
                        var x = new RegExp('\\b' + query, 'ig');

                        try {

                            if (result_value != null) {
                                result_value = result_value.replace(x, function (FullMatch, n) {
                                    return '<span class="highlight">' + FullMatch + '</span>'
                                });
                            }

                            if (result_fDesc != null) {
                                result_fDesc = result_fDesc.replace(x, function (FullMatch, n) {
                                    return '<span class="highlight">' + FullMatch + '</span>'
                                });
                            }

                        } catch{ }
                        if (INVTYPE == 0) {
                            HANDDesc = ', Qty:' + HAND;
                        }
                        if (result_value == null) {
                            result_value = '';
                        }
                        if (result_fDesc == null) {
                            result_fDesc = '';
                        } else { result_fDesc = ' , ' + result_fDesc; }

                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>  " + result_value + '' + HANDDesc + "   <span style='color:Gray;'><b>  </b>" + result_fDesc + "</span></a>")
                            .appendTo(ul);
                    };
                });



                ///////////////////////WareHouse
                //txtGvWarehouse
                $("[id*=txtGvWarehouse]").autocomplete({


                    source: function (request, response) {

                        var dtaaa = new dtaa();

                        dtaaa.prefixText = request.term;

                        query = request.term;

                        var str = request.term;

                        var InvID_GetID = $('#<%=hdntxtBCodeID.ClientID%>').val();

                        console.log($(InvID_GetID).val());

                        dtaaa.InvID = $(InvID_GetID).val();
                        dtaaa.isShowAll = "yes";
                        //debugger;

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
                            var InvID_GetID = $('#<%=hdntxtBCodeID.ClientID%>').val();

                            var hdnWarehouse = document.getElementById(InvID_GetID.replace('txtBCodeID', 'hdnWarehouse'));

                           // debugger;
                            var Str = ui.item.WarehouseID + ", " + ui.item.WarehouseName;

                            $(this).val(Str);

                            $(txtGvWarehouse).val(Str);

                            $(hdnWarehouse).val(ui.item.WarehouseID);

                            var locationID = 0;

                            var warehouseID = $(hdnWarehouse).val();

                            var invID = $(InvID_GetID).val();

                            //// Check ON Hand
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "AccountAutoFill.asmx/IsItemOnHand",
                                data: '{"INVitemID": "' + invID + '","WareHouseID": "' + ui.item.WarehouseID + '","WHLocationID": "0"}',
                                dataType: "json",
                                async: true,
                                success: function (data) {
                                    if (data.d == "false") {
                                        alert("Item selected is not on hand, please choose another one.");
                                        $(txtGvWarehouse).val('');
                                        $(hdnWarehouse).val('');
                                    }
                                },
                                error: function (result) {
                                    // alert("Due to unexpected errors we were unable to load item.");
                                },

                            });

                            ////

                        } catch{ }
                        return false;
                    },
                    focus: function (event, ui) {
                        try {
                            $(this).val(ui.item.WarehouseID);
                        } catch{ }
                        return false;
                    },
                    minLength: 0,
                    delay: 50
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
                                    .append("<a style='color:blue;'>" + result_item + "</a>")
                                    .appendTo(ul);
                            }
                        };
                });

                //warehouselocation

                //txtGvWarehouseLocation
                $("[id*=txtGvWarehouseLocation]").autocomplete({

                    source: function (request, response) {

                        //debugger;

                        var dtaaa = new dtaa();

                        dtaaa.prefixText = request.term;

                        query = request.term;

                        var str = request.term;

                        var InvID_GetID = $('#<%=hdntxtBCodeID.ClientID%>').val();

                        var hdnWarehouse = document.getElementById(InvID_GetID.replace('txtBCodeID', 'hdnWarehouse'));


                        var ID = $(hdnWarehouse).val();


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

                            var InvID_GetID = $('#<%=hdntxtBCodeID.ClientID%>').val();

                            var hdnWarehouse = document.getElementById(InvID_GetID.replace('txtBCodeID', 'hdnWarehouse'));

                            var hdnWarehouseLocationID = document.getElementById(InvID_GetID.replace('txtBCodeID', 'hdnWarehouseLocationID'));



                            var hdnWarehouse = document.getElementById(InvID_GetID.replace('txtBCodeID', 'hdnWarehouse'));

                            var Str = ui.item.ID + ", " + ui.item.Name;
                            $(this).val(Str);

                            $(txtGvWarehouseLocation).val(Str);

                            $(hdnWarehouseLocationID).val(ui.item.ID);

                            var locationID = $(hdnWarehouseLocationID).val();

                            var warehouseID = $(hdnWarehouse).val();

                            var invID = $(InvID_GetID).val();


                            //// Check ON Hand
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "AccountAutoFill.asmx/IsItemOnHand",
                                data: '{"INVitemID": "' + invID + '","WareHouseID": "' + warehouseID + '","WHLocationID": "' + ui.item.ID + '"}',
                                dataType: "json",
                                async: true,
                                success: function (data) {
                                    if (data.d == "false") {
                                        alert("Item selected is not on hand, please choose another one.");
                                        $(txtGvWarehouseLocation).val('');
                                        $(hdnWarehouseLocationID).val('');
                                    }
                                },
                                error: function (result) {
                                    alert("Due to unexpected errors we were unable to load item.");
                                },

                            });

                            ////


                        } catch{ }

                        return false;
                    },
                    focus: function (event, ui) {
                        try {
                            $(this).val(ui.item.ID);
                        } catch{ }
                        return false;
                    },
                    minLength: 0,
                    delay: 50
                }).click(function () {
                    $(this).autocomplete('search', $(this).val())
                })
                $.each($(".WarehouseLocationsearchinput"), function (index, item) {
                    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                       // debugger;
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

            });

        </script>

        <script type="text/javascript">
            $(function () {
                var stselected = $('option:selected', $('#<%=ddlStatus.ClientID%>')).val();
                var isadd = '<%=ViewState["mode"] %>';
                $("#<%=ddlStatus.ClientID%>").bind('change', function () {
                    if (isadd == '1') {
                        if (stselected != "4") {
                            if ($('option:selected', $(this)).val() == '4') {
                                noty({
                                    text: 'This invoice has already posted and cannot be set with Status Marked as Pending.',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: 4000,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                                $("#<%=ddlStatus.ClientID%>").val('0');
                            }
                        }
                    }
                });
                var projVal = '';
                $("#<%=txtProject.ClientID%>").autocomplete({
                    open: function (e, ui) {
                        $(".ui-autocomplete").mCustomScrollbar({
                            setHeight: 182,
                            theme: "dark-3",
                            autoExpandScrollbar: true
                        });
                    },
                    response: function (e, ui) {
                        $(".ui-autocomplete").mCustomScrollbar("destroy");
                    },
                    source: function (request, response) {
                        $('#divProject').hide();
                        projVal = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "TimeCardService.asmx/GetInvoiceByJobID",
                            data: "{'prefixText':'" + request.term + "'}",
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response(data.d);
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load projects");
                            }
                        });
                    },
                    select: function (event, ui) {
                        debugger
                        $("#<%=txtLocation.ClientID%>").val(ui.item.Tag);
                    $("#<%=txtCustomer.ClientID%>").val(ui.item.Name);
                    $("#<%=hdnLocId.ClientID%>").val(ui.item.Loc);
                    $("#<%=txtProject.ClientID%>").val(ui.item.ID + " " + ui.item.fDesc);
                    $("#<%=hdnProjectId.ClientID%>").val(ui.item.ID);
                    $("#<%=txtAddress.ClientID%>").val(ui.item.Address);
                    $("#<%=hdnIsProjectSearch.ClientID%>").val("1");
                    $("#<%=lnkProjectID.ClientID%>").attr('href', 'addinvoice?uid=' + ui.item.ID);


                    document.getElementById("<%=btnGetCode.ClientID%>").click();
                        Materialize.updateTextFields();
                        return false;
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
                    .data("ui-autocomplete")._renderItem = function (ul, item) {
                        var x = new RegExp('\\b' + projVal, 'ig'); // notice the escape \ here...    
                        var _render_id = item.ID;
                        var _render_desc = item.fDesc;
                        var _render_tag = item.Tag;
                        try {
                            if (_render_id != null) {
                                _render_id = _render_id.replace(x, function (FullMatch, n) {
                                    return '<span class="highlight">' + FullMatch + '</span>'
                                });
                            }
                            if (_render_desc != null) {
                                _render_desc = _render_desc.replace(x, function (FullMatch, n) {
                                    return '<span class="highlight">' + FullMatch + '</span>'
                                });
                            }
                            if (_render_tag != null) {
                                _render_tag = _render_tag.replace(x, function (FullMatch, n) {
                                    return '<span class="highlight">' + FullMatch + '</span>'
                                });
                            }
                        } catch (e) { };
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<span class='auto_item'>" + _render_id + " </span><span class='auto_item'>" + _render_desc + "</span><span class='auto_desc'>-" + _render_tag + "</span>")
                            .appendTo(ul);
                    };

                SetClose();
                $("#<%=txtProject.ClientID%>").keyup(function () {
                    SetClose();
                });
                $("#<%=txtLocation.ClientID%>").keyup(function () {
                    SetClose();
                });
                $("#<%=txtCustomer.ClientID%>").keyup(function () {
                    SetClose();
                });
                $('.clearable__clear').click(function () {
                    var isDisabled = $("#<%=txtLocation.ClientID%>").prop('disabled');
                    if (isDisabled == false) {
                        $("#<%=txtLocation.ClientID%>").val('');
                        $("#<%=txtCustomer.ClientID%>").val('');
                        $("#<%=hdnLocId.ClientID%>").val('');
                        $("#<%=hdnPatientId.ClientID%>").val('');
                        $("#<%=txtProject.ClientID%>").val('');
                        $("#<%=hdnProjectId.ClientID%>").val('');
                        $("#<%=txtAddress.ClientID%>").val('');
                        $('#<%=txtJobRemarks.ClientID%>').text('');
                        $('#<%=txtRemarks.ClientID%>').text('');

                        $('#<%=ProjectBillingInfo.ClientID%>').hide();
                        $('#dvProjectInfo').hide();
                        $('#<%=lnkProjectID.ClientID%>').attr('href', 'javascript:void(0)');
                        $('#<%=iProIcon.ClientID%>').attr('style', 'color:#5815c02b !important');
                        $('#ctl00_ContentPlaceHolder1_RadGrid_gvInvoices_ctl00 tbody tr').each(function () {
                            $(this).find('td:eq(2) input[type=text]').val('');
                            $(this).find('td:eq(3) input[type=text]').val('');
                            $(this).find('td:eq(4) textarea').text('');
                            $(this).find('td:eq(5) input[type=text]').val('0.00');
                            $(this).find('td:eq(6) span').text('');
                            $(this).find('td:eq(8) input[type=text]').val('0.00');
                            $(this).find('td:eq(9) span').text('0.00');
                            $(this).find('td:eq(1) select').html('<option value="0">-Select-</option>');
                            $('#dvprojectSlider').html('');
                            CalculateGridAmount();
                        });
                        SetClose();
                    }

                });

                function SetClose() {
                    $Proj = $('#<%=txtProject.ClientID%>'),
                        $Loc = $('#<%=txtLocation.ClientID%>'),
                        $Cus = $('#<%=txtCustomer.ClientID%>');

                    if ($Proj.val() != '') {
                        $Proj.closest('span').find('i').attr('style', 'display:inline');
                    }
                    else {
                        $Proj.closest('span').find('i').attr('style', 'display:none');
                    }
                    if ($Loc.val() != '') {
                        $Loc.closest('span').find('i').attr('style', 'display:inline');
                    }
                    else {
                        $Loc.closest('span').find('i').attr('style', 'display:none');
                    }
                    if ($Cus.val() != '') {
                        $Cus.closest('span').find('i').attr('style', 'display:inline');
                    }
                    else {
                        $Cus.closest('span').find('i').attr('style', 'display:none');
                    }
                }
            });
        </script>

    </telerik:RadScriptBlock>

    <script type="text/javascript">
        function InActiveBillCode(code) {
            noty({
                text: 'Billing code "' + code + '" is InActive. Please select Active Billing code.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 10000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function Billto() {
            if ($('#<%=chkBillTo.ClientID%>').is(':checked')) {
                $("#<%=txtAddress.ClientID%>").prop("disabled", false);
            } else {
                $("#<%=txtAddress.ClientID%>").prop("disabled", true);
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
            var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
            if (hdnLocId.value == '') {
                args.IsValid = false;
            }
        }
        function showModalPopupViaClientCust() {
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.show();
        }
        function EditInvoice() {
            document.getElementById('<%=btnEdit.ClientID%>').click();
        }
        function SelectRowFill(gridview, id, name, hdn, hdnnames, DIV) {
            var rowid = document.getElementById(id);
            var rowname = document.getElementById(name);
            var grid = document.getElementById(gridview);
            var hidden = document.getElementById(hdn);
            var hdnName = document.getElementById(hdnnames);
            hdnName.value = rowname.innerHTML;
            hidden.value = rowid.innerHTML;
            $("#" + DIV).slideUp();
        }


        function ConfirmCombineTicket(count) {
            var ret = confirm('There exists ' + count + ' more tickets for the same workorder. Do you want to include them with this invoice?');
            if (ret == true) {
                document.getElementById('<%=hdnCombine.ClientID%>').value = 1;
            }
            else {
                document.getElementById('<%=hdnCombine.ClientID%>').value = 0;
            }
        }
        function hideModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }

        function ZeroAmtValidate(control) {
            var validated = Page_ClientValidate('Invoice');
            disableControl(control);
            if (validated) {
                var projectvalidate = true;
                var project = $("#<%=txtProject.ClientID%>").val();
                if (project === "") {
                    projectvalidate = confirm('Are you sure you want to save this invoice without a project number?');
                }
                if (projectvalidate === true) {
                    var Warn = false;
                    var total = 0;

                    var masterTable = $find("<%=RadGrid_gvInvoices.ClientID%>").get_masterTableView();
                    var count = masterTable.get_dataItems().length;
                    var item;
                    for (var i = 0; i < count; i++) {
                        item = masterTable.get_dataItems()[i];
                        var amt = item.findElement("lblAmount");
                        var amtVal = $(amt).html();
                        if (amtVal !== "") {
                            total = total + parseFloat(amtVal);
                        }
                    }
                    if (total !== '') {
                        if (parseFloat(total) === 0) {
                            enableControl(control);
                            Warn = true;
                        }
                    }
                    else {
                        enableControl(control);
                        Warn = true;
                    }
                    if (Warn === true) {
                        return confirm('This invoice has a zero amount total. Are you sure you want to save it?');
                    }
                    else if (Warn === false) {
                        enableControl(control);
                        return true;
                    }
                }
            }
            Page_BlockSubmit = false;
            enableControl(control);
            return false;
        }

        function disableControl(control) {
            $(control).css("pointer-events", "none");
        }

        function enableControl(control) {
            setTimeout(function () { $(control).css("pointer-events", "all"); }, 1000);

        }

    </script>

    <script>
        $(document).ready(function () {
            function dtaa() {
                this.prefixText = null;
                this.con = document.getElementById('<%=hdnCon.ClientID%>').value;
                this.custID = null;
            }
            var query = "";
            $("#<%=txtCustomer.ClientID%>").autocomplete({

                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetCustomer",
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
                    $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                    $("#<%=hdnPatientId.ClientID%>").val(ui.item.value);
                    $("#<%=txtLocation.ClientID%>").focus();
                    $("#<%=txtLocation.ClientID%>").val('');
                    $("#<%=hdnLocId.ClientID%>").val('');
                    $("#<%=hdnIsProjectSearch.ClientID%>").val('0');
                    document.getElementById('<%=btnSelectCustomer.ClientID%>').click();
                    Materialize.updateTextFields();
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 50
            })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.label;
                    var result_desc = item.desc;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...         
                    if (result_item) {
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    if (result_item) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul)
                    }
                };
            ///////////// Ajax call for location auto search ////////////////////
            var queryloc = "";
            $("#<%=txtLocation.ClientID%>").autocomplete(
                {

                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.custID = 0;
                        if (document.getElementById('<%=hdnPatientId.ClientID%>').value != '') {
                            dtaaa.custID = document.getElementById('<%=hdnPatientId.ClientID%>').value;
                        }
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
                                $("#divProject").slideUp();
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load customers");
                            }
                        });
                    },
                    select: function (event, ui) {
                        $("#<%=txtLocation.ClientID%>").val(ui.item.label);
                        $("#<%=hdnLocId.ClientID%>").val(ui.item.value);
                        document.getElementById('<%=btnSelectLoc.ClientID%>').click();
                        Materialize.updateTextFields();
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%=txtLocation.ClientID%>").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 50
                })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    var result_item = item.label;
                    var result_desc = item.desc;
                    var x = new RegExp('\\b' + queryloc, 'ig'); // notice the escape \ here...       
                    if (result_item) {
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    if (result_item) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
            $("#<%=txtCustomer.ClientID%>").keyup(function (event) {
                var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
                if (document.getElementById('<%=txtCustomer.ClientID%>').value == '') {
                    hdnPatientId.value = '';
                }
            });
            $("#<%=txtLocation.ClientID%>").keyup(function (event) {
                var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
                if (document.getElementById('<%=txtLocation.ClientID%>').value == '') {
                    hdnLocId.value = '';
                }
            });
            //debugger;
            $("#divProject").slideUp();
            $("#<%=txtProject.ClientID%>").focusin(function () {
                //debugger;
                $("#divProject").slideDown();
            });
            $("#<%=txtProject.ClientID%>").focusout(function () {
                //debugger;
                $("#divProject").slideUp();
            });
            $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);

            $("#<%=txtProject.ClientID%>").change(function () {
                var projectText = $("#<%=txtProject.ClientID%>").val();
                if (projectText == "" || projectText == undefined) {
                    document.getElementById('<%=btnResetProject.ClientID%>').click();
                    Materialize.updateTextFields();
                }
            });
            ///////////// Quick Codes //////////////
            $("#<%=txtRemarks.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtRemarks.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });
        });


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
        <img src="images/wheel.GIF" alt="Be patient..." style="position: fixed; margin-top: 25%; margin-left: 50%;" />
    </div>

    <telerik:RadAjaxManager ID="RadAjaxManager_Invoice" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid_gvInvoices">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoices" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnGetCode">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoices" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                    <telerik:AjaxUpdatedControl ControlID="divUpdate" />
                    <telerik:AjaxUpdatedControl ControlID="divUpdate_1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnResetProject">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoices" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                    <telerik:AjaxUpdatedControl ControlID="divUpdate" />
                    <telerik:AjaxUpdatedControl ControlID="divUpdate_1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkAddnew">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoices" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                    <telerik:AjaxUpdatedControl ControlID="divUpdate_1" />

                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnCopyPrevious">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoices" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                </UpdatedControls>
            </telerik:AjaxSetting>


            <telerik:AjaxSetting AjaxControlID="btnDelete">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoices" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                    <telerik:AjaxUpdatedControl ControlID="divUpdate_1" />
                    <telerik:AjaxUpdatedControl ControlID="divUpdate" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSelectCustomer">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoices" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                    <telerik:AjaxUpdatedControl ControlID="divUpdate" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                    <telerik:AjaxUpdatedControl ControlID="divUpdate_1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSelectLoc">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divUpdate" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                    <telerik:AjaxUpdatedControl ControlID="divUpdate_1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_gvInvoices" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnSubmit" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                    <telerik:AjaxUpdatedControl ControlID="divSuccess" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkPreviewEmail">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lnkPreviewEmail" LoadingPanelID="RadAjaxLoadingPanel_Invoice" />
                    <telerik:AjaxUpdatedControl ControlID="divSuccess" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Invoice" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <div class="divbutton-container">
        <asp:HiddenField ID="hdnIsProjectPO" runat="server" Value="0" />
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;<asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Invoice</asp:Label></div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <a class="dropdown-button" href="#!" data-activates="dropdown1">Reports
                                            </a>
                                        </div>
                                        <ul id="dropdown1" class="dropdown-content">
                                            <li>
                                                <asp:LinkButton ID="lnkPDF" runat="server" Enabled="true" OnClick="lnkPDF_Click"> <i class="fa fa-file-pdf-o" style="color:red; background-color:transparent" aria-hidden="true"></i>&nbsp;&nbsp; Invoice <i class="fa fa-download" aria-hidden="true" style="background-color:transparent;"></i></asp:LinkButton>
                                            </li>
                                            <li>
                                                <a href="#" class="-text">Billing Invoice</a>
                                            </li>
                                        </ul>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkMakePayment" runat="server" Text="Make Payment" OnClick="lnkMakePayment_Click" Visible="false"></asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkPreviewEmail" runat="server" Text="Preview" OnClientClick="return ZeroAmtValidate(this);" OnClick="lnkPreviewEmail_Click" Visible="true"></asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" OnClientClick="return ZeroAmtValidate(this);" ValidationGroup="Invoice">Save</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkReceiptPayment" runat="server" OnClick="lnkReceiptPayment_Click">Receipt Payment</asp:LinkButton>
                                        </div>
                                        <ul class="nav navbar-nav pull-right" style="display: none">
                                            <li class="dropdown dropdown-user">
                                                <a class="btn-box hover-zoom-effect dropdown-button" href="#!" data-activates="dynamicUI">
                                                    <div class="text">
                                                        Print
                                                    </div>
                                                </a>
                                                <ul id="dynamicUI" class="dropdown-content">
                                                    <li>
                                                        <asp:LinkButton ID="lnkPrint" runat="server" CausesValidation="true" OnClick="lnkPrint_Click" Enabled="true">Invoice</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnk_InvoiceMaint" runat="server" CausesValidation="true"
                                                            OnClick="lnk_InvoiceMaint_Click" Enabled="true">Invoice Maintenance Report</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnk_InvoiceException" runat="server" CausesValidation="true"
                                                            OnClick="lnk_InvoiceException_Click" Enabled="true">Invoice Exception report</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnk_InvoiceLNY" runat="server" CausesValidation="true"
                                                            OnClick="lnk_InvoiceLNY_Click" Enabled="true">Invoice-LNY</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkInvTkt" runat="server" CausesValidation="true" OnClick="lnkInvTkt_InvoiceLNY_Click"
                                                            Enabled="true">Invoice With Ticket</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnk_InvoiceAdamMaintenance" runat="server" CausesValidation="true" OnClick="lnkPrint_Click"
                                                            Enabled="true">Maintenance Invoice</asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnk_InvoiceAdamBilling" runat="server" CausesValidation="true" OnClick="lnkPrint_Click"
                                                            Enabled="true">Billing Invoice</asp:LinkButton></li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                            OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label ID="lblInv" Visible="false" runat="server"></asp:Label>
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
                                    <li><a href="#accrdInvoice">Invoice Detail</a></li>

                                    <li id="liHistoryPayment" runat="server" style="display: none"><a href="#accrdPayment">Payment History</a></li>
                                    <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlSave" runat="server">
                                        <asp:Panel ID="pnlNext" runat="server" Visible="false">
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" OnClick="lnkFirst_Click"
                                                    CausesValidation="False">
                                                        <i class="fa fa-angle-double-left"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False"
                                                    OnClick="lnkPrevious_Click">
                                                        <i class="fa fa-angle-left"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False"
                                                    OnClick="lnkNext_Click">
                                                        <i class="fa fa-angle-right"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CssClass="icon-last" CausesValidation="False"
                                                    OnClick="lnkLast_Click">
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
                    <div class="card" style="min-height: 700px !important; border-radius: 6px;">
                        <div class="alert alert-warning" runat="server" id="divSuccess">
                            <button type="button" class="close" data-dismiss="alert">×</button>
                            These month/year period is closed out. You do not have permission to add/update this record.
                        </div>
                        <div class="alert alert-warning" runat="server" id="divProjectClose">
                            <button type="button" class="close" data-dismiss="alert">×</button>
                            Project is closed. You do not have permission to add/update this record.
                        </div>
                        <div class="card-content" style="padding-top: 10px;">

                            <div style="float: left; clear: both;" runat="server" id="divUpdate">
                                <input id="hdnCon" runat="server" type="hidden" />
                                <input id="hdnPatientId" runat="server" type="hidden" />
                                <input id="hdnLocId" runat="server" type="hidden" />
                                <input id="hdnFocus" runat="server" type="hidden" />
                                <input id="hdnStax" runat="server" type="hidden" />
                                <input id="hdnGstTax" runat="server" type="hidden" />
                                <input id="hdnTaxRegion" runat="server" type="hidden" />
                                <input id="hdnsTaxType" runat="server" type="hidden" />
                                <input id="hdnBillCodeJSON" runat="server" type="hidden" />
                                <input id="hdnCombine" runat="server" type="hidden" />
                                <input id="hdnProjectId" runat="server" type="hidden" />
                                <input id="hdnProjectStatus" runat="server" type="hidden" />
                                <input id="hdnTotalAmount" runat="server" type="hidden" />
                                <div class="form-section-row" id="accrdInvoice">
                                    <div class="col s12 m12 l12">
                                        <div class="form-section-row ">
                                            <div class="form-input-row">
                                                <div class="form-section3 invoicemgncustom">
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <span class="clearable">
                                                                <asp:TextBox ID="txtCustomer" runat="server" autocomplete="off"></asp:TextBox><%-- data-provide="typeahead" searchinput  --%>
                                                                <asp:FilteredTextBoxExtender ID="txtCustomer_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtCustomer">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:Button CausesValidation="false" ID="btnSelectCustomer" runat="server" Text="Button"
                                                                    Style="display: none;" OnClick="btnSelectCustomer_Click"
                                                                    UseSubmitBehavior="False" />
                                                                <asp:Label ID="lblcust" runat="server" AssociatedControlID="txtCustomer">Customer - Search By Name, Phone#, Address etc</asp:Label>
                                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtCustomer"
                                                                    ErrorMessage="Please select the customer" ClientValidationFunction="ChkCustomer"
                                                                    Display="None" SetFocusOnError="True" ValidationGroup="Invoice"></asp:CustomValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="CustomValidator1">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtCustomer"
                                                                    Display="None" ErrorMessage="Please select the customer" SetFocusOnError="True" ValidationGroup="Invoice"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator19_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator19">
                                                                </asp:ValidatorCalloutExtender>
                                                                <i class="clearable__clear mr">&times;</i>
                                                            </span>
                                                        </div>
                                                        <div class="srchclr btnlinksicon rowbtn">
                                                            <asp:HyperLink for="txtCustomer" ID="lnkCustomerID" Visible="true" Target="_blank" runat="server"><i class="mdi-social-people" style="margin-left:0px !important;"></i></asp:HyperLink>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <span class="clearable">
                                                                <asp:TextBox ID="txtLocation" runat="server" type="text" CssClass="validate" autocomplete="off"
                                                                    ToolTip="Location Name "></asp:TextBox>

                                                                <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                                                                    Enabled="false" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtLocation">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:Button CausesValidation="false" ID="btnSelectLoc" runat="server" Text="Button"
                                                                    Style="display: none;" OnClick="btnSelectLoc_Click" UseSubmitBehavior="False" />
                                                                <asp:Label ID="Label4" runat="server" AssociatedControlID="txtLocation">Location - Search By Location, Phone#, Address etc</asp:Label>
                                                                <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtLocation" ClientValidationFunction="ChkLocation"
                                                                    Display="None" SetFocusOnError="True" ValidationGroup="Invoice"></asp:CustomValidator>
                                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                    PopupPosition="Right" TargetControlID="CustomValidator2">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLocation"
                                                                    Display="None" ErrorMessage="Location Name Required" SetFocusOnError="True" ValidationGroup="Invoice"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                                </asp:ValidatorCalloutExtender>
                                                                <i class="clearable__clear mr">&times;</i>
                                                            </span>
                                                        </div>
                                                        <div class="srchclr btnlinksicon rowbtn">
                                                            <asp:HyperLink for="txtLocation" ID="lnkLocationID" Visible="true" Target="_blank" runat="server"><i class="mdi-communication-location-on" style="margin-left:0px !important;"></i></asp:HyperLink>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:Label runat="server" ID="lbltxtAddress" AssociatedControlID="txtAddress">Bill To</asp:Label>
                                                            <asp:TextBox ID="txtAddress" runat="server" ToolTip="Address"
                                                                TextMode="MultiLine" MaxLength="8000" CssClass="materialize-textarea pd-negate"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtAddress"
                                                                Display="None" ErrorMessage="Bill To Address Required" SetFocusOnError="True" ValidationGroup="Invoice">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator40_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator40">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <asp:Label runat="server" ID="Label2" AssociatedControlID="txtRemarks">Invoice Remarks</asp:Label>
                                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"
                                                                MaxLength="8000" CssClass="materialize-textarea pd-negate"></asp:TextBox>
                                                            <asp:Image ID="imgVoid" runat="server" ImageUrl="~/images/icons/void.png" Style="height: 35px;" />
                                                            <asp:ImageButton ID="imgPaid" ImageUrl="~/images/icons/paid.png" Height="40px" runat="server" OnClientClick="scrollToAnchor();return false;" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3">
                                                    <div class="input-field col s12 invoicepagetitlewrap">
                                                        <div class="row">
                                                            <div class="invoicemaintitle borderseperator">Invoice</div>
                                                            <div class="invoicemainnm">
                                                                <asp:Label ID="lblCompNme" runat="server"></asp:Label>
                                                            </div>
                                                            <div class="invoicemainaddr">
                                                                <asp:Label ID="lblCompAddress" runat="server"></asp:Label>
                                                                <asp:Label ID="lblCompCity" runat="server"></asp:Label>
                                                                <asp:Label ID="lblCompState" runat="server"></asp:Label>
                                                                <asp:Label ID="lblCompZip" runat="server"></asp:Label>
                                                                <asp:Label ID="lblCompphone" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>
                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            &nbsp;
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12" style="margin-top: 10px;">
                                                        <div class="row">
                                                            <asp:Label runat="server" AssociatedControlID="txtJobRemarks">Project Remarks</asp:Label>
                                                            <asp:TextBox ID="txtJobRemarks" runat="server" TextMode="MultiLine"
                                                                MaxLength="8000" CssClass="materialize-textarea pd-negate"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section3-blank">
                                                    &nbsp;
                                                </div>
                                                <div class="form-section3 invoicemgncustom">

                                                    <div class="form-section4" style="width: 100% !important;">
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label for="txtInvoiceDate" id="Label5" runat="server">Invoice Date</label>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server"
                                                                    ControlToValidate="txtInvoiceDate" Display="None" ErrorMessage="Invoice Date Required"
                                                                    SetFocusOnError="True" ValidationGroup="Invoice"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator36_ValidatorCalloutExtender"
                                                                        runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator36">
                                                                    </asp:ValidatorCalloutExtender>
                                                                <asp:TextBox ID="txtInvoiceDate" CssClass="datepicker_mom" runat="server" MaxLength="50"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;  
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label for="txtInvoiceNo" id="Label8" runat="server">Manual Invoice #</label>
                                                                <asp:TextBox ID="txtInvoiceNo" runat="server" type="text" CssClass="validate pdngate" MaxLength="50" ToolTip="Invoice #"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12">
                                                        <div class="row">
                                                            <span class="clearable">
                                                                <asp:TextBox runat="server" ID="txtProject"
                                                                    type="text" autocomplete="off"></asp:TextBox>
                                                                <label for="txtProject">Project #</label>
                                                                <i class="clearable__clear">&times;</i>
                                                            </span>
                                                            <asp:FilteredTextBoxExtender ID="txtProject_FilteredTextBoxExtender" runat="server"
                                                                Enabled="false" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtProject">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:HiddenField ID="hdnIsProjectSearch" runat="server" Value="" />
                                                        </div>
                                                    </div>

                                                    <div class="srchclr btnlinksicon rowbtn">
                                                        <asp:HyperLink for="txtProject" ID="lnkProjectID" Target="_blank" runat="server"><i runat="server" id="iProIcon" class="mdi-notification-play-install" style="margin-left: 0px !important;"></i></asp:HyperLink>
                                                    </div>

                                                    <div id="divProject" class="grid_container" style="border: none !important; box-shadow: none !important">
                                                        <div class="form-section-row" style="margin-bottom: 0 !important; overflow-x: hidden; overflow-y: scroll; max-height: 200px;">
                                                            <div class="RadGrid RadGrid_Material FormGrid">
                                                                <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                                                    <script type="text/javascript">

                                                                        function SetProjectValues(e, v) {
                                                                            console.log(1);
                                                                            $("#<%=hdnProjectId.ClientID%>").val(e);
                                                                            $("#<%=txtProject.ClientID%>").val(e + " " + v);
                                                                            document.getElementById("<%=btnGetCode.ClientID%>").click();
                                                                        }
                                                                    </script>
                                                                </telerik:RadCodeBlock>


                                                                <div id="dvprojectSlider" style="position: absolute; max-height: 200px; overflow: auto; z-index: 99; background-color: white; border: solid 1px  #9e9e9e; box-shadow: 0 5px #ccc; border-radius: 5px !important">
                                                                    <ul class="ui-menu ui-widget ui-widget-content">
                                                                        <telerik:RadListView ID="RadProjectListView" runat="server" RenderMode="Auto"
                                                                            ItemPlaceholderID="ProjectItemContainer">
                                                                            <ItemTemplate>

                                                                                <li class="ui-menu-item" style="border: solid dashed 1px">
                                                                                    <a style="color: black; display: inline-block;" class="aGetProjectVal" onclick="SetProjectValues('<%# Eval("id") %>','<%#Eval("fdesc")%>');">
                                                                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                                        , 
                                                                           
                                                                                <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>

                                                                                        <asp:Label ID="lbldate" runat="server"><%#Eval("fdate", "{0:MM/dd/yyyy}")%></asp:Label>

                                                                                    </a>
                                                                                    <div style="clear: both"></div>
                                                                                </li>

                                                                            </ItemTemplate>

                                                                        </telerik:RadListView>
                                                                    </ul>

                                                                </div>

                                                            </div>
                                                            <asp:Button ID="btnGetCode" runat="server" CausesValidation="false" Text="Button" Style="display: none;" OnClick="btnGetCode_Click" />
                                                            <asp:Button ID="btnResetProject" runat="server" CausesValidation="false" Text="Button" Style="display: none;" OnClick="btnResetProject_Click" />
                                                        </div>
                                                    </div>

                                                    <div class="input-field col s12" id="dvProjectInfo">
                                                        <div class="row">
                                                            <asp:Label runat="server" AssociatedControlID="txtJobSRemarks" ID="lblJobSRemarks" Visible="false">Special Notes</asp:Label>
                                                            <asp:TextBox ID="txtJobSRemarks" runat="server" TextMode="MultiLine" Visible="false"
                                                                MaxLength="8000" CssClass="materialize-textarea pd-negate"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                    <div class="form-section4" style="width: 100% !important;" runat="server" id="ProjectBillingInfo" visible="false">
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label for="txtBillingType" id="Label1" runat="server">Billing Type</label>
                                                                <asp:TextBox ID="txtBillingType" runat="server" ReadOnly="true" AutoCompleteType="None"
                                                                    MaxLength="15"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label for="txtjobamt" id="Label3" runat="server">Amount</label>
                                                                <asp:TextBox ID="txtjobamt" ReadOnly="true" runat="server" AutoCompleteType="None"
                                                                    MaxLength="15"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label for="txtBillRate" id="lblBillRate" runat="server">Billing Rate</label>
                                                                <asp:TextBox ID="txtBillRate" runat="server" ReadOnly="true" AutoCompleteType="None"
                                                                    MaxLength="15"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label for="txtOt" id="lblOt" runat="server">OT Rate</label>
                                                                <asp:TextBox ID="txtOt" runat="server" ReadOnly="true" AutoCompleteType="None"
                                                                    MaxLength="15"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label for="txtNt" id="lblNt" runat="server">1.7 Rate</label>
                                                                <asp:TextBox ID="txtNt" runat="server" ReadOnly="true" AutoCompleteType="None"
                                                                    MaxLength="15"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label for="txtDt" id="lblDt" runat="server">DT Rate</label>
                                                                <asp:TextBox ID="txtDt" runat="server" ReadOnly="true" AutoCompleteType="None"
                                                                    MaxLength="15"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label for="txtTravel" id="lblTravel" runat="server">Travel Rate</label>
                                                                <asp:TextBox ID="txtTravel" runat="server" ReadOnly="true" AutoCompleteType="None"
                                                                    MaxLength="15"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s2">
                                                            <div class="row">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s5">
                                                            <div class="row">
                                                                <label for="txtMileage" id="lblMileage" runat="server">Mileage</label>
                                                                <asp:TextBox ID="txtMileage" runat="server" ReadOnly="true" AutoCompleteType="None"
                                                                    MaxLength="15"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <asp:CheckBox ID="chkBillTo" runat="server" Text="Bill to Address"
                                                        Visible="false" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="cf"></div>
                                </div>
                            </div>

                            <div class="container accordian-wrap">
                                <div class="col s12 m12 l12">
                                    <div class="row">
                                        <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                                            <li>
                                                <div id="accrdBillingDetails" class="collapsible-header accrd active accordian-text-custom "><i class="mdi-communication-contacts"></i>Billing Details</div>
                                                <div class="collapsible-body" id="divBillingDetails">
                                                    <div class="form-content-wrap">
                                                        <div class="form-content-pd">
                                                            <div class="form-section-row" runat="server" id="divUpdate_1">
                                                                <div class="col s12 m12 l12 borderseperatortop">
                                                                    <div class="form-section-row">
                                                                        <div class="form-input-row">
                                                                            <div class="form-sectioninvoice">

                                                                                <div class="input-field col s12">
                                                                                    <div class="row">
                                                                                        <%--PO Textbox--%>
                                                                                        <asp:TextBox ID="txtPO" runat="server" CssClass="validate" MaxLength="25"></asp:TextBox>

                                                                                        <%--PO Label--%>
                                                                                        <asp:Label runat="server" ID="Label6" AssociatedControlID="txtPO">PO #</asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="input-field col s12">
                                                                                    <div class="row">
                                                                                        <%--Tax Rate Textbox--%>
                                                                                        <asp:TextBox ID="txtStaxrate" runat="server" type="text" CssClass="validate" MaxLength="25"></asp:TextBox>

                                                                                        <%--Tax Rate Label--%>
                                                                                        <asp:Label runat="server" ID="Label7" AssociatedControlID="txtStaxrate">Sales Tax Name with Rate</asp:Label>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-sectioninvoice-blank">
                                                                                &nbsp;
                                                                            </div>
                                                                            <div class="form-sectioninvoice">
                                                                                <div class="input-field col s12">
                                                                                    <div class="row">
                                                                                        <%--Terms Label--%>
                                                                                        <label class="drpdwn-label">Terms</label>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server"
                                                                                            ControlToValidate="ddlTerms" Display="None" ErrorMessage="Terms Required"
                                                                                            SetFocusOnError="True" ValidationGroup="Invoice"></asp:RequiredFieldValidator>
                                                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator41_ValidatorCalloutExtender"
                                                                                            runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator41">
                                                                                        </asp:ValidatorCalloutExtender>
                                                                                        <%--Terms Dropdown--%>
                                                                                        <asp:DropDownList ID="ddlTerms" runat="server" CssClass="browser-default" AutoPostBack="false"
                                                                                            OnSelectedIndexChanged="ddlTerms_SelectedIndexChanged" CausesValidation="true" ValidationGroup="Invoice" onchange="UpdateDueByTerms()">
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="input-field col s12">
                                                                                    <div class="row">
                                                                                        <%--Department Type Label--%>
                                                                                        <label class="drpdwn-label">Department Type</label>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" Enabled="false"
                                                                                            ControlToValidate="ddlDepartment" Display="None" ErrorMessage="Department Required"
                                                                                            SetFocusOnError="True" ValidationGroup="Invoice"></asp:RequiredFieldValidator>
                                                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator32_ValidatorCalloutExtender"
                                                                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator32" PopupPosition="TopLeft">
                                                                                        </asp:ValidatorCalloutExtender>
                                                                                        <%--Department Type Dropdown--%>
                                                                                        <%--     <asp:UpdatePanel runat="server">
                                                                                            <ContentTemplate>--%>
                                                                                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="browser-default">
                                                                                        </asp:DropDownList>
                                                                                        <%--  </ContentTemplate>
                                                                                        </asp:UpdatePanel>--%>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-sectioninvoice-blank">
                                                                                &nbsp;
                                                                            </div>
                                                                            <div class="form-sectioninvoice">
                                                                                <div class="input-field col s12">
                                                                                    <div class="row">
                                                                                        <%--Due Date Label--%>
                                                                                        <label>Due Date</label>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server"
                                                                                            ControlToValidate="txtDueDate" Display="None" ErrorMessage="Due Date Required"
                                                                                            SetFocusOnError="True" ValidationGroup="Invoice"></asp:RequiredFieldValidator>
                                                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator34_ValidatorCalloutExtender"
                                                                                            runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator34">
                                                                                        </asp:ValidatorCalloutExtender>
                                                                                        <%--Due Date Textbox--%>
                                                                                        <asp:TextBox ID="txtDueDate" CssClass="datepicker_mom" runat="server" MaxLength="50"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="input-field col s12">
                                                                                    <div class="row">
                                                                                        <%--Status Label--%>
                                                                                        <label class="drpdwn-label">Status</label>
                                                                                        <%--Status Dropdown--%>
                                                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="browser-default">
                                                                                            <asp:ListItem Value="0">Open</asp:ListItem>
                                                                                            <asp:ListItem Value="1">Paid</asp:ListItem>
                                                                                            <asp:ListItem Value="2">Voided</asp:ListItem>
                                                                                            <asp:ListItem Value="3">Partially Paid</asp:ListItem>
                                                                                            <asp:ListItem Value="4">Marked as Pending</asp:ListItem>
                                                                                            <asp:ListItem Value="5">Paid by Credit Card</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                </div>


                                                                            </div>
                                                                            <div class="form-sectioninvoice-blank">
                                                                                &nbsp;
                                                                            </div>
                                                                            <div class="form-sectioninvoice">
                                                                                <div class="input-field col s12">
                                                                                    <div class="row">
                                                                                        <%--Worker Label--%>
                                                                                        <label class="drpdwn-label">Worker</label>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ControlToValidate="ddlRoute"
                                                                                            Display="None" ErrorMessage="Mech Required" SetFocusOnError="True" Enabled="False" ValidationGroup="Invoice"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator35_ValidatorCalloutExtender"
                                                                                                runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator35">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                        <%--Worker Dropdown--%>
                                                                                        <asp:DropDownList ID="ddlRoute" runat="server" CssClass="browser-default">
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="input-field col s12">
                                                                                    <div class="row">
                                                                                        <%--Status Label--%>
                                                                                        <label class="drpdwn-label">Salesperson</label>
                                                                                        <%--Status Dropdown--%>
                                                                                        <asp:DropDownList ID="ddlsaleperson" runat="server" CssClass="browser-default">
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="cf"></div>
                                                            </div>
                                                            <div class="form-section-row">
                                                                <div class="form-section-row">
                                                                    <div class="btnlinks">
                                                                        <asp:LinkButton ToolTip="Add New"
                                                                            ID="lnkAddnew" runat="server"
                                                                            CausesValidation="false" OnClick="lnkAddnew_Click" UseSubmitBehavior="False">Add</asp:LinkButton>
                                                                        <asp:LinkButton ID="btnCopyPrevious" runat="server" CausesValidation="false"
                                                                            OnClick="btnCopyPrevious_Click" Text="Copy Previous" Style="display: none;"></asp:LinkButton>
                                                                    </div>
                                                                    <div class="btnlinks">
                                                                        <asp:LinkButton ToolTip="Delete"
                                                                            ID="btnDelete" runat="server" CausesValidation="False" UseSubmitBehavior="False"
                                                                            OnClick="btnDelete_Click" OnClientClick="return confirmDelete();">Delete</asp:LinkButton>
                                                                    </div>
                                                                    <div class="btnlinks">
                                                                        <asp:LinkButton
                                                                            ID="btnEdit" runat="server" OnClick="btnEdit_Click" CausesValidation="False" Visible="False">Edit</asp:LinkButton>

                                                                    </div>
                                                                </div>
                                                                <div class="grid_container" style="overflow: scroll;">
                                                                    <input id="hdnBillRate" runat="server" type="hidden" />
                                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                                        <%--   <telerik:RadAjaxPanel ID="RadAjaxPanel1"  runat="server" LoadingPanelID="RadAjaxLoadingPanel_Invoice" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">--%>
                                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvInvoices" HeaderStyle-CssClass="cent"
                                                                            runat="server" AllowSorting="true" OnPreRender="RadGrid_gvInvoices_PreRender" OnItemCreated="RadGrid_gvInvoices_ItemCreated" onblur="resetIndexF6()">
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                <ClientEvents OnKeyPress="AddNewRows" />
                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" ShowFooter="True" DataKeyNames="Line">
                                                                                <Columns>

                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-Width="30" UniqueName="check">
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnSelected" runat="server" />
                                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                            <asp:HiddenField ID="hdnChk" runat="server" Value="" />

                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn HeaderStyle-Width="40" HeaderStyle-HorizontalAlign="Center" UniqueName="dropdrap">
                                                                                        <ItemTemplate>
                                                                                            <div class="handle" style="cursor: move;" title="Move Up/Down">
                                                                                                <img src="images/Dragdrop.png" width="20" />
                                                                                            </div>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="ID" Visible="False" UniqueName="orderColumn">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblIndex" Visible="false" runat="server" Text='<%# Container.DataSetIndex+1 %>'></asp:Label>

                                                                                            <asp:Label ID="lblLine" Style="display: none;" runat="server" Text='<%# Eval("Line") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="Code" HeaderStyle-Width="200" FooterStyle-HorizontalAlign="left" UniqueName="ProjectCode">
                                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                        <ItemTemplate>
                                                                                            <asp:HiddenField ID="hdnIndex" runat="server" Value='<%# Container.ItemIndex+1%>' />
                                                                                            <asp:HiddenField ID="hdnLine" runat="server" Value='<%#  Eval("Line") %>' />
                                                                                            <asp:DropDownList ID="ddlProjectCode" runat="server"
                                                                                                CssClass="browser-default" DataTextField="billtype"
                                                                                                DataValueField="Line" DataSource='<%#dtProjectCodeData%>' SelectedValue='
                                                                                                    <%#  Eval("code")   %>
                                                                                                    '
                                                                                                OnSelectedIndexChanged="ddlProjectCode_SelectedIndexChanged" AutoPostBack="true" EnableViewState="true">
                                                                                            </asp:DropDownList>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblTotal" runat="server">Total</asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="Quan" UniqueName="Quan" HeaderStyle-Width="80">
                                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblQuantity" runat="server" Text='<%# Bind("Quan") %>' MaxLength="15"></asp:TextBox>
                                                                                            <asp:FilteredTextBoxExtender ID="lblQuantity_FilteredTextBoxExtender" runat="server"
                                                                                                Enabled="True" TargetControlID="lblQuantity" ValidChars="1234567890.-">
                                                                                            </asp:FilteredTextBoxExtender>
                                                                                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="lblQuantity"
                                                                                                Display="None" ErrorMessage="Quantity Required" SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender ID="rfvQuantity_ValidatorCalloutExtender" runat="server"
                                                                                                Enabled="True" PopupPosition="TopLeft" TargetControlID="rfvQuantity">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="Billing Code" HeaderStyle-Width="230" UniqueName="BillingCode">
                                                                                        <ItemTemplate>

                                                                                            <asp:TextBox ID="txtBillingCode" runat="server" CssClass="billingsearch" value='<%# Eval("billcode") %>'></asp:TextBox>

                                                                                            <asp:HiddenField ID="txtBCodeID" runat="server" Value='<%# Eval("acct") %>'></asp:HiddenField>
                                                                                            <asp:HiddenField ID="hdnStatus" runat="server" Value='<%# Eval("InvStatus") %>'></asp:HiddenField>
                                                                                            <asp:HiddenField ID="hdnAStatus" runat="server" Value='<%# Eval("AStatus") %>'></asp:HiddenField>
                                                                                            <asp:HiddenField ID="txtBCodeType" runat="server" Value='<%# Eval("INVType") %>' />

                                                                                            <%--<asp:DropDownList ID="ddlBillingCode" runat="server" CssClass="browser-default" DataTextField="BillType" Style="text-align: center; display: none;"
                                                                                                DataValueField="ID" DataSource='<%#dtBillingCodeData%>' SelectedValue='<%# Eval("acct") %>' OnSelectedIndexChanged="ddlBillingCode_SelectedIndexChanged" AutoPostBack="true">
                                                                                            </asp:DropDownList>--%>

                                                                                            <asp:RequiredFieldValidator ID="rfvBillCode" runat="server" ControlToValidate="txtBillingCode"
                                                                                                Display="None" ErrorMessage="Billing Code Required" SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator>

                                                                                            <asp:ValidatorCalloutExtender ID="rfvBillCode_ValidatorCalloutExtender" runat="server"
                                                                                                Enabled="True" PopupPosition="TopLeft" TargetControlID="rfvBillCode">
                                                                                            </asp:ValidatorCalloutExtender>


                                                                                            <%-- Value='<%# Eval("Warehouse") %>'--%>

                                                                                            <asp:HiddenField ID="hdnWarehouse" runat="server" Value="OFC"></asp:HiddenField>

                                                                                            <%--  Value='<%# Eval("WHLocID") %>'--%>

                                                                                            <asp:HiddenField ID="hdnWarehouseLocationID" Value="0" runat="server"></asp:HiddenField>


                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="Description" HeaderStyle-Width="230" UniqueName="Description">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblDescription" Style="padding: 0px!important;" runat="server" Text='<%#Eval("fDesc").ToString().Replace("<br />","")%>' TextMode="MultiLine"
                                                                                                MaxLength="8000" CssClass="materialize-textarea"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="Price Per" UniqueName="Price" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="100" FooterStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblPricePer" runat="server" Text='<%# Eval("Price") != DBNull.Value ? Convert.ToDouble(Eval("Price")).ToString("N", System.Globalization.CultureInfo.InvariantCulture) : "" %>'
                                                                                                MaxLength="15" Style="text-align: center;"></asp:TextBox>
                                                                                            <asp:FilteredTextBoxExtender ID="lblPricePer_FilteredTextBoxExtender" runat="server"
                                                                                                Enabled="True" TargetControlID="lblPricePer" ValidChars="1234567890.-">
                                                                                            </asp:FilteredTextBoxExtender>
                                                                                            <asp:RequiredFieldValidator ID="rfvPricePer" runat="server" ControlToValidate="lblPricePer"
                                                                                                Display="None" Enabled="False" ErrorMessage="Price Per Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                            <asp:ValidatorCalloutExtender ID="rfvPricePer_ValidatorCalloutExtender" runat="server"
                                                                                                Enabled="True" PopupPosition="TopLeft" TargetControlID="rfvPricePer">
                                                                                            </asp:ValidatorCalloutExtender>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblPricePerTotal" runat="server" Style="display: none; width: 100%; text-align: center;"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="Pretax Amount" UniqueName="priceQuant" ItemStyle-CssClass="cent" FooterStyle-CssClass="cent" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="100" FooterStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label Style="font-weight: bold; color: #2392D7; text-align: center;" ID="lblPretaxAmount" runat="server"
                                                                                                Text='<%# Eval("priceQuant") != DBNull.Value ? Convert.ToDouble(Eval("priceQuant")).ToString("N", System.Globalization.CultureInfo.InvariantCulture) : "" %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblPretaxAmountTotal" runat="server"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                     <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="GST" UniqueName="EnableGSTTax"  ItemStyle-CssClass="left-align" HeaderStyle-CssClass="left-align"  ItemStyle-HorizontalAlign="Left"  ItemStyle-Width="100" HeaderStyle-Width="100" FooterStyle-Width="100">
                                                                                         <HeaderTemplate>
                                                                                        
                                                                                            <asp:CheckBox ID="chkSelectAllGtax" runat="server"/><span style="padding-left: 10px;" runat="server" id="lbGSTHeader">GST</span>
                                                                                        
                                                                                         </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkEnableGSTTax" runat="server" Checked='<%# ((System.Data.DataRowView)Container.DataItem).DataView.Table.Columns.Contains("EnableGSTTax")== true?Convert.ToString(DataBinder.Eval(Container.DataItem, "EnableGSTTax"))==""?false:Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "EnableGSTTax")) :false  %>'></asp:CheckBox>
                                                                                            
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="GST Tax" UniqueName="GTaxAmt" HeaderStyle-Width="100" FooterStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblGstTax" runat="server" Enabled="false"
                                                                                                Style="color: #2392D7; text-align: center;" Text='<%# Eval("GTaxAmt") != DBNull.Value ? Convert.ToDouble(Eval("GTaxAmt")).ToString("N", System.Globalization.CultureInfo.InvariantCulture) : "" %>'
                                                                                                MaxLength="15"></asp:TextBox>
                                                                                            <asp:FilteredTextBoxExtender ID="lblGstTax_FilteredTextBoxExtender" runat="server"
                                                                                                Enabled="True" TargetControlID="lblGstTax" ValidChars="1234567890.-">
                                                                                            </asp:FilteredTextBoxExtender>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblGstTaxTotal" runat="server" Style="text-align: center;"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderText="PST" UniqueName="stax" ItemStyle-CssClass="left-align" HeaderStyle-CssClass="left-align" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100" HeaderStyle-Width="100" FooterStyle-Width="100">
                                                                                          <HeaderTemplate>
                                                                                        
                                                                                            <asp:CheckBox ID="chkSelectAllStax" runat="server"/><asp:Label runat="server" id="lbPSTHeader" style="padding-left: 10px;">PST</asp:Label>
                                                                                        
                                                                                         </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkTaxable" runat="server" Checked='<%#Convert.ToBoolean(Eval("stax"))%>'></asp:CheckBox>
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>                                                                                 

                                                                                    <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" HeaderStyle-CssClass="stax-css" UniqueName="STaxAmt" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderStyle-Width="100" FooterStyle-Width="100">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="lblSalesTax" runat="server" Enabled="false"
                                                                                                Style="color: #2392D7; text-align: center;" Text='<%# Eval("STaxAmt") != DBNull.Value ? Convert.ToDouble(Eval("STaxAmt")).ToString("N", System.Globalization.CultureInfo.InvariantCulture) : "" %>'
                                                                                                MaxLength="15"></asp:TextBox>
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
                                                                                            <asp:Label Style="font-weight: bold; color: #2392D7; text-align: center;" ID="lblAmount" runat="server"
                                                                                                Text='<%# Eval("Amount") != DBNull.Value ? Convert.ToDouble(Eval("Amount")).ToString("N", System.Globalization.CultureInfo.InvariantCulture) : "" %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Label ID="lblAmountTotal" runat="server"></asp:Label>
                                                                                        </FooterTemplate>
                                                                                    </telerik:GridTemplateColumn>

                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                        <%-- </telerik:RadAjaxPanel>--%>
                                                                    </div>
                                                                </div>
                                                                <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                                                                    CausesValidation="False" UseSubmitBehavior="False" />
                                                                <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                                                                    TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                                                                    PopupDragHandleControlID="programmaticPopupDragHandle" BackgroundCssClass="pnlUpdateoverlay">
                                                                </asp:ModalPopupExtender>
                                                                <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; background: #fff; border: solid; width: 350px;">
                                                                    <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black; text-align: center;">
                                                                        <div class="title_bar_popup">
                                                                            <a id="hideModalPopupViaClientButton" href="#" style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px;">Cancel</a>
                                                                            <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Ok" OnClick="hideModalPopupViaServerConfirm_Click"
                                                                                CausesValidation="False" Style="float: right; margin-right: 20px; color: #fff; margin-left: 10px;" />
                                                                        </div>
                                                                    </asp:Panel>
                                                                    <div style="padding: 20px;">
                                                                        <strong>
                                                                            <asp:Label ID="lblCount" runat="server"></asp:Label></strong>
                                                                    </div>
                                                                </asp:Panel>
                                                            </div>
                                                            <div class="cf"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </li>


                                            <%--Payment History--%>

                                            <li id="tblPayment" runat="server" style="display: none">
                                                <div id="accrdPayment" class="collapsible-header accrd  accordian-text-custom"><i class="mdi-content-content-paste"></i>Payment History</div>
                                                <div class="collapsible-body">
                                                    <div class="form-content-wrap">
                                                        <div class="form-content-pd">
                                                            <div class="grid_container">
                                                                <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                                    <div class="RadGrid RadGrid_Material">

                                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvPayment" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_gvPayment" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" PagerStyle-AlwaysVisible="true"
                                                                                ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" AllowCustomPaging="True" OnNeedDataSource="RadGrid_gvPayment_NeedDataSource">
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
                                                                                                <asp:Label ID="lblPaymentdate" runat="server" Text='<%# Eval("PaymentDate", "{0:M/d/yyyy}")%>'></asp:Label>
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
                                                                                        <telerik:GridTemplateColumn DataField="Amount" AutoPostBackOnFilter="true" DataType="System.String"
                                                                                            CurrentFilterFunction="Contains" HeaderText="Amount" ShowFilterIcon="false" HeaderStyle-Width="200">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount",  "{0:c}") %>'></asp:Label>
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
                                            <li id="tbLogs" runat="server" style="display: none">
                                                <div id="accrdlogs" class="collapsible-header accrd  accordian-text-custom"><i class="mdi-content-content-paste"></i>Logs</div>
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
                    </div>
                </div>
            </div>
        </div>
        <div class="page-content" style="display: none;">
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="com-cont">
                        <div>
                            <div class="col-lg-12">
                                <div class="col-lg-4 col-md-4 form-group">
                                    <div id="dvCompanyPermission" runat="server">
                                        <div style="padding: 10px 0px 10px 0px;">
                                            Company
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- edit-tab end -->
                <div class="clearfix"></div>
            </div>
            <!-- END DASHBOARD STATS -->
            <div class="clearfix"></div>
        </div>
    </div>
    <%-------$$$$$$$$$$$$$$ RAD WINDOW $$$$$$$$$$$$$$$$-----%>
    <telerik:RadWindowManager ID="RadWindowManagerTicket" runat="server">
        <Windows>

            <telerik:RadWindow ID="RadWindowWareHouse" Skin="Material" VisibleTitlebar="true" Title="" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="600"
                runat="server" Modal="true" Width="600" Height="300">
                <ContentTemplate>

                    <div class="grid_container">
                        <div style="width: 40%; float: left;" class="RadGrid RadGrid_Material FormGrid">
                            <b>&nbsp;Warehouse </b>
                            <asp:TextBox ID="txtGvWarehouse" CssClass="Warehousesearchinput" runat="server" Text=""></asp:TextBox>
                        </div>
                        <div style="width: 40%; float: left;" class="RadGrid RadGrid_Material FormGrid">
                            <b>Location </b>
                            <asp:TextBox ID="lblWHLoc" runat="server" Text=""></asp:TextBox>
                        </div>

                    </div>

                </ContentTemplate>
            </telerik:RadWindow>

        </Windows>
    </telerik:RadWindowManager>

    <asp:HiddenField ID="hdntxtBCodeID" Value="" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hdnISInventoryTrackingON" Value="1" runat="server"></asp:HiddenField>
    <asp:HiddenField runat="server" ID="hdLineNo" />
    <asp:HiddenField runat="server" ID="hdnSelectPOIndex" />
    <%-- <asp:Button CausesValidation="false" ID="btnddlProjectCode" runat="server"
        Style="display: none;"
        UseSubmitBehavior="False" />--%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.maskedinput/1.4.1/jquery.maskedinput.min.js"></script>
    <script src="Design/js/moment.js"></script>



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

            Materialize.updateTextFields();

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });
        });

        function CalculateGridAmount() {
            try {
                // debugger;
                var grid = $find("<%= RadGrid_gvInvoices.ClientID %>");
                var masterTable = grid.get_masterTableView();
                var allItems = masterTable.get_dataItems();

                var hdnsTaxType = document.getElementById('<%=hdnsTaxType.ClientID%>');
                var stax = document.getElementById('<%=hdnStax.ClientID%>');
                var gtax = document.getElementById('<%=hdnGstTax.ClientID%>');

                if (masterTable.get_dataItems().length > 0) {
                    var PricePerTotal = 0;
                    var PretaxAmountTotal = 0;
                    var SalesTaxamountTotal = 0;
                    var GstTaxamountTotal = 0;
                    var AmountTotal = 0;
                    var isGst = 0;
                    $('#<%=RadGrid_gvInvoices.ClientID%>').find('.stax-css').each(function () {
                        if ($(this).html() == "Sales Tax Amount") {
                            isGst = 0;
                        }
                        else {
                            isGst = 1;
                        }
                    });
                    for (i = 0; i < masterTable.get_dataItems().length; i++) {
                        var Pretax = 0;
                        var staxAmt = 0;
                        var gtaxAmt = 0;
                        var Amount = 0;
                        var row = masterTable.get_dataItems()[i];
                        var cell = masterTable.getCellByColumnUniqueName(row, "Quan");
                        var cell1 = masterTable.getCellByColumnUniqueName(row, "Price");
                        var cell3 = masterTable.getCellByColumnUniqueName(row, "priceQuant");
                        var cell5 = masterTable.getCellByColumnUniqueName(row, "stax"); //check box PST
                        var cell2 = masterTable.getCellByColumnUniqueName(row, "STaxAmt");  //Pst amount                  
                        var cell7 = masterTable.getCellByColumnUniqueName(row, "EnableGSTTax");//check box GST
                        var cell4;
                        var cell6;
                        if (isGst == 0) {
                            cell4 = masterTable.getCellByColumnUniqueName(row, "Amount");
                        }
                        else {
                            cell6 = masterTable.getCellByColumnUniqueName(row, "GTaxAmt"); //GST Amount
                            cell4 = masterTable.getCellByColumnUniqueName(row, "Amount");
                        }

                        if (cell.childNodes[1].value != '' && cell1.childNodes[1].value != '') {
                            pretax = parseFloat((parseFloat(cell.childNodes[1].value.toString().replace(/[\$\(\),]/g, '')) * parseFloat(cell1.childNodes[1].value.toString().replace(/[\$\(\),]/g, '')))).toFixed(2);
                            cell3.childNodes[1].innerHTML = pretax.toLocaleString("en-US", { minimumFractionDigits: 2 });
                        }
                        //GST

                        debugger
                         gtaxAmt = 0.00;
                        if (isGst == 1) {
                            if (hdnsTaxType.value == '2') {
                                gtaxAmt = 0.00;
                               
                            } else {

                                 if (cell7.childNodes[1].checked == true) {
                                        gtaxAmt =parseFloat(pretax) * parseFloat(gtax.value.toString().replace(/[\$\(\),]/g, '')) / 100;
                                      
                                } 

                                //if (hdnsTaxType.value == '1') {
                                //    if (cell5.childNodes[1].checked == true) {
                                //        gtaxAmt = Math.round(((parseFloat(pretax) * parseFloat(gtax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                                      
                                //    } 
                                //} else {
                                //    if (cell7.childNodes[1].checked == true) {
                                //        gtaxAmt = Math.round(((parseFloat(pretax) * parseFloat(gtax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                                      
                                //    } 
                                //}
                            }
                            if (cell6 != null) {
                                cell6.childNodes[1].value = (parseFloat(gtaxAmt).toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 });
                            }
                             
                            //if (hdnsTaxType.value != '2') {
                                
                            //    if (gtax.value != '' && cell.childNodes[1].value != '' && cell1.childNodes[1].value != '') {
                            //        if (cell7.childNodes[1].checked == true) {
                            //            gtaxAmt = Math.round(((parseFloat(pretax) * parseFloat(gtax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                            //            cell6.childNodes[1].value = gtaxAmt.toLocaleString("en-US", { minimumFractionDigits: 2 });
                            //        }
                            //        else {
                            //            gtaxAmt = 0.00;
                            //        }
                            //        cell6.childNodes[1].value = gtaxAmt.toLocaleString("en-US", { minimumFractionDigits: 2 });
                            //    }
                            //} else {
                            //    gtaxAmt = 0.00;
                            //    cell6.childNodes[1].value = gtaxAmt.toLocaleString("en-US", { minimumFractionDigits: 2 });
                            //}

                        }




                        if (hdnsTaxType.value == '3') {
                            staxAmt = 0.00;
                            gtaxAmt = 0.00;
                        } else {
                            if (stax.value != '' && cell.childNodes[1].value != '' && cell1.childNodes[1].value != '') {
                                staxAmt = 0.00;
                                if (cell5.childNodes[1].checked == true) {
                                    if (hdnsTaxType.value == '1') {
                                        // Compound tax
                                        if (parseFloat(pretax) < 0) {
                                            //staxAmt = Math.round(((parseFloat(pretax * (-1) + gtaxAmt *(-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, ''))) / 100) * 100) / 100;
                                            staxAmt = parseFloat(parseFloat(pretax * (-1)) + parseFloat(gtaxAmt * (-1))) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, '')) / 100;
                                            staxAmt = staxAmt * (-1);
                                        } else {
                                            staxAmt = parseFloat(parseFloat(pretax) + parseFloat(gtaxAmt)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, '')) / 100;
                                        }

                                    } else {
                                        if (parseFloat(pretax) < 0) {
                                            staxAmt = parseFloat(pretax * (-1)) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, '')) / 100;
                                            staxAmt = staxAmt * (-1);
                                        } else {
                                            staxAmt = parseFloat(pretax) * parseFloat(stax.value.toString().replace(/[\$\(\),]/g, '')) / 100;
                                        }
                                    }
                                }
                                else {
                                    staxAmt = 0.00;
                                }
                                cell2.childNodes[1].value = (parseFloat(staxAmt).toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 });

                            }
                        }
                       
                        
                        


                        if (cell.childNodes[1].value != '' && cell1.childNodes[1].value != '') {
                            total = parseFloat(pretax) + parseFloat(staxAmt);
                            if (isGst == 1) {
                                total = total + gtaxAmt;
                            }
                            cell4.childNodes[1].innerHTML = (parseFloat(total).toFixed(2)).toLocaleString("en-US", { minimumFractionDigits: 2 });
                        }
                        if (cell1.childNodes[1].value != '') {
                            PricePerTotal =parseFloat( (parseFloat(PricePerTotal) + parseFloat(cell1.childNodes[1].value.toString().replace(/[\$\(\),]/g, '')))).toFixed(2) ;
                        }
                        if (cell3.childNodes[1].innerHTML != '') {
                            PretaxAmountTotal = parseFloat((parseFloat(PretaxAmountTotal) + parseFloat(cell3.childNodes[1].innerHTML.toString().replace(/[\$\(\),]/g, '')))).toFixed(2);
                        }
                        if (cell2 != null) {
                            if (cell2.childNodes[1].value != '') {

                            SalesTaxamountTotal =parseFloat((parseFloat(SalesTaxamountTotal) + parseFloat(cell2.childNodes[1].value.toString().replace(/[\$\(\),]/g, '')))).toFixed(2);
                        }
                        }
                        
                        if (cell4.childNodes[1].innerHTML != '') {
                            AmountTotal = parseFloat((parseFloat(AmountTotal) + parseFloat(cell4.childNodes[1].innerHTML.toString().replace(/[\$\(\),]/g, '')))).toFixed(2);
                        }
                        if (isGst == 1) {
                            if (cell6 != null) {
                                if (cell6.childNodes[1].value != '') {
                                    GstTaxamountTotal = parseFloat((parseFloat(GstTaxamountTotal) + parseFloat(cell6.childNodes[1].value.toString().replace(/[\$\(\),]/g, '')))).toFixed(2);
                                }
                            }
                            
                        }
                    }
                    var grid = $find("<%=RadGrid_gvInvoices.ClientID %>");
                    var MasterTable = grid.get_masterTableView();
                    var footerTable = grid.get_masterTableViewFooter().get_element();
                    var lblPricePerTotal = $telerik.findElement(footerTable, "lblPricePerTotal");
                    lblPricePerTotal.innerHTML = PricePerTotal.toLocaleString("en-US", { minimumFractionDigits: 2 });
                    var lblPretaxAmountTotal = $telerik.findElement(footerTable, "lblPretaxAmountTotal");                   
                    lblPretaxAmountTotal.innerHTML = PretaxAmountTotal.toLocaleString("en-US", { minimumFractionDigits: 2 });

                    var lblSalesTaxTotal = $telerik.findElement(footerTable, "lblSalesTaxTotal");
                    if (lblSalesTaxTotal != null) {
                        lblSalesTaxTotal.innerHTML = SalesTaxamountTotal.toLocaleString("en-US", { minimumFractionDigits: 2 });
                    }
                    
                    var lblAmountTotal = $telerik.findElement(footerTable, "lblAmountTotal");
                    lblAmountTotal.innerHTML = AmountTotal.toLocaleString("en-US", { minimumFractionDigits: 2 });
                    if (isGst == 1) {
                        var lblGstTaxTotal = $telerik.findElement(footerTable, "lblGstTaxTotal");
                        lblGstTaxTotal.innerHTML = GstTaxamountTotal.toLocaleString("en-US", { minimumFractionDigits: 2 });
                    }
                }
            } catch (e) {

            }

        }
    </script>
    <script>
        function AddNewRows(sender, eventArgs) {

            var $focused = $(':focus');
            var flag = 0
            try {
                if ($focused[0].id.indexOf("ddlProjectCode") !== -1 || $focused[0].id.indexOf("txtBillingCode") !== -1) {
                    flag = 1
                }
            } catch (ex) {
                flag = 0;
            }

            if (eventArgs.get_keyCode() == 40) {
                if (flag == 0) {
                    document.getElementById('<%=lnkAddnew.ClientID%>').click();
                }

            }

        }
        function pageLoad(sender, args) {


            Materialize.updateTextFields();
            $(window.document).keydown(function (event) {

                if (event.which == 117) {
                    document.getElementById('<%=btnCopyPrevious.ClientID%>').click();
                     return false;
                 }
             })

             $("[id*=ddlProjectCode]").focus(function () {

                 var ddlProjectCode = $(this).attr('id');
                 var hdnIndex = document.getElementById(ddlProjectCode.replace('ddlProjectCode', 'hdnIndex'));
                 var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
                 $(hdnSelectPOIndex).val(hdnIndex.value);
             });
             $("[id*=lblQuantity]").focus(function () {
                 var lblQuantity = $(this).attr('id');
                 var hdnIndex = document.getElementById(lblQuantity.replace('lblQuantity', 'hdnIndex'));
                 var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
                 $(hdnSelectPOIndex).val(hdnIndex.value);

             });
             $("[id*=txtBillingCode]").focus(function () {
                 var txtBillingCode = $(this).attr('id');
                 var hdnIndex = document.getElementById(txtBillingCode.replace('txtBillingCode', 'hdnIndex'));
                 var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
                 $(hdnSelectPOIndex).val(hdnIndex.value);

             });
             $("[id*=lblDescription]").focus(function () {
                 var lblDescription = $(this).attr('id');
                 var hdnIndex = document.getElementById(lblDescription.replace('lblDescription', 'hdnIndex'));
                 var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
                 $(hdnSelectPOIndex).val(hdnIndex.value);

             });
             $("[id*=lblPricePer]").focus(function () {
                 var lblPricePer = $(this).attr('id');
                 var hdnIndex = document.getElementById(lblPricePer.replace('lblPricePer', 'hdnIndex'));
                 var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
                 $(hdnSelectPOIndex).val(hdnIndex.value);

             });
             function dtaa() {
                 this.prefixText = null;
                 this.con = document.getElementById('<%=hdnCon.ClientID%>').value;
                 this.custID = null;
             }
             var query = "";
             $("#<%=txtCustomer.ClientID%>").autocomplete({

                 source: function (request, response) {
                     var dtaaa = new dtaa();
                     dtaaa.prefixText = request.term;
                     query = request.term;
                     $.ajax({
                         type: "POST",
                         contentType: "application/json; charset=utf-8",
                         url: "CustomerAuto.asmx/GetCustomer",
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
                     $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                     $("#<%=hdnPatientId.ClientID%>").val(ui.item.value);
                     $("#<%=txtLocation.ClientID%>").focus();
                     $("#<%=txtLocation.ClientID%>").val('');
                     $("#<%=hdnLocId.ClientID%>").val('');
                     document.getElementById('<%=btnSelectCustomer.ClientID%>').click();
                     Materialize.updateTextFields();
                     return false;
                 },
                 focus: function (event, ui) {
                     $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                     return false;
                 },
                 minLength: 0,
                 delay: 50
             })
                 .data("ui-autocomplete")._renderItem = function (ul, item) {
                     var result_item = item.label;
                     var result_desc = item.desc;
                     var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here... 

                     try {
                         if (result_item) {
                             result_item = result_item.replace(x, function (FullMatch, n) {
                                 return '<span class="highlight">' + FullMatch + '</span>'
                             });
                         }
                         if (result_desc != null) {
                             result_desc = result_desc.replace(x, function (FullMatch, n) {
                                 return '<span class="highlight">' + FullMatch + '</span>'
                             });
                         }
                     } catch (e) { }

                     if (result_item) {
                         return $("<li></li>")
                             .data("item.autocomplete", item)
                             .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                             .appendTo(ul)
                     }
                 };
             ///////////// Ajax call for location auto search ////////////////////
             var queryloc = "";
             $("#<%=txtLocation.ClientID%>").autocomplete(
                 {

                     source: function (request, response) {
                         var dtaaa = new dtaa();
                         dtaaa.prefixText = request.term;
                         dtaaa.custID = 0;
                         if (document.getElementById('<%=hdnPatientId.ClientID%>').value != '') {
                             dtaaa.custID = document.getElementById('<%=hdnPatientId.ClientID%>').value;
                         }
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
                                 $("#divProject").slideUp();
                             },
                             error: function (result) {
                                 alert("Due to unexpected errors we were unable to load customers");
                             }
                         });
                     },
                     select: function (event, ui) {
                         $("#<%=txtLocation.ClientID%>").val(ui.item.label);
                            $("#<%=hdnLocId.ClientID%>").val(ui.item.value);
                            $("#<%=hdnIsProjectSearch.ClientID%>").val('0');
                            document.getElementById('<%=btnSelectLoc.ClientID%>').click();
                         Materialize.updateTextFields();
                         return false;
                     },
                     focus: function (event, ui) {
                         $("#<%=txtLocation.ClientID%>").val(ui.item.label);
                         return false;
                     },
                     minLength: 0,
                     delay: 50
                 })
                 .data("ui-autocomplete")._renderItem = function (ul, item) {
                     var result_item = item.label;
                     var result_desc = item.desc;
                     var x = new RegExp('\\b' + queryloc, 'ig'); // notice the escape \ here...    

                     try {
                         if (result_item) {
                             result_item = result_item.replace(x, function (FullMatch, n) {
                                 return '<span class="highlight">' + FullMatch + '</span>'
                             });
                         }

                         if (result_desc != null) {
                             result_desc = result_desc.replace(x, function (FullMatch, n) {
                                 return '<span class="highlight">' + FullMatch + '</span>'
                             });
                         }

                     } catch (e) { }

                     if (result_item) {
                         return $("<li></li>")
                             .data("item.autocomplete", item)
                             .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                             .appendTo(ul);
                     }


                 };
             $("#<%=txtCustomer.ClientID%>").keyup(function (event) {
                 var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
                 if (document.getElementById('<%=txtCustomer.ClientID%>').value == '') {
                     hdnPatientId.value = '';
                 }
             });
             $("#<%=txtLocation.ClientID%>").keyup(function (event) {
                 var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
                 if (document.getElementById('<%=txtLocation.ClientID%>').value == '') {
                     hdnLocId.value = '';
                 }
             });
             //debugger;
             $("#divProject").slideUp();
             $("#<%=txtProject.ClientID%>").focusin(function () {
                 // debugger;
                 $("#divProject").slideDown();
             });
             $("#<%=txtProject.ClientID%>").focusout(function () {
                 // debugger;
                 $("#divProject").slideUp();
             });
             $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);
             Materialize.updateTextFields();
             CalculateGridAmount();

             $("[id$='RadGrid_gvInvoices']").sortable({
                 items: 'tr.rgRow, tr.rgAltRow',
                 handle: ".handle",
                 cursor: 'move',
                 axis: 'y',
                 dropOnEmpty: false,
                 start: function (e, ui) {
                     ui.item.addClass("selected");
                 },
                 stop: function (e, ui) {
                     ui.item.removeClass("selected", 1000, "swing");
                     updateInvoicegridindex();
                 },
                 receive: function (e, ui) {
                     $(this).find("tbody").append(ui.item);
                 }
             });
             $("#<%=txtInvoiceDate.ClientID %>").pikaday({
                 firstDay: 0,
                 format: 'MM/DD/YYYY',
                 minDate: new Date(1900, 1, 1),
                 maxDate: new Date(2100, 12, 31),
                 yearRange: [1900, 2100]
             });
             $("#<%=txtDueDate.ClientID %>").pikaday({
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(1900, 1, 1),
                maxDate: new Date(2100, 12, 31),
                yearRange: [1900, 2100]
            });

             $("[id*=chkSelectAllGtax]").change(function () {
               // debugger;
                var ret = $(this).prop('checked');
                
                $("#<%=RadGrid_gvInvoices.ClientID %>").find('tr:not(:first, :last)').each(function () {
                    var $tr = $(this);
                    var chk = $tr.find('input[id*=chkEnableGSTTax]');
                    if (ret == true) {
                        chk.prop('checked', true);
                    }
                    else {
                        chk.prop('checked', false);
                    }
                    
                    var ch_id = chk.attr('id');
                    if (ch_id != undefined) {
                        //if (ch_id != "ctl00_ContentPlaceHolder1_gvBills_ctl00_ctl02_ctl00_chkSelectAllGtax") {
                        //     CalculateGridAmount();
                        //}
                        if (ch_id.includes("chkEnableGSTTax", 0)) {
                            CalculateGridAmount();
                        }
                    }

                })

            });
            $("[id*=chkSelectAllStax]").change(function () {
               // debugger;
                var ret = $(this).prop('checked');

                $("#<%=RadGrid_gvInvoices.ClientID %>").find('tr:not(:first, :last)').each(function () {
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
                        //if (ch_id != "ctl00_ContentPlaceHolder1_gvBills_ctl00_ctl02_ctl00_chkSelectAllStax") {
                        //    CalculateGridAmount();
                        //}
                        if (ch_id.includes("chkTaxable", 0)) {
                            CalculateGridAmount();
                        }
                    }

                })

            });

            function updateInvoicegridindex() {
                var grid1 = 1;
                $("[id$='RadGrid_gvInvoices']  tbody > tr").each(function () {
                    var cat = $(this).find('input:hidden[id*="hdnIndex"]');
                   // debugger
                    if (cat !== null && cat.length > 0) {
                        cat.val(grid1);
                        grid1 = grid1 + 1;
                    }
                });
            }
        }




        function resetIndexF6() {
            var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
            $(hdnSelectPOIndex).val(-1);
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
        function warningAcctMessage() {
            noty({
                text: 'This Account\\Billing code is inactive. Please change account\\billing code name before proceeding.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 4000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function confirmDelete() {
            var isDisabled = $("#<%=btnDelete.ClientID%>").attr('disabled');

            if (isDisabled) {
                return false;
            } else {
                return confirm('Are you sure you want to delete this record?');
            }

        }

        function addDays(date, days) {
            var result = new Date(date);
            result.setDate(result.getDate() + days);
            var day = result.getDate();
            var month = result.getMonth() + 1;
            var year = result.getFullYear();
            var datestring = month + "/" + day + "/" + year;
            return datestring;
        }
        function UpdateDueByTerms() {
            debugger
            var ddlTerms = $("#<%=ddlTerms.ClientID%>");
             var txtDueDate = $("#<%=txtDueDate.ClientID%>");
             var txtInvoiceDate = $("#<%=txtInvoiceDate.ClientID%>");

            if (ddlTerms.val() == "0") {
                txtDueDate.Text = txtInvoiceDate.val();
            }
            else if (ddlTerms.val() == "1") {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 10));
            }
            else if (ddlTerms.val() == "2") {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 15));
            }
            else if (ddlTerms.val() == "3") {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 30));
            }
            else if (ddlTerms.val() == "4") {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 45));
            }
            else if (ddlTerms.val() == "5") {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 60));
            }
            else if (ddlTerms.val() == "6") {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 30));
            }
            else if (ddlTerms.val() == "7") {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 90));
            }
            else if (ddlTerms.val() == "8") {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 180));
            }
            else if (ddlTerms.val() == "9") {
                txtDueDate.val(txtInvoiceDate.val());
            }
            else if (ddlTerms.val() == "10") //120 days
            {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 120));
            }
            else if (ddlTerms.val() == "11") //150 days
            {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 150));
            }
            else if (ddlTerms.val() == "12") //210 days
            {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 210));
            }
            else if (ddlTerms.val() == "13") //240 days
            {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 240));
            }
            else if (ddlTerms.val() == "14") //270 days
            {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 270));
            }
            else if (ddlTerms.val() == "15") //300 days
            {
                txtDueDate.val(addDays(txtInvoiceDate.val(), 300));
            }
            else if (ddlTerms.val() == "16") //net due on 10th
            {
                txtDueDate.val("");
            }
            else if (ddlTerms.val() == "17") //net due
            {
                txtDueDate.val("");
            }
            else if (ddlTerms.val() == "18") //Credit card
            {
                txtDueDate.val("");
            }
            Materialize.updateTextFields();
        }

    </script>
</asp:Content>
