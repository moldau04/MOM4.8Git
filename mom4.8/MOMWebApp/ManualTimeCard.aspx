<%@ Page Language="C#" Title="Timesheet || MOM" AutoEventWireup="true" EnableEventValidation="false" Inherits="ManualTimeCard" MasterPageFile="~/Mom.master" Codebehind="ManualTimeCard.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .RadGrid_Material th
        a {
            padding: 0px!important;
            font-size: 0.75rem!important;
        }
       
        .rgHeader {
            padding: 15px 8px!important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="topNav h-65">
        <div id="breadcrumbs-wrapper">
            <header>
                <div class="container row-color-grey">
                    <div class="row">
                        <div class="col s12 m12 l12">
                            <div class="row">
                                <div class="page-title"><i class="mdi-social-people"></i>&nbsp;<asp:Label ID="lblHeader" runat="server">Time Card Input</asp:Label></div>
                                <div class="buttonContainer">
                                    <div class="btnlinks">
                                        <a class="dropdown-button" data-beloworigin="true" href="#!" id="btnSave">Save</a>
                                    </div>
                                    <div class="btnlinks">
                                        <a class="dropdown-button" data-beloworigin="true" href="#!" id="btnClear">Clear</a>
                                    </div>
                                </div>
                                <div class="btnclosewrap" id="divClose" runat="server">
                                     <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click">  <i class="mdi-content-clear"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </header>
        </div>
        <div class="mb-10" ></div>
        <div class="container mb-10">
            <div class="row">
                <div class="srchpane">
                    <div class="srchpaneinner">
                        <div class="col s12 m12 l12">
                            <div class="row">
                                <div class="srchinputwrap">
                                    <label class="drpdwn-label">Supervisor</label>
                                    <select class="browser-default selectst" id="ddlSuper">
                                        <option value="">Select Supervisor</option>
                                    </select>
                                </div>
                                <div class="srchinputwrap">
                                    <label class="drpdwn-label">Worker <span class="reqd">*</span></label>
                                    <select class="browser-default selectst" id="ddlWorker">
                                        <option value="">Select Worker</option>
                                    </select>
                                </div>
                                <div class="srchinputwrap">
                                    <label class="drpdwn-label">Category <span class="reqd">*</span></label>
                                    <select class="browser-default selectst" id="ddlCategory" onchange="updateCategory();">
                                        <option value="">Select Category</option>
                                        <option value="None">None</option>
                                    </select>
                                </div>
                                <div class="srchinputwrap">
                                    <br />
                                    <span class="css-checkbox">
                                        <input type="checkbox" name="mark" id="markReviewed" class="css-checkbox" />
                                        <label for="markReviewed" class="css-label">Reviewed</label></span>
                                </div>
                                <div class="srchinputwrap">
                                    <br />
                                    <span class="css-checkbox">
                                        <input type="checkbox" name="mark" id="timesheet" class="css-checkbox" disabled />
                                        <label for="timesheet" class="css-label">Timesheet</label></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container mb-10">
            <div class="grid_container">
                <div class="form-section-row mb">
                    <div class="RadGrid RadGrid_Material">
                        <table class="rgMasterTable rgClipCells re-css" border="0"  id="tblInput">
                            <thead>
                                <tr>
                                    <th scope="col" class="rgHeader date-css" title="Drag to resize" >
                                       <span class="err">*</span> <a>Date</a>
                                    </th>
                                    <th scope="col" class="rgHeader s-date-css" title="Drag to group or reorder" style="padding:5px 8px!important;">
                                        <span class="err">*</span><a>Start time</a>
                                    </th>
                                    <th scope="col" class="rgHeader work-css" title="Drag to group or reorder" ><a>Work Desc</a>
                                    </th>
                                    <th scope="col" class="rgHeader cat-css" >
                                     <span class="err">*</span> <a>Category</a>
                                    </th>
                                    <th scope="col" class="rgHeader reg-css " ><a>Reg</a>
                                    </th>
                                    <th scope="col" class="rgHeader ot-css" title="Drag to group or reorder"><a>OT</a>
                                    </th>
                                    <th scope="col" class="rgHeader nu-css"  ><a>1.7</a>
                                    </th>
                                    <th scope="col" class="rgHeader dt-css" title="Drag to group or reorder"><a>DT</a>
                                    </th>
                                    <th scope="col" class="rgHeader tr-css" ><a>Travel</a>
                                    </th>
                                    <th scope="col" class="rgHeader mil-css"  ><a>Misc</a>
                                    </th>
                                    <th scope="col" class="rgHeader zon-css" ><a>Zone</a>
                                    </th>
                                    <th scope="col" class="rgHeader re-css" ><a>Toll</a>
                                    </th>
                                    <th scope="col" class="rgHeader pro-css" >
                                        <span class="err">*</span><a>Project</a>
                                    </th>                                   

                                    <th scope="col" class="rgHeader ty-css" >
                                        <span class="err">*</span><a>Type</a>
                                    </th>

                                     <th scope="col" class="rgHeader gr-css"><a>Group</a>
                                    </th>

                                      <th scope="col" class="rgHeader gr-css" style="display:none;"><a>Group</a>
                                    </th>

                                     <th scope="col" class="rgHeader op-css" style="padding:5px 2px!important;" ><a>Op Sequence</a>
                                    </th>

                                    <th scope="col" class="rgHeader code-css" style="padding:5px 7px!important;"><a>Code Desc</a>
                                    </th>

                                    <th scope="col" class="rgHeader wa-css" >
                                        <span class="err">*</span><a>Wage</a>
                                    </th>
                                   
                                    <th scope="col" class="rgHeader equ-css" ><a>Equipment</a>
                                    </th>
                                    <th scope="col" class="rgHeader wo-css" ><a>WO# </a>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                
                            </tbody>
                            <tfoot>
                                <tr class="rgFooter">
                                    <td colspan="4">Total:
                                        <label id="ttime">0.00</label></td>
                                    <td class="re-fo-css" >
                                        <label id="treg">0.00</label></td>
                                    <td class="re-fo-css" >
                                        <label id="tot">0.00</label></td>
                                    <td class="re-fo-css" >
                                        <label id="tnt">0.00</label></td>
                                    <td class="re-fo-css" >
                                        <label id="tdt">0.00</label></td>
                                    <td class="re-fo-css" >
                                        <label id="ttravel">0.00</label></td>
                                    <td class="re-fo-css" >
                                        <label id="tmiles">0.00</label></td>
                                    <td class="re-fo-css" >
                                        <span>$</span><label id="tzone">0.00</label></td>
                                    <td class="re-fo-css" >
                                        <span>$</span><label id="treimb">0.00</label></td>
                                    <td style="white-space: nowrap;" colspan="8"></td>
                                </tr>
                            </tfoot>
                        </table>
                        <div>
                            <i class="mdi-content-add-circle mdi-add-css"  id="addRow"></i>
                            <i class="mdi-navigation-cancel mdi-can-css"  id="rmRow"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        .RadGrid .rgFilterRow > td {
            padding-left: 2px !important;
            padding-right: 2px !important;
            padding-bottom: 0px !important;
            padding-top: 0px !important;
        }

        .popwidth {
            width: 700px;
            height: 300px;
            margin-left: 50px;
        }

        .ui-datepicker-calendar {
            display: none;
        }

        .centerDiv {
            margin: auto;
            width: 50%;
            border: 1px solid #7b867b;
            padding: 10px;
            position: absolute;
            top: 40px;
            bottom: 0;
            left: 0;
            right: 0;
        }

        #overlay {
            position: fixed;
            display: none;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0,0,0,0.5);
            z-index: 1000000;
            cursor: pointer;
        }

        .center {
            margin: auto;
            width: auto;
            padding: 20px;
            box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);
            position: absolute;
            background-color: #fff;
        }

        .err {
            color: red;
            position: absolute;
            margin-left: -8px;
        }

        .selectnew {
            height: 27px !important;
            margin-top: 0px !important;
            margin-bottom: 0 !important;
            width: 100%;
            font-size: 0.9rem !important;
        }

        .RadGrid .rgFooter > td {
            padding-left: 13px !important;
        }

        .RadGrid .rgFilterRow > td > input {
            margin-left: 12px !important;
        }
    </style>

    <div id="DivEqup" class="popup_div popwidth centerDiv">
           <div class="btnlinks m-b-5">
            <a href="javascript:void(0);" id="popupAdd" onclick="addEquipment();">Add</a>
        </div>
        <div class="btnlinks m-close" >
            <a href="javascript:void(0);" id="popupClose" onclick="popupClose();">Close</a>
        </div>
        <div class="grid_container">
            <div class="RadGrid RadGrid_Material FormGrid">
                <table class="rgMasterTable rgClipCells table-add-css" border="0" >
                    <thead>
                        <tr>
                            <th>
                                <input type="checkbox" name="chkAll" style="margin-left: 6px;" /></th>
                            <th>Name</th>
                            <th>Unique#</th>
                            <th>Description</th>
                            <th>Type</th>
                            <th>Category</th>
                            <th>Service Type</th>
                            <th>Building</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody id="tblEquip">
                    </tbody>
                </table>
            </div>
        </div>
    </div>

    <div id="overlay" style="display: none;">
        <img src="images/wheel.GIF" alt="Be patient..." class="lodder" >
    </div>

    <script src="Scripts/jquery.timeentry.min.js"> 

    </script>

    <script type="text/javascript">
         var categoryArr;
        var SelectedRow = null;
        var PrevRow = null;
        var SelectedCellIndex = null;

        function showLoader() {
            $('#overlay').attr('style', 'display:block');
        }

        function hideLoader() {
            $('#overlay').attr('style', 'display:none');
        }

        function twoDecimal(e) {
            var value = parseFloat($(e).val()) + 0 / 100;
            $(e).val(value.toFixed(2));
        }

        function twoDecimalDoller(e) {
            var value = parseFloat($(e).val().replace('$', '')) + 0 / 100;
            $(e).val('$' + value.toFixed(2));
        }

        $(document).ready(function () {
            //debugger
             showLoader();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "TimeCardService.asmx/GetInputCardService",
                data: {},
                dataType: "json",
                async: true,
                success: function (data) {
                    //debugger
                    superArr = data.d.lstSuperVisor;
                    workerArr = data.d.lstWorker;
                    categoryArr = data.d.lstCategory;
                    $.each(data.d.lstSuperVisor, function (key, value) {
                        $("<option/>").val(value.FDesc).text(value.FDesc).appendTo("#ddlSuper");
                    });
                    $.each(data.d.lstWorker, function (key, value) {
                        $("<option/>").val(value.FDesc).text(value.FDesc).appendTo("#ddlWorker");
                    });
                    $.each(data.d.lstCategory, function (key, value) {
                        $("<option/>").val(value.Type).text(value.Type).appendTo("#ddlCategory");
                    });
                       hideLoader();                  
                  Cleartable();
                },
                error: function (result) {
                    hideLoader();
                    alert("408");
                }
            });
           
            $('input[name=time]').timeEntry();
            $('input[name=date]').datepicker({ dateFormat: 'mm/dd/yy' });
            $('input[name=date]').datepicker('setDate', new Date());

            $('#markReviewed').change(function () {
                if ($(this).is(":checked")) {
                    $('#timesheet').removeAttr('disabled');
                }
                else {
                    $('#timesheet').attr('checked', false);
                    $('#timesheet').attr('disabled', true);
                }
            });

            $(document).keydown(function (e) {
                //increase 1
                var isfirst = false;
                if (e.which == 40) {
                   // debugger;
                    if (!isfirst) {
                        if (!$('input[name=time]').is(':focus')) {
                            isfirst = true;
                             var obj = document.activeElement;
                            if (obj != null &&( obj.name == 'project' || obj.name == 'type' || obj.name == 'wage')) {
                               return false;
                            } else {
                                 addRow();
                            }
                
                        }
                    }
                }
                // case F6
                if (e.which == 117) {
                    if (PrevRow != null && PrevRow.length > 0) {
                        var prevTDs = $(PrevRow).children();
                        var selectedTDs = $(SelectedRow).children();
                        
                        for (var i = 0; i < prevTDs.length; i++) {
                            selectedTDs[i].firstElementChild.value = prevTDs[i].firstElementChild.value;
                        }
                        var projectId = $(SelectedRow).children()[12].firstElementChild.value;
                        if (projectId != null && projectId != '') {
                            getWage($(SelectedRow).children()[18].firstElementChild, prevTDs[18].firstElementChild.value, function () {
                                if(SelectedCellIndex != null)
                                    $(SelectedRow).children()[SelectedCellIndex].firstElementChild.focus();
                            });
                        } else {
                            if(SelectedCellIndex != null)
                                $(SelectedRow).children()[SelectedCellIndex].firstElementChild.focus();
                        }
                    }
                    return false;
                }
            });

            $('#btnClear').click(function () {
                Cleartable();
            });

            var superArr;
            var workerArr;
           
           

            $('#ddlSuper').on('change', function () {
                var val = $(this).find(":selected").val();
                $("#ddlWorker").html('');
                $("<option/>").val('').text('Select Worker').appendTo("#ddlWorker");
                $.each(workerArr, function (key, value) {
                    if (val === value.Super.toUpperCase()) {
                        $("<option/>").val(value.FDesc).text(value.FDesc).appendTo("#ddlWorker");
                    }
                });
            });

            $(document).on('click', '#addRow', function () {
                addRow();   
            });

            function addRow() {           
                //increase
               // debugger;
                var tblId = $('#tblInput tbody tr:last');
                var isNewRow = false;
                var selectedRowItems = $(tblId).children();
                if (selectedRowItems != null && selectedRowItems.length > 0) {
                    for (var i = 0; i < selectedRowItems.length; i++) {
                        if (i === 0 || i === 1 || i === 3|| i === 12 || i === 13 || i === 18) {
                            if (selectedRowItems[i].firstElementChild.value == null || selectedRowItems[i].firstElementChild.value == '') {
                                isNewRow = true;
                                break;
                            }
                        }
                    }
                    if (!isNewRow) {
                       // debugger
                        var html = '<tr class="rgFilterRow">' + addInnerTR()+ '</tr>';
                        $(tblId).after(html);
                        var len = $('input[name=date]').length;
                        $('#tblInput tbody tr:last').find('select[name=wage]').html('');
                        popularCategory();
                        $('input[name=date]').each(function (i, e) {
                            if (len == (i + 1)) {
                                $(this).focus();
                            }
                        });
                    } else {
                           noty({ text: 'Please enter required fields!', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 7000, theme: 'noty_theme_default', closable: true });
                    }
                }
               
            }

            $(document).on('click', '#rmRow', function () {
                var tblId = $('#tblInput tbody tr:last');
                var len = $('#tblInput tbody tr').length;
                if (len > 1) {
                    $(tblId).remove();
                }
                totalCal();
            });

            $(document).on('click', '#copyRow', function () {
                //debugger;
                var tblId = $('#tblInput tbody tr:last');
                var row = $("input[name=rdCopy]:checked").closest('tr');

                var html = '<tr class="rgFilterRow">' + $(row).html() + '</tr>';
                $(tblId).after(html);
            });

            $(document).on('focus', 'body', function (e) {
                // For F6
                var ctr = $(e)[0].target;
                var currRow = $(ctr).closest('#tblInput>tbody>tr');
                if (currRow.length > 0) {
                    PrevRow = currRow.prev();
                    SelectedRow = currRow;
                    SelectedCellIndex = $(ctr).closest('td').index();
                } else {
                    PrevRow = null;
                    SelectedRow = null;
                    SelectedCellIndex = null;
                }

                bindEvent();
                $('input[name=time]').timeEntry('destroy');
                $('input[name=time]').timeEntry();
                totalCal();
            });

            $('#btnSave').click(function () {
                if (validation()) {
                    saveRecords();
                }
                else {
                    noty({ text: 'Please enter required fields!', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 7000, theme: 'noty_theme_default', closable: true });
                }
            });
        });
        function checkProjectEmpty(pid) {
            //increase 1
            var selHtml = $(pid).closest('tr').find('td').eq(18).find('select');    
            var apro = $(pid).attr('data-loc');
            if (apro === undefined || $(pid).val() == '' || selHtml.html() == '') {
                $(pid).closest('tr').find('td').eq(14).find('input').val('');
                   $(pid).closest('tr').find('td').eq(14).find('input').attr('data-tid','');
                $(pid).val('');
            }
        }

      function checkTypeEmpty(pid) {         
            var apro = $(pid).attr('data-tid');
            if (apro === undefined ||apro=='' ) {
                $(pid).val('');
            }
        }
        function getProject(pid) {
           // van increase 1
            $(pid).closest('tr').find('td').eq(13).find('input').val('');
            $(pid).closest('tr').find('td').eq(14).find('input').val('');
            $(pid).closest('tr').find('td').eq(14).find('input').attr('data-tid','');
            var selHtml = $(pid).closest('tr').find('td').eq(18).find('select');  $(selHtml).html('');            
            var worker = $('#ddlWorker').val();
            if (worker != '') {
                $(pid).autocomplete({
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
                        var search = $(pid).val();
                        queryloc = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "TimeCardService.asmx/GetTimeCardJob",
                            data: JSON.stringify({ prefixText: search, isJob: 1, loc: 0, jobId: 0, worker: '' }),
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
                        $(pid).val(ui.item.ID);
                        $(pid).attr('data-loc', ui.item.Loc);                        
                        getWage(selHtml);
                        return false;
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    minLength: 0,
                    delay: 0
                })
                    .data("ui-autocomplete")._renderItem = function (ul, item) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<span class='auto_item'>" + item.ID + " </span><span class='auto_item'>" + item.fDesc + "</span><span class='auto_desc'>-" + item.Tag + "</span>")
                            .appendTo(ul);
                    };
            }
            else {
                $(pid).val('');
                noty({ text: 'Please select worker!', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 7000, theme: 'noty_theme_default', closable: true });
            }
        }

        function getEquipement(eid) {
            //increase 1
            var v = $(eid).closest('tr').find('td').eq(12).find('input').val();
            if (typeof (v) != 'undefined' && v != '') {
                var loc = $(eid).closest('tr').find('td').eq(12).find('input').attr('data-loc');
                console.log(loc);
                if (typeof (loc) != 'undefined' && loc != '') {
                    $('#DivEqup').attr('style', 'display:block');
                    var search = $(eid).val();
                    $(eid).attr('id', 'inputid');

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "TimeCardService.asmx/GetTimeCardJob",
                        data: JSON.stringify({ prefixText: search, isJob: 2, loc: loc, jobId: 0, worker: '' }),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var chk = '<input type="checkbox" name="chkEquip"  />';
                            $.each(data.d, function (key, value) {
                                var $row = $('<tr>' +
                                    '<td> <input type="checkbox" name="chkEquip" data-eqid=' + value.Id + ' /> </td>' +
                                    '<td>' + value.Unit + '</td>' +
                                    '<td>' + value.State + '</td>' +
                                    '<td>' + value.Fdesc + '</td>' +
                                    '<td>' + value.Type + '</td>' +
                                    '<td>' + value.Category + '</td>' +
                                    '<td>' + value.Cat + '</td>' +
                                    '<td>' + value.Building + '</td>' +
                                    '<td>' + (value.Status == 0 ? 'Active' : 'Inactive') + '</td>' +
                                    '</tr>');

                                $('#tblEquip').append($row);
                            });
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Equipement");
                        }
                    });
                }
                else {
                    noty({ text: 'Please select project!', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 7000, theme: 'noty_theme_default', closable: true });
                }
            }
            else {
                noty({ text: 'Please select project!', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 7000, theme: 'noty_theme_default', closable: true });
            }
        }

        function popupClose() {
            $('#DivEqup').attr('style', 'display:none');
            $('#tblEquip').html('');
            $('#inputid').removeAttr('id');
        }

        function addEquipment() {
            var tmpArr = [];
            $("input[name=chkEquip]").each(function () {
                if ($(this).is(':checked')) {
                    tmpArr.push($(this).attr('data-eqid'));
                }
            });
            if (tmpArr.length > 0) {
                $('#inputid').val(tmpArr.toString());
                popupClose();
            }
        }
          function getCategory(wid) {
            // increase 1
            var pid = $(wid).closest('tr').find('td').eq(12).find('input').val();
            $('.center').remove();
            $(wid).attr('id', 'wageid');
            var worker = $('#ddlWorker').val();
            if (worker != '') {
                if (pid != '') {
                    var search = $(wid).val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "TimeCardService.asmx/GetTimeCardJob",
                        data: JSON.stringify({ prefixText: search, isJob: 0, loc: 0, jobId: pid, worker: worker }),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            $(wid).html('');
                            $(wid).append($("<option></option>")
                                .attr("value", '')
                                .text('Select'));
                            $.each(data.d, function (key, value) {
                                if (value.Selected == '1') {
                                    $(wid).append($("<option selected></option>")
                                        .attr("value", value.ID)
                                        .text(value.fDesc));
                                }
                                else {
                                    $(wid).append($("<option></option>")
                                        .attr("value", value.ID)
                                        .text(value.fDesc));
                                }
                            });
                            $(wid).attr('data-iscall', false);

                            if (selectedVal != null && selectedVal != '') {
                                $(wid).val(selectedVal);
                            }

                            if (callback && typeof (callback) == "function") {
                                callback();
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Wage");
                        }
                    });
                }
                else {
                    noty({ text: 'Please select project!', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 7000, theme: 'noty_theme_default', closable: true });
                }
            }
            else {
                noty({ text: 'Please select worker!', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 7000, theme: 'noty_theme_default', closable: true });
            }
        }

        function getWage(wid, selectedVal, callback) {
            // increase 1
            var pid = $(wid).closest('tr').find('td').eq(12).find('input').val();
            $('.center').remove();
            $(wid).attr('id', 'wageid');
            var worker = $('#ddlWorker').val();
            if (worker != '') {
                if (pid != '') {
                    var search = $(wid).val();
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "TimeCardService.asmx/GetTimeCardJob",
                        data: JSON.stringify({ prefixText: search, isJob: 0, loc: 0, jobId: pid, worker: worker }),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            $(wid).html('');
                            $(wid).append($("<option></option>")
                                .attr("value", '')
                                .text('Select'));
                            $.each(data.d, function (key, value) {
                                if (value.Selected == '1') {
                                    $(wid).append($("<option selected></option>")
                                        .attr("value", value.ID)
                                        .text(value.fDesc));
                                }
                                else {
                                    $(wid).append($("<option></option>")
                                        .attr("value", value.ID)
                                        .text(value.fDesc));
                                }
                            });
                            $(wid).attr('data-iscall', false);

                            if (selectedVal != null && selectedVal != '') {
                                $(wid).val(selectedVal);
                            }

                            if (callback && typeof (callback) == "function") {
                                callback();
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Wage");
                        }
                    });
                }
                else {
                    noty({ text: 'Please select project!', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 7000, theme: 'noty_theme_default', closable: true });
                }
            }
            else {
                noty({ text: 'Please select worker!', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 7000, theme: 'noty_theme_default', closable: true });
            }
        }

        function selectWage(e) {
            $('#wageid').val($(e).attr('data-wtext'));
            $('#wageid').attr('data-wid', $(e).attr('data-wageid'));
            $('#wageid').removeAttr('id');
            $('.center').remove();
        }

        function getType(tid) {
            //increase
            var jobId = $(tid).closest('tr').find('td').eq(12).find('input').val();
             $(tid).attr('data-tid','');
            var search = $(tid).val();
            if (jobId != '') {
                $(tid).autocomplete({
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
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "TimeCardService.asmx/GetTimeCardJob",
                            data: JSON.stringify({ prefixText: search, isJob: 3, loc: 0, jobId: jobId, worker: '' }),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response(data.d);
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load Type");
                            }
                        });
                    },
                    select: function (event, ui) {

                        $(tid).val(ui.item.fDesc + '/' + ui.item.BType); 

                        
                        $(tid).closest('tr').find('td').eq(14).find('input').val(ui.item.Category);
                        $(tid).closest('tr').find('td').eq(15).find('input').val(ui.item.Category);
                        $(tid).closest('tr').find('td').eq(16).find('input').val(ui.item.Code);
                        $(tid).closest('tr').find('td').eq(17).find('input').val(ui.item.Tag);
                        $(tid).attr('data-tid', ui.item.ID);
                        return false;
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    minLength: 0,
                    delay: 0
                })
                    .data("ui-autocomplete")._renderItem = function (ul, item) {

                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<span class='auto_desc'>" + item.Code + "</span><span class='auto_desc'>-" + item.fDesc + "</span><span class='auto_desc'>-" + item.BType + "</span> <span class='auto_desc'>-" + item.Category + "</span><span class='auto_desc'>-" + item.Tag + "</span>")
                            .appendTo(ul);
                    };
            }
            else {
                noty({ text: 'Please select project!', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: 7000, theme: 'noty_theme_default', closable: true });
            }
        }
         
        function currValidation(e) {
            var valid = /^\d{0,4}(\.\d{0,2})?$/.test($(e).val().replace('$', '')),
                val = $(e).val().replace('$', '');
            if (!valid) {
                $(e).val(val.substring(0, val.length - 1));
            }
        }

        function validation() {
            var isValid = true;

            if ($('#ddlWorker').val() == '') {
                isValid = false;
            }
            if ($('#ddlCategory').val() == '') {
                isValid = false;
            }

            $('input[name=date]').each(function () {
                if ($(this).val() == '') {
                    isValid = false;
                }
            });
            $('input[name=time]').each(function () {
                if ($(this).val() == '') {
                    isValid = false;
                }
            });
            $('input[name=project]').each(function () {
                if ($(this).val() == '') {
                    isValid = false;
                }
                if ($(this).attr('data-loc') == '') {
                    isValid = false;
                }
            });
            $('input[name=type]').each(function () {
                if ($(this).val() == '') {
                    isValid = false;
                }
                if ($(this).attr('data-tid') == '') {
                    isValid = false;
                }
            });
            $('select[name=wage]').each(function () {
                if ($(this).val() == '') {
                    isValid = false;
                }
            });
             $('select[name=category]').each(function () {
                if ($(this).val() == '') {
                    isValid = false;
                }
            });
            return isValid;
        }

        function bindEvent() {
            $('.datepicker_mom').pikaday({
                firstDay: 0,
                format: 'MM/DD/YYYY',
                minDate: new Date(1900, 1, 1),
                maxDate: new Date(2100, 12, 31),
                yearRange: [1900, 2100]
            });
        }

        function saveRecords() {
            var supervisor = $('#ddlSuper').val();
            var worker = $('#ddlWorker').val();
            var category = $('#ddlCategory').val();

            var date = []; $('input[name=date]').each(function () { date.push($(this).val()) });
            var time = []; $('input[name=time]').each(function () { time.push($(this).val()) });
            var desc = []; $('input[name=desc]').each(function () { desc.push($(this).val()) });
            var cate = []; $('select[name=category]').each(function () { cate.push($(this).val()) });
            var reg = []; $('input[name=reg]').each(function () { reg.push($(this).val()) });
            var ot = []; $('input[name=ot]').each(function () { ot.push($(this).val()) });
            var nt = []; $('input[name=nt]').each(function () { nt.push($(this).val()) });
            var dt = []; $('input[name=dt]').each(function () { dt.push($(this).val()) });
            var travel = []; $('input[name=travel]').each(function () { travel.push($(this).val()) });
            var miles = []; $('input[name=miles]').each(function () { miles.push($(this).val()) });
            var zone = []; $('input[name=zone]').each(function () { zone.push($(this).val().replace('$', '')) });
            var reimb = []; $('input[name=reimb]').each(function () { reimb.push($(this).val().replace('$', '')) });
            var project = []; $('input[name=project]').each(function () { project.push($(this).val()) });
            var type = []; $('input[name=type]').each(function () { type.push($(this).attr('data-tid')) });
            var wage = []; $('select[name=wage]').each(function () { wage.push($(this).val()) });
            var group = []; $('input[name=group]').each(function () { group.push($(this).val()) });             
            var equipment = []; $('input[name=equipment]').each(function () { equipment.push($(this).val()) });
            var wo = []; $('input[name=wo]').each(function () { wo.push($(this).val()) });

            var markedReview = ($('#markReviewed').is(':checked')) ? 1 : 0;
            var timesheet = ($('#timesheet').is(':checked')) ? 1 : 0;
            var totalTime = 0;
           
            var param = JSON.stringify({
                super: supervisor, worker: worker, category: category,
                date: date, time: time, desc: desc,
                reg: reg, ot: ot, nt: nt,
                dt: dt, travel: travel,
                miles: miles, zone: zone, reimb: reimb,
                project: project, type: type, wage: wage,
                group: group, equipment: equipment, wo: wo,cate:cate,
                markedReview: markedReview,
                timesheet:timesheet
            });
            showLoader();
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "TimeCardService.asmx/SaveInputCardService",
                data: param,
                dataType: "json",
                async: true,
                success: function (data) {
                    console.log(data.d);
                    if (data.d == true) {
                        Cleartable();
                        noty({ text: 'Saved successfully!', type: 'success', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    }
                    else {
                        noty({ text: 'There are an issues while saving records!', type: 'error', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    }
                    totalCal();
                    hideLoader();
                },
                error: function (result) {
                    hideLoader();
                    alert("Due to unexpected errors we were unable to save records ");
                }
            });
        }

        function totalCal() {
            var tmp = 0.00;
            var tot = 0.00;
            $('input[name=reg]').each(function () {
                tmp = parseFloat($(this).val()) + tmp;
            });
            $('#treg').text(tmp.toFixed(2));
            tot = tot + tmp;
            tmp = 0;
            $('input[name=ot]').each(function () {
                tmp = parseFloat($(this).val()) + tmp;
            });
            $('#tot').text(tmp.toFixed(2));
            tot = tot + tmp;
            tmp = 0;
            $('input[name=nt]').each(function () {
                tmp = parseFloat($(this).val()) + tmp;
            });
            $('#tnt').text(tmp.toFixed(2));
            tot = tot + tmp;
            tmp = 0;
            $('input[name=dt]').each(function () {
                tmp = parseFloat($(this).val()) + tmp;
            });
            $('#tdt').text(tmp.toFixed(2));
            tot = tot + tmp;
            tmp = 0;
            $('input[name=travel]').each(function () {
                tmp = parseFloat($(this).val()) + tmp;
            });
            $('#ttravel').text(tmp.toFixed(2));
            tot = tot + tmp;
            tmp = 0;
            $('input[name=miles]').each(function () {
                tmp = parseFloat($(this).val()) + tmp;
            });
            $('#tmiles').text(tmp.toFixed(2));
            tmp = 0;
            $('input[name=zone]').each(function () {
                tmp = parseFloat($(this).val().replace('$', '')) + tmp;
            });
            $('#tzone').text(tmp.toFixed(2));
            tmp = 0;
            $('input[name=reimb]').each(function () {
                tmp = parseFloat($(this).val().replace('$', '')) + tmp;
            });
            $('#treimb').text(tmp.toFixed(2));

            $('#ttime').text(tot.toFixed(2));

        }

        function Cleartable() {
            $('#tblInput tbody').html('');
            $('#tblInput tbody').html(addTr());
            $('#markReviewed').attr('checked', false);
            $('#timesheet').attr('checked', false);
            $('#ddlSuper').val('');
            $('#ddlWorker').val('');
            $('#ddlCategory').val('');

            bindEvent();
        }
        function revisedRandId() {
     return Math.random().toString(36).replace(/[^a-z]+/g, '').substr(2, 10);
}
        function addTr() {
            var cate = "";
             cate ="<option value=''>Select Category</option><option value='None'>None</option>"
                                        
            for (i = 0; i < categoryArr.length; i++) {
                cate =cate+ "<option value='"+categoryArr[i].Type+"'>"+categoryArr[i].Type +"</option>"
            }
            
          var row = '';
              row = '<tr class="rgFilterRow"><td style="width: 130px;"><input type="text" name="date" value="" class="datepicker_mom" /></td>';
              row = row + '<td style="white-space: nowrap; width: 80px;"><input type="text" name="time" value="08:00 AM" maxlength="10" /></td>';
            row = row + '<td style = "white-space: nowrap;" > <input type="text" name="desc" value="Timesheet" /></td >';
            row = row + '<td style = "white-space: nowrap;" > <select class="browser-default selectnew" name="category" style="width: 90px; margin-left: 13px;">'
             row = row + cate +'</select></td >';
        
              row = row + '<td style="white-space: nowrap;"><input type="text" name="reg" value="0.00" onkeyup="currValidation(this);" onfocusout="twoDecimal(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="ot" value="0.00" onkeyup="currValidation(this);" onfocusout="twoDecimal(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="nt" value="0.00" onkeyup="currValidation(this);" onfocusout="twoDecimal(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="dt" value="0.00" onkeyup="currValidation(this);" onfocusout="twoDecimal(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="travel" value="0.00" onkeyup="currValidation(this);" onfocusout="twoDecimal(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="miles" value="0.00" onkeyup="currValidation(this);" onfocusout="twoDecimal(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="zone" value="$0.00" onkeyup="currValidation(this)" onfocusout="twoDecimalDoller(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="reimb" value="$0.00" onkeyup="currValidation(this)" onfocusout="twoDecimalDoller(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="project" value="" onkeyup="getProject(this);" autocomplete="off" onfocusout="checkProjectEmpty(this)" /></td> ';             
              row = row + '<td style="white-space: nowrap;"><input type="text" name="type" value="" onkeyup="getType(this);" autocomplete="off"  onfocusout="checkTypeEmpty(this)"/></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="ItemsDesc" value="" readonly="readonly"  /></td>';
              row = row + '<td style="white-space: nowrap;display:none;"><input type="text" name="group" value=""  /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="Code" value="" readonly="readonly"  /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="CodeDesc" value="" readonly="readonly"   /></td>';
              row = row + '<td style = "white-space: nowrap;" > <select class="browser-default selectnew" name="wage" style="width: 90px; margin-left: 12px;"><option value=""></option></select></td >';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="equipment" value="" onclick="getEquipement(this);" /></td>';
            row = row + '<td style="white-space: nowrap;"><input type="text" name="wo" value="" style="width: 40px;" /></td></tr > ';
            return row;
        }
        function addInnerTR() {
             var cate = "";
             cate ="<option value=''>Select Category</option><option value='None'>None</option>"
                                        
            for (i = 0; i < categoryArr.length; i++) {
                cate =cate+ "<option value='"+categoryArr[i].Type+"'>"+categoryArr[i].Type +"</option>"
            }
          var row = '';
              row =' <td style="width: 130px;"><input type="text" name="date" value="" class="datepicker_mom" /></td>';
              row = row + '<td style="white-space: nowrap; width: 80px;"><input type="text" name="time" value="08:00 AM" maxlength="10" /></td>';
             row = row + '<td style = "white-space: nowrap;" > <input type="text" name="desc" value="Timesheet" /></td >';
                 row = row + '<td style = "white-space: nowrap;" > <select class="browser-default selectnew" name="category" style="width: 90px; margin-left: 10px;">'
             row = row + cate +'</select></td >';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="reg" value="0.00" onkeyup="currValidation(this);" onfocusout="twoDecimal(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="ot" value="0.00" onkeyup="currValidation(this);" onfocusout="twoDecimal(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="nt" value="0.00" onkeyup="currValidation(this);" onfocusout="twoDecimal(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="dt" value="0.00" onkeyup="currValidation(this);" onfocusout="twoDecimal(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="travel" value="0.00" onkeyup="currValidation(this);" onfocusout="twoDecimal(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="miles" value="0.00" onkeyup="currValidation(this);" onfocusout="twoDecimal(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="zone" value="$0.00" onkeyup="currValidation(this)" onfocusout="twoDecimalDoller(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="reimb" value="$0.00" onkeyup="currValidation(this)" onfocusout="twoDecimalDoller(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="project" value="" onkeyup="getProject(this);" autocomplete="off" onfocusout="checkProjectEmpty(this)" /></td> ';             
              row = row + '<td style="white-space: nowrap;"><input type="text" name="type" value="" onkeyup="getType(this);" autocomplete="off"  onfocusout="checkTypeEmpty(this)"/></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="ItemsDesc" value="" readonly="readonly"  /></td>';
              row = row + '<td style="white-space: nowrap;display:none;"><input type="text" name="group" value=""  /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="Code" value="" readonly="readonly"  /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="CodeDesc" value="" readonly="readonly"   /></td>';
              row = row + '<td style = "white-space: nowrap;" > <select class="browser-default selectnew" name="wage" style="width: 100px; margin-left: 10px;"><option value=""></option></select></td >';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="equipment" value="" onclick="getEquipement(this);" /></td>';
              row = row + '<td style="white-space: nowrap;"><input type="text" name="wo" value="" /></td>';
            return row;
        }
        function updateCategory() {
            var cate = $('#ddlCategory').val();
            $('select[name=category]').each(function () {     
               
                $(this).val(cate);
                
            });
        }
         function popularCategory() {
            var cate = $('#ddlCategory').val();
            $('select[name=category]').each(function () {     
               
                if ($(this).val()=='') { $(this).val(cate) };
                
            });
        }
    </script>
</asp:Content>
