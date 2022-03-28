<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddPO" CodeBehind="AddPO.aspx.cs" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>--%>
    <script type="text/javascript" src="js/Signature/jquery.signaturepad.js"></script>
    <link rel="stylesheet" href="js/Signature/jquery.signaturepad.css" />
    <style>
        .highlight {
            background-color: Yellow;
        }

        .highlighted {
            background-color: Yellow;
        }

        .ui-state-hover, .ui-state-active {
            text-decoration: none !important;
            background-color: transparent !important;
            border-radius: 4px !important;
            -webkit-border-radius: 4px !important;
            -moz-border-radius: 4px !important;
            background-image: none !important;
            width: 100%;
            border: none !important;
        }

        .signature {
            float: left;
            width: 100%;
            margin-top: 25px;
        }

            .signature #signbg {
                width: 100%;
                height: 100px;
                border: 1px solid #000;
                margin-top: 7px;
            }

                .signature #signbg img {
                    width: 100%;
                    height: 100%;
                }

        .sigPad {
            float: left;
            margin-top: 15px;
            width: 100%;
        }

        .sign-title {
            float: left;
            width: 100%;
            padding: 5px 10px;
            background: #316b9d;
        }

            .sign-title .sign-title-l {
                float: left;
                color: #fff;
                cursor: pointer;
            }

            .sign-title .sign-title-r {
                float: right;
                color: #fff;
                cursor: pointer;
            }

        .sigPad .pad {
            width: 100%;
        }

        .RadGrid_Material .rgHeader {
            color: #2e6b89 !important;
            font-weight: bold !important;
        }

        ul.anchor-links li a {
            border-bottom: 1px groove !important;
        }

        .dropdown-content.po-report-dropdown {
            width: auto !important;
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

    <script type="text/javascript">
        //// ES-1708 Mitsu - Approve PO MOM changes
        function MitsuChanges() {         
            
            var hdnCustomProgram = $('#hdnCustomProgram').val();
            
            if (hdnCustomProgram != '' && hdnCustomProgram.toLowerCase() == 'mitsu') {
                document.getElementById('<%= lblComments.ClientID %>').innerHTML = 'PO Description';
  

            } else if (hdnCustomProgram == '' || hdnCustomProgram.toLowerCase() != 'mitsu'){
                 document.getElementById('<%= lblComments.ClientID %>').innerHTML = 'Comments';
            }
        }


        function GetClientUTC() {
            var now = new Date();
            var offset = now.getTimezoneOffset();
            document.getElementById('<%= txtDate.ClientID %>').value = (now.getMonth() + 1) + "/" + now.getDate() + "/" + now.getFullYear();
            var due = new Date(now.setTime(now.getTime() + 30 * 86400000));
            document.getElementById('<%= txtDueDate.ClientID %>').value = (due.getMonth() + 1) + "/" + due.getDate() + "/" + due.getFullYear();
        }

        function HideGridColums(val) {
            var chkValue = false;
            if (val == "true") {
                chkValue = true;

            }

            //var th_warehouse = $("[id*=gvGLItems] th:contains('Warehouse')");
            //var th_location = $("[id*=gvGLItems] th:contains('Location')");

            var th_warehouse = $("[id*=RadGrid_AddPO] th:contains('Warehouse')");
            // var th_location = $("[id*=RadGrid_AddPO] th:contains('Location')");



            th_warehouse.css("display", chkValue ? "" : "none");
            //th_location.css("display", chkValue ? "" : "none");

            //$("[id*=gvGLItems] tr").each(function () {
            $("[id*=RadGrid_AddPO] tr").each(function () {
                $(this).find("td").eq(th_warehouse.index()).css("display", chkValue ? "" : "none");
                //$(this).find("td").eq(th_location.index()).css("display", chkValue ? "" : "none");
            });


        }

        function HideGridTicketColums(val) {

            var chkValue = false;
            if (val == "true") {
                chkValue = true;

            }

            //var th_Ticket = $("[id*=gvGLItems] th:contains('Ticket')");
            var th_Ticket = $("[id*=RadGrid_AddPO] th:contains('Ticket')");



            th_Ticket.css("display", chkValue ? "" : "none");

            //$("[id*=gvGLItems] tr").each(function () {
            $("[id*=RadGrid_AddPO] tr").each(function () {
                $(this).find("td").eq(th_Ticket.index()).css("display", chkValue ? "" : "none");

            });


        }

       

        function itemJSON() {

                <%-- var rawData = $('#<%=gvGLItems.ClientID%>').serializeFormJSON();--%>
                var rawData = $('#<%=RadGrid_AddPO.ClientID%>').serializeFormJSON();
                var formData = JSON.stringify(rawData);
                $('#<%=hdnItemJSON.ClientID%>').val(formData);
           
        }
        

        $(document).ready(function () {
            <%--debugger
            $('<%=btnSubmit.ClientID%>').on('click',function(){
                $('<%=btnSubmit.ClientID%>').off('click');
            });--%>
            //HideGridColums("false");

            //HideGridTicketColums("false");

            $('#ui-active-menuitem').click(function () {
                $('.ui-menu-item').removeClass('ui-autocomplete-loading');
            });

            function dta() {
                this.prefixText = null;
                //this.con = null;
            }

            <%-- InitializeGrids('<%=gvGLItems.ClientID%>');--%>
            InitializeGrids('<%=RadGrid_AddPO.ClientID%>');


            ///////////// Ajax call for vendor auto search ////////////////////                
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.Acct = null;
            }
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
                    try {
                        $(".ui-autocomplete").mCustomScrollbar("destroy");
                    } catch (e){ }
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
                            debugger
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

                        $("#<%=ddlTerms.ClientID%>").val(ui.item.Terms);
                        $("#<%=txtVendorType.ClientID%>").val(ui.item.VendorType);
$("#<%=txtCourrierAcct.ClientID%>").val(ui.item.CourierAccount);
                        
                        UpdateDueByTerms();
                        document.getElementById('<%=btnSelectVendor.ClientID%>').click();
                    }

                    Materialize.updateTextFields();
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

                //var result_desc = item.acct;
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
                        .data("item.autocomplete", item)
                        .append("<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>")
                        .appendTo(ul);
                }
                else {
                    var append_data = "";
                    //Premission Check.....
                    var chk = '<%=Convert.ToString(Session["COPer"]) %>';
                    if (chk == "1") {
                        append_data = "<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>" + "<span class='con_hide' style='color:Gray;'> ," + result_Company + "</span>";
                    }
                    else {
                        append_data = "<span class='auto_item'>" + result_item + "</span> <span class='auto_desc'>" + result_desc + "</span>";
                    }

                    return $("<li ></li>")
                        .data("item.autocomplete", item)
                        .append(append_data)
                        .appendTo(ul);
                }
            };

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

            ///////////// Custom validator function for vendor auto search  ////////////////////
            function ChkVendor(sender, args) {
                var hdnVendorId = document.getElementById('<%=hdnVendorID.ClientID%>');
                if (hdnVendorId.value == '') {
                    args.IsValid = false;
                }
            }

            function clearPhase(txt) {
                try {
                    if ($(txt).val() == '') {
                        var hdnPID = document.getElementById(txt.id.replace('txtGvPhase', 'hdnPID'));
                        $(hdnPID).val('0');
                    }
                } catch (e){ }
            }
            
            function dtaa() {
                this.prefixText = null;
                this.con = null;
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
            function RedirectTicketScreen(ticketid, comp) {
                var str = 'addticket.aspx?id=' + ticketid + '&comp=' + comp + '&pop=1';
                //console.log(str);
                if (str != null) {
                    //console.log("2_"+str);
                    setTimeout(window.location.href = str, 3000);
                }
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
            });

            $("[id*=txtCode]").autocomplete({

                source: function (request, response) {

                    var curr_control = this.element.attr('id');
                    var job = document.getElementById(curr_control.replace('txtCode', 'hdnmainjobID'));
                    var prefixText = request.term;
                    var job = document.getElementById(job.id).value;
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
                    var txtCode = this.id;
                    var hdnMainCode = document.getElementById(txtCode.replace('txtCode', 'hdnMainCode'));
                    var hdOpSq = document.getElementById(txtCode.replace('txtCode', 'hdOpSq'));

                    var str = ui.item.TypeName;
                    if (str == "No Record Found!") {
                        $(this).val("");
                    }
                    else {
                        $(hdnMainCode).val(ui.item.Type);
                        $(hdOpSq).val(ui.item.Code);
                        $(this).val(ui.item.TypeName);
                        if (ui.item.TypeName == "Inventory") {
                            HideGridColums("true");
                            //do inventory default account
                            var txtGvAcctNo = document.getElementById(txtCode.replace('txtCode', 'txtGvAcctNo'));
                            var hdnAcctID = document.getElementById(txtCode.replace('txtCode', 'hdnAcctID'));
                            $(txtGvAcctNo).val(document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value);
                            $(hdnAcctID).val(document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value);
                            //  var txtGvWarehouse = document.getElementById(txtCode.replace('txtCode', 'txtGvWarehouse'));
                            //var txtGvWarehouseLocation = document.getElementById(txtCode.replace('txtCode', 'txtGvWarehouseLocation')); 
                            //txtGvWarehouse.readOnly = false;
                            //txtGvWarehouseLocation.readOnly = false; 
                        }
                        else {
                            // HideGridColums("false");
                            //  var txtGvWarehouse = document.getElementById(txtCode.replace('txtCode', 'txtGvWarehouse'));
                            //var txtGvWarehouseLocation = document.getElementById(txtCode.replace('txtCode', 'txtGvWarehouseLocation')); 
                            //txtGvWarehouse.readOnly = true;
                            //txtGvWarehouseLocation.readOnly = true; 
                            //   $(txtGvWarehouse).val('');
                            //$(txtGvWarehouseLocation).val('');
                        }
                    }
                    CopyProjctCode();
                    return false;
                },
                focus: function (event, ui) {

                    $(this).val(ui.item.TypeName);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".pcodesearchinput"), function (index, item) {

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

            //txtCode
            //$("[id*=txtCode]").change(function () {
            $("[id*=txtCode]").unbind("change").bind("change",function () {
                var txtCode = $(this);
                var strPhase = $(this).val();

                var txtCode1 = $(txtCode).attr('id');
                var hdnTypeId = document.getElementById(txtCode1.replace('txtCode', 'hdnTypeId'));
                var hdnPID = document.getElementById(txtCode1.replace('txtCode', 'hdnPID'));
                var txtGvItem = document.getElementById(txtCode1.replace('txtCode', 'txtGvItem'));
                var hdnItemID = document.getElementById(txtCode1.replace('txtCode', 'hdnItemID'));
                var txtGvDesc = document.getElementById(txtCode1.replace('txtCode', 'txtGvDesc'));

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
                                $(txtCode).val('');
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
                                $(txtCode).val(lbl);
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

            ////txtProject
            $("[id*=txtProject]").autocomplete({

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
                    var txtProject = this.id;
                    var hdMainGvLoc = document.getElementById(txtProject.replace('txtProject', 'hdMainGvLoc'));
                    var hdnmainjobID = document.getElementById(txtProject.replace('txtProject', 'hdnmainjobID'));
                    var hdMainGvAcctNo = document.getElementById(txtProject.replace('txtProject', 'hdMainGvAcctNo'));
                    var hdMainAcctID = document.getElementById(txtProject.replace('txtProject', 'hdMainAcctID'));

                    

                    
                        $(hdnmainjobID).val(ui.item.ID);
                        var jobStr = ui.item.ID + ", " + ui.item.fDesc;
                        $(this).val(jobStr);

                        $(hdMainGvLoc).val(ui.item.Tag);
                        $(hdMainAcctID).val(ui.item.GLExp);
                        var strAcct = ui.item.Acct + ' - ' + ui.item.DefaultAcct
                        $(hdMainGvAcctNo).val(strAcct);

                        CopyProjctCode();
                   
                    return false;
                },
                change: function (event, ui) {
                    
                    if (!ui.item) {
                        $(this).val("");
                        return false;
                    }
                    var txtProject = this.id;
                    var hdnJobID = document.getElementById(txtProject.replace('txtProject', 'hdnmainjobID'));
                    var strJob = document.getElementById(txtProject).value;

                    if (strJob == '') {
                        $(hdnJobID).val('')
                    }
                },
                focus: function (event, ui) {
                    
                    var jobStr = ui.item.ID + ", " + ui.item.fDesc;
                    $(this).val(jobStr);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                var ula = ul;
                var itema = item;
                var result_value = item.ID;
                var result_item = item.fDesc;
                var result_desc = item.Tag;
                var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                result_item = result_item.replace(x, function (FullMatch, n) {
                    return '<span class="highlight">' + FullMatch + '</span>'
                });

                //if (result_value != null) {
                //    result_value = result_value.toString().replace(x, function (FullMatch, n) {
                //        return '<span class="highlight">' + FullMatch + '</span>'
                //    });
                //}

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

            //$.each($(".pmainsearchinput"), function (index, item) {
            //    $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
            //        var ula = ul;
            //        var itema = item;
            //        var result_value = item.ID;
            //        var result_item = item.fDesc;
            //        var result_desc = item.Tag;
            //        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
            //        result_item = result_item.replace(x, function (FullMatch, n) {
            //            return '<span class="highlight">' + FullMatch + '</span>'
            //        });

            //        //if (result_value != null) {
            //        //    result_value = result_value.toString().replace(x, function (FullMatch, n) {
            //        //        return '<span class="highlight">' + FullMatch + '</span>'
            //        //    });
            //        //}

            //        if (result_desc != null) {
            //            result_desc = result_desc.replace(x, function (FullMatch, n) {
            //                return '<span class="highlight">' + FullMatch + '</span>'
            //            });
            //        }
            //        if (result_value == 0) {
            //            return $("<li></li>")
            //                .data("item.autocomplete", item)
            //                .append("<a>" + result_item + "</a>")
            //                .appendTo(ul);
            //        }
            //        else {
            //            return $("<li></li>")
            //                .data("item.autocomplete", item)
            //                .append("<a><b> Project: </b> " + result_value + ", " + result_item + ", <span style='color:Gray;'><b> Loc: </b>" + result_desc + "</span></a>")
            //                .appendTo(ul);
            //        }
            //    };
            //});

            if ('<%=Request.QueryString["id"]%>' != "" && '<%=Request.QueryString["id"]%>' != null) {
                $('.sigPad').signaturePad();
                jQuery('.sign-title-l').click(function () {
                    jQuery('#hdnDrawdata').val("");
                });
                $("#signbg").click(function () {
                    if (isCanvasSupported()) {
                        $("#sign").toggle();
                        $("#sign").focus();
                    }
                    else {
                        alert('Signature not supported on this web browser.');
                    }
                });
                $("#sign").blur(function () {
                    $("#sign").hide();
                });
                $("#convertpngbtn").click(function () {
                    $("#sign").hide();
                    toImage();
                });
                var oImg = document.getElementById("<%=imgSign.ClientID%>");
                var ImgHdn = document.getElementById("<%=hdnImg.ClientID%>");
                if (ImgHdn != null)
                    oImg.src = ImgHdn.value;
            }

            MitsuChanges();

            
        });
        function isCanvasSupported() {
            var elem = document.createElement('canvas');
            return !!(elem.getContext && elem.getContext('2d'));
        }
        ///////////////////////////////    Convert signature to image      ////////////////////////////////
        function toImage() {
            var hdnDrawdata = document.getElementById("hdnDrawdata");
            var hdnImg = document.getElementById("<%=hdnImg.ClientID%>");
            var oImgElement = document.getElementById("<%=imgSign.ClientID%>");
            var canvas = document.getElementById("canvas");
            if (hdnDrawdata.value != "") {
                var img = canvas.toDataURL("image/png");
                oImgElement.src = img;
                hdnImg.value = img;
            }
        }
  
        function ResetBom() {
            $("#<%=txtBudgetUnit.ClientID%>").val('0.00');
            $("#<%=lblBudgetExt.ClientID%>").val('0.00');
            $("#<%=txtQty.ClientID%>").val('');
            $("#<%=txtOpSeq.ClientID%>").val('');
            $("#<%=txtItem.ClientID%>").val('');
            $("#<%=hdnItemID.ClientID%>").val('');
            $("#<%=txtJobDesc.ClientID%>").val('');
            $("#<%=txtUM.ClientID%>").val('');
            $("#<%=ddlBomType.ClientID%>").val('0');
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

            //if (!jQuery.trim($(txtGvQuan).val()) == '') {

            //}

            if (isNaN(parseFloat($(txtGvQuan).val()))) {
                $(txtGvQuan).val('0.00');
                $(txtGvPrice).val('0.00');
            } else if (parseFloat($(txtGvQuan).val()) == 0) {
                $(txtGvPrice).val('0.00');
            }

            //if (!jQuery.trim($(txtGvPrice).val()) == '') {

            //}

            if (isNaN(parseFloat($(txtGvPrice).val()))) {
                $(txtGvPrice).val('0.00');
            }

            if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
                if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
                    if (parseFloat($(txtGvQuan).val()) != 0) {
                        var valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
                        $(txtGvAmount).val(valAmount.toFixed(2));
                    }
                }
            }

            var AmountVal = $(txtGvAmount).val();
            if (parseFloat($(txtGvQuan).val()) == 0 && parseFloat(AmountVal) > 0) {
                $(txtGvQuan).val('1');                
                var QtyPrice = parseFloat(AmountVal) / 1;
                $(txtGvPrice).val(QtyPrice.toFixed(2));
            }

            CalculateTotalAmt();

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }

        function CalculateTotal(obj) {
            try {
                var masterTable = $find("<%=RadGrid_AddPO.ClientID%>").get_masterTableView();
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
                        QtyVal = parseFloat(QtyVal) || 0;
                        if (QtyVal == 0) {
                            QtyVal = 1;
                            $(Qty).val(QtyVal);
                            $(Price).val(parseFloat(AmountVal).toFixed(2));
                        }
                        var QtyPrice = parseFloat(AmountVal) / parseFloat(QtyVal);
                        $(Price).val(QtyPrice.toFixed(2));
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

            $('#<%=lblTotalAmount.ClientID%>').text(tAmount.toFixed(2));
            $('#<%=hdnTotal.ClientID%>').val(tAmount.toFixed(2));
            $('[id*=lblTotalAmt]').text(tAmount.toFixed(2));


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

            var txtGvAcctNo = document.getElementById(txt);
            $(txtGvAcctNo).removeClass("texttransparent");
            $(txtGvAcctNo).addClass("non-trans");

            var txtGvAmount = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvAmount'));
            $(txtGvAmount).removeClass("texttransparent");
            $(txtGvAmount).addClass("non-trans");

            var txtGvPrice = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvPrice'));
            $(txtGvPrice).removeClass("texttransparent");
            $(txtGvPrice).addClass("non-trans");

            var txtGvQuan = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvQuan'));
            $(txtGvQuan).removeClass("texttransparent");
            $(txtGvQuan).addClass("non-trans");

            var txtGvJob = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvJob'));
            $(txtGvJob).removeClass("texttransparent");
            $(txtGvJob).addClass("non-trans");

            var txtGvTicket = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvTicket'));
            $(txtGvTicket).removeClass("texttransparent");
            $(txtGvTicket).addClass("non-trans");

            var txtGvItem = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvItem'));
            $(txtGvItem).removeClass("texttransparent");
            $(txtGvItem).addClass("non-trans");

            var txtGvLoc = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvLoc'));
            $(txtGvLoc).removeClass("texttransparent");
            $(txtGvLoc).addClass("non-trans");

            var txtGvPhase = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvPhase'));
            $(txtGvPhase).removeClass("texttransparent");
            $(txtGvPhase).addClass("non-trans");

            //var txtGvWarehouse = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvWarehouse'));
            //$(txtGvWarehouse).removeClass("texttransparent");
            //$(txtGvWarehouse).addClass("non-trans");

            //var txtGvWarehouseLocation = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvWarehouseLocation'));
            //$(txtGvWarehouseLocation).removeClass("texttransparent");
            //$(txtGvWarehouseLocation).addClass("non-trans");

            var txtGvDue = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvDue'));
            $(txtGvDue).removeClass("texttransparent");
            $(txtGvDue).addClass("non-trans");
            var txtGvDesc = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvDesc'));
            $(txtGvDesc).removeClass("texttransparent");
            $(txtGvDesc).addClass("non-trans");
        }
        function aceItem_itemSelected(sender, e) {
            var hdnItemID = document.getElementById('<%= hdnItemID.ClientID %>');
            hdnItemID.value = e.get_value();
        }
        function SetContextKey() {
            var value = $get("<%=ddlBomType.ClientID %>").value;
            $find('<%=AutoCompleteExtender3.ClientID%>').set_contextKey($get("<%=ddlBomType.ClientID %>").value);
        }

        function KeyPressed(sender, eventArgs) {
      
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
            var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');
            $(hdnSelectPOIndex).val(-1);  
        }

        <%--function disablethis()
        {    
            document.getElementById("<%=btnSubmit.ClientID%>").disabled = true;
            itemJSON();
        }--%>
        <%--function disablethis() {
            $("#<%=btnSubmit.ClientID%>").css("pointer-events", "none");
            itemJSON();
        }

        function enableControl(control) {
            $("#<%=btnSubmit.ClientID%>").css("pointer-events", "all");
        }--%>
        
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="overlay">
        <img src="images/wheel.GIF" alt="Be patient..." class="lodder" />
    </div>
    <asp:HiddenField ID="hdnCustomProgram" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnEN" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnHasInventory" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnIsAutoCompleteSelected" ClientIDMode="Static" runat="server" Value="0" />

    <telerik:RadAjaxManager ID="RadAjaxManager_gvPO" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnAddNewLines">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_AddPO" LoadingPanelID="RadAjaxLoadingPanel_gvPO" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnCopyPrevious">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_AddPO" LoadingPanelID="RadAjaxLoadingPanel_gvPO" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnSubmit" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_gvPO" runat="server">
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
                                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add New Purchase Order</asp:Label>

                                    </div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"
                                                OnClientClick="javascript:return itemJSON(); " ValidationGroup="po">Save</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dynamicUI">Reports
                                            </a>
                                        </div>
                                        <ul id="dynamicUI" class="dropdown-content po-report-dropdown">
                                            <li>
                                                <asp:LinkButton ID="lnk_PO" runat="server" OnClick="lnk_PO_Click" OnClientClick="itemJSON();">PO Report</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lnk_CustomPOReport" runat="server" OnClick="lnk_CustomPOReport_Click" OnClientClick="itemJSON();"><asp:Literal runat="server" Text="<%$ AppSettings:CustomerName%>"/> - PO Report</asp:LinkButton></li><li>
                                                <asp:LinkButton ID="lnk_POReportForTS" runat="server" Visible="false" OnClick="lnk_POReportForTS_Click">PO Approval Report</asp:LinkButton></li><asp:ListView ID="listCustomPO" runat="server" Visible="true">
                                                <LayoutTemplate>
                                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <li id="poTemplateItem" runat="server" class="border-btm">
                                                        <asp:LinkButton runat="server" Enabled="true" OnCommand="btnDownloadPOTemplate_Click" CommandArgument="<%# Container.DataItem %>">
                                                                            <i class="fa fa-file-pdf-o pdfdy"  aria-hidden="true"></i>&nbsp;&nbsp; <%# Container.DataItem %> <i class="fa fa-download pdf-trn" aria-hidden="true" ></i>
                                                        </asp:LinkButton></li></ItemTemplate></asp:ListView></ul></div><div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton></div><div class="rght-content">
                                        <div class="editlabel">
                                            <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label><asp:Label ID="lblPO" runat="server" Text="PO #" Visible="False"></asp:Label><asp:Label ID="lblPOId" runat="server" Visible="False"></asp:Label></div></div></div></div></div></div></header></div><div class="container breadcrumbs-bg-custom">
                <div class="row">
                    <div class="col s12 m12 l12">
                        <div class="row">
                            <div class="tblnks">
                                <ul class="anchor-links">
                                    <li><a href="#accrPurchaseOrder">Purchase Order Details</a></li><li id="liDocuments"><a href="#accrdDocuments">Documents</a></li><li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li></ul></div><div class="tblnksright">
                                <div class="nextprev">

                                    <asp:Panel ID="pnlSave" runat="server">
                                        <asp:Panel ID="pnlNext" runat="server" Visible="False">
                                            <span class="angleicons">
                                                <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False" OnClick="lnkFirst_Click"><i class="fa fa-angle-double-left"></i></asp:LinkButton></span><span class="angleicons"><asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False" OnClick="lnkPrevious_Click"><i class="fa fa-angle-left"></i></asp:LinkButton></span><span class="angleicons"><asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False" OnClick="lnkNext_Click"> <i class="fa fa-angle-right"></i></asp:LinkButton></span><span class="angleicons"><asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False" OnClick="lnkLast_Click"> <i class="fa fa-angle-double-right"></i></asp:LinkButton></span></asp:Panel></asp:Panel></div></div></div></div></div></div></div></div><div class="container accordian-wrap">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <div class="card">
                        <div class="card-content">
                            <div class="form-section-row">
                                <div class="col s12 m12 l12 p-r-0">
                                    <div class="row">
                                        <div class="form-content-wrap" id="accrPurchaseOrder">
                                            <div class="form-content-pd">
                                                <div runat="server" id="divSuccess">
                                                    <button type="button" class="close" data-dismiss="alert">×</button>These month/year period is closed out. You do not have permission to add/update this record. </div><div class="form-section-row">
                                                    <div class="section-ttle">Purchase Order</div><div class="form-input-row">
                                                        <div class="form-section3">

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:RequiredFieldValidator runat="server" ID="rfvVendor" ErrorMessage="Please select Vendor"
                                                                        Display="None" ControlToValidate="txtVendor" ValidationGroup="po"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceVendor" runat="server" Enabled="True" PopupPosition="Right"
                                                                            TargetControlID="rfvVendor" />
                                                                    <asp:CustomValidator ID="cvVendor" runat="server" ClientValidationFunction="ChkVendor"
                                                                        ControlToValidate="txtVendor" Display="None" ErrorMessage="Please select the vendor"
                                                                        SetFocusOnError="True" Enabled="False"></asp:CustomValidator><asp:ValidatorCalloutExtender ID="vceVendor1" runat="server" Enabled="True"
                                                                            TargetControlID="cvVendor">
                                                                        </asp:ValidatorCalloutExtender>

                                                                    <asp:Button ID="btnSelectVendor" runat="server" CausesValidation="False" OnClick="btnSelectVendor_Click"
                                                                        Style="display: none;" Text="Button" />
                                                                    <asp:TextBox ID="txtVendor" runat="server" AutoPostBack="true" OnTextChanged="txtVendor_TextChanged" MaxLength="75"></asp:TextBox><asp:HiddenField ID="hdnVendorID" runat="server" />
                                                                    <asp:Label runat="server" ID="lbltxtVendor" AssociatedControlID="txtVendor">Search By Vendor</asp:Label></div><asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                                                    <ContentTemplate>
                                                                        <div class="srchclr btnlinksicon rowbtn">
                                                                            <asp:HyperLink ID="lnkVenderID" Visible="true" Target="_self" runat="server"><i class="mdi-communication-contacts ml" ></i></asp:HyperLink></div></ContentTemplate><Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnSelectVendor" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </div>



                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:UpdatePanel ID="updPnlAddress" runat="server" UpdateMode="Always">
                                                                        <ContentTemplate>
                                                                            <asp:Label runat="server" ID="lblAddress" AssociatedControlID="txtAddress">Address</asp:Label><asp:TextBox ID="txtAddress" runat="server" CssClass="materialize-textarea" MaxLength="2000"
                                                                                TextMode="MultiLine"></asp:TextBox></ContentTemplate><Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="btnSelectVendor" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                                                        <ContentTemplate>
                                                                            <asp:Label runat="server" ID="lblShipTo" AssociatedControlID="txtShipTo">Ship To</asp:Label><asp:TextBox ID="txtShipTo" runat="server" CssClass="materialize-textarea" MaxLength="2000" TextMode="MultiLine">
                                                                            </asp:TextBox><asp:RequiredFieldValidator ID="rfvShipTo"
                                                                                runat="server" ControlToValidate="txtShipTo" Display="None" ErrorMessage="Ship to is Required" ValidationGroup="po"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceShipTo" runat="server" Enabled="True"
                                                                                    PopupPosition="Right" TargetControlID="rfvShipTo" />
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="txtVendor" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>

                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12 m-t-22" >
                                                                <div class="row">
                                                                    <asp:Label runat="server" ID="lblComments" AssociatedControlID="txtDesc">Comments</asp:Label><asp:TextBox ID="txtDesc" runat="server" CssClass="materialize-textarea" MaxLength="2000" TextMode="MultiLine">
                                                                    </asp:TextBox><asp:RequiredFieldValidator ID="rfvDesc"
                                                                        runat="server" ControlToValidate="txtDesc" Display="None" ErrorMessage="Comments is Required" ValidationGroup="po"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceDesc" runat="server" Enabled="True"
                                                                            PopupPosition="Right" TargetControlID="rfvDesc" />


                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12" id="divStatusComments" runat="server" visible="false">
                                                                <div class="row">
                                                                    <label>Status Comments</label> <asp:TextBox ID="txtStatusComment" runat="server" Rows="3" CssClass="form-control" MaxLength="2000" TextMode="MultiLine">
                                                                    </asp:TextBox></div></div><div class="input-field col s12">
                                                                <div class="row">

                                                                    <div class="btnlinks">

                                                                        <asp:LinkButton Text="Approve" ID="btnApprove" runat="server" Visible="false" OnClick="btnApprove_Click" />
                                                                        <asp:LinkButton Text="Decline" ID="btnDecline" runat="server" Visible="false" OnClick="btnApprove_Click" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-section3-blank">
                                                            &nbsp; </div><div class="form-section3">

                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtDate" runat="server" CssClass="datepicker_mom"
                                                                        MaxLength="15" onkeypress="return false;"></asp:TextBox><%-- <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                                                        TargetControlID="txtDate">
                                                                    </asp:CalendarExtender>--%><asp:RequiredFieldValidator ID="rfvDate" ValidationGroup="po"
                                                                        runat="server" ControlToValidate="txtDate" Display="None" ErrorMessage="Date is Required"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True"
                                                                            PopupPosition="Right" TargetControlID="rfvDate" />
                                                                    <asp:RegularExpressionValidator ID="rfvDate1" ControlToValidate="txtDate" ValidationGroup="po"
                                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                                    </asp:RegularExpressionValidator><asp:ValidatorCalloutExtender ID="vceDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                                        TargetControlID="rfvDate1" />
                                                                    <asp:Label runat="server" ID="lblDate" AssociatedControlID="txtDate">Date</asp:Label></div></div><div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp; </div></div><div class="input-field col s5">
                                                                <div class="row">
                                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                                                        <ContentTemplate>

                                                                            <asp:TextBox ID="txtDueDate" runat="server" CssClass="datepicker_mom"
                                                                                onkeypress="return false;"></asp:TextBox><asp:CalendarExtender ID="txtDueDate_CalendarExtender" runat="server" Enabled="True"
                                                                                    TargetControlID="txtDueDate">
                                                                                </asp:CalendarExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvDueDate" ValidationGroup="po"
                                                                                runat="server" ControlToValidate="txtDueDate" Display="None" ErrorMessage="Due Date is Required"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceDueDate" runat="server" Enabled="True"
                                                                                    PopupPosition="Right" TargetControlID="rfvDueDate" />
                                                                            <asp:RegularExpressionValidator ID="rfvDueDate1" ControlToValidate="txtDueDate" ValidationGroup="po"
                                                                                ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                                                runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                                            </asp:RegularExpressionValidator><asp:ValidatorCalloutExtender ID="vceDueDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                                                TargetControlID="rfvDueDate1" />
                                                                            <asp:Label runat="server" ID="lblDue" AssociatedControlID="txtDueDate">Due</asp:Label></ContentTemplate><Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="ddlTerms" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <label class="drpdwn-label">Payment Terms</label> <asp:DropDownList ID="ddlTerms" runat="server" CssClass="browser-default" onchange="UpdateDueByTerms()">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvTerms" ValidationGroup="po" InitialValue=""
                                                                        runat="server" ControlToValidate="ddlTerms" Display="None" ErrorMessage="Please select terms"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceTerms" runat="server" Enabled="True"
                                                                            PopupPosition="Right" TargetControlID="rfvTerms" />

                                                                </div>
                                                            </div>
                                                            <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp; </div></div><div class="input-field col s5">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtSalesOrderNo" runat="server" MaxLength="50"></asp:TextBox><asp:Label runat="server" ID="lblSalesOrderNo" AssociatedControlID="txtSalesOrderNo">Sales Order #</asp:Label></div></div><div class="input-field col s12"></div>

                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtVendorType" runat="server" placeholder="  " ReadOnly="true"></asp:TextBox><label runat="server" associatedcontrolid="txtVendorType" class="active">Vendor Type</label> </div></div><div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp; </div></div><div class="input-field col s5">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtFOB" runat="server"></asp:TextBox><asp:Label runat="server" ID="lblFOB" AssociatedControlID="txtFOB">Incoterms</asp:Label></div></div><div class="input-field col s5">
                                                                <div class="row">
                                                                    
                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                                                        <ContentTemplate>

                                                                            <asp:TextBox ID="txtShipVia" runat="server">
                                                                            </asp:TextBox><asp:Label runat="server" ID="lblCourier" AssociatedControlID="txtShipVia">Ship Via</asp:Label></ContentTemplate><Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="btnSelectVendor" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                
                                                                
                                                                
                                                                </div></div><div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp; </div></div><div class="input-field col s5">
                                                                <div class="row">
                                                                    



                                                                    <asp:TextBox ID="txtCourrierAcct" runat="server" MaxLength="50"></asp:TextBox><asp:Label runat="server" ID="lbltxtCourrierAcct" AssociatedControlID="txtCourrierAcct">Courier Account #</asp:Label>

                                                                </div>
                                                            </div>

                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <asp:Label class="drpdwn-label" runat="server" ID="lblApprovalStatus" AssociatedControlID="ddlApprovalStatus">Approval Status</asp:Label><asp:DropDownList ID="ddlApprovalStatus" runat="server"
                                                                        OnSelectedIndexChanged="ddlApprovalStatus_SelectedIndexChanged"
                                                                        CssClass="browser-default">
                                                                        <asp:ListItem Value="-1">Select</asp:ListItem><asp:ListItem Value="0">Pending</asp:ListItem><asp:ListItem Value="1">Approved</asp:ListItem><asp:ListItem Value="2">Declined</asp:ListItem><asp:ListItem Value="3">Reapprove</asp:ListItem></asp:DropDownList></div></div><div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp; </div></div><div class="input-field col s5 mt-10">
                                                                <div class="row">
                                                                    <asp:CheckBox ID="chkApproved" CssClass="css-checkbox" runat="server" Text="Approved" />
                                                                </div>
                                                            </div>

                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <div class="signature m-t-0"  runat="server" id="divSignature" visible="false">
                                                                        <div class="fc-label">
                                                                            Signature </div><div id="signbg" class="fc-input fc-input-new" style="width: 268px; height: 100px;">
                                                                            <asp:Image ID="imgSign" runat="server" />
                                                                            <asp:HiddenField ID="hdnImg" runat="server" />
                                                                        </div>
                                                                    </div>

                                                                    <div id="sign" tabindex="0"  class="sign_popup sigPad fc-input">

                                                                        <div class="sign-title">
                                                                            <a class="sign-title-l clearButton">Clear Signature</a> <a id="convertpngbtn" class="sign-title-r">Accept</a> </div><div class="sig">
                                                                            <div class="typed"></div>
                                                                            <canvas class="pad"  id="canvas"></canvas><input id="hdnDrawdata" tabindex="43" type="hidden" name="output" class="output" /></div></div></div></div></div><div class="form-section3-blank">
                                                            &nbsp; </div><div class="form-section3">
                                                            <div class="input-field col s5 pdrt">
                                                                <div class="row">

                                                                    <asp:RequiredFieldValidator ID="rfvPO" ValidationGroup="po"
                                                                        runat="server" ControlToValidate="txtPO" Display="None" ErrorMessage="Please enter PO"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vcePO" runat="server" Enabled="True"
                                                                            PopupPosition="Right" TargetControlID="rfvPO" />

                                                                    <asp:Label runat="server" ID="lablPO" AssociatedControlID="txtPO">PO#</asp:Label><asp:TextBox ID="txtPO" runat="server" disabled="disabled" onkeypress="return isNumberKey(event,this)"
                                                                        MaxLength="50"></asp:TextBox></div></div><div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp; </div></div>
                                                        <div class="input-field col s5 pdrt lblfield r-dis" style="float: right;">
                                                                <div class="row ">
                                                                    <span class="ttlab">Total</span> <span class="ttlval">$<asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                                                                        <telerik:RadToolTip RenderMode="Auto" ID="tooltiplblTotalAmount" runat="server" TargetControlID="lblTotalAmount" ShowEvent="OnClick"
                                Position="MiddleRight" RelativeTo="Element">
                Balance Amount $0.00
            </telerik:RadToolTip>
                                                                                                     </span>

                                                                     
                                                                </div></div>
                                                                
                                                                <div class="input-field col s12"></div>
                                                            <div class="input-field col s5 pdrt">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtStatus" runat="server"
                                                                        Text="New"></asp:TextBox><asp:Label runat="server" ID="lblstatus" AssociatedControlID="txtStatus">PO Status</asp:Label></div></div>                                                              
                                                                <div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp; </div></div>
                                                                
                                                                <div class="input-field col s5 pdrt lblfield r-dis" style="float: right;">
                                                                <div class="row ">
                                                                    <span class="ttlab">Open Amount</span> <span class="ttlval">$<asp:Label ID="lblTotalOpenAmount" runat="server"></asp:Label></span></div></div>


                                                                <%--<div class="input-field col s5 mt-10">
                                                                <div class="row">
                                                                    <asp:CheckBox ID="chkPOClose" CssClass="css-checkbox" runat="server" Text="Close PO" Visible="false" onchange="ClosePOFunction()" />
                                                                    <asp:CheckBox ID="chkAddRPO" CssClass="css-checkbox" runat="server" Text="Receive PO" />
                                                                </div>
                                                            </div>--%>


                                                            <div class="input-field col s12"></div>

                                                            <div class="input-field col s5 pdrt">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtCreatedBy" runat="server" disabled="disabled" MaxLength="50" ReadOnly="true"></asp:TextBox><asp:Label runat="server" ID="lblCreatedby" AssociatedControlID="txtCreatedBy">Created By</asp:Label></div></div><div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp; </div></div><div class="input-field  col s5 pdrt">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtRequestedBy" runat="server"
                                                                        MaxLength="50"></asp:TextBox><asp:Label runat="server" ID="Label5" AssociatedControlID="txtRequestedBy">Requested By</asp:Label></div></div><div class="input-field col s12 " id="emptyLinePOReason" runat="server">
                                                                <%--<div class="row">
                                                                    <span style="font-weight: bold; font-size: smaller;">&nbsp;</span>
                                                                </div>--%>
                                                            </div>
                                                            <div class="input-field col s12 " id="emptyLinePORevision" runat="server">
                                                                <%--<div class="row">
                                                                    <span style="font-weight: bold; font-size: smaller;">&nbsp;</span>
                                                                </div>--%>
                                                            </div>

                                                            <div class="input-field col s5">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtPOCode" runat="server" MaxLength="50"></asp:TextBox><asp:Label runat="server" ID="lblPOCode" AssociatedControlID="txtPOCode">PO Reason Code</asp:Label></div></div><div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp; </div></div><div class="input-field col s5">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtPoRevision" runat="server" MaxLength="3"></asp:TextBox><asp:Label runat="server" ID="lblPORevision" AssociatedControlID="txtPoRevision">PO Revision</asp:Label></div></div>


                                                                <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:CheckBox ID="chkPOClose" CssClass="css-checkbox" runat="server" Text="Close PO" Visible="false" onchange="ClosePOFunction()" />

                                                                <%--</div></div><div class="input-field col s2">
                                                                <div class="row">
                                                                    &nbsp; </div></div><div class="input-field col s5">
                                                                <div class="row">--%>
                                                                    <asp:CheckBox ID="chkAddRPO" CssClass="css-checkbox" runat="server" Text="Receive PO" /></div></div>
                                                                <div class="input-field col s12 ">                                                                      
                                                                <div class="row">
                                                                    <span class="msg-po" >Enter Project and Code only if PO is for a single project</span> </div></div><div class="input-field col s12 pdrt">
                                                                <div class="row">

                                                                    <asp:TextBox ID="txtProject" runat="server" CssClass="pmainsearchinput"></asp:TextBox><asp:Label runat="server" ID="Label3" AssociatedControlID="txtProject">Project</asp:Label><asp:HiddenField ID="hdnmainjobID" runat="server" />
                                                                    <asp:HiddenField ID="hdMainGvLoc" runat="server" />
                                                                    <asp:HiddenField ID="hdMainAcctID" runat="server" />
                                                                    <asp:HiddenField ID="hdMainGvAcctNo" runat="server" />
                                                                    <%--<asp:TextBox ID="txtFOB" runat="server"></asp:TextBox>
                                                                    <asp:Label runat="server" ID="lblFOB" AssociatedControlID="txtFOB">Incoterms</asp:Label>--%>
                                                                </div>
                                                            </div>
                                                            <div class="input-field col s12">
                                                                <div class="row">
                                                                    <asp:TextBox ID="txtCode" runat="server" CssClass="pcodesearchinput"></asp:TextBox><asp:Label runat="server" ID="Label4" AssociatedControlID="txtCode">Code</asp:Label><asp:HiddenField ID="hdnMainCode" runat="server" />

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-section-row">
                                                    <div class="section-ttle">Contact Details</div><div class="form-input-row">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                            <ContentTemplate>
                                                                <div class="form-section3">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:Label runat="server" ID="lblContactName" AssociatedControlID="txtVenderContactName">Contact Name</asp:Label><asp:TextBox ID="txtVenderContactName" disabled="disabled" ReadOnly="true" runat="server"></asp:TextBox></div></div><div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:Label runat="server" AssociatedControlID="txttVenderPhone">Phone</asp:Label><asp:TextBox ID="txttVenderPhone" disabled="disabled" ReadOnly="true" CssClass="phone" runat="server" MaxLength="28" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="form-section3-blank">
                                                                    &nbsp; </div><div class="form-section3">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:Label runat="server" ID="Label1" AssociatedControlID="txtVenderFax">Fax</asp:Label><asp:TextBox ID="txtVenderFax" disabled="disabled" ReadOnly="true" runat="server" MaxLength="28" />
                                                                            <asp:MaskedEditExtender ID="txtFax_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" InputDirection="LeftToRight"
                                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                                CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999"
                                                                                MaskType="Number" TargetControlID="txtVenderFax" ValidateRequestMode="Enabled">
                                                                            </asp:MaskedEditExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:Label runat="server" AssociatedControlID="txtVenderEmailid">Email</asp:Label><asp:TextBox ID="txtVenderEmailid" TextMode="Email" disabled="disabled" ReadOnly="true" runat="server" MaxLength="50" />
                                                                            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                                                                ControlToValidate="txtVenderEmailid" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator><asp:ValidatorCalloutExtender ID="vceEmail" runat="server" Enabled="True"
                                                                                    TargetControlID="revEmail">
                                                                                </asp:ValidatorCalloutExtender>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="form-section3-blank">
                                                                    &nbsp; </div><div class="form-section3">
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:Label runat="server" ID="Label2" AssociatedControlID="txtVenderCellular">Cellular</asp:Label><asp:TextBox ID="txtVenderCellular" disabled="disabled" ReadOnly="true" runat="server" MaxLength="28" />

                                                                        </div>
                                                                    </div>
                                                                    <div class="input-field col s12">
                                                                        <div class="row">
                                                                            <asp:Label runat="server" AssociatedControlID="txtVenderWebsite">Web Address</asp:Label><asp:TextBox ID="txtVenderWebsite" disabled="disabled" ReadOnly="true" runat="server" MaxLength="50" />

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnSelectVendor" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>

                                                <div class="form-section-row">
                                                    <div class="section-ttle">PO Custom </div><div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtPO1" runat="server"></asp:TextBox><label id="lb_txtPO1" runat="server" for="txtPO1">Custom 1</label> </div></div></div><div class="form-section3-blank">
                                                        &nbsp; </div><div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtPO2" runat="server"></asp:TextBox><label id="lb_txtPO2" runat="server" for="txtPO2">Custom 2</label> </div></div></div><div class="cf"></div>
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

    <div class="container accordian-wrap">
        <div class="row">
            <div class="grid_container">
                <div class="RadGrid RadGrid_Material FormGrid">
                    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
                    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvPO" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_AddPO" AllowFilteringByColumn="false" ShowFooter="True" PageSize="50"
                            ShowStatusBar="true" runat="server" AllowSorting="true" Width="100%" PagerStyle-AlwaysVisible="true" OnItemCommand="RadGrid_AddPO_ItemCommand"
                            AllowCustomPaging="True" onblur="resetIndexF6()">
                            <CommandItemStyle />
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowKeyboardNavigation="true">
                                <Selecting AllowRowSelect="True"></Selecting>
                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                <ClientEvents OnKeyPress="KeyPressed" />
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                <Columns>

                                    <telerik:GridTemplateColumn DataField="JobName" AutoPostBackOnFilter="true" HeaderStyle-Width="130" UniqueName="ProjectJob"
                                        CurrentFilterFunction="Contains" HeaderText="Project" ShowFilterIcon="false" AllowFiltering="false" ItemStyle-Width="130">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvJob" runat="server" CssClass="texttransparent psearchinput"
                                                Text='<%# Bind("JobName") %>'></asp:TextBox><asp:HiddenField ID="hdnJobID" Value='<%# Eval("JobID") != DBNull.Value ? Eval("JobID") : "" %>' runat="server" />
                                            <asp:HiddenField ID="hdnIndex" Value='<%#Container.ItemIndex%>' runat="server" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotal" Text="Total" runat="server"></asp:Label></FooterTemplate></telerik:GridTemplateColumn><telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="100"
                                        UniqueName="Ticket" CurrentFilterFunction="Contains" HeaderText="Ticket" ShowFilterIcon="false" ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvTicket" runat="server" CssClass="texttransparent" Text='<%# Bind("Ticket") %>' autocomplete="off"></asp:TextBox></ItemTemplate></telerik:GridTemplateColumn><telerik:GridTemplateColumn DataField="Phase" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="120"
                                        UniqueName="Code" CurrentFilterFunction="Contains" HeaderText="Code" ShowFilterIcon="false" ItemStyle-Width="120">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvPhase" runat="server" CssClass="texttransparent phsearchinput"
                                                Text='<%# Bind("Phase") %>'></asp:TextBox><%-- onchange="clearPhase(this)"--%><asp:HiddenField ID="hdnPID" Value='<%# Eval("PhaseID") != DBNull.Value ? Eval("PhaseID") : "" %>' runat="server" />
                                            <asp:HiddenField ID="hdntxtGvPhase" Value='<%# Bind("Phase") %>' runat="server" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="ItemDesc" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="120"
                                        UniqueName="ItemDesc" CurrentFilterFunction="Contains" HeaderText="Item" ShowFilterIcon="false" ItemStyle-Width="120">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvItem" runat="server" CssClass="texttransparent pisearchinput"
                                                Text='<%# Bind("ItemDesc") %>'></asp:TextBox><asp:HiddenField ID="hdnItemID" Value='<%# Eval("Inv") != DBNull.Value ? Eval("Inv") : "" %>' runat="server" />
                                            <asp:HiddenField ID="hdOpSq" Value='<%# Eval("OpSq")%>' runat="server" />
                                            <asp:HiddenField ID="hdnTypeId" Value='<%# Eval("TypeID") != DBNull.Value ? Eval("TypeID") : "" %>' runat="server" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>


                                    <telerik:GridTemplateColumn DataField="fDesc" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="210"
                                        CurrentFilterFunction="Contains" HeaderText="Item Description" ShowFilterIcon="false" ItemStyle-Width="210">
                                        <ItemTemplate>
                                            <%--<asp:TextBox ID="txtGvDesc" runat="server" CssClass="texttransparent"
                                                Text='<%# Bind("fDesc") %>' MaxLength="255"></asp:TextBox>--%>
                                            <asp:TextBox ID="txtGvDesc" Style="padding: 0px!important;" runat="server" Text='<%#Eval("fDesc")%>' TextMode="MultiLine"
                                                MaxLength="8000" CssClass="materialize-textarea"></asp:TextBox></ItemTemplate></telerik:GridTemplateColumn><telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="120"
                                        CurrentFilterFunction="Contains" UniqueName="WarehouseID" HeaderText="Warehouse" ShowFilterIcon="false" ItemStyle-Width="120">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvWarehouse" ReadOnly='<%# Eval("Phase").ToString() != "Inventory" ? true : false %>'
                                                Text='<%# Bind("Warehousefdesc") %>'
                                                runat="server" CssClass="texttransparent Warehousesearchinput"></asp:TextBox><asp:HiddenField ID="hdnWarehousefdesc" runat="server" Value='<%# Bind("Warehousefdesc") %>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnWarehouse" runat="server" Value='<%# Bind("WarehouseID") %>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="100"
                                        CurrentFilterFunction="Contains" UniqueName="LocationID" HeaderText="Warehouse Location" ShowFilterIcon="false" ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvWarehouseLocation" ReadOnly='<%# Eval("Phase").ToString() != "Inventory" ? true : false %>'
                                                Text='<%# Bind("Locationfdesc") %>'
                                                runat="server" CssClass="texttransparent WarehouseLocationsearchinput "></asp:TextBox><asp:HiddenField ID="hdnLocationfdesc" runat="server" Value='<%# Bind("Locationfdesc") %>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnWarehouseLocationID" runat="server" Value='<%# Bind("LocationID") %>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>




                                    <telerik:GridTemplateColumn DataField="AcctNo" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="100"
                                        UniqueName="AcctGL" CurrentFilterFunction="Contains" HeaderText="Acct No." ShowFilterIcon="false" ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvAcctNo" runat="server" CssClass="texttransparent searchinput"
                                                Text='<%# Bind("AcctNo") %>'></asp:TextBox><asp:HiddenField ID="hdnId" runat="server" Value='<%# Bind("RowID") %>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnTID" runat="server" Value='<%# Eval("ID") != DBNull.Value ? Eval("ID") : "" %>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Bind("Line") %>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnAcctID" runat="server" Value='<%# Eval("AcctID") != DBNull.Value ? Eval("AcctID") : "" %>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnForceClose" runat="server" Value='<%# Bind("ForceClose") %>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="Quan" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="100"
                                        CurrentFilterFunction="Contains" HeaderText="Quan" ShowFilterIcon="false" ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvQuan" runat="server" CssClass="texttransparent" autocomplete="off"
                                                MaxLength="15" Text='<%# Bind("Quan") %>'
                                                onchange="CalTotalVal(this);"></asp:TextBox></ItemTemplate><FooterTemplate>
                                            <asp:Label ID="lblTotalQty" runat="server" Style="text-align: left;"></asp:Label></FooterTemplate></telerik:GridTemplateColumn><telerik:GridTemplateColumn DataField="Price" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="100"
                                        CurrentFilterFunction="Contains" HeaderText="Price" ShowFilterIcon="false" ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvPrice" runat="server" CssClass="texttransparent" autocomplete="off"
                                                MaxLength="15" Text='<%# Bind("Price") %>'
                                                onchange="CalTotalVal(this);"></asp:TextBox></ItemTemplate></telerik:GridTemplateColumn><telerik:GridTemplateColumn DataField="Amount" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="100"
                                        CurrentFilterFunction="Contains" HeaderText="$Amount" ShowFilterIcon="false" ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvAmount" runat="server" CssClass="texttransparent clsAmount" autocomplete="off"
                                                MaxLength="15" onkeypress="return isDecimalKey(this,event)" Text='<%# Bind("Amount") %>' DataFormatString="{0:c}"
                                                onchange="CalculateTotal(this);"></asp:TextBox></ItemTemplate><FooterTemplate>
                                            <asp:Label ID="lblTotalAmt" runat="server" Style="text-align: left;"></asp:Label></FooterTemplate></telerik:GridTemplateColumn><telerik:GridTemplateColumn DataField="Due" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="120"
                                        CurrentFilterFunction="Contains" HeaderText="Due Date" ShowFilterIcon="false" ItemStyle-Width="120">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvDue" runat="server" CssClass="datepicker_mom texttransparent"
                                                Text='<%#  Eval("Due")==DBNull.Value? "" : String.Format("{0:MM/dd/yyyy}", Eval("Due")) %>'></asp:TextBox><asp:RegularExpressionValidator ID="revGvDue" ControlToValidate="txtGvDue" ValidationGroup="po"
                                                    ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                    runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                </asp:RegularExpressionValidator><asp:ValidatorCalloutExtender ID="vceGvDue" runat="server" Enabled="True" PopupPosition="Right"
                                                    TargetControlID="revGvDue" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn DataField="Loc" AutoPostBackOnFilter="true" AllowFiltering="false" HeaderStyle-Width="150"
                                        CurrentFilterFunction="Contains" HeaderText="Location Name" ShowFilterIcon="false" ItemStyle-Width="150">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGvLoc" runat="server" CssClass="texttransparent jsearchinput"
                                                Text='<%# Bind("Loc") %>' onchange="clearJob(this)"></asp:TextBox></ItemTemplate></telerik:GridTemplateColumn><telerik:GridTemplateColumn AutoPostBackOnFilter="true" AllowFiltering="false"
                                        CurrentFilterFunction="Contains" HeaderText="Action" ShowFilterIcon="false" HeaderStyle-Width="60" ItemStyle-Width="60">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibDelete" runat="server" CommandName="DeleteTransaction"
                                                CommandArgument="<%#Container.ItemIndex%>"
                                                ImageUrl="~/images/glyphicons-17-bin.png" Width="13px" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </telerik:RadAjaxPanel>
                </div>
            </div>
        </div>
        <div class="row>
            <div class="btnlinks">
                <asp:LinkButton ID="btnAddNewLines" runat="server" CausesValidation="false" OnClientClick="itemJSON();" OnClick="btnAddNewLines_Click" Text="Add New Lines"></asp:LinkButton><asp:LinkButton ID="btnCopyPrevious" runat="server" CausesValidation="false" OnClientClick="itemJSON();"
                    OnClick="btnCopyPrevious_Click" Text="Copy Previous" Style="display: none;"></asp:LinkButton></div></div><div class="row">
            <asp:Label ID="lblTC" runat="server"></asp:Label></div></div><div class="container accordian-wrap">
        <div class="col s12 m12 l12">
            <div class="row">
                <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                    <li id="tbDocuments" runat="server">
                        <div id="accrdDocuments" class="collapsible-header accrd accordian-text-custom"><i class="mdi-file-attachment"></i>Documents</div><div class="collapsible-body">
                            <%--<div class="form-content-wrap">
                                <div class="form-content-pd">
                                    <div class="grid_container">--%>
                            <asp:Panel ID="pnlDocPermission" runat="server">
                                <div class="form-section-row">
                                    <div class="col s12 m12 l12">
                                        <div class="row">
                                            <asp:FileUpload ID="FileUpload1" runat="server" class="dropify" onchange="AddDocumentClick(this);" />
                                            <%--<asp:FileUpload ID="FileUpload2" runat="server" class="dropify" onchange="ConfirmUpload(this.value);" />--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="btncontainer">
                                    <asp:Panel ID="pnlDocumentButtons" runat="server">
                                        <div class="btnlinks">
                                            <%--<asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" OnClientClick="return checkdelete();">Delete</asp:LinkButton>--%>
                                            <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton><asp:LinkButton ID="lnkUploadDoc" runat="server" CausesValidation="False" OnClick="lnkUploadDoc_Click" Style="display: none">Upload</asp:LinkButton><asp:LinkButton ID="lnkPostback" runat="server" CausesValidation="False" Style="display: none">Postback</asp:LinkButton></div></asp:Panel><div style="clear: both;"></div>
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

                                                            function requestStart1(sender, args) {
                                                                try {
                                                                    requestInitiator = document.activeElement.id;
                                                                    if (document.activeElement.tagName == "INPUT") {
                                                                        selectionStart = document.activeElement.selectionStart;
                                                                    }
                                                                } catch (e) {

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

                                            <telerik:RadAjaxPanel ID="RadAjaxPanel_Documents" PostBackControls="lblName" runat="server" ClientEvents-OnRequestStart="requestStart1" ClientEvents-OnResponseEnd="responseEnd1">
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
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label><asp:HiddenField runat="server" ID="hdnTempId" Value='<%# Eval("id").ToString() == "0"? Eval("TempId"): string.Empty %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <%--<telerik:GridTemplateColumn SortExpression="filename" HeaderText="File Name" DataField="filename" ShowFilterIcon="false">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lblName" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Path") %>'
                                                                                Text='<%# Eval("filename") %>' CommandName="D"> </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>--%>
                                                            <telerik:GridTemplateColumn DataField="filename" SortExpression="filename" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                                HeaderText="File Name" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                                        CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                        OnClientClick="return ViewDocumentClick(this);" OnClick="lblName_Click" Text='<%# Eval("filename") %>'>
                                                                    </asp:LinkButton></ItemTemplate></telerik:GridTemplateColumn><telerik:GridBoundColumn FilterDelay="5" DataField="doctype" HeaderText="File Type" HeaderStyle-Width="140"
                                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="doctype"
                                                                ShowFilterIcon="false">
                                                            </telerik:GridBoundColumn>

                                                            <telerik:GridTemplateColumn SortExpression="portal" HeaderText="Portal" DataField="portal" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# (Eval("portal")!=DBNull.Value) ? Convert.ToBoolean(Eval("portal")): false %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                                                CurrentFilterFunction="Contains" HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="100">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                </ItemTemplate>

                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn SortExpression="remarks" HeaderText="Remarks" DataField="remarks" ShowFilterIcon="false">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtremarks" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox></ItemTemplate></telerik:GridTemplateColumn></Columns></MasterTableView></telerik:RadGrid></telerik:RadAjaxPanel></div></div></div></asp:Panel><%--</div>

                                    <div class="cf"></div>
                                </div>
                            </div>--%><div style="clear: both;"></div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>

    <div class="container accordian-wrap">
        <div class="col s12 m12 l12">
            <div class="row">
                <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
                    <li id="tbLogs" runat="server" style="display: none">
                        <div id="accrdlogs" class="collapsible-header accrd accordian-text-custom"><i class="mdi-content-content-paste"></i>Logs</div><div class="collapsible-body">
                            <div class="form-content-wrap">
                                <div class="form-content-pd">
                                    <div class="grid_container">
                                        <div class="form-section-row m-b-0" >
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

                                                        function requestStart2(sender, args) {
                                                            try {
                                                                requestInitiator = document.activeElement.id;
                                                                if (document.activeElement.tagName == "INPUT") {
                                                                    selectionStart = document.activeElement.selectionStart;
                                                                }
                                                            } catch (e) {

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
                                                <telerik:RadAjaxPanel ID="RadAjaxPanel_gvLogs" runat="server" LoadingPanelID="RadAjaxLoadingPanel_gvLogs" ClientEvents-OnRequestStart="requestStart2" ClientEvents-OnResponseEnd="responseEnd2">
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
                                                                        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("CreatedStamp", "{0:M/d/yyyy}")%>'></asp:Label></ItemTemplate></telerik:GridTemplateColumn><telerik:GridTemplateColumn DataField="CreatedStamp" SortExpression="CreatedStamp" AutoPostBackOnFilter="true" DataType="System.String"
                                                                    CurrentFilterFunction="Contains" HeaderText="Time" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbltime" runat="server" Text='<%# Eval("CreatedStamp","{0: hh:mm tt}") %>'></asp:Label></ItemTemplate></telerik:GridTemplateColumn><telerik:GridTemplateColumn DataField="fUser" SortExpression="fUser" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="User" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUpdBy" runat="server" Text='<%# Eval("fUser") %>'></asp:Label></ItemTemplate></telerik:GridTemplateColumn><telerik:GridTemplateColumn DataField="Field" SortExpression="Field" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="Field" ShowFilterIcon="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblField" runat="server" Text='<%# Eval("field") %>'></asp:Label></ItemTemplate></telerik:GridTemplateColumn><telerik:GridTemplateColumn DataField="OldVal" SortExpression="OldVal" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="Old Value" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOldVal" runat="server" Text='<%# Eval("OldVal") %>'></asp:Label></ItemTemplate></telerik:GridTemplateColumn><telerik:GridTemplateColumn DataField="NewVal" SortExpression="NewVal" AutoPostBackOnFilter="true"
                                                                    CurrentFilterFunction="Contains" HeaderText="New Value" ShowFilterIcon="false" HeaderStyle-Width="305">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNewVal" runat="server" Text='<%# Eval("NewVal") %>'></asp:Label></ItemTemplate></telerik:GridTemplateColumn></Columns></MasterTableView></telerik:RadGrid></telerik:RadAjaxPanel></div></div></div><div class="cf"></div>
                                </div>
                            </div>
                            <div style="clear: both;"></div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnBatch" runat="server" />
    <asp:HiddenField ID="hdnTransID" runat="server" />
    <asp:HiddenField ID="hdnStatus" runat="server" />
    <asp:HiddenField ID="hdnItemJSON" runat="server" />
    <asp:HiddenField ID="hdnTotal" runat="server" />
    <asp:HiddenField ID="hdnRowField" runat="server" />
    <asp:HiddenField ID="hdnPOLimit" runat="server" />
    <div style="display: none;">
        <a href="#" runat="server" value="Add New" id="btnAddNew"></a>
        <asp:ModalPopupExtender ID="mpeBomItem" BackgroundCssClass="ModalPopupBG" BehaviorID="mpeBomItem"
            runat="server" CancelControlID="btnCancel" OkControlID="btnOkay"
            TargetControlID="btnAddNew" PopupControlID="pnlTemplate"
            Drag="true" PopupDragHandleControlID="PopupHeader" OnOkScript="ReloadPage();">
        </asp:ModalPopupExtender>

        <div class="popup_Buttons" style="display: none">
            <input id="btnOkay" value="Done" type="button" /> <input id="btnCancel" value="Cancel" type="button" /> </div><div id="pnlTemplate" class="table-subcategory" >
            <div class="popup_Container">
                <div class="popup_Body">
                    <div class="model-popup-body" >
                        <div class="col-lg-12 col-md-12">
                            <div class="pc-title">
                                <asp:Label CssClass="title_text" Style="float: left" ID="lblBomItem" runat="server"> Add BOM Item </asp:Label><div style="float: right;">
                                    <ul class="lnklist-header">
                                        <li>
                                            <asp:LinkButton CssClass="icon-save" ID="lbtnItemSubmit" runat="server" ValidationGroup="item" ToolTip="Save"
                                                TabIndex="38" OnClick="lbtnItemSubmit_Click"></asp:LinkButton></li><li>
                                            <a id="lbtnClose" style="color: white" onclick="cancel();" class="icon-closed close_button_Form"></a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-12 col-md-12 com-cont-main">
                    <div class="com-cont">
                        <div class="row">
                            <div class="col-lg-12 col-md-12">
                                <asp:HiddenField ID="hdnJobId" runat="server" />
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Op Sequence</label> </div><div class="fc-input">
                                        <asp:TextBox ID="txtOpSeq" runat="server" CssClass="form-control" TabIndex="2" placeholder="Select Op Sequence"
                                            MaxLength="50" onkeyup="EmptyValue(this);"></asp:TextBox><asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="" TargetControlID="txtOpSeq"
                                                EnableCaching="False" ServiceMethod="GetCodeADDPO" UseContextKey="True" MinimumPrefixLength="0"
                                                CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListItemCssClass="autocomplete_listItem"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListElementID="lstAcct"
                                                ID="AutoCompleteExtender2" DelimiterCharacters="" CompletionInterval="250">
                                            </asp:AutoCompleteExtender>
                                        <div id="lstAcct"></div>
                                        <asp:RequiredFieldValidator ID="rfvOpSeq" ValidationGroup="item"
                                            runat="server" ControlToValidate="txtOpSeq" Display="None" ErrorMessage="Please enter Op Sequence"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceOpSeq" runat="server" Enabled="True"
                                                PopupPosition="TopLeft" TargetControlID="rfvOpSeq" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Type</label> </div><div class="fc-input">
                                        <asp:DropDownList ID="ddlBomType" runat="server" DataTextField="Type" CssClass="form-control" onchange="SetContextKey()">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvType" ValidationGroup="item"
                                            runat="server" ControlToValidate="ddlBomType" Display="None" ErrorMessage="Please select type"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceType" runat="server" Enabled="True"
                                                PopupPosition="BottomLeft" TargetControlID="rfvType" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Item</label> </div><div class="fc-input">
                                        <asp:TextBox ID="txtItem" runat="server" CssClass="form-control" TabIndex="2" placeholder="Select Item" onkeyup="SetContextKey()"
                                            MaxLength="50"></asp:TextBox><asp:HiddenField ID="hdnItemID" runat="server" />
                                        <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="" TargetControlID="txtItem"
                                            EnableCaching="False" ServiceMethod="GetItemsADDPO" UseContextKey="True" MinimumPrefixLength="0"
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
                                            SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceItem" runat="server" Enabled="True"
                                                PopupPosition="TopLeft" TargetControlID="rfvItem" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Description</label> </div><div class="fc-input">
                                        <asp:TextBox ID="txtJobDesc" runat="server" CssClass="form-control" TabIndex="2"
                                            MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator ID="rfvJobDesc" ValidationGroup="item"
                                                runat="server" ControlToValidate="txtJobDesc" Display="None" ErrorMessage="Please enter description"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceJobDesc" runat="server" Enabled="True"
                                                    PopupPosition="BottomLeft" TargetControlID="rfvJobDesc" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Qty Required</label> </div><div class="fc-input">
                                        <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" TabIndex="2"
                                            MaxLength="50" onkeypress="return isDecimalKey(this,event)"></asp:TextBox><asp:RequiredFieldValidator ID="rfvQty" ValidationGroup="item"
                                                runat="server" ControlToValidate="txtQty" Display="None" ErrorMessage="Please enter quantity required"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceQty" runat="server" Enabled="True"
                                                    PopupPosition="BottomLeft" TargetControlID="rfvQty" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>U/M</label> </div><div class="fc-input">
                                        <asp:TextBox ID="txtUM" runat="server" CssClass="form-control" TabIndex="2"
                                            MaxLength="50" placeholder="Select U/M"></asp:TextBox><asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="" TargetControlID="txtUM"
                                                EnableCaching="False" ServiceMethod="GetUMADDPO" UseContextKey="false" MinimumPrefixLength="0"
                                                CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListItemCssClass="autocomplete_listItem"
                                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                CompletionListElementID="lstUM"
                                                ID="AutoCompleteExtender1" DelimiterCharacters="" CompletionInterval="250">
                                            </asp:AutoCompleteExtender>
                                        <div id="lstUM"></div>
                                        <asp:RequiredFieldValidator ID="rfvUM" ValidationGroup="item"
                                            runat="server" ControlToValidate="txtUM" Display="None" ErrorMessage="Please select U/M"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceUM" runat="server" Enabled="True"
                                                PopupPosition="TopLeft" TargetControlID="rfvUM" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Budget Unit</label> </div><div class="fc-input">
                                        <asp:TextBox ID="txtBudgetUnit" runat="server" CssClass="form-control" TabIndex="2"
                                            MaxLength="50" onkeypress="return isDecimalKey(this,event)"></asp:TextBox><asp:RequiredFieldValidator ID="rfvBudgetUnit" ValidationGroup="item"
                                                runat="server" ControlToValidate="txtBudgetUnit" Display="None" ErrorMessage="Please enter Budget Unit"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="vceBudgetUnit" runat="server" Enabled="True"
                                                    PopupPosition="BottomLeft" TargetControlID="rfvBudgetUnit" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Budget Ext</label> </div><div class="fc-input pt-5" >
                                        <asp:Label ID="lblBudgetExt" runat="server" Text="0.00"></asp:Label></div></div></div></div></div></div></div></div></div><telerik:RadWindowManager ID="RadWindowManagerInv" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowWarehouse" Skin="Material" VisibleTitlebar="true" Title="Select Op Squence" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="300" Height="200" OnClientClose="OnClientCloseHandler">
                <ContentTemplate>
                    <div id="opDiv">
                    </div>
                    <div style="clear: both;"></div>

                    <div class="btnlinks">
                        <a href="javascript:void(0);" onclick="SetOpToHiddenField();">Save Changes</a> &nbsp;&nbsp; <a href="javascript:void(0);" onclick="SetOpToHiddenPop();">Cancel</a> </div></ContentTemplate></telerik:RadWindow></Windows></telerik:RadWindowManager><asp:HiddenField runat="server" ID="hdOpSeqID" />
    <asp:HiddenField runat="server" ID="hdLineNo" />
    <asp:HiddenField runat="server" ID="hdnInvDefaultAcctID" Value="" />
    <asp:HiddenField runat="server" ID="hdnInvDefaultAcctName" Value="" />
    <asp:HiddenField runat="server" ID="hdnCon" />
    <asp:HiddenField runat="server" ID="hdnSelectPOIndex" />

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <%--<script defer src="https://use.fontawesome.com/releases/v5.0.10/js/all.js"></script>--%>
    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>

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
        function ConfirmUpload(value) {
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
        }

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

            $(window).scroll(function () {
                if ($(window).scrollTop() >= 0) {
                    $('#divButtons').addClass('fixed-header');
                }
                if ($(window).scrollTop() <= 0) {
                    $('#divButtons').removeClass('fixed-header');
                }
            });
 
        });
        function pageLoadLog(sender, args) {
            Materialize.updateTextFields();
        }

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

        function SetOpToHiddenField() {
            var selectedRow = $("#<%=hdOpSeqID.ClientID%>").val();
            var selectedLineNo = $("#<%=hdLineNo.ClientID%>").val();
            var selectedCode = $('input[name=opSquence]:checked').val();
            $("#" + selectedRow).val(selectedCode);
            var lineItem = $("#lineItem" + selectedCode).val();

            $("#" + selectedLineNo).val(lineItem);

            var radwindow = $find('<%=RadWindowWarehouse.ClientID %>');
            radwindow.close();
            $("#" + selectedLineNo).val(lineItem);
        }

        $(document).ready(function () {
            CalculateTotalAmt();
        });


        function CopyProjctCode() {

            var txtProject = $("#<%=txtProject.ClientID%>").val();
            var projectID = $("#<%=hdnmainjobID.ClientID%>").val();

            var txtCode = $("#<%=txtCode.ClientID%>").val();
            var codeID = $("#<%=hdnMainCode.ClientID%>").val();

            var hdMainGvLoc = $("#<%=hdMainGvLoc.ClientID%>").val();
            var hdMainAcctID = $("#<%=hdMainAcctID.ClientID%>").val();

            var hdMainGvAcctNo = $("#<%=hdMainGvAcctNo.ClientID%>").val();
            var txtDueDate = $("#<%=txtDueDate.ClientID%>").val();

            var masterTable = $find("<%=RadGrid_AddPO.ClientID%>").get_masterTableView();
            var count = masterTable.get_dataItems().length;
            var item;
            for (var i = 0; i < count; i++) {
                item = masterTable.get_dataItems()[i];
                var txtGvJob = item.findElement("txtGvJob");
                $(txtGvJob).val(txtProject);

                var hdnJobID = item.findElement("hdnJobID");
                $(hdnJobID).val(projectID);

                var txtGvPhase = item.findElement("txtGvPhase");
                $(txtGvPhase).val(txtCode);

                var hdnPID = item.findElement("hdnPID");
                $(hdnPID).val(codeID);

                var txtGvAcctNo = item.findElement("txtGvAcctNo");
                var hdnAcctID = item.findElement("hdnAcctID");

                if (txtCode == 'Inventory') {


                    $(txtGvAcctNo).val(document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value);
                    $(hdnAcctID).val(document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value);

                }
                else {


                    $(txtGvAcctNo).val(hdMainGvAcctNo);

                    var txtGvLoc = item.findElement("txtGvLoc");
                    $(txtGvLoc).val(hdMainGvLoc);

                    var txtGvDue = item.findElement("txtGvDue");
                    $(txtGvDue).val(txtDueDate);


                    $(hdnAcctID).val(hdMainAcctID);

                }

                var hdnTypeId = item.findElement("hdnTypeId");
                $(hdnTypeId).val(codeID);
            }
        }

        function pageLoad(sender, args) {
            Materialize.updateTextFields();
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.Acct = null;
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

            $("[id*=txtGvDue]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvDue', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });

            $("[id*=txtGvLoc]").focus(function () {
                var txtGvJob = $(this).attr('id');
                var hdnIndex = document.getElementById(txtGvJob.replace('txtGvLoc', 'hdnIndex'));
                var hdnSelectPOIndex = document.getElementById('<%=hdnSelectPOIndex.ClientID%>');

                $(hdnSelectPOIndex).val(hdnIndex.value);
            });--%>

            $("#<%=RadGrid_AddPO.ClientID%> tbody tr input:text, #<%=RadGrid_AddPO.ClientID%> tbody tr input:checkbox, #<%=RadGrid_AddPO.ClientID%> tbody tr select").on("focus", function (e) {
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
            <%--$("[id*=txtGvJob]").unbind("focusout").bind("focusout", function () {
                //debugger;
                var txtGvJob = $(this).attr('id');
                var hdnJobID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnJobID'));
                var strJobsVal = $(this).val();
                if ($(hdnJobID).val() != "" & $(hdnJobID).val() != "0") {
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

                                        $(txtGvAcctNo).val(strAcct);
                                        $(hdnAcctID).val(ui[0].DA);
                                        //$(txtGvAcctName).val(ui[0].DefaultAcct);
                                    }
                                },
                                error: function (result) {
                                    alert("Due to unexpected errors we were unable to load default acct#");
                                }
                            });
                        }
                    }

                    var txtGvDue = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvDue'));
                    var txtDueDate = document.getElementById('<%=txtDueDate.ClientID%>');

                    if (txtGvDue.value == '') {
                        $(txtGvDue).val(txtDueDate.value);
                    }
                }                                                             
            });--%>

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
                var txtGvDue = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvDue'));
                var txtDueDate = document.getElementById('<%=txtDueDate.ClientID%>');

                if (txtGvDue.value == '') {
                    $(txtGvDue).val(txtDueDate.value);
                }
            });


            //$("[id*=txtGvAcctNo]").focusout(function () {
            $("[id*=txtGvAcctNo]").unbind("focusout").bind("focusout",function () {
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
                                    var strAcct = ui[0].Acct + ' - ' + ui[0].DefaultAcct;
                                    $(txtGvAcctNo).val(strAcct);
                                    $(hdnAcctID).val(ui[0].GLExp);
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
                                        var strAcct = ui[0].Acct + ' - ' + ui[0].DefaultAcct;
                                        $(txtGvAcctNo).val(strAcct);
                                        $(hdnAcctID).val(ui[0].DA);
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
            //$("[id*=txtGvAcctNo]").change(function () {
            $("[id*=txtGvAcctNo]").unbind("change").bind("change",function () {
                var txtGvAcctNo = $(this);
                var strAcctNo = $(this).val();

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
                        $(hdnAcctID).val(ui.item.value);
                        $(this).val(strAcct);
                    }

                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.acct);

                    return false;
                },
                //change: function (event, ui) {

                //var txtGvAcctNo = this.id;
                //var hdnAcctID = document.getElementById(txtGvAcctNo.replace('txtGvAcctNo', 'hdnAcctID'));
                //var strAcct = document.getElementById(txtGvAcctNo).value;

                //if (strAcct == '') {
                //    $(hdnAcctID).val('')
                //}
                //},
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
                        return '<span class="highlight">' + FullMatch + '</span>';
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>';
                        });
                    }

                    if (result_value == 0) {

                    }
                    else {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
            });

            $("[id*=txtGvJob]").autocomplete({

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

                    var AcctStatus = ui.item.Status;
                    if (AcctStatus == "0") {

                        $(hdnJobID).val(ui.item.ID);
                        var jobStr = ui.item.ID + ", " + ui.item.fDesc;
                        $(this).val(jobStr);
                        $(txtGvLoc).val(ui.item.Tag);
                        //$(hdnAcctID).val(ui.item.GLExp);
                        //var strAcct = ui.item.Acct + ' - ' + ui.item.DefaultAcct;
                        //$(txtGvAcctNo).val(strAcct);
                        // HideGridTicketColums("true");
                        //debugger

                        var varPhase = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvPhase')).value;
                        var InvDefaultAcctID = document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value;
                        var InvDefaultAcctName = document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value;
                        if (varPhase == 'Inventory') {
                            document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvPhase')).value = '';
                            document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvAcctNo')).value = '';
                            //document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvItem')).value = '';
                            document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvWarehouse')).value = '';
                            document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvWarehouseLocation')).value = '';
                            //document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvDesc')).value = '';
                            //document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvPrice')).value = '';
                            $(hdnAcctID).val(ui.item.GLExp);
                            var strAcct = ui.item.Acct + ' - ' + ui.item.DefaultAcct;
                            $(txtGvAcctNo).val(strAcct);
                        }
                        else {
                            $(hdnAcctID).val(ui.item.GLExp);
                            var strAcct = ui.item.Acct + ' - ' + ui.item.DefaultAcct;
                            $(txtGvAcctNo).val(strAcct);
                        }
                        $('#hdnIsAutoCompleteSelected').val('1');
                    }
                    else {
                        noty({
                            text: ' GL account is Inactive.',
                            type: 'warning',
                            layout: 'topCenter',
                            closeOnSelfClick: false,
                            timeout: 5000,
                            theme: 'noty_theme_default',
                            closable: true
                        });
                    }
                    return false;
                },
                change: function (event, ui) {
                    if (!ui.item) {
                        $(this).val("");
                        return false;
                    }
                    var txtGvJob = this.id;
                    var hdnJobID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnJobID'));
                    var strJob = document.getElementById(txtGvJob).value;

                    if (strJob == '') {
                        $(hdnJobID).val('');
                    }
                },
                focus: function (event, ui) {
                    //$(this).val(ui.item.fDesc);
                    var jobStr = ui.item.ID + ", " + ui.item.fDesc;
                    $(this).val(jobStr);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            });
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

                    //if (result_value != null) {
                    //    result_value = result_value.toString().replace(x, function (FullMatch, n) {
                    //        return '<span class="highlight">' + FullMatch + '</span>'
                    //    });
                    //}

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
                }
            });

            $("[id*=txtGvJob]").unbind("change").bind("change",function () {
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

            $("[id*=txtGvPhase]").autocomplete({

                source: function (request, response) {

                    var curr_control = this.element.attr('id');
                    var hdnJobContr = document.getElementById(curr_control.replace('txtGvPhase', 'hdnJobID'));
                  
                    var prefixText = request.term;
                    var job = document.getElementById(hdnJobContr.id).value;
                    //alert(job.innerHTML);
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
                    var hdntxtGvPhase = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdntxtGvPhase'));
                    var hdOpSq = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdOpSq'));
                    var txtGvDesc = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvDesc'));
                    var str = ui.item.TypeName;
                    if (str == "No Record Found!") {
                        $(this).val("");
                    }
                    else {

                        $(hdnTypeId).val(ui.item.Type);
                        console.log(hdnTypeId.value);
                        $(hdOpSq).val(ui.item.Code);
                        $(this).val(ui.item.TypeName);
                        $(hdntxtGvPhase).val(ui.item.TypeName);
                        $(txtGvDesc).val(ui.item.Desc);
                        console.log(hdntxtGvPhase.value);
                        var txtGvAcctNo;
                        var hdnAcctID;
                        var txtGvWarehouse;
                        var txtGvWarehouseLocation;
                        
                        if (ui.item.TypeName == "Inventory") {
                            try {
                                //HideGridColums("true");
                                //do inventory default account
                                txtGvAcctNo = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvAcctNo'));
                                hdnAcctID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnAcctID'));
                                $(txtGvAcctNo).val(document.getElementById('<%=hdnInvDefaultAcctName.ClientID%>').value);
                                $(hdnAcctID).val(document.getElementById('<%=hdnInvDefaultAcctID.ClientID%>').value);
                                txtGvWarehouse = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouse'));
                                txtGvWarehouseLocation = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouseLocation'));
                                txtGvWarehouse.readOnly = false;
                                txtGvWarehouseLocation.readOnly = false;
                                txtGvItem = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvItem'));
                                txtGvDesc = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvDesc'));
                                $(txtGvItem).val('');
                                $(txtGvDesc).val('');
                            } catch (e){ }
                        }
                        else {
                            // HideGridColums("false");
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
                            } catch (e){ }
                        }
                    }
                    return false;
                },
                focus: function (event, ui) {

                    $(this).val(ui.item.TypeName);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val());
            });

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
                        
            //   -----------------------------------------------
            //$("[id*=txtGvItem]").change(function () {
            $("[id*=txtGvItem]").unbind("change").bind("change",function () {
                //debugger
                var txtGvItem = $(this);
                var strItem = $(this).val();

                var txtGvItem1 = $(txtGvItem).attr('id');
                var hdnTypeId = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnTypeId'));
                var hdnPID = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnPID'));
                var txtGvItem = document.getElementById(txtGvItem1.replace('txtGvItem', 'txtGvItem'));
                var hdnItemID = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnItemID'));
                var txtGvDesc = document.getElementById(txtGvItem1.replace('txtGvItem', 'txtGvDesc'));
                var job = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnJobID')).value;
                var txtGvPrice = document.getElementById(txtGvItem1.replace('txtGvItem', 'txtGvPrice'));
                var typeId = $(hdnTypeId).val();
                if (strItem != "") {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        // url: "AccountAutoFill.asmx/GetAutoFillItemPO",
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
                                //TODO: fill value for Price
                                if (typeId === "8") //Inventory
                                {
                                    $(txtGvPrice).val(ui[0].Price);
                                }
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
            //$("[id*=txtGvPhase]").change(function () {
            $("[id*=txtGvPhase]").unbind("change").bind("change",function () {
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
                var txtGvWarehouse = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'txtGvWarehouse'));
                var txtGvWarehouseLocation = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'txtGvWarehouseLocation'));
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
                                        
                                        $(txtGvWarehouse).attr('readOnly', false);
                                        $(txtGvWarehouseLocation).attr('readOnly', false);
                                        
                                        $(txtGvItem).val('');
                                        $(txtGvDesc).val('');
                                    } catch (e){ }
                                }
                                else {
                                    // HideGridColums("false");
                                    try {
                                        //txtGvWarehouse = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouse'));
                                        //txtGvWarehouseLocation = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvWarehouseLocation'));
                                        //txtGvWarehouse.readOnly = true;
                                        //txtGvWarehouseLocation.readOnly = true;
                                        $(txtGvWarehouse).attr('readOnly', true);
                                        $(txtGvWarehouseLocation).attr('readOnly', true);
                                        $(txtGvWarehouse).val('');
                                        $(txtGvWarehouseLocation).val('');
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

            // Focus out and autocomplete
            //$("[id*=txtGvPhase]").focusout(function () {
            $("[id*=txtGvPhase]").unbind("focusout").bind("focusout",function () {
                $(this).change();
                //alert('Azhar');

                var txtGvPhase = $(this).attr('id');
                var txtGvAcctNo = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvAcctNo'));
                var strAcctNo = $(txtGvAcctNo).val();

                var txtGvAcctNo1 = $(txtGvAcctNo).attr('id');
                var hdnAcctID = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'hdnAcctID'));

                
                if ($(this).val() == '') {
                    $(txtGvAcctNo).val('');
                    $(hdnAcctID).val('0');
                }

                


                <%--var txtGvPhaseId = $(this).attr('id');
                var strPhase = $(this).val();
                var hdnTypeId = document.getElementById(txtGvPhaseId.replace('txtGvPhase', 'hdnTypeId'));
                
                if (strPhase != '') {
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
                            debugger
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
                                        
                                        $(txtGvWarehouse).attr('readOnly', false);
                                        $(txtGvWarehouseLocation).readOnly = false;
                                    } catch (e){ }
                                }
                                else {
                                    // HideGridColums("false");
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
                                    } catch (e){ }
                                }
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Type");
                        }
                    });
                }--%>
            });

            $("[id*=txtGvItem]").autocomplete({
                source: function (request, response) {
                    var curr_control = this.element.attr('id');
                    var job = document.getElementById(curr_control.replace('txtGvItem', 'hdnJobID')).value;
                    var typeId = document.getElementById(curr_control.replace('txtGvItem', 'hdnTypeId')).value;
                    // var hdnOpSq = document.getElementById(curr_control.replace('txtGvItem', 'hdnOpSq')).value;
                    var prefixText = request.term;

                    query = request.term;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",

                        // url: "AccountAutoFill.asmx/GetPhaseExpByJobTypeOpSequence",
                        url: "AccountAutoFill.asmx/GetPhaseByInventoryItem",
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
                    var txtGvPrice = document.getElementById(curr_control.replace('txtGvItem', 'txtGvPrice'));
                    var hdnTypeId = document.getElementById(curr_control.replace('txtGvItem', 'hdnTypeId'));
                    var typeId = $(hdnTypeId).val();

                    var strId = ui.item.ItemID;
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

                    if (strId == null || strId == "0") {
                        $(this).val("");
                        $(hdnItemID).val("");
                        $(hdnPID).val("");
                        <%--  $("#<%=hdnJobId.ClientID%>").val(job);
                        $("#<%=hdnRowField.ClientID%>").val(curr_control);
                        var modal = $find("mpeBomItem");
                        modal.show();--%>
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
                            $(txtGvDesc).val(ui.item.ItemDesc);
                        }
                        if (typeId === "8") // Inventory items
                        {
                            console.log("Inventory item price: " + ui.item.Price);
                            $(txtGvPrice).val(ui.item.Price);
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
            });

            $.each($(".pisearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
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
                    }
                    catch (e){ }

                    if (result_line == "0") {
                        if (result_itemfdesc != null && result_itemfdesc != "")
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a>  " + result_item + ", <span style='color:Gray;'><b>  </b>" + result_itemfdesc + "</span></a>")
                                .appendTo(ul);
                        else
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<a>  " + result_item + "</a>")
                                .appendTo(ul);
                    }
                    else {
                        if (result_item == undefined) {
                            result_item = item.ItemDesc;
                        }

                        if (result_itemfdesc != null && result_itemfdesc != "")
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<span class='auto_item'><i style='display:inline-block; margin-right:8px;width:auto;color:#1565C0 !important;' class='fa fa-check-square' title=''></i>" + result_item + ", <span style='color:Gray;'><b>  </b>" + result_itemfdesc + "</span></span>")
                                .appendTo(ul);
                        else
                            return $("<li></li>")
                                .data("item.autocomplete", item)
                                .append("<span class='auto_item'><i style='display:inline-block; margin-right:8px;width:auto;color:#1565C0 !important;' class='fa fa-check-square' title=''></i>" + result_item + "</span>")
                                .appendTo(ul);
                    }
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
                            alert("Due to unexpected errors we were unable to load location details");
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
                    //$('#hdnIsAutoCompleteSelected').val('1');
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
                            .append("<a><b> Project: </b> " + result_value + ", " + result_item + ", <span style='color:Gray;'><b> Loc: </b>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    }
                };
            });

            function dtInv() {
                this.prefixText = null;
                this.con = null;
                this.InvID = null;
            }
            //txtGvWarehouse
            $("[id*=txtGvWarehouse]").autocomplete({

                

                source: function (request, response) {

                    var txtGvWarehouse_GetID = $(this.element).attr("id");
                    var hdnInvID = document.getElementById(txtGvWarehouse_GetID.replace('txtGvWarehouse', 'hdnItemID'));
                    var hdntxtGvPhase = document.getElementById(txtGvWarehouse_GetID.replace('txtGvWarehouse', 'hdntxtGvPhase'));
                    if (hdntxtGvPhase.value != "Inventory") { return; }

                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    var str = request.term;
                    var ID = $(hdnInvID).val();
                    dtaaa.InvID = ID;
                    dtaaa.isShowAll = "yes";
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        //url: "AccountAutoFill.asmx/GetJobLocations",
                        //data: '{"prefixText": "' + dtaaa.prefixText + '", "IsJob": "' + true + '", "con": "' + dtaaa.con + '"}',
                        url: "AccountAutoFill.asmx/GetWarehouseName",
                        data: JSON.stringify(dtaaa),
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
                //    try {
                //        $(".ui-autocomplete").mCustomScrollbar("destroy");
                //    }
                //    catch (e) { }
                //},
                //source: function (request, response) {
                //    debugger
                //    var dtaaa = new dtInv();
                //    dtaaa.prefixText = request.term;
                //    query = request.term;

                //    var str = request.term;

                //    //var txtGvWarehouse_GetID = $(this.element).attr("id");
                //    //var hdnInvID = document.getElementById(txtGvWarehouse_GetID.replace('txtGvWarehouse', 'hdnItemID'));
                //    //var hdntxtGvPhase = document.getElementById(txtGvWarehouse_GetID.replace('txtGvWarehouse', 'hdntxtGvPhase'));
                //    //if (hdntxtGvPhase.value != "Inventory") { return; }

                //    console.log(hdntxtGvPhase.value);

                //    var ID = $(hdnInvID).val();
                //    dtaaa.InvID = ID;
                //    dtaaa.isShowAll = "yes";
                //    $.ajax({
                //        type: "POST",
                //        contentType: "application/json; charset=utf-8",
                //        url: "AccountAutoFill.asmx/GetWarehouseName",
                //        data: JSON.stringify(dtaaa),
                //        dataType: "json",
                //        async: true,
                //        success: function (data) {

                //            response($.parseJSON(data.d));

                //        },
                //        error: function (result) {
                //            alert("Due to unexpected errors we were unable to load account name");
                //        }
                //    });
                //},
                select: function (event, ui) {
                    try {
                        var txtGvWarehouse = this.id;
                        var hdnWarehouse = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnWarehouse'));
                        var hdnWarehousefdesc = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnWarehousefdesc'));
                        var hdnInvID = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnItemID'));

                        var Str = ui.item.WarehouseID + ", " + ui.item.WarehouseName;

                        $(this).val(Str);
                        $(txtGvWarehouse).val(Str);
                        $(hdnWarehouse).val(ui.item.WarehouseID);
                        $(hdnWarehousefdesc).val(Str);

                        var locationID = 0;
                        var warehouseID = $(hdnWarehouse).val();
                        var invID = $(hdnInvID).val();

                    } catch (e){ }
                    return false;
                },
                focus: function (event, ui) {
                    try {
                        $(this).val(ui.item.WarehouseID);
                    } catch (e){ }
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

                    var hdntxtGvPhase = document.getElementById(txtGvWarehouseLocation_GetID.replace('txtGvWarehouseLocation', 'hdntxtGvPhase'));
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
                        var Str = ui.item.ID + ", " + ui.item.Name;
                        $(this).val(Str);
                        $(hdnLocationfdesc).val(Str);
                        $(txtGvWarehouseLocation).val(Str);
                        $(hdnWarehouseLocationID).val(ui.item.ID);

                        var locationID = $(hdnWarehouseLocationID).val();
                        var warehouseID = $(hdnWarehouse).val();
                        var invID = $(hdnInvID).val();
                    } catch (e){ }



                    return false;
                },
                focus: function (event, ui) {
                    try {
                        $(this).val(ui.item.ID);
                    } catch (e){ }
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
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

            $("#<%=tbDocuments.ClientID%>").addClass("active");
            $("#accrdDocuments").addClass("active");
            $("#<%=tbDocuments.ClientID%> > div.collapsible-body").attr('style', 'display:block;');

            function clearPhase(txt) {
                try {
                    if ($(txt).val() == '') {
                        var hdnPID = document.getElementById(txt.id.replace('txtGvPhase', 'hdnPID'));
                        $(hdnPID).val('0');
                    }
                } catch (e){ }
            }

            ///////////// Quick Codes //////////////
            $("#<%=txtShipTo.ClientID%>").keyup(function (event) {
                debugger
                replaceQuickCodes(event, '<%=txtShipTo.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });

            $("#<%=txtDesc.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtDesc.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });
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
             var txtInvoiceDate = $("#<%=txtDate.ClientID%>");

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
                 txtDueDate.val( addDays(txtInvoiceDate.val(), 270));
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
    <script type="text/javascript">
             Sys.Application.add_init(appl_init);

             function appl_init() {

                 var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance();
                 pgRegMgr.add_beginRequest(BlockUI);
                 pgRegMgr.add_endRequest(UnblockUI);
             }

             function BlockUI(sender, args) {
                var control =document.getElementById('<%=btnSubmit.ClientID%>');
                 if (args._postBackElement.id == control.id) {
                     document.getElementById("overlay").style.display = "block";
                 }
                // document.getElementById("overlay").style.display = "block";
             }
             function UnblockUI(sender, args) {               
                document.getElementById("overlay").style.display = "none";

         }
         
         function ClosePOFunction() {
             var checkBox = document.getElementById('<%=chkPOClose.ClientID%>');
             var text = document.getElementById('<%=txtStatus.ClientID%>');
             if (checkBox.checked == true) {
                 if (text.value == 'Open') {
                     checkBox.checked = false;
                     noty({
                         text: 'Bill/\RPO doesn\'t exist!',
                         type: 'warning',
                         layout: 'topCenter',
                         closeOnSelfClick: false,
                         timeout: 5000,
                         theme: 'noty_theme_default',
                         closable: true
                     });
                     
                 }
             } else {
                 
             }
         }
    </script>
</asp:Content>



