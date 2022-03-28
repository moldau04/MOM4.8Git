<%@ Page Title="Recurring Tickets || MOM" Language="C#" MasterPageFile="~/Mom.master" AutoEventWireup="true" Inherits="RecurringTickets" Codebehind="RecurringTickets.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

 
<script runat="server">




    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {

    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--Grid Control-->
    <link href="Design/css/grid.css" rel="stylesheet" />
    <!--File Upload Control-->
    <link href="Design/js/plugins/dropify/css/dropify.css" type="text/css" rel="stylesheet" media="screen,projection">

    <link href="Design/css/pikaday.css" rel="stylesheet" />


    <style>

#overlay {
  background: #ffffff;
  color: #666666;
  position: fixed;
  height: 100%;
  width: 100%;
  z-index: 5000;
  top: 0;
  left: 0;
  float: left;
  text-align: center;
  padding-top: 25%;
  opacity: .80;
}

    </style>

    <script type="text/javascript">

        function AddTicketClick(hyperlink) {
            if (validateForm()) {
                var id = document.getElementById('<%= hdnAddeTicket.ClientID%>').value;
                if (id == "Y") { 

 

 if(ValidateOptionsPanel())
{ 
   document.getElementById("overlay").style.display="block";
 return true;
}
else{ return false;}



 } else {
                    noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false, dismissQueue: true });
                    return false;
                }
            }
            else return false;

        }
        // For clear ViewState["RecTicketSrch"] 
        function ReloadPage() {
            document.getElementById('<%=btnclear2.ClientID%>').click();
        }

        function CheckDelete() {
            // var result = false;
            var checkNumberRow = 0;
            var valueRowChecked = "";
            valueRowChecked = $(".gvOpenCalls input[type='checkbox']:checked").closest("tr").find(".tdContract a").eq(0).text();
            var checkCheckboxChecked = $(".gvOpenCalls input[type='checkbox']:checked").length;
         
            if (checkCheckboxChecked === 1) {
                checkNumberRow = 1;
            }
            else if (checkCheckboxChecked > 1) {
                checkNumberRow = 2;
            }


            if (checkNumberRow === 1) {
                return confirm('Are you sure you want to remove contract [' + valueRowChecked + '] from this period?');
            }
            else if (checkNumberRow === 2) {
                return confirm('Are you sure you want to remove selected contracts from this period?');
            }
            else {
                alert('Please select Ticket.')
                return false;
            }
        }

        $(document).ready(function () {

              $('#<%=chkisAllTicketsUnassigned.ClientID%>').change(function() {
                 
                  document.getElementById('<%=lnkSearch.ClientID%>').click();
            });

             $('#<%=chkPerEquip.ClientID%>').change(function() {
                         document.getElementById('<%=lnkSearch.ClientID%>').click();
            });

             $('#<%=chkDemand.ClientID%>').change(function() {
                          document.getElementById('<%=lnkSearch.ClientID%>').click();
              });

             $('#<%=chkContrRemarks.ClientID%>').change(function() {
                         document.getElementById('<%=lnkSearch.ClientID%>').click();
            });
             $('#<%=chkIsAllTicketsOnHold.ClientID%>').change(function() {
                          document.getElementById('<%=lnkSearch.ClientID%>').click();
              });
  

            $("#<%= txtStartDt.ClientID %>").keypress(function (event) {
                if (event.keyCode == 13) {
                    document.getElementById('<%=lnkSearch.ClientID%>').click();
                }
            });

            $("#<%= txtEndDate.ClientID %>").keypress(function (event) {
                if (event.keyCode == 13) {
                    document.getElementById('<%=lnkSearch.ClientID%>').click();
                }
            });

            ///////////// Ajax call for customer auto search ////////////////////                
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = '';//document.getElementById('ctl00_ContentPlaceHolder1_hdnCon').value;
                this.custID = null;
            }
            $("#ctl00_ContentPlaceHolder1_txtCustomer").autocomplete({
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
                            try { 
                              $("#ctl00_ContentPlaceHolder1_txtCustomer").val('');
                              $("#ctl00_ContentPlaceHolder1_hdnPatientId").val(''); 
                              $("#ctl00_ContentPlaceHolder1_txtLocation").val('');
                              $("#ctl00_ContentPlaceHolder1_hdnLocId").val('');
                                //alert("Due to unexpected errors we were unable to load customers");
                            } catch{ }
                        } 
                    });
                },
                select: function (event, ui) {
                    try {
                        $("#ctl00_ContentPlaceHolder1_txtCustomer").val(ui.item.label);
                        $("#ctl00_ContentPlaceHolder1_hdnPatientId").val(ui.item.value);
                        $("#ctl00_ContentPlaceHolder1_txtLocation").focus();
                        $("#ctl00_ContentPlaceHolder1_txtLocation").val('');
                        $("#ctl00_ContentPlaceHolder1_hdnLocId").val('');
                    } catch{ }
                    //                 document.getElementById('ctl00_ContentPlaceHolder1_btnSelectCustomer').click();
                    return false;
                },
                focus: function (event, ui) {
                    try {
                        $("#ctl00_ContentPlaceHolder1_txtCustomer").val(ui.item.label);
                    } catch{ }
                    return false;
                },
                minLength: 0,
                delay: 250
            })
                .data("ui-autocomplete")._renderItem = function (ul, item) {
                    try {
                        var result_item = item.label;
                        var result_desc = item.desc;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });
                        }
                        return $("<li></li>")
                            .data("ui-autocomplete-item", item)
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    } catch{ }
                };


            ///////////// Ajax call for location auto search ////////////////////
            var queryloc = "";
            $("#ctl00_ContentPlaceHolder1_txtLocation").autocomplete(
                {
                    source: function (request, response) {
                        //                        if (document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value != '') {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.custID = 0;
                        if (document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value != '') {
                            dtaaa.custID = document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value;
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
                                try {
                                    response($.parseJSON(data.d));
                                } catch{ }
                            },
                            error: function (result) {
                                try {
                                     $("#ctl00_ContentPlaceHolder1_txtLocation").val('');
                                     $("#ctl00_ContentPlaceHolder1_hdnLocId").val('');
                                   // alert("Due to unexpected errors we were unable to load Location.");
                                } catch{ }
                            }
                           
                        });

                 
                    },
                    select: function (event, ui) {
                        try {
                            $("#ctl00_ContentPlaceHolder1_txtLocation").val(ui.item.label);
                            $("#ctl00_ContentPlaceHolder1_hdnLocId").val(ui.item.value);
                        } catch{ }
                        //                        document.getElementById('ctl00_ContentPlaceHolder1_btnSelectLoc').click();
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#ctl00_ContentPlaceHolder1_txtLocation").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
                .data("ui-autocomplete")._renderItem = function (ul, item) {

                    try {
                        var result_item = item.label;
                        var result_desc = item.desc;
                        var x = new RegExp('\\b' + queryloc, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                        if (result_desc != null) {
                            result_desc = result_desc.replace(x, function (FullMatch, n) {
                                return '<span class="highlight">' + FullMatch + '</span>'
                            });
                        }
                        return $("<li></li>")
                            .data("ui-autocomplete-item", item)
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul);
                    } catch{ }
                };


            //            $("#<%--<%=pnlOptionsHeader.ClientID%>--%>").click(function() {
            //                $("#<%--<%=pnlOptions.ClientID%>--%>").slideToggle();
            //                return false;
            //            });


            ///////////// Validations for auto search ////////////////////
            $("#ctl00_ContentPlaceHolder1_txtCustomer").keyup(function (event) {
                var hdnPatientId = document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId');
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtCustomer').value == '') {
                    hdnPatientId.value = '';
                }
            });

            $("#ctl00_ContentPlaceHolder1_txtLocation").keyup(function (event) {
                var hdnLocId = document.getElementById('ctl00_ContentPlaceHolder1_hdnLocId');
                if (document.getElementById('ctl00_ContentPlaceHolder1_txtLocation').value == '') {
                    hdnLocId.value = '';
                }
            });

     

            //TextboxState();
        });


 

        ///////////// Custom validator function for customer auto search  ////////////////////
        function ChkCustomer(sender, args) {
            var hdnPatientId = document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId');
            if (hdnPatientId.value == '') {
                args.IsValid = false;
            }
        }

        ///////////// Custom validator function for location auto search  ////////////////////
        function ChkLocation(sender, args) {
            var hdnLocId = document.getElementById('ctl00_ContentPlaceHolder1_hdnLocId');
            if (hdnLocId.value == '') {
                args.IsValid = false;
            }
        }

        function showModalPopupViaClientCust(lblTicketId, lblComp) {
            document.getElementById('<%= iframeCustomer.ClientID %>').width = "1024px";
            document.getElementById('<%= iframeCustomer.ClientID %>').src = "addticket.aspx?id=" + lblTicketId + "&comp=" + lblComp;
            document.getElementById('<%= Panel2.ClientID %>').style.display = "none";
            var modalPopupBehavior = $find('PMPBehaviour');
            modalPopupBehavior.show();
        }


        function ValidateOptionsPanel() {
            Page_ClientValidate();
            var isv = true;
            var ctrvalid = document.getElementById('<%=notes.ClientID %>')
            <%--var cpe = $find('<%=cpeOption.ClientID %>');--%>

            if (!ctrvalid.isvalid) {
                isv = false;
            }
            if (!isv) {
                //cpe._doOpen();
            }
            else {
                //cpe._doClose();
            }

            if (Page_ClientValidate()) {
                var ConfirmMessage1 = "Some preferred worker are inactive. Unable to process tickets?";

                var ConfirmMessage2 = "You are about to process tickets for selected period. This process will generate tickets for eligible accounts. Are you sure you want to proceed?  \n \n Please note contracts with assigned tickets will not generate for the same period. ";

                var ConfirmMessage3 = "Some location(s) are on credit hold. Would you like to exclude contracts for locations on credit hold?";

                var hdnConfirm = $("#<%=HdnConfirm.ClientID%>").val();

                var hdnCreditHold = $("#<%=hdnCreditHold.ClientID%>").val();

                if (hdnConfirm == "1") {
                    alert(ConfirmMessage1);
                    return false;
                }

                if (hdnCreditHold == "1") {
                    if (confirm(ConfirmMessage3)) return true;
                        else
                    return false;
                }

                return confirm(ConfirmMessage2);

            }
        }


        function isDate(txtDate) {
            var currVal = txtDate;
            if (currVal == '')
                return false;
            //Declare Regex 
            var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
            var dtArray = currVal.match(rxDatePattern); // is format OK?
            if (dtArray == null)
                return false;
            //Checks for mm/dd/yyyy format.
            dtMonth = dtArray[1];
            dtDay = dtArray[3];
            dtYear = dtArray[5];

            if (dtMonth < 1 || dtMonth > 12)
                return false;
            else if (dtDay < 1 || dtDay > 31)
                return false;
            else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
                return false;
            else if (dtMonth == 2) {
                var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
                if (dtDay > 29 || (dtDay == 29 && !isleap))
                    return false;
            }
            return true;
        }

        ///////////// Validate Form ////////////////////
        function validateForm() {
            var check = false;
            var msg = "";
            var fromDate = $(".txtStartDt").val();
            var toDate = $(".txtEndDate").val();
            var preferredWorker = $(".ddlRoute").val();
            var ticketNote = $(".notes").val();
            var checkRequireStartDt = $("[id$='_vceStartDt_popupTable']").is(":visible");
            var checkRequireEndDt = $("[id$='_vceEndDt_popupTable']").is(":visible");
            var checkRequireNotes = $("[id$='_ValidatorCalloutExtender1_popupTable']").is(":visible");


            if (checkRequireStartDt || checkRequireEndDt || checkRequireNotes) {
                check = false;
                return check;
            }

            if (fromDate.length === 0) {
                msg = "Please set a date range."
                alert(msg);
                check = false;
                return check;
            }
            else if (fromDate.length > 0 && !isDate(fromDate)) {
                msg = "From Date is invalid. Please set a date range again."
                alert(msg);
                check = false;
                return check;
            }
            else {
                check = true;
            }

            if (toDate.length === 0) {
                msg = "Please set a date range."
                alert(msg);
                check = false;
                return check;
            }
            else if (toDate.length > 0 && !isDate(toDate)) {
                msg = "End Date is invalid. Please set a date range again."
                alert(msg);
                check = false;
                return check;
            }
            else {
                check = true;
            }
 

            if (ticketNote.length == 0) {
                msg = "Please input Ticket Notes."
                alert(msg);
                check = false;
                return check;
            }
            else {
                check = true;
            }
            return check;
        }

        ///////////// Select all checkbox ////////////////////
        function checkAllChecBox() {
            var checked = $(".chkSelectAll input").is(":checked");
            if (checked) {
                $(".chkSelect input").prop("checked", true)
            }
            else {
                $(".chkSelect input").prop("checked", false)
            }
        }

        ///////////// Unselect all checkbox ////////////////////
        function unCheckSelectAll() {
            var checked = $(".chkSelect input").is(":checked");
            var checkedAll = $(".chkSelectAll input").is(":checked");
            var checkCountCheckbox = $(".chkSelect input:checked").length;
            var checkCountCheckboxSelected = $(".chkSelect input").length
            if (checked && checkedAll) {
                $(".chkSelectAll input").prop("checked", false);
            }

            if (checkCountCheckbox === checkCountCheckboxSelected) {
                $(".chkSelectAll input").prop("checked", true);
            }

        }
        ///////////// Hide select all checkbox ////////////////////
        function hideSelectAllChkb() {
            $(".chkSelectAll").hide();
        }
        ///////////// Show select all checkbox ////////////////////
        function showSelectAllChkb() {
            $(".chkSelectAll").show();
        }
    </script>

    <style type="text/css">
        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */ /*padding-right: 20px;*/
        }
        /* IE 6 doesn't support max-height
	     * we use height instead, but this forces the menu to always be this tall
	     */ * html .ui-autocomplete {
            height: 300px;
        }

        .highlight {
            background-color: Yellow;
        }

        [id$='_popupTable'] {
            width: 200px;
        }

        [id$='ValidatorCalloutExtender1_popupTable'] {
            top: -61px !important;
        }



        .textarea-border {
            border: 1px solid #aaa !important;
            border-radius: 5px !important;
            padding-left: 10px !important;
        }

        .lnkSearch {
            margin-right: 0 !important;
        }

        .display-inline-block {
            display: inline-block;
        }

        /*[id$='_RadAjaxPanel_RecurringTicket'] .raDiv {
            background-position: top !important;
        }*/
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="overlay" style="display:none;">
    <div class="spinner"></div>
    <br/>
    Processing...
