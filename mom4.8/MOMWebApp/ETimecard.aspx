<%@ Page Title="Run Payroll || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="ETimecard" Codebehind="ETimecard.aspx.cs" EnableEventValidation="false" ValidateRequest="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Stimulsoft.Report.WebDesign" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<%@ Register Assembly="Stimulsoft.Report.Web" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
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
                        SelectedRowStyle('<%=RadGrid_RunPayroll.ClientID %>')
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
                        SelectedRowStyle('<%=RadGrid_RunPayroll.ClientID %>')
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
                    SelectedRowStyle('<%=RadGrid_RunPayroll.ClientID %>')
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

            var rawData = $('#<%=RadGrid_RunPayroll.ClientID%>').serializeFormJSON();
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
                                    <div class="page-title"><i class="mdi-communication-contacts"></i>&nbsp;Time Card Input</div>
                                    <asp:Panel runat="server" ID="pnlGridButtons">
                                        <div class="buttonContainer">
                                            <div class="btnlinks">
                                                <%--OnClientClick='return AddRemitTaxClick(this)'--%>
                                                <asp:LinkButton ID="lnksubmit" runat="server" ToolTip="Save" CausesValidation="true" OnClientClick="disableButton(this,''); javascript:return ConfirmRef(this); itemJSON();"  OnClick="lnksubmit_Click">Save</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkProcessPayroll" runat="server" ToolTip="Process Payroll" CausesValidation="true" OnClientClick="itemJSON(); OpenBankModal();return false" style="display:none;">Process Payroll</asp:LinkButton>
                                            </div>
                                            <div class="btnlinks">
                                                <asp:LinkButton ID="lnkQuickCheck" runat="server" ToolTip="Process Other Deductions" CausesValidation="true" OnClientClick="disableButton(this,''); javascript:return ConfirmRef(this); itemJSON();" OnClick="btnQuickCheck_Click" style="display:none;">Process Other Deductions</asp:LinkButton>
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
                <asp:UpdatePanel ID="upPannelSearch" runat="server" >

                    <ContentTemplate>
                        




                        <div class="srchpaneinner">
                        <div class="col s12 m12 l12">
                            <div class="row">
                                <div class="srchinputwrap">
                                    <label class="drpdwn-label">Supervisor</label>
                                     <asp:DropDownList ID="ddlSuper" runat="server" CssClass="browser-default selectst">                                                                    
                                                                </asp:DropDownList>
                                    
                                </div>
                                <div class="srchinputwrap">
                                    <label class="drpdwn-label">Worker <span class="reqd">*</span></label>
                                    <select class="browser-default selectst" id="ddlEmployee">
                                        <option value="">Select Employee</option>
                                    </select>
                                </div>
                                <div class="srchinputwrap">
                                    <label class="drpdwn-label">Category <span class="reqd">*</span></label>
                                    <select class="browser-default selectst" id="ddlCategory" onchange="updateCategory();">
                                        <option value="">Select Category</option>
                                        <option value="None">None</option>
                                    </select>
                                </div>
                               <%-- <div class="srchinputwrap">
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
                                </div>--%>
                            </div>
                        </div>
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
            <div class="form-section-row" style="margin-bottom: 0 !important;">

                <telerik:RadAjaxManager ID="RadAjaxManager_WageDeduction" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="lnkDelete">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll"  />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll"  />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                                
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkChk">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll"  />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="lnkShowAll">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="RadGrid_RunPayroll">
                            <UpdatedControls>   
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll"  />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="btnGetDetail">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="shdnEmpID"/>
                                <%--<telerik:AjaxUpdatedControl ControlID="RadGridPayrollHours"/>--%>
                                
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <%--<telerik:AjaxSetting AjaxControlID="lnkprintchecktemp">
                            <UpdatedControls>   
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="lblRecordCount"  />
                            </UpdatedControls>
                        </telerik:AjaxSetting>--%>
                        

                        <%--<telerik:AjaxSetting AjaxControlID="ddlSearch">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                                <telerik:AjaxUpdatedControl ControlID="ddlType"  />
                                <telerik:AjaxUpdatedControl ControlID="ddlStatus"  />
                                <telerik:AjaxUpdatedControl ControlID="txtSearch"  />
                                
                                
                            </UpdatedControls>
                        </telerik:AjaxSetting>--%>
                        
                        <%--<telerik:AjaxSetting AjaxControlID="lnkClear">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="RadGrid_RunPayroll" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>--%>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
               <%-- <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_WageDeduction" runat="server">
                </telerik:RadAjaxLoadingPanel>--%>

                <div class="RadGrid RadGrid_Material">
                    <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                        <script type="text/javascript">
                            function pageLoad() {
                                var grid = $find("<%= RadGrid_RunPayroll.ClientID %>");
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
                    <%--<telerik:RadAjaxPanel ID="RadAjaxPanel_Deduction" runat="server" LoadingPanelID="RadAjaxLoadingPanel_WageDeduction" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">--%>
                        <%--OnItemEvent="RadGrid_RunPayroll_ItemEvent" OnExcelMLExportRowCreated="RadGrid_RunPayroll_ExcelMLExportRowCreated"--%>
                        <telerik:RadGrid RenderMode="Auto" ID="RadGrid_RunPayroll"  ShowFooter="True" PageSize="50"  FilterType="CheckList" 
                                                                                                OnNeedDataSource="RadGrid_RunPayroll_NeedDataSource"  OnExcelMLExportRowCreated="RadGrid_RunPayroll_ExcelMLExportRowCreated"
                                                                                                 OnItemCreated="RadGrid_RunPayroll_ItemCreated" OnItemEvent="RadGrid_RunPayroll_ItemEvent"
                                                                                                PagerStyle-AlwaysVisible="true" OnPreRender="RadGrid_RunPayroll_PreRender"
                                                                                                ShowStatusBar="true" runat="server" AllowPaging="false" AllowSorting="true" Width="100%" AllowCustomPaging="false">
                                                                                                <CommandItemStyle />
                                                                                                <GroupingSettings CaseSensitive="false" />
                                                                                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                                                                                    <Selecting AllowRowSelect="True"></Selecting>
                                                                                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                                                                                    <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                                                                                </ClientSettings>
                                                                                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True">
                                                                                                    <Columns>
                                                                                                        

                                                                                                        <%--<telerik:GridTemplateColumn HeaderStyle-Width="50" ShowFilterIcon="false" UniqueName="ClientSelectColumn">
											                                                                <HeaderTemplate>
												                                                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="false"/>
											                                                                </HeaderTemplate>
											                                                                <ItemTemplate>
												                                                                <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true"/>
												                                                                <asp:HiddenField ID="hdnpmethod" Value='<%# Bind("pmethod") %>' runat="server" />
												                                                                <asp:HiddenField ID="hdnphour" Value='<%# Bind("phour") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnsalary" Value='<%# Bind("salary") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnpay" Value='<%# Bind("pay") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnusertype" Value='<%# Bind("usertype") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnuserid" Value='<%# Bind("userid") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnid" Value='<%# Bind("ID") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnName" Value='<%# Bind("Name") %>' runat="server" /> 

                                                                                                                
                                                                                                                <asp:HiddenField ID="hdnOtherE" Value='<%# Bind("OtherE") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnsicktime" Value='<%# Bind("sicktime") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdntotal" Value='<%# Bind("total") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnHourlyRate" Value='<%# Bind("HourlyRate") %>' runat="server" /> 

                                                                                                                <asp:HiddenField ID="hdnlblName" Value='<%# Bind("Name") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnlblMethod" Value='<%# Bind("paymethod") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnlblReg" Value='<%# Bind("Reg") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnlblOT" Value='<%# Bind("OT") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnlblNT" Value='<%# Bind("NT") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnlblDT" Value='<%# Bind("DT") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnlblTT" Value='<%# Bind("TT") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnlblHoliday" Value='<%# Bind("holiday") %>' runat="server" /> 
                                                                                                                <asp:HiddenField ID="hdnlblVac" Value='<%# Bind("vacation") %>' runat="server" /> 
                                                                                                                
											                                                                </ItemTemplate>
										                                                                </telerik:GridTemplateColumn>--%>

                                                                                                                <telerik:GridTemplateColumn UniqueName="lblName" FilterDelay="5" DataField="Name" HeaderText="Name" SortExpression="Name" 
                                                                                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" ItemStyle-Width="150" HeaderStyle-Width="150">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:HyperLink ID="lblName" runat="server" Text='<%# Eval("Name") %>' ></asp:HyperLink>
                                                                                                                        <asp:HiddenField ID="hdnpmethod" Value='<%# Bind("pmethod") %>' runat="server" />
												                                                                        <asp:HiddenField ID="hdnphour" Value='<%# Bind("phour") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnsalary" Value='<%# Bind("salary") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnpay" Value='<%# Bind("pay") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnusertype" Value='<%# Bind("usertype") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnuserid" Value='<%# Bind("userid") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnid" Value='<%# Bind("ID") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnName" Value='<%# Bind("Name") %>' runat="server" /> 

                                                                                                                        <asp:HiddenField ID="hdnOtherE" Value='<%# Bind("OtherE") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnsicktime" Value='<%# Bind("sicktime") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdntotal" Value='<%# Bind("total") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnHourlyRate" Value='<%# Bind("HourlyRate") %>' runat="server" /> 

                                                                                                                        <asp:HiddenField ID="hdnlblName" Value='<%# Bind("Name") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnlblMethod" Value='<%# Bind("paymethod") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnlblReg" Value='<%# Bind("Reg") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnlblOT" Value='<%# Bind("OT") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnlblNT" Value='<%# Bind("NT") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnlblDT" Value='<%# Bind("DT") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnlblTT" Value='<%# Bind("TT") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnlblHoliday" Value='<%# Bind("holiday") %>' runat="server" /> 
                                                                                                                        <asp:HiddenField ID="hdnlblVac" Value='<%# Bind("vacation") %>' runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="Date" HeaderText="Date" UniqueName="Date" SortExpression="Date"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Date") %>' ></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="Start" HeaderText="Start" UniqueName="Start" SortExpression="Start"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtStart" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Start") %>' ></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="Resolution" HeaderText="Resolution" UniqueName="Resolution" SortExpression="Resolution"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtResolution" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Resolution") %>' ></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                



                                                                                                                <telerik:GridTemplateColumn   DataField="Reg" HeaderText="Reg" UniqueName="Reg" SortExpression="Reg"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtReg" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Reg", "{0:c}") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="OT" HeaderText="OT" UniqueName="OT" SortExpression="OT"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>                                                                                                                        
                                                                                                                        <asp:TextBox ID="txtOT" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OT", "{0:c}") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="NT" HeaderText="1.7" UniqueName="NT" SortExpression="NT"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>                                                                                                                        
                                                                                                                        <asp:TextBox ID="txtNT" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NT", "{0:c}") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="DT" HeaderText="DT" UniqueName="DT" SortExpression="DT"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>                                                                                                                        
                                                                                                                        <asp:TextBox ID="txtDT" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DT", "{0:c}") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn   DataField="TT" HeaderText="Travel" UniqueName="TT" SortExpression="TT"
                                                                                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>                                                                                                                        
                                                                                                                        <asp:TextBox ID="txtTT" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "TT", "{0:c}") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn    HeaderText="Mileage" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtMilage" runat="server" onkeypress="return isDecimalKey(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "Mileage", "{0:c}") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn    HeaderText="Zone" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtZone" runat="server" onkeypress="return isDecimalKey(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "ZONE", "{0:c}") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn    HeaderText="Reimb" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtReimb" runat="server" onkeypress="return isDecimalKey(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "reimb", "{0:c}") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn    HeaderText="Project" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtProject" runat="server" onkeypress="return isDecimalKey(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "Project") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn    HeaderText="Type" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtType" runat="server" onkeypress="return isDecimalKey(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "Type") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn    HeaderText="Wage" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtWage" runat="server" onkeypress="return isDecimalKey(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "Wage") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn    HeaderText="Unit" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtUnit" runat="server" onkeypress="return isDecimalKey(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "Unit") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn    HeaderText="Group" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtGroup" runat="server" onkeypress="return isDecimalKey(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "Group") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>
                                                                                                                <telerik:GridTemplateColumn    HeaderText="WO#" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:TextBox ID="txtWO" runat="server" onkeypress="return isDecimalKey(this,event)" Text='<%# DataBinder.Eval(Container.DataItem, "WO") %>' DataFormatString="{0:n}"></asp:TextBox>
                                                                                                                    </ItemTemplate>
                                                                                                                </telerik:GridTemplateColumn>

                                                                                                        
                                                                                                    </Columns>
                                                                                                </MasterTableView>
                                                                                            </telerik:RadGrid>

                        
                    <%--</telerik:RadAjaxPanel>--%>
                    <div id="EmpPayrollDetail"   style="display: none;">
                        <iframe id="PayrollDetailiframe" style="border:none!important; width:100%!important;height:700px!important"></iframe>
                    </div>
                    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="PayrollDetail" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                                                    Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                                                    runat="server" Modal="true" Width="1000" Height="700">
                                        </telerik:RadWindow>
                                    <telerik:RadWindow ID="RadWindowBank" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                                                    Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                                                    runat="server" Modal="true" Width="500" Height="400">
                                        <ContentTemplate>
                                            <div style="margin-top: 15px;">
                                                                            <div class="form-section-row">
                                                                                <div class="form-section">
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label class="drpdwn-label">Bank Account </label>
                                                                                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="browser-default" ValidationGroup="Check"
                                                                                                    >
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator runat="server" ID="rfvBank" ControlToValidate="ddlBank"
                                                                                                    ErrorMessage="Please select Bank" Display="None" InitialValue="0"
                                                                                                    ValidationGroup="Check"></asp:RequiredFieldValidator>
                                                                                                <asp:ValidatorCalloutExtender ID="vceBank" runat="server" Enabled="True" PopupPosition="Right"
                                                                                                    TargetControlID="rfvBank" />                                    </div>
                                                                                    </div>
                                                                                </div>
                                                                                <%--<div class="form-section3-blank">
                                                                                    &nbsp;
                                                                                </div>--%>
                                                                                <div class="form-section">
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <label for="txtcheckdate">Check Date</label>
                                                                                            <asp:TextBox ID="txtcheckdate" runat="server" CssClass="txtendDt datepicker_mom validate" MaxLength="10"></asp:TextBox>

                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                
                                                                                <div class="form-section">
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <asp:TextBox ID="txtcheck" runat="server" MaxLength="19" CssClass="Contact-search"></asp:TextBox>
                                                                                            <label for="txtcheck">Check No</label>
                                                                                            <asp:TextBox ID="txtcheckfrom" runat="server" MaxLength="19" CssClass="Contact-search" style="display:none;"></asp:TextBox>
                                                                                            <asp:TextBox ID="txtcheckto" runat="server" MaxLength="19" CssClass="Contact-search" style="display:none;"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="form-section">
                                                                                    <div class="input-field col s12">
                                                                                        <div class="row">
                                                                                            <asp:TextBox ID="txtcheckmemo" runat="server" CssClass="Contact-search"></asp:TextBox>
                                                                                            <label for="txtcheckmemo">Memo on Check</label>
                                                                                                                                    </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div style="clear: both;"></div>
                                                                            <footer style="float: left; padding-left: 0 !important; margin-top: -30px;">
                                                                                <div class="btnlinks">
                                                                                    <%--<a onclick="CloseBankModal();">Print Check</a>--%>
                                                                                    <asp:LinkButton ID="lnkprintchecktemp" runat="server" OnClick="lnkprintchecktemp_Click"  >Print Checks</asp:LinkButton>
                                                                                </div>
                                                                            </footer>
                                                                        </div>
                                        </ContentTemplate>
                                        </telerik:RadWindow>
                                    
                    <telerik:RadWindow ID="RadWindowTemplates" Skin="Material" VisibleTitlebar="true" Behaviors="Default" CenterIfModal="true"
                                                                        Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                                                                        runat="server" Modal="true" Width="1100"  Title="Check Templates">
                                                                       <ContentTemplate>
                                                                            <div>
                                                                                <%--<div class='col s5' style="width: 100%; float: left;">
                                                                                    <div class='cr-title' style="    padding-top: 5px;   font-size: large;   padding-bottom: 5px;">Check Templates </div>
                                                                                </div>--%>
                                                                                <div class='col s5' style="width: 100%; float: left;">
                                                                                    <div class='cr-title' style="    padding-top: 5px;   font-size: initial;   padding-bottom: 5px;">Select a check template. Please note checks will be saved after you exit this screen. </div>
                                                                                </div>
                                                                                
                                                                                <div class='col s5' style="width: 30%; float: left; padding-top: 5px;  margin-bottom: 15px;margin-right: 30px;">
                                                                                    <div class='cr-box'>
                                                                                        <div class='cr-title'>AP – check top </div>
                                                                                        
                                                                                        <%--<div class='cr-img'>
                                                                                            
                                                                                            <asp:Label ID="lbltopcom" runat="server" Text="XYZ Company" style="position: absolute; padding-left: 20px; padding-top: 15px; font-weight: bolder; font-size: 12px;"></asp:Label>
                                                                                            <asp:Label ID="lbltopdd" runat="server" Text="9418 Galvin Ave, ,San Diago, Suit #100" style="position: absolute; padding-left: 20px; padding-top: 60px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lbltopemail" runat="server" Text="info@expertservicesolution.com" style="position: absolute; padding-left: 20px; padding-top: 80px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                            
                                                                                        </div>
                                                                                        <div class='cr-img'>
                                                                                            <img src='images/ReportImages/ApTopCheck.jpg' alt='' style="position: absolute;margin-top: 40px;height: 265px;width: 320px;">
                                                                                        </div>--%>
                                                                                        <div class='cr-date' >
                                                                                            <div class='cr-iocn'>
                                                                                                <asp:DropDownList ID="ddlApTopCheckForLoad" runat="server"
                                                                                                    CssClass="browser-default" OnSelectedIndexChanged="ddlApTopCheckForLoad_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                                <div class='cr-iocn'>
                                                                                                    <asp:ImageButton ID="imgPrintTemp1" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp1_Click" ToolTip="Export to PDF" />
                                                                                                    <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="25px" Width="25px"  OnClick="ImageButton7_Click" ToolTip="Edit Template" />
                                                                                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkSaveDefault_Click" ToolTip="Set as Default" />
                                                                                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" OnClick="ImageButton3_Click" />
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div style="clear: both;"></div>
                                                                                </div>
                                                                                  <div class='col s5' style="width: 30%; float: left; padding-top: 5px;  margin-bottom: 15px;margin-right: 30px;">
                                                                                    <div class='cr-box'>
                                                                                        <div class='cr-title'>AP – check middle </div>
                                                                                        
                                                                                        <%--<div class='cr-img'>
                                                                                            
                                                                                            <asp:Label ID="lblmidcom" runat="server" Text="XYZ Company" style="position: absolute; padding-left: 20px; padding-top: 15px; font-weight: bolder; font-size: 12px;"></asp:Label>
                                                                                            <asp:Label ID="lblmidadd" runat="server" Text="9418 Galvin Ave, ,San Diago, Suit #100" style="position: absolute; padding-left: 20px; padding-top: 60px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lblmidemail" runat="server" Text="info@expertservicesolution.com" style="position: absolute; padding-left: 20px; padding-top: 80px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                        </div>
                                                                                        <div class='cr-img'>
                                                                                            <img src='images/ReportImages/MidCheck.jpg' alt='' style="position: absolute;margin-top: 40px;height: 265px;width: 320px;">
                                                                                        </div>--%>
                                                                                        <div class='cr-date' >

                                                                                            <asp:DropDownList ID="ddlApMiddleCheckForLoad" runat="server"
                                                                                                CssClass="browser-default" OnSelectedIndexChanged="ddlApMiddleCheckForLoad_SelectedIndexChanged">
                                                                                            </asp:DropDownList>
                                                                                            <div class='cr-iocn' >
                                                                                                <asp:ImageButton ID="imgPrintTemp2" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp2_Click" ToolTip="Export to PDF" />
                                                                                                <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="30px" Width="30px" OnClick="ImageButton8_Click" ToolTip="Edit Template" />
                                                                                                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkSaveApMiddleCheck_Click" ToolTip="Set as Default" />
                                                                                                <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" OnClick="ImageButton6_Click" />

                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div style="clear: both;"></div>
                                                                                </div>
                                                                                  <div class='col s5' style="width: 30%; float: left; padding-top: 5px; margin-bottom: 15px;margin-right: 30px;">
                                                                                    <div class='cr-box'>
                                                                                        <div class='cr-title'>AP – Detailed check top </div>
                                                                                        
                                                                                        <%--<div class='cr-img'>
                                                                                            
                                                                                            <asp:Label ID="lbldetailcom" runat="server" Text="XYZ Company" style="position: absolute; padding-left: 20px; padding-top: 15px; font-weight: bolder; font-size: 12px;"></asp:Label>
                                                                                            <asp:Label ID="lbldetailadd" runat="server" Text="9418 Galvin Ave, ,San Diago, Suit #100" style="position: absolute; padding-left: 20px; padding-top: 60px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                            <asp:Label ID="lbldetailemail" runat="server" Text="info@expertservicesolution.com" style="position: absolute; padding-left: 20px; padding-top: 80px; font-size: 10px;" Visible="false"></asp:Label>
                                                                                        </div>
                                                                                        <div class='cr-img'>
                                                                                            <img src='images/ReportImages/TopDetailCheck.jpg' alt='' style="position: absolute;margin-top: 40px;height: 265px;width: 320px;">
                                                                                        </div>--%>
                                                                                        <div class='cr-date' >
                                                                                            <asp:DropDownList ID="ddlTopChecksForLoad" runat="server"
                                                                                                CssClass="browser-default" OnSelectedIndexChanged="ddlTopChecksForLoad_SelectedIndexChanged">
                                                                                            </asp:DropDownList>

                                                                                            <div class='cr-iocn' >
                                                                                                <asp:ImageButton ID="imgPrintTemp6" runat="server" ImageUrl="images/ReportImages/cr-iocn1.png" Height="30px" Width="30px" OnClick="imgPrintTemp6_Click" ToolTip="Export to PDF" />
                                                                                                <asp:ImageButton ID="ImageButton9" runat="server" ImageUrl="images/ReportImages/cr-iocn5.png" Height="30px" Width="30px" OnClick="ImageButton9_Click" ToolTip="Edit Template" />
                                                                                                <asp:ImageButton ID="ImageButton13" runat="server" ImageUrl="images/ReportImages/cr-iocn3.png" Height="30px" Width="30px" OnClick="lnkTopChecks_Click" />
                                                                                                <asp:ImageButton ID="ImageButton14" runat="server" ImageUrl="images/ReportImages/Delete.png" Height="30px" Width="30px" OnClientClick="if (!confirm('Are you sure you want to delete check template?')) return false;" OnClick="ImageButton14_Click" />

                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div style="clear: both;"></div>
                                                                            </div>
                                                                            <div class="btnlinks">
                                                                                <asp:LinkButton ID="btnSave2" runat="server" Visible="false" ValidationGroup="Check" CausesValidation="true" OnClick="btnSubmit_Click">
                                                                                                   Cut Check
                                                                                </asp:LinkButton>
                                                                                <asp:Label ID="txtMessage" runat="server" ForeColor="Green"></asp:Label>
                                                                            </div>
                                                                          <div id="loaders" style="text-align: center; padding-top: 35px; display: none;"><img src="images/ajax-loader-trans.gif" style="height: 30px;" /> </div>
                                                                        </ContentTemplate>

                                                                    </telerik:RadWindow>
                                                                    <telerik:RadWindow ID="RadWindowFirstReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true" 
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1200" Height="700" >
                <ContentTemplate>
                    <div class="rptSti">
                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner1" runat="server" OnSaveReport="StiWebDesigner1_SaveReport" Height="700" Width="100%" OnSaveReportAs="StiWebDesigner1_SaveReportAs" OnExit="StiWebDesigner1_Exit" />
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowSecondReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1200" Height="700" >
                <ContentTemplate>
                    <div class="rptSti">
                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner2" runat="server" OnSaveReport="StiWebDesigner2_SaveReport" Height="700" Width="100%" OnSaveReportAs="StiWebDesigner2_SaveReportAs" OnExit="StiWebDesigner2_Exit" />
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>

            <telerik:RadWindow ID="RadWindowThirdReport" Skin="Material" VisibleTitlebar="true" Title="Edit Templates" Behaviors="Default" CenterIfModal="true"
                Animation="FlyIn" AnimationDuration="200" RenderMode="Auto" VisibleStatusbar="false" MinWidth="450"
                runat="server" Modal="true" Width="1200" Height="700" >
                <ContentTemplate>
                    <div class="rptSti">
                        <cc1:StiWebDesigner RequestTimeout="900000" Visible="false" ID="StiWebDesigner3" runat="server" OnSaveReport="StiWebDesigner3_SaveReport" Height="700" Width="100%" OnSaveReportAs="StiWebDesigner3_SaveReportAs" OnExit="StiWebDesigner3_Exit" />
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
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
    <asp:HiddenField runat="server" ID="shdnEmpID" />
    <asp:Button ID="btnGetDetail" runat="server" OnClick="btnGetDetail_Click" CausesValidation="false" style="display:none;" />
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
                return SelectedRowDelete('<%= RadGrid_RunPayroll.ClientID%>', 'Vendor');
            } else {
                noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                return false;
            }
        }
        
    function clear() {
        ("#ddlType");
        }
        function CalGrid(chk) {
            //var hdnSelected = document.getElementById(chk.replace('chkSelect', 'hdnSelected'));
            //var hdnOriginal = document.getElementById(chk.replace('chkSelect', 'hdnOriginal'));
            //var hdnPrevDue = document.getElementById(chk.replace('chkSelect', 'hdnPrevDue'));
            //var lblDue = document.getElementById(chk.replace('chkSelect', 'lblBalance'))
            //var txtDisc = document.getElementById(chk.replace('chkSelect', 'txtGvDisc'));
            //var txtPay = document.getElementById(chk.replace('chkSelect', 'txtGvPay'));

            //var pay = $(txtPay).val().toString().replace(/[\$\(\),]/g, '');
            //var disc = $(txtDisc).val().toString().replace(/[\$\(\),]/g, '');
            ////////////////////////
            //var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
            //var prevDue = parseFloat($(hdnOriginal).val() - $(hdnSelected).val())
            //var pay = 0;

            //var rpay = pay.toLocaleString("en-US", { minimumFractionDigits: 2 });
            //var rprevDue = prevDue.toLocaleString("en-US", { minimumFractionDigits: 2 });
            if ($("#" + chk).prop('checked') == true) {

                SelectedRowStyle('<%=RadGrid_RunPayroll.ClientID %>')
            }
            else if ($("#" + chk).prop('checked') == false) {

                $(this).closest('tr').removeAttr("style");
            }
            
        }

        function pageLoad(sender, args) {
            $("[id*=chkSelectAll]").change(function () {
                //debugger;
                var ret = $(this).prop('checked');
                
                $("#<%=RadGrid_RunPayroll.ClientID %>").find('tr:not(:first, :last)').each(function () {
                    var $tr = $(this);
                    var chk = $tr.find('input[id*=chkSelect]');
                    if (ret == true) {
                        $tr.css('background-color', '#c3dcf8');
                        chk.prop('checked', true);
                    }
                    else {
                        chk.prop('checked', false);
                        $tr.removeAttr("style");
                    }

                    var ch_id = chk.attr('id');
                    if (ch_id != undefined) {
                        if (ch_id != "ctl00_ContentPlaceHolder1_RadGrid_RunPayroll_ctl00_ctl02_ctl00_chkSelectAll") {
                            CalGrid(ch_id);
                        }
                    }
                    //if ($tr.find('input[id*=txtGvPay]').attr('id') != "" && typeof $tr.find('input[id*=txtGvPay]').attr('id') != 'undefined') {
                    //    var payment = $tr.find('input[id*=txtGvPay]').val().replace(/[\$\(\),]/g, '');

                    //    if (!isNaN(parseFloat(payment))) {
                    //        tPay += parseFloat(payment);

                    //    }
                    //}


                    //if ($tr.find('input[id*=txtGvDisc]').attr('id') != "" && typeof $tr.find('input[id*=txtGvDisc]').attr('id') != 'undefined') {
                    //    var disc = $tr.find('input[id*=txtGvDisc]').val().replace(/[\$\(\),]/g, '');

                    //    if (!isNaN(parseFloat(disc))) {
                    //        tDisc += parseFloat(disc);
                    //    }
                    //}

                    //if ($tr.find('[id*=lblBalance]').attr('id') != "" && typeof $tr.find('[id*=lblBalance]').attr('id') != 'undefined') {
                    //    var bal = $tr.find('[id*=lblBalance]').text().replace(/[\$\,]/g, '');
                    //    if (bal.includes('(')) {
                    //        bal = bal.replace(/[\$\(\),]/g, '');
                    //        bal = -bal;
                    //    }

                    //    if (!isNaN(parseFloat(bal))) {
                    //        tBal += parseFloat(bal);
                    //    }
                    //}
                    //if ($tr.find('input[id*=txtGvPay]').attr('id') != "" && typeof $tr.find('input[id*=txtGvPay]').attr('id') != 'undefined') {
                    //    var payment = $tr.find('input[id*=txtGvPay]').val().replace(/[\$\(\),]/g, '');

                    //    if (!isNaN(parseFloat(payment))) {
                    //        tPay1 += parseFloat(payment);
                    //    }
                    //}
                    //if ($tr.find('input[id*=txtGvDisc]').attr('id') != "" && typeof $tr.find('input[id*=txtGvDisc]').attr('id') != 'undefined') {
                    //    var disc = $tr.find('input[id*=txtGvDisc]').val().replace(/[\$\(\),]/g, '');

                    //    if (!isNaN(parseFloat(disc))) {
                    //        tDisc1 += parseFloat(disc);
                    //    }
                    //}

                    //if ($tr.find('[id*=lblBalance]').attr('id') != "" && typeof $tr.find('[id*=lblBalance]').attr('id') != 'undefined') {
                        
                    //    var bal = $tr.find('[id*=lblBalance]').text().replace(/[\$\,]/g, '');
                    //    if (bal.includes('(')) {
                    //        bal = bal.replace(/[\$\(\),]/g, '');
                    //        bal = -bal;
                    //    }
                    //    if (!isNaN(parseFloat(bal))) {
                    //        tBal1 += parseFloat(bal);
                    //    }
                    //}
                })
                <%--var _currencyInWord = inWords(parseFloat(Math.trunc(tPay)));
                var d = tPay - Math.trunc(tPay);
                if (d > 0) {
                    d = Math.round(d * 100);
                    _currencyInWord = _currencyInWord + " And " + d + " / 100";
                }
                _currencyInWord = "*** " + _currencyInWord + "****************";
                $("#<%=lblSelectedPayment.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $("#<%=lblRequirement.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $("#<%=lblTotalAmount.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $("#<%=lblTotalAmount11.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $("#<%=lblDollar.ClientID%>").html(_currencyInWord);
                $("#<%=hdnTPay.ClientID%>").val(tPay.toString());
                $('.cls-payment').html(cleanUpCurrency("$" + parseFloat(tPay).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $("#<%=lblTotalDiscount.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tDisc).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $('.cls-disc').html(cleanUpCurrency("$" + parseFloat(tDisc).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                <% GetPaymentTotal();%>
                $('.cls-bal').html(cleanUpCurrency("$" + parseFloat(tBal).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $('.cls-payment').html(cleanUpCurrency("$" + parseFloat(tPay1).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $("#<%=lblTotalDiscount.ClientID%>").html(cleanUpCurrency("$" + parseFloat(tDisc1).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $('.cls-disc').html(cleanUpCurrency("$" + parseFloat(tDisc1).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                $('.cls-bal').html(cleanUpCurrency("$" + parseFloat(tBal1).toLocaleString("en-US", { minimumFractionDigits: 2 })));
                document.getElementById('<%=btnSelectChkBox.ClientID%>').click();--%>
            });
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

            //$(".loadIframePayrollDetail").click(function () {
            //    var hasattr = $('#PayrollDetailiframe').attr('src');
            //    if (!hasattr) {
            //        var absURl = window.location.href.toLowerCase();
            //        $('#PayrollDetailiframe').attr('src', absURl.replace('RunPayroll', 'EmpPayrollDetail'));

            //    }
            //});
           
        });

        function OpenPayrollDetailModal(sEmpID) {
            
            var wnd = $find('<%=PayrollDetail.ClientID %>');
            wnd.set_title("Payroll Detail");

            
            
            
        }
        function ClosePayrollDetailModal() {
            var wnd = $find('<%=PayrollDetail.ClientID %>');
            wnd.Close();

        }
        function OpenBankModal() {
            <%--$('#<%=txtVendorType.ClientID%>').val("");
            $('#<%=txtremarksvendor.ClientID%>').val("");
            $('#<%=txtVendorType.ClientID%>').prop("readonly", false);--%>
            var wnd = $find('<%=RadWindowBank.ClientID %>');
            wnd.set_title("Generate Payroll Checks");
            wnd.Show();
        }
        function CloseBankModal() {
            var wnd = $find('<%=RadWindowBank.ClientID %>');
            wnd.Close();
            //document.getElementById('<%=lnkprintchecktemp.ClientID%>').click();
            

        }
        function OpentemplateModal() {
            
            <%--var d1 = parseInt($('#<%=txtcheckfrom.ClientID%>').val());
            var d2 = parseInt($('#<%=txtcheckto.ClientID%>').val());

            if (d2 < d1) {
                noty({
                    text: 'Ending checkno is not less then starting checkno.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
                return;
            }--%>
            CloseBankModal();
            var wnd = $find('<%=RadWindowTemplates.ClientID %>');
            //wnd.set_title("Re-Print Check Range");
            wnd.Show();
        }
        function ClosetemplateModal() {
            var wnd = $find('<%=RadWindowTemplates.ClientID %>');
            wnd.Close();
            //$('html, body').animate({ scrollTop: $('#vendorType').offset().top }, 'slow');
        }
    </script>
</asp:Content>


