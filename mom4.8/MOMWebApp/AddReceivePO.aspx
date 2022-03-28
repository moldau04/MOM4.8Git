<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="AddReceivePO" CodeBehind="AddReceivePO.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />

    <link href="Design/css/pikaday.css" rel="stylesheet" />

    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">
    
    <style>
        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
        }

        .hidden {
            display: none;
        }

        .selectPORow {
            background: rgb(249, 190, 122) none repeat scroll 0% 0% !important;
        }

        .rptSti tr:nth-child(2n+1) {
            background: none !important;
        }

        .rm_mm td {
            background-color: none !important;
        }
        .r-dis {
    color: rgb(5 19 44) !important;
    font-weight: bold;
    background-color: #ceebf8 !important;
    cursor: not-allowed !important;
    height: 2rem !important;
    margin: 11px 0px 10px 0px !important;
}
        .card {
        
        min-height: 100% !important;
    }
        ul.anchor-links li a {
            border-bottom: 1px groove !important;
        }
    </style>

    <script type="text/javascript">

        function DDlChange(ddl) {
            console.log($(ddl).attr('id'));

            var ddlID = $(ddl).attr('id');
            var hdnIsReceiveIssued = document.getElementById(ddlID.replace('drpRecievepoIssued', 'hdnIsReceiveIssued'));
            var hdnIsInventoryTrackingIsOn = document.getElementById(ddlID.replace('drpRecievepoIssued', 'hdnIsInventoryTrackingIsOn'));
            var hdnIsProjectSelect = document.getElementById(ddlID.replace('drpRecievepoIssued', 'hdnIsProjectSelect'));
            var hdnIsItemsExistsInInventory = document.getElementById(ddlID.replace('drpRecievepoIssued', 'hdnIsItemsExistsInInventory'));

            console.log('hdnIsProjectSelect:-' + hdnIsProjectSelect.value);

            console.log('hdnIsItemsExistsInInventory:-' + hdnIsItemsExistsInInventory.value);
            if (hdnIsProjectSelect.value == 0) {
                if ($(ddl).val() == 1) {
                    noty({
                        text: 'Please select project!',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 5000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                    $(ddl).val(hdnIsReceiveIssued.value);
                }
            }

            if (hdnIsItemsExistsInInventory.value == 0) {
                if ($(ddl).val() == 2) {
                    noty({
                        text: 'Item needs to be added first before receiving it to Inventory and a warehouse needs to be specified!',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 5000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                    $(ddl).val(hdnIsReceiveIssued.value);
                }
            }
            console.log($(ddl).val());
        }

        function isDecimalKey(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode === 45) {
                if (el.value !== "") {
                    var isNegCharExisted = (el.value.indexOf('-') === 0);
                    if (isNegCharExisted) return false;
                    else return true;
                }
                else {
                    return true;
                }
            }

            var number = el.value.split('.');

            if (charCode !== 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            if (number.length > 1 && charCode === 46) {
                return false;
            }
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                return false;
            }
            return true;
        }

        function isDecimalKey4(el, evt) {
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
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 3)) {
                return false;
            }
            return true;
        }

        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate();
                r.moveEnd('character', o.value.length);
                if (r.text == '') return o.value.length;
                return o.value.lastIndexOf(r.text);
            } else return o.selectionStart;
        }

        function isNum(event) {

            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            //if (charCode == 13) {
            //    //return evt.focusout(function () { return false;});
            //    // evt.preventDefault();
            //    return false;
            //}

            //onkeypress="if(event.keyCode == 13){ this.blur(); event.cancelBubble = true; event.returnValue = false; if (event.stopPropagation){ event.stopPropagation(); event.preventDefault();} $find("ctl00_ContentPlaceHolder1_RadGrid_ReceivePO_ctl00")._filterNoDelay("ctl00_ContentPlaceHolder1_RadGrid_ReceivePO_ctl00_ctl02_ctl01_FilterTextBox_PO","PO") }"
            if (event.keyCode == 13) {
                this.blur();
                event.cancelBubble = true;
                event.returnValue = false;
                if (event.stopPropagation) {
                    event.stopPropagation();
                    event.preventDefault();
                }
            }
                //if (evt.keyCode == 13) {
                //    this.blur(); evt.cancelBubble = true; evt.returnValue = false;
                //    //return false;
                //}
            else {
                return true;
            }
        }

        function makeDecimal(obj) {
            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloatCustom(document.getElementById(obj.id).value).toFixed(2);
            }
        }

        function makeDecimal4(obj) {
            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloatCustom(document.getElementById(obj.id).value).toFixed(4);
            }
        }

        function parseFloatCustom(strInput) {
            var retVal = 0;
            if (strInput == null || strInput == '') {
                console.log("Error on parseFloatCustom function: the input string is null or empty. return 0 is default in this case");
            } else {
                //setTimeout(function () {
                return parseFloat(strInput.toString().replace(/[\$\(\),]/g, ''));
                //}, 100);
                //retVal = parseFloat(strInput.toString().replace(/[\$\(\),]/g, ''));
            }

            return retVal;
        }

        //function pageLoad(sender, args) {
        $(document).ready(function () {
            ///////////// Ajax call for vendor auto search ////////////////////                

            $("[id*=chkSelectItemDONOTUSED]").change(function () {
                console.log("enter calling");
                var chk = $(this).attr('id');
                var txtReceive = document.getElementById(chk.replace('chkSelectItem', 'txtReceive'));
                var lblDue = document.getElementById(chk.replace('chkSelectItem', 'lblOutstand'));
                var txtReceiveQty = document.getElementById(chk.replace('chkSelectItem', 'txtReceiveQty'));

                var due = parseFloatCustom($(lblDue).text().toString());

                if ($(this).prop('checked') == true) {

                    $(txtReceive).val(due.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        <%-- SelectedRowStyle('<%=gvPOItems.ClientID %>')--%>
                    SelectedRowStyle('<%=RadGrid_POItems.ClientID %>');
                }
                else if ($(this).prop('checked') == false) {
                    due = 0;
                    $(txtReceiveQty).val(due.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    $(txtReceive).val(due.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    $(this).closest('tr').removeAttr("style");
                }
                GetTotal();
            });


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
                    } catch (e) { }
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

                        
                        $("#<%=txtVendorType.ClientID%>").val(ui.item.VendorType);                        
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
        });
        $(document).ready(function () {
            GetTotal();
            
        });

        //}
        function checkGrid() {
            $("[id*=txtReceive]").each(function () {
                txtPay = $(this).attr('id');
                var txtQty = document.getElementById(txtPay.replace('txtReceive', 'txtReceiveQty'));
                var chk = document.getElementById(txtPay.replace('txtReceive', 'chkSelectItem'));

                if (parseFloatCustom($(txtQty).val()) > 0 || parseFloatCustom($(this).val()) > 0) {
                    $(chk).prop('checked', true);
                    <%--SelectedRowStyle('<%=gvPOItems.ClientID %>')--%>
                   <%-- SelectedRowStyle('<%=RadGrid_POItems.ClientID %>')--%>
                }
                else {
                    $(chk).prop('checked', false);
                    $(this).closest('tr').removeAttr("style");
                }
            });
        }

        function GetTotal() {
            var total = 0;
            var totalqty = 0;
            
            $("[id*=txtReceive]").each(function () {
                txtPay = $(this).attr('id');
                var expression = /[\(\)]/g;                     // to identify parentheses
                var chk = document.getElementById(txtPay.replace('txtReceive', 'chkSelectItem'));
              
               // if ($(chk).prop('checked') == true) {
                    if ($(this).val() != '') {
                     
                        total = total + parseFloatCustom($(this).val());
                    }
                //}
                    
            });
            <%--$("#<%=lblTotal.ClientID%>").text("$"+totalval);--%>

            $("[id*=txtReceiveQty]").each(function () {
               
                txtQty = $(this).attr('id');
                var lblDue = document.getElementById(txtQty.replace('txtReceiveQty', 'lblPrice'));
                //var due = parseFloatCustom($(lblDue).text().toString());
                var chk = document.getElementById(txtQty.replace('txtReceiveQty', 'chkSelectItem'));

              //  if ($(chk).prop('checked') == true) {
                    if ($(this).val() != '') {

                        totalqty = totalqty + parseFloatCustom($(this).val());
                    }
               // }
            });
            total = (total - totalqty);
            var qtotalval = totalqty.toLocaleString("en-US", { minimumFractionDigits: 4 });
            $("[id*=lblReceiveQtyFooter]").text(qtotalval);

            $("#<%=lblTotal.ClientID%>").text("$" + total.toLocaleString("en-US", { minimumFractionDigits: 2 }));

            var totalval = total.toLocaleString("en-US", { minimumFractionDigits: 2 });
            $("[id*=lblReceiveFooter]").text(totalval);
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <telerik:RadAjaxManager ID="RadAjaxManager_Location" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid_ReceivePO">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel_POItems" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid_ReceivePO">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divPO" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="RadGrid_ReceivePO">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="divDue" LoadingPanelID="RadAjaxLoadingPanel_Location" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnSelectPO">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_POItems" LoadingPanelID="RadAjaxLoadingPanel_ReceivePO" />
                    <%--<telerik:AjaxUpdatedControl ControlID="divPO" />--%>
                    <telerik:AjaxUpdatedControl ControlID="divDue" />
                    <telerik:AjaxUpdatedControl ControlID="divVendor" />                    
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ReceivePO" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="btnSelectVendor">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_ReceivePO" LoadingPanelID="RadAjaxLoadingPanel_ReceivePO" />
                    <telerik:AjaxUpdatedControl ControlID="divPO" />
                    <telerik:AjaxUpdatedControl ControlID="divDue" />                    
                </UpdatedControls>
            </telerik:AjaxSetting>

            

            

            <telerik:AjaxSetting AjaxControlID="btnSubmit">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnSubmit" LoadingPanelID="RadAjaxLoadingPanel_ReceivePO" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>


    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_ReceivePO" runat="server">
    </telerik:RadAjaxLoadingPanel>


    <div class="alert alert-success" runat="server" id="divSuccess">
        <button type="button" class="close" data-dismiss="alert">×</button>
        These month/year period is closed out. You do not have permission to add/update this record.
    </div>

    <div style="height: 65px !important;">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title">
                                        <i class="mdi-communication-contacts"></i>&nbsp; 
                                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add PO Reception</asp:Label>
                                        <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label>
                                    </div>
                                    <div class="btnlinks">
                                        <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ValidationGroup="po">Save</asp:LinkButton>
                                    </div>
                                    <div class="btnlinks">
                                        <a class="dropdown-button" data-beloworigin="true" href="#" data-activates="dropdown1">Reports
                                        </a>
                                    </div>
                                    <ul id="dropdown1" class="dropdown-content">
                                        <li>
                                            <asp:LinkButton ID="lnkReceivePOReport" runat="server" CausesValidation="False" OnClick="lnkReceivePOReport_Click">Receive Item Report</asp:LinkButton>
                                        </li>
                                    </ul>
                                    <div class="rght-content">
                                        <div class="btnclosewrap">
                                            <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                                    <li><a href="#accrReceivePO">Receive PO Details</a></li>                                    
                                    <li id="liDocuments"><a href="#accrddocuments">Documents</a></li>
                                    <li id="liLogs" runat="server" style="display: none"><a href="#accrdlogs">Logs</a></li>
                                </ul>
                            </div>
                            <div class="tblnksright">
                                <div class="nextprev">
                                    <asp:Panel ID="pnlNext" runat="server">
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkFirst" OnClick="lnkFirst_Click" ToolTip="First" runat="server" CausesValidation="False">
                                                    <i class="fa fa-angle-double-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkPrevious" OnClick="lnkPrevious_Click" ToolTip="Previous" runat="server" CausesValidation="False">
                                                    <i class="fa fa-angle-left"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkNext" OnClick="lnkNext_Click" ToolTip="Next" runat="server" CausesValidation="False">
                                                  <i class="fa fa-angle-right"></i>
                                            </asp:LinkButton>
                                        </span>
                                        <span class="angleicons">
                                            <asp:LinkButton ID="lnkLast" OnClick="lnkLast_Click" ToolTip="Last" runat="server" CausesValidation="False">
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

    <div class="container" id="accrReceivePO">
        <div class="row">
            <div class="col s12 m12 l12">
                <div class="row">
                    <div class="card" style="border-radius: 6px;">
                        <div class="card-content">
                            <div class="form-content-wrap">
                                <div class="form-content-pd">

                                    <div class="form-section-row">
                                        <div class="section-ttle">Purchase Order Details</div>
                                        <div class="form-input-row">
                                            <div class="form-section3">


                                                <div class="input-field col s12" id="divVendor" runat="server">
                                                    <div class="row">
                                                                <asp:Button ID="btnSelectVendor" runat="server" CausesValidation="False" OnClick="btnSelectVendor_Click"
                                                                        Style="display: none;" Text="Button" />
                                                                <asp:TextBox ID="txtVendor" runat="server" MaxLength="75" ></asp:TextBox>
                                                                <asp:HiddenField ID="hdnVendorID" runat="server" />
                                                                <asp:Label runat="server" ID="lbltxtVendor" AssociatedControlID="txtVendor">Vendor</asp:Label>
                                                            
                                                    </div>
                                                </div>

                                                <div class="input-field col s12" id="divPO" runat="server">
                                                    <div class="row">


                                                        <asp:HiddenField ID="hdnAmount" runat="server" />
                                                        <asp:HiddenField ID="hdnMode" runat="server" />

                                                        <%--<asp:RequiredFieldValidator ID="rfvPO" ValidationGroup="po"
                                                            runat="server" ControlToValidate="txtPO" Display="None" ErrorMessage="Please enter PO"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vcePO" runat="server" Enabled="True"
                                                            PopupPosition="Right" TargetControlID="rfvPO" />--%>

                                                        <asp:Button ID="btnSelectPO" runat="server" OnClick="btnSelectPO_Click" Style="display: none;" CausesValidation="false" />
                                                        <asp:TextBox ID="txtPO" runat="server" onkeypress="return isNum(event,this)"
                                                            MaxLength="50" autocomplete="off"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtPO" AssociatedControlID="txtPO">PO#</asp:Label>


                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">

                                                        <asp:TextBox ID="txtDate" runat="server" CssClass="datepicker_mom"
                                                            MaxLength="15" autocomplete="off" onkeypress="return false;"></asp:TextBox>
                                                        <%-- <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtDate">
                                                        </asp:CalendarExtender>--%>
                                                        <asp:RequiredFieldValidator ID="rfvDate" ValidationGroup="po"
                                                            runat="server" ControlToValidate="txtDate" Display="None" ErrorMessage="Date is Required"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True"
                                                            PopupPosition="Right" TargetControlID="rfvDate" />
                                                        <asp:RegularExpressionValidator ID="rfvDate1" ControlToValidate="txtDate" ValidationGroup="po"
                                                            ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                            TargetControlID="rfvDate1" />
                                                        <asp:Label runat="server" ID="lblDate" AssociatedControlID="txtDate">Date</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12" id="divDue" runat="server">
                                                    <div class="row">


                                                        <asp:TextBox ID="txtDueDate" runat="server" CssClass="datepicker_mom"
                                                            onkeypress="return false;"></asp:TextBox>
                                                        <%--<asp:CalendarExtender ID="txtDueDate_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txtDueDate">
                                                        </asp:CalendarExtender>--%>
                                                        <asp:RequiredFieldValidator ID="rfvDueDate" ValidationGroup="po"
                                                            runat="server" ControlToValidate="txtDueDate" Display="None" ErrorMessage="Due Date is Required"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceDueDate" runat="server" Enabled="True"
                                                            PopupPosition="Right" TargetControlID="rfvDueDate" />
                                                        <asp:RegularExpressionValidator ID="rfvDueDate1" ControlToValidate="txtDueDate" ValidationGroup="po"
                                                            ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceDueDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                            TargetControlID="rfvDueDate1" />
                                                        <asp:Label runat="server" ID="lblDuedate" AssociatedControlID="txtDueDate">Due</asp:Label>

                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">


                                                        <asp:TextBox ID="txtRef" runat="server" autocomplete="off"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvRef" ValidationGroup="po"
                                                            runat="server" ControlToValidate="txtRef" Display="None" ErrorMessage="Ref is Required"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceRef" runat="server" Enabled="True"
                                                            PopupPosition="Right" TargetControlID="rfvRef" />
                                                        <asp:Label runat="server" ID="lblRef" AssociatedControlID="txtRef">Ref</asp:Label>

                                                    </div>
                                                </div>
                                                
                                                <div class="input-field col s12">
                                                    <div class="row">


                                                        <asp:TextBox ID="txtTrkWB" runat="server" autocomplete="off"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lblTrkWB" AssociatedControlID="txtTrkWB">Trk/WB#</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:UpdatePanel ID="updPnlAddress" runat="server" UpdateMode="Always">
                                                            <ContentTemplate>
                                                                <asp:Label runat="server" ID="lblAddress" AssociatedControlID="txtAddress">Address</asp:Label>
                                                                <asp:TextBox ID="txtAddress" runat="server" ReadOnly="true" TextMode="MultiLine" MaxLength="2000" CssClass="materialize-textarea"></asp:TextBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtReception" runat="server" ReadOnly="true">
                                                        </asp:TextBox>
                                                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                                            runat="server" ControlToValidate="txtShipTo" Display="None" ErrorMessage="Ship to is Required" ValidationGroup="po"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                            PopupPosition="Right" TargetControlID="rfvShipTo" />--%>
                                                        <asp:Label runat="server" ID="lbltxtReception" AssociatedControlID="txtReception">Reception No#</asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3">

                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtShipTo" runat="server" MaxLength="2000" ReadOnly="true" disabled="disabled" TextMode="MultiLine" CssClass="materialize-textarea">
                                                        </asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvShipTo"
                                                            runat="server" ControlToValidate="txtShipTo" Display="None" ErrorMessage="Ship to is Required" ValidationGroup="po"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceShipTo" runat="server" Enabled="True"
                                                            PopupPosition="Right" TargetControlID="rfvShipTo" />
                                                        <asp:Label runat="server" ID="lbltxtShipTo" AssociatedControlID="txtShipTo">Ship To</asp:Label>

                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">


                                                        <asp:TextBox ID="txtCreatedBy" runat="server" ReadOnly="true"
                                                            MaxLength="50" autocomplete="off"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtCreatedBy" AssociatedControlID="txtCreatedBy">Created By</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:TextBox ID="txtStatus" runat="server" ReadOnly="true"
                                                            Text="New"></asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtStatus" AssociatedControlID="txtStatus">Status</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">
                                                        <asp:UpdatePanel ID="updPnlVendorType" runat="server" UpdateMode="Always">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtVendorType" runat="server" disabled="disabled" ReadOnly="true"></asp:TextBox>
                                                                <asp:Label runat="server" ID="lblVendorType" AssociatedControlID="txtVendorType">Vendor Type</asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12 pdrt lblfield r-dis">
                                                    <div class="row ">


                                                        <span class="ttlab">Total</span>
                                                        <span class="ttlval">
                                                            <asp:Label ID="lblTotal" disabled="disabled" runat="server"></asp:Label></span>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">

                                                        <asp:TextBox ID="txtComments" runat="server" ReadOnly="true">
                                                        </asp:TextBox>
                                                        <asp:Label runat="server" ID="lbltxtComments" AssociatedControlID="txtComments">PO Comments</asp:Label>
                                                    </div>
                                                </div>
                                                <div class="input-field col s12">
                                                    <div class="row">

                                                        <asp:Label runat="server" ID="lbltxtRcomments" AssociatedControlID="txtRcomments">Reception Comments</asp:Label>
                                                        <asp:TextBox ID="txtRcomments" runat="server" CssClass="materialize-textarea" MaxLength="2000" TextMode="MultiLine">
                                                        </asp:TextBox>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-section3-blank">
                                                &nbsp;
                                            </div>
                                            <div class="form-section3" style="text-align: center;">
                                                <div class="upc-rec">
                                                    Open POs 
                                                </div>
                                                <div>
                                                    <div class="grid_container">
                                                        <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                            <div class="RadGrid RadGrid_Material POGrid">
                                                                <%--      <asp:UpdatePanel ID="updPnlPo" runat="server">
                                                                    <ContentTemplate>--%>
                                                                <div class="table-scrollable">
                                                                    <telerik:RadAjaxPanel ID="RadAjaxPanel_ReceivePO" runat="server" LoadingPanelID="RadAjaxLoadingPanel_ReceivePO">
                                                                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_ReceivePO" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50" OnItemCommand="RadGrid_ReceivePO_ItemCommand" OnItemDataBound="RadGrid_ReceivePO_ItemDataBound"
                                                                            PagerStyle-AlwaysVisible="true" OnSelectedIndexChanged="RadGrid_ReceivePO_SelectedIndexChanged" OnNeedDataSource="RadGrid_ReceivePO_NeedDataSource"
                                                                            ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" AllowCustomPaging="True">
                                                                            <CommandItemStyle />
                                                                            <GroupingSettings CaseSensitive="false" />
                                                                            <ClientSettings AllowColumnsReorder="true" EnablePostBackOnRowClick="true" ReorderColumnsOnClient="true">
                                                                                <Selecting AllowRowSelect="True"></Selecting>
                                                                                <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                            </ClientSettings>
                                                                            <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                                <Columns>
                                                                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="30">
                                                                                    </telerik:GridClientSelectColumn>
                                                                                    <telerik:GridTemplateColumn HeaderText="PO#" SortExpression="PO" AutoPostBackOnFilter="true" AllowFiltering="true" UniqueName="PO" CurrentFilterFunction="EqualTo" DataField="PO" DataType="System.Int32" HeaderStyle-Width="70"
                                                                                        ShowFilterIcon="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblID" Text='<%# Bind("PO") %>' runat="server" />
                                                                                           
                                                                                        </ItemTemplate>
                                                                                    </telerik:GridTemplateColumn>
                                                                                    


                                                                                    <%--<telerik:GridBoundColumn DataField="PO" HeaderText="PO#" SortExpression="PO"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" DataType="System.Int32" HeaderStyle-Width="70"
                                                                                        ShowFilterIcon="false">
                                                                                    </telerik:GridBoundColumn>--%>

                                                                                    <telerik:GridBoundColumn DataField="VendorName" HeaderText="Vendor" HeaderStyle-Width="120"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="VendorName"
                                                                                        ShowFilterIcon="false">
                                                                                    </telerik:GridBoundColumn>

                                                                                    <telerik:GridBoundColumn DataField="Due" HeaderText="Due Date" HeaderStyle-Width="100"
                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Due" DataFormatString="{0:MM/dd/yy}"
                                                                                        ShowFilterIcon="false">
                                                                                    </telerik:GridBoundColumn>

                                                                                </Columns>
                                                                            </MasterTableView>
                                                                        </telerik:RadGrid>
                                                                    </telerik:RadAjaxPanel>

                                                                </div>
                                                                <%--  </ContentTemplate>
                                                                </asp:UpdatePanel>--%>
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
            </div>

            <div class="grid_container" style="margin-bottom: 20px;">
                <div class="form-section-row" style="margin-bottom: 0 !important;">

                    <div class="grid_container">
                        <div class="RadGrid RadGrid_Material FormGrid">
                            <%--<asp:UpdatePanel ID="updPnlPOItem" runat="server">
                            <ContentTemplate>--%>
                            <div class="table-scrollable">

                                <telerik:RadCodeBlock ID="RadCodeBlock_POItems" runat="server">
                                    <script type="text/javascript">
                                        function pageLoad() {
                                            var grid = $find("<%= RadGrid_POItems.ClientID %>");
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
                                            try {
                                                var element = document.getElementById(requestInitiator);
                                                if (element && element.tagName == "INPUT") {
                                                    element.focus();
                                                    element.selectionStart = selectionStart;
                                                }
                                            } catch (e) {

                                            }
                                            Materialize.updateTextFields();
                                        }
                                    </script>
                                </telerik:RadCodeBlock>


                                <telerik:RadAjaxPanel ID="RadAjaxPanel_POItems" runat="server" LoadingPanelID="RadAjaxLoadingPanel_ReceivePO" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                    <telerik:RadGrid RenderMode="Auto" ID="RadGrid_POItems" AllowFilteringByColumn="false" ShowFooter="True" PageSize="50"
                                        PagerStyle-AlwaysVisible="true" OnNeedDataSource="RadGrid_POItems_NeedDataSource" OnItemDataBound="RadGrid_POItems_ItemDataBound"
                                        ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" AllowCustomPaging="True">
                                        <CommandItemStyle />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings AllowColumnsReorder="true" EnableAlternatingItems="false" ReorderColumnsOnClient="true">
                                            <Selecting AllowRowSelect="True"></Selecting>
                                            <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                            <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                        </ClientSettings>
                                        <MasterTableView AutoGenerateColumns="false" AllowSorting="false" AllowFilteringByColumn="False" ShowFooter="True">
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="25" ShowFilterIcon="false">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectItem" runat="server" />
                                                        <asp:HiddenField ID="hdnInvID" runat="server" Value='<%# Bind("Inv") %>'></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnRPOItemId" runat="server" Value='<%# Bind("RPOItemId") %>'></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnPOItemId" runat="server" Value='<%# Bind("POItemId") %>'></asp:HiddenField>
                                                        
                                                        
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn Visible="false" HeaderStyle-Width="20" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" Text='<%# Bind("PO") %>' runat="server" />
                                                        <asp:Label ID="lblLine" Text='<%# Bind("Line") %>' runat="server" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>


                                                <telerik:GridBoundColumn DataField="fDesc" HeaderText="Description" SortExpression="fDesc"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="150"
                                                    ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridBoundColumn DataField="Due" HeaderText="DueDate" HeaderStyle-Width="70"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Due" DataFormatString="{0:MM/dd/yy}"
                                                    ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="ItemName" HeaderText="Item" HeaderStyle-Width="100"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Item"
                                                    ShowFilterIcon="false">
                                                </telerik:GridBoundColumn>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="150" SortExpression="Project" HeaderText="Project#" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                    <asp:Label ID="lblJobProject" Visible='<%# Eval("Job").ToString() == "0"? false:true%>' Text="Project:" runat="server" />                    
                                                    <asp:Label ID="lblJobID" Visible='<%# Eval("Job").ToString() == "0"? false:true%>' Text='<%# Eval("Job")%>' runat="server" />

                                                     <asp:Label ID="lblJobName" 
                                                         Visible='<%# Eval("Job").ToString() == "0"? false:true%>'
                                                         Text='<%# DataBinder.Eval(Container.DataItem, "JobName")%>' runat="server" />
                                   
                                                     <asp:Label ID="lblJobProject1" Visible='<%# Eval("Job").ToString() == "0"? false:true%>' Text="/" runat="server" />                    
                                                        
                                                    <asp:Label ID="lblLocationName" Visible='<%# Eval("Job").ToString() == "0"? false:true%>' Text='<%# DataBinder.Eval(Container.DataItem, "LocationName")%>' runat="server" />


                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="60" SortExpression="Ticket" HeaderText="Ticket#" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTicket" Text='<%# DataBinder.Eval(Container.DataItem, "Ticket")%>' runat="server" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <%--<telerik:GridBoundColumn     DataField="OrderedQuan" HeaderText="Qty Ordered" HeaderStyle-Width="100"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="OrderedQuan"
                                                ShowFilterIcon="false">
                                            </telerik:GridBoundColumn>--%>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="80" DataField="OrderedQuan" SortExpression="OrderedQuan" HeaderText="QtyOrdered" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderedQuan" Text='<%# DataBinder.Eval(Container.DataItem, "OrderedQuan")%>' runat="server" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="$Ordered" HeaderStyle-Width="80" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Ordered"
                                                    ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrdered" Text='<%# DataBinder.Eval(Container.DataItem, "Ordered", "{0:c}")%>' runat="server"
                                                            ForeColor='<%# Convert.ToDouble(Eval("Ordered"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <%--<telerik:GridBoundColumn     DataField="Ordered" HeaderText="$Ordered" HeaderStyle-Width="80"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="Ordered" DataFormatString="{0:c}"
                                                ShowFilterIcon="false">
                                            </telerik:GridBoundColumn>--%>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="80" SortExpression="Price" HeaderText="$UnitCost" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrice" Text='<%# DataBinder.Eval(Container.DataItem, "Price" , "{0:c}")%>' runat="server"
                                                            ForeColor='<%# Convert.ToDouble(Eval("Price"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn DataField="PrvInQuan" HeaderText="PrevQtyReceived" HeaderStyle-Width="110"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="PrvInQuan"
                                                    ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrvInQuan" Text='<%# DataBinder.Eval(Container.DataItem, "PrvInQuan")%>' runat="server" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="$PrevReceived" HeaderStyle-Width="110"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="PrvIn"
                                                    ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrvIn" Text='<%# DataBinder.Eval(Container.DataItem, "PrvIn", "{0:c}")%>' runat="server"
                                                            ForeColor='<%# Convert.ToDouble(Eval("PrvIn"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <%--<telerik:GridBoundColumn     DataField="PrvIn" HeaderText="$PrevReceived" HeaderStyle-Width="110"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" SortExpression="PrvIn" DataFormatString="{0:c}" 
                                                ShowFilterIcon="false">
                                            </telerik:GridBoundColumn>--%>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="60" SortExpression="OutstandQuan" HeaderText="BOQty" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                          <%--              <asp:Label ID="lblBOQty" Text='<%# Eval("OutstandQuan")%>' runat="server" Style='<%#Convert.ToDouble(Eval("PrvInQuan"))==0.00?"display:block;": "display:none;"%>' />
                                                        <asp:Label ID="lblFirstBOQty" Text='<%# Eval("PrvInQuan")%>' runat="server" Style='<%#Convert.ToDouble(Eval("PrvInQuan"))==0.00?"display:none;": "display:block;"%>' />--%>

                                                        <asp:Label ID="lblBOQty" Text='<%# Eval("OutstandQuan")%>' runat="server" />
                                                        <asp:Label ID="lblFirstBOQty" Text="0.00" runat="server" Visible="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="60" SortExpression="Outstanding" HeaderText="BO$" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblOutstand" Text='<%# DataBinder.Eval(Container.DataItem, "Outstanding", "{0}")%>' runat="server" Style='<%#Convert.ToString(Eval("PrvIn"))=="0.00"?"display:none;": "display:block;"%>' />
                                                        <asp:Label ID="lblFirstOutstand" Text="$0.00" runat="server" Style='<%#Convert.ToString(Eval("PrvIn"))=="0.00"?"display:block;": "display:none;"%>' />--%>

                                                       <%-- <asp:Label ID="lblOutstand" Text='<%# Eval("Outstanding")%>' runat="server" Style='<%#Convert.ToDouble(Eval("PrvIn"))==0.00?"display:block;": "display:none;"%>' />
                                                        <asp:Label ID="lblFirstOutstand" Text='<%# Eval("PrvIn")%>' runat="server" Style='<%#Convert.ToDouble(Eval("PrvIn"))==0.00?"display:none;": "display:block;"%>' />--%>
                                                         <asp:Label ID="lblOutstand" Text='<%# Eval("Outstanding")%>' runat="server"  />
                                                        <asp:Label ID="lblFirstOutstand" Text=$0.00 runat="server" Visible="false" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalFooter" runat="server" Text="Total:-"></asp:Label>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="80" SortExpression="ReceivedQuan" HeaderText="ReceivedQty" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtReceiveQty" runat="server" onkeypress="return isDecimalKey4(this,event)" Text='<%# Bind("ReceivedQuan") %>' onblur="makeDecimal4(this);" autocomplete="off"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnReceiveQty" runat="server" Value='<%# Bind("ReceivedQuan") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblReceiveQtyFooter" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="80" SortExpression="Received" HeaderText="$Received" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtReceive" runat="server" onkeypress="return isDecimalKey(this,event)" Text='<%# Bind("Received") %>' autocomplete="off" onblur="makeDecimal(this);"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnReceive" runat="server" Value='<%# Bind("Received") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblReceiveFooter" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="80" SortExpression="IsReceiveIssued" HeaderText="Issued" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="drpRecievepoIssued" runat="server" onChange="DDlChange(this);" CssClass="browser-default input-sm input-small">
                                                            <asp:ListItem Text="Select" Value="0" />
                                                            <asp:ListItem Text="Project" Value="1" />
                                                            <asp:ListItem Text="Inventory" Value="2" />
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hdnIsReceiveIssued" runat="server" Value='<%# Bind("IsReceiveIssued") %>'></asp:HiddenField>
                                                        
                                                        <asp:HiddenField ID="hdnIsProjectSelect" runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnIsItemsExistsInInventory" Value='<%# Bind("IsItemsExistsInInventory") %>' runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnIsInventoryCode" Value='<%# Bind("IsInventoryCode") %>' runat="server"></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnfDesc" Value='<%# Bind("fDesc") %>' runat="server"></asp:HiddenField>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="80" SortExpression="WarehouseID" HeaderText="Warehouse" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvWarehouse" runat="server" CssClass="texttransparent Warehousesearchinput"></asp:TextBox>
                                                        <%# Eval("WarehouseName") %>
                                                        <asp:HiddenField ID="hdnWarehouse" runat="server" Value='<%# Bind("WarehouseID") %>'></asp:HiddenField>
                                                        <asp:Label ID="lblWarehouseID" Text='<%# DataBinder.Eval(Container.DataItem, "WarehouseID")%>' runat="server" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderStyle-Width="80" SortExpression="LocationID" HeaderText="Location" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvWarehouseLocation" runat="server" CssClass="texttransparent WarehouseLocationsearchinput"></asp:TextBox>
                                                        <%--  <%# Eval("WarehousLoc") %> --%>
                                                        <asp:HiddenField ID="hdnWarehouseLocationID" runat="server" Value='<%# Bind("LocationID") %>'></asp:HiddenField>
                                                        <asp:Label ID="lblLocationID" Text='<%#Convert.ToString(Eval("LocationID"))=="0"?"":Eval("LocationID")%>' runat="server" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </telerik:RadAjaxPanel>

                            </div>
                        </div>
                        <%-- </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                </div>

            </div>

        </div>

    </div>

    <%------------------>--%>
     <div class="container accordian-wrap">
                <div class="col s12 m12 l12">
                    <div class="row">
                        <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">

                            <li runat="server" id="dvDocuments">
                                <div id="accrddocuments" class="collapsible-header accrd accordian-text-custom"><i class="mdi-action-credit-card"></i>Documents</div>
                                <div class="collapsible-body">
                                    <asp:Panel ID="pnlDocPermission" runat="server">
                                        <%--<asp:Panel ID="pnlDoc" runat="server" Visible="false">
                                        <asp:Panel ID="pnlDocumentButtons" runat="server">--%>
                                        <div class="form-content-wrap">
                                            <div class="form-content-pd">
                                            
                                            <div class="form-section-row">
                                                <div class="col s12 m12 l12">
                                                    <div class="row">
                                                         
                                                            
                                                            <!--<p>Maximum file upload size 2MB.</p>-->
                                                       <%-- <input type="file" id="FileUpload1" runat="server" AllowMultiple="true"
                                                            onchange="AddDocumentClick(this);" class="dropify" />--%>
                                                         <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true"
                                                            onchange="AddDocumentClick(this);" class="dropify" />
                                                        <!--data-max-file-size="2M"-->
                                                               
                                                </div>
                                        </div>

                                        <%--    <div class="btncontainer">
                                    <asp:Panel ID="pnlDocumentButtons" runat="server">
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkUploadDoc" runat="server"
                                                    CausesValidation="False" OnClick="lnkUploadDoc_Click"
                                                    Style="display: none">Upload</asp:LinkButton>
                                                <asp:LinkButton
                                                    ID="lnkPostback" runat="server"
                                                    CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False"
                                                    OnClick="lnkDeleteDoc_Click"
                                                    OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                            </div>
                                        </asp:Panel>
                                                </div>--%>
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
                                            <div class="grid_container" style="margin-top: 10px;">
                                                <div class="form-section-row" style="margin-bottom: 0 !important;">
                                                    <div class="RadGrid RadGrid_Material FormGrid">
                                                        <telerik:RadCodeBlock ID="RadCodeBlock4" runat="server">
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
                                                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Documents" PostBackControls="lblName" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Customer" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                                                            <%---------->--%>
                                                         <%--   <div class="btnlinks" style="margin-left: -10px;">
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click"
                                                                OnClientClick="return DeleteDocumentClick(this);">Delete</asp:LinkButton>
                                                        </div>
                                                        <span class="tro trost">
                                                            <asp:CheckBox ID="chkShowAllDocs" Text="Show All Documents" OnCheckedChanged="chkShowAllDocs_CheckedChanged" class="css-checkbox" Style="padding-left: 5px; color: black; font-weight: 400" ForeColor="Black" AutoPostBack="true" runat="server" />
                                                        </span>--%>
                                                            <%----------------<--%>
                                  



                                                          
                                                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Documents" MasterTableView-RowIndicatorColumn-AutoPostBackOnFilter="true" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"
                                                                PagerStyle-AlwaysVisible="true" ShowStatusBar="true" runat="server" AllowPaging="True" AllowSorting="true" Width="100%" FilterType="CheckList" 
                                                                AllowCustomPaging="True" OnPreRender="RadGrid_Documents_PreRender" OnNeedDataSource="RadGrid_Documents_NeedDataSource" >
                                                                <CommandItemStyle />
                                                                <GroupingSettings CaseSensitive="false" />
                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                    <Selecting AllowRowSelect="True"></Selecting>

                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                    <Resizing ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>

                                                                </ClientSettings>
                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">


                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn Visible="false" AllowFiltering="false" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridClientSelectColumn UniqueName="chkSelect"
                                                                            HeaderStyle-Width="28">
                                                                        </telerik:GridClientSelectColumn>


                                                                        <telerik:GridTemplateColumn DataField="filename" SortExpression="filename" AutoPostBackOnFilter="true" HeaderStyle-Width="30%"
                                                                            CurrentFilterFunction="Contains" HeaderText="File Name" ShowFilterIcon="false">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                                                    CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                                    OnClientClick="return ViewDocumentClick(this);" OnClick="lblName_Click" Text='<%# Eval("filename") %>'>
                                                                                </asp:LinkButton>

                                                                            </ItemTemplate>

                                                                        </telerik:GridTemplateColumn>

                                                                        <telerik:GridTemplateColumn DataField="doctype" SortExpression="doctype" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="File Type" ShowFilterIcon="false" HeaderStyle-Width="15%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("doctype") %>'></asp:Label>

                                                                            </ItemTemplate>

                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn DataField="portal" SortExpression="portal" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Portal" ShowFilterIcon="false"  FilterControlWidth="20" HeaderStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# (Eval("portal")!=DBNull.Value) ? Convert.ToBoolean(Eval("portal")): false %>' />
                                                                            </ItemTemplate>

                                                                        </telerik:GridTemplateColumn>

                                                                        <%--<telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="20%">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                            </ItemTemplate>

                                                                        </telerik:GridTemplateColumn>--%>

                                                                         <telerik:GridTemplateColumn DataField="MSVisible" SortExpression="MSVisible" AutoPostBackOnFilter="true"
                                                                                                            HeaderText="Mobile Service" ShowFilterIcon="false" HeaderStyle-Width="120"
                                                                                                            DataType="System.Int16" UniqueName='MSVisible'>
                                                                                                            <FilterTemplate>
                                                                                                                <telerik:RadComboBox RenderMode="Auto" ID="ImportedFilter" runat="server" OnClientSelectedIndexChanged="ImportedFilterSelectedIndexChanged"
                                                                                                                    SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("MSVisible").CurrentFilterValue %>'
                                                                                                                    Width="100px">
                                                                                                                    <Items>
                                                                                                                        <telerik:RadComboBoxItem Text="All" Value="" />
                                                                                                                        <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                                                                                                        <telerik:RadComboBoxItem Text="No" Value="0" />
                                                                                                                    </Items>
                                                                                                                </telerik:RadComboBox>
                                                                                                                <telerik:RadScriptBlock ID="RadScriptBlock12" runat="server">
                                                                                                                    <script type="text/javascript">
                                                                                                                        function ImportedFilterSelectedIndexChanged(sender, args) {
                                                                                                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                                                                                            var filterVal = args.get_item().get_value();
                                                                                                                            if (filterVal == "") {
                                                                                                                                tableView.filter("MSVisible", filterVal, "NoFilter");
                                                                                                                            }
                                                                                                                            else if (filterVal == "1") {
                                                                                                                                tableView.filter("MSVisible", "1", "EqualTo");
                                                                                                                            }
                                                                                                                            else if (filterVal == "0") {
                                                                                                                                tableView.filter("MSVisible", "0", "EqualTo");
                                                                                                                            }
                                                                                                                        }
                                                                                                                    </script>
                                                                                                                </telerik:RadScriptBlock>
                                                                                                            </FilterTemplate>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:CheckBox ID="chkMSVisible" runat="server" Checked='<%# (Eval("MSVisible")!=DBNull.Value) ? Convert.ToBoolean(Eval("MSVisible")): false %>' />
                                                                                                            </ItemTemplate>
                                                                             </telerik:GridTemplateColumn>


                                                                        <telerik:GridTemplateColumn DataField="remarks" SortExpression="remarks" AutoPostBackOnFilter="true"
                                                                            CurrentFilterFunction="Contains" HeaderText="Remarks" ShowFilterIcon="false" HeaderStyle-Width="20%">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtremarks" Width="500px" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox>
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
                                        </div>
                                <%--</asp:Panel>
                                  </asp:Panel>--%>
                                    </asp:Panel>
                                    <div style="clear: both;"></div>
                                </div>
                            </li>
                        </ul>
                    </div>

                </div>
            </div>


    <%------------------------->--%>

    <div class="container accordian-wrap">
        <div class="col s12 m12 l12">
            <div class="row">
                <ul class="collapsible popout collapsible-accordion form-accordion-head" data-collapsible="expandable">
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

     <!-- Hidden Field for the documents -->
    <asp:HiddenField runat="server" ID="hdnAddeDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDocument" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDocument" Value="Y" />

    <asp:HiddenField ID="hdnItemJSON" runat="server" />
    <asp:HiddenField ID="hdnIsInventoryTrackingIsOn" runat="server" Value="0"></asp:HiddenField>
     <asp:HiddenField ID="hdnCon" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript" src="Design/js/plugins/dropify/js/dropify.min.js"></script>

    <script type="text/javascript">

        function SelectAllCheckboxes(chk) {

            var grid = $find("<%=RadGrid_POItems.ClientID %>");
            var masterTable = grid.get_masterTableView();

            for (var i = 0; i < masterTable.get_dataItems().length; i++) {
                var gridItemElement = masterTable.get_dataItems()[i].findElement("chkSelectItem");

                gridItemElement.checked = chk.checked;

            }

            $("#<%=RadGrid_POItems.ClientID%> input[id*='chkSelectItem']:checkbox").each(function (index) {
                var chk = $(this).attr('id');
                var txtReceive = document.getElementById(chk.replace('chkSelectItem', 'txtReceive'));
                var lblDue = document.getElementById(chk.replace('chkSelectItem', 'lblOutstand'));
                var txtReceiveQty = document.getElementById(chk.replace('chkSelectItem', 'txtReceiveQty'));
                var lblQtyDue = document.getElementById(chk.replace('chkSelectItem', 'lblBOQty'));


                var due = parseFloatCustom($(lblDue).text().toString());
                var QtyDue = parseFloatCustom($(lblQtyDue).text());

                if ($(this).prop('checked') == true) {
                    $(txtReceiveQty).val(QtyDue.toLocaleString("en-US", { minimumFractionDigits: 4 }));
                    $(txtReceive).val(due.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    SelectedRowStyle('<%=RadGrid_POItems.ClientID %>');
                }
                else if ($(this).prop('checked') == false) {
                    due = 0;
                    $(txtReceiveQty).val(due.toLocaleString("en-US", { minimumFractionDigits: 4 }));
                    $(txtReceive).val(due.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    $(this).closest('tr').removeAttr("style");
                }
                GetTotal();

            });
        }

        function SelectedRowStyle(gridview) {
            var grid = document.getElementById(gridview);
            $('#' + gridview + ' tr').each(function () {
                var $tr = $(this);
                var chk = $tr.find('input[id*=chkSelect]');
                if (chk.prop('checked') == true) {
                    $tr.css('background-color', '#e0eefe');
                }
            });
        }

//---------------

        function AddDocumentClick(hyperlink) {

          <%--  var IsAdd = document.getElementById('<%= RadGrid_Documents.ClientID%>').value;
            if (IsAdd == "Y") {
                ConfirmUpload(ctl00_ContentPlaceHolder1_FileUpload1.value);
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }--%>
                 var IsAdd = document.getElementById('<%= hdnAddeDocument.ClientID%>').value;
            if (IsAdd == "Y") {
                ConfirmUpload(ctl00_ContentPlaceHolder1_FileUpload1.value)
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
            }
        }

        function DeleteDocumentClick(hyperlink) {
           // var IsDelete = document.getElementById('<%= RadGrid_Documents.ClientID%>').value;
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
<%--        function ConfirmUpload(value) {
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

        //--------------------------
    </script>




    <script type="text/javascript">

        $(document).ready(function () {

            //--------------->
            console.log("$(document).ready 6");
            //Materialize.updateTextFields();
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


            $(".dropdown-content.select-dropdown li").on("click", function () {
                var that = this;
                setTimeout(function () {
                    if ($(that).parent().hasClass('active')) {
                        $(that).parent().removeClass('active');
                        $(that).parent().hide();
                    }
                }, 100);
            });





            //------------------>

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

            ///////////// Quick Codes //////////////
            $("#<%=txtRcomments.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtRcomments.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });
        });

        function pageLoad(sender, args) {
            
            $("[id*=chkSelectItem]").change(function () {
                var chk = $(this).attr('id');
                var txtReceive = document.getElementById(chk.replace('chkSelectItem', 'txtReceive'));
                var lblDue = document.getElementById(chk.replace('chkSelectItem', 'lblOutstand'));
                var txtReceiveQty = document.getElementById(chk.replace('chkSelectItem', 'txtReceiveQty'));
                var lblQtyDue = document.getElementById(chk.replace('chkSelectItem', 'lblBOQty'));

                var due = parseFloatCustom($(lblDue).text().toString());
                var QtyDue = parseFloatCustom($(lblQtyDue).text());

                if ($(this).prop('checked') == true) {
                    $(txtReceiveQty).val(QtyDue.toLocaleString("en-US", { minimumFractionDigits: 4 }));
                    $(txtReceive).val(due.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    SelectedRowStyle('<%=RadGrid_POItems.ClientID %>');
                }
                else if ($(this).prop('checked') == false) {
                    due = 0;
                    $(txtReceiveQty).val(due.toLocaleString("en-US", { minimumFractionDigits: 4 }));
                    $(txtReceive).val(due.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                    $(this).closest('tr').removeAttr("style");
                }
                GetTotal();
            });

            $("[id*=txtReceiveQty]").change(function () {
                console.log("CONTENT3: txtReceiveQty - change");
                var txtReceiveQty = $(this).attr('id');
                var lblBOQty = document.getElementById(txtReceiveQty.replace('txtReceiveQty', 'lblBOQty'));
                var lblBO = document.getElementById(txtReceiveQty.replace('txtReceiveQty', 'lblOutstand'));
                var BO = parseFloat($(lblBO).text().toString().replace(/\(/, '-').replace(/[\$\(\),]/g, ''));
                var chk = document.getElementById(txtReceiveQty.replace('txtReceiveQty', 'chkSelectItem'));
                var txtReceive = document.getElementById(txtReceiveQty.replace('txtReceiveQty', 'txtReceive'));
                var lblPrice = document.getElementById(txtReceiveQty.replace('txtReceiveQty', 'lblPrice'));
                var Price = parseFloat($(lblPrice).text().toString().replace(/\(/, '-').replace(/[\$\(\),]/g, ''));
                var hdnMode = $("#<%=hdnMode.ClientID%>").val();
                var due = parseFloatCustom($(lblBOQty).text());
                var temTotal = 0;
                var receiveQuan = parseFloatCustom($(this).val());
                
                if (hdnMode === '1') {
                    console.log("Receive edit mode");

                    var hdnReQuan = document.getElementById(txtReceiveQty.replace('txtReceiveQty', 'hdnReceiveQty'));
                    var orgReQuan = parseFloatCustom($(hdnReQuan).val());
                    var availableQuan = due + orgReQuan;
                    
                    if ($(this).val() == '') {
                        $(this).val('0.00');
                    }
                    else {
                        if (receiveQuan >= availableQuan) {
                            //$(this).val(availableQuan);
                            //temTotal = BO;
                            $(this).val(availableQuan);
                            temTotal = availableQuan * Price;
                        }
                        else {
                            $(this).val(receiveQuan);
                            
                        }

                        if (temTotal == 0) {
                            var receiveQuanf = parseFloatCustom($(this).val());
                            temTotal = temTotal + (receiveQuanf * Price);
                            
                            // Check again 
                            if (temTotal == BO) {
                                $(this).val(availableQuan);
                            }
                        }
                    }
                } else if (hdnMode === '0') {
                    console.log("Receive add new mode");
                    if ($(this).val() == '') {
                        $(this).val('0.00');
                    }
                    else {
                        if (receiveQuan >= due) {
                            $(this).val(due);
                            temTotal = BO;
                        }
                        else {
                            $(this).val(receiveQuan);
                        }
                    }
                    if (temTotal == 0) {
                        var receiveQuanf = parseFloatCustom($(this).val());
                        temTotal = temTotal + (receiveQuanf * Price);

                        // Check again 
                        if (temTotal == BO) {
                            $(this).val(due);
                        }
                    }
                }

                $(txtReceive).val(temTotal.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                GetTotal();
            });
            $("[id*=txtReceive]").change(function () {
                var txtReceive = $(this).attr('id');
                console.log("CONTENT3: txtReceive - change: " + txtReceive);
                var ddd = txtReceive.split('_');
                var txt;
                if (ddd != null && ddd.length > 0) {
                    txt = ddd[ddd.length - 1];
                }

                if (txt != null && txt == 'txtReceive') {
                    console.log("CONTENT3: txtReceive - change");
                    var lblDue = document.getElementById(txtReceive.replace('txtReceive', 'lblOutstand'));
                    var lblBOQty = document.getElementById(txtReceive.replace('txtReceive', 'lblBOQty'));
                    var BOQty = parseFloatCustom($(lblBOQty).text());
                    var chk = document.getElementById(txtReceive.replace('txtReceive', 'chkSelectItem'));
                    var qty = document.getElementById(txtReceive.replace('txtReceive', 'txtReceiveQty'));
                    var lblPrice = document.getElementById(txtReceive.replace('txtReceive', 'lblPrice'));
                    var Price = parseFloat($(lblPrice).text().toString().replace(/\(/, '-').replace(/[\$\(\),]/g, ''));
                    var due = parseFloatCustom($(lblDue).text().toString());
                    var receive = parseFloatCustom($(this).val());
                    var hdnMode = $("#<%=hdnMode.ClientID%>").val();
                    var qtyf = 0;

                    if (hdnMode === '1') {
                        console.log("Receive edit mode");
                        var hdnReceive = document.getElementById(txtReceive.replace('txtReceive', 'hdnReceive'));
                        var hdnReceiveQty = document.getElementById(txtReceive.replace('txtReceive', 'hdnReceiveQty'));
                        var orgReceive = parseFloatCustom($(hdnReceive).val());
                        var orgReceiveQty = parseFloatCustom($(hdnReceiveQty).val());
                        var availableReceive = due + orgReceive;

                        if ($(this).val() == '') {
                            $(this).val('0.00');
                        }
                        else {
                            if (receive >= availableReceive) {
                                $(this).val(availableReceive);
                                //qtyf = BOQty;
                                qtyf = BOQty + orgReceiveQty;
                            }
                            else {
                                $(this).val(receive);
                            }
                        }

                        if (qtyf == 0) {
                            var receivef = parseFloatCustom($(this).val());

                            if (Price != 0) {
                                qtyf = receivef / Price;
                            }

                            // Check again 
                            if (qtyf == BOQty) {
                                $(this).val(availableReceive);
                            }
                        }
                    } else if (hdnMode === '0') {
                        console.log("Receive add new mode");
                        if ($(this).val() == '') {
                            $(this).val('0.00');
                        }
                        else {
                            if (receive >= due) {
                                $(this).val(due);
                                qtyf = BOQty;
                            }
                            else {
                                $(this).val(receive);
                            }
                        }
                        if (qtyf == 0) {
                            var receivef = parseFloatCustom($(this).val());

                            if (Price != 0) {
                                qtyf = receivef / Price;
                            }

                            // Check again 
                            if (qtyf == BOQty) {
                                $(this).val(due);
                            }
                        }
                    }

                    //if (qtyf == 0) {
                    //    var receivef = parseFloatCustom($(this).val());
                    //    if (Price != 0) {
                    //        qtyf = receivef / Price;
                    //    }
                    //}
                    $(qty).val(qtyf.toLocaleString("en-US", { minimumFractionDigits: 4 }));
                    
                    GetTotal();
                }
            });

            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.Acct = null;
            };

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

                    console.log("CONTENT3: txtGvWarehouse - autocomplete");
                    dtaaa.InvID = ID;
                    dtaaa.isShowAll = "yes";
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

                    var txtGvWarehouse = this.id;
                    var hdnWarehouse = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnWarehouse'));
                    var hdnInvID = document.getElementById(txtGvWarehouse.replace('txtGvWarehouse', 'hdnInvID'));


                    var Str = ui.item.WarehouseID + ", " + ui.item.WarehouseName;

                    $(this).val(Str);
                    $(txtGvWarehouse).val(Str);
                    $(hdnWarehouse).val(ui.item.WarehouseID);

                    var locationID = 0;
                    var warehouseID = $(hdnWarehouse).val();
                    var invID = $(hdnInvID).val();


                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.WarehouseID);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val());
            });

            $.each($(".Warehousesearchinput"), function (index, item) {
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
                    console.log("CONTENT3: Warehousesearchinput - ui-autocomplete");
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
                    console.log("CONTENT3: txtGvWarehouseLocation - autocomplete");
                    var txtGvWarehouseLocation_GetID = $(this.element).attr("id");
                    var hdnWarehouse = document.getElementById(txtGvWarehouseLocation_GetID.replace('txtGvWarehouseLocation', 'hdnWarehouse'));
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

                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.ID);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val());
            });
            $.each($(".WarehouseLocationsearchinput"), function (index, item) {
                $(item).data("ui-autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ID;
                    var result_item = item.Name;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>';
                    });
                    console.log("CONTENT3: WarehouseLocationsearchinput - ui-autocomplete");

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

            $("#<%=txtPO.ClientID%>").change(function () {
                var dtaaa = new dtaa();
                dtaaa.prefixText = $(this).val();
                console.log("CONTENT3: txtPO - change");
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "AccountAutoFill.asmx/GetPOByIdAjax",
                    data: '{"prefixText": "' + $(this).val() + '"}',
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var ui = $.parseJSON(data.d);

                        if (ui.length == 0) {
                            var strPo = $("#<%=txtPO.ClientID%>").val();
                            $("#<%=txtPO.ClientID%>").val('');
                            noty({
                                text: 'PO #' + strPo + ' doesn\'t exist!',
                                type: 'warning',
                                layout: 'topCenter',
                                closeOnSelfClick: false,
                                timeout: 5000,
                                theme: 'noty_theme_default',
                                closable: true
                            });
                        }
                        else {
                            if (ui[0].StatusName.toLowerCase() != "closed") {
                                console.log(ui[0].VendorName);
                                $("#<%=txtDueDate.ClientID%>").val(ui[0].Due);
                                $("#<%=txtVendor.ClientID%>").val(ui[0].VendorName);
                                $("#<%=txtAddress.ClientID%>").val(ui[0].Address);
                                $("#<%=hdnVendorID.ClientID%>").val(ui[0].Vendor);
                                $("#<%=txtShipTo.ClientID%>").val(ui[0].ShipTo);
                                $("#<%=txtCreatedBy.ClientID%>").val(ui[0].fBy);
                                $("#<%=txtStatus.ClientID%>").val(ui[0].StatusName);
                                $("#<%=txtComments.ClientID%>").val(ui[0].fDesc);
                                $("#<%=hdnAmount.ClientID%>").val(ui[0].Amount);
                                $("#<%=txtVendorType.ClientID%>").val(ui[0].VendorType);
                              <%-- document.getElementById('<%=btnSelectVendor.ClientID%>').click();--%>

                            } else {
                                var strPo = $("#<%=txtPO.ClientID%>").val();
                                $("#<%=txtPO.ClientID%>").val('');
                                noty({
                                    text: 'PO #' + strPo + ' was closed!',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: 5000,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                            }
                        }

                        document.getElementById('<%=btnSelectPO.ClientID%>').click();

                        //Materialize.updateTextFields();
                    },
                    error: function (result) {
                        alert("Due to unexpected errors we were unable to load PO");
                    }
                });

            });

            Materialize.updateTextFields();
        }
    </script>

</asp:Content>