</div>
    <div class="divbutton-container">
        <div id="divButtons" class="">
            <div id="breadcrumbs-wrapper">

                <header>
                    <div class="container row-color-grey">
                        <div class="row">
                            <div class="col s12 m12 l12">
                                <div class="row">
                                    <div class="page-title"><i class="mdi-content-reply-all"></i>&nbsp;Recurring Tickets</div>
                                    <div class="buttonContainer">
                                        <div class="btnlinks">
                                            <asp:LinkButton ID="lnkProcess" runat="server" CausesValidation="true"
                                                OnClick="lnkProcess_Click" ToolTip="Process"
                                                OnClientClick='return AddTicketClick(this)'
                                                Enabled="true" ValidationGroup="dateTime">Process</asp:LinkButton>

                                        </div>
                                        <div class="btnlinks">
                                            <a class="dropdown-button" data-beloworigin="true" href="#!" data-activates="dropdown1">Reports
                                            </a>
                                        </div>
                                        <ul id="dropdown1" class="dropdown-content">
                                            <li>
                                                <a id="lnkAddNewReport" runat="server" onserverclick="lnkAddReport_Click" class="-text">Add New Report</a>
                                            </li>
                                        </ul>
                                        <div class="btnlinks menuAction">
                                            <a onclick="DropdownMenu()" class="dropbtn" style="cursor: pointer;">Actions
                                            </a>
                                        </div>
                                        <ul id="drpMenu" class="nomgn hideMenu menuList">
                                            <li>
                                                <div class="btnlinks mb-4">
                                                    <a id="btnDelete" runat="server" onclick="return CheckDelete();" onserverclick="BtnDelete_Click">Delete
                                                    </a>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="btnlinks">
                                                    <asp:LinkButton ID="lnkExcel" runat="server" OnClick="lnkExcel_Click">Export to Excel</asp:LinkButton>
                                                </div>
                                            </li>
                                        </ul>


                                    </div>
                                    <div class="btnclosewrap">
                                        <asp:LinkButton ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                            OnClick="lnkClose_Click"><i class="mdi-content-clear"></i></asp:LinkButton>
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
                            <ul class="anchor-links" style="float: right;">
                                <li>
                                    <asp:Label runat="server" ID="lblUserName" class="title_text_Name_1"></asp:Label>
                                </li>
                                <li>
                                    <asp:Label runat="server" ID="lblLastProcessDate" class="title_text_Name_1"></asp:Label>
                                </li>

                                <li>
                                    <asp:Label runat="server" ID="lblProcessPeriod" class="title_text_Name_1"></asp:Label>
                                </li>


                            </ul>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div class="container">
        <div class="row">
            <div class="srchpane-advanced">
                <div class="srchpaneinner">
                    <div class="srchtitle srchtitlecustomwidth month-css">
                        Date
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtStartDt" runat="server"  TabIndex="5" CssClass="srchcstm datepicker_mom txtStartDt width-80" ValidationGroup="dateTime" onblur="validateDatetime();" placeholder="From"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvStartDt"
                            runat="server" ControlToValidate="txtStartDt" Display="None" ErrorMessage="Please set a date range." ValidationGroup="dateTime"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:ValidatorCalloutExtender ID="vceStartDt" runat="server" Enabled="True"
                            PopupPosition="Right" TargetControlID="rfvStartDt" />
                    </div>
                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtEndDate" runat="server"  CssClass="srchcstm datepicker_mom txtEndDate width-80" onblur="validateDatetime();" placeholder="To"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEndDt"
                            runat="server" ControlToValidate="txtEndDate" Display="None" ErrorMessage="Please set a date range." ValidationGroup="dateTime"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:ValidatorCalloutExtender ID="vceEndDt" runat="server" Enabled="True"
                            PopupPosition="Right" TargetControlID="rfvEndDt" />
                    </div>
                    <div class="srchtitle srchtitlecustomwidth ">
                        <span id="spnWorker" runat="server">Worker</span>
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlRoute" runat="server" CssClass="browser-default selectst selectsml ddlRoute width-250"  TabIndex="5">
                        </asp:DropDownList>
                    </div>
                    <div class="srchtitle srchtitlecustomwidth">
                        Special Notes
                    </div>
                    <div class="srchinputwrap">
                        <select class="browser-default selectst selectsml">
                            <option>All</option>
                            <option>Yes</option>
                            <option>No</option>
                        </select>
                    </div>

                </div>
                <div class="srchpaneinner">

                    <div class="srchtitle srchtitlecustomwidth ">
                        Customer
                    </div>

                    <div class="srchinputwrap">

                        <asp:TextBox ID="txtCustomer" onkeydown = "return (event.keyCode!=13);" runat="server" autocomplete="off" CssClass="validate srchcstm"
                            placeholder="Customer Name"
                            TabIndex="1"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtCustomer_FilteredTextBoxExtender" runat="server"
                            Enabled="False" FilterMode="InvalidChars" InvalidChars="'\"
                            TargetControlID="txtCustomer">
                        </asp:FilteredTextBoxExtender>
                    </div>
                    <div class="srchtitle srchtitlecustomwidth">
                        Location
                    </div>

                    <div class="srchinputwrap">
                        <asp:TextBox ID="txtLocation" onkeydown = "return (event.keyCode!=13);"  runat="server" autocomplete="off" CssClass="validate srchcstm"
                            placeholder="Location Name"
                            TabIndex="2"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                            Enabled="false" FilterMode="InvalidChars" InvalidChars="'\"
                            TargetControlID="txtLocation">
                        </asp:FilteredTextBoxExtender>
                    </div>
                    <div class="srchtitle srchtitlecustomwidth" style="padding-left: 15px;">
                        State
                    </div>
                    <div class="srchinputwrap">
                        <asp:DropDownList ID="ddlState" runat="server" ToolTip="State" CssClass="browser-default selectst selectsml">
                            <asp:ListItem Value="">Select</asp:ListItem>
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
                        </asp:DropDownList>
                    </div>
                    <div class="srchinputwrap srchclr btnlinksicon lnkSearch">
                        <telerik:RadAjaxPanel ID="RadAjaxPanelSearch" runat="server">
                            <asp:LinkButton ID="lnkSearch" CssClass="submit " runat="server" CausesValidation="false" ToolTip="Refresh"
                                OnClick="lnkSearch_Click"><i class="mdi-action-search"></i></asp:LinkButton>
                        </telerik:RadAjaxPanel>
                    </div>

                    <div class="col lblsz2 lblszfloat" style="width: 200px;">
                        <div class="row">
                            <span class="tro trost">
                                <asp:LinkButton ID="lnkClear" OnClick="lnkClear_Click" runat="server">Clear</asp:LinkButton>
                                <asp:LinkButton ID="btnclear2" OnClick="LinkButton1_Click" runat="server"></asp:LinkButton>
                            </span>
                            <asp:UpdatePanel ID="upSearch" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="srchinputwrap col lblsz2 lblszfloat">
                                        <div class="row">
                                            <span class="tro trost">
                                                <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                                            </span>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                    <div>
                        <asp:HiddenField ID="hdnCon" runat="server" />
                        <asp:HiddenField ID="hdnPatientId" runat="server" />
                        <asp:HiddenField ID="hdnLocId" runat="server" />
                        <asp:Button CausesValidation="false" ID="btnSelectCustomer" runat="server" Text="Button"
                            Style="display: none;" OnClick="btnSelectCustomer_Click" />
                        <asp:Button CausesValidation="false" ID="btnSelectLoc" runat="server" Text="Button"
                            Style="display: none;" OnClick="btnSelectLoc_Click" />
                    </div>
                </div>

                <div class="srchpaneinner mt-10">
                    <div class="section-ttle">Set Options</div>
                    <div class="form-section-row">
                        <div class="input-field col s3">
                            <div class="checkrow">
                                <asp:CheckBox ID="chkContrRemarks" runat="server" Text="Include Contract Description" CssClass="css-checkbox" />
                            </div>
                        </div>
                        <div class="input-field col s2">
                            <div class="checkrow">
                                <asp:CheckBox ID="chkPerEquip" runat="server" Text="Per Equipment" CssClass="css-checkbox" />
                            </div>
                        </div>
                        <div class="input-field col s2">
                            <div class="checkrow">
                                <asp:CheckBox ID="chkDemand" runat="server" Text="On Demand" CssClass="css-checkbox" />
                            </div>
                        </div>
                        <div class="input-field col s2">
                            <div class="checkrow">
                                <asp:CheckBox ID="chkIsAllTicketsOnHold" Text="All Tickets Hold" runat="server" CssClass="css-checkbox" />
                            </div>
                        </div>
                        <div class="input-field col s2">
                            <div class="checkrow">
                                <asp:CheckBox ID="chkisAllTicketsUnassigned" Text="All Tickets Unassigned" runat="server" CssClass="css-checkbox" />
                            </div>
                        </div>
                        <div class="input-field col s12 mt-30">
                            <div class="row">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server"
                                    ControlToValidate="notes" Display="None" ErrorMessage="Ticket Notes Required"
                                    SetFocusOnError="True" ValidationGroup="prc" ></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1"
                                    runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator24">
                                </asp:ValidatorCalloutExtender>
                                <asp:TextBox ID="notes" runat="server" style="width:99%" CssClass="textarea-border materialize-textarea notes" TextMode="MultiLine"></asp:TextBox>
                                <label for="notes" class="txtbrdlbl">Ticket Notes</label>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="grid_container mb-30">
                    <div class="form-section-row mb">
                        <telerik:RadAjaxManager ID="RadAjaxManager_RecurringTickets" runat="server">
                            <AjaxSettings>
                                <telerik:AjaxSetting AjaxControlID="gvOpenCalls">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel_RTicket" LoadingPanelID="RadAjaxLoadingPanel_RecurringTickets" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting> 
                                <telerik:AjaxSetting AjaxControlID="lnkSearch">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel_RTicket" LoadingPanelID="RadAjaxLoadingPanel_RecurringTickets" />
                                        <telerik:AjaxUpdatedControl ControlID="HdnConfirm" />
                                        <telerik:AjaxUpdatedControl ControlID="hdnCreditHold" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                            </AjaxSettings>
                        </telerik:RadAjaxManager>

                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel_RecurringTickets" runat="server">
                        </telerik:RadAjaxLoadingPanel>

                        <div class="RadGrid RadGrid_Material FormGrid">
                            <telerik:RadCodeBlock ID="codeBlock1" runat="server">
                                <script type="text/javascript">
                                    function pageLoad() {
                                        var grid = $find("<%= gvOpenCalls.ClientID %>");
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


                            <telerik:RadAjaxPanel ID="RadAjaxPanel_RTicket" runat="server" LoadingPanelID="RadAjaxLoadingPanel_RecurringTickets" ClientEvents-OnRequestStart="requestStart" ClientEvents-OnResponseEnd="responseEnd"> 

                                <telerik:RadGrid RenderMode="Auto" ID="gvOpenCalls" runat="server" AutoGenerateColumns="False" Width="100%" FilterType="CheckList"
                                    PageSize="100" CssClass="table table-bordered table-striped table-condensed flip-content gvOpenCalls" AllowSorting="True"
                                    ShowFooter="True" PagerStyle-AlwaysVisible="true" ShowStatusBar="true"
                                    OnNeedDataSource="gvOpenCalls_NeedDataSource"
                                    OnItemEvent="gvOpenCalls_ItemEvent"
                                    OnItemCreated="gvOpenCalls_ItemCreated" OnExcelMLExportRowCreated="gvOpenCalls_ExcelMLExportRowCreated"
                                    OnItemDataBound="gvOpenCalls_ItemDataBound"
                                    AllowPaging="true"
                                    MasterTableView-CanRetrieveAllData="false">
                                    <PagerStyle AlwaysVisible="true" Mode="NextPrevAndNumeric" />
                                    <CommandItemStyle />
                                    <GroupingSettings CaseSensitive="false" />
                                    <ActiveItemStyle CssClass="evenrowcolor" />
                                    <ClientSettings AllowColumnsReorder="true" ReorderColumnsOnClient="true">
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" />
                                        <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true"></Resizing>
                                    </ClientSettings>
                                    <MasterTableView AutoGenerateColumns="false" AllowFilteringByColumn="True" ShowFooter="True" DataKeyNames="job">
                                        <Columns>
                                            <telerik:GridTemplateColumn AllowFiltering="false" UniqueName="Comp" DataField="Comp" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" CssClass="chkSelect" runat="server" onchange="unCheckSelectAll();" />
                                                    <asp:Label ID="lblComp" Visible="false" runat="server" Text='<%# Bind("Comp") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" CssClass="chkSelectAll" onchange="checkAllChecBox();" runat="server" />
                                                </HeaderTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Ticket #" SortExpression="id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container, "ItemIndex") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblTicketID" runat="server" Text='<%# Eval("TicketID") %>' />
                                                    <asp:Label ID="RCTlblLocid" runat="server" Text='<%#  Eval("Loc") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblAddress" runat="server" Text='<%#  Eval("Address") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblcity" runat="server" Text='<%#  Eval("City") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblstate" runat="server" Text='<%#  Eval("state") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblZip" runat="server" Text='<%#  Eval("zip") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblcalldate" runat="server" Text='<%# Bind("calldate") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblscheduledt" runat="server" Text='<%# Bind("scheduledt") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblassigned" runat="server" Text='<%# Bind("assigned") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblworker" runat="server" Text='<%# Bind("worker") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblcategory" runat="server" Text='<%# Bind("category") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblElev" runat="server" Text='<%# Bind("Elev") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblOwner" runat="server" Text='<%# Bind("Owner") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblJobremarks" runat="server" Text='<%# Bind("Jobremarks") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblremarks" runat="server" Text='<%# Bind("remarks") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblJob" runat="server" Text='<%# Bind("Job") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblEst" runat="server" Text='<%# Bind("Est") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblworkerstatus" runat="server" Text='<%# Bind("workerstatus") %>'></asp:Label>
                                                    <asp:Label ID="RCTlblcredit" runat="server" Text='<%# Bind("credit") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Assigned Tickets" AutoPostBackOnFilter="true" DataType="System.String" SortExpression="TicketID" DataField="TicketID" UniqueName="TicketID" HeaderStyle-Width="150px" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnTicketID" runat="server" Value='<%# Eval("TicketID") %>' />
                                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" Text="Total :-"></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Contract #" ItemStyle-CssClass="tdContract" UniqueName="job" DataField="job" SortExpression="job"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="lnkJob" runat="server" NavigateUrl='<%# "addreccontract.aspx?rt=1&uid=" + Eval("job") %>'
                                                        Target="_blank" Text='<%# Bind("job") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Customer" HeaderStyle-Width="150" UniqueName="customername" DataField="customername" SortExpression="customername"
                                                AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustName" runat="server" Text='<%# Bind("customername") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Acct #" HeaderStyle-Width="150" UniqueName="locid" DataField="locid" SortExpression="locid" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLocid" runat="server" Text='<%# Bind("locid") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </telerik:GridTemplateColumn>

                                              <telerik:GridTemplateColumn HeaderStyle-Width="100"     HeaderText="Credit Hold" AllowFiltering="false" SortExpression="credit" Visible="true" ShowFilterIcon="false">
                                             
                                            <ItemTemplate> 
                                          <img id="imgCreditH" runat="server" visible='<%# (Eval("credit").ToString() == "1")?true:false %>' title="Credit Hold" src="images/MSCreditHold.png" style="float: left; width: 16px; background-color: rgba(255, 0, 0, 0.34)">
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="LocationName" HeaderStyle-Width="150" UniqueName="locname" DataField="locname" SortExpression="locname" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                <ItemTemplate>
                                                  
                                                    <asp:Label ID="lblLoc" runat="server" Text='<%# Bind("locname") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </telerik:GridTemplateColumn>

                                         
                                        

                                            <telerik:GridTemplateColumn HeaderText="Company" SortExpression="Company" DataField="Company" UniqueName="Company" ShowFilterIcon="false" AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCompany" runat="server" Text='<%# Bind("Company") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Address" HeaderStyle-Width="200" SortExpression="locname" DataField="Address" UniqueName="Address" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLocAdd" runat="server" Text='<%# String.Format("{0}, {1}", Eval("Address"), Eval("City")) %>'></asp:Label>

                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Scheduled Date" SortExpression="edate" DataField="edate" UniqueName="edate" ShowFilterIcon="false" AllowFiltering="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("edate", "{0:MM/dd/yyyy hh:mm tt }") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Hours" SortExpression="est" Aggregate="Sum" FooterAggregateFormatString="{0}" DataField="est" UniqueName="est" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHours" runat="server" Text='<%# Bind("est") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </telerik:GridTemplateColumn>

                                         
                                            
                                           <telerik:GridTemplateColumn DataField="ExpirationDate"  AllowFiltering="false"  AllowSorting="true" SortExpression="ExpirationDate" AutoPostBackOnFilter="true" HeaderStyle-Width="140" DataType="System.String"
                                                            CurrentFilterFunction="Contains" HeaderText="Expiration" ShowFilterIcon="false" UniqueName="ExpirationDate">
                                                            <ItemTemplate>
                                                                <asp:Label Style='<%# (Eval("ExpirationDate", "{0:MM/dd/yyyy}")!="01/01/1900") ?( string.Format("color:{0}",Convert.ToDateTime(Eval("ExpirationDate").ToString())<= System.DateTime.Now ? "RED": "BLACK")):"" %>'
                                                                    ID="lblExpirationdt" runat="server"><%# (Eval("ExpirationDate", "{0:MM/dd/yyyy}")=="01/01/1900") ? "Indefinitely" : Eval("ExpirationDate", "{0:MM/dd/yyyy}") %></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridTemplateColumn HeaderText="Equipment" HeaderStyle-Width="150" SortExpression="unit" AllowFiltering="true" AllowSorting="true" DataField="unit" UniqueName="unit" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                <ItemTemplate> 

                                                     <div><%#  Eval("unit").ToString() %></div>
                                                     

                                                </ItemTemplate>
                                                  <FooterTemplate>
                                                    <asp:Label runat="server" Text="Total equipment :-"></asp:Label>
                                                </FooterTemplate>
                                                 <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </telerik:GridTemplateColumn>

                                            <%--Showing in case of "Per Equipment" unchecked--%>
                                            <telerik:GridTemplateColumn HeaderText="Equipment" HeaderStyle-Width="150" SortExpression="TempEquipmentstr" AllowFiltering="true" AllowSorting="true" DataField="TempEquipmentstr" UniqueName="TempEquipmentstr" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                <ItemTemplate>
                                                    <div><%#  Eval("TempEquipmentstr").ToString().Replace(",",", ") %></div>
                                                     
                                                </ItemTemplate>
                                                 <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                  <FooterTemplate>
                                                    <asp:Label runat="server" Text="  Total equipment :-"></asp:Label>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>

                                          

                                            <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="40" AllowFiltering="false"  SortExpression="EquipmentCount" Aggregate="Sum" FooterAggregateFormatString="{0}" AllowSorting="false" DataField="EquipmentCount" UniqueName="EquipmentCount" ShowFilterIcon="false" AutoPostBackOnFilter="false" CurrentFilterFunction="EqualTo">  
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>

                                               
                                            </telerik:GridTemplateColumn>


                                 <telerik:GridTemplateColumn SortExpression="dwork" DataField="dwork" UniqueName="dwork" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAssdTo" ForeColor='<%# (Eval("workerstatus").ToString() == "1") ?System.Drawing.Color.Red:System.Drawing.Color.Black%>'
                                                        runat="server"
                                                        Text='<%# Bind("dwork") %>'></asp:Label>
                                                </ItemTemplate>
                               </telerik:GridTemplateColumn>

                                     
                                   <telerik:GridTemplateColumn HeaderText="ServiceType" SortExpression="ctype" DataField="ctype" UniqueName="ctype" ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblServtype" runat="server" Text='<%# Bind("ctype") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <SelectedItemStyle CssClass="selectedrowcolor" />
                                    <FilterMenu CssClass="RadFilterMenu_CheckList">
                                    </FilterMenu>
                                </telerik:RadGrid>

                            </telerik:RadAjaxPanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- edit-tab end -->
    <div class="clearfix"></div>

    <!-- END DASHBOARD STATS -->
    <div class="clearfix"></div>
    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
        CausesValidation="False" />
    <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BehaviorID="PMPBehaviour"
        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="Panel1" BackgroundCssClass="pnlUpdateoverlay"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="Panel1" Style="display: none; background: #fff; border: solid;">
        <asp:Panel runat="Server" ID="Panel2" Style="background-color: #DDDDDD; border: solid 1px Gray; color: Black; text-align: center;">
            <div class="title_bar_popup">
                <a id="A1" href="#" style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px;">Close</a>
            </div>
        </asp:Panel>
        <div>
            <iframe id="iframeCustomer" runat="server" width="1024px" height="600px" frameborder="0"></iframe>
        </div>
    </asp:Panel>
    <asp:Button runat="server" ID="hideModalPopupViaServer" Style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px; display: none;"
        Text="Close" OnClick="hideModalPopupViaServer_Click"
        CausesValidation="false" />

    <asp:HiddenField ID="HdnConfirm" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCreditHold" runat="server" Value="0" />
    <asp:HiddenField runat="server" ID="hdnAddeTicket" Value="Y" />

    <asp:HiddenField runat="server" ID="hdnCon1" />


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="javascriptHolder" runat="Server">
    <script src="Design/js/moment.js"></script>
    <script src="Design/js/pikaday.js"></script>
    <script type="text/javascript">
        function validateDatetime() {
            var valueFromDt = new Date($(".txtStartDt").val());
            var valueEndDt = new Date($(".txtEndDate").val());
            var str = "From Date cannot be greater than To Date.";
            if (valueFromDt > valueEndDt) {
                noty({ text: str, type: 'error', layout: 'topCenter', closeOnSelfClick: false, timeout: 5000, theme: 'noty_theme_default', closable: true });
            }

        }

        function DismissPopUp(className) {
            $('.' + className).hide();
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

            $('#addinfo').hide();
            $('.add-btn-click').click(function () {

                $('#addinfo').slideToggle('2000', "swing", function () {
                    // Animation complete.

                });

                if ($('.divbutton-container').height() != 65)
                    $('.divbutton-container').animate({ height: 65 }, 500);
                else
                    $('.divbutton-container').animate({ height: 350 }, 500);


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
            $("#<%=notes.ClientID%>").keyup(function (event) {
                debugger
                replaceQuickCodes(event, '<%=notes.ClientID%>', $("#<%=hdnCon1.ClientID%>").val());
            });
        });
    </script>
    <script>

    </script>
</asp:Content>
