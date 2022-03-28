<%@ Page Title="Remit Tax || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="RemitTax" Codebehind="RemitTax.aspx.cs" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <script type="text/javascript">

    
    $(function () {
            $("[id*=txtPay]").change(function () {
                var txtPay = $(this).attr('id');

                var hdnDedID = document.getElementById(txtPay.replace('txtPay', 'hdnDedID'));
                var hdnBalance = document.getElementById(txtPay.replace('txtPay', 'hdnBalance'));
                var lblDue = document.getElementById(txtPay.replace('txtPay', 'lblBalance'));

                var chk = $(this).parent().prevAll().find('input:checkbox[id$="chkSelect"]');
                //var lblDue = $(this).parent().prev().prev().find('span[id$="lblBalance"]');
                //var hdnBalance = $(this).parent().prev().prev().find('input:hidden[id$="hdnBalance"]');
                ///var hdnDedID = $(this).parent().prev().prev().find('input:hidden[id$="hdnDedID"]');

                var pay = $(this).val().toString().replace(/[\$\(\),]/g, '');

                if (pay == '') {
                    pay = 0;
                    $(this).val('$0.00')
                }

                var DueAmt = parseFloat($(hdnBalance).val().replace(/[\$\(\),]/g, ''));
                var PayAmount = parseFloat(pay);
                var DueAmount = parseFloat($(hdnBalance).val().replace(/[\$\(\),]/g, ''));
                if (parseFloat(PayAmount) > parseFloat(DueAmount)) {
                    noty({
                        text: 'OverPayment is not allowed.',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: true,
                        timeout: false,
                        theme: 'noty_theme_default',
                        closable: false
                    });

                    pay = 0;
                    $(lblDue).text(cleanUpCurrency('$' + DueAmount.toLocaleString("en-US", { minimumFractionDigits: 2 })));

                    total = parseFloat(pay) ;
                    if (total == 0) {
                        $(this).val(pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        $(chk).prop('checked', true);
                        SelectedRowStyle('<%=RadGrid_RemitTax.ClientID %>')
                    }
                    else {
                        $(chk).prop('checked', false);
                        $(this).closest('tr').removeAttr("style");
                    }
                }
                else {
                    
                    $(lblDue).text(cleanUpCurrency('$' + parseFloat(DueAmt-PayAmount).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                    total = parseFloat(PayAmount);
                    if (total != 0) {
                        $(this).val(PayAmount.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                        $(chk).prop('checked', true);
                        SelectedRowStyle('<%=RadGrid_RemitTax.ClientID %>')
                    }
                    else {
                        $(chk).prop('checked', false);
                        $(this).closest('tr').removeAttr("style");
                    }
                }

                <%--CalculatePayTotal();
                CalculatePayTotalSelected();
                document.getElementById('<%=btnSelectChkBox.ClientID%>').click();--%>

            });

        $("[id*=chkSelect]").change(function () {
            try {
                var chk = $(this).attr('id');

                var hdnDedID = document.getElementById(chk.replace('chkSelect', 'hdnDedID'));
                var hdnBalance = document.getElementById(chk.replace('chkSelect', 'hdnBalance'));
                var lblDue = document.getElementById(chk.replace('chkSelect', 'lblBalance'));
                var txtPay = document.getElementById(chk.replace('chkSelect', 'txtPay'));

                var pay = $(txtPay).val().toString().replace(/[\$\(\),]/g, '');
                //////////////////////
                var dueAmt = parseFloat($(hdnBalance).val().toString().replace(/[\$\(\),]/g, ''))
                var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
                //var prevDue = parseFloat($(hdnOriginal).val() - $(hdnSelected).val())
                var pay = 0;

                var rpay = pay.toLocaleString("en-US", { minimumFractionDigits: 2 });
                var rprevDue = due.toLocaleString("en-US", { minimumFractionDigits: 2 });
                if ($(this).prop('checked') == true) {

                    $(txtPay).val(rprevDue)
                    
                    $(lblDue).text(cleanUpCurrency('$' + rpay))
                    SelectedRowStyle('<%=RadGrid_RemitTax.ClientID %>')
                }
                else if ($(this).prop('checked') == false) {
                    $(txtPay).val(rpay)
                    
                    $(lblDue).text(cleanUpCurrency('$' + dueAmt))
                    //  $(lblDue).text(cleanUpCurrency('$' + pay))
                    $(this).closest('tr').removeAttr("style");
                }
                

            } catch (e) {

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
        function isDecimalKey(el, evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode;

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

            var rawData = $('#<%=RadGrid_RemitTax.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            $('#<%=hdnGLItem.ClientID%>').val(formData);
        }
        function disableControl(control) {
            $(control).css("pointer-events", "none");
        }
        function enableControl(control) {
            setTimeout(function () { $(control).css("pointer-events", "all"); }, 1000);

        }
        function ConfirmRef(control) {

            disableControl(control);
            
            <%--var editCase = $('#<%=hdEditCase.ClientID%>').val();
            if (editCase === "true") {
                var qrStr = window.location.search;
                //qrStr = qrStr.split("?")[1].split("=")[1];
                qrStr = GetParameterValues('id');
                itemJSON();
                var urlsedit;
                if (document.getElementById('<%=chkIsRecurr.ClientID%>').checked) {
                    urlsedit = "CustomerAuto.asmx/GetBillRecurrRefExistEditAPBILL";
                }
                else {
                    urlsedit = "CustomerAuto.asmx/GetBillRefExistEditAPBILL";
                }
                $.ajax({
                    type: "POST",
                    // url: "AddBills.aspx/GetBillRefExistEdit",
                    url: urlsedit,
                    async: false,
                    data: '{Ref: "' + $("#<%=txtRef.ClientID%>")[0].value + '",VendorID: "' + $("#<%=hdnVendorID.ClientID%>")[0].value + '",PJID: "' + qrStr + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var chk = response.d;
                        if (chk === "True") {
                            var r = confirm("Ref number with this vendor already exists!");
                            
                            enableControl(control);
                            retVal = false;
                            return;
                            if (r === true) {
                                itemJSON();
                                enableControl(control);
                                retVal = true;
                              

                            } else {
                                enableControl(control);
                                $('#MOMloading').hide();
                                retVal = false;
                            }

                        }
                        else {
                            itemJSON();
                            enableControl(control);
                            retVal = true;
                        }
                    },
                    failure: function (response) {
                        enableControl(control);
                        $('#MOMloading').hide();
                        retVal = false;

                    }

                });
                retVal = true;

            }
            else {--%>
                var retVal = false;
                var vendorId = $('#<%=ddlVendor.ClientID%>').val();
                var urlsinsert;
                
                    urlsinsert = "CustomerAuto.asmx/GetBillRefExistAPBILL";
                
                if (vendorId !== '') {

                    $.ajax({
                        type: "POST",
                        
                        url: urlsinsert,
                        async: false,
                        data: '{Ref: "' + $("#<%=txtRef.ClientID%>")[0].value + '",VendorID: "' + $("#<%=ddlVendor.ClientID%>")[0].value + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            
                            var chk = response.d;
                            if (chk === "True") {
                                var r = confirm("Ref number with this vendor already exists!");
                                if (r === true) {
                                    itemJSON();
                                    enableControl(control);
                                    retVal = true;
                              
                                } else {
                                    enableControl(control);
                                    $('#MOMloading').hide();
                                    retVal = false;
                                }
                            }
                            else {
                                itemJSON();
                                enableControl(control);
                                retVal = true;

                            }
                        },
                        failure: function (response) {
                            enableControl(control);
                            $('#MOMloading').hide();
                            retVal = false;
                        }

                    });
                    return retVal;
                }
                else {
                    enableControl(control);
                }
            //}

        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Remit Tax</div>
                                    <asp:Panel runat="server" ID="pnlGridButtons">
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <%--OnClientClick='return AddRemitTaxClick(this)'--%>
                                                <asp:LinkButton ID="lnksubmit" runat="server" ToolTip="Save" CausesValidation="true" OnClientClick="disableButton(this,''); javascript:return ConfirmRef(this); itemJSON();"  OnClick="lnksubmit_Click">Save</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkQuickCheck" runat="server" ToolTip="Save" CausesValidation="true" OnClientClick="disableButton(this,''); javascript:return ConfirmRef(this); itemJSON();" OnClick="btnQuickCheck_Click">Quick Check</asp:LinkButton>
                                            </div>
                                            <%--<div class="btnlinks">
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClientClick='return EditRemitTaxClick(this)' OnClick="btnEdit_Click">Edit</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks menuAction">
                                                <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                                </a>
                                            </div>

                                            <ul id="drpMenu" class="nomgn hideMenu menuList">
                                                 <li>
                                                    <div class="btnlinks">
                                                        <a id="btnCopy" runat="server" onclientclick='return AddRemitTaxClick(this)' onserverclick="btnCopy_Click">Copy
                                                        </a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick='return DeleteRemitTaxClick(this)' OnClick="lnkDelete_Click">Delete</asp:LinkButton>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="btnlinks">
                                                        <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                    </div>
                                                </li>
                                                
                                            </ul>--%>
                                        </div>
                                    </asp:Panel>

                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" runat="server" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
                                    </div>
                                    <div class="rght-content">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </header>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="srchpane">
                <asp:UpdatePanel ID="upPannelSearch" runat="server" UpdateMode="Conditional">

                    <ContentTemplate>
                        




                        <div class="form-section-row">
                                            <div class="col s12">
                                                <div class="form-section-row">
                                                    <%--<div class="section-ttle">General</div>--%>
                                                    <div class="form-section3">
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <label for="sDate">Date</label>
                                                                            <%--<input id="sDate" type="text" class="datepicker_mom">--%>
                                                                            <asp:TextBox ID="txtHireDt" runat="server" CssClass="txtHireDt datepicker_mom validate" MaxLength="10"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvtxtHireDt" runat="server"
                                                                                ControlToValidate="txtHireDt" Display="None" ErrorMessage="Date Required"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            <asp:ValidatorCalloutExtender
                                                                                ID="vcrfvtxtHireDt" PopupPosition="TopLeft" runat="server" Enabled="True" CssClass="valiateField"
                                                                                TargetControlID="rfvtxtHireDt">
                                                                            </asp:ValidatorCalloutExtender>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                               <asp:TextBox ID="txtRef" runat="server" ></asp:TextBox>
                                                                <label for="txtRef">Ref #</label>
                                                                <asp:RequiredFieldValidator ID="rfvtxtRef" runat="server"
                                                                                ControlToValidate="txtRef" Display="None" ErrorMessage="Ref# Required"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            <asp:ValidatorCalloutExtender
                                                                                ID="vcrfvtxtRef" PopupPosition="TopLeft" runat="server" Enabled="True" CssClass="valiateField"
                                                                                TargetControlID="rfvtxtRef">
                                                                            </asp:ValidatorCalloutExtender>
                                                            </div>
                                                        </div>
                                                        
                                                        

                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtquarter" runat="server" ></asp:TextBox>
                                                                <label for="txtquarter">Quarter</label>
                                                                <asp:RequiredFieldValidator ID="rfvtxtquarter" runat="server"
                                                                                ControlToValidate="txtquarter" Display="None" ErrorMessage="Quarter Required"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            <asp:ValidatorCalloutExtender
                                                                                ID="vcrfvtxtquarter" PopupPosition="TopLeft" runat="server" Enabled="True" CssClass="valiateField"
                                                                                TargetControlID="rfvtxtquarter">
                                                                            </asp:ValidatorCalloutExtender>
                                                            </div>
                                                        </div>
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtyear" runat="server" ></asp:TextBox>
                                                                <label for="txtyear">Year</label>
                                                                <asp:RequiredFieldValidator ID="rfvtxtyear" runat="server"
                                                                                ControlToValidate="txtyear" Display="None" ErrorMessage="Year Required"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            <asp:ValidatorCalloutExtender
                                                                                ID="vcrfvtxtyear" PopupPosition="TopLeft" runat="server" Enabled="True" CssClass="valiateField"
                                                                                TargetControlID="rfvtxtyear">
                                                                            </asp:ValidatorCalloutExtender>
                                                            </div>
                                                        </div>
                                                        
                                                        
                                                    </div>
                                                    <div class="form-section3-blank">
                                                        &nbsp;
                                                    </div>
                                                    <div class="form-section3">
                                                        
                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                              <label class="drpdwn-label">Vendor</label>
                                                                <asp:DropDownList ID="ddlVendor" runat="server" CssClass="browser-default">                                                                    
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>


                                                        <div class="input-field col s12">
                                                            <div class="row">
                                                                <asp:TextBox ID="txtdesc" runat="server" ></asp:TextBox>
                                                                <label for="txtdesc">Description</label>
                                                                <asp:RequiredFieldValidator ID="rfvtxtdesc" runat="server"
                                                                                ControlToValidate="txtdesc" Display="None" ErrorMessage="Description Required"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            <asp:ValidatorCalloutExtender
                                                                                ID="vcrfvtxtdesc" PopupPosition="TopLeft" runat="server" Enabled="True" CssClass="valiateField"
                                                                                TargetControlID="rfvtxtdesc">
                                                                            </asp:ValidatorCalloutExtender>
                                                            </div>
                                                        </div>
                                                        
                                                       
                                                        
                                                    </div>
                                                    <%--<div class="section-ttle">General</div>--%>
                                                </div>
                                                <div class="cf"></div>
                                            </div>
                                            <div class="cf"></div>
                                        </div>






                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="col lblsz2 lblszfloat">
                    <div class="row">
                        <span class="tro trost">
                            <asp:CheckBox ID="lnkChk" runat="server" OnCheckedChanged="lnkchk_Click" AutoPostBack="True" CssClass="css-checkbox" Text="Incl. Inactive"  style="display:none;"></asp:CheckBox>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click"  style="display:none;">Show All </asp:LinkButton>
                        </span>
                        <span class="tro trost">
                            <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click" style="display:none;">Clear</asp:LinkButton>
                        </span>
                        <%--<span class="tro trost">
                             <asp:Label ID="lblRecordCount" runat="server">0 Record(s) found.</asp:Label>
                            
                        </span>--%>
                    </div>
                </div>
            </div>
        </div>

        <div class="grid_container">
            <div class="form-section-row m-b-0" >

                <telerik:RadAjaxManager ID="RadAjaxManager_WageDeduction" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="lnkDelete">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RemitTax" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RemitTax" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                                
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkChk">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RemitTax" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RemitTax" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_RemitTax">
                            <UpdatedControls>   
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RemitTax" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <%--<telerik:AjaxSetting AjaxControlID="ddlSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RemitTax" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="ddlType"  />
                                <telerik:AjaxUpdatedControl ControlID="ddlStatus"  />
                                <telerik:AjaxUpdatedControl ControlID="txtSearch"  />
                                
                                
                            </UpdatedControls>
                        </telerik:AjaxSetting>--%>
                        
                        <%--<telerik:AjaxSetting AjaxControlID="lnkClear">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RemitTax" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>--%>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_WageDeduction" runat="server">
                </telerik:RadAjaxLoadingPanel>

                <div class="RadGrid RadGrid_Material">
                    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                        <script type="text/javascript">
                            function pageLoad() {
                                var grid = $find("<%= RadGrid_RemitTax.ClientID %>");
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
                    <telerik:RadAjaxPanel ID="RadAjaxPanel_Deduction" runat="server" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                        <%--OnItemEvent="RadGrid_RemitTax_ItemEvent" OnExcelMLExportRowCreated="RadGrid_RemitTax_ExcelMLExportRowCreated"--%>
                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_RemitTax" AllowFilteringByColumn="true" ShowFooter="True" PageSize="50"  FilterType="CheckList" 
                                                                                                OnNeedDataSource="RadGrid_RemitTax_NeedDataSource"  OnExcelMLExportRowCreated="RadGrid_RemitTax_ExcelMLExportRowCreated"
                                                                                                 OnItemCreated="RadGrid_RemitTax_ItemCreated" OnItemEvent="RadGrid_RemitTax_ItemEvent"
                                                                                                PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_RemitTax_PreRender"
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
                                                                                                        <%--<telerik:GridClientSelectColumn UniqueName="chkSelect" HeaderStyle-Width="40">
                                                                                                                </telerik:GridClientSelectColumn>--%>

                                                                                                        <telerik:GridTemplateColumn HeaderStyle-Width="50" ShowFilterIcon="false" UniqueName="ClientSelectColumn">
											                                                                <HeaderTemplate>
												                                                                <asp:CheckBox ID="chkSelectAll" runat="server"/>
											                                                                </HeaderTemplate>
											                                                                <ItemTemplate>
												                                                                <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true"/>
												                                                                <asp:HiddenField ID="hdnDedID" Value='<%# Bind("ID") %>' runat="server" />
												                                                                <asp:HiddenField ID="hdnBalance" Value='<%# Bind("Balance") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnType" Value='<%# Bind("Type") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnByW" Value='<%# Bind("ByW") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnBasedOn" Value='<%# Bind("BasedOn") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnAccruedOn" Value='<%# Bind("AccruedOn") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnEmpRate" Value='<%# Bind("EmpRate") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnEmpTop" Value='<%# Bind("EmpTop") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnEmpGL" Value='<%# Bind("EmpGL") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnCompRate" Value='<%# Bind("CompRate") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnCompTop" Value='<%# Bind("CompTop") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnCompGL" Value='<%# Bind("CompGL") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnCompGLE" Value='<%# Bind("CompGLE") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnPaid" Value='<%# Bind("Paid") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnVendor" Value='<%# Bind("Vendor") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnDedType" Value='<%# Bind("DedType") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnReimb" Value='<%# Bind("Reimb") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnJob" Value='<%# Bind("Job") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnBox" Value='<%# Bind("Box") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnFrequency" Value='<%# Bind("Frequency") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnProcess" Value='<%# Bind("Process") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnEmpGLAcct" Value='<%# Bind("EmpGLAcct") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnCompGLAcct" Value='<%# Bind("CompGLAcct") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnCompeGLEAcct" Value='<%# Bind("CompGLEAcct") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnfdesc" Value='<%# Bind("fdesc") %>' runat="server" /> 

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
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="TypeName" HeaderText="Type" UniqueName="TypeName" SortExpression="TypeName"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblTypeName" runat="server" Text='<%# Eval("TypeName") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="VendorName" HeaderText="Vendor" UniqueName="VendorName" SortExpression="VendorName"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblVendorName" runat="server" Text='<%# Eval("VendorName") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="Balance" HeaderText="Balance" UniqueName="Balance" SortExpression="Balance"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblBalance" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}") %>' DataFormatString="{0:n}"></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn    HeaderText="ToPay" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtPay" runat="server" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>


                                                                                                               
                                                                                                        
                                                                                                    </Columns>
                                                                                                </MasterTableView>
                                                                                            </telerik:RadGrid>

                        
                    </telerik:RadAjaxPanel>
                </div>

            </div>
        </div>
    </div>

    <!-- Hidden Field -->
    <asp:HiddenField runat="server" ID="hdnAddDedcutions" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnEditDedcutions" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnDeleteDedcutions" Value="Y" />
    <asp:HiddenField runat="server" ID="hdnViewDedcutions" Value="Y" />
    <asp:HiddenField ID="hdnGLItem" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script type="text/javascript">
        function AddRemitTaxClick(hyperlink) {
            var id = document.getElementById('<%= hdnAddDedcutions.ClientID%>').value;
            if (id == "Y") {
                return true;
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function EditRemitTaxClick(hyperlink) {

            var id = document.getElementById('<%= hdnEditDedcutions.ClientID%>').value;
            if (id == "Y") { return true; } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        function DeleteRemitTaxClick(hyperlink) {
            var id = document.getElementById('<%= hdnDeleteDedcutions.ClientID%>').value;
            if (id == "Y") {
                return SelectedRowDelete('<%= RadGrid_RemitTax.ClientID%>', 'Vendor');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        
    function clear() {
        ("#ddlType");
    }
        
        
    </script>


    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('#colorNav #dynamicUI li').remove();
            //$(reports).each(function (index, report) {
            //    var imagePath = null;
            //    if (report.IsGlobal == true) {
            //        imagePath = "images/globe.png";
            //    }
            //    else {
            //        imagePath = "images/blog_private.png";
            //    }

            //    $('#dynamicUI').append('<li><a href="VendorListReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Vendor"><span><img src=images/reportfolder.png> ' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')

            //});


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
</asp:Content>


