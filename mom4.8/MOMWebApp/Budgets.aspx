<%@ Page Title="" Language="C#" MasterPageFile="~/Mom.master" EnableEventValidation="true" AutoEventWireup="true" Inherits="Budgets" CodeBehind="Budgets.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <link href="Appearance/css/bootstrap-multiselect.css" rel="stylesheet" />
    <script src="Appearance/js/bootstap-multiselect.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=lstAccountTypes]').multiselect({
                includeSelectAllOption: true
            });
        });
        function opencCeateForm() {
            var oWnd = $find("<%=RadCreateWindow.ClientID%>");
            oWnd.show();
        }
        function closeCeateForm() {
            var oWnd = $find("<%=RadCreateWindow.ClientID%>");
            oWnd.close();
        }
        function opencImportForm() {
            var oWnd = $find("<%=RadImportWindow.ClientID%>");
            oWnd.show();
        }
        function Expand(strText) {
            var Hidden1 = document.getElementById('<%=Hidden1.ClientID %>');
            Hidden1.value = strText;
            __doPostBack('<%=RadGrid_Budget.ClientID %>');
        }
        function txtMonthValueChange(txt, itemIndex, lblName, hdnType) {

            var grid = $find("<%=RadGrid_Budget.ClientID %>");
            var hdnrowsedited = document.getElementById("<%=rowNumbersEdited.ClientID%>")
            var rowsEditedValue = hdnrowsedited.value;
            if (rowsEditedValue == "")
                hdnrowsedited.value = itemIndex;
            else
                hdnrowsedited.value += "," + itemIndex;
            var MasterTable = grid.get_masterTableView();
            var row = MasterTable.get_dataItems()[itemIndex];
            var txtMonth = row.findElement(txt);
            var monthValue = txtMonth.value;
            if (txtMonth.value != "")
                txtMonth.value = parseFloat(monthValue.replace(/\,/g, '')).toLocaleString(undefined, { minimumFractionDigits: 2 });
            var txt;
            var monthID = txtMonth.id;
            var monthNumber = monthID.slice(-2).replace('h', '');
            var type = (row.findElement(hdnType));
            var annualTotal = 0.00;
            var monthTotal = 0.00;
            for (var j = 1; j <= 12; j++) {
                txt = row.findElement(lblName.replace("lblname", "txtMonth" + j));
                if (txt.value != "")
                    annualTotal += parseFloat(txt.value.replace(/\,/g, ''));
            }
            var txtAnnual = row.findElement(lblName.replace("lblname", "txtAnnualTotal"));
            txtAnnual.value = (annualTotal).toLocaleString(undefined, { minimumFractionDigits: 2 });

            var month = setMonth(monthNumber, true);
            var hdnType = row.findElement(lblName.replace("lblname", "hdnType"));
            for (var k = 0; k < MasterTable.get_dataItems().length; k++) {

                var rowHdn = MasterTable.get_dataItems()[k].findElement("hdnType");
                var _lblName = MasterTable.get_dataItems()[k].findElement("lblname");
                var text = (_lblName.innerText || _lblName.textContent);
                if (text.indexOf("Total " + type.value) != -1 && rowHdn.value.indexOf(type.value) != -1) {

                    txtColTotal = MasterTable.get_dataItems()[k].findElement("txtMonth" + monthNumber);
                    txtColTotal.value = (monthTotal).toLocaleString(undefined, { minimumFractionDigits: 2 });
                    monthTotal = 0;
                }
                else {

                    if (text.indexOf("Total " + type.value) == -1 && rowHdn.value.indexOf(type.value) != -1) {
                        var txtMonth = MasterTable.get_dataItems()[k].findElement("txtMonth" + monthNumber);
                        var val = (txtMonth.innerText || txtMonth.textContent || txtMonth.value);
                        if (val != "")
                            monthTotal += parseFloat(val.replace(/\,/g, ''));
                    }
                }
            }


            RecalculateTotals(type.value);
        }

        function setMonth(i, IsAbbreviation) {
            var month = "";
            switch (i) {
                case 1:
                    if (IsAbbreviation)
                        month = "Jan";
                    else
                        month = "January";
                    break;
                case 2:
                    if (IsAbbreviation)
                        month = "Feb";
                    else
                        month = "February";
                    break;
                case 3:
                    if (IsAbbreviation)
                        month = "Mar";
                    else
                        month = "March";
                    break;
                case 4:
                    if (IsAbbreviation)
                        month = "Apr";
                    else
                        month = "April";
                    break;
                case 5:
                    month = "May";
                    break;
                case 6:
                    if (IsAbbreviation)
                        month = "Jun";
                    else
                        month = "June";
                    break;
                case 7:
                    if (IsAbbreviation)
                        month = "Jul";
                    else
                        month = "July";
                    break;
                case 8:
                    if (IsAbbreviation)
                        month = "Aug";
                    else
                        month = "August";
                    break;
                case 9:
                    if (IsAbbreviation)
                        month = "Sep";
                    else
                        month = "September";
                    break;
                case 10:
                    if (IsAbbreviation)
                        month = "Oct";
                    else
                        month = "October";
                    break;
                case 11:
                    if (IsAbbreviation)
                        month = "Nov";
                    else
                        month = "November";
                    break;
                case 12:
                    if (IsAbbreviation)
                        month = "Dec";
                    else
                        month = "December";
                    break;
            }

            return month;
        }

        function txtAnnualTotalValueChange(txt, itemIndex, lblName, hdnType) {
            var grid = $find("<%=RadGrid_Budget.ClientID %>");

            var MasterTable = grid.get_masterTableView();
            var row = MasterTable.get_dataItems()[itemIndex];
            var hdnSuppressEvent = row.findElement("annualTotalSuppressEvent");
            var rowEdited = row.findElement("rowEdited");
            rowEdited.value = true;

            var hdnrowsedited = document.getElementById("<%=rowNumbersEdited.ClientID%>")
            var rowsEditedValue = hdnrowsedited.value;
            if (rowsEditedValue == "")
                hdnrowsedited.value = itemIndex;
            else
                hdnrowsedited.value += "," + itemIndex;

            if (hdnSuppressEvent.value == "False") {
                var txtAnnualTotal = row.findElement(txt);//getting label valu
                var totalValue = (txtAnnualTotal.value);
                for (var j = 1; j <= 12; j++) {
                    var id = txt.replace("txtAnnualTotal", "txtMonth" + j);
                    var txt1 = (row.findElement(id));
                    var type = (row.findElement(hdnType));

                    var txtColTotal;
                    var monthTotal = 0;
                    txtAnnualTotal.value = parseFloat(totalValue.replace(/\,/g, '')).toFixed(2).toLocaleString(undefined, { minimumFractionDigits: 2 });

                    if (txtAnnualTotal.value == "NaN")
                        txtAnnualTotal.value = 0.00;
                    if (j < 12)
                        if (!isNaN(parseFloat(((totalValue.replace(/\,/g, '')) / 12)).toFixed(2).toLocaleString(undefined, { minimumFractionDigits: 2 })))
                            txt1.value = parseFloat(((totalValue.replace(/\,/g, '')) / 12)).toFixed(2).toLocaleString(undefined, { minimumFractionDigits: 2 });
                        else
                            txt1.value = 0.00;
                    else if (j == 12) {
                        var divValue = parseFloat(((totalValue.replace(/\,/g, '')) / 12)).toFixed(2);
                        if (!isNaN((totalValue - (divValue * 11)).toFixed(2).toLocaleString(undefined, { minimumFractionDigits: 2 })))
                            txt1.value = parseFloat((totalValue - (divValue * 11))).toFixed(2).toLocaleString(undefined, { minimumFractionDigits: 2 });
                        else
                            txt1.value = 0.00;
                    }

                    for (var k = 0; k < MasterTable.get_dataItems().length; k++) {
                        var rowHdn = MasterTable.get_dataItems()[k].findElement("hdnType");
                        var _lblName = MasterTable.get_dataItems()[k].findElement("lblname");
                        var text = (_lblName.innerText || _lblName.textContent);

                        if (text.indexOf("Total " + type.value) != -1 && rowHdn.value.indexOf(type.value) != -1) {

                            txtColTotal = MasterTable.get_dataItems()[k].findElement("txtMonth" + j);
                            txtColTotal.value = (monthTotal).toLocaleString(undefined, { minimumFractionDigits: 2 });
                            monthTotal = 0;
                        }
                        else {
                            if (text.indexOf("Total " + type.value) == -1 && rowHdn.value.indexOf(type.value) != -1) {
                                var txtMonth = MasterTable.get_dataItems()[k].findElement("txtMonth" + j);
                                var val = (txtMonth.innerText || txtMonth.textContent || txtMonth.value);
                                if (val != "")
                                    monthTotal += parseFloat(val.replace(/\,/g, ''));
                            }
                        }
                    }
                }

                RecalculateTotals(type.value);
            }
        }

        function ClearBlankColumns() {
            var grid = $find("<%=RadGrid_Budget.ClientID %>");
            var MasterTable = grid.get_masterTableView();

            for (var i = 0; i < MasterTable.get_dataItems().length; i++) {
                var cboxSelect = MasterTable.get_dataItems()[i].findElement("cboxSelect");
                var hdnType = MasterTable.get_dataItems()[i].findElement("hdnType");
                var txtColTotal;
                var monthTotal = 0;
                if (cboxSelect != null) {
                    if (cboxSelect.checked == true) {
                        var txtAnnual = MasterTable.get_dataItems()[i].findElement("txtAnnualTotal");
                        if (txtAnnual != null)
                            txtAnnual.value = "";
                        for (var j = 1; j <= 12; j++) {
                            var txt = MasterTable.get_dataItems()[i].findElement("txtMonth" + j);
                            txt.value = "";

                            for (var k = 0; k < MasterTable.get_dataItems().length; k++) {
                                var rowHdn = MasterTable.get_dataItems()[k].findElement("hdnType");
                                var lblName = MasterTable.get_dataItems()[k].findElement("lblname");
                                var text = (lblName.innerText || lblName.textContent);
                                if (text.indexOf("Total " + hdnType.value) != -1 && rowHdn.value.indexOf(hdnType.value) != -1) {
                                    txtColTotal = MasterTable.get_dataItems()[k].findElement("txtMonth" + j);
                                    txtColTotal.value = (monthTotal).toLocaleString(undefined, { minimumFractionDigits: 2 });
                                    monthTotal = 0;
                                }
                                else {
                                    if (text.indexOf("Total " + hdnType.value) == -1 && rowHdn.value.indexOf(hdnType.value) != -1) {
                                        txtMonth = MasterTable.get_dataItems()[k].findElement("txtMonth" + j);
                                        var val = (txtMonth.innerText || txtMonth.textContent || txtMonth.value);
                                        if (val != "")
                                            monthTotal += parseFloat(val.replace(/\,/g, ''));
                                    }
                                }
                            }


                        }
                        RecalculateTotals(hdnType.value);
                        cboxSelect.checked = false;
                    }
                }


            }
            return false;
        }

        function CopyAcross() {
            var grid = $find("<%=RadGrid_Budget.ClientID %>");
            var MasterTable = grid.get_masterTableView();

            var hdnType = null;
            for (var i = 0; i < MasterTable.get_dataItems().length; i++) {
                var cboxSelect = MasterTable.get_dataItems()[i].findElement("cboxSelect");
                var hdnType = MasterTable.get_dataItems()[i].findElement("hdnType");
                var txtColTotal;
                var monthTotal = 0;
                if (cboxSelect != null) {
                    if (cboxSelect.checked == true) {
                        var txtJan = MasterTable.get_dataItems()[i].findElement("txtMonth1");
                        for (var j = 1; j <= 12; j++) {
                            if (j > 1) {
                                var txtM = MasterTable.get_dataItems()[i].findElement("txtMonth" + j);
                                txtM.value = txtJan.value;
                                for (var k = 0; k < MasterTable.get_dataItems().length; k++) {
                                    var rowHdn = MasterTable.get_dataItems()[k].findElement("hdnType");
                                    var lblName = MasterTable.get_dataItems()[k].findElement("lblname");
                                    var text = (lblName.innerText || lblName.textContent);
                                    if (text.indexOf("Total " + hdnType.value) != -1 && rowHdn.value.indexOf(hdnType.value) != -1) {
                                        txtColTotal = MasterTable.get_dataItems()[k].findElement("txtMonth" + j);
                                        txtColTotal.value = (monthTotal).toLocaleString(undefined, { minimumFractionDigits: 2 });
                                        monthTotal = 0;
                                    }
                                    else {
                                        if (text.indexOf("Total " + hdnType.value) == -1 && rowHdn.value.indexOf(hdnType.value) != -1) {
                                            txtMonth = MasterTable.get_dataItems()[k].findElement("txtMonth" + j);
                                            var val = (txtMonth.innerText || txtMonth.textContent || txtMonth.value);
                                            if (val != "")
                                                monthTotal += parseFloat(val.replace(/\,/g, ''));
                                        }
                                    }
                                }
                            }


                        }
                        var txtAnnualTotal = MasterTable.get_dataItems()[i].findElement("txtAnnualTotal");
                        txtAnnualTotal.value = ((parseFloat(txtJan.value.replace(/\,/g, '')) * 12)).toLocaleString(undefined, { minimumFractionDigits: 2 });
                    }
                }
                RecalculateTotals(hdnType.value);
            }

            return false;
        }

        function cmdIncrease() {
            var grid = $find("<%=RadGrid_Budget.ClientID %>");
            var MasterTable = grid.get_masterTableView();
            var tableEl = MasterTable.get_element();
            var hdnType = null;
            for (var i = 0; i < MasterTable.get_dataItems().length; i++) {
                var cboxSelect = MasterTable.get_dataItems()[i].findElement("cboxSelect");
                var hdnType = MasterTable.get_dataItems()[i].findElement("hdnType");
                var txtColTotal;
                var monthTotal = 0;
                if (cboxSelect != null) {
                    if (cboxSelect.checked == true) {
                        var percentage = document.getElementsByClassName("percentage")[0];

                        for (var j = 1; j <= 12; j++) {
                            var txt = MasterTable.get_dataItems()[i].findElement("txtMonth" + j);
                            if (txt.value != "") {
                                var currentValue = parseFloat(txt.value.replace(/\,/g, ''));
                                txt.value = (currentValue + ((currentValue * percentage.value) / 100)).toLocaleString(undefined, { minimumFractionDigits: 2 });
                            }
                            for (var k = 0; k < MasterTable.get_dataItems().length; k++) {
                                var rowHdn = MasterTable.get_dataItems()[k].findElement("hdnType");
                                var lblName = MasterTable.get_dataItems()[k].findElement("lblname");
                                var text = (lblName.innerText || lblName.textContent);
                                if (text.indexOf("Total " + hdnType.value) != -1 && rowHdn.value.indexOf(hdnType.value) != -1) {
                                    txtColTotal = MasterTable.get_dataItems()[k].findElement("txtMonth" + j);
                                    txtColTotal.value = (monthTotal).toLocaleString(undefined, { minimumFractionDigits: 2 });
                                    monthTotal = 0;
                                }
                                else {
                                    if (text.indexOf("Total " + hdnType.value) == -1 && rowHdn.value.indexOf(hdnType.value) != -1) {
                                        txtMonth = MasterTable.get_dataItems()[k].findElement("txtMonth" + j);
                                        var val = (txtMonth.innerText || txtMonth.textContent || txtMonth.value);
                                        if (val != "")
                                            monthTotal += parseFloat(val.replace(/\,/g, ''));
                                    }
                                }
                            }
                        }
                        var txtAnnualTotal = MasterTable.get_dataItems()[i].findElement("txtAnnualTotal");
                        if (txtAnnualTotal.value != "") {
                            var currentAnnualValue = parseFloat(txtAnnualTotal.value.replace(/\,/g, ''));
                            txtAnnualTotal.value = (currentAnnualValue + ((currentAnnualValue * percentage.value) / 100)).toLocaleString(undefined, { minimumFractionDigits: 2 });

                            RecalculateTotals(hdnType.value);
                        }
                    }

                }
            }
        }

        function RecalculateTotals(type) {
            debugger;
            var annualTotal = 0.00;
            var revenuesTotal = new Array();
            var costOfSalesTotal = new Array();
            var expensesTotal = new Array();
            var grid = $find("<%=RadGrid_Budget.ClientID %>");
            var MasterTable = grid.get_masterTableView();
            for (var k = 0; k < MasterTable.get_dataItems().length; k++) {
                var rowHdn = MasterTable.get_dataItems()[k].findElement("hdnType");
                var _lblName = MasterTable.get_dataItems()[k].findElement("lblname");
                var text = (_lblName.innerText || _lblName.textContent);

                if (text.indexOf("Total " + type) == -1 && rowHdn.value.indexOf(type) != -1) {

                    var txt = MasterTable.get_dataItems()[k].findElement("txtAnnualTotal");
                    if (txt.value != "")
                        annualTotal += parseFloat(txt.value.replace(/\,/g, ''));
                }
                if (text.indexOf("Total " + type) != -1 && rowHdn.value.indexOf(type) != -1) {
                    var txtAnnual = MasterTable.get_dataItems()[k].findElement("txtAnnualTotal");
                    txtAnnual.value = (annualTotal).toFixed(2).toLocaleString(undefined, { minimumFractionDigits: 2 });
                }
                if (text == "Total Revenues") {
                    var txtAnnual = MasterTable.get_dataItems()[k].findElement("txtAnnualTotal");
                    revenuesTotal.push(txtAnnual.value.replace(/\,/g, ''));
                    for (var j = 1; j <= 12; j++) {
                        var txtMonth = MasterTable.get_dataItems()[k].findElement("txtMonth" + j);
                        if (txtMonth.value != "")
                            revenuesTotal.push(txtMonth.value.replace(/\,/g, ''));
                    }
                }
                if (text == "Total Cost of Sales") {
                    var txtAnnual = MasterTable.get_dataItems()[k].findElement("txtAnnualTotal");
                    costOfSalesTotal.push(txtAnnual.value.replace(/\,/g, ''));
                    for (var j = 1; j <= 12; j++) {
                        var txtMonth = MasterTable.get_dataItems()[k].findElement("txtMonth" + j);
                        if (txtMonth.value != "")
                            costOfSalesTotal.push(txtMonth.value.replace(/\,/g, ''));
                    }
                }
                if (text == "Total Expenses") {
                    var txtAnnual = MasterTable.get_dataItems()[k].findElement("txtAnnualTotal");
                    expensesTotal.push(txtAnnual.value.replace(/\,/g, ''));
                    for (var j = 1; j <= 12; j++) {
                        var txtMonth = MasterTable.get_dataItems()[k].findElement("txtMonth" + j);
                        if (txtMonth.value != "")
                            expensesTotal.push(txtMonth.value.replace(/\,/g, ''));
                    }
                }
                if (text == "Net Profit Total") {
                    var netProfitTotals = ((parseFloat(revenuesTotal[0].replace(/\,/g, ''))) - (parseFloat(costOfSalesTotal[0].replace(/\,/g, ''))) - (parseFloat(expensesTotal[0].replace(/\,/g, '')))).toFixed(2).toLocaleString(undefined, { minimumFractionDigits: 2 });

                    var txtAnnual = MasterTable.get_dataItems()[k].findElement("txtAnnualTotal");
                    txtAnnual.value = netProfitTotals;
                    for (var j = 1; j <= 12; j++) {
                        var txtMonth = MasterTable.get_dataItems()[k].findElement("txtMonth" + j);
                        txtMonth.value = ((parseFloat(revenuesTotal[j].replace(/\,/g, '')) - parseFloat(costOfSalesTotal[j].replace(/\,/g, '')) - parseFloat(expensesTotal[j].replace(/\,/g, '')))).toFixed(2).toLocaleString(undefined, { minimumFractionDigits: 2 });
                    }
                }
            }
        }

        $(window).scroll(function () {
            if ($(window).scrollTop() >= 0) {
                $('#divButtons').addClass('fixed-header');
            }
            if ($(window).scrollTop() <= 0) {
                $('#divButtons').removeClass('fixed-header');
            }
        });
    </script>
    
    <script>
        function OpenConfirmDialog() {
            var result = "false";
            if (confirm('Are you sure you want to delete this Budget?')) {
                //True .. do something
                result = "true";
                return true;
            }
            else {
                //False .. do something
                result = "false";
                return false;
            }
        }
    </script>

    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to save data?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

        function SaveBudget() {
            var year = document.getElementById('<%=txtYearLoad.ClientID %>');
            if (year.value == "") {
                alert("Please specify a year to save the budget");
                year.focus();
                return false;
            }

            return true;
        }

        function CreateFormShow(sender, args) {
            var txtYear = $find('<%=txtYear.ClientID %>');
            var lblSavedBudgets = $('#<%=lblSavedBudgets.ClientID %>');
            var drpBudgetsList = $('#<%=drpBudgetsList.ClientID %>');
            var drpBudgetType = $('#<%=drpBudgetType.ClientID %>');

            drpBudgetType.val("");
            txtYear.clear();
            lblSavedBudgets.toggle();
            drpBudgetsList.toggle();
        }

        function CreateBudgetValidation() {
            var txtYear = $('#<%=txtYear.ClientID %>');
            var drpBudgetType = $('#<%=drpBudgetType.ClientID %>');
            var drpBudgetsList = $('#<%=drpBudgetsList.ClientID %>');

            if (drpBudgetType.val() == "") {
                alert("Please select budget type.");
                drpBudgetType.focus();
                return false;
            }

            if (txtYear.val() == "") {
                alert("Please specify a year to create a new budget.");
                txtYear.focus();
                return false;
            }

            if (drpBudgetType.val() == "Saved Budgets" && drpBudgetsList.val() == "0") {
                alert("Please specify a budget to create a new one.");
                drpBudgetsList.focus();
                return false;
            }

            return true;
        }
    </script>

    <style type="text/css">
        .radiolabel {
            font-size: 12px;
        }

        .styleAnnualTotal {
            background: none;
            border: none;
        }

        .lstAccountTypes {
            font-family: "Open Sans", sans-serif;
            font-size: 12px;
        }

        .table-scrollable .rgDataDiv {
            height: auto !important;
        }

        .RadInput .riSelect {
            position: absolute !important;
            right: auto !important;
            top: 1px !important;
            bottom: 1px !important;
        }

        .RadGrid_Bootstrap .rgInput,
        .RadGrid_Bootstrap .rgEditRow > td > [type="text"],
        .RadGrid_Bootstrap .rgEditForm td > [type="text"],
        .RadGrid_Bootstrap .rgBatchContainer > [type="text"],
        .RadGrid_Bootstrap .rgFilterBox {
            width: 100% !important;
        }

        .RadInput, .RadInputMgr {
            vertical-align: middle !important;
            width: 100px !important;
            line-height: 1.42857143 !important;
            box-sizing: border-box !important;
        }

            .RadInput .riTextBox, .RadInputMgr {
                padding: 5px 10px !important;
            }

        .RadComboBox_Metro .rcbInner {
            padding-top: 4px !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadAjaxManager ID="RadAjaxManager_Budget" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid_Budget">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Budget"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="txtYearLoad" EventName="TextChanged">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpBudgets" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="txtYear" EventName="TextChanged">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpBudgetsList" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="txtYear" EventName="TextChanged">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblSavedBudgets" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="drpBudgets" EventName="SelectedIndexChanged">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Budget" LoadingPanelID="RadAjaxLoadingPanel_Budget" />
                    <telerik:AjaxUpdatedControl ControlID="lblMessage" />
                    <telerik:AjaxUpdatedControl ControlID="budgetSavePanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkFilter" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Budget" LoadingPanelID="RadAjaxLoadingPanel_Budget" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkSaveBudget" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Budget" LoadingPanelID="RadAjaxLoadingPanel_Budget" />
                    <telerik:AjaxUpdatedControl ControlID="lblMessage" />
                    <telerik:AjaxUpdatedControl ControlID="drpBudgets" />
                    <telerik:AjaxUpdatedControl ControlID="budgetSavePanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkDeleteBudget" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpBudgets" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkRefresh" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpBudgets" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkDeleteBudget" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblMessage" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkRefresh" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblMessage" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkDeleteBudget" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="budgetSavePanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkRefresh" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="budgetSavePanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkDeleteBudget" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Budget" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="lnkRefresh" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_Budget" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmdDeleteBudget" EventName="Click">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpBudgets" LoadingPanelID="RadAjaxLoadingPanel_Budget" />
                    <telerik:AjaxUpdatedControl ControlID="lblMessage" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="drpBudgetType" EventName="SelectedIndexChanged">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="drpBudgetsList" LoadingPanelID="RadAjaxLoadingPanel_Budget" />
                    <telerik:AjaxUpdatedControl ControlID="lblMessage" />
                    <telerik:AjaxUpdatedControl ControlID="lblSavedBudgets" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="drpBudgetType">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lblSavedBudgets" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_Budget" Skin="BlackMetroTouch" runat="server" IsSticky="true">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Transparency="70">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadCreateWindow" Skin="Material" runat="server" Modal="true" Width="550" Height="450" OnClientBeforeShow="CreateFormShow">
                <ContentTemplate>
                    <div class="budget-customize">
                        <div>
                            <p style="font-size: medium; font-weight: bold; text-align: center; margin-top: 10px;">Create New Budget</p>
                            <br />
                            <div style="margin-top: 10px; padding-left: 30px;">
                                <b style="float: left; width: 30%; margin-top: 20px;">Load Data from : </b>
                                <div style="float: left; width: 70%;">
                                    <asp:DropDownList ID="drpBudgetType" runat="server" Width="250px" CssClass="browser-default" OnSelectedIndexChanged="drpBudgetType_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="-- Select Budget Type --" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Actuals" Value="Actuals"></asp:ListItem>
                                        <asp:ListItem Text="Saved Budgets" Value="Saved Budgets"></asp:ListItem>
                                        <asp:ListItem Text="Blank Sheet" Value="Blank Sheet"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div style="margin-top: 15px; padding-left: 30px;">
                                <b style="float: left; width: 30%; margin-top: 20px;">Select Year : &ensp;</b>
                                <div style="float: left; width: 30%; margin-top: 15px;">
                                    <telerik:RadNumericTextBox RenderMode="Auto" ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true" CssClass="browser-default input-sm input-small"
                                        NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" runat="server" ID="txtYear" Width="270px" MinValue="2015" MaxValue="2050"
                                        OnTextChanged="txtYear_TextChanged" AutoPostBack="true" Height="35px">
                                    </telerik:RadNumericTextBox>
                                </div>
                                <div style="clear: both;"></div>
                            </div>
                            <div style="padding-left: 30px;">
                                <b style="float: left; width: 30%; margin-top: 20px;">
                                    <asp:Label runat="server" ID="lblSavedBudgets" Text="Saved Budgets :" Visible="false" /></b>
                                <div style="float: left; width: 70%;">
                                    <div style="float: left; width: 100%;">
                                        <asp:DropDownList ID="drpBudgetsList" Width="250px" Visible="false" runat="server" CssClass="browser-default input-sm input-small"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                            <div class="btnlinks" style="text-align: center; font-size: small; font-weight: bold; margin-top: 15px; width: 100%;">
                                <asp:LinkButton ID="lnkSearch" CommandName="GenerateBudget" OnClick="lnkSearch_Click" runat="server" OnClientClick="return CreateBudgetValidation();">Load</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <telerik:RadWindowManager ID="RadWindowManager2" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadImportWindow" Skin="Material" runat="server" Modal="true" Width="500" Height="350">
                <ContentTemplate>
                    <div class="budget-customize">
                        <div>
                            <p style="font-size: medium; font-weight: bold; text-align: center; margin-top: 10px;">Import budget from Excel</p>
                            <div style="margin-top: 30px; padding-left: 30px;">
                                <b style="float: left; width: 30%;">Upload Excel file: </b>
                                <div style="float: left; width: 70%;">
                                    <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="hidden" />
                                </div>
                            </div>
                            <div class="btnlinks" style="width: 100%; text-align: center; margin-top: 80px">
                                <asp:LinkButton ID="lnkImportFile" runat="server" OnClick="lnkImport_Click">Load</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <div class="divbutton-container">
        <div id="divButtons">
            <div id="breadcrumbs-wrapper">
                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-action-description"></i>&nbsp;<asp:Label ID="lblHeader" runat="server">Budgets</asp:Label></div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="btnCreateBudget" runat="server" Text="Create New Budget" OnClientClick="opencCeateForm();return false" />

                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                        </div>
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkImport" runat="server" OnClientClick="opencImportForm();return false">Import from Excel</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false" OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
    <div class="container accordian-wrap">
        <div class="row">
            <div class="srchpane">
                <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                        Year
                    </div>
                    <div class="srchinputwrap">
                        <telerik:RadNumericTextBox RenderMode="Auto" EnabledStyle-Width="80px" runat="server" ShowSpinButtons="true" IncrementSettings-InterceptArrowKeys="true"
                            NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator="" ID="txtYearLoad" Width="150" MinValue="2000" MaxValue="2050" Skin="Material"
                            OnTextChanged="txtYearLoad_TextChanged" AutoPostBack="true">
                        </telerik:RadNumericTextBox>
                        <asp:RequiredFieldValidator ID="rfvYear1" ValidationGroup="Save" runat="server" ControlToValidate="txtYearLoad" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="srchinputwrap">
                        <telerik:RadComboBox ID="drpBudgets" Skin="Metro" Visible="false" Filter="Contains" EmptyMessage="--Select budget--" AutoPostBack="true" runat="server" OnSelectedIndexChanged="drpBudgets_SelectedIndexChanged" Width="200px"
                            class="browser-default">
                        </telerik:RadComboBox>
                    </div>
                    <asp:Panel class="sc-form" ID="budgetSavePanel" runat="server" Visible="false">
                        <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                            Budget Name
                        </div>
                        <div class="srchinputwrap">
                            <asp:TextBox ID="txtBudgetName" runat="server" CssClass="srchcstm"></asp:TextBox>
                        </div>
                        <div class="srchinputwrap">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkSaveBudget" OnClick="lnkSave_Click" runat="server" Text="Save" ValidationGroup="Save" CausesValidation="true" OnClientClick="return SaveBudget();"/>
                            </div>
                        </div>
                        <div class="srchinputwrap">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkDeleteBudget" OnClientClick="return OpenConfirmDialog();" OnClick="LnkDelete_Click" runat="server" Text="Delete" />
                            </div>
                        </div>
                        <div class="srchinputwrap">
                            <div class="btnlinks">
                                <asp:LinkButton ID="lnkRefresh" OnClick="lnkRefresh_Click" runat="server" Text="Recalculate Totals" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>

                <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                        Account
                    </div>
                    <div class="srchinputwrap">
                        <telerik:RadComboBox RenderMode="Auto" Skin="Metro" ID="rcAccountType" runat="server" Filter="StartsWith" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" OnSelectedIndexChanged="rcAccountType_SelectedIndexChanged"
                            EmptyMessage="--Select Type--" CssClass="browser-default">
                            <Items>
                                <telerik:RadComboBoxItem Text="Revenues" Value="Revenues" />
                                <telerik:RadComboBoxItem Text="Cost of Sales" Value="Cost of Sales" />
                                <telerik:RadComboBoxItem Text="Expenses" Value="Expenses" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>

                    <div class="srchinputwrap btnlinks srchclr " style="margin-top: 10px!important;">
                        <asp:LinkButton ID="lnkFilter" runat="server" Text="Filter" CommandName="ShowData" OnClick="lnkFilter_Click" />
                    </div>
                    <div class="srchinputwrap" style="margin-top: 10px!important;">
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
                    </div>

                    <div class="col lblsz2 lblszfloat">
                    <div class="row">                                               
                        <span class="tro trost">
                            <asp:CheckBox ID="chkInclInactive" runat="server" OnCheckedChanged="chkInclInactive_Click" AutoPostBack="True" CssClass="css-checkbox" Text="Incl. Inactive GL Codes"></asp:CheckBox>
                        </span>
                    </div>
                </div>       
                </div>                   
            </div>

            <div class="grid_container">
                <div class="form-section-row" style="margin-bottom: 0 !important;">
                    <div class="RadGrid RadGrid_Material">
                        <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                            <script type="text/javascript">
                                var requestInitiator = null;
                                var selectionStart = null;

                                function requestStart(sender, args) {
                                    requestInitiator = document.activeElement.id;
                                    if (document.activeElement != null) {
                                        if (document.activeElement.tagName == "INPUT") {
                                            selectionStart = document.activeElement.selectionStart;
                                        }
                                    }
                                    if (args.get_eventTarget().indexOf("cmdExportToExcel") >= 0) {
                                        args.set_enableAjax(false);
                                    }
                                }

                                function responseEnd(sender, args) {
                                    var element = document.getElementById(requestInitiator);
                                    if (element && element.tagName == "INPUT") {
                                        element.focus();
                                        element.selectionStart = selectionStart;
                                    }
                                }

                                function Checked(btn) {
                                    var grid = $find("<%=RadGrid_Budget.ClientID %>");
                                    var masterTable = grid.get_masterTableView();
                                    var btnValue = btn.value;
                                    var allItems = masterTable.get_dataItems();
                                    for (var i = 0; i < allItems.length; i++) {
                                        var gridItemElement = allItems[i].findElement("cboxSelect");
                                        if (gridItemElement != null) {
                                            if (btnValue == "Check") {
                                                gridItemElement.checked = true;
                                                btn.value = "UnCheck";
                                            }
                                            else {
                                                gridItemElement.checked = false;
                                                btn.value = "Check";
                                            }
                                        }
                                    }
                                }
                            </script>
                        </telerik:RadCodeBlock>
                        <asp:HiddenField ID="rowNumbersEdited" runat="server" Value="" />
                        <telerik:RadAjaxPanel ID="RadAjaxPanel_Budget" runat="server" LoadingPanelID="RadAjaxLoadingPanel_Budget" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd">
                            <telerik:RadGrid RenderMode="Auto" ID="RadGrid_Budget" AllowFilteringByColumn="true" ShowFooter="False" PageSize="100" OnPageIndexChanged="RadGrid_Budget_PageIndexChanged"
                                ShowStatusBar="true" runat="server" AllowPaging="False" AllowSorting="true" FilterType="CheckList" OnItemDataBound="RadGrid_Budget_ItemDataBound" OnDataBinding="RadGrid_Budget_DataBinding"
                                OnNeedDataSource="RadGrid_Budget_NeedDataSource" OnItemCreated="RadGrid_Budget_ItemCreated" OnItemCommand="RadGrid_Budget_ItemCommand" OnExcelExportCellFormatting="RadGrid_Budget_ExcelExportCellFormatting"
                                OnFilterCheckListItemsRequested="RadGrid_Budget_FilterCheckListItemsRequested" OnPreRender="RadGrid_Budget_PreRender" OnDataBound="RadGrid_Budget_DataBound"
                                OnInfrastructureExporting="RadGrid_Budget_InfrastructureExporting" OnDetailTableDataBind="RadGrid_Budget_DetailTableDataBind" OnItemInserted="RadGrid_Budget_ItemInserted"
                                OnInsertCommand="RadGrid_Budget_InsertCommand">
                                <CommandItemStyle />
                                <HeaderStyle Font-Bold="true" Font-Size="Small" />
                                <GroupingSettings CaseSensitive="false" />
                                <ItemStyle BackColor="WhiteSmoke" />
                                <AlternatingItemStyle BackColor="White" />
                                <GroupHeaderItemStyle Font-Size="Medium" Font-Bold="true" />
                                <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowGroupExpandCollapse="true">
                                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                    <Resizing ResizeGridOnColumnResize="True" AllowColumnResize="True"></Resizing>
                                </ClientSettings>
                                <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true" Excel-Format="Xlsx" OpenInNewWindow="true" FileName="Budget_Download"></ExportSettings>

                                <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="true" ShowFooter="True" AllowPaging="false" CommandItemDisplay="Top" EnableHierarchyExpandAll="false">
                                    <GroupByExpressions>
                                        <telerik:GridGroupByExpression>
                                            <GroupByFields>
                                                <telerik:GridGroupByField FieldName="Type" />
                                            </GroupByFields>
                                            <SelectFields>
                                                <telerik:GridGroupByField FieldName="TypeName" />
                                            </SelectFields>
                                        </telerik:GridGroupByExpression>
                                    </GroupByExpressions>
                                    <CommandItemTemplate>
                                        <div class="srchpane" style="margin-bottom: -5px !important; padding-top: 20px !important;">
                                            <div class="srchpaneinner" style="margin-bottom: 0px !important;">
                                                <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                                                    Multiplier
                                                </div>
                                                <div class="srchinputwrap">
                                                    <asp:TextBox ID="txtPercentage" CssClass="srchcstm percentage" Width="50px" runat="server"></asp:TextBox>
                                                    %
                                                </div>
                                                <div class="srchinputwrap btnlinks srchclr">
                                                    <asp:LinkButton ID="cmdIncrese" runat="server" Text="Apply" OnClientClick="cmdIncrease(); return false;" />
                                                    <asp:LinkButton ID="cmdClear" runat="server" Text="Clear" OnClientClick="ClearBlankColumns(); " />
                                                    <asp:LinkButton ID="cmdCopyAcross" runat="server" Text="Copy Across" OnClientClick="CopyAcross(); return false;" />
                                                </div>
                                            </div>

                                            <div class="col lblsz2 lblszfloat">
                                                <div class="row">
                                                    <div runat="server" id="budgetHeader" visible="false">
                                                        <span class="tro trost">
                                                            <asp:Label ID="lblBudgetName" runat="server"></asp:Label>
                                                        </span>
                                                        <span class="tro trost">
                                                            <asp:Label ID="lblBudgetYear" runat="server"></asp:Label>
                                                        </span>
                                                        <span class="tro trost">
                                                            <asp:Label ID="lblTotalAccounts" runat="server"></asp:Label>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        </div>
                                    </CommandItemTemplate>
                                    <Columns>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" ItemStyle-Width="40px" HeaderStyle-Width="40px" Exportable="false">
                                            <HeaderTemplate>
                                                <input id="Button3" type="checkbox" value="Check" onclick="Checked(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cboxSelect" runat="server" CommandName="ClearRow" />
                                                <asp:HiddenField ID="hdnCheck" runat="server" />
                                                <input type="hidden" id="hdnActType" value='<%# Eval("TypeName") %>' />
                                                <input type="hidden" id="annualTotalSuppressEvent" value="False" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="Type" HeaderText="Type" ReadOnly="true" UniqueName="Type" Visible="false" Exportable="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Acct" HeaderText="Acct" ReadOnly="true" Visible="false" UniqueName="Acct" Exportable="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="TypeName" HeaderText="TypeName" ReadOnly="true" Visible="false" UniqueName="TypeName" Exportable="false"></telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fDesc" HeaderText="Description" Visible="false" UniqueName="fDesc" Exportable="false"></telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn HeaderText="Account Number" DataField="AcctNumber" UniqueName="AcctNumber" Visible="True" AllowFiltering="true" ShowFilterIcon="false"
                                            HeaderStyle-Width="100px" ItemStyle-Width="100px" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Width="100px" Text='<%# Eval("AcctNumber") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Name" DataField="AcctName" UniqueName="AcctName" Visible="True" AllowFiltering="true" ShowFilterIcon="false"
                                            HeaderStyle-Width="150px" ItemStyle-Width="150px" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                            <ItemTemplate>
                                                <asp:Label ID="lblname" runat="server" Text='<%# Eval("AcctName") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnType" runat="server" Value='<%# Eval("TypeName") %>' />
                                                <asp:HiddenField ID="hdnStatus" runat="server" Value='<%# Eval("Status") %>' />
                                                <asp:HiddenField ID="rowEdited" runat="server" Value="False" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn HeaderStyle-Width="100px" ItemStyle-Width="100px" DataField="Status" HeaderText="Status" ReadOnly="true" UniqueName="Status" Visible="true" Exportable="false" 
                                            AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false"></telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn HeaderText="Total" DataField="AnnualTotal" UniqueName="AnnualTotal" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAnnualTotal" OnTextChanged="txtAnnualTotal_TextChanged1" Width="100px" onchange="txtAnnualTotalValueChange(this)" runat="server" Text='<%# Eval("AnnualTotal") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                                <asp:HiddenField ID="hdnJan" runat="server" Value='<%# Eval("Jan") %>' />
                                                <asp:HiddenField ID="hdnFeb" runat="server" Value='<%# Eval("Feb") %>' />
                                                <asp:HiddenField ID="hdnMar" runat="server" Value='<%# Eval("Mar") %>' />
                                                <asp:HiddenField ID="hdnApr" runat="server" Value='<%# Eval("Apr") %>' />
                                                <asp:HiddenField ID="hdnMay" runat="server" Value='<%# Eval("May") %>' />
                                                <asp:HiddenField ID="hdnJun" runat="server" Value='<%# Eval("Jun") %>' />
                                                <asp:HiddenField ID="hdnJul" runat="server" Value='<%# Eval("Jul") %>' />
                                                <asp:HiddenField ID="hdnAug" runat="server" Value='<%# Eval("Aug") %>' />
                                                <asp:HiddenField ID="hdnSep" runat="server" Value='<%# Eval("Sep") %>' />
                                                <asp:HiddenField ID="hdnOct" runat="server" Value='<%# Eval("Oct") %>' />
                                                <asp:HiddenField ID="hdnNov" runat="server" Value='<%# Eval("Nov") %>' />
                                                <asp:HiddenField ID="hdnDec" runat="server" Value='<%# Eval("Dec") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Jan" DataField="Jan" UniqueName="Jan" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMonth1" Width="100px" runat="server" Text='<%# Eval("Jan") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Feb" DataField="Feb" UniqueName="Feb" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMonth2" Width="100px" runat="server" Text='<%# Eval("Feb") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Mar" DataField="Mar" UniqueName="Mar" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMonth3" Width="100px" runat="server" Text='<%# Eval("Mar") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Apr" DataField="Apr" UniqueName="Apr" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMonth4" Width="100px" runat="server" Text='<%# Eval("Apr") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="May" DataField="May" UniqueName="May" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMonth5" Width="100px" runat="server" Text='<%# Eval("May") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Jun" DataField="Jun" UniqueName="Jun" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMonth6" Width="100px" runat="server" Text='<%# Eval("Jun") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Jul" DataField="Jul" UniqueName="Jul" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMonth7" Width="100px" runat="server" Text='<%# Eval("Jul") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Aug" DataField="Aug" UniqueName="Aug" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMonth8" Width="100px" runat="server" Text='<%# Eval("Aug") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Sep" DataField="Sep" UniqueName="Sep" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMonth9" Width="100px" runat="server" Text='<%# Eval("Sep") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Oct" DataField="Oct" UniqueName="Oct" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMonth10" Width="100px" runat="server" Text='<%# Eval("Oct") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Nov" DataField="Nov" UniqueName="Nov" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMonth11" Width="100px" runat="server" Text='<%# Eval("Nov") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Dec" DataField="Dec" UniqueName="Dec" Visible="True" AllowFiltering="false" ShowFilterIcon="false">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtMonth12" Width="100px" runat="server" Text='<%# Eval("Dec") %>' BorderWidth="0px" CssClass="styleAnnualTotal"></asp:TextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <FilterMenu CssClass="RadFilterMenu_CheckList">
                                </FilterMenu>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                        <input id="Hidden1" name="Hidden1" runat="server" type="hidden" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="oldDesign" style="display: none">
        <div class="page-cont-top">
            <div class="page-bar-right">
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <asp:Panel runat="server" ID="pnlGridButtons">
                        <ul class="lnklist-header">
                            <li>Budgets</li>
                            <li>
                        </ul>
                    </asp:Panel>
                </div>
            </div>
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div>
                        <div class="title_bar">
                            <div id="divSpace" class="Close_button">
                            </div>
                        </div>

                        <div class="search-customer">
                            <div style="width: 75%; float: left; text-align: left;">
                                <div class="sc-form" style="float: left; margin-right: 20px; font-size: 13px; font-weight: bold; width: 100%;">

                                    <div class="sc-form" style="margin-right: 20px; font-size: 13px; font-weight: bold; float: left;">
                                        &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; Select Year : &nbsp; 
                                    </div>
                                    <div style="float: left; padding-right: 30px;">
                                        <asp:Label ID="lblSelectBudget" runat="server" Visible="false" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div style="width: 25%; float: right; text-align: right;">
                                <div style="float: right; margin-top: 0px; text-align: right;">
                                    <b>Account Type :</b>
                                </div>
                            </div>
                        </div>
                        <div class="search-customer">
                        </div>
                        <div class="search-customer">
                        </div>

                        <div class="clearfix">
                        </div>
                        <div class="table-scrollable" style="padding-top: 15px; border: none">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
</asp:Content>
