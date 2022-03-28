<%@ Page Language="C#" MasterPageFile="~/NewSalesMaster.master" AutoEventWireup="true" Inherits="AddEstimateCalculation" Title="Estimate - Mobile Office Manager" Codebehind="AddEstimateCalculation.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-ui-1.9.2.custom.js"></script>
    <%--<script src="http://code.jquery.com/jquery-1.9.1.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        var objcalon = [];
        $(document).ready(function () {

            modalclose();
            CreateTable();

            $("#ddlCalculationType").live("change", function () {
                $("#dvbom").attr("style", "display:none");
                $("#dvtax").attr("style", "display:none");
                disableClearfactor();
                $("#txtName").removeAttr("disabled");
                $("#ddlBomItems").val("0");
                //$("#ddltax").val("0");
                $("#txtfactorvalue").val("");
                $("#ddlfactorOperation").val("0");
                if ($(this).find("option:selected").text() == "BomItem") {
                    $("#dvbom").attr("style", "display:block");
                    $("#txtName").attr("disabled", "disabled");
                    //$("#dvfactor").attr("style", "display:none");
                    //$("#dvApplicableitems").attr("style", "display:none");
                }
                if ($(this).find("option:selected").text() == "Tax") {
                    $("#dvtax").attr("style", "display:block");
                    //$("#txtName").attr("disabled", "disabled");
                }
                if ($(this).find("option:selected").text() == "Others") {

                    enablefactor();
                }
                $("#ddlBomItems").trigger("change");
                //$("#ddltax").trigger("change");
            });

            $("#ddlBomItems").live("change", function () {
                $("#txtName").val('');

                if ($(this).val() != "0") {
                    var name = $(this).find("option:selected").text();
                    //alert(name);
                    $("#txtName").val(name);

                }

            });
            //$("#ddltax").live("change", function () {
            //    $("#txtName").val('');              

            //    if ($(this).val() != "0") {
            //        var name = $(this).find("option:selected").text();
            //        //alert(name);
            //        $("#txtName").val(name);

            //    }

            //});

            $("#linkquoterequestid").live('click', function () {

                showapprovedvendormodal();
            });

            $("#lnkCloseExpenseItem").live('click', function () {
                modalclose();
                return false;
            });

            $("#lnkSaveExpenseItems").live('click', function () {


                //if (Page_ClientValidate("Inv")) {
                //    Save();
                //}
                //else {
                //    for (var i = 0; i < Page_Validators.length; i++) {
                //        var val = Page_Validators[i];
                //        var ctrl = document.getElementById(val.controltovalidate);
                //        if (ctrl != null && ctrl.style != null) {
                //            if (!val.isvalid) {

                //                ctrl.style.borderColor = '#FF0000';
                //                ctrl.style.backgroundColor = '#fce697';
                //            }
                //            else {
                //                ctrl.style.borderColor = '';
                //                ctrl.style.backgroundColor = '';
                //            }
                //        }
                //    }

                //}

                Save();








                modalclose();

                return false;

            });

            $("#lnkSaveTemplate").live('click', function () {

                var objseq = [];

                ($("#responseOptions tr").not("#responseOptions tr:first-child")).each(function () {

                    var id = $(this).find("#id").val();
                    var sequence = $(this).find(".index").text();
                    objseq.push({ ID: id, EstimateCalculationTemplateSequence: sequence });


                });


                if (objseq != null) {
                    $.ajax({
                        type: "POST",
                        url: "AddEstimateCalculation.aspx/SaveEstimateSequence",
                        data: '{calculations:' + JSON.stringify(objseq) + '}',
                        //data: '{searchTerm: "", column: "", page:"' + pageindex + '",stdate:"",enddate:"",Department:"' + tabindex + '"}',
                        //LableName:'Materials',CalculationType:'1',ShowClaculatedValue:'undefined',InputBasedCalculationOperation:'0',Sequence:'1'
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        failure: function (response) {
                            //alert(response.d);
                            //$('#iframeloading').hide();


                        },
                        error: function (response) {
                            // $('#iframeloading').hide();



                        }
                    });
                }








            });

            $("#responseOptions .clickable").live("dblclick", function (e, val) {



                var id = $(this).find("td #id").val();

                alert(id);
            });

            var fixHelperModified = function (e, tr) {
                var $originals = tr.children();
                var $helper = tr.clone();
                $helper.children().each(function (index) {
                    $(this).width($originals.eq(index).width())
                });
                return $helper;
            },
             updateIndex = function (e, ui) {
                 $('td.index', ui.item.parent()).each(function (i) {
                     $(this).html(i + 1);

                 });
             };

            $("#responseOptions>tbody").sortable({
                items: "> tr:not(:first)",
                helper: fixHelperModified,
                stop: updateIndex
            }).disableSelection();
        });


        function modalclose() {
            $("#dvbom").attr("style", "display:none");
            $("#dvtax").attr("style", "display:none");
            $("#programmaticPopup").attr("style", "display:none");
            $("#programmaticPopup").removeClass("programmaticPopup");
            $("#shade").removeClass("ModalPopupBG");



            $("#ddlCalculationType").val('0');
            $("#ddlCalculationType").trigger("change");

            $('#from option').remove();
            $('#to option').remove();
            $("#chkshowvalue").removeAttr("checked");
        }
        function showapprovedvendormodal() {
            $("#dvbom").attr("style", "display:none");
            $("#dvtax").attr("style", "display:none");
            $("#programmaticPopup").attr("style", "display:block");
            $("#programmaticPopup").addClass("programmaticPopup");
            $("#shade").addClass("ModalPopupBG");
            $("#ddlCalculationType").val('0');
            $("#ddlCalculationType").trigger("change");
        }

        function CreateTable() {
            var trHTML = '';
            trHTML += '<table class="table table-bordered table-striped table-condensed flip-content" rules="all" id="responseOptions" style="border-collapse: collapse;" cellspacing="0" border="1">'
            trHTML += '<tbody><tr><th scope="col"><a id="Button2" class="delButton" style="cursor: pointer;"><img src="images/menu_delete.png" title="Delete" width="18px;"></a></th>'
            trHTML += '<th scope="col">Display Sequence</th><th scope="col">Expense Item</th><th scope="col">Type</th></tr>';

            //trHTML += '<tr class="evenrowcolor"><td style="width: 1%;"><input name="id" id="id" value="2009" type="hidden">'
            //trHTML += '<input id="chkSelect" name="chkSelect" type="checkbox"></td>'
            //trHTML += '<td style="width: 1%;" class="index sequence"><span id=lblIndex">1</span><input name="hdnLine" id="hdnLine" value="0" type="hidden"></td>'
            //trHTML += '<td style="width: 3%;"><input name="txtCode" value="100" id="txtCode" style="width: 100%;" class="ui-autocomplete-input" autocomplete="off" role="textbox" aria-autocomplete="list" aria-haspopup="true" type="text">'
            //trHTML += '<input name="hdnCode" id="hdnCode" type="hidden"></td>'
            //trHTML += '<td style="width: 25px;"><select name="ddlBType" id="ddlBType" style="width: 130px;"></select></td></tr>';

            //trHTML += '<tr class="oddrowcolor"><td style="width: 1%;"><input name="hdnestimateid" id="hdnestimateid" value="2009" type="hidden">'
            //trHTML += '<input id="chkSelect" name="chkSelect" type="checkbox"></td>'
            //trHTML += '<td style="width: 1%;" class="index sequence"><span id=lblIndex">2</span><input name="hdnLine" id="hdnLine" value="0" type="hidden"></td>'
            //trHTML += '<td style="width: 3%;"><input name="txtCode" value="200" id="txtCode" style="width: 100%;" class="ui-autocomplete-input" autocomplete="off" role="textbox" aria-autocomplete="list" aria-haspopup="true" type="text">'
            //trHTML += '<input name="hdnCode" id="hdnCode" type="hidden"></td>'
            //trHTML += '<td style="width: 25px;"><select name="ddlBType" id="ddlBType" style="width: 130px;"></select></td></tr>';









            trHTML += '</tbody></table>';

            GetItems("0");
            $('#dvtableitems').append(trHTML);
        }

        function AddToTable(newitem) {
            var table = $("#dvtableitems").find("#responseOptions");
            if (table != null) {
                var trHTML = '';
                var currentlength = ($("#responseOptions tr").not($("#responseOptions tr:first-child"))).length;
                var newrow = newitem.EstimateCalculationTemplateSequence;
                var rowcss = currentlength % 2 == 0 ? "evenrowcolor clickable" : "oddrowcolor clickable";
                trHTML += '<tr class="' + rowcss + '"><td style="width: 1%;"><input name="id" id="id" value="' + newitem.ID + '" type="hidden">'
                trHTML += '<input id="chkSelect" name="chkSelect" type="checkbox"></td>'
                trHTML += '<td style="width: 1%;" class="index sequence"><span id=lblIndex">' + newrow + '</span><input name="hdnLine" id="hdnLine"  value="' + newitem.EstimateCalculationTemplateBOMTID + '" type="hidden"></td>'
                trHTML += '<td style="width: 3%;"><span id=lblExpenseItemName">' + newitem.EstimateCalculationTemplateLableName + '</span><input name="hdnExpenseItemID" id="hdnExpenseItemID" value="' + newitem.EstimateCalculationTemplateBOMTID + '"  type="hidden">'
                trHTML += '<input name="hdnCode" id="hdnCode" type="hidden"></td>'
                trHTML += '<td style="width: 25px;"><span id=lblExpenseType">' + newitem.ExpenseType + '</span><input name="hdnExpenseType" id="hdnExpenseType" value="' + newitem.EstimateCalculationType + '" type="hidden"></td></tr>';
                $("#responseOptions").append(trHTML);
            }
        }

        function disableClearfactor() {
            $("#dvfactor").attr("style", "display:none");
            $("#dvfactor input").val('');
            $("#dvfactor select").val('0');
            $("#dvApplicableitems").attr("style", "display:none");
        }
        function enablefactor() {
            $("#dvfactor").attr("style", "display:block");
            $("#dvfactor input").val('');
            $("#dvfactor select").val('0');
            $("#dvApplicableitems").attr("style", "display:block");


            $.ajax({
                type: "POST",
                url: "AddEstimateCalculation.aspx/GetEstimateCalculation",
                data: '{id: "' + 0 + '"}',
                //data: '{searchTerm: "", column: "", page:"' + pageindex + '",stdate:"",enddate:"",Department:"' + tabindex + '"}',
                //LableName:'Materials',CalculationType:'1',ShowClaculatedValue:'undefined',InputBasedCalculationOperation:'0',Sequence:'1'
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: BindCalculationSelection,
                failure: function (response) {
                    //alert(response.d);
                    $('#iframeloading').hide();


                },
                error: function (response) {
                    $('#iframeloading').hide();



                }
            });


        }

        function moveAll(from, to) {
            $('#' + from + ' option').remove().appendTo('#' + to);
        }

        function moveSelected(from, to) {

            var val = $('#' + from + ' option:selected').val();
            var operationsymbl = $('#ddlcalops option:selected').text();
            $('#' + from + ' option:selected').remove().appendTo('#' + to);

            //if (jQuery.inArray(val, objcalon) != '-1')
            //$.inArray(val, country)
            if (to == 'to')
                objcalon.push({ EstimateCalculationOnTemplateId: val, Operation: operationsymbl });
            else {
                var elementfound = false;
                if (objcalon != null) {
                    for (var i = 0; i < objcalon.length; i++) {
                        if (objcalon[i].EstimateCalculationTemplateId == val) {
                            elementfound = true;
                            objcalon.splice(i, 1);
                        }

                    }
                }
                //var result = objcalon.filter(function (elem) {
                //    return elem != val;
                //});
                //objcalon = result;
            }
        }
        function selectAll() {
            $("select option").attr("selected", "selected");
        }
        function Save() {
            var LableName = $("#txtName").val();
            var BOMT = $("#ddlBomItems").val();
            var CalculationType = $("#ddlCalculationType").val();
            var ShowClaculatedValue = $("#chkshowvalue").attr("checked") == "checked" ? "true" : "false";
            var InputBasedCalculationOperation = $("#ddlfactorOperation").val();
            var calculationfactor = $("#txtfactorvalue").val();
            var currentlength = ($("#responseOptions tr").not($("#responseOptions tr:first-child"))).length;
            var newrow = currentlength + 1;
            var CalculationText = $("#ddlCalculationType option:selected").text();

            //$("#hdnEstimateCalculationTemplateSequence").val(newrow);

            var jsonitems = { "LableName": "'" + LableName + "'", "CalculationType": "'" + CalculationType + "'", "ShowClaculatedValue": "'" + ShowClaculatedValue + "'", "InputBasedCalculationOperation": "'" + InputBasedCalculationOperation + "'", "Sequence": "'" + newrow + "'", "BomType": "'" + BOMT + "'", "Calculationfactor": "'" + calculationfactor + "'", "calculations": JSON.stringify(objcalon), "CalculationText": "'" + CalculationText + "'" };
            //var jsonitems = { "LableName": "'" + LableName + "'", "CalculationType": "'" + CalculationType + "'" };
            var blkstr = [];
            $.each(jsonitems, function (idx2, val2) {
                var str = idx2 + ":" + val2;
                blkstr.push(str);
            });

            $.ajax({
                type: "POST",
                url: "AddEstimateCalculation.aspx/SaveEstimateCalculation",
                data: '{' + blkstr + '}',
                //data: '{searchTerm: "", column: "", page:"' + pageindex + '",stdate:"",enddate:"",Department:"' + tabindex + '"}',
                //LableName:'Materials',CalculationType:'1',ShowClaculatedValue:'undefined',InputBasedCalculationOperation:'0',Sequence:'1'
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
        }
        function OnSuccess(response) {
            var result = response.d.ReponseObject;

            if (response.d.Header.HasError == false) {

                AddToTable(result);
            }
            objcalon = [];
        }
        function GetItems(ID) {
            $.ajax({
                type: "POST",
                url: "AddEstimateCalculation.aspx/GetEstimateCalculation",
                data: '{id: "' + ID + '"}',
                //data: '{searchTerm: "", column: "", page:"' + pageindex + '",stdate:"",enddate:"",Department:"' + tabindex + '"}',
                //LableName:'Materials',CalculationType:'1',ShowClaculatedValue:'undefined',InputBasedCalculationOperation:'0',Sequence:'1'
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: BindItems,
                failure: function (response) {
                    //alert(response.d);
                    $('#iframeloading').hide();


                },
                error: function (response) {
                    $('#iframeloading').hide();



                }
            });
        }
        function BindItems(response) {
            var result = response.d.ReponseObject;

            if (response.d.Header.HasError == false) {

                for (var i = 0; i < result.length; i++) {
                    AddToTable(result[i]);
                }
            }

        }
        function BindCalculationSelection(response) {
            var result = response.d.ReponseObject;

            if (response.d.Header.HasError == false) {

                $('#from option').remove();
                var sel = $('#from');

                for (var i = 0; i < result.length; i++) {

                    sel.append('<option value="' + result[i].ID + '">' + result[i].EstimateCalculationTemplateLableName + '</option>');
                }
            }
        }

    </script>
    <style>
        .show {
            visibility: visible;
        }

        .hide {
            visibility: hidden;
        }

        .pac-container {
            width: 700px !important;
        }

        .pc-titlesmall {
            background: #316b9d;
            font-size: 10px;
            color: #dadedf;
            line-height: 3px !important;
        }

        .mcpWidth {
            width: 68% !important;
        }

        .fixedheadergridv {
            position: absolute;
            top: 145px;
        }

        .fixedfootergridv {
            position: absolute;
            top: 413px;
        }

        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
            position: fixed;
            left: 0px;
            top: 0px;
            z-index: 10000;
            width: 100%;
            height: 100%;
        }

        .error {
            color: #F00;
            vertical-align: top;
            position: absolute;
        }

        .hideerror {
            display: none;
        }

        .showerror {
            display: inline;
        }

        .errorTab {
            visibility: hidden;
            left: auto;
            z-index: 100000;
            width: 1px;
            float: right;
        }

            .errorTab tr span {
                color: red;
            }

        .programmaticPopup {
            background: rgb(255, 255, 255) none repeat scroll 0% 0%;
            border: medium solid;
            position: fixed;
            z-index: 100001;
            left: 460px;
            width: 40%;
            /*top: 1px;*/
        }

        .ui-sortable tr {
            cursor: pointer;
        }

            .ui-sortable tr:hover {
                background: rgba(244,251,17,0.45);
            }

        select {
            width: 200px;
            float: left;
        }

        .controls {
            width: 40px;
            float: left;
            margin: 10px;
        }

            .controls a {
                background-color: #222222;
                border-radius: 4px;
                border: 2px solid #000;
                color: #ffffff;
                padding: 2px;
                font-size: 14px;
                text-decoration: none;
                display: inline-block;
                text-align: center;
                margin: 5px;
                width: 20px;
            }

            .controls #ddlcalops {
                border-radius: 4px;
                border: 2px solid #000;
                /*color: #ffffff;*/
                padding: 2px;
                font-size: 14px;
                text-decoration: none;
                display: inline-block;
                text-align: center;
                margin: 5px;
                /*  width: 20px;
               height:10px;*/
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-cont-top">
    </div>
    <div class="add-estimate">
        <div class="page-cont-top">
        </div>
        <div class="add-estimate">
            <div class="ra-title">
                <ul class="lnklist-header">
                    <li>
                        <asp:Label CssClass="title_text" ID="lblCalculationLableName" runat="server" Text="Calculation Name"></asp:Label>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkSaveTemplate" ClientIDMode="Static" runat="server" ValidationGroup="templ" ToolTip="Save" CssClass="icon-save"></asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkCloseTemplate" runat="server" CausesValidation="False" ForeColor="Red" ToolTip="Close" CssClass="icon-closed"></asp:LinkButton>
                    </li>
                </ul>
            </div>
            <div class="ae-content">
                <div class="col-lg-8 col-md-8" style="width: 100% !important;">

                    <div>
                        <div class="form-col">
                            <div class="fc-label1">
                                #No
                            </div>
                            <div class="fc-input">
                                <asp:TextBox ID="txtcalculationcode" runat="server" ReadOnly="true" Enabled="false"></asp:TextBox>
                                <asp:HyperLink Text="Add Expense Item" runat="server" ID="linkquoterequestid" ClientIDMode="Static" CssClass="btn blue"></asp:HyperLink>
                            </div>

                        </div>
                        <div id="dvtableitems">
                        </div>
                    </div>



                </div>
            </div>
        </div>
    </div>
    <div style="width: 100%;">
        <input tabindex="-1" name="hiddenTargetControlForModalPopup" value="" id="hiddenTargetControlForModalPopup" style="display: none;" type="submit">

        <div id="programmaticPopup">

            <div class="title_bar_popup">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                <asp:Label CssClass="title_text" ID="Label35" runat="server">Define Expense Item</asp:Label>
            </div>
            <div style="padding: 15px">


                <div id="pnlExpenseItems">

                    <asp:HiddenField ID="hdnEstimateCalculationTemplateId" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnEstimateCalculationTemplateSequence" runat="server" ClientIDMode="Static" />
                    <div class="form-col">
                        <div class="fc-label1">
                            Calculation Type
                        </div>
                        <div class="fc-input">
                            <asp:DropDownList ID="ddlCalculationType" runat="server" ClientIDMode="Static" Width="150px"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-col" id="dvbom">
                        <div class="fc-label1">
                            Bom Item
                        </div>
                        <div class="fc-input">
                            <asp:DropDownList ID="ddlBomItems" runat="server" ClientIDMode="Static" Width="150px"></asp:DropDownList>
                        </div>
                    </div>
                    <%-- <div class="form-col" id="dvtax">
                        <div class="fc-label1">
                            Tax
                        </div>
                        <div class="fc-input">
                            <asp:DropDownList ID="ddltax" runat="server" ClientIDMode="Static" Width="150px"></asp:DropDownList>
                        </div>
                    </div>--%>
                    <div class="form-col">
                        <div class="fc-label1">
                            Name
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control"
                                MaxLength="100" Width="150px" ClientIDMode="Static"></asp:TextBox>

                        </div>
                    </div>
                    <div class="form-col" id="dvfactor">
                        <table>
                            <tr>
                                <td>
                                    <div class="fc-label1">
                                        Calculation factor
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtfactorvalue" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </td>
                                <td>&nbsp;&nbsp;</td>
                                <td>
                                    <div>Operation</div>
                                </td>
                                <td>

                                    <div class="fc-input">
                                        <asp:DropDownList ID="ddlfactorOperation" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="0" Text="Select operation"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Percentage(%)"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Addition(+)"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Substract(-)"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Multiply(*)"></asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="form-col" id="dvApplicableitems">
                        <div class="fc-label1">
                            Calculation applicable on
                        </div>
                        <div class="fc-input">

                            <select multiple size="10" id="from" style="min-height: 150px !important; width: 120px;">
                                <%-- <option value="html">Html</option>
                                <option value="css">Css</option>
                                <option value="google">Google</option>
                                <option value="javascript">Javascript</option>
                                <option value="jquery">Jquery</option>
                                <option value="regex">Regex</option>
                                <option value="php">Php</option>
                                <option value="mysql">Mysql</option>
                                <option value="xml">Xml</option>
                                <option value="json">Json</option>--%>
                            </select>
                            <div class="controls">
                                <select id="ddlcalops" style="width: 25px !important;">
                                    <option></option>
                                    <option>+</option>
                                    <option>-</option>
                                </select>

                                <%--<a href="javascript:moveAll('from', 'to')">&gt;&gt;</a>--%>
                                <a href="javascript:moveSelected('from', 'to')">&gt;</a>
                                <a href="javascript:moveSelected('to', 'from')">&lt;</a>
                                <%--<a href="javascript:moveAll('to', 'from')" href="#">&lt;&lt;</a>--%>
                            </div>
                            <select multiple id="to" size="10" name="topics[]" style="min-height: 150px !important; width: 120px;"></select>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label1">
                            Show Value
                        </div>
                        <div class="fc-input">
                            <asp:CheckBox ID="chkshowvalue" runat="server" ClientIDMode="Static" />
                        </div>
                    </div>


                    <div class="custsetup-btn">
                        <asp:Button CssClass="btn default" data-dismiss="modal" runat="server" ID="lnkCloseExpenseItem" ClientIDMode="Static" Text="Close" CausesValidation="False" />
                        <asp:LinkButton ID="lnkSaveExpenseItems" runat="server" CssClass="btn blue"
                            ValidationGroup="invware" ClientIDMode="Static">Save Changes</asp:LinkButton>
                    </div>


                </div>



            </div>

        </div>
        <div class="clearfix"></div>
        <div id="shade"></div>
    </div>
</asp:Content>
